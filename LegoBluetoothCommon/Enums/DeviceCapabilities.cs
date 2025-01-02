// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different capabilities of a LEGO BLE compatible device.
    /// </summary>
    [Flags]
    public enum DeviceCapabilities
    {
        /// <summary>
        /// Supports Central Role.
        /// </summary>
        SupportsCentralRole = 0b0000_0001,

        /// <summary>
        /// Supports Peripheral Role.
        /// </summary>
        SupportsPeripheralRole = 0b0000_0010,

        /// <summary>
        /// Supports LPF2 devices (H/W connectors).
        /// </summary>
        SupportsLPF2Devices = 0b0000_0100,

        /// <summary>
        /// Acts as a Remote Controller (R/C).
        /// </summary>
        ActsAsRemoteController = 0b0000_1000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD1 = 0b0001_0000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD2 = 0b0010_0000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD3 = 0b0100_0000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD4 = 0b1000_0000
    }
}

