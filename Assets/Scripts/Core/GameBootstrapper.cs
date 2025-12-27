using UnityEngine;
using MasterCheff.Managers;

namespace MasterCheff.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _initializeOnAwake = true;
        [SerializeField] private bool _loadSaveOnStart = true;

        private static bool _isInitialized = false;

        private void Awake()
        {
            if (_isInitialized) { Destroy(gameObject); return; }
            if (_initializeOnAwake) Initialize();
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            if (_loadSaveOnStart) LoadSavedData();
            _isInitialized = true;
            Debug.Log("[GameBootstrapper] Initialized");
        }

        private void InitializeManagers()
        {
            if (!GameManager.HasInstance) { var _ = GameManager.Instance; }
            if (!AudioManager.HasInstance) { var _ = AudioManager.Instance; }
            if (!SaveManager.HasInstance) { var _ = SaveManager.Instance; }
            if (!EventManager.HasInstance) { var _ = EventManager.Instance; }
            if (!SceneLoader.HasInstance) { var _ = SceneLoader.Instance; }
        }

        private void LoadSavedData()
        {
            if (SaveManager.HasInstance && SaveManager.Instance.LoadGame())
            {
                var data = SaveManager.Instance.CurrentSaveData;
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.SetMusicVolume(data.musicVolume);
                    AudioManager.Instance.SetSFXVolume(data.sfxVolume);
                    AudioManager.Instance.SetMusicMuted(data.isMusicMuted);
                    AudioManager.Instance.SetSFXMuted(data.isSfxMuted);
                }
            }
        }
    }
}
