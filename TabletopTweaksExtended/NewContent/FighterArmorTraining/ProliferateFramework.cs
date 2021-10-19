using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaksExtended.Extensions;
using TabletopTweaksExtended.MechanicsChanges;
using TabletopTweaksExtended.NewComponents;
using TabletopTweaksExtended.NewComponents.Prerequisites;
using TabletopTweaksExtended.Utilities;

namespace TabletopTweaksExtended.NewContent.FighterArmorTraining
{
    class ProliferateFramework
    {

        public static void AlterAdvancedArmorTraining()
        {
            ArmorTrainingProgression();
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorProgression = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag");

            var AdvancedArmorTrainingSelection = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>("AdvancedArmorTrainingSelection");
            //TODO ADD REMOVER
            


            void ModifyAdvancedArmorTrainingSelection(BlueprintFeatureSelection selection)
            {
                PrerequisiteClassLevel levelComp = selection.Components.OfType<PrerequisiteClassLevel>().FirstOrDefault(x => x.CharacterClass.ToReference<BlueprintCharacterClassReference>().Equals( FighterClass.ToReference<BlueprintCharacterClassReference>()));
                if (levelComp == null)
                {
                    Main.Error($"{selection.Name} lacks fighter class level comp, cannot proceeded");

                }
                else
                {
                    selection.AddPrerequisite<PrerequisitePsuedoProgressionRank>(p =>
                    {
                        p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                        p.Level = levelComp.Level;
                    });
                    selection.RemoveComponent(levelComp);
                }
            }

            ModifyAdvancedArmorTrainingSelection(AdvancedArmorTrainingSelection);
            for (int i = 1; i <=6;i++)
            {
                BlueprintFeatureSelection target = Resources.GetTabletopTweaksBlueprint<BlueprintFeatureSelection>($"AdvancedArmorTraining{i}");
                ModifyAdvancedArmorTrainingSelection(target);
            }

            var ArmorSpeedFlag = Helpers.CreateBlueprint<BlueprintFeature>("ArmorTrainingSpeedHiddenFlag", bp =>
            {
                bp.Ranks = 2;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideInUI = true;
                bp.SetName("Armor Speed Up");
                //bp.AddComponent(Helpers.Create<ArmorSpeedPenaltyRemoval>());

            });



        }



        static void ArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("ArmorTrainingFlag", bp =>
            {
                bp.Ranks = 1;
                bp.SetName("Armor Training");
                bp.HideInCharacterSheetAndLevelUp = true;
            });
            BlueprintUnitProperty armorprop = Helpers.CreateBlueprint<BlueprintUnitProperty>("ArmorTrainingProgressionProperty", x =>
            {

                x.AddComponent(Helpers.Create<PseudoProgressionRankGetter>(y =>
                {

                    y.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();

                }));
            });

            FighterArmorTrainingProgression();
            HellknightArmorTrainingProgress();
            PurifierArmorTrainingProgression();
            ArmoredBattlemageArmorTrainingProgression();
            SteelbloodArmorTrainingProgression();
            HellknightSignifierArmorTrainingProgress();
        }

        static void SteelbloodArmorTrainingProgression()
        {
            var Steelblood = Resources.GetBlueprint<BlueprintArchetype>("32a5dff92373a9641b43e97d453b9369");
            Helpers.CreateBlueprint<BlueprintFeature>("SteelbloodArmorTrainingProgression", x =>
            {

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Steelblood.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.scalar = -2; //Steelblood progression starts two levels late
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Steelblood Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Steeblood level minus two, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });


        }

        static void PurifierArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("CelestialArmorMastery", c =>
            {
                var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                var CelestialArmor = Resources.GetBlueprint<BlueprintFeature>("7dc8d7dede2704640956f7bc4102760a");
                c.SetName("Celestial Armor Training Progression");
                c.SetDescription("Increases your armor training rank by your oracle level minus four, progressing Advanced Armor Training abilities.");
                c.IsClassFeature = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.Ranks = 1;
                c.m_Icon = CelestialArmor.Icon;

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.scalar = -4;
                });


                c.AddComponent(progression);

            });
        }

        static void ArmoredBattlemageArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression", x =>
            {
                var ArmoredBattlemageArchetype = Resources.GetBlueprint<BlueprintArchetype>("67ec8dcae6fb3d3439e5ae874ddc7b9b");
                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.multiplier = 0.8;//Armored battlemage tiers up every five levels, not every four like fighter
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Armored Battlemage Armor Training Progression");
                x.SetDescription("Increases your armor training rank by four-fiifths of your Armored Battlemage level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void HellknightArmorTrainingProgress()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("HellknightArmorTrainingProgression", x =>
            {
                BlueprintCharacterClass Hellknight = Resources.GetBlueprint<BlueprintCharacterClass>("ed246f1680e667b47b7427d51e651059");

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Hellknight.ToReference<BlueprintCharacterClassReference>();
                    //Leaving standard progression/scaling because not giving faster-than-fighter like scaling based on their progession would dictate
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Hellknight Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Hellknight level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void HellknightSignifierArmorTrainingProgress()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("HellknightSigniferArmorTrainingProgression", x =>
            {
                BlueprintCharacterClass Signifier = Resources.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Signifier.ToReference<BlueprintCharacterClassReference>();
                    //Leaving standard progression/scaling because it's a gish build PRC and those are stuck competing with Magus, Warpriest and Battle Oracle. It should hardly be a balance issue if it gets full scaling here
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Hellknight Signifier Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Hellknight Signifier level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void FighterArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("FighterArmorTrainingProgression", x =>
            {

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd").ToReference<BlueprintCharacterClassReference>();
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your fighter level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });

        }

    }

}

