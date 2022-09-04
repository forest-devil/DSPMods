using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DSPMods.MoreFluids
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.FRIENDLY_NAME, PluginInfo.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        protected Harmony harmony;
        public new ManualLogSource Logger;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            this.Logger = base.Logger;
            var configMgr = new ConfigManager(Config);
            Config.SettingChanged += MoreFluids.OnSettingsChanged;

            harmony = new Harmony(PluginInfo.GUID);
            harmony.PatchAll(typeof(MoreFluids));
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
        }
    }
}
