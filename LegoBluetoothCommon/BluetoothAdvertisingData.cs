// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the advertising data for a Bluetooth device.
    /// </summary>
    public class BluetoothAdvertisingData
    {
        /// <summary>
        /// Gets the GUID for the Hub service.
        /// </summary>
        public static Guid HubService { get; } = new Guid("00001623-1212-EFDE-1623-785FEABCD123");

        /// <summary>
        /// Gets the GUID for the Hub characteristic.
        /// </summary>
        public static Guid HubCharacteristic { get; } = new Guid("00001624-1212-EFDE-1623-785FEABCD123");

        // This doesn't seems to be used
        // public byte DataTypeName { get; } = 0xFF;

        /// <summary>
        /// Gets the manufacturer ID.
        /// </summary>
        public ushort ManufacturerID { get; } = 0x0397;

        /// <summary>
        /// Gets or sets the button state.
        /// </summary>
        public bool ButtonState { get; set; }

        /// <summary>
        /// Gets or sets the system type and device number.
        /// </summary>
        public byte SystemTypeAndDeviceNumber { get; set; }

        /// <summary>
        /// Gets or sets the device capabilities.
        /// </summary>
        public DeviceCapabilities DeviceCapabilities { get; set; }

        /// <summary>
        /// Gets or sets the last network.
        /// </summary>
        public byte LastNetwork { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        public byte Option { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothAdvertisingData"/> class.
        /// </summary>
        /// <param name="buttonState">The button state.</param>
        /// <param name="systemTypeAndDeviceNumber">The system type and device number.</param>
        /// <param name="deviceCapabilities">The device capabilities.</param>
        /// <param name="lastNetwork">The last network.</param>
        /// <param name="status">The status.</param>
        /// <param name="option">The option.</param>
        public BluetoothAdvertisingData(bool buttonState, byte systemTypeAndDeviceNumber, DeviceCapabilities deviceCapabilities, byte lastNetwork, Status status, byte option)
        {
            ButtonState = buttonState;
            SystemTypeAndDeviceNumber = systemTypeAndDeviceNumber;
            DeviceCapabilities = deviceCapabilities;
            LastNetwork = lastNetwork;
            Status = status;
            Option = option;
        }

        /// <summary>
        /// Converts the advertising data to a byte array.
        /// </summary>
        /// <returns>A byte array representing the advertising data.</returns>
        public byte[] ToByteArray()
        {
            byte[] byteArray = new byte[6];
            int idx = 0;
            byteArray[idx++] = (byte)(ButtonState ? 1 : 0);
            byteArray[idx++] = SystemTypeAndDeviceNumber;
            byteArray[idx++] = (byte)DeviceCapabilities;
            byteArray[idx++] = LastNetwork;
            byteArray[idx++] = (byte)Status;
            byteArray[idx++] = Option;
            return byteArray;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"ManufacturerID: {ManufacturerID}, ButtonState: {ButtonState}, SystemTypeAndDeviceNumber: {SystemTypeAndDeviceNumber}, DeviceCapabilities: {DeviceCapabilities}, LastNetwork: {LastNetwork}, Status: {Status}, Option: {Option}";
        }
    }
}
