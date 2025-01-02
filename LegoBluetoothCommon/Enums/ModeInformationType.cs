// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different mode information types for the Hub.
    /// </summary>
    public enum ModeInformationType : byte
    {
        /// <summary>
        /// Name of the mode.
        /// </summary>
        Name = 0x00,

        /// <summary>
        /// The raw range.
        /// </summary>
        Raw = 0x01,

        /// <summary>
        /// The percent range.
        /// </summary>
        Percent = 0x02,

        /// <summary>
        /// The SI value range.
        /// </summary>
        SI = 0x03,

        /// <summary>
        /// The standard name of value.
        /// </summary>
        Symbol = 0x04,

        /// <summary>
        /// Mapping.
        /// </summary>
        Mapping = 0x05,

        /// <summary>
        /// Used internally.
        /// </summary>
        InternalUse = 0x06,

        /// <summary>
        /// Motor Bias (0-100%).
        /// </summary>
        MotorBias = 0x07,

        /// <summary>
        /// Capability bits (6 bytes total).
        /// </summary>
        CapabilityBits = 0x08,

        /// <summary>
        /// Value encoding.
        /// </summary>
        ValueFormat = 0x80
    }
}
