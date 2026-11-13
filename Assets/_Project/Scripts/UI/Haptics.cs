using UnityEngine;

namespace BlockBlast.UI
{
    public static class Haptics
    {
        public static void Pulse()
        {
            if (!SettingsPanel.HapticEnabled) return;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
