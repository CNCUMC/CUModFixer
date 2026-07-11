using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

[HarmonyPatch("NewClothing.RshClothing", "Update")]
internal static class NewClothingGuard
{
    [HarmonyFinalizer]
    public static void SuppressException(Exception __e)
    {
        if (__e == null) return;
        if (_warned) return;
        Plugin.Logger.LogWarning($"Suppressed NRE in RshClothing.Update: {__e.Message}");
        _warned = true;
    }

    private static bool _warned;
}
