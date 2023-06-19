---
description: The server side carousel demonstrates how one configuration can
  be created in the Management Client and shared among many Smart Clients.
keywords: Plug-in integration
lang: en-US
title: Server Side Carousel
---

# Server Side Carrousel

The server side carrousel demonstrates how one configuration can be
created in the Management Client and shared among many Smart Clients.

In this sample, a set of cameras and presets can be selected and saved
under a name. Multiple configurations of carrousels can be defined.

The configurations are stored on the server containing the camera
configuration, and Smart Client plug-ins can then get access to these
configurations via the MIP Environment in the Smart Client.

![Configuration in XProtect Management Client](carrouselconfig.jpg)

## Avoid blocking UI thread

Generally, our samples focus on how to use the MIPSDK. This means that 
we avoid having to much exception and thread handling which 
a production ready solution should have. In this sample, we demonstrate
a way to encapsulate calls to `VideoOS.Platform.Configuration` in seperate threads
to avoid blocking the UI thread. We use a combination of tasks and async methods to
accomplish this. 

For basic information about asynchronous programming, read Microsofts introduction 
to the subject here: [Asynchronous programming](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)

## The sample demonstrates

- How to work with configuration of plug-in defined items
- How to store and get plug-in defined configurations
- How to call `VideoOS.Platform.Configuration` asynchronously 

## Using

- VideoOS.Platform.Admin.ItemNode
- VideoOS.Platform.Admin.ItemManager
- VideoOS.Platform.UI.ItemPickerUserControl
- Get and SetConfiguration on `VideoOS.Platform.Configuration`

## Environment

- MIP Environment in XProtect Management Client
- MIP Environment in XProtect Smart Client

## Visual Studio C\# project

- [ServerSideCarrousel.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln');)

## Special notes

![Smart Client plug-in in Setup mode](Carrousel_sc.png)

Smart Client plug-in in Setup mode: Select one of the server side
defined configurations in the Properties pane on the left.

![Smart Client in Live mode](Carrousel_sc2.png)

Smart Client in Live mode: The selected carrousel configuration is
displayed, indicated by a green border around the position in the view.
