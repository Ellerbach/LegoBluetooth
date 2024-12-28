namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different I/O events for the Hub.
    /// </summary>
    public enum IOEvent : byte
    {
        /// <summary>
        /// Detached I/O.
        /// </summary>
        DetachedIO = 0x00,

        /// <summary>
        /// Attached I/O.
        /// </summary>
        AttachedIO = 0x01,

        /// <summary>
        /// Attached Virtual I/O.
        /// </summary>
        AttachedVirtualIO = 0x02
    }
}
