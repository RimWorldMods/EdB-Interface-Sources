using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class AreaAllowedGUI
	{
		public static void DoAllowedAreaSelectors(Rect rect, Pawn p, AllowedAreaMode mode)
		{
			List<Area> allAreas = Find.AreaManager.AllAreas;
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++)
			{
				if (allAreas[i].AssignableAsAllowed(mode))
				{
					num++;
				}
			}
			float num2 = rect.width / (float)num;
			Text.WordWrap = false;
			Text.Font = GameFont.Tiny;
			Rect rect2 = new Rect(rect.x, rect.y, num2, rect.height);
			AreaAllowedGUI.DoAreaSelector(rect2, p, null);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].AssignableAsAllowed(mode))
				{
					float num4 = (float)num3 * num2;
					Rect rect3 = new Rect(rect.x + num4, rect.y, num2, rect.height);
					AreaAllowedGUI.DoAreaSelector(rect3, p, allAreas[j]);
					num3++;
				}
			}
			Text.WordWrap = true;
		}

		public static void DoAllowedAreaSelectors(Rect rect, ref Area areaRestriction, AllowedAreaMode mode, IEnumerable<Pawn> pawns)
		{
			List<Area> allAreas = Find.AreaManager.AllAreas;
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++)
			{
				if (allAreas[i].AssignableAsAllowed(mode))
				{
					num++;
				}
			}
			float num2 = rect.width / (float)num;
			Text.WordWrap = false;
			Text.Font = GameFont.Tiny;
			Rect rect2 = new Rect(rect.x, rect.y, num2, rect.height);
			AreaAllowedGUI.DoAreaSelector(rect2, ref areaRestriction, null, pawns);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].AssignableAsAllowed(mode))
				{
					float num4 = (float)num3 * num2;
					Rect rect3 = new Rect(rect.x + num4, rect.y, num2, rect.height);
					AreaAllowedGUI.DoAreaSelector(rect3, ref areaRestriction, allAreas[j], pawns);
					num3++;
				}
			}
			Text.WordWrap = true;
		}

		private static void DoAreaSelector(Rect rect, ref Area areaRestriction, Area area, IEnumerable<Pawn> pawns)
		{
			rect = rect.ContractedBy(1f);
			GUI.DrawTexture(rect, (area != null) ? area.ColorTexture : BaseContent.GreyTex);
			Text.Anchor = TextAnchor.MiddleLeft;
			string text = AreaUtility.AreaAllowedLabel_Area(area);
			Rect rect2 = rect;
			rect2.xMin += 3f;
			rect2.yMin += 2f;
			Widgets.Label(rect2, text);
			if (areaRestriction == area)
			{
				Widgets.DrawBox(rect, 2);
			}
			if (Mouse.IsOver(rect))
			{
				if (area != null)
				{
					area.MarkForDraw();
				}
				if (Input.GetMouseButton(0))
				{
					areaRestriction = area;
					bool flag = false;
					foreach (Pawn current in pawns)
					{
						if (current.playerSettings.AreaRestriction != area)
						{
							flag = true;
						}
						current.playerSettings.AreaRestriction = area;
					}
					if (flag)
					{
						SoundDefOf.DesignateDragStandardChanged.PlayOneShotOnCamera();
					}
				}
			}
			TooltipHandler.TipRegion(rect, text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private static void DoAreaSelector(Rect rect, Pawn p, Area area)
		{
			rect = rect.ContractedBy(1f);
			GUI.DrawTexture(rect, (area != null) ? area.ColorTexture : BaseContent.GreyTex);
			Text.Anchor = TextAnchor.MiddleLeft;
			string text = AreaUtility.AreaAllowedLabel_Area(area);
			Rect rect2 = rect;
			rect2.xMin += 3f;
			rect2.yMin += 2f;
			Widgets.Label(rect2, text);
			if (p.playerSettings.AreaRestriction == area)
			{
				Widgets.DrawBox(rect, 2);
			}
			if (Mouse.IsOver(rect))
			{
				if (area != null)
				{
					area.MarkForDraw();
				}
				if (Input.GetMouseButton(0) && p.playerSettings.AreaRestriction != area)
				{
					p.playerSettings.AreaRestriction = area;
					SoundDefOf.DesignateDragStandardChanged.PlayOneShotOnCamera();
				}
			}
			TooltipHandler.TipRegion(rect, text);
		}
	}
}
