using System.Reflection;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace CropQuality
{
    public class Mod_CropQuality : Mod
    {
        Listing_Standard listingStandard = new Listing_Standard();

        public Mod_CropQuality(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings_CropQuality>();
            Harmony harmony = new Harmony("rimworld.cropquality");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory()
        {
            return "Crop Quality";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new Rect(100f, 50f, inRect.width * .8f, inRect.height);
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Pawns consider raw quality when choosing food: ", ref ModSettings_CropQuality.preferRaw);
            listingStandard.Gap();
            string simpleLabel = "*Simple meals require ingredient quality of at least:      " + ((QualityCategory)ModSettings_CropQuality.minSimpleQuality).ToString();
            string simpleBuffer = ModSettings_CropQuality.minSimpleQuality.ToString();
            LabeledFloatEntry(listingStandard.GetRect(24f), simpleLabel, ref ModSettings_CropQuality.minSimpleQuality, ref simpleBuffer, 1f, 1f, 0f, 6f);

            string fineLabel = "*Fine meals require ingredient quality of at least:         " + ((QualityCategory)ModSettings_CropQuality.minFineQuality).ToString();
            string fineBuffer = ModSettings_CropQuality.minFineQuality.ToString();
            LabeledFloatEntry(listingStandard.GetRect(24f), fineLabel, ref ModSettings_CropQuality.minFineQuality, ref fineBuffer, 1f, 1f, 0f, 6f);

            string lavishLabel = "*Lavish meals require ingredient quality of at least:      " + ((QualityCategory)ModSettings_CropQuality.minLavishQuality).ToString();
            string lavishBuffer = ModSettings_CropQuality.minLavishQuality.ToString();
            LabeledFloatEntry(listingStandard.GetRect(24f), lavishLabel, ref ModSettings_CropQuality.minLavishQuality, ref lavishBuffer, 1f, 1f, 0f, 6f);

            string effLabel = "*Lavish meals nutrition multiplier (vanilla = 1): ";
            string effBuffer = ModSettings_CropQuality.lavishEfficiency.ToString();
            LabeledFloatEntry(listingStandard.GetRect(24f), effLabel, ref ModSettings_CropQuality.lavishEfficiency, ref effBuffer, 0.05f, 0.1f, .5f, 1f);
            listingStandard.Label("          (Note: Lower uses less ingredients. Efficiency of 0.55 is equivalent to simple/fine meals.)");
            listingStandard.GapLine();
            listingStandard.Label("*Restart required");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public void LabeledFloatEntry(Rect rect, string label, ref float value, ref string editBuffer, float multiplier, float largeMultiplier, float min, float max)
        {
            float num = rect.width / 15f;
            Widgets.Label(rect, label);
            if (multiplier != largeMultiplier)
            {
                if (Widgets.ButtonText(new Rect(rect.xMax - num * 5f, rect.yMin, (float)num, rect.height), (-1 * largeMultiplier).ToString(), true, true, true))
                {
                    value -= largeMultiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
                }
                if (Widgets.ButtonText(new Rect(rect.xMax - num, rect.yMin, num, rect.height), "+" + largeMultiplier.ToString(), true, true, true))
                {
                    value += largeMultiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
                }
            }
            if (Widgets.ButtonText(new Rect(rect.xMax - num * 4f, rect.yMin, num, rect.height), (-1 * multiplier).ToString(), true, true, true))
            {
                value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
            }
            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 2f), rect.yMin, num, rect.height), "+" + multiplier.ToString(), true, true, true))
            {
                value += multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
            }
            Widgets.TextFieldNumeric<float>(new Rect(rect.xMax - (num * 3f), rect.yMin, num, rect.height), ref value, ref editBuffer, min, max);
        }
    }
}
