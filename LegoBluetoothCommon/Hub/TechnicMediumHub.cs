// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Technic Medium Hub (88012) in a LEGO Bluetooth system.
    /// </summary>
    public class TechnicMediumHub : BaseHub
    {
        public const string TechnicMediumHubName = "Technic Hub";

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicMediumHub"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public TechnicMediumHub(IBluetooth bluetooth) : base(bluetooth)
        {
            DefaultName = TechnicMediumHubName;
            Name = TechnicMediumHubName;
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoTechnic, DeviceType.TechnicMediumHub),
                DeviceCapabilities.SupportsLPF2Devices | DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBeCentral | Status.CanBePeripheral | Status.RequestWindow,
                0
            );
            FWVersion = new Version(1, 3, 0, 0);
            HWVersion = new Version(1, 0, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoTechnic, DeviceType.TechnicMediumHub);

            // Ports A-D are external ports
            BaseDevice portA = new BaseDevice(bluetooth, 0x00, 0x00);
            BaseDevice portB = new BaseDevice(bluetooth, 0x00, 0x01);
            BaseDevice portC = new BaseDevice(bluetooth, 0x00, 0x02);
            BaseDevice portD = new BaseDevice(bluetooth, 0x00, 0x03);
            // Port 0x32 is the RGB light
            RgbLight rgbLight = new RgbLight(bluetooth, 0x00, 0x32);
            // Port 0x3B is the current sensor
            Current currentSensor = new Current(bluetooth, 0x00, 0x3B);
            // Port 0x3C is the voltage sensor
            Voltage voltageSensor = new Voltage(bluetooth, 0x00, 0x3C);
            // Port 0x3D is temperature sensor 1
            TechnicMediumHubTemperatureSensor tempSensor1 = new TechnicMediumHubTemperatureSensor(bluetooth, 0x00, 0x3D);
            // Port 0x60 is temperature sensor 2
            TechnicMediumHubTemperatureSensor tempSensor2 = new TechnicMediumHubTemperatureSensor(bluetooth, 0x00, 0x60);
            // Port 0x61 is the accelerometer
            TechnicMediumHubAccelerometer accelerometer = new TechnicMediumHubAccelerometer(bluetooth, 0x00, 0x61);
            // Port 0x62 is the gyro sensor
            TechnicMediumHubGyroSensor gyroSensor = new TechnicMediumHubGyroSensor(bluetooth, 0x00, 0x62);
            // Port 0x63 is the tilt sensor
            TechnicMediumHubTiltSensor tiltSensor = new TechnicMediumHubTiltSensor(bluetooth, 0x00, 0x63);
            // Port 0x64 is the gesture sensor
            TechnicMediumHubGestureSensor gestureSensor = new TechnicMediumHubGestureSensor(bluetooth, 0x00, 0x64);

            Devices.Add(portA);
            Devices.Add(portB);
            Devices.Add(portC);
            Devices.Add(portD);
            Devices.Add(rgbLight);
            Devices.Add(currentSensor);
            Devices.Add(voltageSensor);
            Devices.Add(tempSensor1);
            Devices.Add(tempSensor2);
            Devices.Add(accelerometer);
            Devices.Add(gyroSensor);
            Devices.Add(tiltSensor);
            Devices.Add(gestureSensor);

            HydrateModes();
        }

        /// <inheritdoc/>
        public override void ClientJoiningStateChanged(bool joining)
        {
            base.ClientJoiningStateChanged(joining);
        }
    }
}
