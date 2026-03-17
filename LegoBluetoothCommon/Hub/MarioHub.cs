// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Mario Hub (71360) in a LEGO Bluetooth system.
    /// </summary>
    public class MarioHub : BaseHub
    {
        public const string MarioHubName = "Mario";

        /// <summary>
        /// Initializes a new instance of the <see cref="MarioHub"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public MarioHub(IBluetooth bluetooth) : base(bluetooth)
        {
            DefaultName = MarioHubName;
            Name = MarioHubName;
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.MarioHub),
                DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBePeripheral | Status.RequestWindow,
                0
            );
            FWVersion = new Version(1, 0, 0, 0);
            HWVersion = new Version(1, 0, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.MarioHub);

            // Port 0x00 is the accelerometer
            MarioHubAccelerometer accelerometer = new MarioHubAccelerometer(bluetooth, 0x00, 0x00);
            // Port 0x01 is the tag sensor
            MarioHubTagSensor tagSensor = new MarioHubTagSensor(bluetooth, 0x00, 0x01);
            // Port 0x02 is the pants sensor
            MarioHubPants pants = new MarioHubPants(bluetooth, 0x00, 0x02);
            // Port 0x03 is the debug interface
            MarioHubDebug debug = new MarioHubDebug(bluetooth, 0x00, 0x03);
            // Port 0x06 is the voltage sensor
            Voltage voltageSensor = new Voltage(bluetooth, 0x00, 0x06);

            Devices.Add(accelerometer);
            Devices.Add(tagSensor);
            Devices.Add(pants);
            Devices.Add(debug);
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
