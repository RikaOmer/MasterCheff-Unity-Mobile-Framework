using UnityEngine;
using System;
using MasterCheff.Core;

namespace MasterCheff.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool _pauseOnFocusLost = true;
        [SerializeField] private int _targetFrameRate = 60;

        private GameState _currentState = GameState.Loading;
        private GameState _previousState;
        private bool _isPaused = false;

        public event Action<GameState> OnGameStateChanged;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameOver;
        public event Action OnVictory;

        public GameState CurrentState => _currentState;
        public bool IsPaused => _isPaused;
        public bool IsPlaying => _currentState == GameState.Playing && !_isPaused;
        public int CurrentScore { get; private set; }
        public int HighScore { get; private set; }
        public int CurrentLevel { get; private set; } = 1;

        protected override void OnSingletonAwake()
        {
            Application.targetFrameRate = _targetFrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_pauseOnFocusLost && _currentState == GameState.Playing && !hasFocus)
                PauseGame();
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;
            _previousState = _currentState;
            _currentState = newState;
            OnGameStateChanged?.Invoke(_currentState);
            HandleStateChange();
        }

        private void HandleStateChange()
        {
            switch (_currentState)
            {
                case GameState.Playing: Time.timeScale = 1f; _isPaused = false; break;
                case GameState.Paused: Time.timeScale = 0f; _isPaused = true; OnGamePaused?.Invoke(); break;
                case GameState.GameOver: CheckHighScore(); OnGameOver?.Invoke(); break;
                case GameState.Victory: CheckHighScore(); OnVictory?.Invoke(); break;
            }
        }

        public void PauseGame() { if (_currentState == GameState.Playing) ChangeState(GameState.Paused); }
        public void ResumeGame() { if (_currentState == GameState.Paused) { ChangeState(GameState.Playing); OnGameResumed?.Invoke(); } }
        public void AddScore(int points) { CurrentScore += points; }
        public void ResetScore() { CurrentScore = 0; }
        private void CheckHighScore() { if (CurrentScore > HighScore) { HighScore = CurrentScore; PlayerPrefs.SetInt("HighScore", HighScore); PlayerPrefs.Save(); } }
        public void StartGame() { ResetScore(); CurrentLevel = 1; ChangeState(GameState.Playing); }
        public void TriggerGameOver() { ChangeState(GameState.GameOver); }
        public void TriggerVictory() { ChangeState(GameState.Victory); }
    }
}
