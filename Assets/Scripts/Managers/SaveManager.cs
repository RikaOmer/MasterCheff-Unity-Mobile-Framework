using UnityEngine;
using System;
using System.IO;
using MasterCheff.Core;
using MasterCheff.Data;

namespace MasterCheff.Managers
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] private string _saveFileName = "gamesave.json";
        private string SavePath => Path.Combine(Application.persistentDataPath, _saveFileName);

        public event Action OnSaveCompleted;
        public event Action OnLoadCompleted;
        public GameSaveData CurrentSaveData { get; private set; }

        protected override void OnSingletonAwake() { CurrentSaveData = new GameSaveData(); }

        public void SaveGame()
        {
            try
            {
                CurrentSaveData.lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.WriteAllText(SavePath, JsonUtility.ToJson(CurrentSaveData, true));
                OnSaveCompleted?.Invoke();
            }
            catch (Exception e) { Debug.LogError($"Save failed: {e.Message}"); }
        }

        public bool LoadGame()
        {
            try
            {
                if (!File.Exists(SavePath)) { CurrentSaveData = new GameSaveData(); return false; }
                CurrentSaveData = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(SavePath));
                OnLoadCompleted?.Invoke();
                return true;
            }
            catch { CurrentSaveData = new GameSaveData(); return false; }
        }

        public void DeleteSave() { if (File.Exists(SavePath)) File.Delete(SavePath); CurrentSaveData = new GameSaveData(); }
        public bool HasSaveData() => File.Exists(SavePath);
    }
}
