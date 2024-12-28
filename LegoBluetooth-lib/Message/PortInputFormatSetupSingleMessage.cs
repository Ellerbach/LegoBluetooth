using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message to set up the mode of operation for the LPF2 Device connected to the addressed Port.
    /// </summary>
    public class PortInputFormatSetupSingleMessage : CommonMessageHeader
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
        /// Gets or sets the delta interval to trigger a value update.
        /// </summary>
        public uint DeltaInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notification of value changes is enabled.
        /// </summary>
        public bool NotificationEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortInputFormatSetupSingleMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="mode">The mode to get information for.</param>
        /// <param name="deltaInterval">The delta interval to trigger a value update.</param>
        /// <param name="notificationEnabled">A value indicating whether notification of value changes is enabled.</param>
        public PortInputFormatSetupSingleMessage(ushort length, byte hubID, byte portID, byte mode, uint deltaInterval, bool notificationEnabled)
            : base(length, hubID, MessageType.PortInputFormatSetupSingle)
        {
            PortID = portID;
            Mode = mode;
            DeltaInterval = deltaInterval;
            NotificationEnabled = notificationEnabled;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortInputFormatSetupSingleMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortInputFormatSetupSingleMessage"/> instance.</returns>
        public static new PortInputFormatSetupSingleMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 10)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 10 bytes.", nameof(data));
            }

            var commonHeader = CommonMessageHeader.Decode(data);
            byte portID = data[3];
            byte mode = data[4];
            uint deltaInterval = BitConverter.ToUInt32(data, 5);
            bool notificationEnabled = data[9] == 1;

            return new PortInputFormatSetupSingleMessage(commonHeader.Length, commonHeader.HubID, portID, mode, deltaInterval, notificationEnabled)
            {
                Message = commonHeader.Message,
            };
        }

        /// <summary>
        /// Serializes the PortInputFormatSetupSingleMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortInputFormatSetupSingleMessage.</returns>
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
            data[index++] = Mode;
            Array.Copy(BitConverter.GetBytes(DeltaInterval), 0, data, index, 4);
            index += 4;
            data[index++] = (byte)(NotificationEnabled ? 1 : 0);

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the port input format setup (single) message.
        /// </summary>
        /// <returns>A string representation of the port input format setup (single) message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, Mode: {Mode}, DeltaInterval: {DeltaInterval}, NotificationEnabled: {NotificationEnabled}";
        }
    }
}
