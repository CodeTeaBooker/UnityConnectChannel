# UnityConnectChannel

**A flexible and decoupled event system for Unity using ScriptableObjects. Simplify communication between different parts of your game without direct references.**

<p align="center">
  <a href="https://github.com/CodeTeaBooker/UnityConnectChannel/releases/latest">
    <img alt="Latest Release" src="https://img.shields.io/github/v/release/CodeTeaBooker/UnityConnectChannel?include_prereleases&label=latest%20release&color=blueviolet">
  </a>
  <a href="https://github.com/CodeTeaBooker/UnityConnectChannel/blob/main/LICENSE">
    <img alt="License" src="https://img.shields.io/github/license/CodeTeaBooker/UnityConnectChannel?color=blue">
  </a>
</p>

## Table of Contents

* [Overview](#overview)
* [Features](#features)
* [Getting Started](#getting-started)
    * [Installation](#installation)
    * [Quick Start](#quick-start)
* [Usage Examples](#usage-examples)
    * [Broadcasting Events](#broadcasting-events)
    * [Responding to Events](#responding-to-events)
* [Supported Event Types](#supported-event-types)
* [Creating Custom Event Types](#creating-custom-event-types)
* [Demo](#demo)
* [Contributing](#contributing)
* [License](#license)

## Overview

UnityConnectChannel implements the publish-subscribe pattern using ScriptableObject-based event channels. This enables the seamless decoupling of systems in your Unity project. Rather than maintaining direct references between components, publishers raise events on **Event Channels** (ScriptableObjects) while subscribers capture these events through **Event Listeners** (MonoBehaviours). This architecture eliminates tight coupling, resulting in more modular and maintainable code that's easier to test and more resilient to changes.

## Features

* **Decoupled Communication**: Eliminate direct dependencies between GameObjects and systems.
* **ScriptableObject Based**: Events are assets, making them easy to manage and reference.
* **Type-Safe Events**: Built-in support for common data types (Void, Int, String, Bool) and easy extension for custom types.
* **Inspector Friendly**: Configure listeners and responses directly in the Unity Inspector.
* **Conditional Event Raising**: Trigger events only when specific conditions are met.
* **Debug Capabilities**: Optional logging for event raising and listener registration.
* **Thread Safety**: Designed with thread safety in mind for listener management.
* **Recursive Event Protection**: Prevents stack overflows from recursive event loops.
* **GUID-based Identification**: Unique IDs for channels for easier tracking.

## Getting Started

### Installation

You have a couple of options to install UnityConnectChannel into your Unity project:

**Option 1: Downloading a `.unitypackage` from Releases (Recommended)**

1.  Go to the [**Releases Page**](https://github.com/CodeTeaBooker/UnityConnectChannel/releases) of this repository.
2.  Download the `UnityConnectChannel-vx.x.x.unitypackage` file (where `x.x.x` is the version number) from the latest (or desired) release.
3.  Open your Unity project.
4.  Drag and drop the downloaded `.unitypackage` file into your Project window, or go to `Assets > Import Package > Custom Package...` and select the file.
5.  Click "Import" in the dialog that appears. All necessary files from the `DevToolKit` folder will be imported into your `Assets` directory.

**Option 2: Manual Installation (from source)**

1.  Download or clone this repository.
2.  Navigate to the `Assets/DevToolKit` folder within the cloned/downloaded repository.
3.  Copy this `DevToolKit` folder into your Unity project's `Assets` folder.

### Quick Start

1.  **Create an Event Channel**:
    * Right-click in the Project view within your Unity Editor.
    * Select `Create > Events > [Type]EventChannel` (e.g., `StringEventChannel`).
    * Name your newly created Event Channel asset (e.g., `PlayerNameChangedChannel`).

2.  **Setup a Publisher** (a script that will raise the event):
    ```csharp
    using DevToolKit.EventChannel.Channels; 
    using UnityEngine;

    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private StringEventChannel playerNameChannel;

        public void ChangePlayerName(string newName)
        {
            if (playerNameChannel != null)
            {
                playerNameChannel.Raise(newName);
            }
            else
            {
                Debug.LogWarning("PlayerNameChannel is not assigned in the PlayerManager.");
            }
        }
    }
    ```

3.  **Setup a Listener** (a component that will react to the event):
    * Select a GameObject in your scene.
    * In the Inspector, click "Add Component" and search for the corresponding listener type (e.g., `StringEventListener`).
    * Drag your created `PlayerNameChangedChannel` asset from the Project view to the "Event Channel" field on the `StringEventListener` component.
    * In the "Unity Event Response" section of the listener, click the "+" button to add a new response.
    * Drag the GameObject containing the script with the method you want to call into the object field.
    * From the function dropdown, select the public method you want to execute when the event is raised (e.g., a UI update method that takes a string).

## Usage Examples

### Broadcasting Events

```csharp
// Make sure to assign these channels in the Inspector!
// For VoidEventChannel (no data)
[SerializeField] private VoidEventChannel gameStartChannel;
public void StartGame() 
{
    if (gameStartChannel != null) gameStartChannel.Raise(); 
}

// For IntEventChannel
[SerializeField] private IntEventChannel scoreChannel;
public void UpdateScore(int newScore) 
{
    if (scoreChannel != null) scoreChannel.Raise(newScore);
}

// For conditional raising (example with BoolEventChannel)
[SerializeField] private BoolEventChannel achievementUnlockedChannel;
public void CheckAndUnlockAchievement(int playerScore) 
{
    if (achievementUnlockedChannel != null)
    {
        // Raise the event with 'true' (achievement unlocked) if score > 1000
        // The first parameter to RaiseIf is the value to pass if the condition is true.
        // For a BoolEventChannel, if the condition is met, it will raise 'true'.
        achievementUnlockedChannel.RaiseIf(true, val => playerScore > 1000); 
    }
}
```

### Responding to Events

The event listener component (`VoidEventListener`, `IntEventListener`, `StringEventListener`, `BoolEventListener`, or your custom listeners) handles the subscription. You just need to:

1.  Add the appropriate listener component to your GameObject.
2.  Assign your Event Channel asset to its "Event Channel" field.
3.  Configure the UnityEvent response in the Inspector by linking it to a public method.

```csharp
// Example for a UIController responding to score updates
// This script would be on a GameObject, and the UpdateScoreDisplay method
// would be linked to an IntEventListener's UnityEvent.

using UnityEngine;
// Using Unity's built-in UI Text for this example, as per user's previous update.
using UnityEngine.UI; 

public class UIController : MonoBehaviour 
{
    [SerializeField] private Text scoreText; // Standard UI Text

    // This method will be connected in the Inspector 
    // to an IntEventListener's "Unity Event Response"
    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
```

## Supported Event Types

UnityConnectChannel includes these pre-configured event channels and listeners:

* **Void**: For events that don't carry data (`VoidEventChannel` / `VoidEventListener`)
* **Int**: For integer data (`IntEventChannel` / `IntEventListener`)
* **String**: For string data (`StringEventChannel` / `StringEventListener`)
* **Bool**: For boolean data (`BoolEventChannel` / `BoolEventListener`)

Each type also has a corresponding Unity event class for inspector use:

* `UnityVoidEvent`, `UnityIntEvent`, `UnityStringEvent`, `UnityBoolEvent`

## Creating Custom Event Types

Extend the system with your own data types in four easy steps:

1.  **Define your custom data type**:
    ```csharp
    [System.Serializable]
    public class CustomType
    {
        public int id;
        public string name;
    }
    ```

2.  **Create a custom event channel**:
    ```csharp
    using DevToolKit.EventChannel.Core.Abstractions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewCustomTypeEventChannel", menuName = "Events/CustomTypeEventChannel")]
    public class CustomTypeEventChannel : EventChannelBase<CustomType>
    {
    }
    ```

3.  **Create a custom event listener**:
    ```csharp
    using DevToolKit.EventChannel.Core.Abstractions;

    public class CustomTypeEventListener : EventListener<CustomType, CustomTypeEventChannel>
    {
    }
    ```

4.  **Create a custom UnityEvent**:
    ```csharp
    using UnityEngine.Events;

    [System.Serializable]
    public class UnityCustomTypeEvent : UnityEvent<CustomType>
    {
    }
    ```

## Demo

The included demo scene (`Assets/DevToolKit/Demos/DemoScene.unity`) demonstrates:

* Multiple event types (void, int, string, bool, custom)
* UI interaction with the event system (using standard Unity UI Text components).
* Custom type event communication.
* Debug logging capabilities.

The `DemoScript.cs` and `UIManager.cs` files in `Assets/DevToolKit/Demos/Scripts/` provide practical examples of publishing and subscribing to events.

## Contributing

Contributions are welcome! If you'd like to contribute, please follow these steps:

1.  Fork the repository.
2.  Create a new branch (`git checkout -b feature/AmazingFeature`).
3.  Make your changes.
4.  Commit your changes (`git commit -m 'Add some AmazingFeature'`).
5.  Push to the branch (`git push origin feature/AmazingFeature`).
6.  Open a Pull Request.

Please ensure your code adheres to the existing style and that any new features are well-tested.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Made with ❤️ by CodeTeaBooker**
