using FilesChecker;
using HarmonyLib;
using Paulov.Tarkov.Minimal;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Paulov.Tarkov.Minimal;

public class EnsureConsistencyPatch : IPaulovHarmonyPatch
{
    public MethodInfo GetMethodToPatch()
    {
        var method = Assembly.GetAssembly(typeof(FilesCheckerFactory)).GetTypes().Single(x => x.Name == "ConsistencyController")
            .GetMethods().Single(x => x.Name == "EnsureConsistency" && x.ReturnType == typeof(Task<ICheckResult>));

        Plugin.Logger.LogDebug($"{nameof(EnsureConsistencyPatch)}.{nameof(GetMethodToPatch)}:{method.Name}");

        return method;
    }

    public HarmonyMethod GetPrefixMethod()
    {
        return new HarmonyMethod(this.GetType().GetMethod(nameof(PrefixOverrideMethod), BindingFlags.Public | BindingFlags.Static));
    }

    public static bool PrefixOverrideMethod(ref object __result)
    {
        __result = Task.FromResult<ICheckResult>(new FileCheckerResult());
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

    private class FileCheckerResult : ICheckResult
    {
        public TimeSpan ElapsedTime => TimeSpan.Zero;

        public Exception Exception => null;
    }
}
