using RimWorld;
using Verse;

namespace CropQuality
{
    [DefOf]
    public static class DefOf_CropQuality
    {
        public static ThoughtDef CQ_AteAwfulFood;
        public static ThoughtDef CQ_AtePoorFood;
        public static ThoughtDef CQ_AteGoodFood;
        public static ThoughtDef CQ_AteExcellentFood;

        public static InspirationDef CQ_Inspired_Harvesting;

        static DefOf_CropQuality()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_CropQuality));
        }
    }
}
