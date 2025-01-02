// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Provides helper methods for encoding and decoding system type and device type numbers.
    /// </summary>
    public static class SystemTypeDeviceNumberHelper
    {
        /// <summary>
        /// Encodes the specified system type and device type into a single byte.
        /// </summary>
        /// <param name="systemType">The system type to encode.</param>
        /// <param name="deviceNumber">The device type to encode.</param>
        /// <returns>A byte representing the encoded system type and device type.</returns>
        public static byte Encode(SystemType systemType, DeviceType deviceNumber)
        {
            return (byte)(((byte)systemType << 5) | ((byte)deviceNumber & 0b11111));
        }

        /// <summary>
        /// Decodes the specified byte into a system type and device type.
        /// </summary>
        /// <param name="encodedValue">The byte to decode.</param>
        /// <param name="systemType">When this method returns, contains the decoded system type.</param>
        /// <param name="deviceNumber">When this method returns, contains the decoded device type.</param>
        public static void Decode(byte encodedValue, out SystemType systemType, out DeviceType deviceNumber)
        {
            systemType = (SystemType)(encodedValue >> 5);
            deviceNumber = (DeviceType)(encodedValue & 0b11111);
        }
    }
}
