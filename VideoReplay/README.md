---
description: The video replay sample demonstrates how to get hold of a
  number of video frames from the recorded database.
keywords: Plug-in integration
lang: en-US
title: Smart Client Video Replay
---

# Smart Client Video Replay

The video replay sample demonstrates how to get hold of a number of
video frames from the recorded database.

To setup, create a view, containing one camera and the plug-in sample.
When you select a camera and click the \"Replay last 150 frames
button\", the plug-in asks the MIP Environment for a maximum of 150
frames starting 15 seconds before.

When received, it will place these in a .NET image, 2 times faster
than the original recording. The plug-in then waits 2 seconds and
replays the sequence again.

![Video Replay ViewItem](VideoReplay.png)

## The sample demonstrates

- How to get hold of recorded images
- Listen to currently selected camera in the Smart Client
- How to understand when the Smart Client changes mode
- How to make a simple print of currently displayed ViewItem

## Using

- VideoOS.Platform.Data.JPEGVideoSource
- VideoOS.Platform.Client.ViewItem.Print method

## Environment

- Smart Client MIP Environment

## Visual Studio C\# project

- [VideoReplay.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln');)

## Special notes

The video frames received from the MIP environment have been transcoded
on the client PC to a set of bitmap images. When you click the Print
button and select the Video Replay sample, the
following dialog is displayed and it is ready for printing:

![Video Replay Report](VideoReplayPrint.png)
