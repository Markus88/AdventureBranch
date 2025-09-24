using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureBranch.Core
{
    /// <summary>
    /// Core GameManager that handles overall game state and flow
    /// Manages transitions between different game modes: Menu, World Exploration, Combat, and Romance
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        public GameState currentState = GameState.MainMenu;
        
        [Header("Scene References")]
        public string mainMenuScene = "MainMenu";
        public string worldMapScene = "WorldMap";
        public string combatScene = "Combat";
        public string heroManagementScene = "HeroManagement";
        
        public static GameManager Instance { get; private set; }
        
        // Events
        public System.Action<GameState> OnGameStateChanged;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        private void Start()
        {
            // Initialize game systems
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            // Set initial game state
            ChangeGameState(GameState.MainMenu);
        }
        
        public void ChangeGameState(GameState newState)
        {
            if (currentState == newState) return;
            
            GameState previousState = currentState;
            currentState = newState;
            
            Debug.Log($"Game state changed from {previousState} to {newState}");
            
            // Handle state transitions
            HandleStateTransition(previousState, newState);
            
            // Notify listeners
            OnGameStateChanged?.Invoke(newState);
        }
        
        private void HandleStateTransition(GameState from, GameState to)
        {
            switch (to)
            {
                case GameState.MainMenu:
                    LoadScene(mainMenuScene);
                    break;
                case GameState.WorldExploration:
                    LoadScene(worldMapScene);
                    break;
                case GameState.Combat:
                    LoadScene(combatScene);
                    break;
                case GameState.HeroManagement:
                    LoadScene(heroManagementScene);
                    break;
                case GameState.Romance:
                    // Romance interactions can happen in multiple scenes
                    break;
            }
        }
        
        private void LoadScene(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        
        public void StartNewGame()
        {
            ChangeGameState(GameState.WorldExploration);
        }
        
        public void LoadGame()
        {
            // TODO: Implement save/load system
            Debug.Log("Load game functionality not implemented yet");
        }
        
        public void SaveGame()
        {
            // TODO: Implement save/load system
            Debug.Log("Save game functionality not implemented yet");
        }
        
        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    public enum GameState
    {
        MainMenu,
        WorldExploration,
        Combat,
        HeroManagement,
        Romance,
        Settings,
        Paused
    }
}