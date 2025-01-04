// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

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
        /// Gets or sets the mode/dataset combination bit pointer.
        /// </summary>
        public ushort ModeDatasetCombinationBitPointer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInputFormatCombinedModeMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="combinedControlByte">The combined control byte.</param>
        /// <param name="modeDatasetCombinationBitPointer">The mode/dataset combination bit pointer.</param>
        public PortInputFormatCombinedModeMessage(byte hubID, byte portID, byte combinedControlByte, ushort modeDatasetCombinationBitPointer)
            : base(hubID, MessageType.PortInputFormatCombinedMode)
        {
            // Section 3.24
            PortID = portID;
            CombinedControlByte = combinedControlByte;
            ModeDatasetCombinationBitPointer = modeDatasetCombinationBitPointer;
            Length = 7;
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

            byte portID = data[3];
            byte combinedControlByte = data[4];
            ushort modeDatasetCombinationBitPointer = BitConverter.ToUInt16(data, 5);

            return new PortInputFormatCombinedModeMessage(data[1], portID, combinedControlByte, modeDatasetCombinationBitPointer)
            {
                Message = data,
                Length = data[0],
            };
        }

        /// <summary>
        /// Serializes the PortInputFormatCombinedModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInputFormatCombinedModeMessage.</returns>
        public override byte[] ToByteArray()
        {
            Length = 7;
            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = PortID;
            data[index++] = CombinedControlByte;
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
            return $"{base.ToString()}, PortID: {PortID}, CombinedControlByte: {CombinedControlByte}, ModeDatasetCombinationBitPointer: {ModeDatasetCombinationBitPointer}";
        }
    }
}
