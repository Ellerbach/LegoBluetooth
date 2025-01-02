// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    public static class PortOutputCommandHelper
    {
        /// <summary>
        /// Creates a WriteDirect command payload.
        /// </summary>
        /// <param name="payload">The payload for the sub-command.</param>
        /// <returns>A byte array representing the WriteDirect command payload.</returns>
        public static byte[] WriteDirect(byte[] payload)
        {
            if (payload == null || payload.Length == 0)
            {
                throw new ArgumentException("Payload cannot be null or empty.", nameof(payload));
            }

            byte[] data = new byte[payload.Length + 1];
            Array.Copy(payload, 0, data, 0, payload.Length);

            byte checksum = CalculateChecksum(payload);
            data[data.Length - 1] = checksum;

            return data;
        }

        /// <summary>
        /// Creates a WriteDirectModeData command payload.
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

            byte[] data = new byte[payload.Length + 2];
            data[0] = mode;
            Array.Copy(payload, 0, data, 1, payload.Length);

            byte checksum = CalculateChecksum(payload);
            data[data.Length - 1] = checksum;

            return data;
        }

        /// <summary>
        /// Calculates the checksum for the given payload.
        /// </summary>
        /// <param name="payload">The payload for which to calculate the checksum.</param>
        /// <returns>The calculated checksum.</returns>
        private static byte CalculateChecksum(byte[] payload)
        {
            byte checksum = 0xFF;
            foreach (byte b in payload)
            {
                checksum ^= b;
            }
            return checksum;
        }
    }
}
