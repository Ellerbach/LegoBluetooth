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
        InternalTilt = 0x0028
    }
}
