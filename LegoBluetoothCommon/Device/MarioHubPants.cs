// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth.Device
{
    /// <summary>
    /// Represents a Mario Hub pants sensor device in a LEGO Bluetooth system.
    /// </summary>
    public class MarioHubPants : BaseDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarioHubPants"/> class.
        /// </summary>
        /// <param name="ble">The Bluetooth interface.</param>
        /// <param name="hubID">The ID of the hub.</param>
        /// <param name="portID">The ID of the port.</param>
        public MarioHubPants(IBluetooth ble, byte hubID, byte portID) : base(ble, hubID, portID)
        {
            HWVersion = new Version(2, 0, 0, 0);
            SWVersion = new Version(0, 0, 0, 1);
            DeviceType = IOTypeID.MarioHubPants;
        }

        /// <inheritdoc/>
        public override ArrayList GetDefaultConfiguration()
        {
            var defaultConfig = new ArrayList()
            {
                new byte[] { 0x0B, 0x00, 0x43, 0x02, 0x01, 0x02, 0x01, 0x01, 0x00, 0x00, 0x00 },
                new byte[] { 0x05, 0x00, 0x43, 0x02, 0x02 },
                // Mode 0: PANT
                new byte[] { 0x11, 0x00, 0x44, 0x02, 0x00, 0x00, 0x50, 0x41, 0x4E, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                new byte[] { 0x0E, 0x00, 0x44, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x42 },
                new byte[] { 0x0E, 0x00, 0x44, 0x02, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC8, 0x42 },
                new byte[] { 0x0E, 0x00, 0x44, 0x02, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x42 },
                new byte[] { 0x0A, 0x00, 0x44, 0x02, 0x00, 0x04, 0x69, 0x64, 0x78, 0x00 },
                new byte[] { 0x08, 0x00, 0x44, 0x02, 0x00, 0x05, 0x84, 0x00 },
                new byte[] { 0x0A, 0x00, 0x44, 0x02, 0x00, 0x80, 0x01, 0x00, 0x03, 0x00 },
            };
            return SetDefaultConfiguration(defaultConfig);
        }
    }
}
