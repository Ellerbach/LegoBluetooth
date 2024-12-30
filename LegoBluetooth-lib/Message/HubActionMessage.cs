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
        public byte ActionType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubActionMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="actionType">The action type.</param>
        public HubActionMessage(ushort length, byte hubID, byte actionType)
            : base(length, hubID, MessageType.HubActions)
        {
            ActionType = actionType;
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

            return new HubActionMessage((ushort)data.Length, data[1], actionType)
            {
                Message = data,
            };
        }

        /// <summary>
        /// Serializes the HubActionMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubActionMessage.</returns>
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
            data[index++] = ActionType;

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
