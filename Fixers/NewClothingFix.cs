using System;
using BepInEx;
using HarmonyLib;

namespace CUModFixer.Fixers;

[BepInDependency("com.rushellxyz.newclothing", BepInDependency.DependencyFlags.SoftDependency)]
internal static class NewClothingFix
{
    private static bool _warned;

    [HarmonyPatch("NewClothing.RshClothing", "Update")]
    [HarmonyFinalizer]
    public static Exception UpdateFin(Exception __e)
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