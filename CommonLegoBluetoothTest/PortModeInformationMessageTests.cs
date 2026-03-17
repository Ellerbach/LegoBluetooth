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
    public class PortModeInformationMessageTests
    {
        [TestMethod]
        [DataRow(new byte[] { 17, 0, 0x44, 1, 2, 0, 78, 97, 109, 101, 0, 0, 0, 0, 0, 0, 0 }, (byte)1, (byte)2, (byte)ModeInformationType.Name, "Name", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 14, 0, 0x44, 2, 3, 1, 0, 0, 128, 63, 0, 0, 0, 64 }, (byte)2, (byte)3, (byte)ModeInformationType.Raw, "", 1f, 2f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 14, 0, 0x44, 3, 4, 2, 0, 0, 128, 63, 0, 0, 0, 64 }, (byte)3, (byte)4, (byte)ModeInformationType.Percent, "", 0f, 0f, 1f, 2f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 14, 0, 0x44, 4, 5, 3, 0, 0, 128, 63, 0, 0, 0, 64 }, (byte)4, (byte)5, (byte)ModeInformationType.SI, "", 0f, 0f, 0f, 0f, 1f, 2f, "", (ushort)0, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 11, 0, 0x44, 5, 6, 4, 83, 121, 109, 98, 0 }, (byte)5, (byte)6, (byte)ModeInformationType.Symbol, "", 0f, 0f, 0f, 0f, 0f, 0f, "Symb", (ushort)0, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 8, 0, 0x44, 6, 7, 5, 1, 0 }, (byte)6, (byte)7, (byte)ModeInformationType.Mapping, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)1, (byte)0, new byte[0], new byte[0])]
        [DataRow(new byte[] { 7, 0, 0x44, 7, 8, 7, 9 }, (byte)7, (byte)8, (byte)ModeInformationType.MotorBias, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)9, new byte[0], new byte[0])]
        [DataRow(new byte[] { 12, 0, 0x44, 8, 9, 8, 1, 2, 3, 4, 5, 6 }, (byte)8, (byte)9, (byte)ModeInformationType.CapabilityBits, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[] { 1, 2, 3, 4, 5, 6 }, new byte[0])]
        [DataRow(new byte[] { 10, 0, 0x44, 9, 10, 128, 1, 2, 3, 4 }, (byte)9, (byte)10, (byte)ModeInformationType.ValueFormat, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[] { 1, 2, 3, 4 })]
        public void Decode_ValidData_ReturnsPortModeInformationMessage(byte[] data, byte expectedPortID, byte expectedMode, byte expectedModeInformationType, string expectedName, float expectedRawMin, float expectedRawMax, float expectedPctMin, float expectedPctMax, float expectedSiMin, float expectedSiMax, string expectedSymbol, ushort expectedMapping, byte expectedMotorBias, byte[] expectedCapabilityBits, byte[] expectedValueFormat)
        {
            // Act
            var message = PortModeInformationMessage.Decode(data);

            // Assert
            Assert.AreEqual(expectedPortID, message.PortID);
            Assert.AreEqual(expectedMode, message.Mode);
            Assert.AreEqual(expectedModeInformationType, (byte)message.ModeInformationType);
            Assert.AreEqual(expectedName, message.Name);
            Assert.AreEqual(expectedRawMin, message.RawMin);
            Assert.AreEqual(expectedRawMax, message.RawMax);
            Assert.AreEqual(expectedPctMin, message.PctMin);
            Assert.AreEqual(expectedPctMax, message.PctMax);
            Assert.AreEqual(expectedSiMin, message.SiMin);
            Assert.AreEqual(expectedSiMax, message.SiMax);
            Assert.AreEqual(expectedSymbol, message.Symbol);
            Assert.AreEqual(expectedMapping, message.Mapping);
            Assert.AreEqual(expectedMotorBias, message.MotorBias);
            if (expectedCapabilityBits.Length > 0)
            {
                CollectionAssert.AreEqual(expectedCapabilityBits, message.CapabilityBits);
            }

            if (expectedValueFormat.Length > 0)
            {
                CollectionAssert.AreEqual(expectedValueFormat, message.ValueFormat);
            }
        }

        [TestMethod]
        [DataRow((byte)0, (byte)1, (byte)2, (byte)ModeInformationType.Name, "Name", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0], new byte[] { 17, 0, 0x44, 1, 2, 0, 78, 97, 109, 101, 0, 0, 0, 0, 0, 0, 0 })]
        [DataRow((byte)0, (byte)2, (byte)3, (byte)ModeInformationType.Raw, "", 1f, 2f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0], new byte[] { 14, 0, 0x44, 2, 3, 1, 0, 0, 128, 63, 0, 0, 0, 64 })]
        [DataRow((byte)0, (byte)3, (byte)4, (byte)ModeInformationType.Percent, "", 0f, 0f, 1f, 2f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[0], new byte[] { 14, 0, 0x44, 3, 4, 2, 0, 0, 128, 63, 0, 0, 0, 64 })]
        [DataRow((byte)0, (byte)4, (byte)5, (byte)ModeInformationType.SI, "", 0f, 0f, 0f, 0f, 1f, 2f, "", (ushort)0, (byte)0, new byte[0], new byte[0], new byte[] { 14, 0, 0x44, 4, 5, 3, 0, 0, 128, 63, 0, 0, 0, 64 })]
        [DataRow((byte)0, (byte)5, (byte)6, (byte)ModeInformationType.Symbol, "", 0f, 0f, 0f, 0f, 0f, 0f, "Symb", (ushort)0, (byte)0, new byte[0], new byte[0], new byte[] { 11, 0, 0x44, 5, 6, 4, 83, 121, 109, 98, 0 })]
        [DataRow((byte)0, (byte)6, (byte)7, (byte)ModeInformationType.Mapping, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)1, (byte)0, new byte[0], new byte[0], new byte[] { 8, 0, 0x44, 6, 7, 5, 1, 0 })]
        [DataRow((byte)0, (byte)7, (byte)8, (byte)ModeInformationType.MotorBias, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)9, new byte[0], new byte[0], new byte[] { 7, 0, 0x44, 7, 8, 7, 9 })]
        [DataRow((byte)0, (byte)8, (byte)9, (byte)ModeInformationType.CapabilityBits, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[] { 1, 2, 3, 4, 5, 6 }, new byte[0], new byte[] { 12, 0, 0x44, 8, 9, 8, 1, 2, 3, 4, 5, 6 })]
        [DataRow((byte)0, (byte)9, (byte)10, (byte)ModeInformationType.ValueFormat, "", 0f, 0f, 0f, 0f, 0f, 0f, "", (ushort)0, (byte)0, new byte[0], new byte[] { 1, 2, 3, 4 }, new byte[] { 10, 0, 0x44, 9, 10, 128, 1, 2, 3, 4 })]
        public void ToByteArray_ValidData_ReturnsByteArray(byte hubID, byte portID, byte mode, byte modeInformationType, string name, float rawMin, float rawMax, float pctMin, float pctMax, float siMin, float siMax, string symbol, ushort mapping, byte motorBias, byte[] capabilityBits, byte[] valueFormat, byte[] expectedData)
        {
            // Arrange
            var message = new PortModeInformationMessage(hubID, portID, mode, (ModeInformationType)modeInformationType)
            {
                Name = name,
                RawMin = rawMin,
                RawMax = rawMax,
                PctMin = pctMin,
                PctMax = pctMax,
                SiMin = siMin,
                SiMax = siMax,
                Symbol = symbol,
                Mapping = mapping,
                MotorBias = motorBias,
                CapabilityBits = capabilityBits,
                ValueFormat = valueFormat
            };

            // Act
            var data = message.ToByteArray();

            // Assert
            CollectionAssert.AreEqual(expectedData, data);
        }
    }
}
