using Verse;

namespace CropQuality
{
    public class ModSettings_CropQuality : ModSettings
    {
        public static bool preferRaw = false;
        public static float lavishEfficiency = 1f;

        public static bool mealQuality = false;
        public static int minMealQuality = 0;
        public static int maxMealQuality = 6;

        public static float minSimpleQuality = 0f;
        public static float minFineQuality = 3f;
        public static float minLavishQuality = 4f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref preferRaw, "preferRaw", false);
            Scribe_Values.Look(ref lavishEfficiency, "lavishEfficiency", 1);

            Scribe_Values.Look(ref mealQuality, "mealQuality", true);
            Scribe_Values.Look(ref minMealQuality, "minMealQuality", 0);
            Scribe_Values.Look(ref maxMealQuality, "maxMealQuality", 6);

            Scribe_Values.Look(ref minSimpleQuality, "minSimpleQuality", 0f);
            Scribe_Values.Look(ref minFineQuality, "minFineQuality", 3f);
            Scribe_Values.Look(ref minLavishQuality, "minLavishQuality", 4f);
            base.ExposeData();
        }
    }
}
