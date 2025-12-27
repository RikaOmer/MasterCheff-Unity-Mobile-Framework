# MasterCheff - Unity Mobile Game Infrastructure

Professional Unity Mobile Game Framework with all essential systems for mobile game development.

## 📁 Project Structure

```
Assets/
├── Audio/              # Audio files
├── Prefabs/            # Ready-to-use prefabs
├── Resources/          # Dynamic resources
├── Scenes/             # Game scenes
├── Scripts/
│   ├── Core/           # Base classes (Singleton, GameState, Constants)
│   ├── Data/           # Data structures
│   ├── Input/          # Input handling
│   ├── Managers/       # System managers
│   ├── UI/             # UI components
│   └── Utils/          # Utilities
└── Sprites/            # Graphics
```

## 🎮 Main Systems

- **GameManager** - Game flow and state management
- **AudioManager** - Music, SFX, and volume control
- **UIManager** - Panels, popups, and navigation
- **SaveManager** - Save/load with encryption support
- **EventManager** - Central event system
- **SceneLoader** - Scene transitions with loading screens
- **TouchInputHandler** - Tap, Swipe, Pinch, Hold, Drag
- **ObjectPool** - Object pooling for performance

## 📱 Mobile Support

- SafeAreaHandler for notched devices
- MobileUtils for device info, battery, network
- Automatic quality adjustment
- Haptic feedback support

## 📋 Requirements

- Unity 2021.3 LTS or higher
- TextMeshPro (included in Unity)

## 📄 License

MIT License - Free for commercial and personal use
