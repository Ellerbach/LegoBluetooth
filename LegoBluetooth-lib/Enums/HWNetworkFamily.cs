namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different H/W Network families (colors) for the Hub.
    /// </summary>
    public enum HWNetworkFamily : byte
    {
        /// <summary>
        /// Family 0 (zero) - White.
        /// </summary>
        White = 0x00,

        /// <summary>
        /// Family 1 - Green.
        /// </summary>
        Green = 0x01,

        /// <summary>
        /// Family 2 - Yellow.
        /// </summary>
        Yellow = 0x02,

        /// <summary>
        /// Family 3 - Red.
        /// </summary>
        Red = 0x03,

        /// <summary>
        /// Family 4 - Blue.
        /// </summary>
        Blue = 0x04,

        /// <summary>
        /// Family 5 - Purple.
        /// </summary>
        Purple = 0x05,

        /// <summary>
        /// Family 6 - Light Blue.
        /// </summary>
        LightBlue = 0x06,

        /// <summary>
        /// Family 7 - Teal.
        /// </summary>
        Teal = 0x07,

        /// <summary>
        /// Family 8 - Pink.
        /// </summary>
        Pink = 0x08
    }
}
