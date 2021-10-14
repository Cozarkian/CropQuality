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

        static DefOf_CropQuality()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_CropQuality));
        }
    }
}
