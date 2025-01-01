namespace LegoBluetooth
{
    /// <summary>
    /// Represents an alert from the Hub.
    /// </summary>
    public class Alert
    {
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
