// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;
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
            BluetoothNano bluetooth = new BluetoothNano();

            var hub = new MoveHub(bluetooth);
            hub.FWVersion = new Version(1, 0, 0, 0);
            hub.HWVersion = new Version(1, 0, 0, 0);
            hub.Name = "Move Hub";

            bluetooth.OnError += BluetoothOnError;
            bluetooth.ProcessIncoming = hub.ProcessReceived;
            bluetooth.ClientJoiningStateChanged = hub.ClientJoiningStateChanged;
            bluetooth.Setup(hub.BluetoothAdvertisingData.ToByteArray(), hub.Name);

            bluetooth.Connect();

            // Keep the application running
            Thread.Sleep(Timeout.Infinite);
        }

        private static void BluetoothOnError(StatusError error)
        {
            Debug.WriteLine($"BLE error: {error}");
        }
    }
}
