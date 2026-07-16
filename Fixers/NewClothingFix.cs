using System;
using System.Reflection;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class NewClothingFix
{
    private static bool _installed;
    private static FieldInfo _itField;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        var type = Type.GetType("NewClothing.RshClothing, NewClothing");
        if (type != null)
            _itField = AccessTools.Field(type, "it");

        FixerHelper.PatchMethodWithPrefix(harmony, typeof(NewClothingFix), "NewClothing.RshClothing, NewClothing", "Update", nameof(UpdatePrefix));

        _installed = true;
        Plugin.Logger.LogInfo("NewClothing patches installed.");
    }

    public static bool UpdatePrefix(object __instance)
    {
        if (_itField != null && _itField.GetValue(__instance) == null)
            return false;
        return true;
    }
}
