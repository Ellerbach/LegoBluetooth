// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth.Device
{
    /// <summary>
    /// Represents a Technic Medium Hub temperature sensor device in a LEGO Bluetooth system.
    /// </summary>
    public class TechnicMediumHubTemperatureSensor : BaseDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicMediumHubTemperatureSensor"/> class.
        /// </summary>
        /// <param name="ble">The Bluetooth interface.</param>
        /// <param name="hubID">The ID of the hub.</param>
        /// <param name="portID">The ID of the port.</param>
        public TechnicMediumHubTemperatureSensor(IBluetooth ble, byte hubID, byte portID) : base(ble, hubID, portID)
        {
            HWVersion = new Version(1, 0, 0, 0);
            SWVersion = new Version(1, 0, 0, 0);
            DeviceType = IOTypeID.TechnicMediumHubTemperatureSensor;
        }

        /// <inheritdoc/>
        public override ArrayList GetDefaultConfiguration()
        {
            var defaultConfig = new ArrayList()
            {
                new byte[] { 0x0B, 0x00, 0x43, 0x3D, 0x01, 0x02, 0x01, 0x01, 0x00, 0x00, 0x00 },
                new byte[] { 0x05, 0x00, 0x43, 0x3D, 0x02 },
                new byte[] { 0x11, 0x00, 0x44, 0x3D, 0x00, 0x00, 0x54, 0x45, 0x4D, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                new byte[] { 0x0E, 0x00, 0x44, 0x3D, 0x00, 0x01, 0x00, 0x00, 0x61, 0xC4, 0x00, 0x00, 0x61, 0x44 },
                new byte[] { 0x0E, 0x00, 0x44, 0x3D, 0x00, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42 },
                new byte[] { 0x0E, 0x00, 0x44, 0x3D, 0x00, 0x03, 0x00, 0x00, 0xB4, 0xC2, 0x00, 0x00, 0xB4, 0x42 },
                new byte[] { 0x0A, 0x00, 0x44, 0x3D, 0x00, 0x04, 0x44, 0x45, 0x47, 0x00 },
                new byte[] { 0x08, 0x00, 0x44, 0x3D, 0x00, 0x05, 0x50, 0x00 },
                new byte[] { 0x0A, 0x00, 0x44, 0x3D, 0x00, 0x80, 0x01, 0x01, 0x05, 0x01 },
            };
            return SetDefaultConfiguration(defaultConfig);
        }
    }
}
