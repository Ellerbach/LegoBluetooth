// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different device types within a system type.
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// LEGO WeDo Hub.
        /// </summary>
        WeDoHub = 0b000_00000,

        /// <summary>
        /// LEGO Duplo Train.
        /// </summary>
        DuploTrain = 0b001_00000,

        /// <summary>
        /// LEGO Boost Hub.
        /// </summary>
        BoostHub = 0b010_00000,

        /// <summary>
        /// LEGO 2 Port Hub.
        /// </summary>
        TwoPortHub = 0b010_00001,

        /// <summary>
        /// LEGO 2 Port Handset.
        /// </summary>
        TwoPortHandset = 0b010_00010
    }
}
