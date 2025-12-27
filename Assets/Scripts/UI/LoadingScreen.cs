using UnityEngine;
using UnityEngine.UI;
using MasterCheff.Managers;

namespace MasterCheff.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private GameObject _loadingIcon;
        [SerializeField] private float _rotationSpeed = 180f;

        private float _targetProgress = 0f;

        private void Awake() { if (SceneLoader.HasInstance) SceneLoader.Instance.OnLoadProgress += p => _targetProgress = p; }
        private void OnDestroy() { if (SceneLoader.HasInstance) SceneLoader.Instance.OnLoadProgress -= p => _targetProgress = p; }

        private void Update()
        {
            if (_progressBar != null) _progressBar.value = Mathf.Lerp(_progressBar.value, _targetProgress, Time.deltaTime * 3f);
            if (_loadingIcon != null) _loadingIcon.transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
        }

        public void SetProgress(float p) { _targetProgress = Mathf.Clamp01(p); }
    }
}
