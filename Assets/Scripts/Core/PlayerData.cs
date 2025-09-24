using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureBranch.Hero;

namespace AdventureBranch.Core
{
    /// <summary>
    /// Manages player data including heroes, resources, and progress
    /// Persistent data that survives scene changes
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        [Header("Player Info")]
        public string playerName = "Player";
        public int playerLevel = 1;
        public float totalPlayTime = 0f;
        
        [Header("Heroes")]
        public List<Hero.Hero> ownedHeroes = new List<Hero.Hero>();
        public List<Hero.Hero> activeParty = new List<Hero.Hero>();
        public int maxPartySize = 4;
        
        [Header("Resources")]
        public int gold = 100;
        public Dictionary<string, int> resources = new Dictionary<string, int>();
        
        [Header("Progress")]
        public Vector2Int currentWorldPosition = Vector2Int.zero;
        public List<string> completedQuests = new List<string>();
        public List<string> unlockedAchievements = new List<string>();
        
        public static PlayerData Instance { get; private set; }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePlayerData();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Update()
        {
            // Track play time
            totalPlayTime += Time.deltaTime;
        }
        
        private void InitializePlayerData()
        {
            // Initialize starting resources
            resources["Gold"] = gold;
            resources["Iron"] = 0;
            resources["Wood"] = 0;
            resources["Stone"] = 0;
            resources["Herbs"] = 0;
            resources["Gems"] = 0;
            
            // Create starting heroes
            CreateStartingHeroes();
        }
        
        private void CreateStartingHeroes()
        {
            // Create a basic starting party
            Hero.Hero warrior = new Hero.Hero("Gareth", HeroClass.Warrior);
            Hero.Hero mage = new Hero.Hero("Lyra", HeroClass.Mage);
            Hero.Hero archer = new Hero.Hero("Robin", HeroClass.Archer);
            
            ownedHeroes.Add(warrior);
            ownedHeroes.Add(mage);
            ownedHeroes.Add(archer);
            
            // Add to active party
            activeParty.Add(warrior);
            activeParty.Add(mage);
            activeParty.Add(archer);
        }
        
        public void AddHero(Hero.Hero hero)
        {
            if (!ownedHeroes.Contains(hero))
            {
                ownedHeroes.Add(hero);
                Debug.Log($"Added new hero: {hero.heroName}");
            }
        }
        
        public void RemoveHero(Hero.Hero hero)
        {
            ownedHeroes.Remove(hero);
            activeParty.Remove(hero);
        }
        
        public bool AddToParty(Hero.Hero hero)
        {
            if (activeParty.Count >= maxPartySize)
            {
                Debug.LogWarning("Party is full!");
                return false;
            }
            
            if (!ownedHeroes.Contains(hero))
            {
                Debug.LogWarning("Hero not owned by player!");
                return false;
            }
            
            if (activeParty.Contains(hero))
            {
                Debug.LogWarning("Hero already in party!");
                return false;
            }
            
            activeParty.Add(hero);
            return true;
        }
        
        public void RemoveFromParty(Hero.Hero hero)
        {
            activeParty.Remove(hero);
        }
        
        public void AddResource(string resourceType, int amount)
        {
            if (resources.ContainsKey(resourceType))
            {
                resources[resourceType] += amount;
            }
            else
            {
                resources[resourceType] = amount;
            }
            
            Debug.Log($"Added {amount} {resourceType}. Total: {resources[resourceType]}");
        }
        
        public bool SpendResource(string resourceType, int amount)
        {
            if (!resources.ContainsKey(resourceType) || resources[resourceType] < amount)
            {
                Debug.LogWarning($"Not enough {resourceType}! Have: {resources.GetValueOrDefault(resourceType, 0)}, Need: {amount}");
                return false;
            }
            
            resources[resourceType] -= amount;
            Debug.Log($"Spent {amount} {resourceType}. Remaining: {resources[resourceType]}");
            return true;
        }
        
        public int GetResource(string resourceType)
        {
            return resources.GetValueOrDefault(resourceType, 0);
        }
        
        public void CompleteQuest(string questId)
        {
            if (!completedQuests.Contains(questId))
            {
                completedQuests.Add(questId);
                Debug.Log($"Quest completed: {questId}");
            }
        }
        
        public bool IsQuestCompleted(string questId)
        {
            return completedQuests.Contains(questId);
        }
        
        public void UnlockAchievement(string achievementId)
        {
            if (!unlockedAchievements.Contains(achievementId))
            {
                unlockedAchievements.Add(achievementId);
                Debug.Log($"Achievement unlocked: {achievementId}");
            }
        }
        
        public bool IsAchievementUnlocked(string achievementId)
        {
            return unlockedAchievements.Contains(achievementId);
        }
        
        public string GetFormattedPlayTime()
        {
            int hours = Mathf.FloorToInt(totalPlayTime / 3600f);
            int minutes = Mathf.FloorToInt((totalPlayTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(totalPlayTime % 60f);
            
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }
}