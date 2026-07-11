using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class NewClothingFix
{
    private static bool _installed;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        var ncAsm = Type.GetType("NewClothing.RshClothing, NewClothing")?.Assembly;
        if (ncAsm == null) return;

        var rshClothingType = ncAsm.GetType("NewClothing.RshClothing");
        if (rshClothingType == null) return;

        var updateMethod = AccessTools.Method(rshClothingType, "Update");
        if (updateMethod != null)
        {
            harmony.Patch(updateMethod,
                finalizer: new HarmonyMethod(typeof(NewClothingFix), nameof(UpdateFinalizer)));
        }

        _installed = true;
        Plugin.Logger.LogInfo("NewClothing patches installed.");
    }

    public static Exception UpdateFinalizer(Exception __exception)
    {
        switch (__exception)
        {
            case null:
                return null;
            case NullReferenceException:
                Plugin.Logger.LogWarning($"Suppressed NRE in RshClothing.Update: {__exception.Message}");
                return null;
            default:
                return __exception;
        }
    }
}