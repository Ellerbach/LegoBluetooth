// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System.Threading;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using LegoBluetooth.Device;

namespace LegoBluetooth
{
    public abstract class BaseHub
    {
        /// <summary>
        /// Gets or sets the Bluetooth interface.
        /// </summary>
        internal IBluetooth Bluetooth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHub"/> class.
        /// </summary>
        /// <param name="bluetooth">The Bluetooth interface.</param>
        public BaseHub(IBluetooth bluetooth)
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
            if (joining)
            {
                // Send the device elements
                foreach (BaseDevice dev in Devices)
                {
                    var res = AttachDevice(dev.PortID);
                    if (res != null)
                    {
                        Bluetooth.NotifyValue(res);
                    }
                }
            }
            else
            {
                // Nothing yet
            }
        }

        /// <summary>
        /// Gets or sets the alerts. This is actually a list of <see cref="Alert"/>.
        /// </summary>
        public ArrayList Alerts { get; set; } = new ArrayList();

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
        /// Gets or sets the LEGO Wireless Protocol version.
        /// </summary>
        public Version LEGOWirelessProtocolVersion { get; set; } = new Version(3, 0, 0, 0);

        /// <summary>
        /// Gets or sets the hardware network ID.
        /// </summary>
        public LastNetworkID HWNetworkID { get; set; } = LastNetworkID.DisableHWNetwork;

        /// <summary>
        /// Gets or sets the system type ID.
        /// </summary>
        public byte SystemTypeID { get; set; } = SystemTypeDeviceNumberHelper.Encode(SystemType.LegoWedo20, DeviceType.WeDoHub);

        /// <summary>
        /// Gets or sets the battery voltage.
        /// </summary>
        public byte BatteryVoltage { get; set; } = 100;

        /// <summary>
        /// Gets or sets the battery type.
        /// </summary>
        public BatteryType BatteryType { get; set; } = BatteryType.Normal;

        /// <summary>
        /// Gets or sets the manufacturer name.
        /// </summary>
        public string ManufacturerName { get; set; } = "LEGO System A/S";

        /// <summary>
        /// Gets or sets the radio firmware version.
        /// </summary>
        public string RadioFirmwareVersion { get; set; } = "7.2.c";

        /// <summary>
        /// Gets or sets the hardware network family.
        /// </summary>
        public HWNetworkFamily hWNetworkFamily { get; set; } = HWNetworkFamily.White;

