// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a message that provides feedback for port output commands.
    /// </summary>
    public class PortOutputCommandFeedbackMessage : CommonMessageHeader
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

        /// <summary>
        /// Gets or sets the list of port feedback entries.
        /// </summary>
        public ArrayList PortFeedbacks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortOutputCommandFeedbackMessage"/> class.
        /// </summary>
        /// <param name="length">The length of the entire message in bytes.</param>
        /// <param name="hubID">The Hub ID.</param>
        public PortOutputCommandFeedbackMessage(ushort length, byte hubID)
            : base(length, hubID, MessageType.PortOutputCommandFeedback)
        {
            PortFeedbacks = new ArrayList();
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="PortOutputCommandFeedbackMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="PortOutputCommandFeedbackMessage"/> instance.</returns>
        public static new PortOutputCommandFeedbackMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            var message = new PortOutputCommandFeedbackMessage((ushort)data.Length, data[1])
            {
                Message = data,
            };

            int index = 3;
            while (index < data.Length)
            {
                byte portID = data[index++];
                FeedbackMessage feedback = (FeedbackMessage)data[index++];

                message.PortFeedbacks.Add(new PortFeedbackEntry
                {
                    PortID = portID,
                    Feedback = feedback
                });
            }

            return message;
        }

        /// <summary>
        /// Serializes the PortOutputCommandFeedbackMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the PortOutputCommandFeedbackMessage.</returns>
        public override byte[] ToByteArray()
        {
            ArrayList data = new ArrayList();

            if (Length < 127)
            {
                data.Add((byte)Length);
            }
            else
            {
                data.Add((byte)((Length >> 8) | 0x80));
                data.Add((byte)(Length & 0xFF));
            }

            data.Add(HubID);
            data.Add((byte)MessageType);

            foreach (PortFeedbackEntry feedback in PortFeedbacks)
            {
                data.Add(feedback.PortID);
                data.Add((byte)feedback.Feedback);
            }

            return (byte[])data.ToArray(typeof(byte));
        }

        /// <summary>
        /// Provides a string representation of the port output command feedback message.
        /// </summary>
        /// <returns>A string representation of the port output command feedback message.</returns>
        public override string ToString()
        {
            string feedbacksString = "";
            foreach (PortFeedbackEntry feedback in PortFeedbacks)
            {
                if (feedbacksString.Length > 0)
                {
                    feedbacksString += ", ";
                }
                feedbacksString += $"PortID: {feedback.PortID}, Feedback: {feedback.Feedback}";
            }

            return $"{base.ToString()}, PortFeedbacks: [{feedbacksString}]";
        }
    }
}
