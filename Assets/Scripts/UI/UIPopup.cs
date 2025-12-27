using UnityEngine;
using UnityEngine.UI;
using System;

namespace MasterCheff.UI
{
    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private bool _destroyOnClose = true;
        [SerializeField] private float _animationDuration = 0.2f;
        [SerializeField] private Button _closeButton;
        private CanvasGroup _canvasGroup;

        public event Action OnPopupOpened;
        public event Action OnPopupClosed;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            if (_closeButton != null) _closeButton.onClick.AddListener(Close);
        }

        public virtual void Show() { gameObject.SetActive(true); StartCoroutine(AnimateShow()); }
        public virtual void Hide() { StartCoroutine(AnimateHide()); }
        public void Close() { if (Managers.UIManager.HasInstance) Managers.UIManager.Instance.ClosePopup(this); else Hide(); }

        private System.Collections.IEnumerator AnimateShow()
        {
            _canvasGroup.alpha = 0; transform.localScale = Vector3.one * 0.8f;
            float t = 0;
            while (t < _animationDuration)
            {
                t += Time.unscaledDeltaTime; float p = t / _animationDuration;
                _canvasGroup.alpha = p; transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, p);
                yield return null;
            }
            _canvasGroup.alpha = 1; transform.localScale = Vector3.one; _canvasGroup.interactable = true;
            OnPopupOpened?.Invoke();
        }

        private System.Collections.IEnumerator AnimateHide()
        {
            _canvasGroup.interactable = false;
            float t = 0;
            while (t < _animationDuration)
            {
                t += Time.unscaledDeltaTime; float p = t / _animationDuration;
                _canvasGroup.alpha = 1 - p; transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, p);
                yield return null;
            }
            OnPopupClosed?.Invoke();
            if (_destroyOnClose) Destroy(gameObject); else gameObject.SetActive(false);
        }
    }
}
