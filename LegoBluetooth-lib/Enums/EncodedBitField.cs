using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the different encoded bit-field values.
    /// </summary>
    [Flags]
    public enum EncodedBitField
    {
        /// <summary>
        /// “I can be Peripheral”.
        /// </summary>
        CanBePeripheral = 0b0000_0001,

        /// <summary>
        /// “I can be Central”.
        /// </summary>
        CanBeCentral = 0b0000_0010,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD1 = 0b0000_0100,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD2 = 0b0000_1000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD3 = 0b0001_0000,

        /// <summary>
        /// “Request Window”. A stretching of the Button Pressed (Adding 1 sec. after release) [part of connection process].
        /// </summary>
        RequestWindow = 0b0010_0000,

        /// <summary>
        /// “Request Connect”. Hardcoded request (i.e. CONSTANT flag).
        /// </summary>
        RequestConnect = 0b0100_0000,

        /// <summary>
        /// TBD (To Be Determined).
        /// </summary>
        TBD4 = 0b1000_0000
    }
}

