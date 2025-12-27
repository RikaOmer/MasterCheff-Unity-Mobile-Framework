using UnityEngine;

namespace MasterCheff.Utils
{
    public static class MobileUtils
    {
        public static Rect GetSafeArea() => Screen.safeArea;
        public static bool HasNotch() { var s = Screen.safeArea; return s.y > 0 || s.x > 0 || s.width < Screen.width || s.height < Screen.height; }
        public static float GetAspectRatio() => (float)Screen.width / Screen.height;
        public static bool IsTablet() => GetScreenDiagonalInches() >= 7f || GetAspectRatio() < 1.5f;
        public static float GetScreenDiagonalInches() { float w = Screen.width / Screen.dpi, h = Screen.height / Screen.dpi; return Mathf.Sqrt(w * w + h * h); }
        public static string GetDeviceId() => SystemInfo.deviceUniqueIdentifier;
        public static string GetDeviceModel() => SystemInfo.deviceModel;
        public static int GetAvailableMemoryMB() => SystemInfo.systemMemorySize;
        public static bool IsLowEndDevice() => GetAvailableMemoryMB() < 2048;
        public static float GetBatteryLevel() => SystemInfo.batteryLevel;
        public static bool IsCharging() => SystemInfo.batteryStatus == BatteryStatus.Charging;
        public static bool IsBatteryLow() => SystemInfo.batteryLevel < 0.2f && SystemInfo.batteryLevel >= 0f;
        public static void SetQualityForDevice() { int m = GetAvailableMemoryMB(); QualitySettings.SetQualityLevel(m >= 4096 ? 5 : m >= 3072 ? 4 : m >= 2048 ? 3 : m >= 1024 ? 2 : 1); }
        public static void Vibrate() {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
        public static bool HasInternetConnection() => Application.internetReachability != NetworkReachability.NotReachable;
        public static bool IsOnWiFi() => Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        public static void OpenURL(string url) => Application.OpenURL(url);
    }
}
