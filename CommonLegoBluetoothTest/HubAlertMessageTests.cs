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
    public class HubAlertMessageTests
    {
        [TestMethod]
        [DataRow((byte)0x01, (byte)HubAlertType.LowVoltage, (byte)HubAlertOperation.EnableUpdates, (byte)HubAlertPayload.StatusOK)]
        [DataRow((byte)0x02, (byte)HubAlertType.HighCurrent, (byte)HubAlertOperation.Update, (byte)HubAlertPayload.Alert)]
        public void Constructor_ShouldInitializeProperties(byte hubID, byte alertType, byte alertOperation, byte alertPayload)
        {
            // Act
            var hubAlertMessage = new HubAlertMessage(hubID, (HubAlertType)alertType, (HubAlertOperation)alertOperation, (HubAlertPayload)alertPayload);

            // Assert
            Assert.AreEqual(hubID, hubAlertMessage.HubID);
            Assert.AreEqual(alertType, (byte)hubAlertMessage.AlertType);
            Assert.AreEqual(alertOperation, (byte)hubAlertMessage.AlertOperation);
            Assert.AreEqual(alertPayload, (byte)hubAlertMessage.AlertPayload);
            Assert.AreEqual((ushort)((HubAlertOperation)alertOperation == HubAlertOperation.Update ? 6 : 5), hubAlertMessage.Length);
        }

        [TestMethod]
        //[DataRow(null)]
        [DataRow(new byte[] { 0x01, 0x02, 0x03, 0x04 })]
        public void Decode_ShouldThrowException_WhenDataIsInvalid(byte[] data)
        {
            // Act & Assert
            try
            {
                HubAlertMessage.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        [DataRow(new byte[] { 0x06, 0x01, 0x03, 0x01, 0x04, 0x00 }, (byte)0x01, (byte)HubAlertType.LowVoltage, (byte)HubAlertOperation.Update, (byte)HubAlertPayload.StatusOK)]
        [DataRow(new byte[] { 0x05, 0x02, 0x03, 0x02, 0x02 }, (byte)0x02, (byte)HubAlertType.HighCurrent, (byte)HubAlertOperation.DisableUpdates, (byte)HubAlertPayload.StatusOK)]
        public void Decode_ShouldReturnHubAlertMessage_WhenDataIsValid(byte[] data, byte expectedHubID, byte expectedAlertType, byte expectedAlertOperation, byte expectedAlertPayload)
        {
            // Act
            var hubAlertMessage = HubAlertMessage.Decode(data);

            // Assert
            Assert.IsNotNull(hubAlertMessage);
            Assert.AreEqual(expectedHubID, hubAlertMessage.HubID);
            Assert.AreEqual(expectedAlertType, (byte)hubAlertMessage.AlertType);
            Assert.AreEqual(expectedAlertOperation, (byte)hubAlertMessage.AlertOperation);
            Assert.AreEqual(expectedAlertPayload, (byte)hubAlertMessage.AlertPayload);
            Assert.AreEqual(data[0], hubAlertMessage.Length);
        }

        [TestMethod]
        [DataRow((byte)0x01, (byte)HubAlertType.LowVoltage, (byte)HubAlertOperation.Update, (byte)HubAlertPayload.StatusOK, new byte[] { 0x06, 0x01, 0x03, 0x01, 0x04, 0x00 })]
        [DataRow((byte)0x02, (byte)HubAlertType.HighCurrent, (byte)HubAlertOperation.DisableUpdates, (byte)HubAlertPayload.StatusOK, new byte[] { 0x05, 0x02, 0x03, 0x02, 0x02 })]
        public void ToByteArray_ShouldReturnByteArray(byte hubID, byte alertType, byte alertOperation, byte alertPayload, byte[] expectedData)
        {
            // Arrange
            var hubAlertMessage = new HubAlertMessage(hubID, (HubAlertType)alertType, (HubAlertOperation)alertOperation, (HubAlertPayload)alertPayload);

            // Act
            var result = hubAlertMessage.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, result);
        }
    }
}
