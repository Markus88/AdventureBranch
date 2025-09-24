using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureBranch.Core;

namespace AdventureBranch.World
{
    /// <summary>
    /// Manages world exploration, procedural generation, and rogue-like elements
    /// Handles map generation, resource nodes, and random encounters
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        [Header("World Generation")]
        public int mapWidth = 20;
        public int mapHeight = 20;
        public float encounterChance = 0.1f;
        public float resourceChance = 0.15f;
        
        [Header("Player Position")]
        public Vector2Int playerPosition = Vector2Int.zero;
        
        [Header("World Data")]
        public WorldTile[,] worldMap;
        public List<WorldEvent> availableEvents = new List<WorldEvent>();
        
        // Events
        public System.Action<Vector2Int> OnPlayerMoved;
        public System.Action<WorldEvent> OnEventTriggered;
        
        public static WorldManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            GenerateWorld();
        }
        
        public void GenerateWorld()
        {
            worldMap = new WorldTile[mapWidth, mapHeight];
            
            // Generate base terrain
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    worldMap[x, y] = new WorldTile();
                    worldMap[x, y].position = new Vector2Int(x, y);
                    worldMap[x, y].terrainType = GetRandomTerrain();
                    worldMap[x, y].isExplored = false;
                    
                    // Add random encounters and resources
                    if (Random.Range(0f, 1f) < encounterChance)
                    {
                        worldMap[x, y].hasEncounter = true;
                        worldMap[x, y].encounterType = GetRandomEncounter();
                    }
                    
                    if (Random.Range(0f, 1f) < resourceChance)
                    {
                        worldMap[x, y].hasResource = true;
                        worldMap[x, y].resourceType = GetRandomResource();
                    }
                }
            }
            
            // Ensure starting position is safe
            worldMap[playerPosition.x, playerPosition.y].hasEncounter = false;
            worldMap[playerPosition.x, playerPosition.y].isExplored = true;
            
            Debug.Log($"Generated world map: {mapWidth}x{mapHeight}");
        }
        
        private TerrainType GetRandomTerrain()
        {
            TerrainType[] terrains = System.Enum.GetValues(typeof(TerrainType)) as TerrainType[];
            return terrains[Random.Range(0, terrains.Length)];
        }
        
        private EncounterType GetRandomEncounter()
        {
            EncounterType[] encounters = System.Enum.GetValues(typeof(EncounterType)) as EncounterType[];
            return encounters[Random.Range(0, encounters.Length)];
        }
        
        private ResourceType GetRandomResource()
        {
            ResourceType[] resources = System.Enum.GetValues(typeof(ResourceType)) as ResourceType[];
            return resources[Random.Range(0, resources.Length)];
        }
        
        public bool MovePlayer(Vector2Int direction)
        {
            Vector2Int newPosition = playerPosition + direction;
            
            // Check bounds
            if (newPosition.x < 0 || newPosition.x >= mapWidth || 
                newPosition.y < 0 || newPosition.y >= mapHeight)
            {
                return false;
            }
            
            // Update player position
            playerPosition = newPosition;
            WorldTile currentTile = worldMap[playerPosition.x, playerPosition.y];
            
            // Mark as explored
            if (!currentTile.isExplored)
            {
                currentTile.isExplored = true;
                Debug.Log($"Discovered new area: {currentTile.terrainType}");
            }
            
            OnPlayerMoved?.Invoke(playerPosition);
            
            // Check for events
            CheckForEvents(currentTile);
            
            return true;
        }
        
        private void CheckForEvents(WorldTile tile)
        {
            if (tile.hasEncounter && !tile.encounterCompleted)
            {
                TriggerEncounter(tile);
            }
            
            if (tile.hasResource && !tile.resourceCollected)
            {
                CollectResource(tile);
            }
        }
        
        private void TriggerEncounter(WorldTile tile)
        {
            WorldEvent encounterEvent = CreateEncounterEvent(tile.encounterType);
            OnEventTriggered?.Invoke(encounterEvent);
            tile.encounterCompleted = true;
        }
        
        private void CollectResource(WorldTile tile)
        {
            // Add resource to player inventory
            Debug.Log($"Collected {tile.resourceType}!");
            // TODO: Implement inventory system
            tile.resourceCollected = true;
        }
        
        private WorldEvent CreateEncounterEvent(EncounterType type)
        {
            WorldEvent worldEvent = new WorldEvent();
            worldEvent.eventType = type;
            
            switch (type)
            {
                case EncounterType.Combat:
                    worldEvent.title = "Wild Monsters";
                    worldEvent.description = "A group of monsters blocks your path!";
                    worldEvent.eventAction = () => {
                        // Trigger combat
                        GameManager.Instance.ChangeGameState(GameState.Combat);
                    };
                    break;
                    
                case EncounterType.Treasure:
                    worldEvent.title = "Treasure Chest";
                    worldEvent.description = "You found a mysterious chest!";
                    worldEvent.eventAction = () => {
                        // Award random loot
                        Debug.Log("You found treasure!");
                    };
                    break;
                    
                case EncounterType.Merchant:
                    worldEvent.title = "Traveling Merchant";
                    worldEvent.description = "A merchant offers to trade with you.";
                    worldEvent.eventAction = () => {
                        // Open shop interface
                        Debug.Log("Shop interface not implemented yet");
                    };
                    break;
                    
                case EncounterType.Romance:
                    worldEvent.title = "Mysterious Stranger";
                    worldEvent.description = "You encounter someone interesting...";
                    worldEvent.eventAction = () => {
                        // Trigger romance event
                        GameManager.Instance.ChangeGameState(GameState.Romance);
                    };
                    break;
            }
            
            return worldEvent;
        }
        
        public WorldTile GetTileAt(Vector2Int position)
        {
            if (position.x >= 0 && position.x < mapWidth && 
                position.y >= 0 && position.y < mapHeight)
            {
                return worldMap[position.x, position.y];
            }
            return null;
        }
        
        public List<Vector2Int> GetExploredTiles()
        {
            List<Vector2Int> explored = new List<Vector2Int>();
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (worldMap[x, y].isExplored)
                    {
                        explored.Add(new Vector2Int(x, y));
                    }
                }
            }
            return explored;
        }
    }
    
    [System.Serializable]
    public class WorldTile
    {
        public Vector2Int position;
        public TerrainType terrainType;
        public bool isExplored = false;
        
        // Encounters
        public bool hasEncounter = false;
        public EncounterType encounterType;
        public bool encounterCompleted = false;
        
        // Resources
        public bool hasResource = false;
        public ResourceType resourceType;
        public bool resourceCollected = false;
    }
    
    [System.Serializable]
    public class WorldEvent
    {
        public string title;
        public string description;
        public EncounterType eventType;
        public System.Action eventAction;
    }
    
    public enum TerrainType
    {
        Plains,
        Forest,
        Mountains,
        Desert,
        Swamp,
        Ruins
    }
    
    public enum EncounterType
    {
        Combat,
        Treasure,
        Merchant,
        Romance,
        Quest
    }
    
    public enum ResourceType
    {
        Gold,
        Iron,
        Wood,
        Stone,
        Herbs,
        Gems
    }
}