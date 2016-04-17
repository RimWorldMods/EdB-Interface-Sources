using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class SkillDrawer
	{
		private const float IncButSpacing = 10f;

		public const float ProgBarHeight = 8f;

		public const float SkillYSpacing = 3f;

		public const float SkillRowHeight = 24f;

		public const float SkillBarHeight = 22f;

		public const float IncButX = 205f;

		public const float ProgBarX = 180f;

		public const float LevelNumberX = 140f;

		public const float LeftEdgeMargin = 6f;

		private const float SkillWidth = 380f;

		private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);

		private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);

		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		private static Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

		private static float levelLabelWidth = -1f;

		private void DrawSkill(SkillRecord skill, Vector2 topLeft)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, 380f, 20f);
			if (rect.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			try
			{
				GUI.BeginGroup(rect);
				Text.Anchor = TextAnchor.UpperLeft;
				Rect rect2 = new Rect(6f, -3f, SkillDrawer.levelLabelWidth + 6f, rect.height + 5f);
				rect2.yMin += 3f;
				GUI.color = TabDrawer.TextColor;
				Widgets.Label(rect2, skill.def.skillLabel);
				Rect position = new Rect(rect2.xMax, 0f, 24f, 24f);
				if (skill.passion > Passion.None)
				{
					Texture2D image = (skill.passion == Passion.Major) ? SkillDrawer.PassionMajorIcon : SkillDrawer.PassionMinorIcon;
					GUI.DrawTexture(position, image);
				}
				if (!skill.TotallyDisabled)
				{
					Rect rect3 = new Rect(position.xMax, 0f, rect.width - position.xMax, rect.height);
					Widgets.FillableBar(rect3, (float)skill.level / 20f, SkillDrawer.SkillBarFillTex, null, false);
				}
				Rect rect4 = new Rect(position.xMax + 4f, -2f, 999f, rect2.height);
				rect4.yMin += 3f;
				string label;
				if (skill.TotallyDisabled)
				{
					GUI.color = SkillDrawer.DisabledSkillColor;
					label = "-";
				}
				else
				{
					label = skill.level.ToStringCached();
				}
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect4, label);
			}
			finally
			{
				GUI.EndGroup();
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			TooltipHandler.TipRegion(rect, new TipSignal(SkillDrawer.GetSkillDescription(skill), skill.def.GetHashCode() * 397945));
		}

		private static string GetSkillDescription(SkillRecord sk)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (sk.TotallyDisabled)
			{
				stringBuilder.Append("DisabledLower".Translate().CapitalizeFirst());
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Level".Translate(),
					" ",
					sk.level,
					": ",
					sk.LevelDescriptor
				}));
				if (Game.Mode == GameMode.MapPlaying)
				{
					string text = (sk.level == 20) ? "Experience".Translate() : "ProgressToNextLevel".Translate();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						text,
						": ",
						sk.xpSinceLastLevel.ToString("########0"),
						" / ",
						sk.XpRequiredForLevelUp
					}));
				}
				stringBuilder.Append("Passion".Translate() + ": ");
				switch (sk.passion)
				{
				case Passion.None:
					stringBuilder.Append("PassionNone".Translate(new object[]
					{
						"0.3"
					}));
					break;
				case Passion.Minor:
					stringBuilder.Append("PassionMinor".Translate(new object[]
					{
						"1.0"
					}));
					break;
				case Passion.Major:
					stringBuilder.Append("PassionMajor".Translate(new object[]
					{
						"1.5"
					}));
					break;
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append(sk.def.description);
			return stringBuilder.ToString();
		}

		public void DrawSkillsOf(Pawn p, Vector2 Offset)
		{
			Text.Font = GameFont.Small;
			foreach (SkillDef current in DefDatabase<SkillDef>.AllDefs)
			{
				float x = Text.CalcSize(current.skillLabel).x;
				if (x > SkillDrawer.levelLabelWidth)
				{
					SkillDrawer.levelLabelWidth = x;
				}
			}
			int num = 0;
			foreach (SkillRecord current2 in p.skills.skills)
			{
				float y = (float)num * 24f + Offset.y;
				this.DrawSkill(current2, new Vector2(Offset.x, y));
				num++;
			}
		}
	}
}
