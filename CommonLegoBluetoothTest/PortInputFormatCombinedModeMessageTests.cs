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
    public class PortInputFormatCombinedModeMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 7, 0, 0x48, 1, 2, 0, 1 }, (byte)1, (byte)2, (ushort)256)]
        [DataRow(new byte[] { 7, 0, 0x48, 2, 3, 1, 0 }, (byte)2, (byte)3, (ushort)1)]
        public void Decode_ValidData_ReturnsPortInputFormatCombinedModeMessage(byte[] data, byte expectedPortID, byte expectedCombinedControlByte, ushort expectedModeDatasetCombinationBitPointer)
        {
            // Act
            var message = PortInputFormatCombinedModeMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedCombinedControlByte, message.CombinedControlByte);
            Assert.AreEqual(expectedModeDatasetCombinationBitPointer, message.ModeDatasetCombinationBitPointer);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)2, (ushort)256, new byte[] { 7, 0, 0x48, 1, 2, 0, 1 })]
        [DataRow((byte)0, (byte)2, (byte)3, (ushort)1, new byte[] { 7, 0, 0x48, 2, 3, 1, 0 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte combinedControlByte, ushort modeDatasetCombinationBitPointer, byte[] expectedData)
        {
            // Arrange
            var message = new PortInputFormatCombinedModeMessage(hubID, portID, combinedControlByte, modeDatasetCombinationBitPointer);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
