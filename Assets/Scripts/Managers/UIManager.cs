using UnityEngine;
using System;
using System.Collections.Generic;
using MasterCheff.Core;
using MasterCheff.UI;

namespace MasterCheff.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Canvas _mainCanvas;
        private Dictionary<string, UIPanel> _panels = new Dictionary<string, UIPanel>();
        private Stack<UIPanel> _panelStack = new Stack<UIPanel>();
        private Stack<UIPopup> _popupStack = new Stack<UIPopup>();

        public event Action<UIPanel> OnPanelOpened;
        public event Action<UIPopup> OnPopupOpened;
        public UIPanel CurrentPanel => _panelStack.Count > 0 ? _panelStack.Peek() : null;
        public UIPopup CurrentPopup => _popupStack.Count > 0 ? _popupStack.Peek() : null;
        public bool HasOpenPopup => _popupStack.Count > 0;

        protected override void OnSingletonAwake()
        {
            // Unity 2022.3+ uses FindFirstObjectByType instead of deprecated FindObjectOfType
            if (_mainCanvas == null) _mainCanvas = FindFirstObjectByType<Canvas>();
            RegisterExistingPanels();
        }

        private void RegisterExistingPanels()
        {
            // Unity 2022.3+ uses FindObjectsByType instead of deprecated FindObjectsOfType
            foreach (var panel in FindObjectsByType<UIPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                RegisterPanel(panel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (HasOpenPopup) CloseCurrentPopup();
                else if (_panelStack.Count > 1) GoBack();
            }
        }

        public void RegisterPanel(UIPanel panel) { if (panel != null && !_panels.ContainsKey(panel.PanelName)) _panels[panel.PanelName] = panel; }
        public void ShowPanel(string name) { if (_panels.TryGetValue(name, out var panel)) ShowPanel(panel); }
        public void ShowPanel(UIPanel panel) { if (CurrentPanel != null && CurrentPanel != panel) CurrentPanel.Hide(); _panelStack.Push(panel); panel.Show(); OnPanelOpened?.Invoke(panel); }
        public void GoBack() { if (_panelStack.Count > 1) { _panelStack.Pop().Hide(); CurrentPanel?.Show(); } }
        public void ShowPopup(UIPopup popup) { _popupStack.Push(popup); popup.Show(); OnPopupOpened?.Invoke(popup); }
        public void ClosePopup(UIPopup popup) { popup.Hide(); }
        public void CloseCurrentPopup() { if (CurrentPopup != null) { _popupStack.Pop().Hide(); } }
        public void CloseAllPopups() { while (_popupStack.Count > 0) _popupStack.Pop().Hide(); }
    }
}
