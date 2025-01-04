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
    // TODO: fix all the tests and all the class, this is not working and not interpreting properly
    [TestClass]
    public class PortValueCombinedModeMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 8, 0, 0x46, 1, 0x00, 0x00, 0x00, 0x01 }, (byte)1, new object[] { (byte)0x01 })]
        [DataRow(new byte[] { 10, 0, 0x46, 2, 0x01, 0x01, 0x00, 0x03, 0x04 }, (byte)2, new object[] { (ushort)0x0403 })]
        [DataRow(new byte[] { 12, 0, 0x46, 3, 0x02, 0x02, 0x00, 0x07, 0x08, 0x09, 0x0A }, (byte)3, new object[] { (uint)0x0A090807, (uint)0x0C0B0A09 })]
        [DataRow(new byte[] { 14, 0, 0x46, 4, 0x03, 0x03, 0x00, 0x00, 0x00, 0x80, 0x3F }, (byte)4, new object[] { 1.0f })]
        public void Decode_ValidData_ReturnsPortValueCombinedModeMessage(byte[] data, byte expectedPortID, object[] expectedValues)
        {
            // Act
            var message = PortValueCombinedModeMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedValues.Length, message.PortValues.Count);
            for (int i = 0; i < expectedValues.Length; i++)
            {
                var portValue = (PortValueEntryCombined)message.PortValues[i];
                // Here we need to properly cast in order to make a proper test
                switch (portValue.BitPointer)
                {
                    case 0x00:
                        Assert.AreEqual((byte)expectedValues[i], (byte)portValue.InputValue);
                        break;
                    case 0x01:
                        Assert.AreEqual((ushort)expectedValues[i], (ushort)portValue.InputValue);
                        break;
                    case 0x02:
                        Assert.AreEqual((uint)expectedValues[i], (uint)portValue.InputValue);
                        break;
                    case 0x03:
                        Assert.AreEqual((float)expectedValues[i], (float)portValue.InputValue);
                        break;
                    default:
                        throw new Exception("Unknown bit pointer");
                }
                
            }
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, new object[] { (byte)0x01, (byte)0x02 }, new byte[] { 8, 0, 0x46, 1, 0x00, 0x00, 0x01, 0x02 })]
        [DataRow((byte)0, (byte)2, new object[] { (ushort)0x0403, (ushort)0x0605 }, new byte[] { 10, 0, 0x46, 2, 0x01, 0x00, 0x03, 0x04, 0x05, 0x06 })]
        [DataRow((byte)0, (byte)3, new object[] { (uint)0x0A090807, (uint)0x0C0B0A09 }, new byte[] { 12, 0, 0x46, 3, 0x02, 0x00, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C })]
        [DataRow((byte)0, (byte)4, new object[] { 1.0f, 2.0f }, new byte[] { 14, 0, 0x46, 4, 0x03, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x40 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, object[] values, byte[] expectedData)
        {
            // Arrange
            var message = new PortValueCombinedModeMessage(hubID, portID);
            foreach (var value in values)
            {
                message.PortValues.Add(new PortValueEntryCombined
                {
                    BitPointer = (ushort)(message.PortValues.Count),
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
