using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaksExtended.Extensions;
using TabletopTweaksExtended.NewComponents;
using TabletopTweaksExtended.Utilities;

namespace TabletopTweaksExtended.NewContent.FighterArmorTraining
{
    class PatchAdvancedArmorTraining
    {
        public static void PatchAdvancedArmorTrainings()
        {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            //var Confidence1 = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceLightEffect");
            //var Confidence2 = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceMediumEffect");
            //var Confidence3 = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceHeavyEffect");
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceLightEffect"));
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceMediumEffect"));
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmoredConfidenceHeavyEffect"));

            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorSpecializationLightEffect"));
           
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorSpecializationMediumEffect"));
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("ArmorSpecializationHeavyEffect"));
            PatchContextRankConfig(Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("CriticalDeflectionEffect"));
            var ArmoredJuggernautDR = Resources.GetTabletopTweaksBlueprint<BlueprintUnitProperty>("ArmoredJuggernautDRProperty");
            ArmoredJuggernautDR.Components = new BlueprintComponent[] { };
            ArmoredJuggernautDR.AddComponent(Helpers.Create<ScalingArmoredJuggernautDRProperty>());

            void PatchContextRankConfig(BlueprintFeature target)
            {
                ContextRankConfig scaling = target.Components.OfType<ContextRankConfig>().FirstOrDefault(x => x.m_BaseValueType == ContextRankBaseValueType.ClassLevel && x.m_Class.Contains(FighterClass.ToReference<BlueprintCharacterClassReference>()));
                if (scaling == null)
                {
                    Main.Error($"{target.Name} lacks fighter class level comp, cannot proceeded");
                }
                else
                {
                    scaling.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    scaling.m_CustomProperty = Resources.GetTabletopTweaksBlueprint<BlueprintUnitProperty>("ArmorTrainingProgressionProperty").ToReference<BlueprintUnitPropertyReference>();
                    scaling.m_Class = new BlueprintCharacterClassReference[] { };
                }
            }
        }

    }
}
