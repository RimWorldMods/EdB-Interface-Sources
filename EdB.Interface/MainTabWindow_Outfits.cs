using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Outfits : MainTabWindow_PawnListWithSquads
	{
		private const float TopAreaHeight = 45f;

		private Outfit outfit;

		private bool anyForcedApparel;

		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, this.WindowHeight);
			}
		}

		protected override float WindowHeight
		{
			get
			{
				return 45f + (float)base.PawnsCount * 30f + 65f + base.ExtraHeight;
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.outfit = Find.Map.outfitDatabase.AllOutfits.FirstOrDefault<Outfit>();
		}

		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect position = new Rect(0f, 0f, fillRect.width, 45f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 160f, 35f);
			if (Widgets.TextButton(rect, "ManageOutfits".Translate(), true, false))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(null));
			}
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(175f, 0f, position.width - 175f, position.height);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width / 2f, rect2.height);
			Widgets.Label(rect3, "CurrentOutfit".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			float squadRowHeight = base.SquadRowHeight;
			Rect outRect = new Rect(0f, 45f + squadRowHeight, fillRect.width, fillRect.height - 45f - base.PawnListScrollHeightReduction);
			this.anyForcedApparel = false;
			base.DrawRows(outRect);
			if (base.SquadRowEnabled)
			{
				this.DrawSquadRow(new Rect(0f, 45f, fillRect.width - 16f, squadRowHeight));
			}
			if (base.SquadFilteringEnabled)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				this.DrawSquadSelectionDropdown(new Rect(fillRect.x, fillRect.y + fillRect.height - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
		}

		protected void DrawSquadRow(Rect rect)
		{
			float num = 3f;
			GUI.DrawTexture(rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			float height = rect.height - num - 4f;
			Rect rect2 = new Rect(rect.x + 175f, rect.y, rect.width - 175f, rect.height - num);
			Rect rect3 = new Rect(rect2.x, rect2.y + 2f, rect2.width * 0.333f, height);
			if (Widgets.TextButton(rect3, this.outfit.label, true, false))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (Outfit current in Find.Map.outfitDatabase.AllOutfits)
				{
					Outfit localOut = current;
					list.Add(new FloatMenuOption(localOut.label, delegate
					{
						this.outfit = localOut;
						foreach (Pawn current3 in this.pawns)
						{
							current3.outfits.CurrentOutfit = localOut;
						}
					}, MenuOptionPriority.Medium, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list, false));
			}
			Rect rect4 = new Rect(rect3.xMax + 4f, rect.y + 2f, 100f, height);
			if (Widgets.TextButton(rect4, "OutfitEdit".Translate(), true, false))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(this.outfit));
			}
			Rect rect5 = new Rect(rect4.xMax + 4f, rect.y + 2f, 100f, height);
			if (this.anyForcedApparel && Widgets.TextButton(rect5, "ClearForcedApparel".Translate(), true, false))
			{
				foreach (Pawn current2 in this.pawns)
				{
					current2.outfits.forcedHandler.Reset();
				}
			}
			GUI.BeginGroup(rect);
			GUI.color = new Color(1f, 1f, 1f, 0.2f);
			Widgets.DrawLineHorizontal(0f, 0f, rect.width);
			GUI.color = new Color(1f, 1f, 1f, 0.35f);
			Widgets.DrawLineHorizontal(0f, base.SquadRowHeight - 3f, rect.width);
			Widgets.DrawLineHorizontal(0f, base.SquadRowHeight - 2f, rect.width);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Rect rect6 = new Rect(0f, 0f, 175f, 30f);
			rect6.xMin += 15f;
			GUI.color = new Color(1f, 1f, 1f, 1f);
			if (base.SquadFilteringEnabled && SquadManager.Instance.SquadFilter != null)
			{
				Widgets.Label(rect6, SquadManager.Instance.SquadFilter.Name);
			}
			else
			{
				Widgets.Label(rect6, "EdB.Squads.AllColonistsSquadName".Translate());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			GUI.EndGroup();
		}

		protected override void DrawPawnRow(Rect rect, Pawn p)
		{
			Rect rect2 = new Rect(rect.x + 175f, rect.y, rect.width - 175f, rect.height);
			Rect rect3 = new Rect(rect2.x, rect2.y + 2f, rect2.width * 0.333f, rect2.height - 4f);
			if (Widgets.TextButton(rect3, p.outfits.CurrentOutfit.label, true, false))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (Outfit current in Find.Map.outfitDatabase.AllOutfits)
				{
					Outfit localOut = current;
					list.Add(new FloatMenuOption(localOut.label, delegate
					{
						p.outfits.CurrentOutfit = localOut;
					}, MenuOptionPriority.Medium, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list, false));
			}
			Rect rect4 = new Rect(rect3.xMax + 4f, rect.y + 2f, 100f, rect.height - 4f);
			if (Widgets.TextButton(rect4, "OutfitEdit".Translate(), true, false))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(p.outfits.CurrentOutfit));
			}
			Rect rect5 = new Rect(rect4.xMax + 4f, rect.y + 2f, 100f, rect.height - 4f);
			if (p.outfits.forcedHandler.SomethingIsForced)
			{
				this.anyForcedApparel = true;
				if (Widgets.TextButton(rect5, "ClearForcedApparel".Translate(), true, false))
				{
					p.outfits.forcedHandler.Reset();
				}
				TooltipHandler.TipRegion(rect5, new TipSignal(delegate
				{
					string text = "ForcedApparel".Translate() + ":\n";
					foreach (Apparel current2 in p.outfits.forcedHandler.ForcedApparel)
					{
						text = text + "\n   " + current2.LabelCap;
					}
					return text;
				}, p.GetHashCode() * 612));
			}
		}
	}
}
