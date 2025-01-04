// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    public abstract class GenericDevice
    {
        private readonly IBluetooth _ble;

        public byte PortID { get; internal set; }

        public byte HubID { get; internal set; }

        public GenericDevice(IBluetooth ble, byte hubID, byte portID)
        {
            _ble = ble ?? throw new ArgumentNullException();
            HubID = hubID;
            PortID = portID;
        }
    }
}
