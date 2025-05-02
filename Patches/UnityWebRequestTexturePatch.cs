using HarmonyLib;
using System;
using System.Reflection;
using Paulov.Tarkov.Minimal.Models;
using UnityEngine.Networking;

namespace Paulov.Tarkov.Minimal.Patches
{
    public sealed class UnityWebRequestTexturePatch : IPaulovHarmonyPatch
    {
        public MethodBase GetMethodToPatch()
        {
            const string methodName = nameof(UnityWebRequestTexture.GetTexture);
            
            Type classType = typeof(UnityWebRequestTexture);
            MethodInfo method = classType.GetMethod(methodName, [typeof(string)]);

            if(method is null) throw new MissingMethodException(classType.FullName, methodName);
            
            Plugin.Logger.LogDebug($"{nameof(UnityWebRequestTexturePatch)}.{nameof(GetMethodToPatch)}:{method.Name}");

            return method;
        }

        public HarmonyMethod GetPrefixMethod()
        {
            return null;
        }

        public HarmonyMethod GetPostfixMethod()
        {
            return new HarmonyMethod(this.GetType().GetMethod(nameof(PostfixOverrideMethod), BindingFlags.Public | BindingFlags.Static));
        }

        public static void PostfixOverrideMethod(UnityWebRequest __result)
        {
            __result.certificateHandler = new FakeCertificateHandler();
            __result.disposeCertificateHandlerOnDispose = true;
            __result.timeout = 15000;
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
}
