namespace MasterCheff.Core
{
    public static class Constants
    {
        public static class Scenes
        {
            public const string LOADING = "Loading";
            public const string MAIN_MENU = "MainMenu";
            public const string GAMEPLAY = "Gameplay";
        }
        public static class PlayerPrefsKeys
        {
            public const string HIGH_SCORE = "HighScore";
            public const string MUSIC_VOLUME = "MusicVolume";
            public const string SFX_VOLUME = "SFXVolume";
        }
        public static class Tags
        {
            public const string PLAYER = "Player";
            public const string ENEMY = "Enemy";
        }
        public static class Balance
        {
            public const int MAX_LIVES = 3;
            public const float INVINCIBILITY_TIME = 2f;
        }
        public static class Mobile
        {
            public const int TARGET_FRAME_RATE = 60;
            public const float SWIPE_THRESHOLD = 50f;
        }
    }
}
