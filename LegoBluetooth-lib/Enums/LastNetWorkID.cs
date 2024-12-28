namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different Last Network IDs used in the LEGO BLE device pairing process.
    /// </summary>
    public enum LastNetworkID
    {
        /// <summary>
        /// NONE (unknown).
        /// </summary>
        None = 0,

        // The ID’s used for “Last Connection” Network ID’s. Used in H/W network “auto connect”.
        // LastConnection1 = 1,
        // LastConnection2 = 2,
        // LastConnection3 = 3,
        // ... (continue for values 4 to 250)
        // LastConnection250 = 250,

        /// <summary>
        /// DEFAULT 1, Locked.
        /// </summary>
        Locked = 251,

        /// <summary>
        /// DEFAULT 2, NOT Locked.
        /// </summary>
        NotLocked = 252,

        /// <summary>
        /// DEFAULT 3, RSSI Dependent.
        /// </summary>
        RSSIDependent = 253,

        /// <summary>
        /// DEFAULT 4, DISABLE H/W Network.
        /// </summary>
        DisableHWNetwork = 254,

        /// <summary>
        /// DON’T CARE - NOT Implemented.
        /// </summary>
        DontCareNotImplemented = 255
    }
}

