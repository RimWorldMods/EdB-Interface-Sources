using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class BillDrawer
	{
		private static MethodInfo statusStringGetter;

		private static MethodInfo drawConfigInterfaceMethod;

		public static Texture2D ButtonTexReorderUp;

		public static Texture2D ButtonTexReorderDown;

		public static Texture2D ButtonTexSuspend;

		public static readonly Texture2D ButtonTexDeleteX;

		public static readonly Texture2D ButtonTexMinus;

		public static readonly Texture2D ButtonTexPlus;

		public static readonly Texture2D ButtonBGAtlas;

		public static readonly Texture2D ButtonBGAtlasMouseover;

		public static readonly Texture2D ButtonBGAtlasClick;

		public static Color ButtonColor;

		public static Color ButtonColorDisabled;

		public static float ProductionBillPadding;

		static BillDrawer()
		{
			BillDrawer.ButtonTexDeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
			BillDrawer.ButtonTexMinus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);
			BillDrawer.ButtonTexPlus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);
			BillDrawer.ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);
			BillDrawer.ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);
			BillDrawer.ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);
			BillDrawer.ButtonColor = new Color(1f, 0.8627f, 0.2235f);
			BillDrawer.ButtonColorDisabled = new Color(BillDrawer.ButtonColor.r, BillDrawer.ButtonColor.g, BillDrawer.ButtonColor.b, 0.0627f);
			BillDrawer.ProductionBillPadding = 3f;
			PropertyInfo property = typeof(Bill).GetProperty("StatusString", BindingFlags.Instance | BindingFlags.NonPublic);
			BillDrawer.statusStringGetter = property.GetGetMethod(true);
			BillDrawer.drawConfigInterfaceMethod = typeof(Bill).GetMethod("DrawConfigInterface", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public static void ResetTextures()
		{
			BillDrawer.ButtonTexReorderUp = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/ReorderUp", true);
			BillDrawer.ButtonTexReorderDown = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/ReorderDown", true);
			BillDrawer.ButtonTexSuspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);
		}

		public static Bill DrawListing(BillStack billStack, Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ScrollView scrollView)
		{
			Bill result = null;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (billStack.Count < 10)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.TextButton(rect2, "AddBill".Translate(), true, false))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker(), false));
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect viewRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			scrollView.Begin(viewRect);
			float num = 0f;
			for (int i = 0; i < billStack.Count; i++)
			{
				Bill bill = billStack[i];
				Rect rect3 = BillDrawer.DrawProductionBill(billStack, bill, 0f, num, scrollView.ViewWidth, i);
				if (!bill.DeletedOrDereferenced && rect3.Contains(Event.current.mousePosition))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			scrollView.End(num + 60f);
			GUI.EndGroup();
			return result;
		}

		public static Rect DrawProductionBill(BillStack billStack, Bill bill, float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width - 16f, 60f);
			float width2 = rect.width - BillDrawer.ProductionBillPadding * 2f - 100f;
			float num = Text.CalcHeight(bill.LabelCap, width2);
			string text = (string)BillDrawer.statusStringGetter.Invoke(bill, null);
			Text.Font = GameFont.Tiny;
			float width3 = rect.width - BillDrawer.ProductionBillPadding * 2f - 48f;
			float num2 = (!string.IsNullOrEmpty(text)) ? (Text.CalcHeight(text, width3) - 4f) : 0f;
			Text.Font = GameFont.Small;
			float height = 44f + num + num2;
			rect.height = height;
			Bill_Production productionBill = bill as Bill_Production;
			Color white = Color.white;
			if (!bill.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawAltRect(rect);
				if (index % 2 == 1)
				{
					Widgets.DrawAltRect(rect);
				}
			}
			GUI.color = new Color(0.2969f, 0.3359f, 0.3789f);
			Widgets.DrawBox(rect, 1);
			GUI.color = white;
			Rect position = rect.ContractedBy(BillDrawer.ProductionBillPadding);
			GUI.BeginGroup(position);
			Rect rect2 = new Rect(2f, 3f, 24f, 24f);
			if (billStack.IndexOf(bill) > 0)
			{
				GUI.color = BillDrawer.ButtonColor;
				if (Widgets.ImageButton(rect2, BillDrawer.ButtonTexReorderUp, white))
				{
					billStack.Reorder(bill, -1);
					SoundDef.Named("TickHigh").PlayOneShotOnCamera();
				}
			}
			else if (billStack.Count > 1)
			{
				GUI.color = BillDrawer.ButtonColorDisabled;
				GUI.DrawTexture(rect2, BillDrawer.ButtonTexReorderUp);
			}
			Rect rect3 = new Rect(2f, 28f, 24f, 24f);
			if (billStack.IndexOf(bill) < billStack.Count - 1)
			{
				GUI.color = BillDrawer.ButtonColor;
				if (Widgets.ImageButton(rect3, BillDrawer.ButtonTexReorderDown, white))
				{
					billStack.Reorder(bill, 1);
					SoundDef.Named("TickLow").PlayOneShotOnCamera();
				}
			}
			else if (billStack.Count > 1)
			{
				GUI.color = BillDrawer.ButtonColorDisabled;
				GUI.DrawTexture(rect3, BillDrawer.ButtonTexReorderDown);
			}
			GUI.color = white;
			Rect rect4 = new Rect(36f, 4f, width2, num);
			Widgets.Label(rect4, bill.recipe.LabelCap);
			float height2 = 26f;
			float top = rect4.height + 6f;
			if (productionBill != null)
			{
				Rect rect5 = new Rect(32f, top, 180f, height2);
				string text2 = null;
				if (productionBill.repeatMode == BillRepeatMode.RepeatCount)
				{
					text2 = "DoXTimes".Translate();
					text2 = text2.Replace("X", string.Empty + productionBill.repeatCount);
					productionBill.targetCount = productionBill.repeatCount;
				}
				if (productionBill.repeatMode == BillRepeatMode.TargetCount)
				{
					text2 = "DoUntilYouHaveX".Translate();
					text2 = text2.Replace("X", string.Empty + productionBill.targetCount);
					productionBill.repeatCount = productionBill.targetCount;
				}
				if (productionBill.repeatMode == BillRepeatMode.Forever)
				{
					text2 = "DoForever".Translate();
				}
				if (BillDrawer.TextButton(rect5, text2, white, 0f))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					list.Add(new FloatMenuOption("DoXTimes".Translate(), delegate
					{
						productionBill.repeatMode = BillRepeatMode.RepeatCount;
					}, MenuOptionPriority.Medium, null, null));
					FloatMenuOption item = new FloatMenuOption("DoUntilYouHaveX".Translate(), delegate
					{
						if (!productionBill.recipe.WorkerCounter.CanCountProducts(productionBill))
						{
							Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageSound.RejectInput);
						}
						else
						{
							productionBill.repeatMode = BillRepeatMode.TargetCount;
						}
					}, MenuOptionPriority.Medium, null, null);
					list.Add(item);
					list.Add(new FloatMenuOption("DoForever".Translate(), delegate
					{
						productionBill.repeatMode = BillRepeatMode.Forever;
					}, MenuOptionPriority.Medium, null, null));
					Find.WindowStack.Add(new FloatMenu(list, false));
				}
				if (!text.NullOrEmpty())
				{
					Rect rect6 = new Rect(rect5.x + 3f, rect5.y + rect5.height + 4f, width3, num2);
					Text.Font = GameFont.Tiny;
					Widgets.Label(rect6, text);
					Text.Font = GameFont.Small;
				}
				Rect rect7 = new Rect(213f, top, 27f, height2);
				if (BillDrawer.TextButton(rect7, "-", white, -1f))
				{
					if (productionBill.repeatMode == BillRepeatMode.Forever)
					{
						productionBill.repeatMode = BillRepeatMode.RepeatCount;
						productionBill.repeatCount = 1;
					}
					if (productionBill.repeatMode == BillRepeatMode.TargetCount)
					{
						productionBill.targetCount = Mathf.Max(1, productionBill.targetCount - 1);
					}
					if (productionBill.repeatMode == BillRepeatMode.RepeatCount)
					{
						productionBill.repeatCount = Mathf.Max(1, productionBill.repeatCount - 1);
					}
					SoundDef.Named("TickLow").PlayOneShotOnCamera();
				}
				Rect rect8 = new Rect(243f, top, 27f, height2);
				if (BillDrawer.TextButton(rect8, "+", white, -1f))
				{
					if (productionBill.repeatMode == BillRepeatMode.Forever)
					{
						productionBill.repeatMode = BillRepeatMode.RepeatCount;
						productionBill.repeatCount = 1;
					}
					if (productionBill.repeatMode == BillRepeatMode.TargetCount)
					{
						productionBill.targetCount++;
					}
					if (productionBill.repeatMode == BillRepeatMode.RepeatCount)
					{
						productionBill.repeatCount++;
					}
					SoundDef.Named("TickHigh").PlayOneShotOnCamera();
				}
				Rect rect9 = new Rect(276f, top, 35f, height2);
				if (BillDrawer.TextButton(rect9, "...", white, 0f))
				{
					Find.WindowStack.Add(new Dialog_BillConfig(productionBill, ((Thing)productionBill.billStack.billGiver).Position));
				}
			}
			GUI.color = white;
			Rect rect10 = new Rect(position.width - 28f, 1f, 24f, 24f);
			if (Widgets.ImageButton(rect10, BillDrawer.ButtonTexDeleteX, white))
			{
				billStack.Delete(bill);
			}
			Rect butRect = new Rect(rect10);
			butRect.x -= butRect.width + 4f;
			GUI.color = BillDrawer.ButtonColor;
			if (Widgets.ImageButton(butRect, BillDrawer.ButtonTexSuspend, white))
			{
				bill.suspended = !bill.suspended;
			}
			GUI.EndGroup();
			if (bill.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect11 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect11, TexUI.GrayTextBG);
				Widgets.Label(rect11, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			return rect;
		}

		public static Rect DrawMedicalBill(BillStack billStack, Bill bill, float x, float y, float width, int index)
		{
			string text = (string)BillDrawer.statusStringGetter.Invoke(bill, null);
			Rect rect = new Rect(x, y, width, 48f);
			float width2 = rect.width - 106f;
			float num = Text.CalcHeight(bill.LabelCap, width2);
			float num2 = num + 10f;
			rect.height = ((num2 >= rect.height) ? num2 : rect.height);
			if (!text.NullOrEmpty())
			{
				rect.height += 17f;
			}
			Color white = Color.white;
			if (!bill.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawAltRect(rect);
				if (index % 2 == 1)
				{
					Widgets.DrawAltRect(rect);
				}
			}
			GUI.color = new Color(0.2969f, 0.3359f, 0.3789f);
			Widgets.DrawBox(rect, 1);
			GUI.color = white;
			try
			{
				GUI.BeginGroup(rect);
				Rect rect2 = new Rect(10f, 4f, 24f, 20f);
				GUI.color = BillDrawer.ButtonColor;
				if (billStack.IndexOf(bill) > 0)
				{
					if (Widgets.ImageButton(rect2, BillDrawer.ButtonTexReorderUp, white))
					{
						billStack.Reorder(bill, -1);
						SoundDefOf.TickHigh.PlayOneShotOnCamera();
					}
				}
				else if (billStack.Count > 1)
				{
					GUI.color = BillDrawer.ButtonColorDisabled;
					GUI.DrawTexture(rect2, BillDrawer.ButtonTexReorderUp);
				}
				Rect rect3 = new Rect(10f, 26f, 24f, 20f);
				if (billStack.IndexOf(bill) < billStack.Count - 1)
				{
					GUI.color = BillDrawer.ButtonColor;
					if (Widgets.ImageButton(rect3, BillDrawer.ButtonTexReorderDown, white))
					{
						billStack.Reorder(bill, 1);
						SoundDefOf.TickLow.PlayOneShotOnCamera();
					}
				}
				else if (billStack.Count > 1)
				{
					GUI.color = BillDrawer.ButtonColorDisabled;
					GUI.DrawTexture(rect3, BillDrawer.ButtonTexReorderDown);
				}
				GUI.color = white;
				Rect rect4 = new Rect(42f, 1f, width2, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				if (bill.suspended)
				{
					GUI.color = new Color(white.r, white.g, white.b, 0.45f * white.a);
				}
				Widgets.Label(rect4, bill.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				BillDrawer.drawConfigInterfaceMethod.Invoke(bill, new object[]
				{
					rect.AtZero(),
					white
				});
				Rect rect5 = new Rect(rect.width - 28f, 4f, 24f, 24f);
				if (Widgets.ImageButton(rect5, TexButton.DeleteX, white))
				{
					billStack.Delete(bill);
				}
				Rect butRect = new Rect(rect5);
				butRect.x -= butRect.width + 4f;
				GUI.color = BillDrawer.ButtonColor;
				if (Widgets.ImageButton(butRect, BillDrawer.ButtonTexSuspend, white))
				{
					bill.suspended = !bill.suspended;
				}
				if (!text.NullOrEmpty())
				{
					Text.Font = GameFont.Tiny;
					Rect rect6 = new Rect(24f, rect.height - 17f, rect.width - 24f, 17f);
					Widgets.Label(rect6, text);
				}
			}
			finally
			{
				GUI.EndGroup();
			}
			if (bill.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect7 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect7, TexUI.GrayTextBG);
				GUI.color = new Color(0.9f, 0.9f, 0.9f, 1f);
				Widgets.Label(rect7, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		public static bool TextButton(Rect rect, string label, Color optionColor, float labelAdjustmentX = 0f)
		{
			Texture2D atlas = BillDrawer.ButtonBGAtlas;
			if (rect.Contains(Event.current.mousePosition))
			{
				atlas = BillDrawer.ButtonBGAtlasMouseover;
				if (Input.GetMouseButton(0))
				{
					atlas = BillDrawer.ButtonBGAtlasClick;
				}
			}
			Widgets.DrawAtlas(rect, atlas);
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect2 = new Rect(rect);
			rect2.x += labelAdjustmentX;
			rect2.y += 1f;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			return Widgets.InvisibleButton(rect);
		}
	}
}
