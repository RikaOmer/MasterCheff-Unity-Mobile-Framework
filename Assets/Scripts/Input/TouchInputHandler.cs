using UnityEngine;
using System;
using System.Collections.Generic;
using MasterCheff.Core;

namespace MasterCheff.Input
{
    public class TouchInputHandler : Singleton<TouchInputHandler>
    {
        [SerializeField] private float _tapThreshold = 0.2f;
        [SerializeField] private float _swipeMinDistance = 50f;
        [SerializeField] private float _swipeMaxTime = 0.5f;
        [SerializeField] private float _holdThreshold = 0.5f;

        private Dictionary<int, TouchData> _touches = new Dictionary<int, TouchData>();
        private float _lastTapTime;
        private Vector2 _lastTapPosition;

        public event Action<Vector2> OnTap;
        public event Action<Vector2> OnDoubleTap;
        public event Action<Vector2> OnHoldStart;
        public event Action<Vector2> OnHoldEnd;
        public event Action<SwipeDirection, Vector2> OnSwipe;
        public event Action<float> OnPinch;
        public event Action<Vector2, Vector2> OnDrag;

        public int TouchCount => UnityEngine.Input.touchCount;
        public bool IsTouching => UnityEngine.Input.touchCount > 0;

        public enum SwipeDirection { None, Up, Down, Left, Right }
        private class TouchData { public Vector2 startPos; public float startTime; public bool isHolding; public bool hasMoved; }

        private void Update()
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                var touch = UnityEngine.Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touches[touch.fingerId] = new TouchData { startPos = touch.position, startTime = Time.time };
                        break;
                    case TouchPhase.Moved:
                        if (_touches.TryGetValue(touch.fingerId, out var md))
                        {
                            if (Vector2.Distance(md.startPos, touch.position) > 20) md.hasMoved = true;
                            OnDrag?.Invoke(touch.position, touch.deltaPosition);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (_touches.TryGetValue(touch.fingerId, out var sd) && !sd.isHolding && !sd.hasMoved && Time.time - sd.startTime >= _holdThreshold)
                        {
                            sd.isHolding = true;
                            OnHoldStart?.Invoke(touch.position);
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (_touches.TryGetValue(touch.fingerId, out var ed))
                        {
                            float dur = Time.time - ed.startTime;
                            float dist = Vector2.Distance(ed.startPos, touch.position);
                            if (ed.isHolding) OnHoldEnd?.Invoke(touch.position);
                            else if (!ed.hasMoved && dur < _tapThreshold) HandleTap(touch.position);
                            else if (dur < _swipeMaxTime && dist > _swipeMinDistance) OnSwipe?.Invoke(GetSwipeDir(ed.startPos, touch.position), touch.position);
                            _touches.Remove(touch.fingerId);
                        }
                        break;
                }
            }
            if (UnityEngine.Input.touchCount == 2) HandlePinch();
        }

        private void HandleTap(Vector2 pos)
        {
            if (Time.time - _lastTapTime < 0.3f && Vector2.Distance(pos, _lastTapPosition) < 50) { OnDoubleTap?.Invoke(pos); _lastTapTime = 0; }
            else { OnTap?.Invoke(pos); _lastTapTime = Time.time; _lastTapPosition = pos; }
        }

        private float _lastPinchDist;
        private void HandlePinch()
        {
            var t0 = UnityEngine.Input.GetTouch(0); var t1 = UnityEngine.Input.GetTouch(1);
            float dist = Vector2.Distance(t0.position, t1.position);
            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began) _lastPinchDist = dist;
            else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
            {
                float delta = (dist - _lastPinchDist) / Screen.height;
                if (Mathf.Abs(delta) > 0.01f) { OnPinch?.Invoke(delta); _lastPinchDist = dist; }
            }
        }

        private SwipeDirection GetSwipeDir(Vector2 start, Vector2 end)
        {
            Vector2 d = end - start;
            return Mathf.Abs(d.x) > Mathf.Abs(d.y) ? (d.x > 0 ? SwipeDirection.Right : SwipeDirection.Left) : (d.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
        }

        public Vector2 ScreenToWorld2D(Vector2 screenPos) => Camera.main?.ScreenToWorldPoint(screenPos) ?? Vector2.zero;
    }
}
