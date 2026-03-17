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
    public class PortValueCombinedModeMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 7, 0, 0x46, 1, 0x01, 0x00, 0x01 }, (byte)1, (ushort)0x0001, new byte[] { 0x01 })]
        [DataRow(new byte[] { 8, 0, 0x46, 2, 0x02, 0x00, 0x03, 0x04 }, (byte)2, (ushort)0x0002, new byte[] { 0x03, 0x04 })]
        [DataRow(new byte[] { 10, 0, 0x46, 3, 0x04, 0x00, 0x07, 0x08, 0x09, 0x0A }, (byte)3, (ushort)0x0004, new byte[] { 0x07, 0x08, 0x09, 0x0A })]
        [DataRow(new byte[] { 10, 0, 0x46, 4, 0x08, 0x00, 0x00, 0x00, 0x80, 0x3F }, (byte)4, (ushort)0x0008, new byte[] { 0x00, 0x00, 0x80, 0x3F })]
        public void Decode_ValidData_ReturnsPortValueCombinedModeMessage(byte[] data, byte expectedPortID, ushort expectedBitPointer, byte[] expectedPayload)
        {
            // Act
            var message = PortValueCombinedModeMessage.Decode(data);

            // Assert
            // Decode stores raw payload since value parsing requires knowing
            // the mode's ValueFormat from prior setup.
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedBitPointer, message.BitPointer);
            Assert.AreEqual((ushort)data[0], message.Length);
            Assert.AreEqual(0, message.PortValues.Count);
            CollectionAssert.AreEqual(expectedPayload, message.Payload);
        }

        [TestMethod]
        public void ToByteArray_ByteValues_ReturnsByteArray()
        {
            var message = new PortValueCombinedModeMessage(0, 1);
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 0, InputValue = (byte)0x01 });
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 1, InputValue = (byte)0x02 });
            var data = message.ToByteArray();
            // bitmask: bit0 | bit1 = 0x03
            CollectionAssert.AreEqual(new byte[] { 8, 0, 0x46, 1, 0x03, 0x00, 0x01, 0x02 }, data);
        }

        [TestMethod]
        public void ToByteArray_UshortValues_ReturnsByteArray()
        {
            var message = new PortValueCombinedModeMessage(0, 2);
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 0, InputValue = (ushort)0x0403 });
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 1, InputValue = (ushort)0x0605 });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 10, 0, 0x46, 2, 0x03, 0x00, 0x03, 0x04, 0x05, 0x06 }, data);
        }

        [TestMethod]
        public void ToByteArray_UintValues_ReturnsByteArray()
        {
            var message = new PortValueCombinedModeMessage(0, 3);
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 0, InputValue = (uint)0x0A090807 });
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 1, InputValue = (uint)0x0C0B0A09 });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 14, 0, 0x46, 3, 0x03, 0x00, 0x07, 0x08, 0x09, 0x0A, 0x09, 0x0A, 0x0B, 0x0C }, data);
        }

        [TestMethod]
        public void ToByteArray_FloatValues_ReturnsByteArray()
        {
            var message = new PortValueCombinedModeMessage(0, 4);
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 0, InputValue = 1.0f });
            message.PortValues.Add(new PortValueEntryCombined { BitPointer = 1, InputValue = 2.0f });
            var data = message.ToByteArray();
            CollectionAssert.AreEqual(new byte[] { 14, 0, 0x46, 4, 0x03, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x40 }, data);
        }
    }
}
