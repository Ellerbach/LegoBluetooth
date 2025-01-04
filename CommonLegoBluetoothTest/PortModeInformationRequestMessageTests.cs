// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using LegoBluetooth;
#if NANOFRAMEWORK_1_0
using nanoFramework.TestFramework;
#endif

namespace CommonLegoBluetoothTest
{
    [TestClass]
    public class PortModeInformationRequestMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 6, 0, 0x22, 1, 2, 0 }, (byte)1, (byte)2, (byte)ModeInformationType.Name)]
        [DataRow(new byte[] { 6, 0, 0x22, 2, 3, 1 }, (byte)2, (byte)3, (byte)ModeInformationType.Raw)]
        [DataRow(new byte[] { 6, 0, 0x22, 3, 4, 2 }, (byte)3, (byte)4, (byte)ModeInformationType.Percent)]
        [DataRow(new byte[] { 6, 0, 0x22, 4, 5, 3 }, (byte)4, (byte)5, (byte)ModeInformationType.SI)]
        [DataRow(new byte[] { 6, 0, 0x22, 5, 6, 4 }, (byte)5, (byte)6, (byte)ModeInformationType.Symbol)]
        [DataRow(new byte[] { 6, 0, 0x22, 6, 7, 5 }, (byte)6, (byte)7, (byte)ModeInformationType.Mapping)]
        [DataRow(new byte[] { 6, 0, 0x22, 7, 8, 7 }, (byte)7, (byte)8, (byte)ModeInformationType.MotorBias)]
        [DataRow(new byte[] { 6, 0, 0x22, 8, 9, 8 }, (byte)8, (byte)9, (byte)ModeInformationType.CapabilityBits)]
        [DataRow(new byte[] { 6, 0, 0x22, 9, 10, 128 }, (byte)9, (byte)10, (byte)ModeInformationType.ValueFormat)]
        public void Decode_ValidData_ReturnsPortModeInformationRequestMessage(byte[] data, byte expectedPortID, byte expectedMode, byte expectedInformationType)
        {
            // Act
            var message = PortModeInformationRequestMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedMode, message.Mode);
            Assert.AreEqual(expectedInformationType, (byte)message.InformationType);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)2, (byte)ModeInformationType.Name, new byte[] { 6, 0, 0x22, 1, 2, 0 })]
        [DataRow((byte)0, (byte)2, (byte)3, (byte)ModeInformationType.Raw, new byte[] { 6, 0, 0x22, 2, 3, 1 })]
        [DataRow((byte)0, (byte)3, (byte)4, (byte)ModeInformationType.Percent, new byte[] { 6, 0, 0x22, 3, 4, 2 })]
        [DataRow((byte)0, (byte)4, (byte)5, (byte)ModeInformationType.SI, new byte[] { 6, 0, 0x22, 4, 5, 3 })]
        [DataRow((byte)0, (byte)5, (byte)6, (byte)ModeInformationType.Symbol, new byte[] { 6, 0, 0x22, 5, 6, 4 })]
        [DataRow((byte)0, (byte)6, (byte)7, (byte)ModeInformationType.Mapping, new byte[] { 6, 0, 0x22, 6, 7, 5 })]
        [DataRow((byte)0, (byte)7, (byte)8, (byte)ModeInformationType.MotorBias, new byte[] { 6, 0, 0x22, 7, 8, 7 })]
        [DataRow((byte)0, (byte)8, (byte)9, (byte)ModeInformationType.CapabilityBits, new byte[] { 6, 0, 0x22, 8, 9, 8 })]
        [DataRow((byte)0, (byte)9, (byte)10, (byte)ModeInformationType.ValueFormat, new byte[] { 6, 0, 0x22, 9, 10, 128 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte mode, byte informationType, byte[] expectedData)
        {
            // Arrange
            var message = new PortModeInformationRequestMessage(hubID, portID, mode, (ModeInformationType)informationType);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
