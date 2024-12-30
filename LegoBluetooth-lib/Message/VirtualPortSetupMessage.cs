using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message used to control synchronization of Ports.
    /// </summary>
    public class VirtualPortSetupMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the sub-command for the virtual port setup.
        /// </summary>
        public byte SubCommand { get; set; }

        /// <summary>
        /// Gets or sets the Port ID for the virtual port (used when SubCommand is 0).
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the Port ID A for the first synchronizable port (used when SubCommand is 1).
        /// </summary>
        public byte PortIDA { get; set; }

        /// <summary>
        /// Gets or sets the Port ID B for the second synchronizable port (used when SubCommand is 1).
        /// </summary>
        public byte PortIDB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPortSetupMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="subCommand">The sub-command for the virtual port setup.</param>
        /// <param name="portID">The Port ID for the virtual port (used when SubCommand is 0).</param>
        /// <param name="portIDA">The Port ID A for the first synchronizable port (used when SubCommand is 1).</param>
        /// <param name="portIDB">The Port ID B for the second synchronizable port (used when SubCommand is 1).</param>
        public VirtualPortSetupMessage(ushort length, byte hubID, byte subCommand, byte portID = 0, byte portIDA = 0, byte portIDB = 0)
            : base(length, hubID, MessageType.VirtualPortSetup)
        {
            SubCommand = subCommand;
            PortID = portID;
            PortIDA = portIDA;
            PortIDB = portIDB;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="VirtualPortSetupMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="VirtualPortSetupMessage"/> instance.</returns>
        public static new VirtualPortSetupMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            byte subCommand = data[3];
            byte portID = 0;
            byte portIDA = 0;
            byte portIDB = 0;

            if (subCommand == 0x00 && data.Length >= 5)
            {
                portID = data[4];
            }
            else if (subCommand == 0x01 && data.Length >= 6)
            {
                portIDA = data[4];
                portIDB = data[5];
            }
            else
            {
                throw new ArgumentException("Invalid data array for the specified sub-command.", nameof(data));
            }

            return new VirtualPortSetupMessage((ushort)data.Length, data[1], subCommand, portID, portIDA, portIDB)
            {
                Message = data,
            };
        }

        /// <summary>
        /// Serializes the VirtualPortSetupMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the VirtualPortSetupMessage.</returns>
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
            data[index++] = SubCommand;

            if (SubCommand == 0x00)
            {
                data[index++] = PortID;
            }
            else if (SubCommand == 0x01)
            {
                data[index++] = PortIDA;
                data[index++] = PortIDB;
            }

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the virtual port setup message.
        /// </summary>
        /// <returns>A string representation of the virtual port setup message.</returns>
        public override string ToString()
        {
            if (SubCommand == 0x00)
            {
                return $"{base.ToString()}, SubCommand: {SubCommand}, PortID: {PortID}";
            }
            else if (SubCommand == 0x01)
            {
                return $"{base.ToString()}, SubCommand: {SubCommand}, PortIDA: {PortIDA}, PortIDB: {PortIDB}";
            }
            else
            {
                return $"{base.ToString()}, SubCommand: {SubCommand}";
            }
        }
    }
}
