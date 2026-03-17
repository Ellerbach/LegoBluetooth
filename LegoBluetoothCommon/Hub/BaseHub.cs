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
        /// Delegate for handling port output commands.
        /// </summary>
        /// <param name="msg">The port output command message.</param>
        public delegate void PortOutputCommandHandler(PortOutputCommandMessage msg);

        /// <summary>
        /// Event raised when a port output command is received from the host.
        /// </summary>
        public event PortOutputCommandHandler OnPortOutputCommand;

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
        /// Gets or sets the name of the device.
        /// </summary>
        public string DefaultName { get; set; } = string.Empty;

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
        /// Gets or sets the hub ID.
        /// </summary>
        public byte HubID { get; internal set; } = 0;

        private Timer _sensorTimer;
        private readonly ArrayList _subscribedInputPorts = new ArrayList();
        private readonly Random _random = new Random((int)(DateTime.UtcNow.Ticks & 0x7FFFFFFF));
        private readonly Hashtable _motorPowers = new Hashtable();
        private readonly Hashtable _activePortModes = new Hashtable();

        /// <summary>
        /// Forces a garbage collection to reclaim memory on constrained devices.
        /// </summary>
        private static void RunGC()
        {
#if NANOFRAMEWORK
            nanoFramework.Runtime.Native.GC.Run(true);
#endif
        }

        /// <summary>
        /// Processes the received data.
        /// </summary>
        /// <param name="data">The received data.</param>
        public void ProcessReceived(byte[] data)
        {
            var req = CommonMessageHeader.Decode(data);
            Debug.WriteLine($"Received: {req.MessageType} (0x{((byte)req.MessageType):X2})");

            if (req.MessageType == MessageType.HubProperties)
            {
                var prop = (HubPropertyMessage)req;
                ProcessHubProperties(prop);
            }
            else if (req.MessageType == MessageType.HubActions)
            {
                ProcessHubAction(req);
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
            else if (req.MessageType == MessageType.PortModeInformationRequest)
            {
                ProcessPortModeInformationRequest((PortModeInformationRequestMessage)req);
            }
            else if (req.MessageType == MessageType.PortInputFormatSetupCombinedMode)
            {
                ProcessPortInputFormatSetupCombinedMode(req);
            }
            else if (req.MessageType == MessageType.PortInputFormatSetupSingle)
            {
                ProcessPortInputFormatSetupSingle(req);
            }
            else if (req.MessageType == MessageType.VirtualPortSetup)
            {
                ProcessVirtualPortSetup(req);
            }
            else if (req.MessageType == MessageType.PortOutputCommand)
            {
                ProcessPortOutputCommand((PortOutputCommandMessage)req);
            }
            else
            {
                Debug.WriteLine($"Unhandled message type: {req.MessageType} (0x{((byte)req.MessageType):X2})");
            }
        }

        private void ProcessPortModeInformationRequest(PortModeInformationRequestMessage req)
        {
            var dev = GetDeviceFromPortId(req.PortID);
            if (dev != null)
            {
                var mode = dev.GetPortModeDetails(req.PortID, req.Mode);
                if (mode != null)
                {
                    Bluetooth.NotifyValue((byte [])mode.CreateByteArrays(new ModeInformationType[] { req.InformationType })[0]);
                }
            }
        }

        private void ProcessHubAction(CommonMessageHeader req)
        {
            var action = (HubActionMessage)req;
            Debug.WriteLine($"Hub Action: {action.ActionType}");

            switch (action.ActionType)
            {
                case ActionType.SwitchOffHub:
                    Bluetooth.NotifyValue(new HubActionMessage(HubID, ActionType.HubWillSwitchOff).ToByteArray());
                    break;
                case ActionType.Disconnect:
                    Bluetooth.NotifyValue(new HubActionMessage(HubID, ActionType.HubWillDisconnect).ToByteArray());
                    break;
                case ActionType.ActivateBusyIndication:
                case ActionType.ResetBusyIndication:
                case ActionType.VCCPortControlOn:
                case ActionType.VCCPortControlOff:
                    // Acknowledged silently
                    break;
                default:
                    Debug.WriteLine($"Unhandled Hub Action: {action.ActionType}");
                    break;
            }
        }

        private void ProcessVirtualPortSetup(CommonMessageHeader req)
        {
            var msg = (VirtualPortSetupMessage)req;
            Debug.WriteLine($"Virtual Port Setup: SubCommand={msg.SubCommand}, PortA={msg.PortIDA}, PortB={msg.PortIDB}");

            if (msg.SubCommand == 0x01)
            {
                // Connect two ports into a virtual port
                var devA = GetDeviceFromPortId(msg.PortIDA);
                var devB = GetDeviceFromPortId(msg.PortIDB);
                if (devA == null || devB == null)
                {
                    Bluetooth.NotifyValue(new ErrorMessage(HubID, (byte)MessageType.VirtualPortSetup, ErrorCode.InvalidUse).ToByteArray());
                    return;
                }

                // Find existing virtual port for this pair
                foreach (BaseDevice dev in Devices)
                {
                    if (dev.IsVirtual && dev.PortIdA == msg.PortIDA && dev.PortIdB == msg.PortIDB)
                    {
                        // Already exists, send attach notification
                        var attach = AttachDevice(dev.PortID);
                        if (attach != null)
                        {
                            Bluetooth.NotifyValue(attach);
                        }

                        return;
                    }
                }

                Bluetooth.NotifyValue(new ErrorMessage(HubID, (byte)MessageType.VirtualPortSetup, ErrorCode.InvalidUse).ToByteArray());
            }
            else if (msg.SubCommand == 0x00)
            {
                // Disconnect virtual port
                var dev = GetDeviceFromPortId(msg.PortID);
                if (dev != null && dev.IsVirtual)
                {
                    var detach = DetachDevice(dev.PortID);
                    if (detach != null)
                    {
                        Bluetooth.NotifyValue(detach);
                    }
                }
                else
                {
                    Bluetooth.NotifyValue(new ErrorMessage(HubID, (byte)MessageType.VirtualPortSetup, ErrorCode.InvalidUse).ToByteArray());
                }
            }
            else
            {
                Bluetooth.NotifyValue(new ErrorMessage(HubID, (byte)MessageType.VirtualPortSetup, ErrorCode.InvalidUse).ToByteArray());
            }
        }

        private void ProcessPortInputFormatSetupSingle(CommonMessageHeader req)
        {
            var msg = (PortInputFormatSetupSingleMessage)req;
            // We need to answer we've setup with message 0x47
            Bluetooth.NotifyValue((new PortInputFormatSingleMessage(HubID, msg.PortID, msg.Mode, msg.DeltaInterval, msg.NotificationEnabled)).ToByteArray());

            // Track the active mode for this port (used when motor commands arrive)
            _activePortModes[msg.PortID] = msg.Mode;

            // Always send current port value (0x45) so the app knows the current state
            SendPortValueForMode(msg.PortID, msg.Mode);

            if (msg.NotificationEnabled && msg.DeltaInterval > 0)
            {
                // Track this subscription
                var sub = new SensorSubscription { PortID = msg.PortID, Mode = msg.Mode, DeltaInterval = msg.DeltaInterval };
                lock (_subscribedInputPorts)
                {
                    // Remove existing subscription for same port+mode
                    for (int i = _subscribedInputPorts.Count - 1; i >= 0; i--)
                    {
                        var existing = (SensorSubscription)_subscribedInputPorts[i];
                        if (existing.PortID == msg.PortID && existing.Mode == msg.Mode)
                        {
                            _subscribedInputPorts.RemoveAt(i);
                        }
                    }

                    _subscribedInputPorts.Add(sub);
                }

                // Start the sensor timer if not already running
                if (_sensorTimer == null)
                {
                    _sensorTimer = new Timer(SensorTimerCallback, null, 500, 500);
                }
            }
            else if (!msg.NotificationEnabled)
            {
                // Remove subscription for this port+mode
                lock (_subscribedInputPorts)
                {
                    for (int i = _subscribedInputPorts.Count - 1; i >= 0; i--)
                    {
                        var existing = (SensorSubscription)_subscribedInputPorts[i];
                        if (existing.PortID == msg.PortID && existing.Mode == msg.Mode)
                        {
                            _subscribedInputPorts.RemoveAt(i);
                        }
                    }

                    if (_subscribedInputPorts.Count == 0 && _sensorTimer != null)
                    {
                        _sensorTimer.Dispose();
                        _sensorTimer = null;
                    }
                }
            }
        }

        private void SensorTimerCallback(object state)
        {
            lock (_subscribedInputPorts)
            {
                foreach (SensorSubscription sub in _subscribedInputPorts)
                {
                    sub.TickCount++;
                    var dev = GetDeviceFromPortId(sub.PortID);
                    if (dev != null && (dev.DeviceType == IOTypeID.Voltage || dev.DeviceType == IOTypeID.Current))
                    {
                        // Voltage/Current: every 2 seconds (4 ticks at 500ms)
                        if (sub.TickCount < 4)
                        {
                            continue;
                        }
                    }

                    sub.TickCount = 0;
                    SendSensorValue(sub);
                }
            }

            // Force garbage collection to reclaim temporary byte arrays
            RunGC();
        }

        private void SendSensorValue(SensorSubscription sub)
        {
            var dev = GetDeviceFromPortId(sub.PortID);
            if (dev == null)
            {
                return;
            }

            var modeDetails = dev.GetPortModeDetails(sub.PortID, sub.Mode);
            if (modeDetails == null || modeDetails.ValueFormat == null)
            {
                return;
            }

            int datasetSize;
            switch (modeDetails.ValueFormat.Type)
            {
                case DataType.Byte:
                    datasetSize = 1;
                    break;
                case DataType.UInt16:
                    datasetSize = 2;
                    break;
                case DataType.UInt32:
                case DataType.Single:
                    datasetSize = 4;
                    break;
                default:
                    return;
            }

            int valueSize = modeDetails.ValueFormat.NumberOfDatasets * datasetSize;
            byte[] valueData = new byte[valueSize];

            short newRaw = 0;
            PopulateSensorData(dev, sub, valueData, out newRaw);

            // Only send if value changed by >= deltaInterval from last sent value
            int diff = newRaw > sub.LastSentValue ? newRaw - sub.LastSentValue : sub.LastSentValue - newRaw;
            if (sub.LastSentValue != -1 && diff < (int)sub.DeltaInterval)
            {
                return;
            }

            sub.LastSentValue = newRaw;

            // Build 0x45 message directly to avoid ArrayList allocations (OOM on constrained devices)
            int msgLen = 3 + 1 + valueSize; // header(1) + hubID(1) + type(1) + portID(1) + value
            byte[] msg = new byte[msgLen];
            msg[0] = (byte)msgLen;
            msg[1] = HubID;
            msg[2] = (byte)MessageType.PortValueSingle;
            msg[3] = sub.PortID;
            Array.Copy(valueData, 0, msg, 4, valueSize);
            Bluetooth.NotifyValue(msg);
        }

        /// <summary>
        /// Sends a 0x45 PortValueSingle for a port with the given active mode.
        /// Called immediately when a command changes the port's value.
        /// </summary>
        private void SendPortValueForMode(byte portID, byte mode)
        {
            var dev = GetDeviceFromPortId(portID);
            if (dev == null)
            {
                return;
            }

            var modeDetails = dev.GetPortModeDetails(portID, mode);
            if (modeDetails == null || modeDetails.ValueFormat == null)
            {
                return;
            }

            int datasetSize;
            switch (modeDetails.ValueFormat.Type)
            {
                case DataType.Byte:
                    datasetSize = 1;
                    break;
                case DataType.UInt16:
                    datasetSize = 2;
                    break;
                case DataType.UInt32:
                case DataType.Single:
                    datasetSize = 4;
                    break;
                default:
                    return;
            }

            int valueSize = modeDetails.ValueFormat.NumberOfDatasets * datasetSize;
            byte[] valueData = new byte[valueSize];

            // Use a temporary subscription to reuse PopulateSensorData
            var tempSub = new SensorSubscription { PortID = portID, Mode = mode, DeltaInterval = 1 };
            short newRaw;
            PopulateSensorData(dev, tempSub, valueData, out newRaw);

            // Build 0x45 message directly to avoid ArrayList allocations (OOM on constrained devices)
            int msgLen = 3 + 1 + valueSize; // header(1) + hubID(1) + type(1) + portID(1) + value
            byte[] msg = new byte[msgLen];
            msg[0] = (byte)msgLen;
            msg[1] = HubID;
            msg[2] = (byte)MessageType.PortValueSingle;
            msg[3] = portID;
            Array.Copy(valueData, 0, msg, 4, valueSize);
            Bluetooth.NotifyValue(msg);
        }

        /// <summary>
        /// Ensures a subscription exists for a motor port so that periodic 0x45 notifications are sent.
        /// Uses the active mode set via 0x41, or the mode from the command payload.
        /// </summary>
        private void EnsureMotorSubscription(byte portID, byte commandMode)
        {
            // Determine the mode: prefer the mode set via 0x41, fall back to the command mode
            byte mode = _activePortModes.Contains(portID) ? (byte)_activePortModes[portID] : commandMode;

            lock (_subscribedInputPorts)
            {
                // Check if a subscription already exists for this port
                foreach (SensorSubscription existing in _subscribedInputPorts)
                {
                    if (existing.PortID == portID && existing.Mode == mode)
                    {
                        // Already tracking — timer will send the next update
                        return;
                    }
                }

                // No subscription yet — create one so the timer sends periodic updates
                // DeltaInterval=0 means always send, even if the value hasn't changed
                var sub = new SensorSubscription { PortID = portID, Mode = mode, DeltaInterval = 0 };
                _subscribedInputPorts.Add(sub);

                if (_sensorTimer == null)
                {
                    _sensorTimer = new Timer(SensorTimerCallback, null, 500, 500);
                }
            }
        }

        /// <summary>
        /// Populates simulated sensor data based on device type and mode.
        /// </summary>
        private void PopulateSensorData(BaseDevice dev, SensorSubscription sub, byte[] valueData, out short newRaw)
        {
            newRaw = 0;

            if (dev.DeviceType == IOTypeID.Voltage && sub.Mode == 0)
            {
                // ~7.2V baseline (raw ~2917), small jitter ±5
                newRaw = (short)(2917 + _random.Next(11) - 5);
                byte[] raw = BitConverter.GetBytes(newRaw);
                Array.Copy(raw, 0, valueData, 0, 2);
            }
            else if (dev.DeviceType == IOTypeID.Current && sub.Mode == 0)
            {
                // ~100mA baseline (raw ~168), small jitter ±3
                newRaw = (short)(168 + _random.Next(7) - 3);
                byte[] raw = BitConverter.GetBytes(newRaw);
                Array.Copy(raw, 0, valueData, 0, 2);
            }
            else if (dev.DeviceType == IOTypeID.DuploTrainBaseMotor)
            {
                if (sub.Mode == 0)
                {
                    // T MOT: current power (Byte) — ramp toward target
                    sbyte target = GetMotorPower(sub.PortID);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, target, 10);
                    newRaw = (short)sub.CurrentSpeed;
                    valueData[0] = (byte)(sbyte)sub.CurrentSpeed;
                }
                else if (sub.Mode == 1)
                {
                    // ONSEC: seconds on (UInt32), increments with timer
                    sub.AccumulatedValue += 1;
                    newRaw = (short)(sub.AccumulatedValue & 0x7FFF);
                    byte[] raw = BitConverter.GetBytes(sub.AccumulatedValue);
                    Array.Copy(raw, 0, valueData, 0, 4);
                }
            }
            else if (dev.DeviceType == IOTypeID.DuploTrainBaseColorSensor)
            {
                if (sub.Mode == 0 || sub.Mode == 1)
                {
                    // COLOR / C TAG: 0xFF = no color / no tag
                    newRaw = (short)0xFF;
                    valueData[0] = 0xFF;
                }
                else if (sub.Mode == 2)
                {
                    // REFLT: reflected light percentage (Byte)
                    newRaw = 0;
                }
                else if (sub.Mode == 3 && valueData.Length >= 6)
                {
                    // RGB I: ambient RGB (3×UInt16)
                    newRaw = 0;
                }
            }
            else if (dev.DeviceType == IOTypeID.DuploTrainBaseSpeedometer)
            {
                if (sub.Mode == 0)
                {
                    // SPEED: proportional to motor power, ramps gradually (Int16)
                    sbyte target = GetMotorPower((byte)0);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, target * 3, 8);
                    newRaw = (short)(sub.CurrentSpeed + _random.Next(3) - 1);
                    byte[] raw = BitConverter.GetBytes(newRaw);
                    Array.Copy(raw, 0, valueData, 0, 2);
                }
                else if (sub.Mode == 1)
                {
                    // COUNT: revolution counter (UInt32)
                    sbyte power = GetMotorPower((byte)0);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, power, 10);
                    if (sub.CurrentSpeed != 0)
                    {
                        sub.AccumulatedValue += sub.CurrentSpeed > 0 ? 1 : -1;
                    }

                    newRaw = (short)(sub.AccumulatedValue & 0x7FFF);
                    byte[] raw = BitConverter.GetBytes(sub.AccumulatedValue);
                    Array.Copy(raw, 0, valueData, 0, 4);
                }
            }
            else if (dev.DeviceType == IOTypeID.InternalMotorWithTacho)
            {
                if (sub.Mode == 0)
                {
                    // POWER: current power (Byte) — ramp toward target
                    sbyte target = GetMotorPower(sub.PortID);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, target, 15);
                    newRaw = (short)sub.CurrentSpeed;
                    valueData[0] = (byte)(sbyte)sub.CurrentSpeed;
                }
                else if (sub.Mode == 1)
                {
                    // SPEED: motor speed percentage (signed Byte, -100..100) — ramp toward target
                    sbyte target = GetMotorPower(sub.PortID);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, target, 15);
                    newRaw = (short)sub.CurrentSpeed;
                    valueData[0] = (byte)(sbyte)sub.CurrentSpeed;
                }
                else if (sub.Mode == 2 && valueData.Length >= 4)
                {
                    // POS: position in degrees (Int32) — accumulate based on current speed
                    sbyte target = GetMotorPower(sub.PortID);
                    sub.CurrentSpeed = RampToward(sub.CurrentSpeed, target, 15);
                    sub.AccumulatedValue += sub.CurrentSpeed * 3;
                    newRaw = (short)(sub.AccumulatedValue & 0x7FFF);
                    byte[] raw = BitConverter.GetBytes(sub.AccumulatedValue);
                    Array.Copy(raw, 0, valueData, 0, 4);
                }
            }
            else if (dev.DeviceType == IOTypeID.InternalTilt)
            {
                if (sub.Mode == 0 && valueData.Length >= 2)
                {
                    // ANGLE: X,Y angles (2×Byte), small jitter around 0
                    sbyte x = (sbyte)(_random.Next(5) - 2);
                    sbyte y = (sbyte)(_random.Next(5) - 2);
                    newRaw = (short)x;
                    valueData[0] = (byte)x;
                    valueData[1] = (byte)y;
                }
                else if (sub.Mode == 1)
                {
                    // TILT: tilt direction (Byte), 0 = flat
                    newRaw = 0;
                }
                else if (sub.Mode == 2)
                {
                    // ORINT: orientation (Byte), 0 = rest
                    newRaw = 0;
                }
                else if (sub.Mode == 3)
                {
                    // IMPCT: impact count (UInt32)
                    newRaw = 0;
                }
                else if (sub.Mode == 4 && valueData.Length >= 3)
                {
                    // ACCEL: XYZ acceleration (3×Byte)
                    newRaw = 0;
                }
            }
            else if (dev.DeviceType == IOTypeID.TechnicMediumHubAccelerometer && sub.Mode == 0 && valueData.Length >= 6)
            {
                // GRV: XYZ gravity in mG (3×Int16), hub at rest: Z ~1000
                short z = (short)(1000 + _random.Next(5) - 2);
                newRaw = z;
                WriteInt16LE(valueData, 0, (short)(_random.Next(5) - 2));
                WriteInt16LE(valueData, 2, (short)(_random.Next(5) - 2));
                WriteInt16LE(valueData, 4, z);
            }
            else if (dev.DeviceType == IOTypeID.TechnicMediumHubGyroSensor && sub.Mode == 0 && valueData.Length >= 6)
            {
                // ROT: XYZ rotation rate in DPS (3×Int16), at rest ~0
                newRaw = 0;
                WriteInt16LE(valueData, 0, (short)(_random.Next(5) - 2));
                WriteInt16LE(valueData, 2, (short)(_random.Next(5) - 2));
                WriteInt16LE(valueData, 4, (short)(_random.Next(5) - 2));
            }
            else if (dev.DeviceType == IOTypeID.TechnicMediumHubTiltSensor)
            {
                if (sub.Mode == 0 && valueData.Length >= 6)
                {
                    // POS: XYZ tilt in degrees (3×Int16), at rest ~0
                    newRaw = 0;
                    WriteInt16LE(valueData, 0, (short)(_random.Next(5) - 2));
                    WriteInt16LE(valueData, 2, (short)(_random.Next(5) - 2));
                    WriteInt16LE(valueData, 4, 0);
                }
                else if (sub.Mode == 1)
                {
                    // IMP: impact count (UInt32)
                    newRaw = 0;
                }
            }
            else if (dev.DeviceType == IOTypeID.TechnicMediumHubTemperatureSensor && sub.Mode == 0 && valueData.Length >= 2)
            {
                // TEMP: temperature in 0.1°C (Int16), ~25.0°C = 250
                newRaw = (short)(250 + _random.Next(5) - 2);
                WriteInt16LE(valueData, 0, newRaw);
            }
            else if (dev.DeviceType == IOTypeID.TechnicMediumHubGestureSensor && sub.Mode == 0)
            {
                // GEST: gesture code (Byte), 0 = no gesture
                newRaw = 0;
            }
            else if (dev.DeviceType == IOTypeID.MarioHubAccelerometer)
            {
                if (sub.Mode == 0)
                {
                    // RAW: XYZ (3×Byte)
                    newRaw = 0;
                }
                else if (sub.Mode == 1)
                {
                    // GEST: gesture (2×UInt16)
                    newRaw = 0;
                }
            }
            else if (dev.DeviceType == IOTypeID.MarioHubTagSensor)
            {
                if (sub.Mode == 0)
                {
                    // TAG: tag ID (2×UInt16), 0 = no tag
                    newRaw = 0;
                }
                else if (sub.Mode == 1)
                {
                    // RGB: (3×Byte)
                    newRaw = 0;
                }
            }
            else if (dev.DeviceType == IOTypeID.MarioHubPants && sub.Mode == 0)
            {
                // PANT: pants code (Byte), 0 = none
                newRaw = 0;
            }
            else if (dev.DeviceType == IOTypeID.RGBLight)
            {
                if (sub.Mode == 0)
                {
                    // COL O: current color index (Byte)
                    sbyte color = GetMotorPower(sub.PortID);
                    newRaw = (short)color;
                    valueData[0] = (byte)color;
                }
                else if (sub.Mode == 1 && valueData.Length >= 3)
                {
                    // RGB O: current RGB values (3×Byte)
                    newRaw = 0;
                }
            }
            else
            {
                // Generic fallback: report last written value if available
                sbyte lastVal = GetMotorPower(sub.PortID);
                newRaw = (short)lastVal;
                if (valueData.Length >= 1)
                {
                    valueData[0] = (byte)lastVal;
                }
            }
        }

        /// <summary>
        /// Gets the tracked motor power for a given port.
        /// </summary>
        private sbyte GetMotorPower(byte portID)
        {
            if (_motorPowers.Contains(portID))
            {
                return (sbyte)_motorPowers[portID];
            }

            return 0;
        }

        /// <summary>
        /// Writes a 16-bit signed integer in little-endian format to a byte array.
        /// </summary>
        private static void WriteInt16LE(byte[] data, int offset, short value)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
        }

        /// <summary>
        /// Gradually moves <paramref name="current"/> toward <paramref name="target"/> by up to <paramref name="step"/> per tick.
        /// Simulates motor acceleration and deceleration.
        /// </summary>
        private static int RampToward(int current, int target, int step)
        {
            int diff = target - current;
            if (diff > step)
            {
                return current + step;
            }

            if (diff < -step)
            {
                return current - step;
            }

            return target;
        }

        private void ProcessPortInputFormatSetupCombinedMode(CommonMessageHeader req)
        {
            var msg = (PortInputFormatSetupCombinedModeMessage)req;
            var dev = GetDeviceFromPortId(msg.PortID);
            if (dev == null)
            {
                // No device to setup, return an error
                Bluetooth.NotifyValue((new ErrorMessage(HubID, (byte)MessageType.PortInputFormatSetupCombinedMode, ErrorCode.InvalidUse)).ToByteArray());
                return;
            }

            // different cases for different sub command
            switch (msg.SubCommand)
            {
                case PortInputFormatSetupSubCommand.SetModeAndDataSetCombinations:
                    Debug.WriteLine($"SetModeAndDataSetCombinations: Port={msg.PortID}, CombIdx={msg.CombinationIndex}");
                    break;
                case PortInputFormatSetupSubCommand.LockDeviceForSetup:
                    // Answer with a 0x48 Port Input Format Setup Combined Mode message
                    Bluetooth.NotifyValue((new PortInputFormatCombinedModeMessage(HubID, msg.PortID, msg.CombinationIndex, 0)).ToByteArray());
                    break;
                case PortInputFormatSetupSubCommand.UnlockAndStartWithMultiUpdateEnabled:
                    Debug.WriteLine($"UnlockAndStartMultiUpdate: Port={msg.PortID}");
                    break;
                case PortInputFormatSetupSubCommand.UnlockAndStartWithMultiUpdateDisabled:
                    Debug.WriteLine($"UnlockAndStartNoMultiUpdate: Port={msg.PortID}");
                    break;
                case PortInputFormatSetupSubCommand.ResetSensor:
                    Debug.WriteLine($"ResetSensor: Port={msg.PortID}");
                    break;
                default:
                    Bluetooth.NotifyValue((new ErrorMessage(HubID, (byte)MessageType.PortInputFormatSetupCombinedMode, ErrorCode.InvalidUse)).ToByteArray());
                    break;
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
            var dev = GetDeviceFromPortId(req.PortID);
            if (dev == null)
            {
                return;
            }

            if (req.InformationType == InformationType.ModeInfo)
            {
                // Respond with 0x43 PortInformation ModeInfo
                var msg = new PortInformationMessage(
                    HubID, req.PortID, InformationType.ModeInfo,
                    dev.PortCapabilities, (byte)dev.Modes.Count,
                    dev.InputModes, dev.OutpoutModes, null);
                Bluetooth.NotifyValue(msg.ToByteArray());
            }
            else if (req.InformationType == InformationType.PossibleModeCombinations)
            {
                // Respond with 0x43 PortInformation PossibleModeCombinations
                ushort[] combos = new ushort[dev.ModeCombinations.Count];
                for (int i = 0; i < dev.ModeCombinations.Count; i++)
                {
                    combos[i] = (ushort)dev.ModeCombinations[i];
                }

                var msg = new PortInformationMessage(
                    HubID, req.PortID, InformationType.PossibleModeCombinations,
                    0, 0, 0, 0, combos);
                Bluetooth.NotifyValue(msg.ToByteArray());
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
            else if (prop.Operation == HubPropertyOperation.Set)
            {
                var notify = new HubPropertyMessage(0, prop.Property, HubPropertyOperation.Update, new byte[0]);

                switch (prop.Property)
                {
                    case HubProperty.AdvertisingName:
                        Name= Encoding.UTF8.GetString(prop.Payload, 0, prop.Payload.Length).TrimEnd('\0');
                        notify.Payload = Encoding.UTF8.GetBytes(Name);
                        break;
                    case HubProperty.HWNetworkID:
                        HWNetworkID = (LastNetworkID)prop.Payload[0];
                        notify.Payload = new byte[] { (byte)HWNetworkID };
                        break;
                    case HubProperty.HardwareNetworkFamily:
                        HWNetworkID = (LastNetworkID)prop.Payload[0];
                        notify.Payload = new byte[] { (byte)HWNetworkID };
                        break;
                    default:
                        Bluetooth.NotifyValue((new ErrorMessage(HubID, (byte)MessageType.HubProperties, ErrorCode.InvalidUse)).ToByteArray());
                        break;
                }

                Bluetooth.NotifyValue(notify.ToByteArray());
            }
            else if(prop.Operation == HubPropertyOperation.Reset)
            {
                var notify = new HubPropertyMessage(0, prop.Property, HubPropertyOperation.Update, new byte[0]);

                switch (prop.Property)
                {
                    case HubProperty.AdvertisingName:
                        Name = DefaultName;
                        notify.Payload = Encoding.UTF8.GetBytes(Name);
                        break;
                    case HubProperty.HWNetworkID:
                        HWNetworkID = LastNetworkID.DisableHWNetwork;
                        notify.Payload = new byte[] { (byte)HWNetworkID };
                        break;
                    default:
                        Bluetooth.NotifyValue((new ErrorMessage(HubID, (byte)MessageType.HubProperties, ErrorCode.InvalidUse)).ToByteArray());
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
        /// Processes the port output command.
        /// </summary>
        /// <param name="msg">The port output command message.</param>
        internal void ProcessPortOutputCommand(PortOutputCommandMessage msg)
        {
            Debug.WriteLine($"PortOutputCommand: Port={msg.PortID}, SubCmd={msg.SubCommand}, Payload={BitConverter.ToString(msg.Payload)}");

            // Raise event so application code can act on the command
            OnPortOutputCommand?.Invoke(msg);

            // Track the last written output value for any port
            if (msg.SubCommand == SubCommandType.WriteDirectModeData && msg.Payload.Length >= 2)
            {
                _motorPowers[msg.PortID] = (sbyte)msg.Payload[1];
            }
            else if (msg.SubCommand == SubCommandType.WriteDirect && msg.Payload.Length >= 1)
            {
                _motorPowers[msg.PortID] = (sbyte)msg.Payload[0];
            }

            // Ensure periodic 0x45 notifications flow for motor ports.
            // When a motor command changes the value, the hub should report motor status.
            if (msg.SubCommand == SubCommandType.WriteDirect || msg.SubCommand == SubCommandType.WriteDirectModeData)
            {
                EnsureMotorSubscription(msg.PortID, msg.SubCommand == SubCommandType.WriteDirectModeData && msg.Payload.Length >= 1 ? msg.Payload[0] : (byte)0);
            }

            // WriteDirect and WriteDirectModeData complete immediately.
            // Send 0x82 feedback with Idle | BufferEmptyCommandCompleted (0x0A) when requested.
            // The sharpbrick/powered-up SDK awaits this feedback before sending the next command.
            if (msg.SubCommand == SubCommandType.WriteDirect || msg.SubCommand == SubCommandType.WriteDirectModeData)
            {
                if ((msg.StartupCompletion & StartupCompletionInfo.CommandFeedback) == StartupCompletionInfo.CommandFeedback)
                {
                    // Build 0x82 directly to avoid ArrayList allocations
                    byte[] fb = new byte[] { 0x05, msg.HubID, 0x82, msg.PortID, (byte)(FeedbackMessage.Idle | FeedbackMessage.BufferEmptyCommandCompleted) };
                    Bluetooth.NotifyValue(fb);
                }

                return;
            }

            // For other subcommands (timed moves, goto position, etc.), send feedback
            var feedback = new PortOutputCommandFeedbackMessage(msg.HubID);
            feedback.PortFeedbacks.Add(new PortFeedbackEntry
            {
                PortID = msg.PortID,
                Feedback = FeedbackMessage.BufferEmptyCommandInProgress
            });
            Bluetooth.NotifyValue(feedback.ToByteArray());
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

    /// <summary>
    /// Tracks a sensor subscription for periodic value updates.
    /// </summary>
    internal class SensorSubscription
    {
        public byte PortID { get; set; }
        public byte Mode { get; set; }
        public uint DeltaInterval { get; set; }
        public int LastSentValue { get; set; } = -1;
        public int AccumulatedValue { get; set; } = 0;
        public int TickCount { get; set; } = 0;
        public int CurrentSpeed { get; set; } = 0;
    }
}
