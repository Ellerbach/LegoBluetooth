// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Text;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message to trigger the Hub to go into boot mode for firmware update.
    /// </summary>
    public class GoIntoBootModeMessage : CommonMessageHeader
    {
        /// <summary>
        /// The safety string required to trigger the boot mode.
        /// </summary>
        public const string SafetyString = "LPF2-Boot";

        /// <summary>
        /// Initializes a new instance of the <see cref="GoIntoBootModeMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        public GoIntoBootModeMessage(byte hubID)
            : base(hubID, MessageType.FWUpdateGoIntoBootMode)
        {
            // Section 5.1
            Length = 12;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="GoIntoBootModeMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="GoIntoBootModeMessage"/> instance.</returns>
        public static new GoIntoBootModeMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 12)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 12 bytes.", nameof(data));
            }

            string safetyString = Encoding.UTF8.GetString(data, 3, 9);

            if (safetyString != SafetyString)
            {
                throw new ArgumentException("Invalid safety string.", nameof(data));
            }

            return new GoIntoBootModeMessage(data[1])
            {
                Message = data,
                Length = 12,
            };
        }

        /// <summary>
        /// Serializes the GoIntoBootModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the GoIntoBootModeMessage.</returns>
        public override byte[] ToByteArray()
        {
            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(SafetyString), 0, data, index, 9);

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the Go Into Boot Mode message.
        /// </summary>
        /// <returns>A string representation of the Go Into Boot Mode message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, SafetyString: {SafetyString}";
        }
    }
}
