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
    public class VirtualPortSetupMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 5, 0, 0x61, 0x00, 0x01 }, (byte)0, (byte)0x00, (byte)0x01, (byte)0, (byte)0)]
        [DataRow(new byte[] { 6, 0, 0x61, 0x01, 0x02, 0x03 }, (byte)0, (byte)0x01, (byte)0, (byte)0x02, (byte)0x03)]
        public void Decode_ValidData_ReturnsVirtualPortSetupMessage(byte[] data, byte expectedHubID, byte expectedSubCommand, byte expectedPortID, byte expectedPortIDA, byte expectedPortIDB)
        {
            // Act
            var message = VirtualPortSetupMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedHubID, message.HubID);
            Assert.AreEqual(expectedSubCommand, message.SubCommand);
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedPortIDA, message.PortIDA);
            Assert.AreEqual(expectedPortIDB, message.PortIDB);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)0x00, (byte)0x01, (byte)0, (byte)0, new byte[] { 5, 0, 0x61, 0x00, 0x01 })]
        [DataRow((byte)0, (byte)0x01, (byte)0, (byte)0x02, (byte)0x03, new byte[] { 6, 0, 0x61, 0x01, 0x02, 0x03 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte subCommand, byte portID, byte portIDA, byte portIDB, byte[] expectedData)
        {
            // Arrange
            var message = new VirtualPortSetupMessage(hubID, subCommand, portID, portIDA, portIDB);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
