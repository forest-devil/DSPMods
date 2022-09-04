using HarmonyLib;
using System;
using System.Linq;

namespace DSPMods.MoreFluids
{
    public static class MoreFluids
    {
        private static int[] _originalIds;

        [HarmonyPostfix, HarmonyPatch(typeof(ItemProto), "InitFluids")]
        public static void ItemProto_InitFluids_Postfix()
        {
            _originalIds = ItemProto.fluids;
            PatchFluidsArray();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VFPreload), "InvokeOnLoadWorkEnded")]
        public static void LDBVFPreloadPostPatchPostfix()
        {
            PatchIsFluidProperties();
        }

        public static void OnSettingsChanged(object sender, EventArgs e)
        {
            PatchFluidsArray();
            PatchIsFluidProperties();
        }

        private static void PatchFluidsArray()
        {
            Plugin.Instance.Logger.LogInfo("Patching ItemProto.fluids array...");
            ItemProto.fluids = _originalIds.AddRangeToArray(ConfigManager.Instance.EnabledFluidIds.ToArray());
            Plugin.Instance.Logger.LogInfo(string.Join(", ", ItemProto.fluids));
        }

        private static void PatchIsFluidProperties()
        {
            Plugin.Instance.Logger.LogInfo("Patching IsFluid property...");
            foreach (var id in ConfigManager.Instance.ManagedFluidIds)
            {
                var itemProto = LDB.items.Select(id);
                if (itemProto != null)
                {
                    var enabled = ConfigManager.Instance.EnabledFluidIds.Contains(id);
                    itemProto.IsFluid = enabled;

                    Plugin.Instance.Logger.LogInfo($"[{itemProto.Name}] IsFluid = {enabled}");
                }
            }
        }
    }
}
