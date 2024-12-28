/// <summary>
/// Represents the sub-command types for the port output command.
/// </summary>
public enum SubCommandType : byte
{
    /// <summary>
    /// Start power with a single power value.
    /// </summary>
    StartPower = 0x00,

    /// <summary>
    /// Start power with two power values.
    /// </summary>
    StartPowerDual = 0x02,

    /// <summary>
    /// Set acceleration time.
    /// </summary>
    SetAccTime = 0x05,

    /// <summary>
    /// Set deceleration time.
    /// </summary>
    SetDecTime = 0x06,

    /// <summary>
    /// Start speed with a single speed value.
    /// </summary>
    StartSpeed = 0x07,

    /// <summary>
    /// Start speed with two speed values.
    /// </summary>
    StartSpeedDual = 0x08,

    /// <summary>
    /// Start speed for a specified time.
    /// </summary>
    StartSpeedForTime = 0x09,

    /// <summary>
    /// Start speed for a specified time with two speed values.
    /// </summary>
    StartSpeedForTimeDual = 0x0A,

    /// <summary>
    /// Start speed for a specified number of degrees.
    /// </summary>
    StartSpeedForDegrees = 0x0B,

    /// <summary>
    /// Start speed for a specified number of degrees with two speed values.
    /// </summary>
    StartSpeedForDegreesDual = 0x0C,

    /// <summary>
    /// Go to an absolute position.
    /// </summary>
    GotoAbsolutePosition = 0x0D,

    /// <summary>
    /// Go to absolute positions with two position values.
    /// </summary>
    GotoAbsolutePositionDual = 0x0E,

    /// <summary>
    /// Preset the encoder to a specified position.
    /// </summary>
    PresetEncoder = 0x14,

    /// <summary>
    /// Preset the encoder to specified positions for two encoders.
    /// </summary>
    PresetEncoderDual = 0x14,

    /// <summary>
    /// Preset the tilt impact.
    /// </summary>
    TiltImpactPreset = 0x15,

    /// <summary>
    /// Configure the tilt orientation.
    /// </summary>
    TiltConfigOrientation = 0x16,

    /// <summary>
    /// Configure the tilt impact.
    /// </summary>
    TiltConfigImpact = 0x17,

    /// <summary>
    /// Perform a factory calibration for tilt.
    /// </summary>
    TiltFactoryCalibration = 0x18,

    /// <summary>
    /// Perform a hardware reset.
    /// </summary>
    HardwareReset = 0x19,

    /// <summary>
    /// Set the RGB color by color number.
    /// </summary>
    SetRgbColorNo = 0x1A,

    /// <summary>
    /// Set the RGB color by individual RGB values.
    /// </summary>
    SetRgbColors = 0x1B
}