using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdventureBranch.Core;

namespace AdventureBranch.UI
{
    /// <summary>
    /// Main Menu UI controller
    /// Handles menu interactions and game start options
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public Button newGameButton;
        public Button loadGameButton;
        public Button settingsButton;
        public Button quitButton;
        
        [Header("Panels")]
        public GameObject mainMenuPanel;
        public GameObject settingsPanel;
        
        private void Start()
        {
            InitializeUI();
        }
        
        private void InitializeUI()
        {
            // Setup button events
            if (newGameButton != null)
                newGameButton.onClick.AddListener(OnNewGameClicked);
                
            if (loadGameButton != null)
                loadGameButton.onClick.AddListener(OnLoadGameClicked);
                
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
                
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);
            
            // Show main menu panel
            ShowMainMenu();
        }
        
        public void OnNewGameClicked()
        {
            Debug.Log("Starting new game...");
            GameManager.Instance.StartNewGame();
        }
        
        public void OnLoadGameClicked()
        {
            Debug.Log("Loading game...");
            GameManager.Instance.LoadGame();
        }
        
        public void OnSettingsClicked()
        {
            Debug.Log("Opening settings...");
            ShowSettings();
        }
        
        public void OnQuitClicked()
        {
            Debug.Log("Quitting game...");
            GameManager.Instance.QuitGame();
        }
        
        public void ShowMainMenu()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
                
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
        }
        
        public void ShowSettings()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
                
            if (settingsPanel != null)
                settingsPanel.SetActive(true);
        }
        
        public void OnBackFromSettings()
        {
            ShowMainMenu();
        }
    }
}