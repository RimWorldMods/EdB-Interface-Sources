using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class TabDrawer
	{
		private const float TabHoriztonalOverlap = 10f;

		private const float MaxTabWidth = 200f;

		private const float TabHeight = 32f;

		public static readonly Texture2D AlternateRowTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		public static Texture2D WhiteTexture = null;

		public static Vector2 TabPanelSize = new Vector2(360f, 670f);

		public static Color TextColor = new Color(0.8f, 0.8f, 0.8f);

		public static Color HeaderColor = new Color(1f, 1f, 1f);

		public static Color SeparatorColor = new Color(0.3f, 0.3f, 0.3f);

		public static Color NoneColor = new Color(0.4f, 0.4f, 0.4f);

		public static Color BoxColor = new Color(0.1172f, 0.1328f, 0.1484f);

		public static Color BoxBorderColor = new Color(0.52157f, 0.52157f, 0.52157f);

		private static List<TabRecord> tabList = new List<TabRecord>();

		public static void ResetTextures()
		{
			TabDrawer.WhiteTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f));
		}

		public static void DrawBox(Rect rect)
		{
			GUI.color = TabDrawer.BoxColor;
			GUI.DrawTexture(rect, TabDrawer.WhiteTexture);
			GUI.color = TabDrawer.BoxBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		public static float DrawHeader(float left, float top, float contentWidth, string labelText, bool drawSeparator = true, TextAnchor alignment = TextAnchor.UpperLeft)
		{
			TextAnchor alignment2 = GUI.skin.label.alignment;
			float num = 2f;
			GUI.skin.label.alignment = alignment;
			Rect rect = new Rect(left, top, contentWidth, 22f);
			rect.y = top;
			Text.Font = GameFont.Small;
			GUI.color = TabDrawer.HeaderColor;
			Widgets.Label(rect, labelText);
			if (drawSeparator)
			{
				GUI.color = TabDrawer.SeparatorColor;
				Widgets.DrawLineHorizontal(left, rect.y + rect.height - num, rect.width);
				GUI.color = TabDrawer.TextColor;
			}
			float num2 = top + (rect.height - num);
			GUI.skin.label.alignment = alignment2;
			return num2 - top;
		}

		public static float DrawNameAndBasicInfo(float left, float top, Pawn pawn, float contentWidth, bool allowRename = false)
		{
			float num = top;
			float num2 = 5f;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			Rect rect = new Rect(left, num, contentWidth, 30f);
			Text.Font = GameFont.Medium;
			if (pawn != null)
			{
				string text = (pawn.story == null) ? pawn.Label : pawn.Name.ToStringFull;
				Vector2 vector = Text.CalcSize(text);
				GUI.color = TabDrawer.HeaderColor;
				Widgets.Label(rect, text);
				float num3 = rect.height - num2;
				if (allowRename && pawn.IsColonist)
				{
					Rect rect2 = new Rect(left + vector.x + 7f, num - 2f, 30f, 30f);
					TooltipHandler.TipRegion(rect2, new TipSignal("RenameColonist".Translate()));
					if (Widgets.ImageButton(rect2, TabDrawer.Rename))
					{
						Find.WindowStack.Add(new Dialog_ChangeNameTriple(pawn));
					}
				}
				num += num3;
			}
			Vector2 vector2 = new Vector2(contentWidth, 24f);
			Text.Font = GameFont.Small;
			GUI.color = TabDrawer.HeaderColor;
			Rect rect3 = new Rect(left, num, vector2.x, vector2.y);
			string text2 = (pawn.gender == Gender.Male) ? "Male".Translate().CapitalizeFirst() : "Female".Translate().CapitalizeFirst();
			if (!pawn.def.label.Equals(pawn.KindLabel))
			{
				Widgets.Label(rect3, string.Concat(new string[]
				{
					text2,
					" ",
					pawn.def.LabelCap,
					" ",
					pawn.KindLabel,
					", ",
					"AgeIndicator".Translate(new object[]
					{
						pawn.ageTracker.AgeNumberString
					})
				}));
				TooltipHandler.TipRegion(rect3, () => pawn.ageTracker.AgeTooltipString, 6873641);
			}
			else
			{
				Widgets.Label(rect3, string.Concat(new string[]
				{
					text2,
					" ",
					pawn.def.LabelCap,
					", ",
					"AgeIndicator".Translate(new object[]
					{
						pawn.ageTracker.AgeNumberString
					})
				}));
			}
			num += vector2.y;
			return num - top - 3f;
		}

		public static TabRecord DrawTabs(Rect baseRect, IEnumerable<TabRecord> tabsEnum, Texture2D atlas)
		{
			TabDrawer.tabList.Clear();
			foreach (TabRecord current in tabsEnum)
			{
				TabDrawer.tabList.Add(current);
			}
			TabRecord tabRecord = null;
			TabRecord tabRecord2 = (from t in TabDrawer.tabList
			where t.selected
			select t).FirstOrDefault<TabRecord>();
			if (tabRecord2 == null)
			{
				Debug.LogWarning("Drew tabs without any being selected.");
				return TabDrawer.tabList[0];
			}
			float num = baseRect.width + (float)(TabDrawer.tabList.Count - 1) * 10f;
			float tabWidth = num / (float)TabDrawer.tabList.Count;
			if (tabWidth > 200f)
			{
				tabWidth = 200f;
			}
			Rect position = new Rect(baseRect);
			position.y -= 32f;
			position.height = 9999f;
			GUI.BeginGroup(position);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Small;
			Func<TabRecord, Rect> func = delegate(TabRecord tab)
			{
				int num2 = TabDrawer.tabList.IndexOf(tab);
				float left = (float)num2 * (tabWidth - 10f);
				return new Rect(left, 1f, tabWidth, 32f);
			};
			List<TabRecord> list = TabDrawer.tabList.ListFullCopy<TabRecord>();
			list.Remove(tabRecord2);
			list.Add(tabRecord2);
			TabRecord tabRecord3 = null;
			List<TabRecord> list2 = list.ListFullCopy<TabRecord>();
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				TabRecord tabRecord4 = list2[i];
				Rect rect = func(tabRecord4);
				if (tabRecord3 == null && rect.Contains(Event.current.mousePosition))
				{
					tabRecord3 = tabRecord4;
				}
				MouseoverSounds.DoRegion(rect);
				if (Widgets.InvisibleButton(rect))
				{
					tabRecord = tabRecord4;
				}
			}
			foreach (TabRecord current2 in list)
			{
				Rect rect2 = func(current2);
				TabDrawer.DrawTab(current2, rect2, atlas);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			if (tabRecord != null)
			{
				SoundDefOf.SelectDesignator.PlayOneShotOnCamera();
				if (tabRecord.clickedAction != null)
				{
					tabRecord.clickedAction();
				}
			}
			return tabRecord;
		}

		public static void DrawTab(TabRecord record, Rect rect, Texture2D atlas)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect texRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			Rect texRect2 = new Rect(30f, 0f, 4f, (float)atlas.height).ToUVRect(new Vector2((float)atlas.width, (float)atlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), atlas);
			Widgets.DrawTexturePart(drawRect3, texRect2, atlas);
			Widgets.DrawTexturePart(drawRect2, texRect, atlas);
			Rect rect2 = rect;
			if (rect.Contains(Event.current.mousePosition))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			Widgets.Label(rect2, record.label);
			GUI.color = Color.white;
			if (!record.selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect texRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, texRect3, atlas);
			}
		}
	}
}