        /// <summary>
        /// Gets or sets the devices. They have to be <see cref="BaseDevice"/>.
        /// </summary>
        public ArrayList Devices { get; internal set; } = new ArrayList();

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
                ProcessHubProperties(prop);
            }
            else if (req.MessageType == MessageType.HubAlerts)
            {
                var alert = (HubAlertMessage)req;
                ProcessAlert(alert);
            }
            else if ((req.MessageType == MessageType.PortInformation) || (req.MessageType == MessageType.PortModeInformation))
            {
                ProcessPortInformation(req);
            }
            else if (req.MessageType == MessageType.PortInformationRequest)
            {
                ProcessPortInformationRequest((PortInformationRequestMessage)req);
            }
        }

        /// <summary>
        /// Hydrates the modes for all devices.
        /// </summary>
        public void HydrateModes()
        {
            foreach (BaseDevice device in Devices)
            {
                device.HydrateModes();
            }
        }

        /// <summary>
        /// Processes the port information request.
        /// </summary>
        /// <param name="req">The port information request message.</param>
        internal void ProcessPortInformationRequest(PortInformationRequestMessage req)
        {
            // Send the serialized elements for the specific port ID
            foreach (BaseDevice device in Devices)
            {
                if (device.PortID == req.PortID)
                {
                    if (req.InformationType == InformationType.ModeInfo)
                    {
                        foreach (byte[] elem in device.GetPortInformations())
                        {
                            Bluetooth.NotifyValue(elem);
                        }
                    }
                    else if (req.InformationType == InformationType.PortValue)
                    {
                        // TODO: Send the port value
                    }
                }
            }
        }

        /// <summary>
        /// Processes the port information.
        /// </summary>
        /// <param name="req">The common message header.</param>
        internal void ProcessPortInformation(CommonMessageHeader req)
        {
            byte portId = (req.MessageType == MessageType.PortInformation ? ((PortInformationMessage)req).PortID : ((PortModeInformationMessage)req).PortID);
            byte hubId = req.HubID;
            foreach (BaseDevice device in Devices)
            {
                if ((device.HubID == hubId) && (device.PortID == portId))
                {
                    device.PopulateModes(req);
                }
            }
        }

        /// <summary>
        /// Processes the hub properties.
        /// </summary>
        /// <param name="prop">The hub property message.</param>
        internal void ProcessHubProperties(HubPropertyMessage prop)
        {
            if ((prop.Operation == HubPropertyOperation.RequestUpdate) || (prop.Operation == HubPropertyOperation.EnableUpdates))
            {
                var notify = new HubPropertyMessage(0, prop.Property, HubPropertyOperation.Update, new byte[0]);

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
                        notify.Payload = new byte[] { BatteryVoltage };
                        break;
                    case HubProperty.BatteryType:
                        // Normal batteries
                        notify.Payload = new byte[] { (byte)BatteryType };
                        break;
                    case HubProperty.ManufacturerName:
                        notify.Payload = Encoding.UTF8.GetBytes(ManufacturerName);
                        break;
                    case HubProperty.RadioFirmwareVersion:
                        notify.Payload = Encoding.UTF8.GetBytes(RadioFirmwareVersion);
                        break;
                    case HubProperty.LEGOWirelessProtocolVersion:
                        notify.Payload = new byte[] { (byte)LEGOWirelessProtocolVersion.Minor, (byte)LEGOWirelessProtocolVersion.Major };
                        break;
                    case HubProperty.SystemTypeID:
                        notify.Payload = new byte[] { SystemTypeID };
                        break;
                    case HubProperty.HWNetworkID:
                        notify.Payload = new byte[] { (byte)HWNetworkID };
                        break;
                    case HubProperty.PrimaryMACAddress:
                        notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAB };
                        break;
                    case HubProperty.SecondaryMACAddress:
                        notify.Payload = new byte[] { 0x00, 0x16, 0x53, 0xAE, 0xEE, 0xAC };
                        break;
                    case HubProperty.HardwareNetworkFamily:
                        notify.Payload = new byte[] { (byte)hWNetworkFamily };
                        break;
                    default:
                        break;
                }

                Bluetooth.NotifyValue(notify.ToByteArray());
            }
        }

        /// <summary>
        /// Processes the alert.
        /// </summary>
        /// <param name="alert">The hub alert message.</param>
        internal void ProcessAlert(HubAlertMessage alert)
        {
            if ((alert.AlertOperation == HubAlertOperation.EnableUpdates) || (alert.AlertOperation == HubAlertOperation.RequestUpdates))
            {
                var notify = new HubAlertMessage(0, alert.AlertType, HubAlertOperation.Update, HubAlertPayload.StatusOK);
                Bluetooth.NotifyValue(notify.ToByteArray());

                if (alert.AlertOperation == HubAlertOperation.EnableUpdates)
                {
                    // Add it to the list of alerts
                    Alert al = new Alert(alert.AlertType, alert.AlertOperation);
                    try
                    {
                        Alerts.Add(al);
                    }
                    catch (Exception)
                    {
                        // Nothing, most likely existing alert already in the list
                    }
                }
            }
            else if (alert.AlertOperation == HubAlertOperation.DisableUpdates)
            {
                Alert al = new Alert(alert.AlertType, alert.AlertOperation);
                try
                {
                    Alerts.Remove(al);
                }
                catch (Exception)
                {
                    // Nothing, most likely existing alert not in the list
                }
            }
        }

        /// <summary>
        /// Gets the device from the port ID.
        /// </summary>
        /// <param name="portId">The port ID.</param>
        /// <returns>The device associated with the port ID.</returns>
        internal BaseDevice GetDeviceFromPortId(byte portId)
        {
            foreach (BaseDevice device in Devices)
            {
                if (device.PortID == portId)
                {
                    return device;
                }
            }

            return null;
        }

        /// <summary>
        /// Attaches the device to the specified port ID.
        /// </summary>
        /// <param name="portId">The port ID.</param>
        /// <returns>The attached device message as a byte array.</returns>
        internal byte[] AttachDevice(byte portId)
        {
            var dev = GetDeviceFromPortId(portId);
            if ((dev != null) && (dev.DeviceType != IOTypeID.Unknown))
            {
                var msg = new HubAttachedIOMessage(dev.HubID, portId, dev.IsVirtual ? IOEvent.AttachedVirtualIO : IOEvent.AttachedIO, dev.DeviceType, dev.HWVersion, dev.SWVersion, dev.PortIdA, dev.PortIdB);
                return msg.ToByteArray();
            }

            return null;
        }

        /// <summary>
        /// Detaches the device to the specified port ID.
        /// </summary>
        /// <param name="portId">The port ID.</param>
        /// <returns>The detached device message as a byte array.</returns>
        internal byte[] DetachDevice(byte portId)
        {
            var dev = GetDeviceFromPortId(portId);
            if (dev != null)
            {
                var msg = new HubAttachedIOMessage(dev.HubID, portId, IOEvent.DetachedIO, dev.DeviceType, dev.HWVersion, dev.SWVersion, 0, 0);
                return msg.ToByteArray();
            }

            return null;
        }
    }
}
