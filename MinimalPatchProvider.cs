using System;
using System.Collections.Generic;
using System.Linq;
using Paulov.Bepinex.Framework;
using Paulov.Bepinex.Framework.Patches;

namespace Paulov.Tarkov.Minimal;

public class MinimalPatchProvider : IPatchProvider
{
    public IEnumerable<IPaulovHarmonyPatch> GetPatches()
    {
        IOrderedEnumerable<Type> assemblyTypes = GetType().Assembly.GetTypes().OrderBy(x => x.Name);
        foreach (Type type in assemblyTypes)
        {
            if(type.GetInterface(nameof(IPaulovHarmonyPatch)) is null) continue;
            yield return (IPaulovHarmonyPatch)Activator.CreateInstance(type);
        }
    }
}