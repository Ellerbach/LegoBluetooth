// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth.Device
{
    /// <summary>
    /// Represents a base device in a LEGO Bluetooth system.
    /// </summary>
    public class BaseDevice
    {
        private readonly IBluetooth _ble;

        /// <summary>
        /// Gets or sets the ID of the port.
        /// </summary>
        public byte PortID { get; internal set; }

        /// <summary>
        /// Gets or sets the ID of the hub.
        /// </summary>
        public byte HubID { get; internal set; }

        /// <summary>
        /// Gets or sets the input modes.
        /// </summary>
        public ushort InputModes { get; set; }

        /// <summary>
        /// Gets or sets the output modes.
        /// </summary>
        public ushort OutpoutModes { get; set; }

        /// <summary>
        /// Gets or sets the port capabilities.
        /// </summary>
        public Capabilities PortCapabilities { get; set; }

        /// <summary>
        /// Gets or sets the list of port modes.
        /// </summary>
        public ArrayList Modes { get; internal set; } = new ArrayList();

        /// <summary>
        /// Gets or sets the list of possible mode combinations.
        /// </summary>
        public ArrayList ModeCombinations { get; internal set; } = new ArrayList();

        /// <summary>
        /// Gets or sets the hardware versions.
        /// </summary>
        public Version HWVersion { get; set; } = new Version(0, 0);

        /// <summary>
        /// Gets or sets the software versions.
        /// </summary>
        public Version SWVersion { get; set; } = new Version(0, 0);

        /// <summary>
        /// Gets or sets the device type.
        /// </summary>
        public IOTypeID DeviceType { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the device is virtual.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the port ID A for the virtual device.
        /// </summary>
        public byte PortIdA { get; set; }

        /// <summary>
        /// Gets or sets the port ID B for the virtual device.
        /// </summary>
        public byte PortIdB { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDevice"/> class.
        /// </summary>
        /// <param name="ble">The Bluetooth interface.</param>
        /// <param name="hubID">The ID of the hub.</param>
        /// <param name="portID">The ID of the port.</param>
        /// <exception cref="ArgumentNullException">Thrown when the Bluetooth interface is null.</exception>
        public BaseDevice(IBluetooth ble, byte hubID, byte portID)
        {
            _ble = ble ?? throw new ArgumentNullException();
            HubID = hubID;
            PortID = portID;
        }

        /// <summary>
        /// Gets the port mode details for the specified port and mode.
        /// </summary>
        /// <param name="portId">The ID of the port.</param>
        /// <param name="mode">The index of the mode.</param>
        /// <returns>The <see cref="PortModeDetails"/> for the specified port and mode, or null if not found.</returns>
        public PortModeDetails GetPortModeDetails(byte portId, byte mode)
        {
            foreach (PortModeDetails portMode in Modes)
            {
                if ((portMode.PortID == portId) && (portMode.ModeIndex == mode))
                {
                    return portMode;
                }
            }

            return null;
        }

        /// <summary>
        /// Populates the modes based on the provided message.
        /// </summary>
        /// <param name="msg">The message containing mode information.</param>
        public void PopulateModes(CommonMessageHeader msg)
        {
            if (msg.MessageType == MessageType.PortModeInformation)
            {
                PortModeInformationMessage info = (PortModeInformationMessage)msg;
                var mode = GetPortModeDetails(info.PortID, info.Mode);
                if (mode != null)
                {
                    mode.PopulateFromMessage(info);
                }
            }
            else if (msg.MessageType == MessageType.PortInformation)
            {
                PortInformationMessage info = (PortInformationMessage)msg;
                if (info.InformationType == InformationType.ModeInfo)
                {
                    if (Modes.Count == 0)
                    {
                        for (int i = 0; i < info.TotalModeCount; i++)
                        {
                            var mode = new PortModeDetails()
                            {
                                PortID = info.PortID,
                                HubID = info.HubID,
                                ModeIndex = (byte)i,
                                IsInput = ((info.InputModes >> i) & 1) == 1,
                                IsOutput = ((info.OutputModes >> i) & 1) == 1,
                            };
                            Modes.Add(mode);
                        }

                        InputModes = info.InputModes;
                        OutpoutModes = info.OutputModes;
                        PortCapabilities = info.Capabilities;
                    }
                }
                else if (info.InformationType == InformationType.PossibleModeCombinations)
                {
                    for (int i = 0; i < info.ModeCombinations.Length; i++)
                    {
                        ModeCombinations.Add(info.ModeCombinations[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Populates the modes based on the default configuration.
        /// </summary>
        public void HydrateModes()
        {
            var conf = GetDefaultConfiguration();
            if (conf != null)
            {
                foreach (byte[] item in conf)
                {
                    PopulateModes(CommonMessageHeader.Decode(item));
                }

                conf.Clear();

                // This is going to clean the memory
                GC.SuppressFinalize(conf);                
            }
        }

        /// <summary>
        /// Gets the port mode details for the device.
        /// </summary>
        /// <returns>And array list of byte array containing all the mode details.</returns>
        public ArrayList GetPortInformations()
        {
            ArrayList list = new ArrayList();
            foreach (PortModeDetails mode in Modes)
            {
                // Serialize each mode
                foreach (byte[] elems in mode.CreateByteArrays())
                {
                    list.Add(elems);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets the default port mode information for the device on form of a byte array array list.
        /// </summary>
        /// <remarks>This is memory intensive, please clean and release the buffer once you are done.</remarks>
        /// <returns>An array list containing all the mode details serialized like in the protocol.</returns>
        public virtual ArrayList GetDefaultConfiguration()
        {
            return null;
        }

        /// <summary>
        /// Sets the default configuration for the hub ID and port ID to the configuration Array.
        /// </summary>
        /// <param name="defaultConfig">The array list of byte array to adjust.</param>
        /// <returns>The adjusted configuration.</returns>
        internal ArrayList SetDefaultConfiguration(ArrayList defaultConfig)
        {
            // Populate the default configuration with the proper hubid and portid
            for (int i = 0; i < defaultConfig.Count; i++)
            {
                byte[] config = (byte[])defaultConfig[i];
                config[1] = HubID;
                config[3] = PortID;
            }

            return defaultConfig;
        }
    }
}
