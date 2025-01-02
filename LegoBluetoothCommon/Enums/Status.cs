// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

[Flags]
public enum Status
{
    /// <summary>
    /// I can be Peripheral
    /// </summary>
    CanBePeripheral = 0x01,

    /// <summary>
    /// I can be Central
    /// </summary>
    CanBeCentral = 0x02,

    /// <summary>
    /// TBD
    /// </summary>
    TBD1 = 0x04,

    /// <summary>
    /// TBD
    /// </summary>
    TBD2 = 0x08,

    /// <summary>
    /// TBD
    /// </summary>
    TBD3 = 0x10,

    /// <summary>
    /// Request Window. A stretching of the Button Pressed (Adding 1 sec. after release) [part of connection process].
    /// </summary>
    RequestWindow = 0x20,

    /// <summary>
    /// Request Connect. Hardcoded request (i.e. CONSTANT flag)
    /// </summary>
    RequestConnect = 0x40,

    /// <summary>
    /// TBD
    /// </summary>
    TBD4 = 0x80
}
