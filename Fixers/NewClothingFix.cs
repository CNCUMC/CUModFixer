using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace CUModFixer.Fixers;

[BepInDependency("com.rushellxyz.newclothing", BepInDependency.DependencyFlags.SoftDependency)]
internal static class NewClothingFix
{
    private static Type Type => AccessTools.TypeByName("NewClothing.RshClothing");
    private static bool _warned;

    [HarmonyPrepare]
    public static bool Prepare()
    {
        var ok = Type != null;
        if (ok) Plugin.Logger.LogInfo("  RshClothingGuard: target found");
        return ok;
    }

    [HarmonyTargetMethod]
    public static MethodBase Method() => AccessTools.Method(Type, "Update");

    [HarmonyPatch("NewClothing.RshClothing", "Update")]
    [HarmonyFinalizer]
    public static Exception UpdateFinalizer(Exception __e)
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