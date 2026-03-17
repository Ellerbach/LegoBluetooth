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
        [DataRow(new byte[] { 11, 0, 0x43, 1, 1, 2, 3, 0, 1, 0, 2 }, (byte)1, (byte)InformationType.ModeInfo, (byte)2, (byte)3, (ushort)256, (ushort)512)]
        [DataRow(new byte[] { 11, 0, 0x43, 2, 1, 3, 4, 1, 0, 2, 0 }, (byte)2, (byte)InformationType.ModeInfo, (byte)3, (byte)4, (ushort)1, (ushort)2)]
        public void Decode_ModeInfo_ReturnsPortInformationMessage(byte[] data, byte expectedPortID, byte expectedInformationType, byte expectedCapabilities, byte expectedTotalModeCount, ushort expectedInputModes, ushort expectedOutputModes)
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
        }

        [TestMethod]
        public void Decode_PossibleModeCombinations_ReturnsCorrectCombinations()
        {
            // Arrange — wire: [length, hubID, 0x43, portID, infoType, combo0_lo, combo0_hi, combo1_lo, combo1_hi]
            byte[] data = new byte[] { 9, 0, 0x43, 3, 2, 1, 0, 2, 0 };

            // Act
            var message = PortInformationMessage.Decode(data);

            // Assert
            Assert.AreEqual((byte)3, message.PortID);
            Assert.AreEqual((byte)InformationType.PossibleModeCombinations, (byte)message.InformationType);
            Assert.AreEqual(2, message.ModeCombinations.Length);
            Assert.AreEqual((ushort)1, message.ModeCombinations[0]);
            Assert.AreEqual((ushort)2, message.ModeCombinations[1]);
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
        [DataRow((byte)0, (byte)1, (byte)InformationType.ModeInfo, (byte)2, (byte)3, (ushort)256, (ushort)512, new byte[] { 11, 0, 0x43, 1, 1, 2, 3, 0, 1, 0, 2 })]
        [DataRow((byte)0, (byte)2, (byte)InformationType.ModeInfo, (byte)3, (byte)4, (ushort)1, (ushort)2, new byte[] { 11, 0, 0x43, 2, 1, 3, 4, 1, 0, 2, 0 })]
        public void ToByteArray_ModeInfo_ReturnsByteArray(byte hubID, byte portID, byte informationType, byte capabilities, byte totalModeCount, ushort inputModes, ushort outputModes, byte[] expectedData)
        {
            // Arrange
            var message = new PortInformationMessage(hubID, portID, (InformationType)informationType, (Capabilities)capabilities, totalModeCount, inputModes, outputModes, null);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }

        [TestMethod]
        public void ToByteArray_PossibleModeCombinations_ReturnsByteArray()
        {
            // Arrange
            var message = new PortInformationMessage(0, 3, InformationType.PossibleModeCombinations, 0, 0, 0, 0, new ushort[] { 1, 2 });

            // Act
            var data = message.ToByteArray();

            // Assert
            byte[] expectedData = new byte[] { 9, 0, 0x43, 3, 2, 1, 0, 2, 0 };
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
