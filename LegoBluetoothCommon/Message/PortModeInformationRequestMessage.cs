// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message to request information about the different modes of the LPF2 Device connected to the port.
    /// </summary>
    public class PortModeInformationRequestMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the mode to get information for.
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// Gets or sets the mode information type.
        /// </summary>
        public ModeInformationType InformationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortModeInformationRequestMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="mode">The mode to get information for.</param>
        /// <param name="informationType">The mode information type.</param>
        public PortModeInformationRequestMessage(byte hubID, byte portID, byte mode, ModeInformationType informationType)
            : base(hubID, MessageType.PortModeInformationRequest)
        {
            // Section 3.16
            PortID = portID;
            Mode = mode;
            InformationType = informationType;
            Length = 6;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortModeInformationRequestMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortModeInformationRequestMessage"/> instance.</returns>
        public static new PortModeInformationRequestMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 6)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 6 bytes.", nameof(data));
            }

            byte portID = data[3];
            byte mode = data[4];
            ModeInformationType informationType = (ModeInformationType)data[5];

            return new PortModeInformationRequestMessage(data[1], portID, mode, informationType)
            {
                Message = data,
                Length = data[0],
            };
        }

        /// <summary>
        /// Serializes the PortModeInformationRequestMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortModeInformationRequestMessage.</returns>
        public override byte[] ToByteArray()
        {
            Length = 6;

            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = PortID;
            data[index++] = Mode;
            data[index++] = (byte)InformationType;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port mode information request message.
        /// </summary>
        /// <returns>A string representation of the port mode information request message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, Mode: {Mode}, InformationType: {InformationType}";
        }
    }
}
