using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class GameFix
{
    private static bool _warned;

    [HarmonyPatch(typeof(BuildingEntity), "Update")]
    [HarmonyFinalizer]
    public static Exception BuildingEntityUpdateFinalizer(Exception __exception)
    {
        switch (__exception)
        {
            case null:
                return null;
            case ArgumentException argEx:
            {
                if (_warned) return null;
                _warned = true;
                Plugin.Logger.LogWarning("Suppressed ArgumentException in BuildingEntity.Update: " + argEx.Message);
                return null;
            }
            default:
                return __exception;
        }
    }
}
