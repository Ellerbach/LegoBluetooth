// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the details of a port mode in a LEGO Bluetooth device.
    /// </summary>
    public class PortModeDetails
    {
        // See section 2.20 of the LEGO Wireless Protocol Document
        /// <summary>
        /// Gets or sets the ID of the hub.
        /// </summary>
        public byte HubID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the port.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the index of the mode.
        /// </summary>
        public byte ModeIndex { get; set; }

        /// <summary>
        /// Gets or sets the name of the mode.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the symbol representing the mode.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the mode is an input.
        /// </summary>
        public bool IsInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mode is an output.
        /// </summary>
        public bool IsOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mode is virtual.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal places for the mode's values.
        /// </summary>
        public byte Decimals { get; set; }

        /// <summary>
        /// Gets or sets the minimum raw value for the mode.
        /// </summary>
        public float RawMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum raw value for the mode.
        /// </summary>
        public float RawMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum percentage value for the mode.
        /// </summary>
        public float PctMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum percentage value for the mode.
        /// </summary>
        public float PctMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum SI value for the mode.
        /// </summary>
        public float SiMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum SI value for the mode.
        /// </summary>
        public float SiMax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input supports null.
        /// </summary>
        public bool InputSupportsNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input supports functional mapping 2.0.
        /// </summary>
        public bool InputSupportFunctionalMapping20 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input is absolute.
        /// </summary>
        public bool InputAbsolute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input is relative.
        /// </summary>
        public bool InputRelative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input is discrete.
        /// </summary>
        public bool InputDiscrete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output supports null.
        /// </summary>
        public bool OutputSupportsNull { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output supports functional mapping 2.0.
        /// </summary>
        public bool OutputSupportFunctionalMapping20 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is absolute.
        /// </summary>
        public bool OutputAbsolute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is relative.
        /// </summary>
        public bool OutputRelative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is discrete.
        /// </summary>
        public bool OutputDiscrete { get; set; }

        /// <summary>
        /// Gets or sets the motor bias.
        /// </summary>
        public byte MotorBias { get; set; }

        /// <summary>
        /// Gets or sets the vamue format.
        /// </summary>
        public ValueFormat ValueFormat { get; set; }

        public void PopulateFromMessage(PortModeInformationMessage msgPortInfo)
        {

            // We don't do anything if not the right port ID or hub ID or ModeID
            if (msgPortInfo.HubID != HubID || msgPortInfo.PortID != PortID || msgPortInfo.Mode != ModeIndex)
            {
                return;
            }

            switch (msgPortInfo.ModeInformationType)
            {
                case ModeInformationType.Name:
                    Name = msgPortInfo.Name;
                    break;
                case ModeInformationType.Raw:
                    RawMax = msgPortInfo.RawMax;
                    RawMin = msgPortInfo.RawMin;
                    break;
                case ModeInformationType.Percent:
                    PctMax = msgPortInfo.PctMax;
                    PctMin = msgPortInfo.PctMin;
                    break;
                case ModeInformationType.SI:
                    SiMax = msgPortInfo.SiMax;
                    SiMin = msgPortInfo.SiMin;
                    break;
                case ModeInformationType.Symbol:
                    Symbol = msgPortInfo.Symbol;
                    break;
                case ModeInformationType.Mapping:
                    // Information Type == MAPPING (005)
                    // Mapping
                    // 
                    // xxxx xxxx  INPUT SIDE
                    // Bit 7    Supports NULL value
                    // Bit 6    Supports Functional Mapping 2.0+
                    // Bit 5    N/A
                    // Bit 4    ABS (Absolute [min..max])
                    // Bit 3    REL (Relative [-1..1])
                    // Bit 2    DIS (Discrete [0, 1, 2, 3])
                    // Bit 1    N/A
                    // Bit 0    N/A
                    // 
                    // yyyy yyyy  OUTPUT SIDE
                    // Bit 7    Supports NULL value
                    // Bit 6    Supports Functional Mapping 2.0+
                    // Bit 5    N/A
                    // Bit 4    ABS (Absolute [min..max])
                    // Bit 3    REL (Relative [-1..1])
                    // Bit 2    DIS (Discrete [0, 1, 2, 3])
                    // Bit 1    N/A
                    // Bit 0    N/A
                    // 
                    // The roles are: The host of the sensor (even a simple and dumb black box) can then decide what to do with the sensor without any setup (default mode 0 (zero)). Using the LSB first (highest priority).
                    // 
                    // Uint16 xxxx xxxx yyyy yyyy
                    // Decode input side
                    InputSupportsNull = ((msgPortInfo.Mapping >> 15) & 1) == 1;
                    InputSupportFunctionalMapping20 = ((msgPortInfo.Mapping >> 14) & 1) == 1;
                    InputAbsolute = ((msgPortInfo.Mapping >> 12) & 1) == 1;
                    InputRelative = ((msgPortInfo.Mapping >> 11) & 1) == 1;
                    InputDiscrete = ((msgPortInfo.Mapping >> 10) & 1) == 1;

                    // Decode output side
                    OutputSupportsNull = ((msgPortInfo.Mapping >> 7) & 1) == 1;
                    OutputSupportFunctionalMapping20 = ((msgPortInfo.Mapping >> 6) & 1) == 1;
                    OutputAbsolute = ((msgPortInfo.Mapping >> 4) & 1) == 1;
                    OutputRelative = ((msgPortInfo.Mapping >> 3) & 1) == 1;
                    OutputDiscrete = ((msgPortInfo.Mapping >> 2) & 1) == 1;

                    break;
                case ModeInformationType.MotorBias:
                    MotorBias = msgPortInfo.MotorBias;
                    break;
                case ModeInformationType.CapabilityBits:
                    // TBD
                    break;
                case ModeInformationType.ValueFormat:
                    ValueFormat = ValueFormat.Decode(msgPortInfo.ValueFormat);
                    break;
                default:
                    throw new ArgumentException("No valid fomat");
                    break;
            }
        }

        public ArrayList CreateByteArrays(ModeInformationType[] modes = null)
        {
            var byteArrayList = new ArrayList();

            if (modes == null)
            {
                modes = new ModeInformationType[]
                {
                    ModeInformationType.Name,
                    ModeInformationType.Raw,
                    ModeInformationType.Percent,
                    ModeInformationType.SI,
                    ModeInformationType.Symbol,
                    ModeInformationType.Mapping,
                    ModeInformationType.MotorBias,
                    ModeInformationType.ValueFormat
                };
            }

            foreach (ModeInformationType mode in modes)
            {
                switch (mode)
                {
                    case ModeInformationType.Name:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.Name)
                        {
                            Name = Name
                        }.ToByteArray());
                        break;
                    case ModeInformationType.Raw:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.Raw)
                        {
                            RawMin = RawMin,
                            RawMax = RawMax
                        }.ToByteArray());
                        break;
                    case ModeInformationType.Percent:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.Percent)
                        {
                            PctMin = PctMin,
                            PctMax = PctMax
                        }.ToByteArray());
                        break;
                    case ModeInformationType.SI:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.SI)
                        {
                            SiMin = SiMin,
                            SiMax = SiMax
                        }.ToByteArray());
                        break;
                    case ModeInformationType.Symbol:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.Symbol)
                        {
                            Symbol = Symbol
                        }.ToByteArray());
                        break;
                    case ModeInformationType.Mapping:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.Mapping)
                        {
                            Mapping = (ushort)(
                                (InputSupportsNull ? 1 << 15 : 0) |
                                (InputSupportFunctionalMapping20 ? 1 << 14 : 0) |
                                (InputAbsolute ? 1 << 12 : 0) |
                                (InputRelative ? 1 << 11 : 0) |
                                (InputDiscrete ? 1 << 10 : 0) |
                                (OutputSupportsNull ? 1 << 7 : 0) |
                                (OutputSupportFunctionalMapping20 ? 1 << 6 : 0) |
                                (OutputAbsolute ? 1 << 4 : 0) |
                                (OutputRelative ? 1 << 3 : 0) |
                                (OutputDiscrete ? 1 << 2 : 0))
                        }.ToByteArray());
                        break;
                    case ModeInformationType.MotorBias:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.MotorBias)
                        {
                            MotorBias = MotorBias
                        }.ToByteArray());
                        break;
                    case ModeInformationType.ValueFormat:
                        byteArrayList.Add(new PortModeInformationMessage(HubID, PortID, ModeIndex, ModeInformationType.ValueFormat)
                        {
                            ValueFormat = ValueFormat.Encode()
                        }.ToByteArray());
                        break;
                }
            }

            return byteArrayList;
        }
    }
}
