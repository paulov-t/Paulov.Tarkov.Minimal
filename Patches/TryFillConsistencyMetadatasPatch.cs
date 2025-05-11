using FilesChecker;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Paulov.Bepinex.Framework;
using Paulov.Bepinex.Framework.Patches;

namespace Paulov.Tarkov.Minimal.Patches;

public class TryFillConsistencyMetadatasPatch : NullPaulovHarmonyPatch
{
    private static Type GetConsistencyControllerType()
    {
        Type[] fileCheckerTypes = Assembly.GetAssembly(typeof(FilesCheckerFactory)).GetTypes();
        foreach (Type type in fileCheckerTypes)
        {
            Plugin.Logger.LogDebug($"{nameof(TryFillConsistencyMetadatasPatch)}.{nameof(GetConsistencyControllerType)}: {type.Name}");
        }
        return fileCheckerTypes.Single(x => x.Name == "ConsistencyController");
    }

    public override IEnumerable<MethodBase> GetMethodsToPatch()
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo[] controllerMethods = GetConsistencyControllerType().GetMethods(flags);
        MethodInfo method = controllerMethods.Single(x => x.Name == "TryFillConsistencyMetadatas");

        Plugin.Logger.LogDebug($"{nameof(TryFillConsistencyMetadatasPatch)}.{nameof(GetMethodsToPatch)}:{method.Name}");

        yield return method;
    }

    public override HarmonyMethod GetTranspilerMethod()
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
}