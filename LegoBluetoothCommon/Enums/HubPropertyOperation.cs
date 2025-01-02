// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the Hub Property Operation.
    /// </summary>
    public enum HubPropertyOperation
    {
        /// <summary>
        /// Set (Downstream).
        /// </summary>
        Set = 0x01,

        /// <summary>
        /// Enable Updates (Downstream).
        /// </summary>
        EnableUpdates = 0x02,

        /// <summary>
        /// Disable Updates (Downstream).
        /// </summary>
        DisableUpdates = 0x03,

        /// <summary>
        /// Reset (Downstream).
        /// </summary>
        Reset = 0x04,

        /// <summary>
        /// Request Update (Downstream).
        /// </summary>
        RequestUpdate = 0x05,

        /// <summary>
        /// Update (Upstream).
        /// </summary>
        Update = 0x06
    }
}
