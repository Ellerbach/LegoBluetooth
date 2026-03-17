// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Two Port Hub (City Hub, 88009) in a LEGO Bluetooth system.
    /// </summary>
    public class TwoPortHub : BaseHub
    {
        public const string TwoPortHubName = "Hub";

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoPortHub"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public TwoPortHub(IBluetooth bluetooth) : base(bluetooth)
        {
            DefaultName = TwoPortHubName;
            Name = TwoPortHubName;
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.TwoPortHub),
                DeviceCapabilities.SupportsLPF2Devices | DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBeCentral | Status.CanBePeripheral | Status.RequestWindow,
                0
            );
            FWVersion = new Version(1, 4, 0, 0);
            HWVersion = new Version(1, 0, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.TwoPortHub);

            // Port A and B are external ports
            BaseDevice portA = new BaseDevice(bluetooth, 0x00, 0x00);
            BaseDevice portB = new BaseDevice(bluetooth, 0x00, 0x01);
            // Port 0x32 is the RGB light
            RgbLight rgbLight = new RgbLight(bluetooth, 0x00, 0x32);
            // Port 0x3B is the current sensor
            Current currentSensor = new Current(bluetooth, 0x00, 0x3B);
            // Port 0x3C is the voltage sensor
            Voltage voltageSensor = new Voltage(bluetooth, 0x00, 0x3C);

            Devices.Add(portA);
            Devices.Add(portB);
            Devices.Add(rgbLight);
            Devices.Add(currentSensor);
            Devices.Add(voltageSensor);

            HydrateModes();
        }

        /// <inheritdoc/>
        public override void ClientJoiningStateChanged(bool joining)
        {
            base.ClientJoiningStateChanged(joining);
        }
    }
}
