using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
