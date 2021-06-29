---
description: This sample includes areas that show most of the messages
  supported in the Smart Client.
keywords: Plug-in integration
lang: en-US
title: Smart Client View and Window Tool
---

# Smart Client View and Window Tool

This sample includes areas that show most of the messages supported in
the Smart Client.

The areas are divided into tabs:

-   Multi Window -- How to work with floating windows
-   PTZ Move -- Controlling cameras
-   PTZ Absolute -- How to get and set / go to a specific absolute
    position
-   Indicator & App - Smart Client application control
-   Lens Control -- Commands that can be sent to a camera
-   Playback Control -- Smart Client playback control via programming
-   AUX -- Extension to PTZ control for specific devices
-   View Create -- How to create a new view layout and assign views
-   View Control -- Controlling options for a view
-   View Edit -- How to edit an existing view layout
-   View Dump -- Shows a tree view will all known views
-   Workspaces - How to work with workspaces

![Smart Client View and Windows Tool](smartclientview_windowstool.jpg)

The current selected tab shows that 6 messages can be sent to control
the camera blinking LEDs, and 6 messages to control how certain Smart
Client parts should appear.

## The sample demonstrates

-   How to work with messages
-   Assist in developing plug-ins

## Using

-   VideoOS.Platform.EnvironmentManager
-   VideoOS.Platform.Messaging

## Environment

-   MIP Environment for Smart Client

## Visual Studio C\# project

-   [SCViewAndWindow.csproj](javascript:openLink('..\\\\PluginSamples\\\\SCViewAndWindow\\\\SCViewAndWindow.csproj');)

## Special notes

Many of the messages and events used in this sample are only relevant
for the Smart Client, if sent in other environment, they will simply be
ignored.

Lens control: It should be observed that camera support of these
commands vary greatly from camera to camera.
