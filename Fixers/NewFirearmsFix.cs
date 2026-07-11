using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

// ── MpScareCheck ──
[HarmonyPatch]
internal class MpScareCheckGuard
{
    private static Type TargetType => AccessTools.TypeByName("NewFirearms.RshGun");

    [HarmonyPrepare]
    public static bool Prepare()
    {
        return TargetType != null;
    }

    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        return AccessTools.DeclaredMethod(TargetType, "MpScareCheck");
    }

    [HarmonyPrefix]
    public static bool Prefix(object __instance)
    {
        try
        {
            var mb = __instance as UnityEngine.MonoBehaviour;
            if (mb == null) return false;
            var tt = Type.GetType("KrokoshaCasualtiesMP.KrokoshaScavMultiGameObjectNetworkTracker, KrokoshaCasualtiesMP");
            return tt != null && mb.gameObject.GetComponent(tt) != null;
        }
        catch { return false; }
    }

    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __e)
    {
        if (__e == null) return null;
        Plugin.Logger.LogWarning($"Suppressed NewFirearms MpScareCheck exception: {__e.Message}");
        return null;
    }
}

// ── IsOnBack ──
[HarmonyPatch]
internal class IsOnBackGuard
{
    private static bool _warned;

    [HarmonyPrepare]
    public static bool Prepare()
    {
        return AccessTools.TypeByName("NewFirearms.RshGun") != null;
    }

    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        return AccessTools.DeclaredMethod(AccessTools.TypeByName("NewFirearms.RshGun"), "IsOnBack");
    }

    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __e, ref bool __r)
    {
        if (__e == null) return null;
        if (!_warned) { Plugin.Logger.LogWarning($"Suppressed NewFirearms IsOnBack exception: {__e.Message}"); _warned = true; }
        __r = false;
        return null;
    }
}

// ── HandleLegacyGunUi ──
[HarmonyPatch]
internal class LegacyGunUiGuard
{
    [HarmonyPrepare]
    public static bool Prepare()
    {
        return AccessTools.TypeByName("NewFirearms.PlayerCameraPatch1") != null;
    }

    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        return AccessTools.DeclaredMethod(AccessTools.TypeByName("NewFirearms.PlayerCameraPatch1"), "HandleLegacyGunUi");
    }

    [HarmonyFinalizer] public static Exception Finalizer(Exception __e) => null;
}

// ── Stun collider log spam ──
[HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning", new[] { typeof(object) })]
internal class StunLogGuard
{
    private static bool _warned;

    [HarmonyPrefix]
    public static bool Prefix(object message)
    {
        if (message?.ToString()?.Contains("Can not add stun collider") != true) return true;
        if (_warned) return false;
        _warned = true;
        return true;
    }
}
