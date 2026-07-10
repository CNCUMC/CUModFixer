using System;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CUModFixer;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.cncumc.cumodfixer";
    public const string Name = "CUMod Fixer";
    public const string Version = "1.0.0";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);
    private static Harmony _staticHarmony;  

    public void Awake()
    {
        Logger = base.Logger;
        _harmony.PatchAll();
        _staticHarmony = _harmony;
        
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        
        var newFirearmsAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "NewFirearms");
        if (newFirearmsAssembly != null)
        {
            MpScareCheckGuard.Install(_staticHarmony);
        }
    }
    
    private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
    {
        try
        {
            if (args.LoadedAssembly.GetName().Name != "NewFirearms") return;
            MpScareCheckGuard.Install(_staticHarmony);
        }
        catch
        {
            // ignored
        }
    }

    internal class MpScareCheckGuard
    {
        private static bool _installed;
        
        internal static void Install(Harmony harmony)
        {
            if (_installed) return;
            
            var rshGunType = Type.GetType("NewFirearms.RshGun, NewFirearms");
            if (rshGunType == null) return;
            var method = AccessTools.Method(rshGunType, "MpScareCheck");
            if (method == null) return;
            
            harmony.Patch(method,
                prefix: new HarmonyMethod(typeof(MpScareCheckGuard), nameof(Prefix)),
                finalizer: new HarmonyMethod(typeof(MpScareCheckGuard), nameof(Finalizer)));
            
            _installed = true;
            Logger.LogInfo("NewFirearms MpScareCheck guard installed.");
        }

        public static bool Prefix(object __instance)
        {
            try
            {
                var rshGunType = Type.GetType("NewFirearms.RshGun, NewFirearms");
                if (rshGunType == null) return false;
                
                var gameObjectProperty = rshGunType.GetProperty("gameObject");
                if (gameObjectProperty == null) return false;
                
                var gameObject = gameObjectProperty.GetValue(__instance) as UnityEngine.GameObject;
                if (gameObject == null) return false;
                
                var trackerType = Type.GetType("KrokoshaCasualtiesMP.KrokoshaScavMultiGameObjectNetworkTracker, KrokoshaCasualtiesMP");
                if (trackerType == null) return false;
                
                var component = gameObject.GetComponent(trackerType);
                return component != null;
            }
            catch
            {
                return false;
            }
        }

        public static Exception Finalizer(Exception __exception)
        {
            if (__exception == null) return null;
            Logger.LogWarning($"Suppressed NewFirearms MpScareCheck exception: {__exception.Message}");
            return null;
        }
    }
}