using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using JobMarketTweaker.Config;


namespace JobMarketTweaker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    internal class JobMarketTweaker : BaseUnityPlugin
    {
        public const string PluginGuid = "me.Aerin.MGT2mod.JobMarketTweaker";
        public const string PluginName = "Job Market Tweaker";
        public const string PluginVersion = "1.0.0.0";
        public static Harmony harmony = new Harmony(PluginGuid);

        void Awake()
        {
            ConfigManager configManager = new ConfigManager(Config);
            LoadHooks();
        }

        void LoadHooks()
        {
            Logger.LogInfo(nameof(LoadHooks));
            harmony.PatchAll();
        }
    }
}
