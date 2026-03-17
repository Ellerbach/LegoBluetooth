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
    public class PortOutputCommandMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 8, 0, 0x81, 1, 0x10, 0x01, 0x01, 0x02 }, (byte)1, (byte)StartupCompletionInfo.ExecuteImmediately, (byte)SubCommandType.StartPower, new byte[] { 0x01, 0x02 })]
        [DataRow(new byte[] { 10, 0, 0x81, 2, 0x11, 0x02, 0x03, 0x04, 0x05, 0x06 }, (byte)2, (byte)(StartupCompletionInfo.ExecuteImmediately | StartupCompletionInfo.CommandFeedback), (byte)SubCommandType.StartPowerDual, new byte[] { 0x03, 0x04, 0x05, 0x06 })]
        public void Decode_ValidData_ReturnsPortOutputCommandMessage(byte[] data, byte expectedPortID, byte expectedStartupCompletion, byte expectedSubCommand, byte[] expectedPayload)
        {
            // Act
            var message = PortOutputCommandMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedStartupCompletion, (byte)message.StartupCompletion);
            Assert.AreEqual(expectedSubCommand, (byte)message.SubCommand);
            CollectionAssert.AreEqual(expectedPayload, message.Payload);
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)StartupCompletionInfo.ExecuteImmediately, (byte)SubCommandType.StartPower, new byte[] { 0x01, 0x02 }, new byte[] { 8, 0, 0x81, 1, 0x10, 0x01, 0x01, 0x02 })]
        [DataRow((byte)0, (byte)2, (byte)(StartupCompletionInfo.ExecuteImmediately | StartupCompletionInfo.CommandFeedback), (byte)SubCommandType.StartPowerDual, new byte[] { 0x03, 0x04, 0x05, 0x06 }, new byte[] { 10, 0, 0x81, 2, 0x11, 0x02, 0x03, 0x04, 0x05, 0x06 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte startupCompletion, byte subCommand, byte[] payload, byte[] expectedData)
        {
            // Arrange
            var message = new PortOutputCommandMessage(hubID, portID, (StartupCompletionInfo)startupCompletion, (SubCommandType)subCommand, payload);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
