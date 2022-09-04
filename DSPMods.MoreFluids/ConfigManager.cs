using BepInEx.Configuration;
using System;
using System.Collections.Generic;

namespace DSPMods.MoreFluids
{
    public class ConfigManager
    {
        public static ConfigManager Instance { get; private set; }

        public readonly int[] ManagedFluidIds = new[] { 1141, 1142, 1143, 1122, 1208 };
        public readonly IList<int> EnabledFluidIds = new List<int>();
        private readonly Dictionary<int, ConfigEntry<bool>> _configEntries = new Dictionary<int, ConfigEntry<bool>>();
        public ConfigManager(ConfigFile config)
        {
            if (Instance != null && Instance != this)
            {
                return;
            }
            else
            {
                Instance = this;
            }

            foreach (var id in ManagedFluidIds)
            {
                var itemProto = LDB.items.Select(id);
                if (itemProto != null)
                {
                    var entry = config.Bind("FluidIds", id.ToString(), true, itemProto.Name.Translate());
                    if (entry != null)
                    {
                        _configEntries[id] = entry;
                        if (entry.Value)
                        {
                            EnabledFluidIds.Add(id);
                        }
                    }
                }
            }
            Plugin.Instance.Logger.LogInfo("Configuration loaded...");

            config.SettingChanged += OnSettingsChanged;
        }

        public void OnSettingsChanged(object sender, EventArgs e)
        {
            Plugin.Instance.Logger.LogInfo("Settings changed...");
            EnabledFluidIds.Clear();
            foreach (var id in ManagedFluidIds)
            {
                var itemProto = LDB.items.Select(id);
                if (itemProto != null)
                {
                    if (_configEntries.TryGetValue(itemProto.ID, out var entry) && entry.Value)
                        EnabledFluidIds.Add(id);
                }
            }
        }
    }
}
