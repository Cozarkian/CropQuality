using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;
using HarmonyLib;

namespace CropQuality
{
    [HarmonyPatch(typeof(FoodUtility), "ThoughtsFromIngesting")]
    public class CropThoughts
    {
        public static void Postfix(ref List<FoodUtility.ThoughtFromIngesting> __result, Pawn ingester, Thing foodSource)
        {
            if (ingester.IsCaravanMember())
            {
                return;
            }
            if (ingester.jobs.curJob?.def != JobDefOf.Ingest && !ModSettings_CropQuality.preferRaw )
            {
                return;
            }
            CompQuality compQuality = foodSource.TryGetComp<CompQuality>();
            if (compQuality != null && foodSource.def.ingestible.preferability == FoodPreferability.RawTasty)
            {
                //Log.Message("Getting quality thought");
                ThoughtDef thoughtDef = null;
                switch (compQuality.Quality)
                {
                    case QualityCategory.Awful:
                        thoughtDef = DefOf_CropQuality.CQ_AteAwfulFood;
                        break;
                    case QualityCategory.Poor:
                        thoughtDef = DefOf_CropQuality.CQ_AtePoorFood;
                        break;
                    case QualityCategory.Good:
                        thoughtDef = DefOf_CropQuality.CQ_AteGoodFood;
                        break;
                    case QualityCategory.Excellent:
                        thoughtDef = DefOf_CropQuality.CQ_AteExcellentFood;
                        break;
                    default:
                        break;
                }
                //Log.Message("Gaining thought " + thoughtDef.ToString());
                if (thoughtDef != null)
                {
                    FoodUtility.ThoughtFromIngesting item = new FoodUtility.ThoughtFromIngesting
                    {
                        thought = thoughtDef,
                        fromPrecept = null
                    };
                    __result.Add(item);
                }
            }
        }
    }
}
