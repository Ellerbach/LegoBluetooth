using System;

namespace LegoBluetooth
{
    public static class SystemTypeDeviceNumberHelper
    {
        public static byte Encode(SystemType systemType, DeviceType deviceNumber)
        {
            return (byte)(((byte)systemType << 5) | ((byte)deviceNumber & 0b11111));
        }

        public static void Decode(byte encodedValue, out SystemType systemType, out DeviceType deviceNumber)
        {
            systemType = (SystemType)(encodedValue >> 5);
            deviceNumber = (DeviceType)(encodedValue & 0b11111);
        }
    }
}