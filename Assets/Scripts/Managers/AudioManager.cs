using UnityEngine;
using System.Collections.Generic;
using MasterCheff.Core;

namespace MasterCheff.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private int _sfxPoolSize = 10;

        private List<AudioSource> _sfxPool;
        private int _currentPoolIndex = 0;
        private float _masterVolume = 1f;
        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;
        private bool _isMusicMuted = false;
        private bool _isSFXMuted = false;

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SFXVolume => _sfxVolume;

        protected override void OnSingletonAwake()
        {
            if (_musicSource == null)
            {
                var obj = new GameObject("MusicSource");
                obj.transform.SetParent(transform);
                _musicSource = obj.AddComponent<AudioSource>();
                _musicSource.loop = true;
            }

            _sfxPool = new List<AudioSource>();
            var container = new GameObject("SFXPool");
            container.transform.SetParent(transform);
            for (int i = 0; i < _sfxPoolSize; i++)
            {
                var sfx = new GameObject($"SFX_{i}").AddComponent<AudioSource>();
                sfx.transform.SetParent(container.transform);
                _sfxPool.Add(sfx);
            }

            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        public void PlayMusic(AudioClip clip) { if (clip != null) { _musicSource.clip = clip; _musicSource.Play(); } }
        public void StopMusic() { _musicSource.Stop(); }

        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null || _isSFXMuted) return;
            var src = _sfxPool[_currentPoolIndex];
            _currentPoolIndex = (_currentPoolIndex + 1) % _sfxPool.Count;
            src.clip = clip;
            src.volume = _sfxVolume * _masterVolume * volumeScale;
            src.Play();
        }

        public void PlayUISound(AudioClip clip, float volumeScale = 1f) { PlaySFX(clip, volumeScale); }
        public void SetMusicVolume(float v) { _musicVolume = Mathf.Clamp01(v); _musicSource.volume = _isMusicMuted ? 0 : _musicVolume * _masterVolume; }
        public void SetSFXVolume(float v) { _sfxVolume = Mathf.Clamp01(v); }
        public void SetMusicMuted(bool m) { _isMusicMuted = m; SetMusicVolume(_musicVolume); }
        public void SetSFXMuted(bool m) { _isSFXMuted = m; }
    }
}
