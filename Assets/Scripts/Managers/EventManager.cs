using System;
using System.Collections.Generic;
using MasterCheff.Core;

namespace MasterCheff.Managers
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, Action> _events = new Dictionary<string, Action>();
        private Dictionary<string, Delegate> _paramEvents = new Dictionary<string, Delegate>();

        public void Subscribe(string name, Action listener) { if (_events.ContainsKey(name)) _events[name] += listener; else _events[name] = listener; }
        public void Unsubscribe(string name, Action listener) { if (_events.ContainsKey(name)) _events[name] -= listener; }
        public void Trigger(string name) { if (_events.TryGetValue(name, out var e)) e?.Invoke(); }

        public void Subscribe<T>(string name, Action<T> listener) { if (_paramEvents.ContainsKey(name)) _paramEvents[name] = Delegate.Combine(_paramEvents[name], listener); else _paramEvents[name] = listener; }
        public void Trigger<T>(string name, T param) { if (_paramEvents.TryGetValue(name, out var e)) (e as Action<T>)?.Invoke(param); }
        public void ClearEvent(string name) { _events.Remove(name); _paramEvents.Remove(name); }
        public void ClearAllEvents() { _events.Clear(); _paramEvents.Clear(); }
    }

    public static class GameEvents
    {
        public const string GAME_START = "GameStart";
        public const string GAME_PAUSE = "GamePause";
        public const string GAME_OVER = "GameOver";
        public const string PLAYER_SCORE = "PlayerScore";
        public const string ITEM_COLLECTED = "ItemCollected";
    }
}
