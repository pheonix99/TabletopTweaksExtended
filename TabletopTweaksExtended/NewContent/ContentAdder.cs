using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace TabletopTweaksExtended.NewContent
{
    class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Loading New Content");
                

               

                

               
                Features.PurifierLimitedCures.AddPurifierLimitedCures();

                Feats.ProdigiousTWF.AddProdigiousTWF();

                FighterArmorTraining.ProliferateFramework.AlterAdvancedArmorTraining();
                FighterArmorTraining.PatchAdvancedArmorTraining.PatchAdvancedArmorTrainings();


            }
        }
    }
}
