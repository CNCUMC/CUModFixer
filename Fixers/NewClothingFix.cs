using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
internal class NewClothingFix
{
    private static Type TargetType => AccessTools.TypeByName("NewClothing.RshClothing");
    private static bool _warned;

    [HarmonyPrepare] 
    public static bool Prepare() 
    {
        var ok = TargetType != null;
        Plugin.Logger.LogInfo($"[DEBUG] NewClothingFix.Prepare: TargetType={TargetType != null}, ok={ok}");
        return ok;
    }
    
    [HarmonyTargetMethod] 
    public static MethodBase TargetMethod() 
    {
        var m = AccessTools.DeclaredMethod(TargetType, "Update");
        Plugin.Logger.LogInfo($"[DEBUG] NewClothingFix.TargetMethod: method={m != null}");
        return m;
    }

    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __e)
    {
        switch (__e)
        {
            case null: return null;
            case NullReferenceException:
                if (_warned) return null;
                Plugin.Logger.LogWarning($"Suppressed NRE in RshClothing.Update: {__e.Message}");
                _warned = true;
                return null;
            default: return __e;
        }
    }
}
