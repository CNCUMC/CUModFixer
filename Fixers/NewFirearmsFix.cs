using System;
using BepInEx;
using HarmonyLib;

namespace CUModFixer.Fixers;

[BepInDependency("com.rushellxyz.newfirearms", BepInDependency.DependencyFlags.SoftDependency)]
internal static class NewFirearmsFix
{
    private static bool _isOnBackWarned;
    private static bool _stunWarned;

    [HarmonyPatch("NewFirearms.RshGun", "MpScareCheck")]
    [HarmonyPrefix]
    public static bool MpScareCheckPrefix(object __instance)
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

    [HarmonyPatch("NewFirearms.RshGun", "MpScareCheck")]
    [HarmonyFinalizer]
    public static Exception MpScareCheckFin(Exception __e)
    {
        if (__e == null) return null;
        Plugin.Logger.LogWarning($"Suppressed NewFirearms MpScareCheck exception: {__e.Message}");
        return null;
    }

    [HarmonyPatch("NewFirearms.RshGun", "IsOnBack")]
    [HarmonyFinalizer]
    public static Exception IsOnBackFin(Exception __e, ref bool __r)
    {
        if (__e == null) return null;
        if (!_isOnBackWarned) { Plugin.Logger.LogWarning($"Suppressed NewFirearms IsOnBack exception: {__e.Message}"); _isOnBackWarned = true; }
        __r = false;
        return null;
    }

    [HarmonyPatch("NewFirearms.PlayerCameraPatch1", "HandleLegacyGunUi")]
    [HarmonyFinalizer]
    public static Exception HandleLegacyGunUiFin(Exception __e) => null;

    // ── Stun collider log spam ──
    [HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning", [typeof(object)])]
    [HarmonyPrefix]
    public static bool StunLogPrefix(object message)
    {
        if (message?.ToString()?.Contains("Can not add stun collider") != true) return true;
        if (_stunWarned) return false;
        _stunWarned = true;
        return true;
    }
}