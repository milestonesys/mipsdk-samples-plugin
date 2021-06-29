---
description: The AddUserWithConfigApi plugin demonstrates how the
  configuration API can be used from a Smart Client plugin. It enables a
  Smart Client operator to create a new basic user and add it to a
  chosen role.
keywords:
- Plug-in integration
- Smart Client
lang: en-US
title: Smart Client Add User
---

# Smart Client Add User

The AddUserWithConfigApi plugin demonstrates how the configuration API
can be used from a Smart Client plugin. It enables a Smart Client
operator to create a new basic user and add it to a chosen role.

![](AddUserSidePlugin.png)

## How to use it

-   Start the Smart Client. Login to XProtect system
-   Find the 'Add User Side panel'
-   Enter a user name and a password, select a role and press 'Commit'
-   Check the 'Result' pane for the result of the request

## The sample demonstrates

-   How to create and modify object using configuration API

## Using

-   Configuration API resources:
    -   /BasicUserFolder
    -   /Role\[...\]/UserFolder
    -   /RoleFolder
-   VideoOS.ConfigurationAPI.ItemTypes
-   VideoOS.ConfigurationAPI.ConfigurationItem
-   VideoOS.ConfigurationAPI.IConfigurationService

## Visual Studio C\# project

-   [AddUserWithConfigApi.csproj](javascript:openLink('..\\\\PluginSamples\\\\AddUserWithConfigApi\\\\AddUserWithConfigApi.csproj');)
