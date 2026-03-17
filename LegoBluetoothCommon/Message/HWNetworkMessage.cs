// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a H/W Network 0x08 message.
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
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="commandType">The H/W Network command type.</param>
        /// <param name="payload">The payload for the H/W Network message.</param>
        public HWNetworkMessage(byte hubID, HWNetworkCommandType commandType, byte payload)
            : base(hubID, MessageType.HWNetworkCommands)
        {
            // Section 3.10
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
            if (data == null || data.Length < 4)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            HWNetworkCommandType commandType = (HWNetworkCommandType)data[3];
            byte payload = 0;

            // Size is either 4 or 5 bytes. If it's 5, the 5th byte is the payload.
            if (data.Length > 4)
            {
                payload = data[4];
            }

            return new HWNetworkMessage(data[1], commandType, payload)
            {
                Message = data,
                Length = data[0],
            };
        }

        /// <summary>
        /// Serializes the HubPropertyMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubPropertyMessage.</returns>
        public override byte[] ToByteArray()
        {
            switch (CommandType)
            {
                case HWNetworkCommandType.ConnectionRequest:
                case HWNetworkCommandType.FamilySet:
                case HWNetworkCommandType.Family:
                case HWNetworkCommandType.SubFamily:
                case HWNetworkCommandType.SubFamilySet:
                case HWNetworkCommandType.ExtendedFamily:
                    Length = 5;
                    break;
                case HWNetworkCommandType.FamilyRequest:
                case HWNetworkCommandType.GetSubFamily:
                case HWNetworkCommandType.ExtendedFamilySet:
                case HWNetworkCommandType.GetFamily:
                case HWNetworkCommandType.GetExtendedFamily:
                case HWNetworkCommandType.ResetLongPressTiming:
                case HWNetworkCommandType.JoinDenied:
                default:
                    Length = 4;
                    break;
            }

            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = (byte)CommandType;
            if (Length >= 5)
            {
                data[index++] = Payload;
            }

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
