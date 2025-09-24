using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using AdventureBranch.Hero;
using AdventureBranch.World;

namespace AdventureBranch.Utils
{
    /// <summary>
    /// Handles saving and loading game data
    /// Manages player progress, hero data, and world state
    /// </summary>
    public static class SaveSystem
    {
        private static readonly string SaveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        private static readonly string SaveFileName = "gamesave.json";
        private static readonly string SaveFilePath = Path.Combine(SaveDirectory, SaveFileName);
        
        public static void SaveGame(GameSaveData saveData)
        {
            try
            {
                // Ensure save directory exists
                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }
                
                // Serialize save data to JSON
                string jsonData = JsonUtility.ToJson(saveData, true);
                
                // Write to file
                File.WriteAllText(SaveFilePath, jsonData);
                
                Debug.Log($"Game saved successfully to: {SaveFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }
        
        public static GameSaveData LoadGame()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    Debug.LogWarning("No save file found.");
                    return null;
                }
                
                // Read from file
                string jsonData = File.ReadAllText(SaveFilePath);
                
                // Deserialize from JSON
                GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonData);
                
                Debug.Log("Game loaded successfully.");
                return saveData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                return null;
            }
        }
        
        public static bool SaveExists()
        {
            return File.Exists(SaveFilePath);
        }
        
        public static void DeleteSave()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    File.Delete(SaveFilePath);
                    Debug.Log("Save file deleted.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete save file: {e.Message}");
            }
        }
        
        public static GameSaveData CreateNewSaveData()
        {
            GameSaveData newSave = new GameSaveData();
            newSave.playerName = "Player";
            newSave.playTime = 0f;
            newSave.currentLevel = 1;
            newSave.playerPosition = Vector2Int.zero;
            newSave.heroes = new List<Hero.Hero>();
            newSave.worldSeed = Random.Range(int.MinValue, int.MaxValue);
            
            return newSave;
        }
    }
    
    [System.Serializable]
    public class GameSaveData
    {
        [Header("Player Data")]
        public string playerName;
        public float playTime;
        public int currentLevel;
        public Vector2Int playerPosition;
        
        [Header("Hero Data")]
        public List<Hero.Hero> heroes = new List<Hero.Hero>();
        
        [Header("World Data")]
        public int worldSeed;
        public List<Vector2Int> exploredTiles = new List<Vector2Int>();
        
        [Header("Progress Data")]
        public List<string> completedQuests = new List<string>();
        public List<string> unlockedAchievements = new List<string>();
        
        [Header("Resources")]
        public int gold = 100;
        public Dictionary<string, int> resources = new Dictionary<string, int>();
        
        [Header("Settings")]
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
    }
}