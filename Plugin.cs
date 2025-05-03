using BepInEx;
using BepInEx.Logging;

namespace Paulov.Tarkov.Minimal;

[BepInPlugin("Paulov.Tarkov.Minimal", "Paulov.Tarkov.Minimal", "2025.1.13")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        // Create HarmonyPatchManager and Enable the Patches
        var hpm = new HarmonyPatchManager("Paulov's Main Harmony Manager");
        hpm.EnablePatches();
    }
}
