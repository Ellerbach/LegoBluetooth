// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Text;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that returns specified Mode Information Types about the LPF2 device connected to the Port.
    /// </summary>
    public class PortModeInformationMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the mode of the Input.
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// Gets or sets the mode information type.
        /// </summary>
        public ModeInformationType ModeInformationType { get; set; }

        /// <summary>
        /// Gets or sets the name for Information Type == NAME.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw minimum value for Information Type == RAW.
        /// </summary>
        public float RawMin { get; set; }

        /// <summary>
        /// Gets or sets the raw maximum value for Information Type == RAW.
        /// </summary>
        public float RawMax { get; set; }

        /// <summary>
        /// Gets or sets the percent minimum value for Information Type == PCT.
        /// </summary>
        public float PctMin { get; set; }

        /// <summary>
        /// Gets or sets the percent maximum value for Information Type == PCT.
        /// </summary>
        public float PctMax { get; set; }

        /// <summary>
        /// Gets or sets the SI minimum value for Information Type == SI.
        /// </summary>
        public float SiMin { get; set; }

        /// <summary>
        /// Gets or sets the SI maximum value for Information Type == SI.
        /// </summary>
        public float SiMax { get; set; }

        /// <summary>
        /// Gets or sets the symbol for Information Type == SYMBOL.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the mapping for Information Type == MAPPING.
        /// </summary>
        public ushort Mapping { get; set; }

        /// <summary>
        /// Gets or sets the motor bias for Information Type == MOTOR BIAS.
        /// </summary>
        public byte MotorBias { get; set; }

        /// <summary>
        /// Gets or sets the capability bits for Information Type == CAPABILITY BITS.
        /// </summary>
        public byte[] CapabilityBits { get; set; }

        /// <summary>
        /// Gets or sets the value format for Information Type == VALUE FORMAT.
        /// </summary>
        public byte[] ValueFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortModeInformationMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="mode">The mode of the Input.</param>
        /// <param name="modeInformationType">The mode information type.</param>
        public PortModeInformationMessage(ushort length, byte hubID, byte portID, byte mode, ModeInformationType modeInformationType)
            : base(length, hubID, MessageType.PortModeInformation)
        {
            PortID = portID;
            Mode = mode;
            ModeInformationType = modeInformationType;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortModeInformationMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortModeInformationMessage"/> instance.</returns>
        public static new PortModeInformationMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 7)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 7 bytes.", nameof(data));
            }

            byte portID = data[3];
            byte mode = data[4];
            ModeInformationType modeInformationType = (ModeInformationType)data[5];

            var message = new PortModeInformationMessage((ushort)data.Length, data[1], portID, mode, modeInformationType)
            {
                Message = data,
            };

            switch (modeInformationType)
            {
                case ModeInformationType.Name:
                    message.Name = Encoding.UTF8.GetString(data, 6, data.Length - 6);
                    break;
                case ModeInformationType.Raw:
                    message.RawMin = BitConverter.ToSingle(data, 6);
                    message.RawMax = BitConverter.ToSingle(data, 10);
                    break;
                case ModeInformationType.Percent:
                    message.PctMin = BitConverter.ToSingle(data, 6);
                    message.PctMax = BitConverter.ToSingle(data, 10);
                    break;
                case ModeInformationType.SI:
                    message.SiMin = BitConverter.ToSingle(data, 6);
                    message.SiMax = BitConverter.ToSingle(data, 10);
                    break;
                case ModeInformationType.Symbol:
                    message.Symbol = Encoding.UTF8.GetString(data, 6, data.Length - 6);
                    break;
                case ModeInformationType.Mapping:
                    message.Mapping = BitConverter.ToUInt16(data, 6);
                    break;
                case ModeInformationType.MotorBias:
                    message.MotorBias = data[6];
                    break;
                case ModeInformationType.CapabilityBits:
                    message.CapabilityBits = new byte[6];
                    Array.Copy(data, 6, message.CapabilityBits, 0, 6);
                    break;
                case ModeInformationType.ValueFormat:
                    message.ValueFormat = new byte[4];
                    Array.Copy(data, 6, message.ValueFormat, 0, 4);
                    break;
                default:
                    throw new ArgumentException("Invalid mode information type.", nameof(data));
            }

            return message;
        }

        /// <summary>
        /// Serializes the PortModeInformationMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortModeInformationMessage.</returns>
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
            data[index++] = PortID;
            data[index++] = Mode;
            data[index++] = (byte)ModeInformationType;

            switch (ModeInformationType)
            {
                case ModeInformationType.Name:
                    Array.Copy(Encoding.UTF8.GetBytes(Name), 0, data, index, Name.Length);
                    break;
                case ModeInformationType.Raw:
                    Array.Copy(BitConverter.GetBytes(RawMin), 0, data, index, 4);
                    index += 4;
                    Array.Copy(BitConverter.GetBytes(RawMax), 0, data, index, 4);
                    break;
                case ModeInformationType.Percent:
                    Array.Copy(BitConverter.GetBytes(PctMin), 0, data, index, 4);
                    index += 4;
                    Array.Copy(BitConverter.GetBytes(PctMax), 0, data, index, 4);
                    break;
                case ModeInformationType.SI:
                    Array.Copy(BitConverter.GetBytes(SiMin), 0, data, index, 4);
                    index += 4;
                    Array.Copy(BitConverter.GetBytes(SiMax), 0, data, index, 4);
                    break;
                case ModeInformationType.Symbol:
                    Array.Copy(Encoding.UTF8.GetBytes(Symbol), 0, data, index, Symbol.Length);
                    break;
                case ModeInformationType.Mapping:
                    Array.Copy(BitConverter.GetBytes(Mapping), 0, data, index, 2);
                    break;
                case ModeInformationType.MotorBias:
                    data[index] = MotorBias;
                    break;
                case ModeInformationType.CapabilityBits:
                    Array.Copy(CapabilityBits, 0, data, index, 6);
                    break;
                case ModeInformationType.ValueFormat:
                    Array.Copy(ValueFormat, 0, data, index, 4);
                    break;
                default:
                    throw new ArgumentException("Invalid mode information type.", nameof(ModeInformationType));
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port mode information message.
        /// </summary>
        /// <returns>A string representation of the port mode information message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, Mode: {Mode}, ModeInformationType: {ModeInformationType}";
        }
    }
}
