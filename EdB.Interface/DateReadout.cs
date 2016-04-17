using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public static class DateReadout
	{
		public const float Height = 22f;

		private static string dateString;

		private static int dateStringDay = -1;

		private static readonly List<string> fastHourStrings = new List<string>();

		private static readonly float TicksPerMinute = 20.833334f;

		public static void DateOnGUI(Rect dateRect)
		{
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			GUI.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = dateRect.width * 1f / 6f;
			Text.Font = GameFont.Small;
			Rect rect = new Rect(num - 50f, 0f, 100f, 22f);
			Widgets.Label(rect, DateReadout.fastHourStrings[GenDate.HourInt]);
			float num2 = dateRect.width * 3f / 6f;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(num2 - 50f, 0f, 100f, 22f);
			if (GenDate.DayOfMonth != DateReadout.dateStringDay)
			{
				DateReadout.dateString = GenDate.CurrentMonthDateShortString;
				DateReadout.dateStringDay = GenDate.DayOfMonth;
			}
			Widgets.Label(rect2, DateReadout.dateString);
			float num3 = dateRect.width * 5f / 6f;
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(num3 - 50f, 0f, 100f, 22f);
			Widgets.Label(rect3, GenDate.CurrentYear.ToStringCached());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			TooltipHandler.TipRegion(dateRect, new TipSignal(() => "DateReadoutTip".Translate(new object[]
			{
				GenDate.DaysPassed,
				10,
				GenDate.CurrentSeason.Label()
			}), 86423));
		}

		public static void AlternateDateOnGUI(Rect dateRect, bool amPm, int minuteInterval)
		{
			if (Mouse.IsOver(dateRect))
			{
				Widgets.DrawHighlight(dateRect);
			}
			GUI.BeginGroup(dateRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleRight;
			float num = 9f;
			float left = dateRect.width * 0f / num;
			Text.Font = GameFont.Small;
			float num2 = dateRect.width / num;
			Rect rect = new Rect(left, 0f, num2 * 4.25f, 22f);
			int num3 = GenDate.HourInt;
			int num4 = (Find.TickManager.TicksGame + Find.TickManager.gameStartAbsTick) % 30000;
			int num5 = (int)((float)(num4 % 1250) / DateReadout.TicksPerMinute) / minuteInterval * minuteInterval;
			string text = string.Empty;
			if (amPm)
			{
				text = " " + ((num3 >= 12) ? "PM" : "AM");
				if (num3 == 0)
				{
					num3 = 12;
				}
				else if (num3 > 12)
				{
					num3 -= 12;
				}
			}
			Widgets.Label(rect, string.Concat(new object[]
			{
				(num3 >= 10) ? null : "0",
				num3,
				":",
				(num5 >= 10) ? null : "0",
				num5,
				text
			}));
			float num6 = dateRect.width * 3f / num;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(num6 - 8f, 0f, num2 * 4f, 22f);
			if (GenDate.DayOfMonth != DateReadout.dateStringDay)
			{
				DateReadout.dateString = GenDate.CurrentMonthDateShortString;
				DateReadout.dateStringDay = GenDate.DayOfMonth;
			}
			Widgets.Label(rect2, DateReadout.dateString);
			float num7 = dateRect.width * 7f / num;
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(num7 - 8f, 0f, num2 * 2f, 22f);
			Widgets.Label(rect3, GenDate.CurrentYear.ToStringCached());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			TooltipHandler.TipRegion(dateRect, new TipSignal(() => "DateReadoutTip".Translate(new object[]
			{
				GenDate.DaysPassed,
				10,
				GenDate.CurrentSeason.Label()
			}), 86423));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static void Reinit()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.fastHourStrings.Clear();
			for (int i = 0; i < 24; i++)
			{
				DateReadout.fastHourStrings.Add(i + "LetterHour".Translate());
			}
		}
	}
}
