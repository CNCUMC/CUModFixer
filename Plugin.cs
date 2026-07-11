using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CUModFixer;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.cncumc.cumodfixer";
    public const string Name = "CUModFixer";
    public const string Version = "1.0.1";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);
    private static Harmony _staticHarmony;  

    public void Awake()
    {
        Logger = base.Logger;
        _harmony.PatchAll();
        _staticHarmony = _harmony;
        
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        
        Fixers.NewFirearmsFix.Install(_staticHarmony);
        Fixers.NewClothingFix.Install(_staticHarmony);
        Fixers.KrokoshaCasualtiesMPFix.Install(_staticHarmony);
    }
    
    private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
    {
        try
        {
            switch (args.LoadedAssembly.GetName().Name)
            {
                case "NewFirearms":
                    Fixers.NewFirearmsFix.Install(_staticHarmony);
                    break;
                case "NewClothing":
                    Fixers.NewClothingFix.Install(_staticHarmony);
                    break;
            }
        }
        catch
        {
            // ignored
        }
    }
}