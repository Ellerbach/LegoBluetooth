// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a H/W Network message.
    /// </summary>
    public class HWNetworkMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the H/W Network command type.
        /// </summary>
        public HWNetworkCommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the payload for the H/W Network message.
        /// </summary>
        public byte Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HWNetworkMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="commandType">The H/W Network command type.</param>
        /// <param name="payload">The payload for the H/W Network message.</param>
        public HWNetworkMessage(ushort length, byte hubID, HWNetworkCommandType commandType, byte payload)
            : base(length, hubID, MessageType.HWNetworkCommands)
        {
            CommandType = commandType;
            Payload = payload;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="HWNetworkMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="HWNetworkMessage"/> instance.</returns>
        public static new HWNetworkMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            HWNetworkCommandType commandType = (HWNetworkCommandType)data[3];
            byte payload = data[4];

            return new HWNetworkMessage((ushort)data.Length, data[1], commandType, payload)
            {
                Message = data,
            };
        }

        /// <summary>
        /// Serializes the HubPropertyMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubPropertyMessage.</returns>
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
            data[index++] = (byte)CommandType;
            data[index++] = Payload;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the H/W Network message.
        /// </summary>
        /// <returns>A string representation of the H/W Network message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, CommandType: {CommandType}, Payload: {Payload}";
        }
    }
}
