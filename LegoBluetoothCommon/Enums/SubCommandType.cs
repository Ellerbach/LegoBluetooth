// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

/// <summary>
/// Represents the sub-command types for the port output command (0x81).
/// Only values that correspond to actual protocol sub-command bytes are included.
/// Commands encoded via WriteDirect (0x50) or WriteDirectModeData (0x51) use
/// those sub-commands with a mode byte, not their own sub-command value.
/// </summary>
public enum SubCommandType : byte
{
    /// <summary>
    /// Start power with a single power value (sub-command 0x01).
    /// </summary>
    StartPower = 0x01,

    /// <summary>
    /// Start power with two power values (sub-command 0x02).
    /// </summary>
    StartPowerDual = 0x02,

    /// <summary>
    /// Set acceleration time (sub-command 0x05).
    /// </summary>
    SetAccTime = 0x05,

    /// <summary>
    /// Set deceleration time (sub-command 0x06).
    /// </summary>
    SetDecTime = 0x06,

    /// <summary>
    /// Start speed with a single speed value (sub-command 0x07).
    /// </summary>
    StartSpeed = 0x07,

    /// <summary>
    /// Start speed with two speed values (sub-command 0x08).
    /// </summary>
    StartSpeedDual = 0x08,

    /// <summary>
    /// Start speed for a specified time (sub-command 0x09).
    /// </summary>
    StartSpeedForTime = 0x09,

    /// <summary>
    /// Start speed for a specified time with two speed values (sub-command 0x0A).
    /// </summary>
    StartSpeedForTimeDual = 0x0A,

    /// <summary>
    /// Start speed for a specified number of degrees (sub-command 0x0B).
    /// </summary>
    StartSpeedForDegrees = 0x0B,

    /// <summary>
    /// Start speed for a specified number of degrees with two speed values (sub-command 0x0C).
    /// </summary>
    StartSpeedForDegreesDual = 0x0C,

    /// <summary>
    /// Go to an absolute position (sub-command 0x0D).
    /// </summary>
    GotoAbsolutePosition = 0x0D,

    /// <summary>
    /// Go to absolute positions with two position values (sub-command 0x0E).
    /// </summary>
    GotoAbsolutePositionDual = 0x0E,

    /// <summary>
    /// Preset encoder for two motors (sub-command 0x14).
    /// Single-motor PresetEncoder uses WriteDirectModeData (0x51).
    /// </summary>
    PresetEncoder = 0x14,

    /// <summary>
    /// Write direct data to a port (sub-command 0x50).
    /// </summary>
    WriteDirect = 0x50,

    /// <summary>
    /// Write direct mode data to a port (sub-command 0x51).
    /// </summary>
    WriteDirectModeData = 0x51,
}