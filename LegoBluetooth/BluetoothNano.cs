// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using System;
using System.Diagnostics;
using static LegoBluetooth.IBluetooth;

namespace LegoBluetooth
{
    public class BluetoothNano : IBluetooth
    {
        private readonly BluetoothLEServer _server;
        private GattLocalCharacteristic _ble;
        private string _name;
        private GattServiceProvider _serviceProvider;

        public event IBluetooth.OnErrorHandler OnError;

        public IBluetooth.ProcessIncomingHandler ProcessIncoming { get; set; }
        public IBluetooth.ClientJoiningStateChangedHandler ClientJoiningStateChanged { get; set; }

        public BluetoothNano()
        {
            _server = BluetoothLEServer.Instance;
        }

        public bool Setup(byte[] adv, string name)
        {
            _name = name;

            try
            {
                // Give device a name
                _server.DeviceName = _name;

                //The GattServiceProvider is used to create and advertise the primary service definition.
                //An extra device information service will be automatically created.
                GattServiceProviderResult result = GattServiceProvider.Create(BluetoothAdvertisingData.HubService);

                DataWriter sw = new DataWriter();
                sw.WriteBytes(adv);

                // Create the GATT characteristic
                var legoHubCharacteristic = new GattLocalCharacteristicParameters
                {
                    CharacteristicProperties = GattCharacteristicProperties.WriteWithoutResponse | GattCharacteristicProperties.Write | GattCharacteristicProperties.Notify,
                    WriteProtectionLevel = GattProtectionLevel.Plain,
                    UserDescription = _name,
                    StaticValue = sw.DetachBuffer(),
                };

                _serviceProvider = result.ServiceProvider;

                GattLocalService service = _serviceProvider.Service;
                var legoHubService = _serviceProvider.Service;

                // Add the characteristic to the service
                var charres = legoHubService.CreateCharacteristic(BluetoothAdvertisingData.HubCharacteristic, legoHubCharacteristic);
                _ble = charres.Characteristic;

                // Once all the Characteristics have been created you need to advertise the Service so 
                // other devices can see it. Here we also say the device can be connected too and other
                // devices can see it. 

                var advert = new GattServiceProviderAdvertisingParameters()
                {
                    IsConnectable = true,
                    IsDiscoverable = true,
                };
                advert.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData(0x0397, sw.DetachBuffer()));

                Debug.WriteLine("Advertising LEGO Hub...");
                _serviceProvider.StartAdvertising(advert);

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception connect: {ex.Message}");
                OnError?.Invoke(StatusError.ConnectionError);
            }

            return false;
        }

        public bool Connect()
        {
            // Ensure it's started
            _server.Start();

            _ble.WriteRequested += (o, e) =>
            {
                try
                {
                    GattWriteRequest request = e.GetRequest();
                    DataReader dr = DataReader.FromBuffer(request.Value);
                    byte[] data = new byte[request.Value.Length];
                    dr.ReadBytes(data);
                    Debug.WriteLine($"-> {BitConverter.ToString(data)}");

                    ProcessIncoming(data);

                    if (request.Option == GattWriteOption.WriteWithResponse)
                    {
                        request.Respond();
                    }
                }
                catch (Exception)
                {
                    OnError?.Invoke(StatusError.WriteError);
                }
            };

            _ble.SubscribedClientsChanged += (o, e) =>
            {
                try
                {
                    if (_ble.SubscribedClients.Length > 0)
                    {
                        Debug.WriteLine("New Subscribed Client");
                        //_serviceProvider.StopAdvertising();
                        //Debug.WriteLine("Stop advertising LEGO Hub");
                        ClientJoiningStateChanged(true);
                    }
                    else
                    {
                        Debug.WriteLine("No Subscribed Client!");
                        ClientJoiningStateChanged(false);
                        //Debug.WriteLine("Advertising LEGO Hub...");
                        //_serviceProvider.StartAdvertising();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception subscribe: {ex.Message}");
                    OnError?.Invoke(StatusError.ConnectionError);                  
                }
            };

            IsConnected = true;
            return IsConnected;
        }

        public bool Disconnect()
        {
            _serviceProvider.StopAdvertising();
            _server.Stop();
            IsConnected = false;
            return IsConnected;
        }

        public bool IsConnected {  get; private set; }
        
        public bool NotifyValue(byte[] data)
        {
            bool status = false;
            if (_ble != null)
            {
                if (_ble.SubscribedClients.Length > 0)
                {
                    var dw = new DataWriter();
                    dw.WriteBytes(data);
                    _ble.NotifyValue(dw.DetachBuffer());
                    Debug.WriteLine($"<- {BitConverter.ToString(data)}");
                    status = true;
                }
            }
            else
            {
                Debug.WriteLine("No BLE characteristic to notify");
            }

            return status;
        }
    }
}
