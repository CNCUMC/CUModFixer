using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
internal class KrokoshaCasualtiesMPFix
{
    private static Type TargetType => AccessTools.TypeByName("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent");
    
    [HarmonyPrepare]
    public static bool Prepare()
    {
        return TargetType != null;
    }
    
    [HarmonyTargetMethod] 
    public static MethodBase TargetMethod() 
    {
        return AccessTools.DeclaredMethod(TargetType, "Update");
    }
    
    [HarmonyFinalizer] 
    public static Exception Finalizer(Exception __e) => null;
}
