# Overview

This project contains the full source code for the [Mesh Maker VR](http://store.steampowered.com/app/576790/Mesh_Maker_VR/) UI system. It includes Buttons, Panels, and automatic layout tools like Row and Column containers. It also provides File Open and SaveAs dialogs using a PhysX based kinetic scroller.
Scaffold scripts are provided to quickly build these components from the Editor or from code. The intent is to save time when designing complex VR application UIs.

# Dependencies

* Download the SteamVR Plugin from the Unity Asset Store. The example scene expect it to be at Assets/SteamVR.
* If you want to use VRTK (not required) download VRTK via git and copy Assets/VRTK over.

# NOTES

* The current version of VRTK adds "None" to the Supported VR SDK manager in Settings -> Player, so I've added a UseOpenVR gameobject with a UseOpenVR script to automatically switch back to OpenVR. This is not necessary in your code if you don't use VRTK.
* The TranslucentController.cs script only works with SteamVR at the moment (because VRTK provides no way to change the default controller model material), so the VRTK examples only work with SteamVR. This is the only VRTK limitation and could be solved with custom controller models.

# Naming

* Controller - Touch controller specific scripts.
* Panel - The 3d background for a UI.
* Container - An object that controllers the layout of child objects.
* Factory - A script to quickly build aggregate objects.

# Demo
Compiled demo exe available here: [https://github.com/createthis/VRUI_demo](https://github.com/createthis/VRUI_demo)

![keyboard image](http://i.imgur.com/650cDDP.gif "Keyboard")
![toggle buttons image](http://i.imgur.com/k4CysCr.gif "Toggle Buttons")
