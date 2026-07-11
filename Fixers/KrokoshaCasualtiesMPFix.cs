using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
class KrokoshaCasualtiesMPFix
{
    [HarmonyPrepare]
    public static bool Prepare()
    {
        return AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent") != null;
    }

    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        return AccessTools.DeclaredMethod(AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent"), "Update");
    }

    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __e) => null;
}
