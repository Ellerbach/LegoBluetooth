# .NET Lego BLE implementation for nanoFramework

This project provides a .NET and .NET nanoFramework implementation of the LEGO® Bluetooth Low Energy (BLE) protocol. It aims to facilitate communication between LEGO devices and host systems by implementing the public [LEGO BLE protocol](https://lego.github.io/lego-ble-wireless-protocol-docs/index.html). The repository includes [documentation](./Docs/README.md), examples, and additional elements that are not covered in the official LEGO documentation. It is based on the work from the [sharpbrick project](https://github.com/sharpbrick/docs/tree/master) and focuses on being able to create a custom device rather than being a host. Being a host is also technically possible as all the protocol is implemented.

## Disclaimer

LEGO® is a trademark of the LEGO Group of companies which does not sponsor, authorize or endorse this project. This repository is an implementation of a [public protocol](https://lego.github.io/lego-ble-wireless-protocol-docs/index.html) and is not affiliated with the LEGO Group.

## Hardware requirement

To implement the LEGO® Bluetooth Low Energy (BLE) protocol, a device with sufficient memory is required due to the memory limitations associated with Bluetooth communication. A simple ESP32 WROOM module does not provide enough memory to handle the BLE protocol effectively. Therefore, it is recommended to use a device like the ESP32-S3, which offers more memory and better performance for such tasks.

Any ESP32 module with PSRAM and nanoFramework support will work for this project. The additional PSRAM provides the necessary memory to manage the Bluetooth stack and the associated data processing. It is crucial to ensure that the chosen ESP32 module is [compatible with nanoFramework](https://github.com/nanoframework/nf-interpreter) to leverage its capabilities for this implementation.

Additionally, the specific firmware with Bluetooth support needs to be flashed onto the ESP32 module. This firmware enables the Bluetooth functionality required for communication between LEGO devices and the host system. Detailed instructions for flashing the firmware and setting up the development environment can be found in the documentation provided in this repository.

## Usage for .NET nanoFramework

TBD

## Usage for .NET

TBD

## Work to be done

- [ ] Implement the Port Mode Info request and responses
- [ ] Implement the Port Input Format request and responses
- [ ] Implement the notification for value changes
- [ ] Implement the notifications for alerts
- [ ] Implement the Port Output and Port Output feedback
- [ ] Implement the HW Network
