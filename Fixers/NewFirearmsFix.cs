using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class NewFirearmsFix
{
    private static bool _installed;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        FixerHelper.PatchMethodWithPrefix(harmony, typeof(NewFirearmsFix), "NewFirearms.RshGun, NewFirearms", "MpScareCheck", nameof(MpScareCheckPrefix));
        FixerHelper.PatchMethodWithPrefix(harmony, typeof(NewFirearmsFix), "NewFirearms.RshGun, NewFirearms", "IsOnBack", nameof(IsOnBackPrefix));
        FixerHelper.PatchMethodWithPrefix(harmony, typeof(NewFirearmsFix), "NewFirearms.PlayerCameraPatch1, NewFirearms", "HandleLegacyGunUi", nameof(HandleLegacyGunUiPrefix));

        _installed = true;
        Plugin.Logger.LogInfo("NewFirearms patches installed.");
    }

    public static bool MpScareCheckPrefix(object __instance)
    {
        var type = __instance.GetType();
        var prop = type.GetProperty("NetworkTracker");
        if (prop == null) return true;
        var value = prop.GetValue(__instance);
        if (value == null) return false;
        return true;
    }

    public static bool IsOnBackPrefix(object __instance)
    {
        var type = __instance.GetType();
        var field = type.GetField("body", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field == null) return true;
        var body = field.GetValue(__instance);
        if (body == null) return false;
        return true;
    }

    public static bool HandleLegacyGunUiPrefix()
    {
        return PlayerCamera.main?.body != null;
    }
}
