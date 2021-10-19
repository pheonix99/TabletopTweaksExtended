using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaksExtended.Config;
using TabletopTweaksExtended.Extensions;
using TabletopTweaksExtended.Utilities;

namespace TabletopTweaksExtended.Bugfixes.Classes
{
    static class Oracle
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Purifier Resources");

                PatchPurifier();
            }
            static void PatchPurifier()
            {
                var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");




                PatchLevel3Revelation();
                PatchRestoreCure();
                void PatchLevel3Revelation()
                {

                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsDisabled("Level3Revelation")) { return; }

                    //var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                    LevelEntry target = PuriferArchetype.RemoveFeatures.FirstOrDefault(x => x.Level == 3);
                    if (target != null)
                    {
                        PuriferArchetype.RemoveFeatures = PuriferArchetype.RemoveFeatures.RemoveFromArray(target);
                    }
                    
                    Main.LogPatch("Patched", PuriferArchetype);
                }

                void PatchRestoreCure()
                {

                    if (ModSettings.Fixes.Oracle.Archetypes["Purifier"].IsDisabled("RestoreEarlyCure")) { return; }

                    var earlycure = Resources.GetTabletopTweaksBlueprint<BlueprintFeature>("PurifierLimitedCures");
                    
                    LevelEntry l = PuriferArchetype.AddFeatures.FirstOrDefault(x => x.Level == 1);
                    if (l == null)
                    {
                        l = new LevelEntry
                        {
                            Level = 1,
                            Features = { earlycure }
                        };

                       
                        PuriferArchetype.AddFeatures = PuriferArchetype.AddFeatures.AddToArray(l);
                    }
                    else
                    {
                        
                        l.Features.Add(earlycure);//Doubling up here won't hurt
                    }
                    Main.LogPatch("Patched", earlycure);
                }

            }

        }
    }
}
