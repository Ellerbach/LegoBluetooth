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
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        public PortValueCombinedModeMessage(byte hubID, byte portID)
            : base(hubID, MessageType.PortValueCombinedMode)
        {
            // Section 3.22
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

            // We need to compute the proper length
            int index = data.Length > 127 ? 2 : 1;

            byte hubId = data[index++];
            // Skipping the message type
            index++;
            byte portID = data[index++];
            ushort bitPointer = BitConverter.ToUInt16(data, index);
            index += 2;

            var message = new PortValueCombinedModeMessage(hubId, portID)
            {
                Message = data,
                Length = (ushort)(data.Length > 127 ? data.Length - 1: data.Length),
            };

            // TODO This doesn't work, needs adjustements
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

                message.PortValues.Add(new PortValueEntryCombined
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
            int length = 1;

            data.Add(HubID);
            length++;
            data.Add((byte)MessageType);
            length++;
            data.Add(PortID);
            length++;

            foreach (PortValueEntryCombined portValue in PortValues)
            {
                byte[] bitPointerBytes = BitConverter.GetBytes(portValue.BitPointer);
                foreach (byte b in bitPointerBytes)
                {
                    data.Add(b);
                    length++;
                }

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

            // Now need to encode properly the length
            Length = (ushort)length;
            if (Length < 127)
            {
                data.Insert(0, (byte)Length);
            }
            else
            {
                data.Insert(0, (byte)((Length >> 8) | 0x80));
                data.Insert(1, (byte)(Length & 0xFF));
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
            foreach (PortValueEntryCombined portValue in PortValues)
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
