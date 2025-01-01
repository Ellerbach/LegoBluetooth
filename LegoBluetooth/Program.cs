using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using LegoBluetooth;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace LegoBluetoothHub
{
    public class Program
    {
        private static GattLocalCharacteristic _legoCharacteristic;

        public static void Main()
        {
            // BluetoothLEServer is a singleton object so gets its instance. The Object is created when you first access it
            // and can be disposed to free up memory.
            BluetoothLEServer server = BluetoothLEServer.Instance;

            // Give device a name
            server.DeviceName = "Move Hub";

            server.Pairing.PairingRequested += PairingRequested;
            server.Pairing.PairingComplete += PairingComplete;

            BluetoothAdvertisingData advertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub),
                DeviceCapabilities.SupportsLPF2Devices | DeviceCapabilities.SupportsLPF2Devices,
                (byte)LastNetworkID.None,
                Status.CanBePeripheral,
                0
            );

            //The GattServiceProvider is used to create and advertise the primary service definition.
            //An extra device information service will be automatically created.
            GattServiceProviderResult result = GattServiceProvider.Create(BluetoothAdvertisingData.HubService);

            DataWriter sw = new DataWriter();
            sw.WriteBytes(advertisingData.ToByteArray());

            // Create the GATT characteristic
            var legoHubCharacteristic = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.WriteWithoutResponse | GattCharacteristicProperties.Write | GattCharacteristicProperties.Notify,
                WriteProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "Move Hub",
            };

            GattServiceProvider serviceProvider = result.ServiceProvider;

            GattLocalService service = serviceProvider.Service;
            var legoHubService = serviceProvider.Service;

            // Add the characteristic to the service
            var charres = legoHubService.CreateCharacteristic(BluetoothAdvertisingData.HubCharacteristic, legoHubCharacteristic);
            _legoCharacteristic = charres.Characteristic;

            _legoCharacteristic.WriteRequested += (o, e) =>
            {
                GattWriteRequest request = e.GetRequest();
                DataReader dr = DataReader.FromBuffer(request.Value);
                byte[] data = new byte[request.Value.Length];
                dr.ReadBytes(data);
                Debug.WriteLine($"-> {BitConverter.ToString(data)}");

                var req = CommonMessageHeader.Decode(data);
                Debug.WriteLine($"Data: {req}");

                // Respond if Write requires response
                if (request.Option == GattWriteOption.WriteWithResponse)
                {
                    if (req.MessageType == MessageType.HubProperties)
                    {
                        var prop = (HubPropertyMessage)req;
                        if (prop.Operation == HubPropertyOperation.RequestUpdate)
                        {
                            var buff = new DataWriter();
                            var notify = new HubPropertyMessage(0, 0, prop.Property, HubPropertyOperation.Update, new byte[0]);

                            switch (prop.Property)
                            {
                                case HubProperty.AdvertisingName:
                                    notify.Payload = Encoding.UTF8.GetBytes("Move Hub");
                                    break;
                                case HubProperty.Button:
                                    notify.Payload = new byte[] { 0x00 };
                                    break;
                                case HubProperty.FWVersion:
                                    notify.Payload = new byte[] { 0x20, 0x00, 0x00, 0x20 };
                                    break;
                                case HubProperty.HWVersion:
                                    notify.Payload = new byte[] { 0x04, 0x00, 0x00, 0x00 };
                                    break;
                                case HubProperty.RSSI:
                                    notify.Payload = new byte[] { 0xB0 };
                                    break;
                                case HubProperty.BatteryVoltage:
                                    notify.Payload = new byte[] { 0x32 };
                                    break;
                                case HubProperty.BatteryType:
                                    notify.Payload = new byte[] { 0x01 };
                                    break;
                                case HubProperty.ManufacturerName:
                                    notify.Payload = Encoding.UTF8.GetBytes("LEGO System A/S");
                                    break;
                                case HubProperty.RadioFirmwareVersion:
                                    notify.Payload = Encoding.UTF8.GetBytes("7.2.c");
                                    break;
                                case HubProperty.LEGOWirelessProtocolVersion:
                                    notify.Payload = new byte[] { 0x02, 0x00 };
                                    break;
                                case HubProperty.SystemTypeID:
                                    notify.Payload = new byte[] { SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub) };
                                    break;
                                case HubProperty.HWNetworkID:
                                    notify.Payload = new byte[] { (byte)LastNetworkID.DisableHWNetwork };
                                    break;
                                case HubProperty.PrimaryMACAddress:
                                    notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAB };
                                    break;
                                case HubProperty.SecondaryMACAddress:
                                    notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAC };
                                    break;
                                case HubProperty.HardwareNetworkFamily:
                                    notify.Payload = new byte[] { 0x00 };
                                    break;
                                default:
                                    break;
                            }

                            if (_legoCharacteristic.SubscribedClients.Length > 0)
                            {
                                var toSend = notify.ToByteArray();
                                Debug.WriteLine($"<- {BitConverter.ToString(toSend)}");
                                buff.WriteBytes(toSend);
                                _legoCharacteristic.NotifyValue(buff.DetachBuffer());
                            }
                        }
                    }

                    request.Respond();
                }
            };

            _legoCharacteristic.SubscribedClientsChanged += (o, e) =>
            {
                if (e != null)
                {
                    GattLocalCharacteristic req = (GattLocalCharacteristic)e;
                    Console.WriteLine($"object type: {o.GetType().Name}");
                    foreach (GattSubscribedClient client in req.SubscribedClients)
                    {
                        Console.WriteLine($"Client: {client.Session.DeviceId}");
                    }
                }

                Console.WriteLine("Subscribed Clients Changed");
            };

            // Once all the Characteristics have been created you need to advertise the Service so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it. 

            var adv = new GattServiceProviderAdvertisingParameters()
            {
                IsConnectable = true,
                IsDiscoverable = true,
            };
            adv.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData(0x0397, sw.DetachBuffer()));

            serviceProvider.StartAdvertising(adv);

            Console.WriteLine("Advertising LEGO Hub...");

            // Keep the application running
            Thread.Sleep(Timeout.Infinite);
        }

        private static void PairingComplete(object sender, DevicePairingEventArgs args)
        {
            Debug.WriteLine($"Pairing complete: {args.Status}");
        }

        private static void PairingRequested(object sender, DevicePairingRequestedEventArgs args)
        {
            Debug.WriteLine($"Pairing requested: {args.PairingKind}");
            args.Accept();
        }
    }
}
