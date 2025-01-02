// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message for subscribing to Hub alerts and requesting the current state of these alerts.
    /// </summary>
    public class HubAlertMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the alert type.
        /// </summary>
        /// <remarks>
        /// Identifies the property subject to the performed operation.
        /// </remarks>
        public HubAlertType AlertType { get; set; }

        /// <summary>
        /// Gets or sets the alert operation.
        /// </summary>
        /// <remarks>
        /// The operation to perform on the specified property.
        /// </remarks>
        public HubAlertOperation AlertOperation { get; set; }

        /// <summary>
        /// Gets or sets the alert payload.
        /// </summary>
        /// <remarks>
        /// The payload of the alert message in the upstream direction.
        /// </remarks>
        public HubAlertPayload AlertPayload { get; set; } = HubAlertPayload.NoStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="HubAlertMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="alertType">The alert type.</param>
        /// <param name="alertOperation">The alert operation.</param>
        /// <param name="alertPayload">The alert payload.</param>
        public HubAlertMessage(ushort length, byte hubID, HubAlertType alertType, HubAlertOperation alertOperation, HubAlertPayload alertPayload)
            : base(length, hubID, MessageType.HubAlerts)
        {
            AlertType = alertType;
            AlertOperation = alertOperation;
            AlertPayload = alertPayload;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="HubAlertMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="HubAlertMessage"/> instance.</returns>
        public static new HubAlertMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            HubAlertType alertType = (HubAlertType)data[3];
            HubAlertOperation alertOperation = (HubAlertOperation)data[4];
            // In theory, we don't have any alert downstream.
            HubAlertPayload alertPayload = HubAlertPayload.NoStatus;
            if (data.Length > 5)
            {
                alertPayload = (HubAlertPayload)data[5];
            }            

            return new HubAlertMessage((ushort)data.Length, data[1], alertType, alertOperation, alertPayload)
            {
                Message = data,
            };
        }

        /// <summary>
        /// Serializes the HubAlertMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the HubAlertMessage.</returns>
        public override byte[] ToByteArray()
        {
            Length = (ushort)(5 + (AlertPayload != HubAlertPayload.NoStatus ? 1 : 0));

            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = (byte)AlertType;
            data[index++] = (byte)AlertOperation;
            data[index++] = (byte)AlertPayload;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the hub alert message.
        /// </summary>
        /// <returns>A string representation of the hub alert message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, AlertType: {AlertType}, AlertOperation: {AlertOperation}, AlertPayload: {AlertPayload}";
        }
    }
}
