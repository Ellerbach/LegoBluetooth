// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message used to send output commands to the port.
    /// </summary>
    public class PortOutputCommandMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the startup and completion information.
        /// </summary>
        public StartupCompletionInfo StartupCompletion { get; set; }

        /// <summary>
        /// Gets or sets the sub-command to execute.
        /// </summary>
        public SubCommandType SubCommand { get; set; }

        /// <summary>
        /// Gets or sets the payload for the sub-command.
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortOutputCommandMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="startupCompletion">The startup and completion information.</param>
        /// <param name="subCommand">The sub-command to execute.</param>
        /// <param name="payload">The payload for the sub-command.</param>
        public PortOutputCommandMessage(byte hubID, byte portID, StartupCompletionInfo startupCompletion, SubCommandType subCommand, byte[] payload)
            : base(hubID, MessageType.PortOutputCommand)
        {
            // Section 3.26
            PortID = portID;
            StartupCompletion = startupCompletion;
            SubCommand = subCommand;
            Payload = payload;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortOutputCommandMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortOutputCommandMessage"/> instance.</returns>
        public static new PortOutputCommandMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 6)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 6 bytes.", nameof(data));
            }

            int index = data.Length > 127 ? 2 : 1;
            byte hubId = data[index++];
            // Skipping command type
            index++;
            byte portID = data[index++];
            StartupCompletionInfo startupCompletion = (StartupCompletionInfo)data[index++];
            SubCommandType subCommand = (SubCommandType)data[index++];
            byte[] payload = new byte[data.Length - index];
            Array.Copy(data, index, payload, 0, payload.Length);

            return new PortOutputCommandMessage(hubId, portID, startupCompletion, subCommand, payload)
            {
                Message = data,
                // Using data.Length to avoid computing Length even if may not be correct
                Length = (ushort)data.Length,
            };
        }

        /// <summary>
        /// Serializes the PortOutputCommandMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortOutputCommandMessage.</returns>
        public override byte[] ToByteArray()
        {
            Length = (ushort)(6 + Payload.Length);

            byte[] data;
            int index = 0;

            if (Length < 127)
            {
                data = new byte[Length];
                data[index++] = (byte)Length;
            }
            else
            {
                data = new byte[Length + 1];
                data[index++] = (byte)((Length >> 8) | 0x80);
                data[index++] = (byte)(Length & 0xFF);
            }

            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = PortID;
            data[index++] = (byte)StartupCompletion;
            data[index++] = (byte)SubCommand;
            Array.Copy(Payload, 0, data, index, Payload.Length);

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port output command message.
        /// </summary>
        /// <returns>A string representation of the port output command message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, StartupCompletion: {StartupCompletion}, SubCommand: {SubCommand}, Payload: {BitConverter.ToString(Payload)}";
        }
    }
}