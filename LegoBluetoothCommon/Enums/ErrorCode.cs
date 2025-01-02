// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different error codes for the Hub.
    /// </summary>
    public enum ErrorCode : byte
    {
        /// <summary>
        /// Acknowledgment.
        /// </summary>
        ACK = 0x01,

        /// <summary>
        /// Message Acknowledgment.
        /// </summary>
        MACK = 0x02,

        /// <summary>
        /// Buffer Overflow.
        /// </summary>
        BufferOverflow = 0x03,

        /// <summary>
        /// Timeout.
        /// </summary>
        Timeout = 0x04,

        /// <summary>
        /// Command NOT recognized.
        /// </summary>
        CommandNotRecognized = 0x05,

        /// <summary>
        /// Invalid use (e.g. parameter error(s)).
        /// </summary>
        InvalidUse = 0x06,

        /// <summary>
        /// Overcurrent.
        /// </summary>
        Overcurrent = 0x07,

        /// <summary>
        /// Internal ERROR.
        /// </summary>
        InternalError = 0x08
    }
}
