// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a 0x46 message that updates the host with values from a Combined Mode sensor.
    /// </summary>
    public class PortValueCombinedModeMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the combined Mode/Dataset bit-pointer indicating which mode/dataset combinations have values.
        /// </summary>
        public ushort BitPointer { get; set; }

        /// <summary>
        /// Gets or sets the list of port value entries.
        /// </summary>
        public ArrayList PortValues { get; set; }

        /// <summary>
        /// Gets or sets the Payload associated to this request as it requires to know the mode for a proper decoding.
        /// </summary>
        public byte[] Payload { get; internal set; }

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

            byte hubId = data[index++];
            // Skipping the message type
            index++;
            byte portID = data[index++];
            ushort bitPointer = BitConverter.ToUInt16(data, index);
            index += 2;

            var message = new PortValueCombinedModeMessage(hubId, portID)
            {
                Message = data,
                Length = length,
                BitPointer = bitPointer,
            };

            message.Payload = new byte[data.Length - index];
            Array.Copy(data, index, message.Payload, 0, message.Payload.Length);

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

            // Compute single combined bitmask from all entries
            ushort combinedBitPointer = 0;
            foreach (PortValueEntryCombined portValue in PortValues)
            {
                combinedBitPointer |= (ushort)(1 << portValue.BitPointer);
            }

            byte[] bitPointerBytes = BitConverter.GetBytes(combinedBitPointer);
            foreach (byte b in bitPointerBytes)
            {
                data.Add(b);
                length++;
            }

            // Write values sequentially (no per-entry bit-pointer)
            foreach (PortValueEntryCombined portValue in PortValues)
            {
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
        /// Provides a string representation of the port value (combined mode) message.
        /// </summary>
        /// <returns>A string representation of the port value (combined mode) message.</returns>
        public override string ToString()
        {
            string portValuesString = "";
            if (PortValues != null)
            {
                foreach (PortValueEntryCombined portValue in PortValues)
                {
                    if (portValuesString.Length > 0)
                    {
                        portValuesString += ", ";
                    }
                    portValuesString += $"BitPointer: {portValue.BitPointer}, InputValue: {portValue.InputValue}";
                }
            }

            if (Payload != null)
            {
                portValuesString += $", Payload: {BitConverter.ToString(Payload)}";
            }

            return $"{base.ToString()}, PortID: {PortID}, PortValues: [{portValuesString}]";
        }
    }
}
