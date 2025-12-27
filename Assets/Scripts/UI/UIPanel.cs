using UnityEngine;
using System;

namespace MasterCheff.UI
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] private string _panelName;
        [SerializeField] private float _animationDuration = 0.3f;
        private CanvasGroup _canvasGroup;
        private bool _isShowing = false;

        public event Action OnPanelShown;
        public event Action OnPanelHidden;
        public string PanelName => string.IsNullOrEmpty(_panelName) ? gameObject.name : _panelName;
        public bool IsShowing => _isShowing;

        protected virtual void Awake() { _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>(); }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            _isShowing = true;
            StartCoroutine(AnimateShow());
        }

        public virtual void Hide() { StartCoroutine(AnimateHide()); }

        private System.Collections.IEnumerator AnimateShow()
        {
            _canvasGroup.alpha = 0;
            float t = 0;
            while (t < _animationDuration) { t += Time.unscaledDeltaTime; _canvasGroup.alpha = t / _animationDuration; yield return null; }
            _canvasGroup.alpha = 1; _canvasGroup.interactable = true; _canvasGroup.blocksRaycasts = true;
            OnPanelShown?.Invoke();
        }

        private System.Collections.IEnumerator AnimateHide()
        {
            _canvasGroup.interactable = false;
            float t = 0;
            while (t < _animationDuration) { t += Time.unscaledDeltaTime; _canvasGroup.alpha = 1 - t / _animationDuration; yield return null; }
            _isShowing = false; _canvasGroup.blocksRaycasts = false; gameObject.SetActive(false);
            OnPanelHidden?.Invoke();
        }
    }
}
