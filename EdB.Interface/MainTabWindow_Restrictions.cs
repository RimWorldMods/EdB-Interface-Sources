using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Restrictions : MainTabWindow_PawnListWithSquads
	{
		private const float TopAreaHeight = 65f;

		private const float CopyPasteColumnWidth = 52f;

		private const float CopyPasteIconSize = 24f;

		private const float TimeTablesWidth = 500f;

		private const float AAGapWidth = 6f;

		private TimeAssignmentDef selectedAssignment = TimeAssignmentDefOf.Work;

		private List<TimeAssignmentDef> clipboard;

		private float hourWidth;

		private Pawn_TimetableTracker timetable = new Pawn_TimetableTracker();

		private Area areaRestriction;

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
				return 65f + (float)base.PawnsCount * 30f + 65f + base.ExtraHeight;
			}
		}

		private void CopyFrom(Pawn p)
		{
			this.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		public override void PreOpen()
		{
			base.PreOpen();
			for (int i = 0; i < 24; i++)
			{
				this.timetable.SetAssignment(i, TimeAssignmentDefOf.Anything);
			}
			this.areaRestriction = null;
			this.clipboard = null;
		}

		private void DoTimeAssignment(Rect rect, Pawn p, int hour)
		{
			rect = rect.ContractedBy(1f);
			TimeAssignmentDef assignment = p.timetable.GetAssignment(hour);
			GUI.DrawTexture(rect, assignment.ColorTexture);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 2);
				if (assignment != this.selectedAssignment && Input.GetMouseButton(0))
				{
					SoundDefOf.DesignateDragStandardChanged.PlayOneShotOnCamera();
					p.timetable.SetAssignment(hour, this.selectedAssignment);
				}
			}
		}

		private void DoTimeAssignment(Rect rect, int hour)
		{
			rect = rect.ContractedBy(1f);
			TimeAssignmentDef assignment = this.timetable.GetAssignment(hour);
			GUI.DrawTexture(rect, assignment.ColorTexture);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 2);
				bool flag = false;
				if (Input.GetMouseButton(0))
				{
					if (assignment != this.selectedAssignment)
					{
						this.timetable.SetAssignment(hour, this.selectedAssignment);
					}
					foreach (Pawn current in this.pawns)
					{
						if (current.timetable.GetAssignment(hour) != assignment)
						{
							current.timetable.SetAssignment(hour, this.selectedAssignment);
							flag = true;
						}
					}
				}
				if (flag)
				{
					SoundDefOf.DesignateDragStandardChanged.PlayOneShotOnCamera();
				}
			}
		}

		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect position = new Rect(0f, 0f, fillRect.width, 65f);
			GUI.BeginGroup(position);
			Rect rect = new Rect(0f, 0f, 217f, position.height);
			this.DrawTimeAssignmentSelectorGrid(rect);
			float num = 227f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerLeft;
			for (int i = 0; i < 24; i++)
			{
				Rect rect2 = new Rect(num + 4f, 0f, this.hourWidth, position.height + 3f);
				Widgets.Label(rect2, i.ToString());
				num += this.hourWidth;
			}
			num += 6f;
			Rect rect3 = new Rect(num, 0f, position.width - num - 16f, Mathf.Round(position.height / 2f));
			Text.Font = GameFont.Small;
			if (Widgets.TextButton(rect3, "ManageAreas".Translate(), true, false))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas());
			}
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect4 = new Rect(num, 0f, position.width - num, position.height + 3f);
			Widgets.Label(rect4, "AllowedArea".Translate());
			GUI.EndGroup();
			if (base.SquadFilteringEnabled)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				this.DrawSquadSelectionDropdown(new Rect(fillRect.x, fillRect.y + fillRect.height - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			float squadRowHeight = base.SquadRowHeight;
			Rect outRect = new Rect(0f, position.height + squadRowHeight, fillRect.width, fillRect.height - position.height - base.PawnListScrollHeightReduction);
			base.DrawRows(outRect);
			if (base.SquadRowEnabled)
			{
				this.DrawSquadRow(new Rect(0f, position.height, fillRect.width - 16f, squadRowHeight));
			}
		}

		protected void DrawSquadRow(Rect rect)
		{
			float num = 3f;
			GUI.DrawTexture(rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(175f, 4f, 24f, 24f);
			if (this.clipboard != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax + 2f;
				if (Widgets.ImageButton(rect3, TexButton.Paste))
				{
					foreach (Pawn current in this.pawns)
					{
						this.PasteTo(current);
					}
					SoundDefOf.TickLow.PlayOneShotOnCamera();
				}
				TooltipHandler.TipRegion(rect3, "Paste".Translate());
			}
			float num2 = 227f;
			this.hourWidth = 20.83333f;
			for (int i = 0; i < 24; i++)
			{
				Rect rect4 = new Rect(num2, 0f, this.hourWidth, rect.height - num);
				this.DoTimeAssignment(rect4, i);
				num2 += this.hourWidth;
			}
			GUI.color = Color.white;
			num2 += 6f;
			Rect rect5 = new Rect(num2, 0f, rect.width - num2, rect.height - num);
			AreaAllowedGUI.DoAllowedAreaSelectors(rect5, ref this.areaRestriction, AllowedAreaMode.Humanlike, this.pawns);
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
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(175f, 4f, 24f, 24f);
			if (Widgets.ImageButton(rect2, TexButton.Copy))
			{
				this.CopyFrom(p);
				SoundDefOf.TickHigh.PlayOneShotOnCamera();
			}
			TooltipHandler.TipRegion(rect2, "Copy".Translate());
			if (this.clipboard != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax + 2f;
				if (Widgets.ImageButton(rect3, TexButton.Paste))
				{
					this.PasteTo(p);
					SoundDefOf.TickLow.PlayOneShotOnCamera();
				}
				TooltipHandler.TipRegion(rect3, "Paste".Translate());
			}
			float num = 227f;
			this.hourWidth = 20.83333f;
			for (int i = 0; i < 24; i++)
			{
				Rect rect4 = new Rect(num, 0f, this.hourWidth, rect.height);
				this.DoTimeAssignment(rect4, p, i);
				num += this.hourWidth;
			}
			GUI.color = Color.white;
			num += 6f;
			Rect rect5 = new Rect(num, 0f, rect.width - num, rect.height);
			AreaAllowedGUI.DoAllowedAreaSelectors(rect5, p, AllowedAreaMode.Humanlike);
			GUI.EndGroup();
		}

		private void DrawTimeAssignmentSelectorFor(Rect rect, TimeAssignmentDef ta)
		{
			rect = rect.ContractedBy(2f);
			GUI.DrawTexture(rect, ta.ColorTexture);
			if (Widgets.InvisibleButton(rect))
			{
				this.selectedAssignment = ta;
				SoundDefOf.TickHigh.PlayOneShotOnCamera();
			}
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.white;
			Widgets.Label(rect, ta.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.selectedAssignment == ta)
			{
				Widgets.DrawBox(rect, 2);
			}
		}

		private void DrawTimeAssignmentSelectorGrid(Rect rect)
		{
			rect.yMax -= 2f;
			Rect rect2 = rect;
			rect2.xMax = rect2.center.x;
			rect2.yMax = rect2.center.y;
			this.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Anything);
			rect2.x += rect2.width;
			this.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Work);
			rect2.y += rect2.height;
			rect2.x -= rect2.width;
			this.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Joy);
			rect2.x += rect2.width;
			this.DrawTimeAssignmentSelectorFor(rect2, TimeAssignmentDefOf.Sleep);
		}

		private void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = this.clipboard[i];
			}
		}
	}
}
