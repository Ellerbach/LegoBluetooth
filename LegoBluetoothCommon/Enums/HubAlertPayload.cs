// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the payload of a Hub alert message in the upstream direction.
    /// </summary>
    public enum HubAlertPayload : byte
    {
        /// <summary>
        /// Used not to send the status when creating a downstream class.
        /// </summary>
        NoStatus = 0x01,

        /// <summary>
        /// Status OK.
        /// </summary>
        StatusOK = 0x00,

        /// <summary>
        /// Alert!
        /// </summary>
        Alert = 0xFF
    }
}
