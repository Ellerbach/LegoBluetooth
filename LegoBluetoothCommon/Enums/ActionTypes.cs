// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace LegoBluetooth
{
    /// <summary>
    /// Represents the various Action Types for the LEGO Hub.
    /// </summary>
    public enum ActionType
    {
        // Downstream (reserved 0-0x2F)

        /// <summary>
        /// Switch Off Hub.
        /// </summary>
        SwitchOffHub = 0x01,

        /// <summary>
        /// Disconnect.
        /// </summary>
        Disconnect = 0x02,

        /// <summary>
        /// VCC Port Control On.
        /// </summary>
        VCCPortControlOn = 0x03,

        /// <summary>
        /// VCC Port Control Off.
        /// </summary>
        VCCPortControlOff = 0x04,

        /// <summary>
        /// Activate BUSY Indication (Shown by RGB. Actual RGB settings preserved).
        /// </summary>
        ActivateBusyIndication = 0x05,

        /// <summary>
        /// Reset BUSY Indication (RGB shows the previously preserved RGB settings).
        /// </summary>
        ResetBusyIndication = 0x06,

        // ... (other downstream action types can be added here)

        /// <summary>
        /// Shutdown the Hub without any up-stream information sent. Used for fast power down in production. Suggested for PRODUCTION USE ONLY!
        /// </summary>
        ShutdownHubFast = 0x2F,

        // Upstream (reserved 0x30-0x64)

        /// <summary>
        /// Hub Will Switch Off.
        /// </summary>
        HubWillSwitchOff = 0x30,

        /// <summary>
        /// Hub Will Disconnect.
        /// </summary>
        HubWillDisconnect = 0x31,

        /// <summary>
        /// Hub Will Go Into Boot Mode.
        /// </summary>
        HubWillGoIntoBootMode = 0x32
    }
}


