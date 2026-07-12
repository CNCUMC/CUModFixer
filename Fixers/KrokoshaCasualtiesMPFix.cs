using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

// ── KrokoshaGunScriptTrackerComponent.Update ──
[HarmonyPatch]
internal static class KrokoshaUpdateGuard
{
    [HarmonyPrepare]
    public static bool Prepare() =>
        AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent") != null;

    [HarmonyTargetMethod]
    public static MethodInfo GetTargetMethod() =>
        AccessTools.Method(AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent"), "Update");

    [HarmonyFinalizer]
    public static void SuppressException(Exception __e)
    {
        if (__e == null) return;
        Plugin.Logger.LogWarning($"Suppressed Krokosha Update exception: {__e.Message}");
    }
}
