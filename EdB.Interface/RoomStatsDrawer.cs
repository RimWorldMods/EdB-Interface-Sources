using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal static class RoomStatsDrawer
	{
		private sealed class Anonymous
		{
			internal Rect windowRect;

			internal Room room;

			internal void Iterate()
			{
				ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.GuiFrame);
				Text.Font = GameFont.Small;
				float num = 19f;
				Rect rect = new Rect(19f, num, this.windowRect.width - 38f, 100f);
				GUI.color = Color.white;
				Widgets.Label(rect, RoomStatsDrawer.GetRoomRoleLabel(this.room));
				num += 25f;
				for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
				{
					RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
					if (!roomStatDef.isHidden)
					{
						float stat = this.room.GetStat(roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
						if (this.room.Role.IsStatRelated(roomStatDef))
						{
							GUI.color = RoomStatsDrawer.RelatedStatColor;
						}
						else
						{
							GUI.color = Color.gray;
						}
						Rect rect2 = new Rect(rect.x, num, RoomStatsDrawer.statLabelColumnWidth, 23f);
						Widgets.Label(rect2, roomStatDef.LabelCap);
						Rect rect3 = new Rect(rect2.xMax + 35f, num, RoomStatsDrawer.scoreColumnWidth, 23f);
						string label;
						if (roomStatDef.displayRounded)
						{
							label = Mathf.RoundToInt(stat).ToString();
						}
						else
						{
							label = stat.ToString("0.##");
						}
						Widgets.Label(rect3, label);
						Rect rect4 = new Rect(rect3.xMax + 35f, num, RoomStatsDrawer.scoreStageLabelColumnWidth, 23f);
						Widgets.Label(rect4, scoreStage.label);
						num += 25f;
					}
				}
				GUI.color = Color.white;
			}
		}

		private const int WindowPadding = 19;

		private const int LineHeight = 23;

		private const int SpaceBetweenLines = 2;

		private const float SpaceBetweenColumns = 35f;

		private static float statLabelColumnWidth = 0f;

		private static float scoreColumnWidth = 0f;

		private static float scoreStageLabelColumnWidth = 0f;

		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden)
					{
						num++;
					}
				}
				return num;
			}
		}

		private static bool ShouldShowRoomStats
		{
			get
			{
				if (!Find.PlaySettings.showRoomStats)
				{
					return false;
				}
				if (Mouse.IsInputBlockedNow)
				{
					return false;
				}
				if (!Gen.MouseCell().InBounds() || Gen.MouseCell().Fogged())
				{
					return false;
				}
				Room room = Gen.MouseCell().GetRoom();
				return room != null && room.Role != RoomRoleDefOf.None;
			}
		}

		private static void CalculateColumnsSizes(Room room)
		{
			RoomStatsDrawer.statLabelColumnWidth = 0f;
			RoomStatsDrawer.scoreColumnWidth = 0f;
			RoomStatsDrawer.scoreStageLabelColumnWidth = 0f;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden)
				{
					RoomStatsDrawer.statLabelColumnWidth = Mathf.Max(RoomStatsDrawer.statLabelColumnWidth, Text.CalcSize(roomStatDef.LabelCap).x);
					float stat = room.GetStat(roomStatDef);
					string label = roomStatDef.GetScoreStage(stat).label;
					RoomStatsDrawer.scoreStageLabelColumnWidth = Mathf.Max(RoomStatsDrawer.scoreStageLabelColumnWidth, Text.CalcSize(label).x);
					string text;
					if (roomStatDef.displayRounded)
					{
						text = Mathf.RoundToInt(stat).ToString();
					}
					else
					{
						text = stat.ToString("0.##");
					}
					RoomStatsDrawer.scoreColumnWidth = Mathf.Max(RoomStatsDrawer.scoreColumnWidth, Text.CalcSize(text).x);
				}
			}
			RoomStatsDrawer.scoreColumnWidth = Mathf.Max(RoomStatsDrawer.scoreColumnWidth, 40f);
		}

		public static void DrawRoomOverlays()
		{
			if (!RoomStatsDrawer.ShouldShowRoomStats)
			{
				return;
			}
			Room room = Gen.MouseCell().GetRoom();
			room.DrawFieldEdges();
		}

		private static string GetRoomRoleLabel(Room room)
		{
			Pawn owner = room.Owner;
			string result;
			if (owner != null)
			{
				result = "SomeonesSomething".Translate(new object[]
				{
					owner.NameStringShort,
					room.Role.label
				});
			}
			else
			{
				result = room.Role.LabelCap;
			}
			return result;
		}

		public static void RoomStatsOnGUI()
		{
			RoomStatsDrawer.Anonymous temp = new RoomStatsDrawer.Anonymous();
			if (!RoomStatsDrawer.ShouldShowRoomStats)
			{
				return;
			}
			temp.room = Gen.MouseCell().GetRoom();
			Text.Font = GameFont.Small;
			RoomStatsDrawer.CalculateColumnsSizes(temp.room);
			temp.windowRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 108f + RoomStatsDrawer.statLabelColumnWidth + RoomStatsDrawer.scoreColumnWidth + RoomStatsDrawer.scoreStageLabelColumnWidth, (float)(65 + RoomStatsDrawer.DisplayedRoomStatsCount * 25));
			RoomStatsDrawer.Anonymous temp5 = temp;
			temp5.windowRect.x = temp5.windowRect.x + 5f;
			RoomStatsDrawer.Anonymous temp2 = temp;
			temp2.windowRect.y = temp2.windowRect.y + 5f;
			if (temp.windowRect.xMax > (float)Screen.width)
			{
				RoomStatsDrawer.Anonymous temp3 = temp;
				temp3.windowRect.x = temp3.windowRect.x - (temp.windowRect.width + 10f);
			}
			if (temp.windowRect.yMax > (float)Screen.height)
			{
				RoomStatsDrawer.Anonymous temp4 = temp;
				temp4.windowRect.y = temp4.windowRect.y - (temp.windowRect.height + 10f);
			}
			Find.WindowStack.ImmediateWindow(74975, temp.windowRect, WindowLayer.Super, delegate
			{
				ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.InspectRoomStats, KnowledgeAmount.GuiFrame);
				Text.Font = GameFont.Small;
				float num = 19f;
				Rect rect = new Rect(19f, num, temp.windowRect.width - 38f, 100f);
				GUI.color = Color.white;
				Widgets.Label(rect, RoomStatsDrawer.GetRoomRoleLabel(temp.room));
				num += 25f;
				for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
				{
					RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
					if (!roomStatDef.isHidden)
					{
						float stat = temp.room.GetStat(roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
						if (temp.room.Role.IsStatRelated(roomStatDef))
						{
							GUI.color = RoomStatsDrawer.RelatedStatColor;
						}
						else
						{
							GUI.color = Color.gray;
						}
						Rect rect2 = new Rect(rect.x, num, RoomStatsDrawer.statLabelColumnWidth, 23f);
						Widgets.Label(rect2, roomStatDef.LabelCap);
						Rect rect3 = new Rect(rect2.xMax + 35f, num, RoomStatsDrawer.scoreColumnWidth, 23f);
						string label;
						if (roomStatDef.displayRounded)
						{
							label = Mathf.RoundToInt(stat).ToString();
						}
						else
						{
							label = stat.ToString("0.##");
						}
						Widgets.Label(rect3, label);
						Rect rect4 = new Rect(rect3.xMax + 35f, num, RoomStatsDrawer.scoreStageLabelColumnWidth, 23f);
						Widgets.Label(rect4, scoreStage.label);
						num += 25f;
					}
				}
				GUI.color = Color.white;
			}, true, false, 1f);
		}
	}
}
