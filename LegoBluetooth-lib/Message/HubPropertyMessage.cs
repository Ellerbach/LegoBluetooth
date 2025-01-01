using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Hub Property Message which includes the common message header and additional properties.
    /// </summary>
    public class HubPropertyMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Hub Property.
        /// </summary>
        /// <remarks>
        /// Identifies the Hub Property subject to the performed operation.
        /// </remarks>
        public HubProperty Property { get; set; }

        /// <summary>
        /// Gets or sets the Hub Property Operation.
        /// </summary>
        /// <remarks>
        /// Identifies the type of Hub Property Operation performed.
        /// </remarks>
        public HubPropertyOperation Operation { get; set; }

        /// <summary>
        /// Gets or sets the payload of the message.
        /// </summary>
        /// <remarks>
        /// The Hub Property Payload. The Common Header Length field governs the length of the payload.
        /// </remarks>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubPropertyMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="property">The Hub Property.</param>
        /// <param name="operation">The Hub Property Operation.</param>
        /// <param name="payload">The payload of the message.</param>
        public HubPropertyMessage(ushort length, byte hubID, HubProperty property, HubPropertyOperation operation, byte[] payload)
            : base(length, hubID, MessageType.HubProperties)
        {
            Property = property;
            Operation = operation;
            Payload = payload;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="HubPropertyMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="HubPropertyMessage"/> instance.</returns>
        public static new HubPropertyMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            ushort length;
            int index = 0;

            if ((data[0] & 0x80) == 0x80)
            {
                if (data.Length < 6)
                {
                    throw new ArgumentException("Invalid data array. Must contain at least 6 bytes for extended length encoding.", nameof(data));
                }
                length = (ushort)(((data[0] & 0x7F) << 8) | data[1]);
                index = 2;
            }
            else
            {
                length = data[0];
                index = 1;
            }

            byte hubID = data[index++];
            byte messageType = data[index++];
            HubProperty property = (HubProperty)data[index++];
            HubPropertyOperation operation = (HubPropertyOperation)data[index++];

            byte[] payload = new byte[length - 5];
            if (payload.Length > 0)
            {
                Array.Copy(data, index, payload, 0, length - 5);
            }

            return new HubPropertyMessage(length, hubID, property, operation, payload);
        }

        /// <summary>
        /// Serializes the HubPropertyMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubPropertyMessage.</returns>
        public override byte[] ToByteArray()
        {
            byte[] data;
            int index = 0;

            Length = (Payload != null) ? (ushort)(5 + Payload.Length) : (ushort)5;

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
            data[index++] = (byte)Property;
            data[index++] = (byte)Operation;

            if ((Payload != null) && (Payload.Length > 0))
            {
                Array.Copy(Payload, 0, data, index, Payload.Length);
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the Hub Property Message.
        /// </summary>
        /// <returns>A string representation of the Hub Property Message.</returns>
        public override string ToString()
        {
            return $"Length: {Length}, HubID: {HubID}, MessageType: {MessageType}, Property: {Property}, Operation: {Operation}, Payload: {BitConverter.ToString(Payload)}";
        }
    }
}

