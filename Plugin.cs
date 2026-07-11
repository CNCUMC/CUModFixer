using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CUModFixer;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.cncumc.cumodfixer";
    public const string Name = "CUModFixer";
    public const string Version = "1.0.2";
    internal new static ManualLogSource Logger;

    public void Awake()
    {
        Logger = base.Logger;
        Harmony.CreateAndPatchAll(typeof(Plugin).Assembly);
    }
}