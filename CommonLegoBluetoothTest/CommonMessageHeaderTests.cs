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
    public class CommonMessageHeaderTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            byte hubID = 0x01;
            MessageType messageType = MessageType.HubProperties;

            // Act
            var header = new CommonMessageHeader(hubID, messageType);

            // Assert
            Assert.AreEqual(hubID, header.HubID);
            Assert.AreEqual((byte)messageType, (byte)header.MessageType);
        }

        [TestMethod]
        public void Decode_ShouldThrowException_WhenDataIsNull()
        {
            // Arrange
            byte[] data = null;

            // Act & Assert
            try
            {
                CommonMessageHeader.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void Decode_ShouldThrowException_WhenDataLengthIsLessThan3()
        {
            // Arrange
            byte[] data = new byte[2];

            // Act & Assert
            try
            {
                CommonMessageHeader.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void Decode_ShouldReturnCommonMessageHeader_WhenDataIsValid()
        {
            // Arrange
            byte[] data = new byte[] { 0x05, 0x00, 0x01, 0x04, 0x05 };

            // Act
            var header = CommonMessageHeader.Decode(data);

            // Assert
            Assert.IsNotNull(header);
            Assert.AreEqual((ushort)5, header.Length);
            Assert.AreEqual((ushort)0x00, header.HubID);
            Assert.AreEqual((byte)MessageType.HubProperties, (byte)header.MessageType);
        }

        [TestMethod]
        public void ToByteArray_ShouldReturnMessage()
        {
            // Arrange
            byte[] message = new byte[] { 0x01, 0x02, 0x03 };
            var header = new CommonMessageHeader(0x00, MessageType.HubProperties)
            {
                Message = message
            };

            // Act
            var result = header.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(message, result);
        }
    }
}
