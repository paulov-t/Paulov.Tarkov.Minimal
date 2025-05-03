using System;
using EFT;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Paulov.Tarkov.Minimal;

public sealed class BattlEyePatch : IPaulovHarmonyPatch
{
    public MethodBase GetMethodToPatch()
    {
        const string methodName = "RunValidation";
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;


        Type classType = Assembly.GetAssembly(typeof(AbstractGame)).GetTypes()
            .Single(x => x.GetMethod(methodName, flags) != null);
        MethodInfo method = classType.GetMethod(methodName, flags);

        if (method is null) throw new MissingMethodException(classType.FullName, methodName);
        
        Plugin.Logger.LogDebug($"{nameof(BattlEyePatch)}.{nameof(GetMethodToPatch)}:{method.Name}");

        return method;
    }

    public HarmonyMethod GetPrefixMethod()
    {
        return new HarmonyMethod(this.GetType().GetMethod(nameof(PrefixOverrideMethod), BindingFlags.Public | BindingFlags.Static));
    }

    public static bool PrefixOverrideMethod(ref Task __result, ref bool ___bool_0)
    {
        ___bool_0 = true;
        __result = Task.CompletedTask;
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
