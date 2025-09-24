using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureBranch.Hero
{
    /// <summary>
    /// Manages romance interactions and relationship mechanics
    /// Handles dialogue, relationship progression, and romance events
    /// </summary>
    public class RomanceManager : MonoBehaviour
    {
        [Header("Romance Settings")]
        public List<RomanceOption> availableRomances = new List<RomanceOption>();
        public DialogueDatabase dialogueDatabase;
        
        [Header("Current Romance State")]
        public RomanceOption currentRomance;
        public bool inRomanceScene = false;
        
        // Events
        public System.Action<RomanceOption> OnRomanceStarted;
        public System.Action<RomanceOption, int> OnRelationshipChanged;
        public System.Action<DialogueEntry> OnDialogueStarted;
        
        public static RomanceManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void StartRomanceInteraction(RomanceOption romance)
        {
            currentRomance = romance;
            inRomanceScene = true;
            
            OnRomanceStarted?.Invoke(romance);
            
            // Start dialogue based on relationship level
            DialogueEntry dialogue = GetAppropriateDialogue(romance);
            if (dialogue != null)
            {
                StartDialogue(dialogue);
            }
        }
        
        public void StartDialogue(DialogueEntry dialogue)
        {
            OnDialogueStarted?.Invoke(dialogue);
            Debug.Log($"Starting dialogue: {dialogue.characterName} - {dialogue.text}");
        }
        
        private DialogueEntry GetAppropriateDialogue(RomanceOption romance)
        {
            if (dialogueDatabase == null || dialogueDatabase.dialogues == null)
                return null;
            
            // Find dialogue appropriate for current relationship level
            foreach (var dialogue in dialogueDatabase.dialogues)
            {
                if (dialogue.characterName == romance.characterName &&
                    dialogue.requiredRelationshipLevel <= romance.relationshipLevel)
                {
                    return dialogue;
                }
            }
            
            return null;
        }
        
        public void MakeDialogueChoice(DialogueChoice choice)
        {
            if (currentRomance == null) return;
            
            // Apply choice effects
            IncreaseRelationship(currentRomance, choice.relationshipChange);
            
            Debug.Log($"Player chose: {choice.text} (Relationship change: {choice.relationshipChange})");
            
            // Check for relationship milestones
            CheckRelationshipMilestones(currentRomance);
        }
        
        public void IncreaseRelationship(RomanceOption romance, int amount)
        {
            int oldLevel = romance.relationshipLevel;
            romance.relationshipLevel += amount;
            romance.relationshipLevel = Mathf.Clamp(romance.relationshipLevel, 0, 100);
            
            OnRelationshipChanged?.Invoke(romance, romance.relationshipLevel - oldLevel);
            
            // Update hero relationship if applicable
            if (romance.associatedHero != null)
            {
                romance.associatedHero.IncreaseRelationship(amount);
            }
        }
        
        private void CheckRelationshipMilestones(RomanceOption romance)
        {
            // Unlock new dialogue options at certain relationship levels
            if (romance.relationshipLevel >= 25 && !romance.milestoneReached[0])
            {
                romance.milestoneReached[0] = true;
                UnlockDialogue(romance, "friendship_milestone");
                Debug.Log($"Reached friendship milestone with {romance.characterName}!");
            }
            
            if (romance.relationshipLevel >= 50 && !romance.milestoneReached[1])
            {
                romance.milestoneReached[1] = true;
                UnlockDialogue(romance, "romance_milestone");
                Debug.Log($"Reached romance milestone with {romance.characterName}!");
            }
            
            if (romance.relationshipLevel >= 75 && !romance.milestoneReached[2])
            {
                romance.milestoneReached[2] = true;
                UnlockDialogue(romance, "deep_romance_milestone");
                Debug.Log($"Reached deep romance milestone with {romance.characterName}!");
            }
            
            if (romance.relationshipLevel >= 100 && !romance.milestoneReached[3])
            {
                romance.milestoneReached[3] = true;
                UnlockDialogue(romance, "true_love_milestone");
                Debug.Log($"Reached true love with {romance.characterName}!");
            }
        }
        
        private void UnlockDialogue(RomanceOption romance, string dialogueKey)
        {
            if (!romance.unlockedDialogues.Contains(dialogueKey))
            {
                romance.unlockedDialogues.Add(dialogueKey);
            }
        }
        
        public void EndRomanceInteraction()
        {
            inRomanceScene = false;
            currentRomance = null;
        }
        
        public RomanceOption GetRomanceByName(string characterName)
        {
            return availableRomances.Find(r => r.characterName == characterName);
        }
        
        public List<RomanceOption> GetAvailableRomances()
        {
            return availableRomances.FindAll(r => r.isAvailable);
        }
    }
    
    [System.Serializable]
    public class RomanceOption
    {
        public string characterName;
        public string description;
        public bool isAvailable = true;
        public int relationshipLevel = 0;
        public Hero associatedHero;
        
        [Header("Romance Progression")]
        public bool[] milestoneReached = new bool[4]; // Friendship, Romance, Deep Romance, True Love
        public List<string> unlockedDialogues = new List<string>();
        
        [Header("Visual")]
        public Sprite characterPortrait;
    }
    
    [System.Serializable]
    public class DialogueDatabase
    {
        public List<DialogueEntry> dialogues = new List<DialogueEntry>();
    }
    
    [System.Serializable]
    public class DialogueEntry
    {
        public string characterName;
        public string text;
        public int requiredRelationshipLevel = 0;
        public List<DialogueChoice> choices = new List<DialogueChoice>();
        public string dialogueKey;
    }
    
    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public int relationshipChange = 0;
        public string nextDialogueKey;
    }
}