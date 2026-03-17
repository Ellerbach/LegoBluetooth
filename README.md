# .NET Lego BLE implementation for nanoFramework

This project provides a .NET and .NET nanoFramework implementation of the LEGO® Bluetooth Low Energy (BLE) protocol. It aims to facilitate communication between LEGO devices and host systems by implementing the public [LEGO BLE protocol](https://lego.github.io/lego-ble-wireless-protocol-docs/index.html). The repository includes [documentation](./Docs/README.md), [protocol details](./Docs/protocol-details.md), examples, and additional elements that are not covered in the official LEGO documentation. It is based on the work from the [sharpbrick project](https://github.com/sharpbrick/docs/tree/master) and focuses on being able to create a custom device rather than being a host. Being a host is also technically possible as all the protocol is implemented.

## Disclaimer

LEGO® is a trademark of the LEGO Group of companies which does not sponsor, authorize or endorse this project. This repository is an implementation of a [public protocol](https://lego.github.io/lego-ble-wireless-protocol-docs/index.html) and is not affiliated with the LEGO Group.

## Hardware requirement

To implement the LEGO® Bluetooth Low Energy (BLE) protocol, a device with sufficient memory is required due to the memory limitations associated with Bluetooth communication. A simple ESP32 WROOM module does not provide enough memory to handle the BLE protocol effectively. Therefore, it is recommended to use a device like the ESP32-S3, which offers more memory and better performance for such tasks.

Any ESP32 module with PSRAM and nanoFramework support will work for this project. The additional PSRAM provides the necessary memory to manage the Bluetooth stack and the associated data processing. It is crucial to ensure that the chosen ESP32 module is [compatible with nanoFramework](https://github.com/nanoframework/nf-interpreter) to leverage its capabilities for this implementation.

Additionally, the specific firmware with Bluetooth support needs to be flashed onto the ESP32 module. This firmware enables the Bluetooth functionality required for communication between LEGO devices and the host system. Detailed instructions for flashing the firmware and setting up the development environment can be found in the documentation provided in this repository.

## Project structure

The solution is organized as follows:

| Project | Description |
|---|---|
| **LegoBluetoothCommon** | Shared project containing all protocol messages, enums, device classes, and hub implementations. Referenced by both nanoFramework and .NET projects. |
| **LegoBluetooth** | nanoFramework library providing the `BluetoothNano` BLE implementation. |
| **LegoDevice** | nanoFramework application that runs the hub emulator on an ESP32. |
| **DumpPayloads** | nanoFramework utility to scan and dump raw BLE payloads from real LEGO hubs. |
| **DotnetLegoBluetoothTest** | .NET 8.0 unit tests (143 tests) covering all message types. |
| **NanoLegoBluetoothTests** | nanoFramework unit test project (runs on device or emulator). |
| **CommonLegoBluetoothTest** | Shared test project referenced by both test projects above. |
| **DecoderHelper** | .NET console application for decoding captured BLE communication logs. |

## Supported hub types

The following LEGO hub types can be emulated:

| Hub | Class | BLE Name | Description |
|---|---|---|---|
| Move Hub (88006) | `MoveHub` | "Move Hub" | 2 internal motors with tacho, RGB light, tilt sensor, current/voltage sensors |
| Two Port Hub / City Hub (88009) | `TwoPortHub` | "Hub" | 2 external ports, RGB light, current/voltage sensors |
| Technic Medium Hub (88012) | `TechnicMediumHub` | "Technic Hub" | 4 external ports, RGB light, 2 temperature sensors, accelerometer, gyro, tilt, gesture sensors |
| Duplo Train Base (10874) | `DuploTrainBaseHub` | "Train Base" | Motor, speaker, RGB light, color sensor, speedometer, voltage sensor |
| Mario Hub (71360) | `MarioHub` | "Mario" | Accelerometer, tag sensor, pants sensor, debug interface, voltage sensor |

## Supported device classes

All devices are in the `LegoBluetooth.Device` namespace with binary mode data based on the [sharpbrick documentation](https://github.com/sharpbrick/docs/tree/master):

- `Current` — Current sensor
- `Voltage` — Voltage sensor
- `RgbLight` — RGB LED light
- `MoveHubInternalMotor` — Internal motor with tacho (Move Hub)
- `MoveHubTiltSensor` — Tilt sensor (Move Hub)
- `MoveHub0x46` — Unknown internal device (Move Hub)
- `TechnicMediumHubAccelerometer` — 3-axis accelerometer (Technic Medium Hub)
- `TechnicMediumHubGyroSensor` — Gyro sensor (Technic Medium Hub)
- `TechnicMediumHubTiltSensor` — Tilt sensor (Technic Medium Hub)
- `TechnicMediumHubTemperatureSensor` — Temperature sensor (Technic Medium Hub)
- `TechnicMediumHubGestureSensor` — Gesture sensor (Technic Medium Hub)
- `DuploTrainBaseMotor` — Train motor (Duplo Train Base)
- `DuploTrainBaseSpeaker` — Speaker (Duplo Train Base)
- `DuploTrainBaseColorSensor` — Color sensor (Duplo Train Base)
- `DuploTrainBaseSpeedometer` — Speedometer (Duplo Train Base)
- `MarioHubAccelerometer` — Accelerometer (Mario Hub)
- `MarioHubTagSensor` — Tag/barcode sensor (Mario Hub)
- `MarioHubPants` — Pants sensor (Mario Hub)
- `MarioHubDebug` — Debug interface (Mario Hub)

## Implemented protocol messages

All message types from the [LEGO BLE Wireless Protocol 3.0.00](https://lego.github.io/lego-ble-wireless-protocol-docs/index.html) are implemented:

| Message | Type | Direction | Description |
|---|---|---|---|
| Hub Property (0x01) | `HubPropertyMessage` | Both | Get/set hub properties (name, FW version, battery, etc.) |
| Hub Action (0x02) | `HubActionMessage` | Both | Hub actions (disconnect, shutdown, etc.) |
| Hub Alert (0x03) | `HubAlertMessage` | Both | Hub alerts (low voltage, high current, etc.) |
| Hub Attached I/O (0x04) | `HubAttachedIOMessage` | Device→Host | Announce attached/detached devices on ports |
| Error (0x05) | `ErrorMessage` | Device→Host | Generic error responses |
| HW Network (0x08) | `HWNetworkMessage` | Both | Hardware network commands |
| Port Information Request (0x21) | `PortInformationRequestMessage` | Host→Device | Request port mode/combination info |
| Port Mode Information Request (0x22) | `PortModeInformationRequestMessage` | Host→Device | Request detailed mode info (name, range, format, etc.) |
| Port Input Format Setup Single (0x41) | `PortInputFormatSetupSingleMessage` | Host→Device | Subscribe to single-mode notifications |
| Port Input Format Setup Combined (0x42) | `PortInputFormatSetupCombinedModeMessage` | Host→Device | Subscribe to combined-mode notifications |
| Port Information (0x43) | `PortInformationMessage` | Device→Host | Port mode and combination details |
| Port Mode Information (0x44) | `PortModeInformationMessage` | Device→Host | Detailed mode information |
| Port Value Single (0x45) | `PortValueSingleMessage` | Device→Host | Single-mode sensor value notifications |
| Port Value Combined (0x46) | `PortValueCombinedModeMessage` | Device→Host | Combined-mode sensor value notifications |
| Port Input Format Single (0x47) | `PortInputFormatSingleMessage` | Device→Host | Confirm input format setup |
| Port Input Format Combined (0x48) | `PortInputFormatCombinedModeMessage` | Device→Host | Confirm combined input format setup |
| Virtual Port Setup (0x61) | `VirtualPortSetupMessage` | Host→Device | Create/delete virtual ports |
| Port Output Command (0x81) | `PortOutputCommandMessage` | Host→Device | Motor and output commands |
| Port Output Command Feedback (0x82) | `PortOutputCommandFeedbackMessage` | Device→Host | Command execution feedback |
| Go Into Boot Mode (0x10) | `GoIntoBootModeMessage` | Host→Device | Enter bootloader mode |

## Usage for .NET nanoFramework

The `LegoDevice` project is the main application that runs on an ESP32 with PSRAM and BLE support.

### Running the emulator

1. Flash the ESP32 with [nanoFramework firmware](https://github.com/nanoframework/nf-interpreter) with Bluetooth support.
2. Open the solution in Visual Studio with the nanoFramework extension installed.
3. In `LegoDevice/Program.cs`, uncomment the hub type you want to emulate:

```csharp
// Uncomment the hub you want to emulate:
//var hub = CreateMoveHub(bluetooth);
//var hub = CreateTwoPortHub(bluetooth);
//var hub = CreateTechnicMediumHub(bluetooth);
var hub = CreateDuploTrainBaseHub(bluetooth);
//var hub = CreateMarioHub(bluetooth);
```

4. Deploy and run on the ESP32 device.
5. The device will advertise as a LEGO hub over BLE. Use the official LEGO app or any compatible host application to connect.

### Handling motor commands

Subscribe to the `OnPortOutputCommand` event to receive motor and output commands from the host:

```csharp
hub.OnPortOutputCommand += (PortOutputCommandMessage msg) =>
{
    Debug.WriteLine($"Motor command: Port={msg.PortID}, SubCommand={msg.SubCommand}");
    // Drive your actual hardware here
};
```

### Sensor notifications

The hub emulator automatically:
- Sends periodic sensor value notifications (0x45) every 500ms for subscribed ports.
- Simulates motor speed ramping with gradual acceleration/deceleration.
- Reports voltage and current values every 2 seconds.
- Sends correct port output command feedback (0x82) with `Idle | BufferEmptyCommandCompleted` so the host application continues sending commands.

### Memory management

On nanoFramework targets, the emulator periodically calls `nanoFramework.Runtime.Native.GC.Run(true)` to reclaim memory on constrained devices. Direct byte array construction is used instead of message object serialization to minimize allocations.

## Usage for .NET

The protocol message classes in `LegoBluetoothCommon` can be used in any .NET application to encode and decode LEGO BLE protocol messages. The shared project is referenced by the .NET test project, demonstrating full protocol compatibility.

### Decoding messages

```csharp
byte[] data = new byte[] { 0x0F, 0x00, 0x04, 0x00, 0x01, 0x27, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10 };
CommonMessageHeader header = CommonMessageHeader.Decode(data);
// header.MessageType will be MessageType.HubAttachedIO
HubAttachedIOMessage msg = new HubAttachedIOMessage(data);
// msg.PortID, msg.IOTypeID, etc.
```

### Running the tests

The `DotnetLegoBluetoothTest` project contains 143 unit tests covering all message types:

```
dotnet test DotnetLegoBluetoothTest/DotnetLegoBluetoothTest.csproj
```

## Completed work

- [x] Implement all protocol message types (encoding and decoding)
- [x] Implement Hub Attached I/O for all hub types
- [x] Implement the Port Information request and responses (0x21/0x43)
- [x] Implement the Port Mode Information request and responses (0x22/0x44)
- [x] Implement the Port Input Format request and responses (0x41/0x42/0x47/0x48)
- [x] Implement the notification for sensor value changes (0x45/0x46)
- [x] Implement the notifications for alerts (0x03)
- [x] Implement the Port Output Command and feedback (0x81/0x82)
- [x] Implement the HW Network messages (0x08)
- [x] Implement the Virtual Port Setup (0x61)
- [x] Implement 5 hub types (MoveHub, TwoPortHub, TechnicMediumHub, DuploTrainBaseHub, MarioHub)
- [x] Implement 20 device classes with binary mode data
- [x] Motor simulation with speed ramping
- [x] Periodic sensor notifications via timer
- [x] Memory management for constrained nanoFramework devices
- [x] 143 unit tests covering all message types

## Known limitations

- Generics and async patterns are not supported on nanoFramework, so the implementation uses synchronous patterns.
- The emulator simulates sensor values; actual hardware integration requires handling the `OnPortOutputCommand` event.
- ESP32 modules without PSRAM may not have sufficient memory for the BLE stack.
