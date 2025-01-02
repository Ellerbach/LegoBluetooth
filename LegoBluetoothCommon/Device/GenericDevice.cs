// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System.Threading;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace LegoBluetooth
{
    public abstract class GenericDevice
    {
        /// <summary>
        /// Gets or sets the Bluetooth interface.
        /// </summary>
        internal IBluetooth Bluetooth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDevice"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public GenericDevice(IBluetooth bluetooth)
        {
            Bluetooth = bluetooth;
            bluetooth.ProcessIncoming = ProcessReceived;
            bluetooth.ClientJoiningStateChanged = ClientJoiningStateChanged;
        }

        /// <summary>
        /// Handles the client joining state change.
        /// </summary>
        /// <param name="joining">Indicates whether the client is joining.</param>
        public virtual void ClientJoiningStateChanged(bool joining)
        {
            if(joining)
            {              
                // Nothing yet
            }
            else
            {
                // Nothing yet
            }
        }

        /// <summary>
        /// Gets or sets the alerts.
        /// </summary>
        public Alert[] Alerts { get; set; } = new Alert[0];

        /// <summary>
        /// Gets or sets the Bluetooth advertising data.
        /// </summary>
        public BluetoothAdvertisingData BluetoothAdvertisingData { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the button is pressed.
        /// </summary>
        public bool Button { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public Version FWVersion { get; set; } = new Version(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        public Version HWVersion { get; set; } = new Version(0, 0, 0, 0);

        /// <summary>
        /// Processes the received data.
        /// </summary>
        /// <param name="data">The received data.</param>
        public void ProcessReceived(byte[] data)
        {
            var req = CommonMessageHeader.Decode(data);
            Debug.WriteLine($"Data: {req}");


            if (req.MessageType == MessageType.HubProperties)
            {
                var prop = (HubPropertyMessage)req;
                if (prop.Operation == HubPropertyOperation.RequestUpdate)
                {
                    var notify = new HubPropertyMessage(0, 0, prop.Property, HubPropertyOperation.Update, new byte[0]);

                    switch (prop.Property)
                    {
                        case HubProperty.AdvertisingName:
                            notify.Payload = Encoding.UTF8.GetBytes(Name);
                            break;
                        case HubProperty.Button:
                            notify.Payload = new byte[] { (byte)(Button ? 0x01 : 0x00) };
                            break;
                        case HubProperty.FWVersion:
                            notify.Payload = BitConverter.GetBytes(VersionNumberEncoder.Encode(FWVersion));
                            break;
                        case HubProperty.HWVersion:
                            notify.Payload = BitConverter.GetBytes(VersionNumberEncoder.Encode(HWVersion));
                            break;
                        case HubProperty.RSSI:
                            // Just to have a value
                            notify.Payload = new byte[] { 0xB0 };
                            break;
                        case HubProperty.BatteryVoltage:
                            // 100%
                            notify.Payload = new byte[] { 0x64 };
                            break;
                        case HubProperty.BatteryType:
                            // Normal batteries
                            notify.Payload = new byte[] { 0x00 };
                            break;
                        case HubProperty.ManufacturerName:
                            notify.Payload = Encoding.UTF8.GetBytes("LEGO System A/S");
                            break;
                        case HubProperty.RadioFirmwareVersion:
                            notify.Payload = Encoding.UTF8.GetBytes("7.2.c");
                            break;
                        case HubProperty.LEGOWirelessProtocolVersion:
                            notify.Payload = new byte[] { 0x02, 0x00 };
                            break;
                        case HubProperty.SystemTypeID:
                            notify.Payload = new byte[] { SystemTypeDeviceNumberHelper.Encode(SystemType.LegoSystem1, DeviceType.BoostHub) };
                            break;
                        case HubProperty.HWNetworkID:
                            notify.Payload = new byte[] { (byte)LastNetworkID.DisableHWNetwork };
                            break;
                        case HubProperty.PrimaryMACAddress:
                            notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAB };
                            break;
                        case HubProperty.SecondaryMACAddress:
                            notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAC };
                            break;
                        case HubProperty.HardwareNetworkFamily:
                            notify.Payload = new byte[] { 0x00 };
                            break;
                        default:
                            break;
                    }
                    
                    Bluetooth.NotifyValue(notify.ToByteArray());
                }
            }
        }
    }
}
