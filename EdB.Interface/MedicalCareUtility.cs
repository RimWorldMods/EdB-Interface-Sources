using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class MedicalCareUtility
	{
		public const float CareSetterHeight = 28f;

		public const float CareSetterWidth = 140f;

		public static Texture2D[] careTextures;

		public static Texture2D HealthOptionActive = null;

		public static Texture2D HealthOptionInactive = null;

		public static Vector2 ButtonSize = new Vector2(32f, 32f);

		public static float ButtonPadding = 8f;

		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return (double)meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= 0.99;
			case MedicalCareCategory.NormalOrWorse:
				return (double)meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= 1.01;
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, MedicalCareUtility.ButtonSize.x, MedicalCareUtility.ButtonSize.y);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				if (medCare == mc)
				{
					GUI.DrawTexture(rect2, MedicalCareUtility.HealthOptionActive);
				}
				else
				{
					GUI.DrawTexture(rect2, MedicalCareUtility.HealthOptionInactive);
				}
				if (Mouse.IsOver(rect))
				{
				}
				Rect position = rect2.ContractedBy(2f);
				GUI.DrawTexture(position, MedicalCareUtility.careTextures[i]);
				if (Widgets.InvisibleButton(rect2))
				{
					medCare = mc;
					SoundDefOf.TickHigh.PlayOneShotOnCamera();
				}
				TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				rect2.x += MedicalCareUtility.ButtonSize.x + MedicalCareUtility.ButtonPadding;
			}
		}

		public static void Reset()
		{
			MedicalCareUtility.careTextures = new Texture2D[5];
			MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
			MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
			MedicalCareUtility.careTextures[2] = ThingDefOf.HerbalMedicine.uiIcon;
			MedicalCareUtility.careTextures[3] = ThingDefOf.Medicine.uiIcon;
			MedicalCareUtility.careTextures[4] = ThingDefOf.GlitterworldMedicine.uiIcon;
			MedicalCareUtility.HealthOptionActive = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/HealthOptionActive", true);
			MedicalCareUtility.HealthOptionInactive = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/HealthOptionInactive", true);
		}
	}
}
