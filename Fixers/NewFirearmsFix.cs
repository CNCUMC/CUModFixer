using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class NewFirearmsFix
{
    private static bool _installed;
    private static bool _isOnBackWarned;

    internal static void Install(Harmony harmony)
    {
        if (_installed) return;

        var nfAsm = Type.GetType("NewFirearms.RshGun, NewFirearms")?.Assembly;
        if (nfAsm == null) return;

        var rshGunType = nfAsm.GetType("NewFirearms.RshGun");

        var mpScareCheck = AccessTools.Method(rshGunType, "MpScareCheck");
        if (mpScareCheck != null)
        {
            harmony.Patch(mpScareCheck,
                prefix: new HarmonyMethod(typeof(NewFirearmsFix), nameof(MpScareCheckPrefix)),
                finalizer: new HarmonyMethod(typeof(NewFirearmsFix), nameof(MpScareCheckFinalizer)));
        }

        var isOnBack = AccessTools.Method(rshGunType, "IsOnBack");
        if (isOnBack != null)
        {
            harmony.Patch(isOnBack,
                finalizer: new HarmonyMethod(typeof(NewFirearmsFix), nameof(IsOnBackFinalizer)));
        }

        var cameraPatchType = nfAsm.GetType("NewFirearms.PlayerCameraPatch1");
        if (cameraPatchType != null)
        {
            var handleGunUi = AccessTools.Method(cameraPatchType, "HandleLegacyGunUi");
            if (handleGunUi != null)
            {
                harmony.Patch(handleGunUi,
                    finalizer: new HarmonyMethod(typeof(NewFirearmsFix), nameof(HandleLegacyGunUiFinalizer)));
            }
        }

        _installed = true;
        Plugin.Logger.LogInfo("NewFirearms patches installed.");
    }

    public static bool MpScareCheckPrefix(object __instance)
    {
        try
        {
            var mb = __instance as UnityEngine.MonoBehaviour;
            if (mb == null) return false;
            var trackerType = Type.GetType("KrokoshaCasualtiesMP.KrokoshaScavMultiGameObjectNetworkTracker, KrokoshaCasualtiesMP");
            if (trackerType == null) return false;
            return mb.gameObject.GetComponent(trackerType) != null;
        }
        catch { return false; }
    }

    public static Exception MpScareCheckFinalizer(Exception __exception)
    {
        if (__exception == null) return null;
        Plugin.Logger.LogWarning($"Suppressed NewFirearms MpScareCheck exception: {__exception.Message}");
        return null;
    }

    public static Exception IsOnBackFinalizer(Exception __exception, ref bool __result)
    {
        if (__exception == null) return null;
        if (!_isOnBackWarned)
        {
            Plugin.Logger.LogWarning($"Suppressed NewFirearms IsOnBack exception: {__exception.Message}");
            _isOnBackWarned = true;
        }
        __result = false;
        return null;
    }

    public static Exception HandleLegacyGunUiFinalizer(Exception __exception) => null;
}

[HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning", [typeof(object)])]
internal static class StunLogSuppressor
{
    [HarmonyPrepare]
    public static bool Prepare() =>
        Type.GetType("NewFirearms.RshGun, NewFirearms") != null;

    private static bool _stunWarned;

    [HarmonyPrefix]
    public static bool Prefix(object message)
    {
        if (message?.ToString().Contains("Can not add stun collider") != true) return true;
        if (_stunWarned) return false;
        _stunWarned = true;
        return true;
    }
}