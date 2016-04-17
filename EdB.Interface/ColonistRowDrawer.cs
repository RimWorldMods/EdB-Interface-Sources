using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ColonistRowDrawer : ListWidgetItemDrawer<TrackedColonist>
	{
		private Vector2 padding = new Vector2(9f, 8f);

		private Vector2 iconSize = new Vector2(41f, 40f);

		private Vector2 iconPadding = new Vector2(16f, 8f);

		private Vector2 paddingTotal;

		private List<Texture> rowTextures = new List<Texture>();

		private Color textColor = new Color(0.85f, 0.85f, 0.85f);

		private Color selectedTextColor = new Color(1f, 1f, 1f);

		private Color deadColor = new Color(0.5f, 0.5f, 0.5f);

		private Texture blackTexture;

		public ColonistRowDrawer()
		{
			this.paddingTotal = new Vector2(this.padding.x * 2f, this.padding.y * 2f);
			this.blackTexture = SolidColorMaterials.NewSolidColorTexture(new Color(0.1523f, 0.168f, 0.1836f));
		}

		public void AddRowColor(Color color)
		{
			this.rowTextures.Add(SolidColorMaterials.NewSolidColorTexture(color));
		}

		public float GetHeight(int index, TrackedColonist colonist, Vector2 cursor, float width, bool selected, bool disabled)
		{
			return 56f;
		}

		public Vector2 Draw(int index, TrackedColonist colonist, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string nameStringShort = colonist.Pawn.NameStringShort;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 56f;
			Rect rect = new Rect(cursor.x, cursor.y, width, num);
			Rect rect2 = new Rect(cursor.x + this.padding.x, cursor.y + this.padding.y, this.iconSize.x, this.iconSize.y);
			GUI.DrawTexture(rect2, this.blackTexture);
			GUI.color = new Color(0.3f, 0.3f, 0.3f);
			Widgets.DrawBox(rect2, 1);
			Rect drawRect = new Rect(rect2.x + this.iconSize.x - 1f, rect2.y, 7f, this.iconSize.y);
			bool flag = colonist.Dead || colonist.Missing;
			bool cryptosleep = colonist.Cryptosleep;
			if (!flag)
			{
				if (colonist.Incapacitated)
				{
					GUI.color = new Color(0.7843f, 0f, 0f);
				}
				else if ((double)colonist.HealthPercent < 0.95)
				{
					GUI.color = new Color(0.7843f, 0.7843f, 0f);
				}
				else
				{
					GUI.color = new Color(0f, 0.7843f, 0f);
				}
				if (colonist.Missing)
				{
					GUI.color = new Color(0.4824f, 0.4824f, 0.4824f);
				}
				float num2 = drawRect.height * colonist.HealthPercent;
				GUI.DrawTexture(new Rect(drawRect.x, drawRect.y + drawRect.height - num2, drawRect.width, num2), BaseContent.WhiteTex);
			}
			GUI.color = new Color(0.3f, 0.3f, 0.3f);
			Widgets.DrawBox(drawRect, 1);
			Rect position = new Rect(cursor.x + this.padding.x + 1f, cursor.y, this.iconSize.x, this.iconSize.y + this.padding.y - 1f);
			try
			{
				GUI.BeginGroup(position);
				Vector2 vector = new Vector2(64f, 64f);
				Vector2 vector2 = new Vector2(-12f, 5f);
				Vector2 vector3 = new Vector2(-12f, -9f);
				bool flag2 = true;
				if (flag)
				{
					GUI.color = this.deadColor;
					flag2 = false;
				}
				else if (cryptosleep)
				{
					GUI.color = ColonistBarDrawer.ColorFrozen;
					flag2 = false;
				}
				if (flag2)
				{
					GUI.color = colonist.Pawn.story.skinColor;
				}
				Rect position2 = new Rect(vector2.x, vector2.y, vector.x, vector.y);
				GUI.DrawTexture(position2, colonist.Pawn.drawer.renderer.graphics.nakedGraphic.MatFront.mainTexture);
				bool flag3 = false;
				foreach (ApparelGraphicRecord current in colonist.Pawn.drawer.renderer.graphics.apparelGraphics)
				{
					if (current.sourceApparel.def.apparel.LastLayer != ApparelLayer.Overhead)
					{
						if (flag2)
						{
							GUI.color = current.sourceApparel.DrawColor;
						}
						GUI.DrawTexture(position2, current.graphic.MatFront.mainTexture);
					}
					else
					{
						flag3 = true;
					}
				}
				if (flag2)
				{
					GUI.color = colonist.Pawn.story.skinColor;
				}
				Rect position3 = new Rect(vector3.x, vector3.y, vector.x, vector.y);
				GUI.DrawTexture(position3, colonist.Pawn.drawer.renderer.graphics.headGraphic.MatFront.mainTexture);
				if (!flag3)
				{
					if (flag2)
					{
						GUI.color = colonist.Pawn.story.hairColor;
					}
					GUI.DrawTexture(position3, colonist.Pawn.drawer.renderer.graphics.hairGraphic.MatFront.mainTexture);
				}
				else
				{
					foreach (ApparelGraphicRecord current2 in colonist.Pawn.drawer.renderer.graphics.apparelGraphics)
					{
						if (current2.sourceApparel.def.apparel.LastLayer == ApparelLayer.Overhead)
						{
							if (flag2)
							{
								GUI.color = current2.sourceApparel.DrawColor;
							}
							GUI.DrawTexture(position3, current2.graphic.MatFront.mainTexture);
						}
					}
				}
			}
			finally
			{
				GUI.EndGroup();
			}
			GUI.color = Color.white;
			try
			{
				if (selected)
				{
					GUI.color = this.selectedTextColor;
				}
				else
				{
					GUI.color = this.textColor;
				}
				string text = null;
				if (flag)
				{
					if (colonist.Missing)
					{
						text = "EdB.Squads.Window.SquadMemberStatus.Missing".Translate();
					}
					else
					{
						text = "EdB.Squads.Window.SquadMemberStatus.Dead".Translate();
					}
				}
				else if (colonist.Cryptosleep)
				{
					text = "EdB.Squads.Window.SquadMemberStatus.Cryptosleep".Translate();
				}
				if (text == null)
				{
					Rect rect3 = new Rect(cursor.x + this.padding.x + this.iconSize.x + this.iconPadding.x, cursor.y + this.padding.y + 2f, width - this.paddingTotal.x - this.iconSize.x - this.iconPadding.x, num - this.paddingTotal.y);
					Widgets.Label(rect3, nameStringShort);
				}
				else
				{
					Rect rect4 = new Rect(cursor.x + this.padding.x + this.iconSize.x + this.iconPadding.x, cursor.y + this.padding.y + 5f, width - this.paddingTotal.x - this.iconSize.x - this.iconPadding.x, (num - this.paddingTotal.y) / 2f);
					Text.Anchor = TextAnchor.LowerLeft;
					Widgets.Label(rect4, nameStringShort);
					rect4.y = rect4.y + rect4.height - 3f;
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Tiny;
					Widgets.Label(rect4, text);
				}
			}
			finally
			{
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
			if (!colonist.Dead && !colonist.Missing)
			{
				string tooltipText = this.GetTooltipText(colonist);
				TooltipHandler.TipRegion(rect, new TipSignal(tooltipText, tooltipText.GetHashCode()));
			}
			return new Vector2(cursor.x, cursor.y + rect.height);
		}

		protected string GetTooltipText(TrackedColonist colonist)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = colonist.Pawn;
			foreach (SkillRecord current in pawn.skills.skills)
			{
				stringBuilder.AppendLine(current.def.skillLabel + ": " + current.level);
			}
			List<WorkTags> list = pawn.story.DisabledWorkTags.ToList<WorkTags>();
			if (list.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("IncapableOf".Translate());
				stringBuilder.AppendLine(": ");
				foreach (WorkTags current2 in list)
				{
					stringBuilder.Append("   ");
					stringBuilder.AppendLine(current2.LabelTranslated());
				}
			}
			if (pawn.story != null && pawn.story.traits != null && pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("Traits".Translate());
				stringBuilder.AppendLine(": ");
				for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
				{
					Trait trait = pawn.story.traits.allTraits[i];
					stringBuilder.Append("   ");
					stringBuilder.AppendLine(trait.LabelCap);
				}
			}
			if (pawn.equipment != null)
			{
				ThingWithComps primary = pawn.equipment.Primary;
				if (primary != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(primary.LabelBaseCap);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
