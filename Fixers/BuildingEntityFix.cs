using HarmonyLib;
using UnityEngine;

namespace CUModFixer.Fixers;

internal static class BuildingEntityFix
{
    private static bool _installed;
    private static GameObject _dustBigPrefab;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        FixerHelper.PatchMethodWithPrefix(harmony, typeof(BuildingEntityFix), "BuildingEntity, Assembly-CSharp", "Update", nameof(UpdatePrefix));

        _installed = true;
        Plugin.Logger.LogInfo("BuildingEntity patches installed.");
    }

    public static bool UpdatePrefix()
    {
        if (_dustBigPrefab == null)
            _dustBigPrefab = Resources.Load<GameObject>("DustBig");

        if (_dustBigPrefab == null)
            return false;
        return true;
    }
}
