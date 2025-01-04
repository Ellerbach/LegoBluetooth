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
    // TODO: fix all this, this is not working properly
    [TestClass]
    public class PortValueSingleMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 7, 0, 0x45, 1, 0x00, 0x01 }, new object[] { (byte)0x01 })]
        [DataRow(new byte[] { 9, 0, 0x45, 2, 0x01, 0x03, 0x04 }, new object[] { (ushort)0x0403 })]
        [DataRow(new byte[] { 11, 0, 0x45, 3, 0x02, 0x07, 0x08, 0x09, 0x0A }, new object[] { (uint)0x0A090807 })]
        [DataRow(new byte[] { 11, 0, 0x45, 4, 0x03, 0x00, 0x00, 0x80, 0x3F }, new object[] { 1.0f })]
        public void Decode_ValidData_ReturnsPortValueSingleMessage(byte[] data, object[] expectedValues)
        {
            // Act
            var message = PortValueSingleMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedValues.Length, message.PortValues.Count);
            for (int i = 0; i < expectedValues.Length; i++)
            {
                var portValue = (PortValueEntry)message.PortValues[i];
                Assert.AreEqual(expectedValues[i], portValue.InputValue);
            }
        }

        [TestMethod]
        [DataRow((byte)0, new object[] { (byte)0x01 }, new byte[] { 7, 0, 0x45, 0, 0x00, 0x01 })]
        [DataRow((byte)0, new object[] { (ushort)0x0403 }, new byte[] { 9, 0, 0x45, 0, 0x01, 0x03, 0x04 })]
        [DataRow((byte)0, new object[] { (uint)0x0A090807 }, new byte[] { 11, 0, 0x45, 0, 0x02, 0x07, 0x08, 0x09, 0x0A })]
        [DataRow((byte)0, new object[] { 1.0f }, new byte[] { 11, 0, 0x45, 0, 0x03, 0x00, 0x00, 0x80, 0x3F })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, object[] values, byte[] expectedData)
        {
            // Arrange
            var message = new PortValueSingleMessage(hubID);
            foreach (var value in values)
            {
                message.PortValues.Add(new PortValueEntry
                {
                    PortID = 0,
                    InputValue = value
                });
            }

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
