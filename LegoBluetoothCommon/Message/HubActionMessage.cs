// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message for performing control actions on the connected Hub.
    /// </summary>
    public class HubActionMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        /// <remarks>
        /// Identifies the property subject to the performed operation.
        /// </remarks>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubActionMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="actionType">The action type.</param>
        public HubActionMessage(byte hubID, ActionType actionType)
            : base(hubID, MessageType.HubActions)
        {
            // Section 3.6
            ActionType = actionType;
            Length = 4;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="HubActionMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="HubActionMessage"/> instance.</returns>
        public static new HubActionMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 4)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 4 bytes.", nameof(data));
            }

            byte actionType = data[3];

            return new HubActionMessage(data[1], (ActionType)actionType)
            {
                Message = data,
                Length = 4
            };
        }

        /// <summary>
        /// Serializes the HubActionMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubActionMessage.</returns>
        public override byte[] ToByteArray()
        {            
            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = (byte)ActionType;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the hub action message.
        /// </summary>
        /// <returns>A string representation of the hub action message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, ActionType: {ActionType}";
        }
    }
}
