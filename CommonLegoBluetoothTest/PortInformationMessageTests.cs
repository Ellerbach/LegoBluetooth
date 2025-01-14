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
    public class PortInformationMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 11, 0, 0x43, 1, 1, 2, 3, 0, 1, 0, 2 }, (byte)1, (byte)InformationType.ModeInfo, (byte)2, (byte)3, (ushort)256, (ushort)512, new byte[0])]
        [DataRow(new byte[] { 11, 0, 0x43, 2, 1, 3, 4, 1, 0, 2, 0 }, (byte)2, (byte)InformationType.ModeInfo, (byte)3, (byte)4, (ushort)1, (ushort)2, new byte[0])]
        [DataRow(new byte[] { 9, 0, 0x43, 3, 2, 1, 0, 2, 0 }, (byte)3, (byte)InformationType.PossibleModeCombinations, (byte)0, (byte)0, (ushort)0, (ushort)0, new byte[] { 1, 0, 2, 0 })]
        public void Decode_ValidData_ReturnsPortInformationMessage(byte[] data, byte expectedPortID, byte expectedInformationType, byte expectedCapabilities, byte expectedTotalModeCount, ushort expectedInputModes, ushort expectedOutputModes, byte[] expectedModeCombinations)
        {
            // Act
            var message = PortInformationMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedInformationType, (byte)message.InformationType);
            Assert.AreEqual(expectedCapabilities, (byte)message.Capabilities);
            Assert.AreEqual(expectedTotalModeCount, message.TotalModeCount);
            Assert.AreEqual(expectedInputModes, message.InputModes);
            Assert.AreEqual(expectedOutputModes, message.OutputModes);
            if (expectedModeCombinations.Length != 0)
            {
                CollectionAssert.AreEqual(expectedModeCombinations, message.ModeCombinations);
            }
        }

        [TestMethod]
        //[DataRow(null)]
        [DataRow(new byte[] { })]
        [DataRow(new byte[] { 1, 2, 3 })]
        public void Decode_InvalidData_ThrowsArgumentException(byte[] data)
        {
            try
            {
                // Act
                PortInformationMessage.Decode(data);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)InformationType.ModeInfo, (byte)2, (byte)3, (ushort)256, (ushort)512, new byte[0], new byte[] { 11, 0, 0x43, 1, 1, 2, 3, 0, 1, 0, 2 })]
        [DataRow((byte)0, (byte)2, (byte)InformationType.ModeInfo, (byte)3, (byte)4, (ushort)1, (ushort)2, new byte[0], new byte[] { 11, 0, 0x43, 2, 1, 3, 4, 1, 0, 2, 0 })]
        [DataRow((byte)0, (byte)3, (byte)InformationType.PossibleModeCombinations, (byte)0, (byte)0, (ushort)0, (ushort)0, new byte[] { 1, 0, 2, 0 }, new byte[] { 9, 0, 0x43, 3, 2, 1, 0, 2, 0 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte informationType, byte capabilities, byte totalModeCount, ushort inputModes, ushort outputModes, byte[] modeCombinations, byte[] expectedData)
        {
            // Arrange
            var message = new PortInformationMessage(hubID, portID, (InformationType)informationType, (Capabilities)capabilities, totalModeCount, inputModes, outputModes, modeCombinations);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
