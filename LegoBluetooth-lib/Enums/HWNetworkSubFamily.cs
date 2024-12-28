namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different H/W Network sub-families (flashes) for the Hub.
    /// </summary>
    public enum HWNetworkSubFamily : byte
    {
        /// <summary>
        /// Sub-Family 1 - One Flash.
        /// </summary>
        OneFlash = 0x01,

        /// <summary>
        /// Sub-Family 2 - Two Flashes.
        /// </summary>
        TwoFlashes = 0x02,

        /// <summary>
        /// Sub-Family 3 - Three Flashes.
        /// </summary>
        ThreeFlashes = 0x03,

        /// <summary>
        /// Sub-Family 4 - Four Flashes.
        /// </summary>
        FourFlashes = 0x04,

        /// <summary>
        /// Sub-Family 5 - Five Flashes.
        /// </summary>
        FiveFlashes = 0x05,

        /// <summary>
        /// Sub-Family 6 - Six Flashes.
        /// </summary>
        SixFlashes = 0x06,

        /// <summary>
        /// Sub-Family 7 - Seven Flashes.
        /// </summary>
        SevenFlashes = 0x07
    }
}
