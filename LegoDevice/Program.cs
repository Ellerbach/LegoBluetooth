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

            // Uncomment the hub you want to emulate:
            //var hub = CreateMoveHub(bluetooth);
            //var hub = CreateTwoPortHub(bluetooth);
            //var hub = CreateTechnicMediumHub(bluetooth);
            var hub = CreateDuploTrainBaseHub(bluetooth);
            //var hub = CreateMarioHub(bluetooth);

            hub.OnPortOutputCommand += HubOnPortOutputCommand;

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

        private static MoveHub CreateMoveHub(BluetoothNano bluetooth)
        {
            return new MoveHub(bluetooth);
        }

        private static TwoPortHub CreateTwoPortHub(BluetoothNano bluetooth)
        {
            return new TwoPortHub(bluetooth);
        }

        private static TechnicMediumHub CreateTechnicMediumHub(BluetoothNano bluetooth)
        {
            return new TechnicMediumHub(bluetooth);
        }

        private static DuploTrainBaseHub CreateDuploTrainBaseHub(BluetoothNano bluetooth)
        {
            return new DuploTrainBaseHub(bluetooth);
        }

        private static MarioHub CreateMarioHub(BluetoothNano bluetooth)
        {
            return new MarioHub(bluetooth);
        }

        private static void BluetoothOnError(StatusError error)
        {
            Debug.WriteLine($"BLE error: {error}");
        }

        private static void HubOnPortOutputCommand(PortOutputCommandMessage msg)
        {
            Debug.WriteLine($"Motor command: Port={msg.PortID}, SubCommand={msg.SubCommand}, Payload={BitConverter.ToString(msg.Payload)}");
        }
    }
}
