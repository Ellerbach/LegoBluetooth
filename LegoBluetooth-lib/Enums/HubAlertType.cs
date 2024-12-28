namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different types of alerts that can be sent by the Hub.
    /// </summary>
    public enum HubAlertType : byte
    {
        /// <summary>
        /// Indicates a low voltage condition.
        /// </summary>
        LowVoltage = 0x01,

        /// <summary>
        /// Indicates a high current condition.
        /// </summary>
        HighCurrent = 0x02,

        /// <summary>
        /// Indicates a low signal strength condition.
        /// </summary>
        LowSignalStrength = 0x03,

        /// <summary>
        /// Indicates an over power condition.
        /// </summary>
        OverPowerCondition = 0x04
    }
}
