using System;
using System.Threading;
using LegoBluetooth;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace LegoBluetoothHub
{
    public class Program
    {
        public static void Main()
        {
            // BluetoothLEServer is a singleton object so gets its instance. The Object is created when you first access it
            // and can be disposed to free up memory.
            BluetoothLEServer server = BluetoothLEServer.Instance;

            // Give device a name
            server.DeviceName = "LegoESP32";

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
            GattServiceProviderResult result = GattServiceProvider.Create(advertisingData.HubService);

            DataWriter sw = new DataWriter();
            sw.WriteBytes(advertisingData.ToByteArray());

            // Create the GATT characteristic
            var legoHubCharacteristic = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.WriteWithoutResponse | GattCharacteristicProperties.Write | GattCharacteristicProperties.Notify,
                WriteProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "LEGO Hub",
            };

            GattServiceProvider serviceProvider = result.ServiceProvider;

            GattLocalService service = serviceProvider.Service;
            var legoHubService = serviceProvider.Service;

            // Add the characteristic to the service
            var charres = legoHubService.CreateCharacteristic(advertisingData.HubCharacteristic, legoHubCharacteristic);
            var legoCharacteristic = charres.Characteristic;

            legoCharacteristic.WriteRequested += (o, e) =>
            {
                GattWriteRequest request = e.GetRequest();
                Console.WriteLine($"Data: {request.Value.Length} bytes");
                DataReader dr = DataReader.FromBuffer(request.Value);
                byte[] data = new byte[request.Value.Length];
                dr.ReadBytes(data);

                var req = CommonMessageHeader.Decode(data);
                Console.WriteLine($"Data: {req}");

                Console.WriteLine("Sending message to LEGO Hub");
                //Console.WriteLine($"Data: {e.Session.DeviceId.Id}");
            };

            legoCharacteristic.SubscribedClientsChanged += (o, e) =>
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
    }
}
