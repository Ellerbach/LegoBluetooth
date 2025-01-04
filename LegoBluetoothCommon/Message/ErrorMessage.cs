// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents a generic error message from the Hub.
    /// </summary>
    public class ErrorMessage : CommonMessageHeader
    {
        /// <summary>
        /// Gets or sets the command type that introduced this message.
        /// </summary>
        public byte CommandType { get; set; }

        /// <summary>
        /// Gets or sets the error code introduced by the command.
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="hubID">The Hub ID.</param>
        /// <param name="commandType">The command type that introduced this message.</param>
        /// <param name="errorCode">The error code introduced by the command.</param>
        public ErrorMessage(byte hubID, byte commandType, ErrorCode errorCode)
            : base(hubID, MessageType.GenericErrorMessages)
        {
            // Section 3.9
            CommandType = commandType;
            ErrorCode = errorCode;
            Length = 5;
        }

        /// <summary>
        /// Decodes a byte array to create a <see cref="ErrorMessage"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the message data.</param>
        /// <returns>A <see cref="ErrorMessage"/> instance.</returns>
        public static new ErrorMessage Decode(byte[] data)
        {
            if (data == null || data.Length < 5)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 5 bytes.", nameof(data));
            }

            byte commandType = data[3];
            ErrorCode errorCode = (ErrorCode)data[4];

            return new ErrorMessage(data[1], commandType, errorCode)
            {
                Message = data,
                Length = 5,
            };
        }

        /// <summary>
        /// Serializes the ErrorMessage to a byte array.
        /// </summary>
        /// <returns>A byte array representing the ErrorMessage.</returns>
        public override byte[] ToByteArray()
        {            
            byte[] data = new byte[Length];
            int index = 0;

            data[index++] = (byte)Length;
            data[index++] = HubID;
            data[index++] = (byte)MessageType;
            data[index++] = CommandType;
            data[index++] = (byte)ErrorCode;

            Message = data;

            return data;
        }

        /// <summary>
        /// Provides a string representation of the error message.
        /// </summary>
        /// <returns>A string representation of the error message.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, CommandType: {CommandType}, ErrorCode: {ErrorCode}";
        }
    }
}
