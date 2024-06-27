---
description: This sample shows how to create and operate toolbar plug-ins
  in the Smart Client. Toolbar plug-ins can be placed in the workspace
  and in the view item toolbars. Workspace toolbar items should activate
  functionality relating to the workspace and view item toolbar items
  should activate functionality relating to the view items. Toolbar
  plug-ins can specify which workspace, workspace state and view item
  type they should be displayed for. Workspace toolbar plug-ins can be 
  put under workspace toolbar plug-in groups to organize them better.
keywords: Plug-in integration
lang: en-US
title: Smart Client Toolbar Plug-in
---

# Smart Client Toolbar Plug-in

This sample shows how to create and operate toolbar plug-ins in the Smart
Client. Toolbar plug-ins can be placed in the workspace and in the view
item toolbars. Workspace toolbar items should activate functionality
relating to the workspace and view item toolbar items should activate
functionality relating to the view items. Toolbar plug-ins can specify
which workspace, workspace state and view item type they should be
displayed for. Workspace toolbar plug-ins can be put under a group 
by setting their <i>GroupId</i> property to the id of a workspace toolbar 
plug-in group. These groups are displayed as menu buttons in workspace toolbar
and help with organizing workspace toolbar plug-ins.

The sample demonstrates how to create workspace and view item toolbar 
plug-ins, and workspace toolbar groups. Three toolbar items of workspace 
and view item plug-ins are created. A workspace toolbar plug-in group is 
created, and workspace toolbar items are set to be displayed under this group. 
The toolbar items will, when activated, change the background color of the MIP 
view item called 'Background color View Item' which is also included in this 
sample. The workspace toolbar items will change the color for all the view 
items and the view item toolbar items will only change color for its related 
view item.

![](SCToolbar.png)

## The sample demonstrates

- Create workspace toolbar plug-ins
- Create a workspace toolbar plug-in group
- Put workspace toolbar plug-ins under the workspace toolbar plug-in group
- Create view item toolbar plug-ins
- Controlling the state of the toolbar item
- Create a view item plug-in
- Communication between MIP plug-ins

## Using

- VideoOS.Platform.Client.ViewItemToolbarPlugin
- VideoOS.Platform.Client.ViewItemToolbarPluginInstance
- VideoOS.Platform.Client.ViewItemToolbarPlaceDefinition
- VideoOS.Platform.Client.WorkSpaceToolbarPlugin
- VideoOS.Platform.Client.WorkSpaceToolbarPluginInstance
- VideoOS.Platform.Client.WorkSpaceToolbarPluginGroup
- VideoOS.Platform.Client.WorkSpaceToolbarPlaceDefinition

## Environment

- Smart Client MIP Environment

## Visual Studio C\# project

- [SCToolbarPlugin.csproj](javascript:clone('https://github.com/milestonesys/mipsdk-samples-plugin','src/PluginSamples.sln');)
