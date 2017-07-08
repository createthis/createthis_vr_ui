# Overview

This project contains the full source code for the [Mesh Maker VR](http://store.steampowered.com/app/576790/Mesh_Maker_VR/) UI system. It includes Buttons, Panels, and automatic layout tools like Row and Column containers.
It also provides File Open and SaveAs dialogs using a PhysX based kinetic scroller.
Factory scripts are provided to quickly build these components from the Editor or from code. The intent is to save time when designing complex VR application UIs.

# Tutorial Videos

[![Youtube Video Overview](https://img.youtube.com/vi/4BQ3y7y577U/0.jpg)](https://www.youtube.com/watch?v=4BQ3y7y577U)
[![VR UI Profiles](https://img.youtube.com/vi/qdyMXNkXdQY/0.jpg)](https://youtu.be/qdyMXNkXdQY)
[![Getting Started and Color Picker](https://img.youtube.com/vi/IHZIVWOqJCk/0.jpg)](https://youtu.be/IHZIVWOqJCk)

# Dependencies

* SteamVR Plugin - Download the SteamVR Plugin from the Unity Asset Store. All example scenes require SteamVR, though theoretically the VRTK scene could be modified to use Oculus SDK instead.
* Color Picker (optional) - Download this Free asset from the Unity Asset Store: [Simple color picker by Filipp Keks](https://www.assetstore.unity3d.com/#!/content/7353?aid=1100l35sb). Next, add the `;COLOR_PICKER` *Platform custom #define* (see instructions below).
* VRTK (optional) - Download VRTK via git and copy Assets/VRTK over. You will also need to add the `;VRTK` *Platform custom #define* (see instructions below).

# Platform Custom Defines

To add a *Platform custom #define* in Unity:
1. Navigate to `File -> Build Settings -> Other Settings -> Scripting Define Symbols`.
1. Append your custom define to the list. Make sure you press ENTER after typing the define or it won't save.
1. You may want to exit Unity and delete your Library folder in order to force Unity to recompile all scripts using the new custom define.

See https://docs.unity3d.com/Manual/PlatformDependentCompilation.html for more info.

# NOTES

* The current version of VRTK adds `None` to the Supported VR SDK manager in Settings -> Player, by default.
  This makes switching between non-vrtk and vrtk scenes difficult, so we've disabled the *Auto Manage VR Settings* checkbox on the VRTK object in the VRTK scene.
  We've also defaulted the SDK to SteamVR.
* The TranslucentController.cs script only works with SteamVR at the moment (because VRTK provides no way to change the default controller model material), so the VRTK examples only work with SteamVR.
  This is the only VRTK limitation and could be solved with custom controller models.
* Unfortunately, we can't include the `Simple color picker` source code in this project due to licensing. The Unity Asset Store licenses the "Simple color picker" asset as a Free download. This means
  it is free to download and use in your commercial binary product, but distribution of the source code is prohibited. We've reached out to the author to request MIT licensing, but received no response.


# Naming

* Controller - Touch controller specific scripts.
* Panel - The 3d background for a UI.
* Container - An object that controllers the layout of child objects.
* Factory - A script to quickly build aggregate objects.
* Profile - A MonoBehaviour containing data necessary to populate a UI component.
* Defaults - A static class that contains system wide default Profiles (See VRUI_Defaults gameobject in example scenes for corresponding MonoBehaviour).

# Demo

Compiled demo exe available here: [https://github.com/createthis/VRUI_demo](https://github.com/createthis/VRUI_demo)

![keyboard image](http://i.imgur.com/650cDDP.gif "Keyboard")
![toggle buttons image](http://i.imgur.com/k4CysCr.gif "Toggle Buttons")

# Chat

Slack channel: [createthisvrui](https://createthisvrui.slack.com) [(Join link)](https://join.slack.com/createthisvrui/shared_invite/MTkxNTk5MzM3ODI0LTE0OTY0OTY1NzgtYTcwYmE2YjY2YQ)

# Patreon

Love this project and want to help it be successful? Support us on Patreon, today: [CreateThis VR UI Patreon Page](https://www.patreon.com/createthis)
