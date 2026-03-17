// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a 0x21 message to request port information and value.
    /// </summary>
    public class PortInformationRequestMessage : CommonMessageHeader
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
        /// Initializes a new instance of the <see cref="PortInformationRequestMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="informationType">The information type.</param>
        public PortInformationRequestMessage(byte hubID, byte portID, InformationType informationType)
            : base(hubID, MessageType.PortInformationRequest)
        {
            // 3.15
            PortID = portID;
            InformationType = informationType;
            Length = 5;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInformationRequestMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInformationRequestMessage"/> instance.</returns>
        public static new PortInformationRequestMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            byte portID = data[3];
            InformationType informationType = (InformationType)data[4];

            return new PortInformationRequestMessage(data[1], portID, informationType)
            {
                Message = data,
                Length = data[0]
            };
        }

        /// <summary>
        /// Serializes the PortInformationRequestMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInformationRequestMessage.</returns>
        public override byte[] ToByteArray()
        {
            Length = 5;
            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = PortID;
            data[index++] = (byte)InformationType;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port information request message.
        /// </summary>
        /// <returns>A string representation of the port information request message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, InformationType: {InformationType}";
        }
    }
}
