using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Networking;

namespace Paulov.Tarkov.Minimal.Patches
{
    public sealed class UnityWebRequestTexturePatch : IPaulovHarmonyPatch
    {
        public MethodBase GetMethodToPatch()
        {
            var method = typeof(UnityWebRequestTexture).GetMethod(nameof(UnityWebRequestTexture.GetTexture), new[] { typeof(string) });

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

        public class FakeCertificateHandler : CertificateHandler
        {
            public override bool ValidateCertificate(byte[] certificateData) => true;
        }
    }
}
