// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Duplo Train Base Hub (10874) in a LEGO Bluetooth system.
    /// </summary>
    public class DuploTrainBaseHub : BaseHub
    {
        public const string DuploTrainBaseName = "Train Base";

        /// <summary>
        /// Initializes a new instance of the <see cref="DuploTrainBaseHub"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public DuploTrainBaseHub(IBluetooth bluetooth) : base(bluetooth)
        {
            DefaultName = DuploTrainBaseName;
            Name = DuploTrainBaseName;
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoDuplo, DeviceType.DuploTrain),
                DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBePeripheral | Status.RequestWindow,
                0
            );
            FWVersion = new Version(1, 6, 0, 0);
            HWVersion = new Version(1, 0, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoDuplo, DeviceType.DuploTrain);

            // Port 0x00 is the motor
            DuploTrainBaseMotor motor = new DuploTrainBaseMotor(bluetooth, 0x00, 0x00);
            // Port 0x01 is the speaker
            DuploTrainBaseSpeaker speaker = new DuploTrainBaseSpeaker(bluetooth, 0x00, 0x01);
            // Port 0x11 is the RGB light
            RgbLight rgbLight = new RgbLight(bluetooth, 0x00, 0x11);
            // Port 0x12 is the color sensor
            DuploTrainBaseColorSensor colorSensor = new DuploTrainBaseColorSensor(bluetooth, 0x00, 0x12);
            // Port 0x13 is the speedometer
            DuploTrainBaseSpeedometer speedometer = new DuploTrainBaseSpeedometer(bluetooth, 0x00, 0x13);
            // Port 0x14 is the voltage sensor
            Voltage voltageSensor = new Voltage(bluetooth, 0x00, 0x14);

            Devices.Add(motor);
            Devices.Add(speaker);
            Devices.Add(rgbLight);
            Devices.Add(colorSensor);
            Devices.Add(speedometer);
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
