// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a port feedback entry.
    /// </summary>
    public class PortFeedbackEntry
    {
        /// <summary>
        /// Gets or sets the Port ID for the feedback message.
        /// </summary>
        public byte PortID { get; set; }

        /// <summary>
        /// Gets or sets the feedback message as bit-fields.
        /// </summary>
        public FeedbackMessage Feedback { get; set; }
    }
}
