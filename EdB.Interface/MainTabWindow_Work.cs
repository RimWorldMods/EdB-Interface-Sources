using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Work : MainTabWindow_PawnListWithSquads
	{
		private const float TopAreaHeight = 40f;

		protected const float LabelRowHeight = 50f;

		private static List<WorkTypeDef> VisibleWorkTypeDefsInPriorityOrder;

		private float workColumnSpacing = -1f;

		protected SquadPriorities squadPriorities = new SquadPriorities();

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
				return 90f + (float)base.PawnsCount * 30f + 65f + base.ExtraHeight;
			}
		}

		public static void Reinit()
		{
			MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder = (from def in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where def.visible
			select def).ToList<WorkTypeDef>();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			Rect position = new Rect(0f, 0f, rect.width, 40f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Find.Map.playSettings.useWorkPriorities;
			Widgets.LabelCheckbox(rect2, "ManualPriorities".Translate(), ref Find.Map.playSettings.useWorkPriorities, false);
			if (useWorkPriorities != Find.Map.playSettings.useWorkPriorities)
			{
				foreach (Pawn current in Find.ListerPawns.FreeColonists)
				{
					current.workSettings.Notify_UseWorkPrioritiesChanged();
				}
			}
			float num = position.width / 3f;
			float num2 = position.width * 2f / 3f;
			Rect rect3 = new Rect(num - 50f, 5f, 160f, 30f);
			Rect rect4 = new Rect(num2 - 50f, 5f, 160f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, "<= " + "HigherPriority".Translate());
			Widgets.Label(rect4, "LowerPriority".Translate() + " =>");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			Rect position2 = new Rect(0f, 40f, rect.width, rect.height - 40f);
			GUI.BeginGroup(position2);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			float squadRowHeight = base.SquadRowHeight;
			Rect outRect = new Rect(0f, 50f + squadRowHeight, position2.width, position2.height - 50f - base.PawnListScrollHeightReduction);
			this.workColumnSpacing = (position2.width - 16f - 175f) / (float)MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count;
			float num3 = 175f;
			int num4 = 0;
			foreach (WorkTypeDef current2 in MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder)
			{
				Vector2 vector = Text.CalcSize(current2.labelShort);
				float num5 = num3 + 15f;
				Rect rect5 = new Rect(num5 - vector.x / 2f, 0f, vector.x, vector.y);
				if (num4 % 2 == 1)
				{
					rect5.y += 20f;
				}
				if (Mouse.IsOver(rect5))
				{
					Widgets.DrawHighlight(rect5);
				}
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect5, current2.labelShort);
				WorkTypeDef localDef = current2;
				TooltipHandler.TipRegion(rect5, new TipSignal(() => localDef.gerundLabel + "\n\n" + localDef.description, localDef.GetHashCode()));
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
				Widgets.DrawLineVertical(num5, rect5.yMax - 3f, 50f - rect5.yMax + 3f);
				Widgets.DrawLineVertical(num5 + 1f, rect5.yMax - 3f, 50f - rect5.yMax + 3f);
				GUI.color = Color.white;
				num3 += this.workColumnSpacing;
				num4++;
			}
			base.DrawRows(outRect);
			if (base.SquadRowEnabled)
			{
				this.DrawSquadRow(new Rect(0f, 50f, position2.width - 16f, squadRowHeight));
			}
			GUI.EndGroup();
			if (base.SquadFilteringEnabled)
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				this.DrawSquadSelectionDropdown(new Rect(rect.x, rect.y + rect.height - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
		}

		protected void DrawSquadRow(Rect rect)
		{
			GUI.DrawTexture(rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			float num = 175f;
			Text.Font = GameFont.Medium;
			for (int i = 0; i < MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count; i++)
			{
				WorkTypeDef wType = MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder[i];
				Vector2 topLeft = new Vector2(num, rect.y + 2.5f);
				WidgetsWork.DrawWorkBoxForSquad(topLeft, wType, this.squadPriorities, this.pawns);
				num += this.workColumnSpacing;
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
			Rect rect2 = new Rect(0f, 0f, 175f, 30f);
			rect2.xMin += 15f;
			GUI.color = new Color(1f, 1f, 1f, 1f);
			if (base.SquadFilteringEnabled && SquadManager.Instance.SquadFilter != null)
			{
				Widgets.Label(rect2, SquadManager.Instance.SquadFilter.Name);
			}
			else
			{
				Widgets.Label(rect2, "EdB.Squads.AllColonistsSquadName".Translate());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			GUI.EndGroup();
		}

		protected override void DrawPawnRow(Rect rect, Pawn p)
		{
			float num = 175f;
			Text.Font = GameFont.Medium;
			for (int i = 0; i < MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count; i++)
			{
				WorkTypeDef workTypeDef = MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder[i];
				Vector2 topLeft = new Vector2(num, rect.y + 2.5f);
				WidgetsWork.DrawWorkBoxFor(topLeft, p, workTypeDef);
				Rect rect2 = new Rect(topLeft.x, topLeft.y, 25f, 25f);
				TooltipHandler.TipRegion(rect2, WidgetsWork.TipForPawnWorker(p, workTypeDef));
				num += this.workColumnSpacing;
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			MainTabWindow_Work.Reinit();
			this.squadPriorities.Reset();
		}
	}
}
