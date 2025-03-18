# Description
  The `SCHidPlugin` sample demonstrates how to develop a Smart Client plug-in for 
  a Human Interface Device (HID). In general, HID represents hardware equipment (joystick, controller, or keyboard)
  used by operators to control PTZ cameras and the Smart Client. Using the Smart Client HID plug-in, it is possible
  to attach arbitrary hardware HID equipment to the Smart Client, assign actions to keys and key sequences, and control cameras 
  (e.g., PTZ, focus, etc.).

  There are two types of plug-ins distinguished:
  - **Auto-discovery plug-ins**: Plug-ins that operate a plug-and-play HID (e.g., USB joysticks, keyboards, wireless 
    controllers, etc.).
  - **Manual discovery plug-ins**: Plug-ins that connect non-plug-and-play equipment (e.g., LAN joysticks and keyboards, specialized 
    equipment that require dedicated extension hardware cards and software drivers). 

  A manual discovery plug-in allows operators to enter the required information to connect to hardware (for example, 
  for a LAN joystick, the information may include an IP address and port to connect to). In contrast, the auto-discovery
  plug-in relies on Windows plug-and-play services to establish a hardware connection.

---
Keywords: 
  - Plug-in integration
  - HID plug-in
  - auto-discovery plug-in
  - manual discovery plug-in
  - dynamic workspace toolbar menu

Lang: 
  - en-US

Title:
  - Smart Client HID Plug-in
---

# Smart Client HID Plug-in

The `SCSampleHidPlugin` contains two plug-ins:
- `AutodiscoveryHidPlugin`
- `ManualDiscoveryHidPlugin`

You configure plug-ins through the `SmartClient -> Settings -> Joystick` tab and interact with them through the 
workspace toolbar menu **Sample virtual HID**.

Both plug-in types imitate hardware that has one axis and three buttons.
When activated using the workspace menu in Live mode, the plug-ins will start panning the selected active PTZ camera, 
imitating human interaction with the joystick. Using the same menu, it is possible to press virtual joystick buttons and see how 
assigned Smart Client actions get activated.

The `AutodiscoveryHidPlugin` will register a ***single*** `AutodiscoveryHidInstance` that represents a virtual joystick. In a real-life 
scenario, the plug-in may create several instances of auto-discovery plug-ins (e.g., when the operator connects multiple devices to the PC).

The `ManualDiscoveryHidPlugin` can create several instances of the `ManualDiscoveryInstance`. To establish a connection to the 
virtual joystick, the `ManualDiscoveryInstance` will query the operator for the ***Device initialization string***.

**Note:** A good starting point for developing a new HID plug-in would be to use the project template `MIP HID Plugin Solution` 
(part of the Milestone MIP SDK Templates VS extension).

## The Sample Demonstrates

- How to develop HID plug-ins for the Smart Client
- How to implement auto- and manual discovery plug-ins
- How to build a dynamic workspace toolbar menu

## Using

- `VideoOS.Platform.Client`
- `VideoOS.Platform.Client.Hid`
- `VideoOS.Platform.UI.Controls`

## Environment

- Smart Client MIP Environment

## Visual Studio C# Project

- [SCHidPlugin.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln'))
