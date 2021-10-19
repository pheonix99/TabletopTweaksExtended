﻿using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaksExtended.Config
{
    public class Fixes : IUpdatableSettings
    {
        public bool NewSettingsOffByDefault = false;

        public ClassGroup Fighter = new ClassGroup();
        public ClassGroup Oracle = new ClassGroup();

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as Fixes;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;

            Fighter.LoadClassGroup(loadedSettings.Fighter, NewSettingsOffByDefault);

            Oracle.LoadClassGroup(loadedSettings.Oracle, NewSettingsOffByDefault);

        }

        public class ClassGroup : IDisableableGroup
        {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Base;
            public SortedDictionary<string, NestedSettingGroup> Archetypes = new SortedDictionary<string, NestedSettingGroup>();

            public ClassGroup()
            {
                Base = new NestedSettingGroup(this);
            }

            public void LoadClassGroup(ClassGroup group, bool frozen)
            {
                DisableAll = group.DisableAll;
                Base.LoadSettingGroup(group.Base, frozen);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key))
                    {
                        Archetypes[entry.Key].LoadSettingGroup(entry.Value, frozen);
                    }
                });
                Archetypes.ForEach(entry => entry.Value.Parent = this);
            }
        }

        public class CrusadeGroup : IDisableableGroup
        {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Buildings;

            public CrusadeGroup()
            {
                Buildings = new NestedSettingGroup(this);
            }

            public void LoadCrusadeGroup(CrusadeGroup group, bool frozen)
            {
                DisableAll = group.DisableAll;
                Buildings.LoadSettingGroup(group.Buildings, frozen);
            }
        }

        public class ItemGroup : IDisableableGroup
        {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Armor;
            public NestedSettingGroup Equipment;
            public NestedSettingGroup Weapons;

            public ItemGroup()
            {
                Armor = new NestedSettingGroup(this);
                Equipment = new NestedSettingGroup(this);
                Weapons = new NestedSettingGroup(this);
            }

            public void LoadItemGroup(ItemGroup group, bool frozen)
            {
                DisableAll = group.DisableAll;
                Armor.LoadSettingGroup(group.Armor, frozen);
                Equipment.LoadSettingGroup(group.Equipment, frozen);
                Weapons.LoadSettingGroup(group.Weapons, frozen);
            }
        }
    }
}
