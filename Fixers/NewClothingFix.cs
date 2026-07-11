using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch]
internal class NewClothingFix
{
    private static Type TargetType => AccessTools.TypeByName("NewClothing.RshClothing");
    private static bool _warned;

    [HarmonyPrepare] public static bool Prepare() => TargetType != null;
    [HarmonyTargetMethod] public static MethodBase TargetMethod() => AccessTools.Method(TargetType, "Update");

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
