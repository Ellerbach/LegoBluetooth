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
    public class PortValueSingleMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 5, 0, 0x45, 1, 0x01 }, new byte[] { 1, 0x01 })]
        [DataRow(new byte[] { 6, 0, 0x45, 2, 0x03, 0x04 }, new byte[] { 2, 0x03, 0x04 })]
        [DataRow(new byte[] { 8, 0, 0x45, 3, 0x07, 0x08, 0x09, 0x0A }, new byte[] { 3, 0x07, 0x08, 0x09, 0x0A })]
        [DataRow(new byte[] { 8, 0, 0x45, 4, 0x00, 0x00, 0x80, 0x3F }, new byte[] { 4, 0x00, 0x00, 0x80, 0x3F })]
        public void Decode_ValidData_ReturnsPortValueSingleMessage(byte[] data, byte[] expectedPayload)
        {
            // Act
            var message = PortValueSingleMessage.Decode(data);

            // Assert
            // Decode stores raw payload (PortID + value bytes) since value
            // parsing requires knowing the mode's ValueFormat from prior setup.
            Assert.AreEqual((ushort)data[0], message.Length);
            Assert.AreEqual(0, message.PortValues.Count);
            CollectionAssert.AreEqual(expectedPayload, message.Payload);
        }

        [TestMethod]
        public void ToByteArray_ByteValue_ReturnsByteArray()
        {
            var message = new PortValueSingleMessage(0);
            message.PortValues.Add(new PortValueEntry { PortID = 0, InputValue = (byte)0x01 });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 5, 0, 0x45, 0, 0x01 }, data);
        }

        [TestMethod]
        public void ToByteArray_UshortValue_ReturnsByteArray()
        {
            var message = new PortValueSingleMessage(0);
            message.PortValues.Add(new PortValueEntry { PortID = 0, InputValue = (ushort)0x0403 });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 6, 0, 0x45, 0, 0x03, 0x04 }, data);
        }

        [TestMethod]
        public void ToByteArray_UintValue_ReturnsByteArray()
        {
            var message = new PortValueSingleMessage(0);
            message.PortValues.Add(new PortValueEntry { PortID = 0, InputValue = (uint)0x0A090807 });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 8, 0, 0x45, 0, 0x07, 0x08, 0x09, 0x0A }, data);
        }

        [TestMethod]
        public void ToByteArray_FloatValue_ReturnsByteArray()
        {
            var message = new PortValueSingleMessage(0);
            message.PortValues.Add(new PortValueEntry { PortID = 0, InputValue = 1.0f });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 8, 0, 0x45, 0, 0x00, 0x00, 0x80, 0x3F }, data);
        }
    }
}
