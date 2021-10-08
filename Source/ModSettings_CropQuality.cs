using Verse;

namespace CropQuality
{
    public class ModSettings_CropQuality : ModSettings
    {
        public static bool preferRaw = false;
        public static float lavishEfficiency = 1f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref preferRaw, "preferRaw", false);
            Scribe_Values.Look(ref lavishEfficiency, "lavishEfficiency", 1);
            //Scribe_Values.Look(ref lavishWaste, "lavishWaste", 1);
            base.ExposeData();
        }
    }
}
