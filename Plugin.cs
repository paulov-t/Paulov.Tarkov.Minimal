using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Paulov.Bepinex.Framework;

namespace Paulov.Tarkov.Minimal;

[BepInDependency("Paulov.Bepinex.Framework", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin("Paulov.Tarkov.Minimal", "Paulov.Tarkov.Minimal", "2025.1.13")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        
        HarmonyPatchManager hpm = new("Paulov's Minimal Harmony Manager", new MinimalPatchProvider());
        hpm.EnablePatches();
    }
}
