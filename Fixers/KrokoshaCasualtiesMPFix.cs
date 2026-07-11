using System;
using BepInEx;
using HarmonyLib;

namespace CUModFixer.Fixers;

[BepInDependency("KrokoshaCasualtiesMP", BepInDependency.DependencyFlags.SoftDependency)]
internal static class KrokoshaCasualtiesMPFix
{
    [HarmonyPatch("KrokoshaCasualtiesMP.KrokoshaGunScriptTrackerComponent", "Update")]
    [HarmonyFinalizer]
    public static Exception UpdateFinalizer(Exception __e) => null;
}