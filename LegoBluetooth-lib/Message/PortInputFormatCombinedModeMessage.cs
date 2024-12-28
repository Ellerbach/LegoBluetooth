using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that provides a handshake notification for the setup of one or more bit-pointers in the 16-bit Mode/Dataset Combination Bit Pointer.
    /// </summary>
    public class PortInputFormatCombinedModeMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID for this combined sensor.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the combined control byte.
        /// </summary>
        public byte CombinedControlByte { get; set; }

        /// <summary>
        /// Gets or sets the used combination index.
        /// </summary>
        public byte UsedCombinationIndex { get; set; }

        /// <summary>
        /// Gets or sets the mode/dataset combination bit pointer.
        /// </summary>
        public ushort ModeDatasetCombinationBitPointer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInputFormatCombinedModeMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="combinedControlByte">The combined control byte.</param>
        /// <param name="usedCombinationIndex">The used combination index.</param>
        /// <param name="modeDatasetCombinationBitPointer">The mode/dataset combination bit pointer.</param>
        public PortInputFormatCombinedModeMessage(ushort length, byte hubID, byte portID, byte combinedControlByte, byte usedCombinationIndex, ushort modeDatasetCombinationBitPointer)
            : base(length, hubID, MessageType.PortInputFormatCombinedMode)
        {
            PortID = portID;
            CombinedControlByte = combinedControlByte;
            UsedCombinationIndex = usedCombinationIndex;
            ModeDatasetCombinationBitPointer = modeDatasetCombinationBitPointer;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInputFormatCombinedModeMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInputFormatCombinedModeMessage"/> instance.</returns>
        public static new PortInputFormatCombinedModeMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 7)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 7 bytes.", nameof(data));
            }

            var commonHeader = CommonMessageHeader.Decode(data);
            byte portID = data[3];
            byte combinedControlByte = data[4];
            byte usedCombinationIndex = data[5];
            ushort modeDatasetCombinationBitPointer = BitConverter.ToUInt16(data, 6);

            return new PortInputFormatCombinedModeMessage(commonHeader.Length, commonHeader.HubID, portID, combinedControlByte, usedCombinationIndex, modeDatasetCombinationBitPointer)
            {
                Message = commonHeader.Message,
            };
        }

        /// <summary>
        /// Serializes the PortInputFormatCombinedModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInputFormatCombinedModeMessage.</returns>
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
            data[index++] = PortID;
            data[index++] = CombinedControlByte;
            data[index++] = UsedCombinationIndex;
            Array.Copy(BitConverter.GetBytes(ModeDatasetCombinationBitPointer), 0, data, index, 2);

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port input format (combined mode) message.
        /// </summary>
        /// <returns>A string representation of the port input format (combined mode) message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, CombinedControlByte: {CombinedControlByte}, UsedCombinationIndex: {UsedCombinationIndex}, ModeDatasetCombinationBitPointer: {ModeDatasetCombinationBitPointer}";
        }
    }
}
