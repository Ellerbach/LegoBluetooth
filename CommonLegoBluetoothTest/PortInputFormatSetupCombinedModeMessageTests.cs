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
    public class PortInputFormatSetupCombinedModeMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 7, 0, 0x42, 1, 1, 2, 3 }, (byte)1, (byte)PortInputFormatSetupSubCommand.SetModeAndDataSetCombinations, (byte)2, new byte[] { 3 })]
        [DataRow(new byte[] { 5, 0, 0x42, 2, 2 }, (byte)2, (byte)PortInputFormatSetupSubCommand.LockDeviceForSetup, (byte)0, new byte[0])]
        public void Decode_ValidData_ReturnsPortInputFormatSetupCombinedModeMessage(byte[] data, byte expectedPortID, byte expectedSubCommand, byte expectedCombinationIndex, byte[] expectedModeDataSetCombinations)
        {
            // Act
            var message = PortInputFormatSetupCombinedModeMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedSubCommand, (byte)message.SubCommand);
            Assert.AreEqual(expectedCombinationIndex, message.CombinationIndex);
            if(expectedModeDataSetCombinations.Length != 0)
            {
                CollectionAssert.AreEqual(expectedModeDataSetCombinations, message.ModeDataSetCombinations);
            }            
        }        

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)PortInputFormatSetupSubCommand.SetModeAndDataSetCombinations, (byte)2, new byte[] { 3 }, new byte[] { 7, 0, 0x42, 1, 1, 2, 3 })]
        [DataRow((byte)0, (byte)2, (byte)PortInputFormatSetupSubCommand.LockDeviceForSetup, (byte)0, new byte[0], new byte[] { 5, 0, 0x42, 2, 2 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte subCommand, byte combinationIndex, byte[] modeDataSetCombinations, byte[] expectedData)
        {
            // Arrange
            var message = new PortInputFormatSetupCombinedModeMessage(hubID, portID, (PortInputFormatSetupSubCommand)subCommand, combinationIndex, modeDataSetCombinations);

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
