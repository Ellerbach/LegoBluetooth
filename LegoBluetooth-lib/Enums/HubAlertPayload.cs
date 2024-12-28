namespace LegoBluetooth
{
    /// <summary>
    /// Represents the payload of a Hub alert message in the upstream direction.
    /// </summary>
    public enum HubAlertPayload : byte
    {
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
