using HarmonyLib;
using System.Reflection;

namespace Paulov.Tarkov.Minimal
{
    public interface IPaulovHarmonyPatch
    {
        MethodBase GetMethodToPatch();
        HarmonyMethod GetPrefixMethod();
        HarmonyMethod GetPostfixMethod();
        HarmonyMethod GetTranspilerMethod();
        HarmonyMethod GetFinalizerMethod();
        HarmonyMethod GetILManipulatorMethod();
    }
}
