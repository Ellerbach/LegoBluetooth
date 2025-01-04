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
    public class HWNetworkMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 0x05, 0x01, 0x08, 0x02, 0x01 }, (byte)HWNetworkCommandType.ConnectionRequest, (byte)0x01)]
        [DataRow(new byte[] { 0x04, 0x01, 0x08, 0x03 }, (byte)HWNetworkCommandType.FamilyRequest, (byte)0x00)]
        public void Decode_ShouldReturnCorrectHWNetworkMessage(byte[] data, byte expectedCommandType, byte expectedPayload)
        {
            // Act
            var message = HWNetworkMessage.Decode(data);

            // Assert
            Assert.IsNotNull(message);
            Assert.AreEqual(expectedCommandType, (byte)message.CommandType);
            Assert.AreEqual(expectedPayload, message.Payload);
        }

        [TestMethod]
        [DataRow(new byte[] { 0x03, 0x01, 0x08 }, typeof(ArgumentException))]
        //[DataRow(null, typeof(ArgumentException))]
        public void Decode_ShouldThrowExceptionForInvalidData(byte[] data, Type expectedExceptionType)
        {
            // Act & Assert
            try
            {
                HWNetworkMessage.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, expectedExceptionType); ;
            }            
        }

        [TestMethod]
        [DataRow((byte)0x01, (byte)HWNetworkCommandType.ConnectionRequest, (byte)0x01, new byte[] { 0x05, 0x01, 0x08, 0x02, 0x01 })]
        [DataRow((byte)0x01, (byte)HWNetworkCommandType.FamilyRequest, (byte)0x00, new byte[] { 0x04, 0x01, 0x08, 0x03 })]
        public void ToByteArray_ShouldReturnCorrectByteArray(byte hubID, byte commandType, byte payload, byte[] expectedData)
        {
            // Arrange
            var message = new HWNetworkMessage(hubID, (HWNetworkCommandType)commandType, payload);

            // Act
            var result = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, result);
        }       
    }
}
