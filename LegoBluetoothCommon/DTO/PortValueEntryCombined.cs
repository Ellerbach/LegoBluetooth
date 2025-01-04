// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a port value entry.
    /// </summary>
    public class PortValueEntryCombined
    {
        /// <summary>
        /// Gets or sets the bit pointer for the mode/dataset combination.
        /// </summary>
        public ushort BitPointer { get; set; }

        /// <summary>
        /// Gets or sets the input value of the addressed port.
        /// </summary>
        public object InputValue { get; set; }
    }
}
