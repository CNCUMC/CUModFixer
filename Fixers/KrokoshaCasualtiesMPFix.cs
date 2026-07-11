using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
internal static class KrokoshaCasualtiesMPFix
{
    private static Type TargetType => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");

    [HarmonyPrepare]
    public static bool Prepare() => TargetType != null;

    [HarmonyTargetMethod]
    public static MethodBase TargetMethod() => AccessTools.Method(TargetType, "Update");

    [HarmonyFinalizer]
    public static Exception UpdateFinalizer(Exception __exception) => null;
}