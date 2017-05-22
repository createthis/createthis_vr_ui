# Overview

This project contains the full source code to the Mesh Maker VR UI system. It includes Buttons, Panels, and automatic layout tools like Row and Column containers. It also provides File Open and SaveAs dialogs using a PhysX based kinetic scroller.
The intent is to save time when designing complex VR application UIs.

# Dependencies

* Download the SteamVR Plugin from the Unity Asset Store. The example scene expect it to be at Assets/SteamVR.
* Download VRTK via git and copy Assets/VRTK over.

# NOTES

* The current version of VRTK adds "None" to the Supported VR SDK manager in Settings -> Player, so you will need to remove "None" anytime you switch from the VRTK scene to the non VRTK scene.
* The TranslucentController.cs script only works with SteamVR at the moment (because VRTK provides no way to change the default controller model material), so the VRTK examples only work with SteamVR. This is the only VRTK limitation and could be solved with custom controller models.

# Naming

* Controller - Touch controller specific scripts.
* Panel - The 3d background for a UI.
* Container - An object that controllers the layout of child objects.
