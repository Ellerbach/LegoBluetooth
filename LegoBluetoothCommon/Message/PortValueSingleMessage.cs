// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a 0x45 message that updates the host with addressed LPF2 Device data.
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
        /// Gets or sets the payload of the message as the value depends of the mode, it cannot be decoded right away.
        /// </summary>
        public byte[] Payload { get; set; }

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

            ushort length;
            int index;
            if ((data[0] & 0x80) == 0x80)
            {
                length = (ushort)((data[0] & 0x7F) | (data[1] << 7));
                index = 2;
            }
            else
            {
                length = data[0];
                index = 1;
            }

            var message = new PortValueSingleMessage(data[index])
            {
                Message = data,
                Length = length,
            };

            // Skip hubID and messageType
            index += 2;

            message.Payload = new byte[data.Length - index];
            Array.Copy(data, index, message.Payload, 0, message.Payload.Length);

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

            // If Payload is set directly, use it as raw data (port + value bytes)
            if (Payload != null && Payload.Length > 0 && PortValues.Count == 0)
            {
                foreach (byte b in Payload)
                {
                    data.Add(b);
                    length++;
                }
            }
            else
            {
                foreach (PortValueEntry portValue in PortValues)
            {
                data.Add((byte)portValue.PortID);
                length++;

                switch (portValue.InputValue)
                {
                    case byte value8:
                        data.Add(value8);
                        length++;
                        break;
                    case ushort value16:
                        foreach (byte b in BitConverter.GetBytes(value16))
                        {
                            data.Add(b);
                            length++;
                        }

                        break;
                    case uint value32:
                        foreach (byte b in BitConverter.GetBytes(value32))
                        {
                            data.Add(b);
                            length++;
                        }

                        break;
                    case float valueFloat:
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
            }

            // Compute properly the length
            if (length > 127)
            {
                length += 1; // Account for 2-byte length header
                data.Insert(0, (byte)((length & 0x7F) | 0x80));
                data.Insert(1, (byte)(length >> 7));
            }
            else
            {
                data.Insert(0, (byte)length);
            }

            Length = (ushort)length;

            return (byte[])data.ToArray(typeof(byte));
        }

        /// <summary>
        /// Provides a string representation of the port value (single) message.
        /// </summary>
        /// <returns>A string representation of the port value (single) message.</returns>
        public override string ToString()
        {
            string portValuesString = string.Empty;
            if (PortValues != null)
            {
                foreach (PortValueEntry portValue in PortValues)
                {
                    if (portValuesString.Length > 0)
                    {
                        portValuesString += ", ";
                    }
                    portValuesString += $"PortID: {portValue.PortID}, InputValue: {portValue.InputValue}";
                }
            }

            if (Payload != null)
            {
                portValuesString += $", Payload: {BitConverter.ToString(Payload)}";
            }

            return $"{base.ToString()}, PortValues: [{portValuesString}]";
        }
    }
}
