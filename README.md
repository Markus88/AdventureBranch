# AdventureBranch

**Genre:** 3D Strategy RPG with Auto-battler elements and Rogue-like elements  
**Platform:** PC (Unity-based, cross-platform)  
**Engine:** Unity 2022.3.21f1

AdventureBranch is a Unity-based 3D game that combines multiple gameplay mechanics from successful titles to create a unique gaming experience. The game blends auto-battler mechanics, world exploration, hero management, and romance elements in a cohesive package.

## 🎮 Game Features

### Core Gameplay Mechanics

- **Auto-battler Combat System**: Strategic combat where heroes fight automatically based on AI decisions
- **World Exploration**: Procedurally generated map with rogue-like elements
- **Hero Management**: Recruit, upgrade, and customize heroes with different classes and abilities
- **Romance System**: Build relationships with characters through dialogue and interactions
- **Resource Management**: Collect and manage various resources during exploration

### Hero Classes

- **Warrior**: High health and attack, strong defense
- **Mage**: High magic power, area of effect abilities
- **Archer**: High speed and ranged attacks
- **Healer**: Support abilities, keeps team alive
- **Rogue**: High speed, critical hit specialist
- **Paladin**: Balanced stats with healing and defense

### World Features

- **Procedural Generation**: Each playthrough offers a unique world layout
- **Multiple Terrain Types**: Plains, Forest, Mountains, Desert, Swamp, Ruins
- **Random Encounters**: Combat, Treasure, Merchants, Romance events, Quests
- **Resource Nodes**: Gold, Iron, Wood, Stone, Herbs, Gems

## 🛠️ Technical Architecture

### Core Systems

- **GameManager**: Central game state management and scene transitions
- **CombatManager**: Handles auto-battler combat mechanics
- **WorldManager**: Manages procedural world generation and exploration
- **RomanceManager**: Handles character relationships and dialogue
- **AudioManager**: Centralized audio system for music and SFX
- **SaveSystem**: Game state persistence and loading

### Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # Core game systems
│   ├── Combat/         # Combat and battle mechanics
│   ├── Hero/           # Hero management and stats
│   ├── World/          # World generation and exploration
│   ├── UI/             # User interface systems
│   ├── Audio/          # Audio management
│   └── Utils/          # Utility classes and helpers
├── Prefabs/
│   ├── Characters/     # Hero and enemy prefabs
│   ├── UI/             # UI prefabs and components
│   ├── Environment/    # World objects and terrain
│   └── Effects/        # Visual and audio effects
├── Scenes/             # Game scenes
├── Materials/          # 3D materials and shaders
├── Textures/           # 2D textures and sprites
├── Models/             # 3D models (FBX, OBJ, GLB, etc.)
├── Audio/
│   ├── Music/          # Background music tracks
│   └── SFX/            # Sound effects
└── Resources/          # Runtime-loaded assets
```

## 🎨 3D Asset Integration

The game supports multiple 3D model formats from **meshy.ai**:
- **FBX** - Primary format for animated characters
- **OBJ** - Static environment objects
- **GLB** - Optimized runtime models
- **USDZ** - High-quality scene models
- **STL** - Simple geometry objects
- **Blend** - Direct Blender integration
- **3MF** - 3D manufacturing format

## 🚀 Getting Started

### Prerequisites

- Unity 2022.3.21f1 or later
- Basic understanding of Unity development
- Git for version control

### Development Setup

1. Clone the repository
2. Open project in Unity 2022.3.21f1+
3. Let Unity import all packages and assets
4. Open the MainMenu scene to start development
5. Use the GameManager to test different game states

### Building the Game

1. Go to File > Build Settings
2. Add scenes in this order:
   - MainMenu
   - WorldMap
   - Combat
   - HeroManagement
3. Select target platform (PC/Mac/Linux)
4. Click Build to create executable

## 🎯 Game Flow

1. **Main Menu** → Player starts here, can begin new game or load existing
2. **World Exploration** → Navigate procedurally generated map
3. **Random Encounters** → Trigger events based on exploration
4. **Combat** → Auto-battler system resolves battles
5. **Hero Management** → Upgrade heroes, manage equipment
6. **Romance** → Interact with characters, build relationships

## 🔧 Development Notes

### Key Design Patterns

- **Singleton Pattern**: Used for managers (GameManager, AudioManager, etc.)
- **Event System**: Decoupled communication between systems
- **State Machine**: Game state management and transitions
- **Observer Pattern**: UI updates and game event handling

### Extension Points

- **New Hero Classes**: Add to HeroClass enum and implement stat distributions
- **Additional Terrain**: Extend TerrainType enum and generation logic
- **Custom Encounters**: Create new EncounterType and event handlers
- **Romance Options**: Add new characters to RomanceManager

## 📝 Future Enhancements

- [ ] Multiplayer auto-battler mode
- [ ] Advanced skill trees and abilities
- [ ] Crafting and item creation system
- [ ] Guild system and social features
- [ ] Seasonal events and content updates
- [ ] Mobile platform support
- [ ] Mod support and custom content

## 🤝 Contributing

This is a demonstration project showcasing Unity game development architecture. The codebase is designed to be extensible and maintainable for future development.

## 📄 License

This project is for educational and demonstration purposes. All code is provided as-is for learning Unity game development patterns and architecture.