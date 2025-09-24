using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureBranch.Hero;

namespace AdventureBranch.Combat
{
    /// <summary>
    /// Manages auto-battler combat system
    /// Handles turn order, AI decisions, and battle flow
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        [Header("Combat Settings")]
        public float turnDelay = 1.5f;
        public int maxFormationSize = 6;
        
        [Header("Battle State")]
        public CombatState currentState = CombatState.Setup;
        public List<CombatUnit> playerUnits = new List<CombatUnit>();
        public List<CombatUnit> enemyUnits = new List<CombatUnit>();
        
        private Queue<CombatUnit> turnQueue = new Queue<CombatUnit>();
        private CombatUnit currentUnit;
        
        // Events
        public System.Action<CombatResult> OnCombatEnd;
        public System.Action<CombatUnit> OnUnitTurn;
        public System.Action<CombatUnit, CombatUnit, int> OnDamageDealt;
        
        public static CombatManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void StartCombat(List<Hero.Hero> playerHeroes, List<Hero.Hero> enemies)
        {
            SetupCombat(playerHeroes, enemies);
            currentState = CombatState.InProgress;
            StartCoroutine(CombatLoop());
        }
        
        private void SetupCombat(List<Hero.Hero> playerHeroes, List<Hero.Hero> enemies)
        {
            // Clear previous battle data
            playerUnits.Clear();
            enemyUnits.Clear();
            
            // Create combat units from heroes
            foreach (var hero in playerHeroes)
            {
                CombatUnit unit = new CombatUnit(hero, true);
                playerUnits.Add(unit);
            }
            
            foreach (var enemy in enemies)
            {
                CombatUnit unit = new CombatUnit(enemy, false);
                enemyUnits.Add(unit);
            }
            
            // Calculate turn order based on speed
            CalculateTurnOrder();
        }
        
        private void CalculateTurnOrder()
        {
            List<CombatUnit> allUnits = new List<CombatUnit>();
            allUnits.AddRange(playerUnits);
            allUnits.AddRange(enemyUnits);
            
            // Sort by speed (descending)
            allUnits.Sort((a, b) => b.hero.currentStats.speed.CompareTo(a.hero.currentStats.speed));
            
            turnQueue.Clear();
            foreach (var unit in allUnits)
            {
                if (unit.isAlive)
                    turnQueue.Enqueue(unit);
            }
        }
        
        private IEnumerator CombatLoop()
        {
            while (currentState == CombatState.InProgress)
            {
                if (turnQueue.Count == 0)
                {
                    CalculateTurnOrder(); // Recalculate for next round
                }
                
                if (CheckBattleEnd())
                {
                    yield break;
                }
                
                currentUnit = turnQueue.Dequeue();
                
                if (currentUnit.isAlive)
                {
                    OnUnitTurn?.Invoke(currentUnit);
                    yield return StartCoroutine(ProcessUnitTurn(currentUnit));
                }
                
                yield return new WaitForSeconds(turnDelay);
            }
        }
        
        private IEnumerator ProcessUnitTurn(CombatUnit unit)
        {
            // Auto-battler AI makes decisions
            CombatAction action = DecideAction(unit);
            yield return StartCoroutine(ExecuteAction(unit, action));
        }
        
        private CombatAction DecideAction(CombatUnit unit)
        {
            // Simple AI logic for auto-battler
            List<CombatUnit> targets = unit.isPlayerUnit ? enemyUnits : playerUnits;
            List<CombatUnit> allies = unit.isPlayerUnit ? playerUnits : enemyUnits;
            
            // Remove dead units from targeting
            targets.RemoveAll(t => !t.isAlive);
            allies.RemoveAll(a => !a.isAlive);
            
            if (targets.Count == 0) return null;
            
            // Check if unit should heal
            if (ShouldHeal(unit, allies))
            {
                CombatUnit healTarget = GetWeakestAlly(allies);
                return new CombatAction(ActionType.Heal, healTarget);
            }
            
            // Default to attacking weakest enemy
            CombatUnit attackTarget = GetWeakestEnemy(targets);
            return new CombatAction(ActionType.Attack, attackTarget);
        }
        
        private bool ShouldHeal(CombatUnit unit, List<CombatUnit> allies)
        {
            // Healers should heal when ally health is low
            if (unit.hero.heroClass != HeroClass.Healer) return false;
            
            foreach (var ally in allies)
            {
                if (ally.currentHealth < ally.maxHealth * 0.3f)
                    return true;
            }
            return false;
        }
        
