// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Interface for Bluetooth communication.
    /// </summary>
    public interface IBluetooth
    {
        /// <summary>
        /// Delegate for processing incoming data.
        /// </summary>
        /// <param name="data">The incoming data as a byte array.</param>
        delegate void ProcessIncomingHandler(byte[] data);

        /// <summary>
        /// Gets or sets the handler for processing incoming data.
        /// </summary>
        ProcessIncomingHandler ProcessIncoming { get; set; }

        /// <summary>
        /// Delegate for handling client joining state changes.
        /// </summary>
        /// <param name="joining">Indicates whether a client is joining.</param>
        delegate void ClientJoiningStateChangedHandler(bool joining);

        /// <summary>
        /// Gets or sets the handler for client joining state changes.
        /// </summary>
        ClientJoiningStateChangedHandler ClientJoiningStateChanged { get; set; }

        /// <summary>
        /// Delegate for handling errors.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        delegate void OnErrorHandler(StatusError error);

        /// <summary>
        /// Event triggered when an error occurs.
        /// </summary>
        event OnErrorHandler OnError;

        /// <summary>
        /// Connects to the Bluetooth device.
        /// </summary>
        /// <returns>True if the connection was successful, otherwise false.</returns>
        bool Connect();

        /// <summary>
        /// Gets a value indicating whether the device is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Disconnects from the Bluetooth device.
        /// </summary>
        /// <returns>True if the disconnection was successful, otherwise false.</returns>
        bool Disconnect();

        /// <summary>
        /// Notifies the Bluetooth device with the specified data.
        /// </summary>
        /// <param name="data">The data to notify as a byte array.</param>
        /// <returns>True if the notification was successful, otherwise false.</returns>
        bool NotifyValue(byte[] data);
    }
}
