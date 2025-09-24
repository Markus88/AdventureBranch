using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureBranch.Audio
{
    /// <summary>
    /// Manages all audio in the game including music and sound effects
    /// Provides easy interface for playing audio across different game states
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        public AudioSource musicSource;
        public AudioSource sfxSource;
        
        [Header("Music Tracks")]
        public AudioClip mainMenuMusic;
        public AudioClip worldMapMusic;
        public AudioClip combatMusic;
        public AudioClip romanceMusic;
        
        [Header("Sound Effects")]
        public AudioClip buttonClickSFX;
        public AudioClip attackSFX;
        public AudioClip healSFX;
        public AudioClip levelUpSFX;
        public AudioClip treasureSFX;
        
        [Header("Settings")]
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        
        public static AudioManager Instance { get; private set; }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudio();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializeAudio()
        {
            // Create audio sources if they don't exist
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("Music Source");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }
            
            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFX Source");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
            }
            
            // Apply initial settings
            UpdateAudioSettings();
        }
        
        public void PlayMusic(AudioClip music)
        {
            if (!musicEnabled || music == null) return;
            
            if (musicSource.clip == music && musicSource.isPlaying) return;
            
            musicSource.clip = music;
            musicSource.Play();
        }
        
        public void PlaySFX(AudioClip sfx)
        {
            if (!sfxEnabled || sfx == null) return;
            
            sfxSource.PlayOneShot(sfx);
        }
        
        public void StopMusic()
        {
            musicSource.Stop();
        }
        
        public void PauseMusic()
        {
            musicSource.Pause();
        }
        
        public void ResumeMusic()
        {
            musicSource.UnPause();
        }
        
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume;
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            sfxSource.volume = sfxVolume;
        }
        
        public void SetMusicEnabled(bool enabled)
        {
            musicEnabled = enabled;
            if (!enabled)
            {
                StopMusic();
            }
        }
        
        public void SetSFXEnabled(bool enabled)
        {
            sfxEnabled = enabled;
        }
        
        private void UpdateAudioSettings()
        {
            musicSource.volume = musicVolume;
            sfxSource.volume = sfxVolume;
        }
        
        // Convenience methods for specific game states
        public void PlayMainMenuMusic()
        {
            PlayMusic(mainMenuMusic);
        }
        
        public void PlayWorldMapMusic()
        {
            PlayMusic(worldMapMusic);
        }
        
        public void PlayCombatMusic()
        {
            PlayMusic(combatMusic);
        }
        
        public void PlayRomanceMusic()
        {
            PlayMusic(romanceMusic);
        }
        
        // Convenience methods for common SFX
        public void PlayButtonClick()
        {
            PlaySFX(buttonClickSFX);
        }
        
        public void PlayAttackSound()
        {
            PlaySFX(attackSFX);
        }
        
        public void PlayHealSound()
        {
            PlaySFX(healSFX);
        }
        
        public void PlayLevelUpSound()
        {
            PlaySFX(levelUpSFX);
        }
        
        public void PlayTreasureSound()
        {
            PlaySFX(treasureSFX);
        }
        
        // Save/Load audio settings
        public void SaveAudioSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        public void LoadAudioSettings()
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
            musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
            
            UpdateAudioSettings();
        }
    }
}