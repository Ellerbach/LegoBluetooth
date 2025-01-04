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
    public class PortInputFormatSetupSingleMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 10, 0, 0x41, 1, 2, 0, 1, 0, 0, 1 }, (byte)1, (byte)2, (uint)256, true)]
        [DataRow(new byte[] { 10, 0, 0x41, 2, 3, 1, 0, 0, 0, 0 }, (byte)2, (byte)3, (uint)1, false)]
        public void Decode_ValidData_ReturnsPortInputFormatSetupSingleMessage(byte[] data, byte expectedPortID, byte expectedMode, uint expectedDeltaInterval, bool expectedNotificationEnabled)
        {
            // Act
            var message = PortInputFormatSetupSingleMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedMode, message.Mode);
            Assert.AreEqual(expectedDeltaInterval, message.DeltaInterval);
            Assert.AreEqual(expectedNotificationEnabled, message.NotificationEnabled);
        }        

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)2, (uint)256, true, new byte[] { 10, 0, 0x41, 1, 2, 0, 1, 0, 0, 1 })]
        [DataRow((byte)0, (byte)2, (byte)3, (uint)1, false, new byte[] { 10, 0, 0x41, 2, 3, 1, 0, 0, 0, 0 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte mode, uint deltaInterval, bool notificationEnabled, byte[] expectedData)
        {
            // Arrange
            var message = new PortInputFormatSetupSingleMessage(hubID, portID, mode, deltaInterval, notificationEnabled);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
