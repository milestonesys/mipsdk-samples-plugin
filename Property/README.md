---
description: The Property sample is a plugin sample that explores saving
  and retrieval of values from the XProtect server in various plugin
  locations.
keywords:
- Plug-in integration
- Management Client
- Smart Client
- All MIP environments
lang: en-US
title: Property
---

# Property

The Property sample is a plugin sample that explores saving and
retrieval of values from the XProtect server in various plugin
locations.

## Management Client

![Management Client](PropertyMC.png)

## Smart Client

### View Item

![Smart Client View Item](propViewItem.png)

### Side Panel

![Smart Client View Item](Propertysidep.png)

### WorkSpace

![Smart Client WorkSpace](PropWorkspace.png)

### Options

![Smart Client Options](Propertysettings.png)

### Setup Properties

![Smart Client Setup Properties](Propertyproperties.png)

## All MIP environments background plugin

The background plugin reads the global version of the property and
writes to the MIP log.

## The sample demonstrates

How the option properties can be read and written from various places.
The properties are stored on the server.

## Using

- GetOptionsConfiguration / SaveOptionsConfiguration
- LoadProperties -- GetProperty / SetProperty - SaveProperties
- VideoOS.Platform.Admin.ToolsOptionsDialogPlugin /
  VideoOS.Platform.Client.OptionsDialogPlugin
- VideoOS.Platform.Client.ViewItemManager
- VideoOS.Platform.Client.SidePanelPlugin

## Environment

- The sample is relevant for all MIP environments

## Visual Studio C\# project

- [Property.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln');)
