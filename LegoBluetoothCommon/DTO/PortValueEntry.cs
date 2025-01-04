// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a port value entry.
    /// </summary>
    public class PortValueEntry
    {
        /// <summary>
        /// Gets or sets the Port ID for en value.
        /// </summary>
        public ushort PortID { get; set; }

        /// <summary>
        /// Gets or sets the input value of the addressed port.
        /// </summary>
        public object InputValue { get; set; }
    }
}
