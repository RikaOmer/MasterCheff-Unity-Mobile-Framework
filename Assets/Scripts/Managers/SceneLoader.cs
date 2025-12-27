using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using MasterCheff.Core;

namespace MasterCheff.Managers
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private float _minimumLoadTime = 0.5f;
        [SerializeField] private float _fadeTime = 0.3f;
        private CanvasGroup _fadeGroup;
        private bool _isLoading = false;

        public event Action OnLoadStart;
        public event Action<float> OnLoadProgress;
        public event Action OnLoadComplete;
        public bool IsLoading => _isLoading;
        public string CurrentScene => SceneManager.GetActiveScene().name;

        protected override void OnSingletonAwake() { CreateFadeCanvas(); }

        private void CreateFadeCanvas()
        {
            var canvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
            canvas.transform.SetParent(transform);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;
            var img = new GameObject("Fade").AddComponent<UnityEngine.UI.Image>();
            img.transform.SetParent(canvas.transform);
            img.color = Color.black;
            var rt = img.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.sizeDelta = Vector2.zero;
            _fadeGroup = img.gameObject.AddComponent<CanvasGroup>();
            _fadeGroup.alpha = 0; _fadeGroup.blocksRaycasts = false;
        }

        public void LoadScene(string sceneName) { if (!_isLoading) StartCoroutine(LoadAsync(sceneName)); }
        public void ReloadCurrentScene() { LoadScene(CurrentScene); }

        private IEnumerator LoadAsync(string scene)
        {
            _isLoading = true; OnLoadStart?.Invoke();
            yield return Fade(1f);
            var op = SceneManager.LoadSceneAsync(scene);
            op.allowSceneActivation = false;
            float start = Time.unscaledTime;
            while (!op.isDone)
            {
                OnLoadProgress?.Invoke(op.progress / 0.9f);
                if (op.progress >= 0.9f && Time.unscaledTime - start >= _minimumLoadTime) op.allowSceneActivation = true;
                yield return null;
            }
            yield return Fade(0f);
            _isLoading = false; OnLoadComplete?.Invoke();
        }

        private IEnumerator Fade(float target)
        {
            _fadeGroup.blocksRaycasts = true;
            float start = _fadeGroup.alpha, t = 0;
            while (t < _fadeTime) { t += Time.unscaledDeltaTime; _fadeGroup.alpha = Mathf.Lerp(start, target, t / _fadeTime); yield return null; }
            _fadeGroup.alpha = target; _fadeGroup.blocksRaycasts = target > 0.5f;
        }
    }
}
