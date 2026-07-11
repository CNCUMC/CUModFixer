using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class KrokoshaCasualtiesMPFix
{
    private static bool _installed;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        var trackerType = 
            Type.GetType("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent, KrokoshaCasualtiesMP");
        if (trackerType == null) return;

        var updateMethod = AccessTools.Method(trackerType, "Update");
        if (updateMethod != null)
        {
            harmony.Patch(updateMethod,
                finalizer: new HarmonyMethod(typeof(KrokoshaCasualtiesMPFix), nameof(UpdateFinalizer)));
        }

        _installed = true;
        Plugin.Logger.LogInfo("Krokosha patches installed.");
    }

    public static Exception UpdateFinalizer(Exception __exception) => null;
}