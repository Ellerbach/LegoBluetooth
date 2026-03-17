// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different I/O Type IDs for the Hub.
    /// </summary>
    public enum IOTypeID : ushort
    {
        /// <summary>
        /// Unknown. In that case, the device type won't be advertized.
        /// </summary>
        Unknown = 0x0000,

        /// <summary>
        /// Motor.
        /// </summary>
        Motor = 0x0001,

        /// <summary>
        /// System Train Motor.
        /// </summary>
        SystemTrainMotor = 0x0002,

        /// <summary>
        /// Button.
        /// </summary>
        Button = 0x0005,

        /// <summary>
        /// LED Light.
        /// </summary>
        LEDLight = 0x0008,

        /// <summary>
        /// Voltage.
        /// </summary>
        Voltage = 0x0014,

        /// <summary>
        /// Current.
        /// </summary>
        Current = 0x0015,

        /// <summary>
        /// Piezo Tone (Sound).
        /// </summary>
        PiezoTone = 0x0016,

        /// <summary>
        /// RGB Light.
        /// </summary>
        RGBLight = 0x0017,

        /// <summary>
        /// External Tilt Sensor.
        /// </summary>
        ExternalTiltSensor = 0x0022,

        /// <summary>
        /// Motion Sensor.
        /// </summary>
        MotionSensor = 0x0023,

        /// <summary>
        /// Vision Sensor.
        /// </summary>
        VisionSensor = 0x0025,

        /// <summary>
        /// External Motor with Tacho.
        /// </summary>
        ExternalMotorWithTacho = 0x0026,

        /// <summary>
        /// Internal Motor with Tacho.
        /// </summary>
        InternalMotorWithTacho = 0x0027,

        /// <summary>
        /// Internal Tilt.
        /// </summary>
        InternalTilt = 0x0028,

        /// <summary>
        /// Duplo Train Base Motor.
        /// </summary>
        DuploTrainBaseMotor = 0x0029,

        /// <summary>
        /// Duplo Train Base Speaker.
        /// </summary>
        DuploTrainBaseSpeaker = 0x002A,

        /// <summary>
        /// Duplo Train Base Color Sensor.
        /// </summary>
        DuploTrainBaseColorSensor = 0x002B,

        /// <summary>
        /// Duplo Train Base Speedometer.
        /// </summary>
        DuploTrainBaseSpeedometer = 0x002C,

        /// <summary>
        /// Technic Large Linear Motor.
        /// </summary>
        TechnicLargeLinearMotor = 0x002E,

        /// <summary>
        /// Technic Medium Hub Gesture Sensor.
        /// </summary>
        TechnicMediumHubGestureSensor = 0x0036,

        /// <summary>
        /// Technic Medium Hub Accelerometer.
        /// </summary>
        TechnicMediumHubAccelerometer = 0x0039,

        /// <summary>
        /// Technic Medium Hub Gyro Sensor.
        /// </summary>
        TechnicMediumHubGyroSensor = 0x003A,

        /// <summary>
        /// Technic Medium Hub Tilt Sensor.
        /// </summary>
        TechnicMediumHubTiltSensor = 0x003B,

        /// <summary>
        /// Technic Medium Hub Temperature Sensor.
        /// </summary>
        TechnicMediumHubTemperatureSensor = 0x003C,

        /// <summary>
        /// Unknown sensor (MoveHub port 0x46).
        /// </summary>
        Hub42 = 0x0042,

        /// <summary>
        /// Mario Hub Debug.
        /// </summary>
        MarioHubDebug = 0x0046,

        /// <summary>
        /// Mario Hub Accelerometer.
        /// </summary>
        MarioHubAccelerometer = 0x0047,

        /// <summary>
        /// Mario Hub Tag Sensor.
        /// </summary>
        MarioHubTagSensor = 0x0049,

        /// <summary>
        /// Mario Hub Pants.
        /// </summary>
        MarioHubPants = 0x004A,

        /// <summary>
        /// Used for unkonw devices with a default configuration.
        /// </summary>
        Other = 0xFFFF
    }
}
