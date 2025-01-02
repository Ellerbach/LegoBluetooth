// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the status errors that can occur in Bluetooth operations.
    /// </summary>
    public enum StatusError
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// An error occurred while writing data.
        /// </summary>
        WriteError = 1,

        /// <summary>
        /// An error occurred while notifying data.
        /// </summary>
        NotifyError = 2,

        /// <summary>
        /// An error occurred while establishing a connection.
        /// </summary>
        ConnectionError = 3,
    }
}
