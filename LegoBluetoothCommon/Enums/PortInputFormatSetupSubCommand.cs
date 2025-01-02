// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different sub-commands for Port Input Format Setup.
    /// </summary>
    public enum PortInputFormatSetupSubCommand : byte
    {
        /// <summary>
        /// Set ModeAndDataSet combination(s).
        /// </summary>
        SetModeAndDataSetCombinations = 0x01,

        /// <summary>
        /// Lock LPF2 Device for setup.
        /// </summary>
        LockDeviceForSetup = 0x02,

        /// <summary>
        /// Unlock and start with multi-update enabled.
        /// </summary>
        UnlockAndStartWithMultiUpdateEnabled = 0x03,

        /// <summary>
        /// Unlock and start with multi-update disabled.
        /// </summary>
        UnlockAndStartWithMultiUpdateDisabled = 0x04,

        /// <summary>
        /// Not used.
        /// </summary>
        NotUsed = 0x05,

        /// <summary>
        /// Reset Sensor.
        /// </summary>
        ResetSensor = 0x06
    }
}
