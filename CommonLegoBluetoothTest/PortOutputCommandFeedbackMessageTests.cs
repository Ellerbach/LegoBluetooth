// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Collections;
using LegoBluetooth;
#if NANOFRAMEWORK_1_0
using nanoFramework.TestFramework;
#endif

namespace CommonLegoBluetoothTest
{
    [TestClass]
    public class PortOutputCommandFeedbackMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 5, 0, 0x82, 1, 2, }, new byte[] { 1, 2 })]
        [DataRow(new byte[] { 7, 0, 0x82, 2, 3, 4, 5 }, new byte[] { 2, 3, 4, 5 })]
        public void Decode_ValidData_ReturnsPortOutputCommandFeedbackMessage(byte[] data, byte[] expectedFeedbacks)
        {
            // Act
            var message = PortOutputCommandFeedbackMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedFeedbacks.Length / 2, message.PortFeedbacks.Count);
            for (int i = 0; i < expectedFeedbacks.Length; i += 2)
            {
                var feedback = (PortFeedbackEntry)message.PortFeedbacks[i / 2];
                Assert.AreEqual(expectedFeedbacks[i], feedback.PortID);
                Assert.AreEqual((FeedbackMessage)expectedFeedbacks[i + 1], feedback.Feedback);
            }
        }

        [TestMethod]
        [DataRow((byte)0, new byte[] { 1, 2 }, new byte[] { 5, 0, 0x82, 1, 2 })]
        [DataRow((byte)0, new byte[] { 2, 3, 4, 5 }, new byte[] { 7, 0, 0x82, 2, 3, 4, 5 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte[] feedbacks, byte[] expectedData)
        {
            // Arrange
            var message = new PortOutputCommandFeedbackMessage(hubID);
            for (int i = 0; i < feedbacks.Length; i += 2)
            {
                message.PortFeedbacks.Add(new PortFeedbackEntry
                {
                    PortID = feedbacks[i],
                    Feedback = (FeedbackMessage)feedbacks[i + 1]
                });
            }

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
