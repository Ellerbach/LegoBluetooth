// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the capabilities of a port mode in a LEGO Bluetooth device.
    /// </summary>
    [Flags]
    public enum Capabilities : byte
    {
        /// <summary>
        /// Output (seen from Hub).
        /// </summary>
        Output = 0x01,

        /// <summary>
        /// Input (seen from Hub).
        /// </summary>
        Input = 0x02,

        /// <summary>
        /// Logical Combinable.
        /// </summary>
        LogicalCombinable = 0x04,

        /// <summary>
        /// Logical Synchronizable.
        /// </summary>
        LogicalSynchronizable = 0x08,
    }
}
