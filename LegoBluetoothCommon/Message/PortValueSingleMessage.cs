// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that updates the host with addressed LPF2 Device data.
    /// </summary>
    public class PortValueSingleMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the list of port value entries.
        /// </summary>
        public ArrayList PortValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortValueSingleMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        public PortValueSingleMessage(byte hubID)
            : base(hubID, MessageType.PortValueSingle)
        {
            // Section 3.21
            PortValues = new ArrayList();
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortValueSingleMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortValueSingleMessage"/> instance.</returns>
        public static new PortValueSingleMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            var message = new PortValueSingleMessage(data[1])
            {
                Message = data,
            };

            // TODO: fix all this, this is not working properly
            int index = 3;
            while (index < data.Length)
            {
                byte portID = data[index++];
                byte valueType = data[index++];

                object inputValue;
                switch (valueType)
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
                    PortID = portID,
                    InputValue = inputValue
                });
            }

            return message;
        }

        /// <summary>
        /// Serializes the PortValueSingleMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortValueSingleMessage.</returns>
        public override byte[] ToByteArray()
        {
            ArrayList data = new ArrayList();

            int length = 1;

            data.Add(HubID);
            length++;
            data.Add((byte)MessageType);
            length++;

            foreach (PortValueEntry portValue in PortValues)
            {
                data.Add(portValue.PortID);
                length++;

                switch (portValue.InputValue)
                {
                    case byte value8:
                        // 8 bit
                        data.Add(0x00);
                        length++;
                        data.Add(value8);
                        length++;
                        break;
                    case ushort value16:
                        // 16 bit
                        data.Add(0x01);
                        length++;
                        foreach (byte b in BitConverter.GetBytes(value16))
                        {
                            data.Add(b);
                            length++;
                        }

                        break;
                    case uint value32:
                        // 32 bit
                        data.Add(0x02);
                        length++;
                        foreach (byte b in BitConverter.GetBytes(value32))
                        {
                            data.Add(b);
                            length++;
                        }

                        break;
                    case float valueFloat:
                        // FLOAT
                        data.Add(0x03);
                        length++;
                        foreach (byte b in BitConverter.GetBytes(valueFloat))
                        {
                            data.Add(b);
                            length++;
                        }

                        break;
                    default:
                        throw new ArgumentException("Invalid input value type.", nameof(portValue.InputValue));
                }
            }

            Length = (ushort)length;
            // Compute properly the length
            if (length > 127)
            {
                data.Insert(0, (byte)((length >> 8) | 0x80));
                data.Insert(1, (byte)(length & 0xFF));
            }
            else
            {
                data.Insert(0, (byte)length);
            }

            return (byte[])data.ToArray(typeof(byte));
        }

        /// <summary>
        /// Provides a string representation of the port value (single) message.
        /// </summary>
        /// <returns>A string representation of the port value (single) message.</returns>
        public override string ToString()
        {
            string portValuesString = string.Empty;
            foreach (PortValueEntry portValue in PortValues)
            {
                if (portValuesString.Length > 0)
                {
                    portValuesString += ", ";
                }
                portValuesString += $"PortID: {portValue.PortID}, InputValue: {portValue.InputValue}";
            }

            return $"{base.ToString()}, PortValues: [{portValuesString}]";
        }
    }
}
