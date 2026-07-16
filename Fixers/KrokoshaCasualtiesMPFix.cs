using System;
using HarmonyLib;
using UnityEngine;

namespace CUModFixer.Fixers;

internal static class KrokoshaCasualtiesMPFix
{
    private static bool _installed;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        FixerHelper.PatchMethodWithPrefix(harmony, typeof(KrokoshaCasualtiesMPFix), "KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent, KrokoshaCasualtiesMP", "Update", nameof(UpdatePrefix));

        _installed = true;
        Plugin.Logger.LogInfo("KrokoshaCasualtiesMP patches installed.");
    }

    public static bool UpdatePrefix()
    {
        return PlayerCamera.main?.body != null;
    }
}
