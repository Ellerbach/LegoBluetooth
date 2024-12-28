using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that returns the specified information about the LPF2 Device connected to the Port.
    /// </summary>
    public class PortInformationMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the information type.
        /// </summary>
        public InformationType InformationType { get; set; }

        /// <summary>
        /// Gets or sets the capabilities for MODE INFO.
        /// </summary>
        public byte Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the total mode count for MODE INFO.
        /// </summary>
        public byte TotalModeCount { get; set; }

        /// <summary>
        /// Gets or sets the available input port modes for MODE INFO.
        /// </summary>
        public ushort InputModes { get; set; }

        /// <summary>
        /// Gets or sets the available output port modes for MODE INFO.
        /// </summary>
        public ushort OutputModes { get; set; }

        /// <summary>
        /// Gets or sets the mode combinations for POSSIBLE MODE COMBINATIONS.
        /// </summary>
        public ushort[] ModeCombinations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInformationMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="informationType">The information type.</param>
        /// <param name="capabilities">The capabilities for MODE INFO.</param>
        /// <param name="totalModeCount">The total mode count for MODE INFO.</param>
        /// <param name="inputModes">The available input port modes for MODE INFO.</param>
        /// <param name="outputModes">The available output port modes for MODE INFO.</param>
        /// <param name="modeCombinations">The mode combinations for POSSIBLE MODE COMBINATIONS.</param>
        public PortInformationMessage(ushort length, byte hubID, byte portID, InformationType informationType, byte capabilities, byte totalModeCount, ushort inputModes, ushort outputModes, ushort[] modeCombinations)
            : base(length, hubID, MessageType.PortInformation)
        {
            PortID = portID;
            InformationType = informationType;
            Capabilities = capabilities;
            TotalModeCount = totalModeCount;
            InputModes = inputModes;
            OutputModes = outputModes;
            ModeCombinations = modeCombinations;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInformationMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInformationMessage"/> instance.</returns>
        public static new PortInformationMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 11)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 11 bytes.", nameof(data));
            }

            var commonHeader = CommonMessageHeader.Decode(data);
            byte portID = data[3];
            InformationType informationType = (InformationType)data[4];

            if (informationType == InformationType.ModeInfo)
            {
                byte capabilities = data[5];
                byte totalModeCount = data[6];
                ushort inputModes = BitConverter.ToUInt16(data, 7);
                ushort outputModes = BitConverter.ToUInt16(data, 9);

                return new PortInformationMessage(commonHeader.Length, commonHeader.HubID, portID, informationType, capabilities, totalModeCount, inputModes, outputModes, null)
                {
                    Message = commonHeader.Message,
                };
            }
            else if (informationType == InformationType.PossibleModeCombinations)
            {
                int combinationCount = (data.Length - 5) / 2;
                ushort[] modeCombinations = new ushort[combinationCount];
                for (int i = 0; i < combinationCount; i++)
                {
                    modeCombinations[i] = BitConverter.ToUInt16(data, 5 + i * 2);
                }

                return new PortInformationMessage(commonHeader.Length, commonHeader.HubID, portID, informationType, 0, 0, 0, 0, modeCombinations)
                {
                    Message = commonHeader.Message,
                };
            }
            else
            {
                throw new ArgumentException("Invalid information type.", nameof(data));
            }
        }

        /// <summary>
        /// Serializes the PortInformationMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInformationMessage.</returns>
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
            data[index++] = (byte)InformationType;

            if (InformationType == InformationType.ModeInfo)
            {
                data[index++] = Capabilities;
                data[index++] = TotalModeCount;
                Array.Copy(BitConverter.GetBytes(InputModes), 0, data, index, 2);
                index += 2;
                Array.Copy(BitConverter.GetBytes(OutputModes), 0, data, index, 2);
                index += 2;
            }
            else if (InformationType == InformationType.PossibleModeCombinations)
            {
                if (ModeCombinations != null)
                {
                    for (int i = 0; i < ModeCombinations.Length; i++)
                    {
                        Array.Copy(BitConverter.GetBytes(ModeCombinations[i]), 0, data, index, 2);
                        index += 2;
                    }
                }
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port information message.
        /// </summary>
        /// <returns>A string representation of the port information message.</returns>
        public override string ToString()
        {
            string ret = $"{base.ToString()}, PortID: {PortID}, InformationType: {InformationType}, Capabilities: {Capabilities}, TotalModeCount: {TotalModeCount}, InputModes: {InputModes}, OutputModes: {OutputModes}, ModeCombinations: ";
            for (int i = 0; i < ModeCombinations.Length; i++)
            {
                ret += $"{ModeCombinations[i]} ";
            }

            return ret;

        }
    }
}
