// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that provides a handshake notification of changes made to the Port Input Format.
    /// </summary>
    public class PortInputFormatSingleMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the mode of the addressed Input.
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// Gets or sets the delta interval for triggering a new value update notification.
        /// </summary>
        public uint DeltaInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notifications are enabled.
        /// </summary>
        public bool NotificationEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInputFormatSingleMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="mode">The mode of the addressed Input.</param>
        /// <param name="deltaInterval">The delta interval for triggering a new value update notification.</param>
        /// <param name="notificationEnabled">A value indicating whether notifications are enabled.</param>
        public PortInputFormatSingleMessage(ushort length, byte hubID, byte portID, byte mode, uint deltaInterval, bool notificationEnabled)
            : base(length, hubID, MessageType.PortInputFormatSingle)
        {
            PortID = portID;
            Mode = mode;
            DeltaInterval = deltaInterval;
            NotificationEnabled = notificationEnabled;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInputFormatSingleMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInputFormatSingleMessage"/> instance.</returns>
        public static new PortInputFormatSingleMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 10)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 10 bytes.", nameof(data));
            }

            byte portID = data[3];
            byte mode = data[4];
            uint deltaInterval = BitConverter.ToUInt32(data, 5);
            bool notificationEnabled = data[9] == 1;

            return new PortInputFormatSingleMessage((ushort)data.Length, data[1], portID, mode, deltaInterval, notificationEnabled)
            {
                Message = data,
            };
        }

        /// <summary>
        /// Serializes the PortInputFormatSingleMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInputFormatSingleMessage.</returns>
        public override byte[] ToByteArray()
        {
            byte[] data;
            int index = 0;

            if (Length < 127)
            {
                data = new byte[Length + 1];
                data[index++] = (byte)Length;
            }
            else
            {
                data = new byte[Length + 2];
                data[index++] = (byte)((Length >> 8) | 0x80);
                data[index++] = (byte)(Length & 0xFF);
            }

            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = PortID;
            data[index++] = Mode;
            Array.Copy(BitConverter.GetBytes(DeltaInterval), 0, data, index, 4);
            index += 4;
            data[index++] = (byte)(NotificationEnabled ? 1 : 0);

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port input format (single) message.
        /// </summary>
        /// <returns>A string representation of the port input format (single) message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, Mode: {Mode}, DeltaInterval: {DeltaInterval}, NotificationEnabled: {NotificationEnabled}";
        }
    }
}
