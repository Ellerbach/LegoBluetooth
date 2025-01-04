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
    public class PortInformationRequestMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 5, 0, 0x21, 1, 1 }, (byte)1, (byte)InformationType.ModeInfo)]
        [DataRow(new byte[] { 5, 0, 0x21, 2, 2 }, (byte)2, (byte)InformationType.PossibleModeCombinations)]
        public void Decode_ValidData_ReturnsPortInformationRequestMessage(byte[] data, byte expectedPortID, byte expectedInformationType)
        {
            // Act
            var message = PortInformationRequestMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedInformationType, (byte)message.InformationType);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)InformationType.ModeInfo, new byte[] { 5, 0, 0x21, 1, 1 })]
        [DataRow((byte)0, (byte)2, (byte)InformationType.PossibleModeCombinations, new byte[] { 5, 0, 0x21, 2, 2 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte informationType, byte[] expectedData)
        {
            // Arrange
            var message = new PortInformationRequestMessage(hubID, portID, (InformationType)informationType);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
