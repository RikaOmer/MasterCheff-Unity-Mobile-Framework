using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MasterCheff.Managers;

namespace MasterCheff.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _pressedScale = 0.95f;
        [SerializeField] private float _animationSpeed = 10f;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private bool _enableHaptic = true;

        private Button _button;
        private Vector3 _originalScale;
        private Vector3 _targetScale;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _originalScale = transform.localScale;
            _targetScale = _originalScale;
            _button.onClick.AddListener(OnClick);
        }

        private void Update() { transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.unscaledDeltaTime * _animationSpeed); }

        public void OnPointerDown(PointerEventData e) { if (_button.interactable) _targetScale = _originalScale * _pressedScale; }
        public void OnPointerUp(PointerEventData e) { _targetScale = _originalScale; }

        private void OnClick()
        {
            if (_clickSound != null && AudioManager.HasInstance) AudioManager.Instance.PlayUISound(_clickSound);
            if (_enableHaptic)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
            }
        }
    }
}
