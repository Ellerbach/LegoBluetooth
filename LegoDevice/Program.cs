// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using LegoBluetooth;
using System;
using System.Diagnostics;
using System.Threading;

namespace LegoBluetoothHub
{
    public class Program
    {
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
            var ret = bluetooth.Setup(hub.BluetoothAdvertisingData.ToByteArray(), hub.Name);
            if (!ret)
            {
                Debug.WriteLine("Error setting up the Bluetooth");
                return;
            }

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
