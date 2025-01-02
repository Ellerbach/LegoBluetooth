// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth;
using System;
using System.Diagnostics;
using System.Threading;
using LegoBluetooth;
using System.Collections;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Runtime.Native;

namespace DumpPayloads
{
    public class Program
    {
        // Devices found by watcher
        private readonly static Hashtable s_foundDevices = new();
        private readonly static BluetoothLEAdvertisementWatcher watcher = new();
        private static GattCharacteristic _gc;
        private static bool _search;

        public static void Main()
        {
            _search = true;
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            //watcher.AdvertisementFilter.Advertisement = new BluetoothLEAdvertisement() { ServiceUuids = new[] { BluetoothAdvertisingData.HubService } };
            watcher.Received += Watcher_Received;

            Debug.WriteLine("Hello from nanoFramework!");

            while (_search)
            {
                Console.WriteLine("Starting BluetoothLEAdvertisementWatcher");
                watcher.Start();

                // Run until we have found some devices to connect to
                while (s_foundDevices.Count == 0)
                {
                    Thread.Sleep(5000);
                }

                Console.WriteLine("Stopping BluetoothLEAdvertisementWatcher");

                // We can't connect if watch running so stop it.
                if (watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
                {
                    watcher.Stop();
                }

                Console.WriteLine($"Devices found {s_foundDevices.Count}");
                Console.WriteLine("Connecting and Reading Sensors");

                //foreach (DictionaryEntry entry in s_foundDevices)
                //{
                //    BluetoothLEDevice device = entry.Value as BluetoothLEDevice;

                //    // Connect and register notify events
                //    if (ConnectAndRegister(device))
                //    {
                //        //if (s_dataDevices.Contains(device.BluetoothAddress))
                //        //{
                //        //    s_dataDevices.Remove(device.BluetoothAddress);
                //        //}
                //        //s_dataDevices.Add(device.BluetoothAddress, device);
                //    }
                //}
                //s_foundDevices.Clear();
            }

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // Print information about received advertisement
            // You don't receive all information in 1 event and it can be split across 2 events
            // AdvertisementTypes 0 and 4
            Console.WriteLine($"Received advertisement address:{args.BluetoothAddress:X}/{args.BluetoothAddressType} Name:{args.Advertisement.LocalName}  Advert type:{args.AdvertisementType}  Services:{args.Advertisement.ServiceUuids.Length}");

            if (args.Advertisement.ServiceUuids.Length > 0)
            {
                Console.WriteLine($"Advert Service UUID {args.Advertisement.ServiceUuids[0]}");
            }

            // Look for advert with our primary service UUID from Bluetooth Sample 3
            if (IsValidDevice(args))
            {
                Console.WriteLine($"Found a Lego sensor :{args.BluetoothAddress:X}");

                // Add it to list as a BluetoothLEDevice
                var dev = BluetoothLEDevice.FromBluetoothAddress(args.BluetoothAddress, args.BluetoothAddressType);
                s_foundDevices.Add(args.BluetoothAddress, dev);
                _search = false;
                watcher.Stop();
                ConnectAndRegister(dev);
            }
        }

        private static bool IsValidDevice(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.ServiceUuids.Length > 0 &&
                args.Advertisement.ServiceUuids[0].Equals(BluetoothAdvertisingData.HubService))
            {
                if (!s_foundDevices.Contains(args.BluetoothAddress))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ConnectAndRegister(BluetoothLEDevice device)
        {
            bool result = false;
            ArrayList toSend = new ArrayList() {
                new byte[] { 0x05, 0x00, 0x01, 0x04, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x03, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x09, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x08, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x07, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x0A, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x0D, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x02, 0x02 },
                new byte[] { 0x05, 0x00, 0x01, 0x02, 0x05 },
                //new byte[] { 0x05, 0x00, 0x01, 0x05, 0x02 },
                //new byte[] { 0x05, 0x00, 0x01, 0x05, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x06, 0x02 },
                new byte[] { 0x05, 0x00, 0x01, 0x06, 0x05 },
                new byte[] { 0x05, 0x00, 0x01, 0x01, 0x02 },
                new byte[] { 0x05, 0x00, 0x01, 0x01, 0x05 },                
                new byte[] { 0x09, 0x00, 0x81, 0x10, 0x11, 0x07, 0x32, 0x64, 0x00 },
            };

            if (device == null)
            {
                return result;
            }

            GattDeviceServicesResult sr = device.GetGattServicesForUuid(BluetoothAdvertisingData.HubService);
            Debug.WriteLine("Connection to device with services");
            if (sr.Status == GattCommunicationStatus.Success)
            {
                Debug.WriteLine($"Connected to {device.BluetoothAddress:X}");
                // Connected and services read
                result = true;
                Debug.WriteLine($"Number of services: {sr.Services.Length}");

                // Pick up all temperature characteristics
                foreach (GattDeviceService service in sr.Services)
                {
                    Console.WriteLine($"Service UUID {service.Uuid}");

                    GattCharacteristicsResult cr = service.GetCharacteristicsForUuid(BluetoothAdvertisingData.HubCharacteristic);
                    if (cr.Status == GattCommunicationStatus.Success)
                    {
                        _gc = cr.Characteristics[0];
                        var ret = _gc.WriteClientCharacteristicConfigurationDescriptorWithResult(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                        if (ret.Status != GattCommunicationStatus.Success)
                        {
                            return false;
                        }

                        _gc.ValueChanged += (sender, e) =>
                        {
                            var datareader = DataReader.FromBuffer(e.CharacteristicValue);
                            byte[] data = new byte[e.CharacteristicValue.Length];
                            datareader.ReadBytes(data);
                            Debug.WriteLine($"-> {BitConverter.ToString(data)}");
                        };

                        foreach (byte[] ts in toSend)
                        {
                            DataWriter dr = new DataWriter();
                            dr.WriteBytes(ts);
                            var res = _gc.WriteValueWithResult(dr.DetachBuffer());
                            if (res.Status == GattCommunicationStatus.Success)
                            {
                                Debug.WriteLine($"<- {BitConverter.ToString(ts)}");
                            }

                            Thread.Sleep(100);
                        }
                    }
                }
            }

            return result;
        }
    }
}
