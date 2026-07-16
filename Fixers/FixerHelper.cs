using System;
using HarmonyLib;

namespace CUModFixer.Fixers;

internal static class FixerHelper
{
    public static void PatchMethod(Harmony harmony, Type fixerType, string fullTypeName, string methodName,
        string finalizerName)
    {
        var type = Type.GetType(fullTypeName);
        var assembly = type?.Assembly;
        if (assembly == null) return;

        var shortName = fullTypeName.Split(',')[0].Trim();
        var type2 = assembly.GetType(shortName);
        if (type2 == null) return;

        var methodInfo = AccessTools.Method(type2, methodName);
        if (methodInfo != null)
            harmony.Patch(methodInfo, null, null, null, new HarmonyMethod(fixerType, finalizerName), null);
    }

    public static void PatchMethodWithPrefix(Harmony harmony, Type fixerType, string fullTypeName, string methodName,
        string prefixName)
    {
        var type = Type.GetType(fullTypeName);
        var assembly = type?.Assembly;
        if (assembly == null) return;

        var shortName = fullTypeName.Split(',')[0].Trim();
        var type2 = assembly.GetType(shortName);
        if (type2 == null) return;

        var methodInfo = AccessTools.Method(type2, methodName);
        if (methodInfo != null)
            harmony.Patch(methodInfo, prefix: new HarmonyMethod(fixerType, prefixName));
    }

    public static Exception SuppressNre(Exception ex, string methodName)
    {
        if (ex == null) return null;
        if (ex is not NullReferenceException) return ex;
        Plugin.Logger.LogWarning($"Suppressed NRE in {methodName}: {ex.Message}");
        return null;
    }
}
