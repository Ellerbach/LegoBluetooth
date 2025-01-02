// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different information types for the Hub.
    /// </summary>
    public enum InformationType : byte
    {
        /// <summary>
        /// Port Value.
        /// </summary>
        PortValue = 0x00,

        /// <summary>
        /// Mode Info.
        /// </summary>
        ModeInfo = 0x01,

        /// <summary>
        /// Possible Mode Combinations.
        /// Should only be used if the “Logical Combinable”-bit is set in the (MODE INFO Capabilities byte).
        /// </summary>
        PossibleModeCombinations = 0x02
    }
}
