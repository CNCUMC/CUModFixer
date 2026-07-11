using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

// ── MpScareCheck ──
[HarmonyPatch]
internal static class MpScareCheckGuard
{
    [HarmonyPrepare]
    public static bool Prepare() => AccessTools.TypeByName("NewFirearms.RshGun") != null;

    [HarmonyTargetMethod]
    public static MethodInfo GetTargetMethod() => AccessTools.Method(AccessTools.TypeByName("NewFirearms.RshGun"), "MpScareCheck");

    // Prefix: skip original if no Krokosha tracker component exists (prevents "Attempt to action on item without..." exception)
    [HarmonyPrefix]
    public static bool SkipIfNoTracker(object __instance)
    {
        try
        {
            var mb = __instance as UnityEngine.MonoBehaviour;
            if (mb == null) return true; // No MonoBehaviour, allow original

            var trackerType = Type.GetType("KrokoshaCasualtiesMP.KrokoshaScavMultiGameObjectNetworkTracker, KrokoshaCasualtiesMP");
            if (trackerType == null) return true; // No tracker type at all, allow original

            // If gun has NO tracker component, skip the original to prevent exception
            var hasTracker = mb.gameObject.GetComponent(trackerType) != null;
            return hasTracker;
        }
        catch 
        {
            return false; // On error, skip original to prevent exception
        }
    }

    // Finalizer: suppress all exceptions from MpScareCheck (safety net)
    [HarmonyFinalizer]
    public static void SuppressException(Exception __e)
    {
        if (__e == null) return;
        Plugin.Logger.LogWarning($"Suppressed NewFirearms MpScareCheck exception: {__e.Message}");
    }
}

// ── IsOnBack ──
[HarmonyPatch]
internal static class IsOnBackGuard
{
    [HarmonyPrepare]
    public static bool Prepare() => AccessTools.TypeByName("NewFirearms.RshGun") != null;

    [HarmonyTargetMethod]
    public static MethodInfo GetTargetMethod() => AccessTools.Method(AccessTools.TypeByName("NewFirearms.RshGun"), "IsOnBack");

    [HarmonyFinalizer]
    public static void SuppressException(Exception __e, ref bool __r)
    {
        if (__e == null) return;
        if (_isOnBackWarned) return;
        Plugin.Logger.LogWarning($"Suppressed NewFirearms IsOnBack exception: {__e.Message}");
        _isOnBackWarned = true;
        __r = false;
    }

    private static bool _isOnBackWarned;
}

// ── HandleLegacyGunUi ──
[HarmonyPatch]
internal static class LegacyGunUiGuard
{
    [HarmonyPrepare]
    public static bool Prepare() => AccessTools.TypeByName("NewFirearms.PlayerCameraPatch1") != null;

    [HarmonyTargetMethod]
    public static MethodInfo GetTargetMethod() => AccessTools.Method(AccessTools.TypeByName("NewFirearms.PlayerCameraPatch1"), "HandleLegacyGunUi");

    [HarmonyFinalizer]
    public static void SuppressException(Exception __e) { }
}

// ── Stun collider log spam ──
[HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning", [typeof(object)])]
internal static class StunLogGuard
{
    [HarmonyPrefix]
    public static bool FilterStunWarning(object message)
    {
        if (message?.ToString()?.Contains("Can not add stun collider") != true) return true;
        if (_stunWarned) return false;
        _stunWarned = true;
        return true;
    }

    private static bool _stunWarned;
}
