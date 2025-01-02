// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that updates the host with values from a Combined Mode sensor.
    /// </summary>
    public class PortValueCombinedModeMessage : CommonMessageHeader
    {
        /// <summary>
        /// Represents a port value entry.
        /// </summary>
        public class PortValueEntry
        {
            /// <summary>
            /// Gets or sets the bit pointer for the mode/dataset combination.
            /// </summary>
            public ushort BitPointer { get; set; }

            /// <summary>
            /// Gets or sets the input value of the addressed port.
            /// </summary>
            public object InputValue { get; set; }
        }

        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the list of port value entries.
        /// </summary>
        public ArrayList PortValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortValueCombinedModeMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        public PortValueCombinedModeMessage(ushort length, byte hubID, byte portID)
            : base(length, hubID, MessageType.PortValueCombinedMode)
        {
            PortID = portID;
            PortValues = new ArrayList();
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortValueCombinedModeMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortValueCombinedModeMessage"/> instance.</returns>
        public static new PortValueCombinedModeMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 6)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 6 bytes.", nameof(data));
            }

            byte portID = data[3];
            ushort bitPointer = BitConverter.ToUInt16(data, 4);

            var message = new PortValueCombinedModeMessage((ushort)data.Length, data[1], portID)
            {
                Message = data,
            };

            int index = 6;
            while (index < data.Length)
            {
                object inputValue;
                switch (bitPointer)
                {
                    case 0x00: // 8 bit
                        inputValue = data[index++];
                        break;
                    case 0x01: // 16 bit
                        inputValue = BitConverter.ToUInt16(data, index);
                        index += 2;
                        break;
                    case 0x02: // 32 bit
                        inputValue = BitConverter.ToUInt32(data, index);
                        index += 4;
                        break;
                    case 0x03: // FLOAT
                        inputValue = BitConverter.ToSingle(data, index);
                        index += 4;
                        break;
                    default:
                        throw new ArgumentException("Invalid value type.", nameof(data));
                }

                message.PortValues.Add(new PortValueEntry
                {
                    BitPointer = bitPointer,
                    InputValue = inputValue
                });

                bitPointer++;
            }

            return message;
        }

        /// <summary>
        /// Serializes the PortValueCombinedModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortValueCombinedModeMessage.</returns>
        public override byte[] ToByteArray()
        {
            ArrayList data = new ArrayList();

            if (Length < 127)
            {
                data.Add((byte)Length);
            }
            else
            {
                data.Add((byte)((Length >> 8) | 0x80));
                data.Add((byte)(Length & 0xFF));
            }

            data.Add(HubID);
            data.Add((byte)MessageType);
            data.Add(PortID);

            foreach (PortValueEntry portValue in PortValues)
            {
                byte[] bitPointerBytes = BitConverter.GetBytes(portValue.BitPointer);
                foreach (byte b in bitPointerBytes)
                {
                    data.Add(b);
                }

                switch (portValue.InputValue)
                {
                    case byte value8:
                        data.Add(value8);
                        break;
                    case ushort value16:
                        foreach (byte b in BitConverter.GetBytes(value16))
                        {
                            data.Add(b);
                        }
                        break;
                    case uint value32:
                        foreach (byte b in BitConverter.GetBytes(value32))
                        {
                            data.Add(b);
                        }
                        break;
                    case float valueFloat:
                        foreach (byte b in BitConverter.GetBytes(valueFloat))
                        {
                            data.Add(b);
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid input value type.", nameof(portValue.InputValue));
                }
            }

            return (byte[])data.ToArray(typeof(byte));
        }

        /// <summary>
        /// Provides a string representation of the port value (combined mode) message.
        /// </summary>
        /// <returns>A string representation of the port value (combined mode) message.</returns>
        public override string ToString()
        {
            string portValuesString = "";
            foreach (PortValueEntry portValue in PortValues)
            {
                if (portValuesString.Length > 0)
                {
                    portValuesString += ", ";
                }
                portValuesString += $"BitPointer: {portValue.BitPointer}, InputValue: {portValue.InputValue}";
            }

            return $"{base.ToString()}, PortID: {PortID}, PortValues: [{portValuesString}]";
        }
    }
}
