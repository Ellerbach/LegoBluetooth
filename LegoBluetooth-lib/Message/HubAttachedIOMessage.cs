using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message for subscribing to Hub alerts and requesting the current state of these alerts.
    /// </summary>
    public class HubAttachedIOMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the Port ID associated with the Attached I/O assigned by the Hub.
        /// </summary>
        /// <remarks>
        /// Port ID
        /// ID range    Description
        /// 000 - 049	Hub connector(0, 1, 2 .. 49)[##]
        /// 050 - 100   Internal
        /// 101 - 255   Reserved
        /// </remarks>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the I/O event.
        /// </summary>
        public IOEvent Event { get; set; }

        /// <summary>
        /// Gets or sets the I/O Type ID.
        /// </summary>
        public IOTypeID IOTypeID { get; set; }

        /// <summary>
        /// Gets or sets the hardware revision of the I/O.
        /// </summary>
        public int HardwareRevision { get; set; }

        /// <summary>
        /// Gets or sets the software revision of the I/O.
        /// </summary>
        public int SoftwareRevision { get; set; }

        /// <summary>
        /// Gets or sets the first Port ID forming the Virtual I/O.
        /// </summary>
        public byte PortIDA { get; set; }

        /// <summary>
        /// Gets or sets the second Port ID forming the Virtual I/O.
        /// </summary>
        public byte PortIDB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubAttachedIOMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="portID">The Port ID.</param>
        /// <param name="event">The I/O event.</param>
        /// <param name="ioTypeID">The I/O Type ID.</param>
        /// <param name="hardwareRevision">The hardware revision.</param>
        /// <param name="softwareRevision">The software revision.</param>
        /// <param name="portIDA">The first Port ID forming the Virtual I/O.</param>
        /// <param name="portIDB">The second Port ID forming the Virtual I/O.</param>
        public HubAttachedIOMessage(ushort length, byte hubID, byte portID, IOEvent @event, IOTypeID ioTypeID, int hardwareRevision, int softwareRevision, byte portIDA, byte portIDB)
            : base(length, hubID, MessageType.HubAttachedIO)
        {
            PortID = portID;
            Event = @event;
            IOTypeID = ioTypeID;
            HardwareRevision = hardwareRevision;
            SoftwareRevision = softwareRevision;
            PortIDA = portIDA;
            PortIDB = portIDB;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="HubAttachedIOMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="HubAttachedIOMessage"/> instance.</returns>
        public static new HubAttachedIOMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            var commonHeader = CommonMessageHeader.Decode(data);
            byte portID = data[3];
            IOEvent @event = (IOEvent)data[4];

            if (@event == IOEvent.DetachedIO)
            {
                return new HubAttachedIOMessage(commonHeader.Length, commonHeader.HubID, portID, @event, 0, 0, 0, 0, 0);
            }
            else if (@event == IOEvent.AttachedIO)
            {
                if (data.Length < 15)
                {
                    throw new ArgumentException("Invalid data array. Must contain at least 15 bytes for Attached I/O.", nameof(data));
                }

                IOTypeID ioTypeID = (IOTypeID)BitConverter.ToUInt16(data, 5);
                int hardwareRevision = BitConverter.ToInt32(data, 7);
                int softwareRevision = BitConverter.ToInt32(data, 11);

                return new HubAttachedIOMessage(commonHeader.Length, commonHeader.HubID, portID, @event, ioTypeID, hardwareRevision, softwareRevision, 0, 0);
            }
            else if (@event == IOEvent.AttachedVirtualIO)
            {
                if (data.Length < 9)
                {
                    throw new ArgumentException("Invalid data array. Must contain at least 9 bytes for Attached Virtual I/O.", nameof(data));
                }

                IOTypeID ioTypeID = (IOTypeID)BitConverter.ToUInt16(data, 5);
                byte portIDA = data[7];
                byte portIDB = data[8];

                return new HubAttachedIOMessage(commonHeader.Length, commonHeader.HubID, portID, @event, ioTypeID, 0, 0, portIDA, portIDB);
            }
            else
            {
                throw new ArgumentException("Invalid I/O event type.", nameof(data));
            }
        }

        /// <summary>
        /// Serializes the HubAttachedIOMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubAttachedIOMessage.</returns>
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
            data[index++] = (byte)Event;
            if (Event == IOEvent.AttachedIO)
            {
                Array.Copy(BitConverter.GetBytes((ushort)IOTypeID), 0, data, index, 2);
                index += 2;
                Array.Copy(BitConverter.GetBytes(HardwareRevision), 0, data, index, 4);
                index += 4;
                Array.Copy(BitConverter.GetBytes(SoftwareRevision), 0, data, index, 4);
                index += 4;
            }
            else if (Event == IOEvent.AttachedVirtualIO)
            {
                Array.Copy(BitConverter.GetBytes((ushort)IOTypeID), 0, data, index, 2);
                index += 2;
                data[index++] = PortIDA;
                data[index++] = PortIDB;
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the hub attached I/O message.
        /// </summary>
        /// <returns>A string representation of the hub attached I/O message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, PortID: {PortID}, Event: {Event}, IOTypeID: {IOTypeID}, HardwareRevision: {HardwareRevision}, SoftwareRevision: {SoftwareRevision}, PortIDA: {PortIDA}, PortIDB: {PortIDB}";
        }
    }
}
