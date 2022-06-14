---
description: The RGB Video Enhancement sample is utilizing the media
  toolkit to retrieve live and playback images from any XProtect server,
  delivered as RGB image.
keywords:
- Plug-in integration
- Smart Client
lang: en-US
title: Smart Client RGB Video Enhancement
---

# Smart Client RGB Video Enhancement

The RGB Video Enhancement sample utilizes the media toolkit to
retrieve live and playback images from any XProtect server, delivered as
RGB image.

The RGB formatted images, as well as a number of other formats, can be
retrieved directly in a Smart Client plugin.

This can be used to create pixel manipulation with the RGB image before
it is displayed to the operator. The sample has a simple C++ library
routine that can adjust colors and color offset to illustrate the
corporation between the media toolkit as given in C\# with a C++
library.

Please ensure to have a look at the special notes below for instructions
on how to build and run this sample.

![](rgb.png)

The screenshot has the same camera displayed in a normal camera view
item (right side), and the RGB sample on the left hand side. As the
scrollbars are adjusted the following images will be transformed as newest
values.

## Setting up the plugin

Install the plugin like any other Milestone Plugin. Start the Smart Client 
and go into setup mode. In setup mode open "MIP SDK Samples", in there drag 
"RGBVideoEnhancement" into an camera view. Then, select the "RGBVideoEnhancement" 
you have just dragged. Scroll down in the side panel to the left and find the button 
"Select camera...", this button is only visible when the "RGBVideoEnhancement" is selected.
Click on the button to choose a camera to view in the plugin. 

## The sample demonstrates

-   Selecting a camera
-   Retrieve live bitmaps
-   Retrieve live status packets
-   Changing between live and playback tab/workspace
-   Synchronizing playback images in playback with specified time

## Using

-   VideoOS.Platform.Live.BitmapLiveSource.cs
-   VideoOS.Platform.Live.LiveSourceBitmapContent.cs
-   VideoOS.Platform.Live.LiveSourceStatus.cs

## Environment

-   Smart Client MIP Environment
-   .Net Library MIP Environment -- (Shown in another sample)

## Visual Studio C\# project

-   [RGBVideoEnhancement.csproj](javascript:openLink('..\\\\PluginSamples\\\\RGBVideoEnhancement\\\\RGBVideoEnhancement.csproj');)

## Special notes

In order to build it you must first select the correct platform toolset
for the RGBEnhancement project and then build this project.

Also, you must deploy RGBVideoEnhancement.dll and RGBEnhancement.dll under the plugin folder. 
