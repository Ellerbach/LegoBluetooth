namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different H/W Network command types for the Hub.
    /// </summary>
    public enum HWNetworkCommandType : byte
    {
        /// <summary>
        /// Connection Request.
        /// </summary>
        ConnectionRequest = 0x02,

        /// <summary>
        /// Family Request [New family if available].
        /// </summary>
        FamilyRequest = 0x03,

        /// <summary>
        /// Family Set.
        /// </summary>
        FamilySet = 0x04,

        /// <summary>
        /// Join Denied.
        /// </summary>
        JoinDenied = 0x05,

        /// <summary>
        /// Get Family.
        /// </summary>
        GetFamily = 0x06,

        /// <summary>
        /// Family.
        /// </summary>
        Family = 0x07,

        /// <summary>
        /// Get SubFamily.
        /// </summary>
        GetSubFamily = 0x08,

        /// <summary>
        /// SubFamily.
        /// </summary>
        SubFamily = 0x09,

        /// <summary>
        /// SubFamily Set.
        /// </summary>
        SubFamilySet = 0x0A,

        /// <summary>
        /// Get Extended Family.
        /// </summary>
        GetExtendedFamily = 0x0B,

        /// <summary>
        /// Extended Family.
        /// </summary>
        ExtendedFamily = 0x0C,

        /// <summary>
        /// Extended Family Set.
        /// </summary>
        ExtendedFamilySet = 0x0D,

        /// <summary>
        /// Reset Long Press Timing.
        /// </summary>
        ResetLongPressTiming = 0x0E
    }
}
