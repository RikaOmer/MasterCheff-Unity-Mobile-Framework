using UnityEngine;
using System;

namespace MasterCheff.Utils
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _duration;
        private float _remaining;
        private bool _isRunning;
        private bool _isPaused;

        public event Action OnComplete;
        public event Action<float> OnTick;

        public float Duration => _duration;
        public float RemainingTime => _remaining;
        public float Progress => _duration > 0 ? (_duration - _remaining) / _duration : 0f;
        public bool IsRunning => _isRunning;
        public bool IsComplete => _remaining <= 0 && !_isRunning;

        public Timer(float duration) { _duration = duration; _remaining = duration; }

        public void Start() { _remaining = _duration; _isRunning = true; _isPaused = false; }
        public void Start(float d) { _duration = d; Start(); }
        public void Stop() { _isRunning = false; _isPaused = false; }
        public void Pause() { if (_isRunning) _isPaused = true; }
        public void Resume() { if (_isRunning) _isPaused = false; }
        public void Reset() { _remaining = _duration; _isRunning = false; }

        public void Tick(float dt)
        {
            if (!_isRunning || _isPaused) return;
            _remaining -= dt;
            OnTick?.Invoke(_remaining);
            if (_remaining <= 0) { _remaining = 0; _isRunning = false; OnComplete?.Invoke(); }
        }

        public string GetFormattedTime() { int m = Mathf.FloorToInt(_remaining / 60), s = Mathf.FloorToInt(_remaining % 60); return $"{m:00}:{s:00}"; }
    }

    public class Cooldown
    {
        private float _duration;
        private float _lastUse = float.MinValue;

        public float Duration => _duration;
        public float RemainingTime => Mathf.Max(0, _duration - (Time.time - _lastUse));
        public bool IsReady => Time.time >= _lastUse + _duration;

        public Cooldown(float d) { _duration = d; }
        public bool Use() { if (!IsReady) return false; _lastUse = Time.time; return true; }
        public void Reset() { _lastUse = float.MinValue; }
    }
}
