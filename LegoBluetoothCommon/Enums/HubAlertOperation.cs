// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different operations that can be performed on Hub alerts.
    /// </summary>
    public enum HubAlertOperation : byte
    {
        /// <summary>
        /// Enable updates (Downstream).
        /// </summary>
        EnableUpdates = 0x01,

        /// <summary>
        /// Disable updates (Downstream).
        /// </summary>
        DisableUpdates = 0x02,

        /// <summary>
        /// Request updates (Downstream).
        /// </summary>
        RequestUpdates = 0x03,

        /// <summary>
        /// Update (Upstream).
        /// </summary>
        Update = 0x04
    }
}

