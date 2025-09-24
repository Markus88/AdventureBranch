using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureBranch.Hero
{
    /// <summary>
    /// Core Hero class representing individual characters in the game
    /// Handles stats, abilities, equipment, and relationships
    /// </summary>
    [System.Serializable]
    public class Hero
    {
        [Header("Basic Info")]
        public string heroName;
        public HeroClass heroClass;
        public int level = 1;
        public int experience = 0;
        
        [Header("Combat Stats")]
        public HeroStats baseStats;
        public HeroStats currentStats;
        
        [Header("Equipment")]
        public List<Equipment> equippedItems = new List<Equipment>();
        
        [Header("Abilities")]
        public List<Ability> abilities = new List<Ability>();
        
        [Header("Romance & Relationships")]
        public int relationshipLevel = 0;
        public List<string> unlockedDialogue = new List<string>();
        
        public Hero(string name, HeroClass heroClass)
        {
            this.heroName = name;
            this.heroClass = heroClass;
            InitializeStats();
        }
        
        private void InitializeStats()
        {
            // Initialize base stats based on hero class
            baseStats = GetClassBaseStats(heroClass);
            currentStats = new HeroStats(baseStats);
        }
        
        private HeroStats GetClassBaseStats(HeroClass heroClass)
        {
            switch (heroClass)
            {
                case HeroClass.Warrior:
                    return new HeroStats(100, 20, 15, 10, 8, 12);
                case HeroClass.Mage:
                    return new HeroStats(70, 8, 10, 25, 20, 15);
                case HeroClass.Archer:
                    return new HeroStats(85, 15, 12, 12, 18, 20);
                case HeroClass.Healer:
                    return new HeroStats(80, 10, 12, 22, 15, 18);
                default:
                    return new HeroStats(80, 12, 12, 12, 12, 12);
            }
        }
        
        public void LevelUp()
        {
            level++;
            // Increase stats based on class growth rates
            IncreaseStatsOnLevelUp();
        }
        
        private void IncreaseStatsOnLevelUp()
        {
            HeroStats growth = GetClassGrowthRates(heroClass);
            baseStats.health += growth.health;
            baseStats.attack += growth.attack;
            baseStats.defense += growth.defense;
            baseStats.magic += growth.magic;
            baseStats.magicDefense += growth.magicDefense;
            baseStats.speed += growth.speed;
            
            // Update current stats
            currentStats = new HeroStats(baseStats);
        }
        
        private HeroStats GetClassGrowthRates(HeroClass heroClass)
        {
            switch (heroClass)
            {
                case HeroClass.Warrior:
                    return new HeroStats(8, 3, 2, 1, 1, 2);
                case HeroClass.Mage:
                    return new HeroStats(5, 1, 1, 4, 3, 2);
                case HeroClass.Archer:
                    return new HeroStats(6, 2, 2, 1, 2, 3);
                case HeroClass.Healer:
                    return new HeroStats(6, 1, 2, 3, 2, 3);
                default:
                    return new HeroStats(6, 2, 2, 2, 2, 2);
            }
        }
        
        public void GainExperience(int exp)
        {
            experience += exp;
            CheckForLevelUp();
        }
        
        private void CheckForLevelUp()
        {
            int expNeeded = GetExperienceNeededForLevel(level + 1);
            if (experience >= expNeeded)
            {
                LevelUp();
                CheckForLevelUp(); // Check for multiple level ups
            }
        }
        
        private int GetExperienceNeededForLevel(int targetLevel)
        {
            return targetLevel * targetLevel * 100; // Simple exponential growth
        }
        
        public void EquipItem(Equipment item)
        {
            // Remove existing item of same type
            equippedItems.RemoveAll(e => e.type == item.type);
            equippedItems.Add(item);
            RecalculateStats();
        }
        
        public void UnequipItem(Equipment item)
        {
            equippedItems.Remove(item);
            RecalculateStats();
        }
        
        private void RecalculateStats()
        {
            currentStats = new HeroStats(baseStats);
            
            // Apply equipment bonuses
            foreach (Equipment item in equippedItems)
            {
                currentStats.health += item.healthBonus;
                currentStats.attack += item.attackBonus;
                currentStats.defense += item.defenseBonus;
                currentStats.magic += item.magicBonus;
                currentStats.magicDefense += item.magicDefenseBonus;
                currentStats.speed += item.speedBonus;
            }
        }
        
        public void IncreaseRelationship(int amount = 1)
        {
            relationshipLevel += amount;
            relationshipLevel = Mathf.Clamp(relationshipLevel, 0, 100);
        }
    }
    
    [System.Serializable]
    public class HeroStats
    {
        public int health;
        public int attack;
        public int defense;
        public int magic;
        public int magicDefense;
        public int speed;
        
        public HeroStats() { }
        
        public HeroStats(int hp, int att, int def, int mag, int mdef, int spd)
        {
            health = hp;
            attack = att;
            defense = def;
            magic = mag;
            magicDefense = mdef;
            speed = spd;
        }
        
        public HeroStats(HeroStats other)
        {
            health = other.health;
            attack = other.attack;
            defense = other.defense;
            magic = other.magic;
            magicDefense = other.magicDefense;
            speed = other.speed;
        }
    }
    
    public enum HeroClass
    {
        Warrior,
        Mage,
        Archer,
        Healer,
        Rogue,
        Paladin
    }
    
    [System.Serializable]
    public class Equipment
    {
        public string name;
        public EquipmentType type;
        public int healthBonus;
        public int attackBonus;
        public int defenseBonus;
        public int magicBonus;
        public int magicDefenseBonus;
        public int speedBonus;
    }
    
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Helmet,
        Boots,
        Accessory
    }
    
    [System.Serializable]
    public class Ability
    {
        public string name;
        public string description;
        public int manaCost;
        public int damage;
        public AbilityType type;
        public int cooldown;
    }
    
    public enum AbilityType
    {
        Attack,
        Heal,
        Buff,
        Debuff,
        Special
    }
}