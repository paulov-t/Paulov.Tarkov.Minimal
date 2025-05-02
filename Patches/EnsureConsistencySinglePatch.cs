using FilesChecker;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Paulov.Tarkov.Minimal.Models;

namespace Paulov.Tarkov.Minimal;

public sealed class EnsureConsistencySinglePatch : IPaulovHarmonyPatch
{
    public MethodBase GetMethodToPatch()
    {

        var method = Assembly.GetAssembly(typeof(FilesCheckerFactory)).GetTypes().Single(x => x.Name == "ConsistencyController")
            .GetMethods().Single(x => x.Name == "EnsureConsistencySingle" && x.ReturnType == typeof(Task<ICheckResult>));

        Plugin.Logger.LogDebug($"{nameof(EnsureConsistencySinglePatch)}.{nameof(GetMethodToPatch)}:{method.Name}");

        return method;
    }

    public HarmonyMethod GetPrefixMethod()
    {
        return new HarmonyMethod(this.GetType().GetMethod(nameof(PrefixOverrideMethod), BindingFlags.Public | BindingFlags.Static));
    }

    public static bool PrefixOverrideMethod(ref object __result)
    {
        __result = Task.FromResult<ICheckResult>(new FakeFileCheckerResult());
        return false;
    }


    public HarmonyMethod GetPostfixMethod()
    {
        return null;
    }

    public HarmonyMethod GetTranspilerMethod()
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
