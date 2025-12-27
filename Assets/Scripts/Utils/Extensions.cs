using UnityEngine;
using System.Collections.Generic;

namespace MasterCheff.Utils
{
    public static class Extensions
    {
        public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
        public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
        public static Vector2 WithX(this Vector2 v, float x) => new Vector2(x, v.y);
        public static Vector2 WithY(this Vector2 v, float y) => new Vector2(v.x, y);
        public static Vector2 ToVector2XY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector3 ToVector3(this Vector2 v, float z = 0f) => new Vector3(v.x, v.y, z);
        public static Vector3 Flat(this Vector3 v) => new Vector3(v.x, 0f, v.z);

        public static void SetPositionX(this Transform t, float x) { t.position = t.position.WithX(x); }
        public static void SetPositionY(this Transform t, float y) { t.position = t.position.WithY(y); }
        public static void SetPositionZ(this Transform t, float z) { t.position = t.position.WithZ(z); }
        public static void Reset(this Transform t) { t.localPosition = Vector3.zero; t.localRotation = Quaternion.identity; t.localScale = Vector3.one; }
        public static void DestroyChildren(this Transform t) { for (int i = t.childCount - 1; i >= 0; i--) Object.Destroy(t.GetChild(i).gameObject); }

        public static Color WithAlpha(this Color c, float a) => new Color(c.r, c.g, c.b, a);
        public static T GetRandom<T>(this IList<T> list) => list == null || list.Count == 0 ? default : list[Random.Range(0, list.Count)];
        public static void Shuffle<T>(this IList<T> list) { int n = list.Count; while (n > 1) { n--; int k = Random.Range(0, n + 1); T v = list[k]; list[k] = list[n]; list[n] = v; } }
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component => go.GetComponent<T>() ?? go.AddComponent<T>();
        public static float Remap(this float v, float fromMin, float fromMax, float toMin, float toMax) => (v - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
