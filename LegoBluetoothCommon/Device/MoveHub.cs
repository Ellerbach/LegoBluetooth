// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Collections;

namespace LegoBluetooth
{
    public class MoveHub : GenericDevice
    {
        public MoveHub(IBluetooth bluetooth) : base(bluetooth)
        {
            Name = "Move Hub";
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub),
                DeviceCapabilities.SupportsLPF2Devices | DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBeCentral | Status.CanBePeripheral,
                0
            );
            FWVersion = new Version(2, 0, 0, 17);
            HWVersion = new Version(0, 4, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub);
        }

        public override void ClientJoiningStateChanged(bool joining)
        {
            // Calling first the base method to send the device elements
            base.ClientJoiningStateChanged(joining);
            if (joining)
            {                
                // Send the device elements
                ArrayList elements = new ArrayList()
                    {
                        new byte[] { 0x0F, 0x00, 0x04, 0x00, 0x01, 0x27, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x01, 0x01, 0x27, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x02, 0x01, 0x25, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x03, 0x01, 0x26, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10 },
                        new byte[] { 0x09, 0x00, 0x04, 0x10, 0x02, 0x27, 0x00, 0x00, 0x01 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x32, 0x01, 0x17, 0x00, 0x00, 0x00, 0x00, 0x01, 0x06, 0x00, 0x00, 0x20 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x3A, 0x01, 0x28, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x02 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x3B, 0x01, 0x15, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x3C, 0x01, 0x14, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 },
                        new byte[] { 0x0F, 0x00, 0x04, 0x46, 0x01, 0x42, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10 },
                    };

                foreach (byte[] element in elements)
                {
                    Bluetooth.NotifyValue(element);
                    Thread.Sleep(10);
                }
            }
            else
            {

            }
        }
    }
}
