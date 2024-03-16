using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static JobMarketTweaker.CreateCharacterJobMarketController;

namespace JobMarketTweaker.Config
{
    [BepInPlugin(JobMarketTweaker.PluginGuid, JobMarketTweaker.PluginName, JobMarketTweaker.PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    public class ConfigManager
    {
        private ConfigFile ConfigFile { get; set; }

        /// <summary>
        /// Constructor with LoadConfig
        /// </summary>
        /// <param name="configFile"></param>
        public ConfigManager(ConfigFile configFile)
        {
            ConfigFile = configFile;
            InitializePerkSettings(ConfigFile);
            LoadConfig();
        }

        // =============================================================================================================
        // Config sections
        // =============================================================================================================
        private const string ModSettingsSection = "0. MOD Settings";
        private const string ApplicantsSettingSection = "1. Applicant Settings";
        private const string ApplicantsPerksSettingSection = "2. Applicant Perks Settings";
        private const string ApplicantsPerksTogglingSettingSection = "3. Toggle Applicant Perks";

        // =============================================================================================================
        // Config entries 
        // =============================================================================================================
        public static ConfigEntry<bool> IsModEnabled { get; private set; }

        // Core Config ---------------------------------------------------------------
        //1. MaximumApplicantsCount
        public static ConfigEntry<int> MaximumApplicantsCount { get; private set; }
        //2. AdditionalApplicants
        public static ConfigEntry<int> AdditionalApplicants { get; private set; }
        //3. ExtraAdditionalApplicants
        public static ConfigEntry<int> ExtraAdditionalApplicants { get; private set; }
        //4. IsInSandBoxModeApplied
        public static ConfigEntry<bool> IsInSandBoxModeApplied { get; private set; }

        // Perk Settings ---------------------------------------------------------------
        //1. PerkSelectionChance
        public static ConfigEntry<int> PerkSelectionChance { get; private set; }
        //2. PerkSelectionLoopCount
        public static ConfigEntry<int> PerkSelectionLoopCount { get; private set; }

        //3. MaxPerksCount
        public static ConfigEntry<int> MaxPerksCount { get; private set; }

        // MaxPerkCount

        // Perk Togging Settings ---------------------------------------------------------------
        public static Dictionary<PerkType, ConfigEntry<bool>> PerkEnabledSettings { get; private set; }

        public static void InitializePerkSettings(ConfigFile configFile)
        {
            PerkEnabledSettings = new Dictionary<PerkType, ConfigEntry<bool>>();

            foreach (PerkType perk in Enum.GetValues(typeof(PerkType)))
            {
                // ここでConfigEntryを初期化して、各Perkに対応する設定をDictionaryに追加する
                // この例では、デフォルトで全てのPerkを有効にしている
                ConfigEntry<bool> perkEnabled = configFile.Bind(ApplicantsPerksTogglingSettingSection, perk.ToString(), true, $"Enable or disable the {perk} perk.");
                PerkEnabledSettings.Add(perk, perkEnabled);
            }
            UpdatePerkSettings();
        }

        public static void UpdatePerkSettings()
        {
            // ここでPerkEnabledSettingsを使って、Perkの有効/無効を設定する
            // PerkEnabledSettingsを順にループし、Falseの場合は、CreateCharacterJobMarketController.DisablePerkを呼び出す
            InitLimitedPerksList();
            // PerkEnabledSettingsを順にループし、Falseの場合はDisablePerkを呼び出す
            foreach (var perkSetting in PerkEnabledSettings)
            {
                if (!perkSetting.Value.Value) // 設定がfalseの場合
                {
                    AddDisabledPerk(perkSetting.Key);
                }
                else
                {
                    AddEnabledPerk(perkSetting.Key);
                }
            }
        }

        // =============================================================================================================
        /// <summary>
        /// Loading when the game starts
        /// </summary>
        private void LoadConfig()
        {
            // =============================================================================================================
            // Config setting definitions here
            // =============================================================================================================

            // Main Settings
            IsModEnabled = ConfigFile.Bind(
                ModSettingsSection,
                "Activate the MOD",
                true,
                "Toggle 'Enabled' to activate the mod");

            // ----------------------------------------------------------------------------------------------------------------
            // 1. MaximumApplicantsCount
            MaximumApplicantsCount = ConfigFile.Bind<int>(
                section: ApplicantsSettingSection,
                key: "1. Maximum Applicants Count",
                defaultValue: 30,
                new ConfigDescription("Set the maximum number of applicants in the Job Market. " +
                "[JP]労働市場の応募者の上限値を設定します。 (Vanilla)Default : 30",
                new AcceptableValueRange<int>(0, 9999))  // ここで範囲を指定
            );

            // 2. AdditionalApplicants
            AdditionalApplicants = ConfigFile.Bind<int>(
                section: ApplicantsSettingSection,
                key: "2. Additional Applicants",
                defaultValue: 3,
                new ConfigDescription("Set the additional number of applicants in the Job Market. " +
                "[JP]労働市場の追加応募者数を設定します。 Default : 3, Only used in Offline",
                new AcceptableValueRange<int>(0, 99))  // ここで範囲を指定
            );

            // 3. ExtraAdditionalApplicants
            ExtraAdditionalApplicants = ConfigFile.Bind<int>(
                section: ApplicantsSettingSection,
                key: "3. Extra Additional Applicants",
                defaultValue: 0,
                new ConfigDescription("Set the 'extra' additional number of applicants in the Job Market. " +
                "[JP]労働市場のさらなる追加応募者数を設定します。 Default : 0, Only used in Offline and Sand Box Mode",
                new AcceptableValueRange<int>(0, 99))  // ここで範囲を指定
            );

            // 4. IsInSandBoxModeApplied
            IsInSandBoxModeApplied = ConfigFile.Bind<bool>(
                section: ApplicantsSettingSection,
                key: "4. Apply in Sandbox Mode",
                defaultValue: false,
                new ConfigDescription("If set to 'True', MGT2 will ignore the sandbox settings and change the Job Market settings.　" +
                "[JP]Trueにすると、サンドボックスの設定を無視して、労働市場の設定変更をします。 Default : false")
            );

            // Perk Settings ---------------------------------------------------------------
            // 5. PerkSelectionChance
            PerkSelectionChance = ConfigFile.Bind<int>(
                section: ApplicantsPerksSettingSection,
                key: "Perk Selection Chance",
                defaultValue: 20,
                new ConfigDescription("Set the chance of selecting a perk. " +
                "[JP]Perkを選択する確率を設定します。 Default : 20",
                new AcceptableValueRange<int>(0, 100))  // ここで範囲を指定
            );

            // 6. PerkSelectionLoopCount
            PerkSelectionLoopCount = ConfigFile.Bind<int>(
                section: ApplicantsPerksSettingSection,
                key: "Perk Selection Loop Count",
                defaultValue: 20,
                new ConfigDescription("Set the number of loops for selecting a perk. " +
                "[JP]Perkを選択するループ数を設定します。 Default : 20 " +
                "I'm not entirely sure why, but EggCode Games opted for a 20-loop setting for perk selection. Go figure...",
                new AcceptableValueRange<int>(1, 50))  // ここで範囲を指定
            );

            // 7. MaxPerksCount
            MaxPerksCount = ConfigFile.Bind<int>(
                section: ApplicantsPerksSettingSection,
                key: "Max Perks Count",
                defaultValue: 4,
                new ConfigDescription("Set the maximum number of perks that can be selected. " +
                "[JP]選択できるPerkの最大数を設定します。 Default : 4",
                new AcceptableValueRange<int>(0, 30))  // ここで範囲を指定
            );

            //








            // =============================================================================================================
            // Config setting event handlers here
            ConfigFile.SettingChanged += OnConfigSettingChanged;
            // =============================================================================================================
        }
        /// <summary>
        /// DEBUG: Event handler for config setting changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigSettingChanged(object sender, SettingChangedEventArgs e)
        {
#if DEBUG
            Debug.Log(JobMarketTweaker.PluginName + " : Config setting is changed");
#endif
            UpdatePerkSettings();
        }
    }
}
