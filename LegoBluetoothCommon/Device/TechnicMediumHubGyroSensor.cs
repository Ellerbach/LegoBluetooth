// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth.Device
{
    /// <summary>
    /// Represents a Technic Medium Hub gyro sensor device in a LEGO Bluetooth system.
    /// </summary>
    public class TechnicMediumHubGyroSensor : BaseDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicMediumHubGyroSensor"/> class.
        /// </summary>
        /// <param name="ble">The Bluetooth interface.</param>
        /// <param name="hubID">The ID of the hub.</param>
        /// <param name="portID">The ID of the port.</param>
        public TechnicMediumHubGyroSensor(IBluetooth ble, byte hubID, byte portID) : base(ble, hubID, portID)
        {
            HWVersion = new Version(0, 0, 0, 1);
            SWVersion = new Version(0, 0, 0, 1);
            DeviceType = IOTypeID.TechnicMediumHubGyroSensor;
        }

        /// <inheritdoc/>
        public override ArrayList GetDefaultConfiguration()
        {
            var defaultConfig = new ArrayList()
            {
                new byte[] { 0x0B, 0x00, 0x43, 0x62, 0x01, 0x02, 0x01, 0x01, 0x00, 0x00, 0x00 },
                new byte[] { 0x05, 0x00, 0x43, 0x62, 0x02 },
                new byte[] { 0x11, 0x00, 0x44, 0x62, 0x00, 0x00, 0x52, 0x4F, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                new byte[] { 0x0E, 0x00, 0x44, 0x62, 0x00, 0x01, 0xD7, 0x36, 0xDF, 0xC6, 0xD7, 0x36, 0xDF, 0x46 },
                new byte[] { 0x0E, 0x00, 0x44, 0x62, 0x00, 0x02, 0x00, 0x00, 0xC8, 0xC2, 0x00, 0x00, 0xC8, 0x42 },
                new byte[] { 0x0E, 0x00, 0x44, 0x62, 0x00, 0x03, 0x00, 0x00, 0xFA, 0xC4, 0x00, 0x00, 0xFA, 0x44 },
                new byte[] { 0x0A, 0x00, 0x44, 0x62, 0x00, 0x04, 0x44, 0x50, 0x53, 0x00 },
                new byte[] { 0x08, 0x00, 0x44, 0x62, 0x00, 0x05, 0x50, 0x00 },
                new byte[] { 0x0A, 0x00, 0x44, 0x62, 0x00, 0x80, 0x03, 0x01, 0x03, 0x00 },
            };
            return SetDefaultConfiguration(defaultConfig);
        }
    }
}
