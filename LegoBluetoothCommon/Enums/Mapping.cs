// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different mapping capabilities for the Hub.
    /// </summary>
    [Flags]
    public enum Mapping : byte
    {
        /// <summary>
        /// Supports NULL value.
        /// </summary>
        SupportsNullValue = 1 << 7,

        /// <summary>
        /// Supports Functional Mapping 2.0+.
        /// </summary>
        SupportsFunctionalMapping = 1 << 6,

        /// <summary>
        /// Absolute [min..max].
        /// </summary>
        Absolute = 1 << 4,

        /// <summary>
        /// Relative [-1..1].
        /// </summary>
        Relative = 1 << 3,

        /// <summary>
        /// Discrete [0, 1, 2, 3].
        /// </summary>
        Discrete = 1 << 2
    }
}
