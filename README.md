# Overview

This project contains the full source code for the [Mesh Maker VR](http://store.steampowered.com/app/576790/Mesh_Maker_VR/) UI system. It includes Buttons, Panels, and automatic layout tools like Row and Column containers. It also provides File Open and SaveAs dialogs using a PhysX based kinetic scroller.
Factory scripts are provided to quickly build these components from the Editor or from code. The intent is to save time when designing complex VR application UIs.

[![Youtube Video Overview](https://img.youtube.com/vi/4BQ3y7y577U/0.jpg)](https://www.youtube.com/watch?v=4BQ3y7y577U)
[![VR UI Profiles](https://img.youtube.com/vi/qdyMXNkXdQY/0.jpg)](https://youtu.be/qdyMXNkXdQY)

# Dependencies

* Download the SteamVR Plugin from the Unity Asset Store. The example scenes require SteamVR, though theoretically the VRTK scene could be modified to use Oculus SDK instead.
* If you want to use VRTK (not required) download VRTK via git and copy Assets/VRTK over. You will also need to add 
  a *Platform custom #define* to Unity to enable VRTK code extensions. To do this go to File -> Build Settings -> Other Settings -> Scripting Define Symbols
  and append `;VRTK` to the list. Make sure you press ENTER after typing VRTK or it won't save. Also, you'll probably want to exit Unity and delete your Library
  folder at this point in order to force Unity to recompile all scripts using the VRTK define.
  See https://docs.unity3d.com/Manual/PlatformDependentCompilation.html for more info.
# NOTES

* The current version of VRTK adds "None" to the Supported VR SDK manager in Settings -> Player, by default, so I've disabled the Auto Manage VR Settings checkbox on the VRTK object in the VRTK scene and defaulted the SDK to SteamVR.
* The TranslucentController.cs script only works with SteamVR at the moment (because VRTK provides no way to change the default controller model material), so the VRTK examples only work with SteamVR. This is the only VRTK limitation and could be solved with custom controller models.

# Naming

* Controller - Touch controller specific scripts.
* Panel - The 3d background for a UI.
* Container - An object that controllers the layout of child objects.
* Factory - A script to quickly build aggregate objects.
* Profile - A MonoBehaviour containing data necessary to populate a UI component.
* Defaults - static class that contain system wide default Profiles (See VRUI_Defaults gameobject in example scenes for corresponding MonoBehaviour).

# Demo
Compiled demo exe available here: [https://github.com/createthis/VRUI_demo](https://github.com/createthis/VRUI_demo)

![keyboard image](http://i.imgur.com/650cDDP.gif "Keyboard")
![toggle buttons image](http://i.imgur.com/k4CysCr.gif "Toggle Buttons")

# Chat

Slack channel: [createthisvrui.slack.com](https://createthisvrui.slack.com) [Join createthisvrui invite link](https://join.slack.com/createthisvrui/shared_invite/MTkxNTk5MzM3ODI0LTE0OTY0OTY1NzgtYTcwYmE2YjY2YQ)

# Patreon

Love this project and want to help it be successful? Support us on Patreon, today: [CreateThis VR UI Patreon Page](https://www.patreon.com/createthis)
