---
description: This sample shows how video sequences from one or more
  cameras can be exported to a single AVI file.
keywords: Plug-in integration
lang: en-US
title: Smart Client AVI Sequence Export
---

# Smart Client AVI Sequence Export

This sample shows how video sequences from one or more cameras can be
exported to a single AVI file.

The sample enables you to select a number of cameras, and to export for
each a specific time period. It also allows you to specify an overlay
text to be put on each sequence and the time stamp of each frame to be
written as an overlay.

The sequences will always be written to the AVI file in the order they
are specified - regardless of the actual chronological order.

Please note that the same functionality can be implemented in a
Component Integration application.

The plugin requires Smart Client 2017 R3 or later to work, as the
sequential AVI export is only supported from that version.

![](AviSequenceExport.png)

## The sample demonstrates

- How the AVIExporter can be used to concatenate multiple video
  sequences into one AVI file.

## Using

- VideoOS.Platform.Data.AVIExporter
- VideoOS.Platform.Data.SequenceAviExportElement

## Environment

- Smart Client MIP Environment

## Visual Studio C\# project

- [SCAviSequenceExport.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln');)
