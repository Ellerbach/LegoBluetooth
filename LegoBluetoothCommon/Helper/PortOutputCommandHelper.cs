// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    public static class PortOutputCommandHelper
    {
        /// <summary>
        /// Creates a WriteDirect (0x50) command payload.
        /// Per spec, the payload is passed through directly with no checksum.
        /// </summary>
        /// <param name="payload">The payload for the sub-command.</param>
        /// <returns>A byte array representing the WriteDirect command payload.</returns>
        public static byte[] WriteDirect(byte[] payload)
        {
            if (payload == null || payload.Length == 0)
            {
                throw new ArgumentException("Payload cannot be null or empty.", nameof(payload));
            }

            byte[] data = new byte[payload.Length];
            Array.Copy(payload, 0, data, 0, payload.Length);

            return data;
        }

        /// <summary>
        /// Creates a WriteDirectModeData (0x51) command payload.
        /// Per spec, format is: Mode, Payload... (no checksum).
        /// </summary>
        /// <param name="mode">The mode for the sub-command.</param>
        /// <param name="payload">The payload for the sub-command.</param>
        /// <returns>A byte array representing the WriteDirectModeData command payload.</returns>
        public static byte[] WriteDirectModeData(byte mode, byte[] payload)
        {
            if (payload == null || payload.Length == 0)
            {
                throw new ArgumentException("Payload cannot be null or empty.", nameof(payload));
            }

            byte[] data = new byte[payload.Length + 1];
            data[0] = mode;
            Array.Copy(payload, 0, data, 1, payload.Length);

            return data;
        }
    }
}
