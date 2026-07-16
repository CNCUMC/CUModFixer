using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using CUModFixer.Fixers;
using HarmonyLib;

namespace CUModFixer;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.cncumc.cumodfixer";
    public const string Name = "CUModFixer";
    public const string Version = "1.1.1";
    internal new static ManualLogSource Logger;
    private static Harmony _staticHarmony;
    private readonly Harmony _harmony = new(Guid);

    public void Awake()
    {
        Logger = base.Logger;
        _harmony.PatchAll();
        _staticHarmony = _harmony;

        foreach (var guid in Chainloader.PluginInfos.Values.Select(mods => mods.Metadata.GUID))
            switch (guid)
            {
                case "com.rushellxyz.newclothing":
                    NewClothingFix.Install(_staticHarmony);
                    break;
                case "com.rushellxyz.newfirearms":
                    NewFirearmsFix.Install(_staticHarmony);
                    break;
                case "KrokoshaCasualtiesMP":
                    KrokoshaCasualtiesMPFix.Install(_staticHarmony);
                    break;
            }
    }
}
