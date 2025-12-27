using UnityEngine;

namespace MasterCheff.Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaHandler : MonoBehaviour
    {
        [SerializeField] private bool _conformX = true;
        [SerializeField] private bool _conformY = true;

        private RectTransform _rt;
        private Rect _lastSafeArea;

        private void Awake() { _rt = GetComponent<RectTransform>(); ApplySafeArea(); }
        private void Update() { if (_lastSafeArea != Screen.safeArea) ApplySafeArea(); }

        public void ApplySafeArea()
        {
            var safe = Screen.safeArea;
            if (safe == _lastSafeArea) return;
            _lastSafeArea = safe;

            Vector2 min = safe.position, max = safe.position + safe.size;
            min.x /= Screen.width; min.y /= Screen.height;
            max.x /= Screen.width; max.y /= Screen.height;

            if (!_conformX) { min.x = _rt.anchorMin.x; max.x = _rt.anchorMax.x; }
            if (!_conformY) { min.y = _rt.anchorMin.y; max.y = _rt.anchorMax.y; }

            _rt.anchorMin = min; _rt.anchorMax = max;
        }
    }
}
