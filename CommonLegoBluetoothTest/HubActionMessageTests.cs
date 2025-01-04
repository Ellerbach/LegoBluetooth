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
    public class HubActionMessageTests
    {
        [TestMethod]
        [DataRow((byte)0x01, (byte)ActionType.SwitchOffHub)]
        [DataRow((byte)0x02, (byte)ActionType.Disconnect)]
        public void Constructor_ShouldInitializeProperties(byte hubID, byte actionType)
        {
            // Act
            var message = new HubActionMessage(hubID, (ActionType)actionType);

            // Assert
            Assert.AreEqual(hubID, message.HubID);
            Assert.AreEqual(actionType, (byte)message.ActionType);
            Assert.AreEqual((ushort)4, message.Length);
            Assert.AreEqual(MessageType.HubActions, message.MessageType);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x04, 0x01, 0x02, 0x01 }, (byte)0x01, (byte)ActionType.SwitchOffHub)]
        [DataRow(new byte[] { 0x04, 0x02, 0x02, 0x02 }, (byte)0x02, (byte)ActionType.Disconnect)]
        public void Decode_ShouldReturnHubActionMessage_WhenDataIsValid(byte[] data, byte expectedHubID, byte expectedActionType)
        {
            // Act
            var message = HubActionMessage.Decode(data);

            // Assert
            Assert.IsNotNull(message);
            Assert.AreEqual(expectedHubID, message.HubID);
            Assert.AreEqual(expectedActionType, (byte)message.ActionType);
            Assert.AreEqual((ushort)4, message.Length);
        }

        [TestMethod]
        //[DataRow(null)]
        [DataRow(new byte[] { 0x01, 0x02 })]
        public void Decode_ShouldThrowException_WhenDataIsInvalid(byte[] data)
        {
            // Act & Assert
            try
            {
                HubActionMessage.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        [DataRow((byte)0x01, (byte)ActionType.SwitchOffHub, new byte[] { 0x04, 0x01, 0x02, 0x01 })]
        [DataRow((byte)0x02, (byte)ActionType.Disconnect, new byte[] { 0x04, 0x02, 0x02, 0x02 })]
        public void ToByteArray_ShouldReturnByteArray(byte hubID, byte actionType, byte[] expectedData)
        {
            // Arrange
            var message = new HubActionMessage(hubID, (ActionType)actionType);

            // Act
            var result = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, result);
        }
    }
}
