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
    public class ErrorMessageTests
    {
        [TestMethod]
        [DataRow((byte)0x01, (byte)0x02, (byte)ErrorCode.ACK)]
        [DataRow((byte)0x03, (byte)0x04, (byte)ErrorCode.BufferOverflow)]
        public void Constructor_ShouldInitializeProperties(byte hubID, byte commandType, byte errorCode)
        {
            // Act
            var errorMessage = new ErrorMessage(hubID, commandType, (ErrorCode)errorCode);

            // Assert
            Assert.AreEqual(hubID, errorMessage.HubID);
            Assert.AreEqual(commandType, errorMessage.CommandType);
            Assert.AreEqual(errorCode, (byte)errorMessage.ErrorCode);
            Assert.AreEqual((ushort)5, errorMessage.Length);
        }

        [TestMethod]
        //[DataRow(null)]
        [DataRow(new byte[] { 0x01, 0x02, 0x03, 0x04 })]
        public void Decode_ShouldThrowException_WhenDataIsInvalid(byte[] data)
        {
            // Act & Assert
            try
            {
                ErrorMessage.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        [DataRow(new byte[] { 0x05, 0x01, 0x05, 0x02, 0x03 }, (byte)0x01, (byte)0x02, (byte)ErrorCode.BufferOverflow)]
        public void Decode_ShouldReturnErrorMessage_WhenDataIsValid(byte[] data, byte expectedHubID, byte expectedCommandType, byte expectedErrorCode)
        {
            // Act
            var errorMessage = ErrorMessage.Decode(data);

            // Assert
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(expectedHubID, errorMessage.HubID);
            Assert.AreEqual(expectedCommandType, errorMessage.CommandType);
            Assert.AreEqual(expectedErrorCode, (byte)errorMessage.ErrorCode);
            Assert.AreEqual((ushort)5, errorMessage.Length);
        }

        [TestMethod]
        [DataRow((byte)0x01, (byte)0x02, (byte)ErrorCode.ACK, new byte[] { 0x05, 0x01, 0x05, 0x02, 0x01 })]
        public void ToByteArray_ShouldReturnByteArray(byte hubID, byte commandType, byte errorCode, byte[] expectedData)
        {
            // Arrange
            var errorMessage = new ErrorMessage(hubID, commandType, (ErrorCode)errorCode);

            // Act
            var result = errorMessage.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, result);
        }
    }
}
