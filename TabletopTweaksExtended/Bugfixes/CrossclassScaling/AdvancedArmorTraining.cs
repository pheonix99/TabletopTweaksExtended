using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaksExtended.Config;
using TabletopTweaksExtended.Extensions;

namespace TabletopTweaksExtended.Bugfixes.CrossclassScaling
{
    class AdvancedArmorTraining
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                Main.LogHeader("Patching Fighter");

                PatchFighter();
                PatchSteelblood();
                PatchHellknight();
                PatchHellknightSignifier();
                PatchArmoredBattlemage();
                PatchPurifier();
            }

            static void PatchFighter()
            {
                var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
                var FighterArmorProgression = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("FighterArmorTrainingProgression");
                var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");
                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                var BaseProgression = FighterClass.Progression;
                BaseProgression.LevelEntries.First(x => x.Level == 1).m_Features.Add(FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>());
                foreach (BlueprintArchetype Archetype in FighterClass.Archetypes)
                {
                    if (Archetype.RemoveFeatures.Count(x => x.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()) || x.m_Features.Contains(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>())) >= 4)
                    {
                        LevelEntry first = Archetype.RemoveFeatures.FirstOrDefault(x => x.Level == 1);
                        if (first == null)
                        {
                            Archetype.RemoveFeatures.AddItem(new LevelEntry
                            {
                                Level = 1,
                                m_Features = new System.Collections.Generic.List<BlueprintFeatureBaseReference>
                                    {
                                        FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>(),


                                    }
                            });
                        }
                        else
                        {
                            first.m_Features.AddItem(FighterArmorProgression.ToReference<BlueprintFeatureBaseReference>());
                        }
                    }
                }
            }

            static void PatchSteelblood()
            {

                var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");
                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                var Steelblood = Resources.GetBlueprint<BlueprintArchetype>("32a5dff92373a9641b43e97d453b9369");
                Steelblood.AddFeatures.First(x => x.Level == 1).m_Features.Add(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("SteelbloodArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());

                var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                foreach (LevelEntry i in Steelblood.AddFeatures.Where(x => x.Features.Contains(ArmorTraining) && x.Level > 5))
                {
                    if (i.Level > 5)
                    {
                        i.Features.Remove(ArmorTraining);
                        i.Features.Add(ArmorTrainingSelection);
                    }
                    i.Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                }

            }

            static void PatchHellknight()
            {
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                BlueprintCharacterClass Hellknight = Resources.GetBlueprint<BlueprintCharacterClass>("ed246f1680e667b47b7427d51e651059");
                var BaseProgression = Hellknight.Progression;
                LevelEntry level1 = BaseProgression.LevelEntries.FirstOrDefault(x => x.Level == 1);
                level1.m_Features.Add(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("HellknightArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());

                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");

                BaseProgression.LevelEntries
                    .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                    .ForEach(entry =>
                    {
                        entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                        entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                        entry.m_Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    });

                Main.LogPatch("Patched", BaseProgression);

            }

            static void PatchHellknightSignifier()
            {
                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                BlueprintCharacterClass Signifier = Resources.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");
                var BaseProgression = Signifier.Progression;
                LevelEntry level1 = BaseProgression.LevelEntries.FirstOrDefault(x => x.Level == 1);
                level1.m_Features.Add(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("HellknightSigniferArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());

                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");


                BaseProgression.LevelEntries
                    .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                    .ForEach(entry =>
                    {
                        entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                        entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                        entry.m_Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    });

                Main.LogPatch("Patched", BaseProgression);

            }
            static void PatchArmoredBattlemage()
            {
                
                    var ArmoredBattlemageArchetype = Resources.GetBlueprint<BlueprintArchetype>("67ec8dcae6fb3d3439e5ae874ddc7b9b");
                    var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");
                    if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }

                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 1).m_Features.Add(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression").ToReference<BlueprintFeatureBaseReference>());
                    var ArmoredBattlemageArmorTraining = Resources.GetBlueprint<BlueprintFeature>("7be523d531bb17449bdba98df0e197ff");

                    var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                    var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");

                    ArmoredBattlemageArmorTraining.RemoveComponents<AddFacts>(x => true);//wipes all the armor trainings - couldn't find a syntax that's more specific that would boot, sorry

                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 8).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 13).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 18).Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());

                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 3).Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 8).Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 13).Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    ArmoredBattlemageArchetype.AddFeatures.First(x => x.Level == 18).Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());

                    ArmoredBattlemageArmorTraining.AddComponent<AddFeatureOnClassLevel>(x => {
                        x.Level = 3;
                        x.m_Class = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                        x.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                    });


                    Main.LogPatch("Patched", ArmoredBattlemageArmorTraining);
                
            }
            static void PatchPurifier()
            {

                if (ModSettings.Fixes.Fighter.Base.IsDisabled("AdvancedArmorTraining")) { return; }
                var CelestialArmor = Resources.GetBlueprint<BlueprintFeature>("7dc8d7dede2704640956f7bc4102760a");
                var CelestialArmorMastery = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("CelestialArmorMastery");
                var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
                var ArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection");
                var ArmorTrainingSpeedFeature = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingSpeedFeature");
                var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                CelestialArmor.RemoveComponents<AddFacts>(x => true);//This is ugly but I can't get a conditonal to work here

                CelestialArmor.AddComponent<AddFeatureOnClassLevel>(x => {
                    x.Level = 7;
                    x.m_Class = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = CelestialArmorMastery.ToReference<BlueprintFeatureReference>();

                });
                CelestialArmor.AddComponent<AddFeatureOnClassLevel>(x => {
                    x.Level = 7;
                    x.m_Class = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();



                });
                PuriferArchetype.AddFeatures.First(x => x.Level == 7).Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());

                void AddSelectionToLevel(int level)
                {
                    LevelEntry l = PuriferArchetype.AddFeatures.FirstOrDefault(x => x.Level == level);
                    if (l == null)
                    {
                        l = new LevelEntry
                        {
                            Level = level



                        };
                        l.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                        l.m_Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                        PuriferArchetype.AddFeatures.AddItem(l);

                    }
                    else
                    {
                        l.Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                        l.Features.Add(ArmorTrainingSpeedFeature.ToReference<BlueprintFeatureBaseReference>());
                    }
                }

                AddSelectionToLevel(11);
                AddSelectionToLevel(15);
                AddSelectionToLevel(19);

                Main.LogPatch("Patched", CelestialArmor);
            }
        }
    }
}
