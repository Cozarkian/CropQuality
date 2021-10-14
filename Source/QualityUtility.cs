using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;

namespace CropQuality
{
    [HarmonyPatch]
    public class QualityUtility
    {
        static bool Prepare(MethodInfo original)
        {
            return !ModLister.HasActiveModWithName("Quality Framework");
        }

        [HarmonyPatch(typeof(QuestManager), "Notify_PlantHarvested")]
        [HarmonyPrefix]
        public static void HarvestQuality(Pawn worker, Thing harvested)
        {
            CompQuality compQuality = harvested.TryGetComp<CompQuality>();
            if (compQuality != null && worker.skills.GetSkill(SkillDefOf.Plants) != null)
            {
                int harvestSkill = worker.skills.GetSkill(SkillDefOf.Plants).Level;
                //bool inspired = worker.InspirationDef == DefOf_CropQuality.CQ_Inspired_Harvesting;
                QualityCategory quality = RimWorld.QualityUtility.GenerateQualityCreatedByPawn(harvestSkill, false);
                if (quality < QualityCategory.Normal && harvested.def.ingestible.preferability < FoodPreferability.RawTasty)
                    quality = QualityCategory.Normal;
                if (quality > QualityCategory.Excellent)
                    quality = QualityCategory.Excellent;
                compQuality.SetQuality(quality, ArtGenerationContext.Colony);
            }
        }   
        
        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyPostfix]
        public static void MeatQuality(Thing product)
        {
            //Log.Message("Meat Quality");
            if (product.def.IsMeat)
            {
                CompQuality comp = product.TryGetComp<CompQuality>();
                if (comp != null)
                {
                    QualityCategory quality = comp.Quality;
                    if (quality < QualityCategory.Normal)
                        quality = QualityCategory.Normal;
                    if (quality > QualityCategory.Excellent)
                        quality = QualityCategory.Excellent;
                    comp.SetQuality(quality, ArtGenerationContext.Colony);
                } 
            }
        }

        [HarmonyPatch(typeof(ThingSetMakerUtility), "AssignQuality")]
        [HarmonyPostfix]
        public static void AssignFoodQuality(Thing thing)
        {
            if (thing.def.ingestible != null && (thing.def.ingestible.preferability == FoodPreferability.RawTasty || thing.def.ingestible.preferability == FoodPreferability.RawBad))
            {
                CompQuality compQuality = thing.TryGetComp<CompQuality>();
                if (compQuality != null)
                {
                    QualityCategory quality = compQuality.Quality;
                    if (quality < QualityCategory.Normal)
                        quality = QualityCategory.Normal;
                    if (quality > QualityCategory.Excellent)
                        quality = QualityCategory.Excellent;
                    compQuality.SetQuality(quality, ArtGenerationContext.Outsider);
                }
            }
        }
    }
}
