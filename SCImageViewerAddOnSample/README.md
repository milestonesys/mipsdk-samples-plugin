---
description: The sample demonstrates how to use independent playback 
  and digital zoom through the ImageViewerAddOn class. 
keywords:
- Plug-in integration
- Smart Client
- ImageViewerAddOn
lang: en-US
title: Smart Client ImageViewerAddOn
---

# Smart Client ImageViewerAddOn

This sample demonstrates how to use the ImageViewerAddOn to control 
independent playback and digital zoom. 

The sample shows how independent playback mode of a camera can be 
enabled or disabled as well as the playback can be started or stopped. 

Digital zoom can also be enabled or disabled and the sample has 
controls to digitally navigate the camera. As another option for 
controlling digital zoom, the sample shows how to use DigitalZoomRectangle
to specify a square that will be zoomed to. 

To enable digital zoom or independent playback, choose a camera
view item and then click on the enable button in the left-hand sidebar. 
Once enabled, the controls corresponding to the enabled functionality 
become active.


![](CameraPlaybackController.png)

## The sample demonstrates

-   How independent playback mode and playback controller can be
    used to control the video play from an individual camera.
-   How digital zoom is activated and used. 

## Using

-   VideoOS.Platform.Client.SidePanelWpfUserControl
-   VideoOS.Platform.ClientControl.NewImageViewerControlEvent
-   VideoOS.Platform.Client.ImageViewerAddOn
-   VideoOS.Platform.Client.ImageViewerAddOn.IndependentPlaybackEnabled
-   VideoOS.Platform.Client.ImageViewerAddOn.IndependentPlaybackController
-   VideoOS.Platform.Client.ImageViewerAddOn.DigitalZoomEnabled
-   VideoOS.Platform.Client.ImageViewerAddOn.DigitalZoomRectangle
-   VideoOS.Platform.Client.ImageViewerAddOn.DigitalZoomMove

## Environment

-   Smart Client MIP Environment

## Visual Studio C\# project

-   [SCImageViewerAddOnSample.csproj](javascript:openLink('..\\\\PluginSamples\\\\SCImageViewerAddOnSample\\\\SCImageViewerAddOnSample.csproj');)
