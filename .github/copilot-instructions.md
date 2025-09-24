# Copilot Instructions for AdventureBranch

## Project Overview

**AdventureBranch** is a Unity-based 3D Strategy RPG that combines auto-battler mechanics, world exploration, hero management, and romance elements. The game draws inspiration from successful titles like Heroes of Might and Magic 3, Fire Emblem Three Houses, Mage and Monsters II, and Fields of Mistria.

**Engine**: Unity 2022.3.21f1  
**Genre**: 3D Strategy RPG with Auto-battler and Rogue-like elements  
**Platform**: PC (Unity-based, cross-platform)

## Architecture & Design Patterns

### Core Systems
- **GameManager**: Central state management using Singleton pattern
- **CombatManager**: Auto-battler combat mechanics with turn-based systems
- **WorldManager**: Procedural world generation and exploration
- **RomanceManager**: Character relationships and dialogue systems
- **AudioManager**: Centralized audio management
- **SaveSystem**: Game state persistence using JSON serialization

### Key Design Patterns
- **Singleton Pattern**: Used for all manager classes (GameManager, AudioManager, etc.)
- **Event System**: Decoupled communication between systems using UnityEvents
- **State Machine**: Game state management with clear state transitions
- **Observer Pattern**: UI updates and game event handling
- **Command Pattern**: For combat actions and game commands

### Project Structure
```
Assets/
├── Scripts/
│   ├── Core/           # Core game systems and managers
│   ├── Combat/         # Auto-battler combat mechanics
│   ├── Hero/           # Hero management and progression
│   ├── World/          # World generation and exploration
│   ├── UI/             # User interface systems
│   ├── Audio/          # Audio management
│   └── Utils/          # Utility classes and helpers
├── Prefabs/
│   ├── Characters/     # Hero and enemy prefabs
│   ├── UI/             # UI prefabs and components
│   ├── Environment/    # World objects and terrain
│   └── Effects/        # Visual and audio effects
├── Scenes/             # Game scenes (MainMenu, WorldMap, Combat, etc.)
├── Materials/          # 3D materials and shaders
└── Textures/           # 2D textures and sprites
```

## Game Systems & Mechanics

### Core Game States
- **MainMenu**: Entry point with New Game/Load/Settings options
- **WorldExploration**: Procedural map navigation with random encounters
- **Combat**: Auto-battler system with strategic unit placement
- **HeroManagement**: Character progression and equipment
- **Romance**: Relationship building and dialogue interactions

### Auto-battler Combat System
- Turn-based combat with automated execution
- Strategic unit placement on tactical grid
- Unit synergies and combo effects
- Multiple unit types: Tank, DPS, Support, Specialist
- AI-driven combat decisions with player strategy input

### World Generation
- Procedural world maps with diverse biomes
- Terrain types: Plains, Forest, Mountains, Desert, Swamp, Ruins
- Random encounters: Combat, Treasure, Merchants, Romance events
- Resource nodes: Gold, Iron, Wood, Stone, Herbs, Gems
- Exploration-based progression system

### Hero System
- Multiple hero classes with unique abilities
- Deep skill tree progression systems
- Equipment system with meaningful upgrades
- Stat-based character development
- Army leadership and buffing mechanics

## Coding Standards & Conventions

### C# Guidelines
- **Namespaces**: Use `AdventureBranch.<SystemName>` format
- **Classes**: PascalCase with descriptive names
- **Methods**: PascalCase for public, camelCase for private
- **Variables**: camelCase for private fields, PascalCase for public properties
- **Constants**: ALL_CAPS with underscores
- **Enums**: PascalCase with descriptive values

### Unity-Specific Patterns
- **MonoBehaviour Lifecycle**: Use Awake() for initialization, Start() for setup
- **Singleton Implementation**: Check for existing instance in Awake()
- **Coroutines**: Use for timed actions and async operations
- **ScriptableObjects**: For data containers and configuration
- **Prefab Organization**: Maintain clear hierarchy and naming

### Documentation Standards
- **XML Documentation**: Use for all public methods and classes
- **Inline Comments**: Explain complex logic and business rules
- **Region Blocks**: Organize large classes by functionality
- **TODO Comments**: Mark incomplete features clearly

## 3D Asset Integration

### Supported Formats
- **3D Models**: FBX, OBJ, GLB, USDZ, STL, Blend, 3MF
- **Asset Source**: Meshy.ai for 3D model generation
- **Materials**: Use Unity's Standard or URP materials
- **Textures**: Power-of-2 dimensions, appropriate compression

### Asset Organization
- **Models**: Organize by category (Characters, Environment, Props)
- **Materials**: Shared materials in dedicated folders
- **Textures**: Consistent naming conventions
- **Prefabs**: Complete gameobjects with all components

## Development Guidelines

### Extension Points
- **New Hero Classes**: Extend HeroClass enum and implement stat distributions
- **Additional Terrain**: Add to TerrainType enum and generation logic
- **Custom Encounters**: Create new EncounterType and event handlers
- **Romance Options**: Add characters to RomanceManager system

### Performance Considerations
- **Object Pooling**: For frequently spawned objects (combat units, effects)
- **Async Operations**: Use coroutines for long-running tasks
- **Memory Management**: Proper disposal of resources and event listeners
- **LOD Systems**: For 3D models based on distance
- **Texture Streaming**: For large texture sets

### Testing & Debugging
- **Scene Testing**: Each scene should be testable independently
- **Debug Logging**: Use consistent logging levels and categories
- **Inspector Values**: Expose key parameters for runtime tuning
- **Error Handling**: Graceful degradation for missing assets/data

## Code Quality & Maintenance

### Error Handling
- Use try-catch blocks for file operations and external API calls
- Validate inputs and provide meaningful error messages
- Implement fallback behaviors for missing assets
- Log errors with context information

### Security Considerations
- **Save Data**: Validate loaded data to prevent corruption
- **Input Validation**: Sanitize all user inputs
- **Resource Access**: Check file existence before operations
- **Memory Leaks**: Properly unsubscribe from events

### Performance Best Practices
- **Update Methods**: Minimize operations in Update()
- **String Operations**: Use StringBuilder for concatenation
- **Collection Management**: Use appropriate data structures
- **Garbage Collection**: Minimize allocations in hot paths

## Game-Specific Context

### Romance System Implementation
- Build relationship values through dialogue choices
- Gift system with item preferences per character
- Social events tied to calendar/seasonal systems
- Character backstory integration with romance progression

### Combat Balance
- Unit cost vs. effectiveness ratios
- Synergy bonuses for tactical variety  
- Counter-play mechanics between unit types
- Scaling difficulty based on player progression

### World Design Philosophy
- Meaningful exploration rewards
- Balanced risk/reward for encounters
- Multiple paths to objectives
- Replayability through procedural generation

## Common Issues & Solutions

### Unity-Specific Problems
- **Missing References**: Always check for null before use
- **Scene Loading**: Use proper scene management patterns
- **Asset Loading**: Implement proper loading screens for large assets
- **Platform Differences**: Test on target platforms early

### Game System Integration
- **Manager Dependencies**: Initialize in proper order
- **State Transitions**: Validate state changes are legal
- **Data Persistence**: Handle save/load edge cases
- **UI Updates**: Ensure UI reflects current game state

Remember to maintain consistency with existing code patterns and always consider the player experience when implementing new features.