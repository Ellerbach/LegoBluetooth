// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents an alert from the Hub.
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Alert"/> class with the specified alert type and operation.
        /// </summary>
        /// <param name="alertType">The type of the alert.</param>
        /// <param name="operation">The operation of the alert.</param>
        public Alert(HubAlertType alertType, HubAlertOperation operation)
        {
            AlertType = alertType;
            Operation = operation;
        }

        /// <summary>
        /// Gets or sets the type of the alert.
        /// </summary>
        public HubAlertType AlertType { get; set; }

        /// <summary>
        /// Gets or sets the operation of the alert.
        /// </summary>
        public HubAlertOperation Operation { get; set; }
    }
}
