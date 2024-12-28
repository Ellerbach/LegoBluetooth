using System;

namespace LegoBluetooth
{
    public class BluetoothAdvertisingData
    {
        public Guid HubService { get; } = new Guid("00001623 -1212-EFDE-1623-785FEABCD123");

        public Guid HubCharacteristic { get; } = new Guid("00001624 -1212-EFDE-1623-785FEABCD123");

        public byte Length { get; } = 0x09;
        public byte DataTypeName { get; } = 0xFF;
        public ushort ManufacturerID { get; } = 0x0397;
        public bool ButtonState { get; set; }
        public byte SystemTypeAndDeviceNumber { get; set; }
        public DeviceCapabilities DeviceCapabilities { get; set; }
        public byte LastNetwork { get; set; }
        public byte Status { get; set; }
        public byte Option { get; set; }

        public BluetoothAdvertisingData(bool buttonState, byte systemTypeAndDeviceNumber, DeviceCapabilities deviceCapabilities, byte lastNetwork, byte status, byte option)
        {
            ButtonState = buttonState;
            SystemTypeAndDeviceNumber = systemTypeAndDeviceNumber;
            DeviceCapabilities = deviceCapabilities;
            LastNetwork = lastNetwork;
            Status = status;
            Option = option;
        }

        public override string ToString()
        {
            return $"Length: {Length}, DataTypeName: {DataTypeName}, ManufacturerID: {ManufacturerID}, ButtonState: {ButtonState}, SystemTypeAndDeviceNumber: {SystemTypeAndDeviceNumber}, DeviceCapabilities: {DeviceCapabilities}, LastNetwork: {LastNetwork}, Status: {Status}, Option: {Option}";
        }
    }
}
