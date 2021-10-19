using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaksExtended.Config
{
    public class AddedContent : IUpdatableSettings
    {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup FighterAdvancedArmorTraining = new SettingGroup();

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as AddedContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;

            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            FighterAdvancedArmorTraining.LoadSettingGroup(loadedSettings.FighterAdvancedArmorTraining, NewSettingsOffByDefault);

        }
    }
}
