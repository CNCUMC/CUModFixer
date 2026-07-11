using System;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using HarmonyLib;

namespace CUModFixer.Fixers;

[BepInDependency("com.alexx_.buildmod", BepInDependency.DependencyFlags.SoftDependency)]
internal static class BuildModFix
{
    private static bool _initialized;
    private static Assembly _stubAssembly;

    [HarmonyPrepare]
    public static bool Prepare()
    {
        if (_initialized) return false;
        _initialized = true;

        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        return false; // no Harmony patch needed
    }

    private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var name = new AssemblyName(args.Name);
        if (name.Name != "KrokoshaCasualtiesMP") return null;

        if (_stubAssembly != null) return _stubAssembly;

        // Create a minimal stub assembly so JIT compilation doesn't throw
        var an = new AssemblyName("KrokoshaCasualtiesMP");
        var ab = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
        var mb = ab.DefineDynamicModule("MainModule");

        // Define stub types that BuildModeMod references
        var stubAttrs = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract;

        var multiType = mb.DefineType("KrokoshaCasualtiesMP.KrokoshaScavMultiplayer", stubAttrs);
        multiType.DefineField("is_server", typeof(bool), FieldAttributes.Public | FieldAttributes.Static);
        multiType.DefineField("is_client", typeof(bool), FieldAttributes.Public | FieldAttributes.Static);
        multiType.DefineField("network_system_is_running", typeof(bool), FieldAttributes.Public | FieldAttributes.Static);
        multiType.CreateType();

        var netType = mb.DefineType("KrokoshaCasualtiesMP.Net", stubAttrs);
        netType.DefineField("is_server", typeof(bool), FieldAttributes.Public | FieldAttributes.Static);
        netType.DefineField("running", typeof(bool), FieldAttributes.Public | FieldAttributes.Static);
        netType.CreateType();

        var assetsType = mb.DefineType("KrokoshaCasualtiesMP.KrokoshaCoopModAssets", stubAttrs);
        var loadMethod = assetsType.DefineMethod("LoadAssets", MethodAttributes.Public | MethodAttributes.Static);
        loadMethod.GetILGenerator().Emit(OpCodes.Ret);
        assetsType.CreateType();

        _stubAssembly = ab;
        return _stubAssembly;
    }
}