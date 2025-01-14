// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Collections;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a Move Hub device with an unknown type in a LEGO Bluetooth system.
    /// </summary>
    public class MoveHub : BaseHub
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveHub0x46"/> class.
        /// </summary>
        /// <param name="ble">The Bluetooth interface.</param>
        /// <param name="hubID">The ID of the hub.</param>
        /// <param name="portID">The ID of the port.</param>
        public MoveHub(IBluetooth bluetooth) : base(bluetooth)
        {
            Name = "Move Hub";
            BluetoothAdvertisingData = new BluetoothAdvertisingData(
                false,
                SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub),
                DeviceCapabilities.SupportsLPF2Devices | DeviceCapabilities.SupportsPeripheralRole,
                (byte)LastNetworkID.DisableHWNetwork,
                Status.RequestConnect | Status.CanBeCentral | Status.CanBePeripheral | Status.RequestWindow,
                0
            );
            FWVersion = new Version(2, 0, 0, 17);
            HWVersion = new Version(0, 4, 0, 0);
            SystemTypeID = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub);

            // Adding the 2 internal motors
            // Todo adjust hub ID
            MoveHubInternalMotor moveHubInternalMotorA = new MoveHubInternalMotor(bluetooth, 0x00, 0x00);
            MoveHubInternalMotor moveHubInternalMotorB = new MoveHubInternalMotor(bluetooth, 0x00, 0x01);
            // By default there is nothing on port C and D
            BaseDevice portC = new BaseDevice(bluetooth, 0x00, 0x02);
            BaseDevice portD = new BaseDevice(bluetooth, 0x00, 0x03);
            // port 16 is the virtual port for the internal motor A and B
            MoveHubInternalMotor moveHubInternalMotorAB = new MoveHubInternalMotor(bluetooth, 0x00, 0x10);
            moveHubInternalMotorAB.IsVirtual = true;
            moveHubInternalMotorAB.PortIdA = 0x00;
            moveHubInternalMotorAB.PortIdB = 0x01;
            // port 32 is the virtual port for the rgb light sensor
            RgbLight rgbLightSensor = new RgbLight(bluetooth, 0x00, 0x32);
            // port 3A is the internal tilt sensor
            MoveHubTiltSensor tiltSensor = new MoveHubTiltSensor(bluetooth, 0x00, 0x3A);
            // Port 3B is the current sensor
            Current currentSensor = new Current(bluetooth, 0x00, 0x3B);
            // Port 3C is the voltage sensor
            Voltage voltageSensor = new Voltage(bluetooth, 0x00, 0x3C);
            // Port 70 is the virtual port for the unknown sensor
            MoveHub0x46 moveHub0X46 = new MoveHub0x46(bluetooth, 0x00, 0x46);

            Devices.Add(moveHubInternalMotorA);
            Devices.Add(moveHubInternalMotorB);
            Devices.Add(portC);
            Devices.Add(portD);
            Devices.Add(moveHubInternalMotorAB);
            Devices.Add(rgbLightSensor);
            Devices.Add(tiltSensor);
            Devices.Add(currentSensor);
            Devices.Add(voltageSensor);
            Devices.Add(moveHub0X46);

            // This currently consumes too much memory
            // HydrateModes();
        }

        /// <inheritdoc/>
        public override void ClientJoiningStateChanged(bool joining)
        {
            // Calling first the base method to send the device elements
            base.ClientJoiningStateChanged(joining);
        }
    }
}
