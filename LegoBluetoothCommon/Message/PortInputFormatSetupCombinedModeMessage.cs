// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message to set up a sensor which can be used with more combinations of Mode and DataSets (CombinedMode).
    /// </summary>
    public class PortInputFormatSetupCombinedModeMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the sub-command for Port Input Format Setup.
        /// </summary>
        public PortInputFormatSetupSubCommand SubCommand { get; set; }

        /// <summary>
        /// Gets or sets the combination index for the LPF2 device.
        /// </summary>
        public byte CombinationIndex { get; set; }

        /// <summary>
        /// Gets or sets the mode and dataset combinations.
        /// </summary>
        public byte[] ModeDataSetCombinations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInputFormatSetupCombinedModeMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="subCommand">The sub-command for Port Input Format Setup.</param>
        /// <param name="combinationIndex">The combination index for the LPF2 device.</param>
        /// <param name="modeDataSetCombinations">The mode and dataset combinations.</param>
        public PortInputFormatSetupCombinedModeMessage(ushort length, byte hubID, byte portID, PortInputFormatSetupSubCommand subCommand, byte combinationIndex, byte[] modeDataSetCombinations)
            : base(length, hubID, MessageType.PortInputFormatSetupCombinedMode)
        {
            PortID = portID;
            SubCommand = subCommand;
            CombinationIndex = combinationIndex;
            ModeDataSetCombinations = modeDataSetCombinations;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInputFormatSetupCombinedModeMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInputFormatSetupCombinedModeMessage"/> instance.</returns>
        public static new PortInputFormatSetupCombinedModeMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            byte portID = data[3];
            PortInputFormatSetupSubCommand subCommand = (PortInputFormatSetupSubCommand)data[4];

            if (subCommand == PortInputFormatSetupSubCommand.SetModeAndDataSetCombinations)
            {
                if (data.Length < 7)
                {
                    throw new ArgumentException("Invalid data array. Must contain at least 7 bytes for SetModeAndDataSetCombinations sub-command.", nameof(data));
                }

                byte combinationIndex = data[5];
                byte[] modeDataSetCombinations = new byte[data.Length - 6];
                Array.Copy(data, 6, modeDataSetCombinations, 0, modeDataSetCombinations.Length);

                return new PortInputFormatSetupCombinedModeMessage((ushort)data.Length, data[1], portID, subCommand, combinationIndex, modeDataSetCombinations)
                {
                    Message = data,
                };
            }
            else
            {
                return new PortInputFormatSetupCombinedModeMessage((ushort)data.Length, data[1], portID, subCommand, 0, null)
                {
                    Message = data,
                };
            }
        }

        /// <summary>
        /// Serializes the PortInputFormatSetupCombinedModeMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInputFormatSetupCombinedModeMessage.</returns>
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
            data[index++] = (byte)SubCommand;

            if (SubCommand == PortInputFormatSetupSubCommand.SetModeAndDataSetCombinations)
            {
                data[index++] = CombinationIndex;
                if (ModeDataSetCombinations != null)
                {
                    Array.Copy(ModeDataSetCombinations, 0, data, index, ModeDataSetCombinations.Length);
                }
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port input format setup (combined mode) message.
        /// </summary>
        /// <returns>A string representation of the port input format setup (combined mode) message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, SubCommand: {SubCommand}, CombinationIndex: {CombinationIndex}{(ModeDataSetCombinations == null ? string.Empty : $", ModeDataSetCombinations: {BitConverter.ToString(ModeDataSetCombinations)}")}";
        }
    }
}
