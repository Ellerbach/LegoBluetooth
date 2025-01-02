// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the common message header for all messages in the LEGO Hub Characteristic (UUID: 1624).
    /// </summary>
    public class CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the length of the entire message in bytes.
        /// </summary>
        /// <remarks>
        /// Length of entire message in bytes (escaped and 2 bytes for long messages).
        /// For further information about encoding, see Message Length Encoding.
        /// </remarks>
        public ushort Length { get; internal set; }

        /// <summary>
        /// Gets or sets the Hub ID.
        /// </summary>
        /// <remarks>
        /// NOT USED at the moment! Always set to 0x00 (zero).
        /// Hub identifier is needed when a Hub is connected as a bridge to a network formerly built without a SMART DEVICE.
        /// </remarks>
        public byte HubID { get; set; } = 0x00;

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        /// <remarks>
        /// Identifies the kind of message transmitted. For message types, see Message Types.
        /// </remarks>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the payload of the message.
        /// </summary>
        public byte[] Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonMessageHeader"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="messageType">The message type.</param>
        public CommonMessageHeader(ushort length, byte hubID, MessageType messageType)
        {
            Length = length;
            HubID = hubID;
            MessageType = messageType;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="CommonMessageHeader"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="CommonMessageHeader"/> instance.</returns>
        public static CommonMessageHeader Decode(byte[] data)
        {
            if (data == null || data.Length < 3)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 3 bytes.", nameof(data));
            }

            ushort length;
            int index = 0;

            if ((data[0] & 0x80) == 0x80)
            {
                if (data.Length < 4)
                {
                    throw new ArgumentException("Invalid data array. Must contain at least 4 bytes for extended length encoding.", nameof(data));
                }
                length = (ushort)(((data[0] & 0x7F) << 8) | data[1]);
                index = 2;
            }
            else
            {
                length = data[0];
                index = 1;
            }

            byte hubID = data[index];
            MessageType messageType = (MessageType)data[index + 1];

            switch (messageType)
            {
                case MessageType.HubProperties:
                    return HubPropertyMessage.Decode(data);
                case MessageType.HubActions:
                    return HubActionMessage.Decode(data);
                case MessageType.HubAlerts:
                    return HubAlertMessage.Decode(data);
                case MessageType.HubAttachedIO:
                    return HubAttachedIOMessage.Decode(data);
                case MessageType.GenericErrorMessages:
                    return ErrorMessage.Decode(data);
                case MessageType.HWNetworkCommands:
                    return HWNetworkMessage.Decode(data);
                case MessageType.FWUpdateGoIntoBootMode:
                    return GoIntoBootModeMessage.Decode(data);
                case MessageType.PortInformationRequest:
                    return PortInformationRequestMessage.Decode(data);
                case MessageType.PortModeInformationRequest:
                    return PortModeInformationRequestMessage.Decode(data);
                case MessageType.PortInputFormatSetupSingle:
                    return PortInputFormatSetupSingleMessage.Decode(data);
                case MessageType.PortInputFormatSetupCombinedMode:
                    return PortInputFormatSetupCombinedModeMessage.Decode(data);
                case MessageType.PortInformation:
                    return PortInformationMessage.Decode(data);
                case MessageType.PortModeInformation:
                    return PortModeInformationMessage.Decode(data);
                case MessageType.PortValueSingle:
                    return PortValueSingleMessage.Decode(data);
                case MessageType.PortValueCombinedMode:
                    return PortValueCombinedModeMessage.Decode(data);
                case MessageType.PortInputFormatSingle:
                    return PortInputFormatSingleMessage.Decode(data);
                case MessageType.PortInputFormatCombinedMode:
                    return PortInputFormatCombinedModeMessage.Decode(data);
                case MessageType.VirtualPortSetup:
                    return VirtualPortSetupMessage.Decode(data);
                case MessageType.PortOutputCommand:
                    return PortOutputCommandMessage.Decode(data);
                case MessageType.PortOutputCommandFeedback:
                    return PortOutputCommandFeedbackMessage.Decode(data);
                case MessageType.FWUpdateLockStatusRequest:
                case MessageType.FWUpdateLockMemory:
                case MessageType.FWLockStatus:
                default:
                    return new CommonMessageHeader(length, hubID, (MessageType)messageType)
                    {
                        Message = data,
                    };
            }
        }

        /// <summary>
        /// Serialize as a byte array.
        /// </summary>
        /// <returns>A byte array.</returns>
        public virtual byte[] ToByteArray() => Message;

        /// <summary>
        /// Provides a string representation of the common message header.
        /// </summary>
        /// <returns>A string representation of the common message header.</returns>
        public override string ToString()
        {
            return $"Length: {Length}, HubID: {HubID}, MessageType: {MessageType}";
        }
    }
}