        private CombatUnit GetWeakestAlly(List<CombatUnit> allies)
        {
            CombatUnit weakest = allies[0];
            foreach (var ally in allies)
            {
                if (ally.currentHealth < weakest.currentHealth)
                    weakest = ally;
            }
            return weakest;
        }
        
        private CombatUnit GetWeakestEnemy(List<CombatUnit> enemies)
        {
            CombatUnit weakest = enemies[0];
            foreach (var enemy in enemies)
            {
                if (enemy.currentHealth < weakest.currentHealth)
                    weakest = enemy;
            }
            return weakest;
        }
        
        private IEnumerator ExecuteAction(CombatUnit actor, CombatAction action)
        {
            if (action == null) yield break;
            
            switch (action.type)
            {
                case ActionType.Attack:
                    yield return StartCoroutine(ExecuteAttack(actor, action.target));
                    break;
                case ActionType.Heal:
                    yield return StartCoroutine(ExecuteHeal(actor, action.target));
                    break;
            }
        }
        
        private IEnumerator ExecuteAttack(CombatUnit attacker, CombatUnit target)
        {
            int damage = CalculateDamage(attacker, target);
            target.TakeDamage(damage);
            
            OnDamageDealt?.Invoke(attacker, target, damage);
            
            Debug.Log($"{attacker.hero.heroName} attacks {target.hero.heroName} for {damage} damage!");
            
            if (!target.isAlive)
            {
                Debug.Log($"{target.hero.heroName} has been defeated!");
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator ExecuteHeal(CombatUnit healer, CombatUnit target)
        {
            int healAmount = healer.hero.currentStats.magic * 2;
            target.Heal(healAmount);
            
            Debug.Log($"{healer.hero.heroName} heals {target.hero.heroName} for {healAmount} health!");
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private int CalculateDamage(CombatUnit attacker, CombatUnit target)
        {
            int baseDamage = attacker.hero.currentStats.attack;
            int defense = target.hero.currentStats.defense;
            int damage = Mathf.Max(1, baseDamage - defense / 2);
            
            // Add some randomness
            damage = Random.Range((int)(damage * 0.8f), (int)(damage * 1.2f));
            
            return damage;
        }
        
        private bool CheckBattleEnd()
        {
            bool playersAlive = playerUnits.Exists(u => u.isAlive);
            bool enemiesAlive = enemyUnits.Exists(u => u.isAlive);
            
            if (!playersAlive)
            {
                EndCombat(CombatResult.Defeat);
                return true;
            }
            
            if (!enemiesAlive)
            {
                EndCombat(CombatResult.Victory);
                return true;
            }
            
            return false;
        }
        
        private void EndCombat(CombatResult result)
        {
            currentState = CombatState.Ended;
            OnCombatEnd?.Invoke(result);
            
            if (result == CombatResult.Victory)
            {
                // Award experience to surviving players
                int expGained = 100; // Base experience
                foreach (var unit in playerUnits)
                {
                    if (unit.isAlive)
                    {
                        unit.hero.GainExperience(expGained);
                    }
                }
            }
        }
    }
    
    [System.Serializable]
    public class CombatUnit
    {
        public Hero.Hero hero;
        public bool isPlayerUnit;
        public int currentHealth;
        public int maxHealth;
        public bool isAlive => currentHealth > 0;
        
        public CombatUnit(Hero.Hero hero, bool isPlayerUnit)
        {
            this.hero = hero;
            this.isPlayerUnit = isPlayerUnit;
            this.maxHealth = hero.currentStats.health;
            this.currentHealth = maxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
        }
        
        public void Heal(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(maxHealth, currentHealth);
        }
    }
    
    public class CombatAction
    {
        public ActionType type;
        public CombatUnit target;
        
        public CombatAction(ActionType type, CombatUnit target)
        {
            this.type = type;
            this.target = target;
        }
    }
    
    public enum CombatState
    {
        Setup,
        InProgress,
        Ended
    }
    
    public enum CombatResult
    {
        Victory,
        Defeat,
        Escape
    }
    
    public enum ActionType
    {
        Attack,
        Heal,
        Buff,
        Debuff,
        Special
    }
}