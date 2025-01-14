using EFT;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Paulov.Tarkov.Minimal
{
    public class BattlEyePatch : IPaulovHarmonyPatch
    {
        public MethodInfo GetMethodToPatch()
        {
            var methodName = "RunValidation";
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var method = Assembly.GetAssembly(typeof(AbstractGame)).GetTypes().Single(x => x.GetMethod(methodName, flags) != null)
                .GetMethod(methodName, flags);

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
}
