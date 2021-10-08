using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CropQuality
{
    [StaticConstructorOnStartup]
    public class DefPatch
    {
        static DefPatch()
        {
            List<RecipeDef> allRecipes = DefDatabase<RecipeDef>.AllDefsListForReading;
            QualityRange range = new QualityRange();
            range.max = QualityCategory.Legendary;
            for (int i = 0; i < allRecipes.Count; i++)
            {
                RecipeDef recipe = allRecipes[i];
                if (recipe.workSkill == SkillDefOf.Cooking
                    && recipe.ProducedThingDef != null
                    && recipe.ProducedThingDef.ingestible != null)
                {
                    FoodPreferability preferability = recipe.ProducedThingDef.ingestible.preferability;
                    if (preferability == FoodPreferability.MealFine)
                    {
                        range.min = QualityCategory.Good;
                        recipe.fixedIngredientFilter.AllowedQualityLevels = range;
                        recipe.defaultIngredientFilter.AllowedQualityLevels = range;
                    }
                    if (preferability == FoodPreferability.MealLavish)
                    {
                        range.min = QualityCategory.Excellent;
                        recipe.fixedIngredientFilter.AllowedQualityLevels = range;
                        recipe.defaultIngredientFilter.AllowedQualityLevels = range;
                        if (ModSettings_CropQuality.lavishEfficiency != 1)
                        {
                            for (int j = 0; j < recipe.ingredients.Count; j++)
                            {
                                recipe.ingredients[j].SetBaseCount(recipe.ingredients[j].GetBaseCount() * ModSettings_CropQuality.lavishEfficiency);
                            }
                        }
                    }
                }
            }

            List<ThingDef> allFood = DefDatabase<ThingDef>.AllDefsListForReading;
            for (int m = 0; m < allFood.Count; m++)
            {
                ThingDef def = allFood[m];
                CompProperties comp = new CompProperties();
                comp.compClass = typeof(CompQuality);
                if (def.IsMeat && !def.ingestible.IsMeal && !def.HasComp(typeof(CompQuality)))
                {
                    def.comps.Add(comp);
                    def.BaseMarketValue = def.BaseMarketValue * .8f;
                }

                if (def.plant != null && def.plant.harvestedThingDef != null)
                {
                    ThingDef crop = def.plant.harvestedThingDef;
                    if (!crop.HasComp(typeof(CompQuality)) && crop.IsIngestible && (crop.ingestible.preferability == FoodPreferability.RawBad || crop.ingestible.preferability == FoodPreferability.RawTasty))
                    {
                        crop.comps.Add(comp);
                        crop.BaseMarketValue = crop.BaseMarketValue * .8f;
                    }
                }

            }

        }

            /*
            if (ModSettings_CropQuality.lavishEfficiency)
            {
                List<ThingDef> mealDefs = DefDatabase<ThingDef>.AllDefsListForReading;
                for (int k = 0; k < mealDefs.Count; k++)
                {
                    ThingDef meal = mealDefs[k];
                    if (meal.IsIngestible && meal.ingestible.preferability == FoodPreferability.MealLavish)
                    {
                        for (int l = 0; l < meal.statBases.Count; l++)
                        {
                            StatModifier nutrition = meal.statBases[l];
                            if (nutrition.stat == StatDefOf.Nutrition)
                            {
                                nutrition.value -= .1f;
                            }
                        }
                    }
                }
            }
            */
    }
}
