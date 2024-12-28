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
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        public GoIntoBootModeMessage(ushort length, byte hubID)
            : base(length, hubID, MessageType.FWUpdateGoIntoBootMode)
        {
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

            var commonHeader = CommonMessageHeader.Decode(data);
            string safetyString = Encoding.UTF8.GetString(data, 3, 9);

            if (safetyString != SafetyString)
            {
                throw new ArgumentException("Invalid safety string.", nameof(data));
            }

            return new GoIntoBootModeMessage(commonHeader.Length, commonHeader.HubID)
            {
                Message = commonHeader.Message,
            };
        }

        /// <summary>
        /// Serializes the GoIntoBootModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the GoIntoBootModeMessage.</returns>
        public byte[] ToByteArray()
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
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(SafetyString), 0, data, index, 9);
            index += 9;

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
