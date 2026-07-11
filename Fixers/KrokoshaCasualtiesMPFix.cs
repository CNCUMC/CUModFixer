using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
internal class KrokoshaCasualtiesMPFix
{
    [HarmonyPrepare] public static bool Prepare() => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent") != null;
    [HarmonyTargetMethod] public static MethodBase TargetMethod() => AccessTools.Method(AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent"), "Update");
    [HarmonyFinalizer] public static Exception Finalizer(Exception __e) => null;
}
