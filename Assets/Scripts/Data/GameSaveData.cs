using System;
using System.Collections.Generic;

namespace MasterCheff.Data
{
    [Serializable]
    public class GameSaveData
    {
        public int saveVersion = 1;
        public string lastSaveTime;
        public int currentLevel = 1;
        public int highScore = 0;
        public int coins = 0;
        public float musicVolume = 0.7f;
        public float sfxVolume = 1.0f;
        public bool isMusicMuted = false;
        public bool isSfxMuted = false;
        public List<string> unlockedAchievements = new List<string>();
        public List<LevelData> levelProgress = new List<LevelData>();
    }

    [Serializable]
    public class LevelData
    {
        public int levelId;
        public bool isUnlocked;
        public bool isCompleted;
        public int stars;
        public int bestScore;
    }

    [Serializable]
    public class InventoryItem
    {
        public string itemId;
        public int quantity;
        public bool isEquipped;
    }
}
