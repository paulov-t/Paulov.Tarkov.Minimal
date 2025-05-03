using FilesChecker;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Paulov.Tarkov.Minimal.Patches;

public class TryFillConsistencyMetadatasPatch : IPaulovHarmonyPatch
{
    private static Type GetConsistencyControllerType()
    {
        Type[] fileCheckerTypes = Assembly.GetAssembly(typeof(FilesCheckerFactory)).GetTypes();
        for (var i = 0; i < fileCheckerTypes.Length; i++)
        {
            Plugin.Logger.LogDebug($"{nameof(TryFillConsistencyMetadatasPatch)}.{nameof(GetConsistencyControllerType)}: {fileCheckerTypes[i].Name}");
        }
        return fileCheckerTypes.Single(x => x.Name == "ConsistencyController");
    }

    public MethodBase GetMethodToPatch()
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo[] controllerMethods = GetConsistencyControllerType().GetMethods(flags);
        MethodInfo method = controllerMethods.Single(x => x.Name == "TryFillConsistencyMetadatas");

        Plugin.Logger.LogDebug($"{nameof(TryFillConsistencyMetadatasPatch)}.{nameof(GetMethodToPatch)}:{method.Name}");

        return method;
    }

    public HarmonyMethod GetTranspilerMethod()
    {
        return new HarmonyMethod(this.GetType().GetMethod(nameof(TranspilerMethod), BindingFlags.Public | BindingFlags.Static));
    }

    public static IEnumerable<CodeInstruction> TranspilerMethod()
    {
        //Loads the consistency metadata dictionary onto the stack then calls .Clear() and returns
        List<CodeInstruction> codeInstructions =
        [
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldfld, AccessTools.Field(GetConsistencyControllerType(), "_fileConsistencyMetadatas")),
            new(OpCodes.Callvirt, AccessTools.Method(typeof(Dictionary<string, FileConsistencyMetadata>), nameof(Dictionary<string, FileConsistencyMetadata>.Clear))),
            new(OpCodes.Ret)
        ];

        return codeInstructions;
    }



    public HarmonyMethod GetPrefixMethod()
    {
        return null;
    }

    public HarmonyMethod GetPostfixMethod()
    {
        return null;
    }

    public HarmonyMethod GetFinalizerMethod()
    {
        return null;
    }

    public HarmonyMethod GetILManipulatorMethod()
    {
        return null;
    }
}