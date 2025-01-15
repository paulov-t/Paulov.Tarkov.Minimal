using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paulov.Tarkov.Minimal
{
    public class HarmonyPatchManager
    {
        private readonly ManualLogSource logger;
        private List<Harmony> harmonyList;

        public HarmonyPatchManager(in string managerName)
        {
            harmonyList = new List<Harmony>();
            logger = BepInEx.Logging.Logger.CreateLogSource(managerName ?? GetType().Name);
        }

        public void EnablePatches()
        {
            foreach (var patch in this.GetType().Assembly.GetTypes()
               .Where(x => x.GetInterface(nameof(IPaulovHarmonyPatch)) != null)
               .Where(x => !x.IsAbstract)
               .OrderBy(t => t.Name).ToArray())
            {
                try
                {
                    var harmony = new Harmony(patch.Name);
                    var obj = Activator.CreateInstance(patch) as IPaulovHarmonyPatch;
                    if (obj.GetMethodToPatch() == null)
                        continue;

                    harmony.Patch(obj.GetMethodToPatch(), obj.GetPrefixMethod(), obj.GetPostfixMethod(), obj.GetTranspilerMethod(), obj.GetFinalizerMethod(), obj.GetILManipulatorMethod());
                    harmonyList.Add(harmony);
                }
                catch (Exception e)
                {
                    logger.LogError(e);
                }
            }
        }

        public void DisablePatches()
        {
            foreach (var harmony in harmonyList)
                harmony.UnpatchSelf();
        }

    }
}
