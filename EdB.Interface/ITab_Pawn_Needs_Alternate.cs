using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Needs_Alternate : ITab_Pawn_Needs
	{
		private const float ThoughtHeight = 20f;

		private const float MoodNumberWidth = 32f;

		private const float MoodX = 235f;

		private const float ThoughtIntervalY = 24f;

		private const float NeedsColumnWidth = 225f;

		private const float ThoughtSpacing = 4f;

		private static List<ThoughtDef> thoughtTypesPresent;

		private static readonly Color NoEffectColor;

		private static readonly Color MoodColorNegative;

		private static readonly Color MoodColor;

		private static readonly Vector2 FullSize;

		public static Texture2D BarInstantMarkerTex;

		protected FieldInfo threshPercentsField;

		private Vector2 thoughtScrollPosition = default(Vector2);

		private List<Need> displayNeeds = new List<Need>();

		private List<float> threshPercentsForMood = new List<float>();

		public static float ThoughtValueWidth;

		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Needs_Alternate(PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabNeeds";
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = TabDrawer.TabPanelSize;
			this.threshPercentsField = typeof(Need).GetField("threshPercents", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		static ITab_Pawn_Needs_Alternate()
		{
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent = new List<ThoughtDef>();
			ITab_Pawn_Needs_Alternate.NoEffectColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);
			ITab_Pawn_Needs_Alternate.MoodColorNegative = new Color(0.8f, 0.4f, 0.4f);
			ITab_Pawn_Needs_Alternate.MoodColor = new Color(0.1f, 1f, 0.1f);
			ITab_Pawn_Needs_Alternate.FullSize = new Vector2(580f, 520f);
			ITab_Pawn_Needs_Alternate.ThoughtValueWidth = 40f;
			ITab_Pawn_Needs_Alternate.BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);
		}

		protected new void DoMoodAndThoughts(Rect rect)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width, 70f);
			this.DrawMood(rect2, base.SelPawn, base.SelPawn.needs.mood);
			Rect rect3 = new Rect(0f, 80f, rect.width, rect.height - 120f);
			rect3 = rect3.ContractedBy(10f);
			rect3.x += 10f;
			rect3.width -= 20f;
			this.DrawThoughtListing(rect3);
			GUI.EndGroup();
		}

		protected void DrawNeed(Rect rect, Need need)
		{
			List<float> threshPercents = (List<float>)this.threshPercentsField.GetValue(need);
			this.DrawNeed(rect, need, threshPercents);
		}

		protected void DrawMood(Rect rect, Pawn pawn, Need_Mood mood)
		{
			this.threshPercentsForMood.Clear();
			this.threshPercentsForMood.Add(pawn.mindState.breaker.HardBreakThreshold);
			this.threshPercentsForMood.Add(pawn.mindState.breaker.SoftBreakThreshold);
			this.DrawNeed(rect, mood, this.threshPercentsForMood);
		}

		private float DoNeeds(Rect rect)
		{
			this.displayNeeds.Clear();
			List<Need> allNeeds = base.SelPawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].def.showOnNeedList)
				{
					this.displayNeeds.Add(allNeeds[i]);
				}
			}
			this.displayNeeds.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
			float num = 14f;
			float num2 = num;
			int num3 = this.displayNeeds.Count / 2;
			int num4 = this.displayNeeds.Count - num3;
			int num5 = (num3 <= num4) ? num4 : num3;
			float num6 = 0f;
			for (int j = 0; j < num3; j++)
			{
				Need need = this.displayNeeds[j];
				Rect rect2 = new Rect(rect.x, rect.y + num2, rect.width, Mathf.Min(90f, rect.height / (float)num5));
				this.DrawNeed(rect2, need);
				num2 = rect2.yMax - 10f;
				if (num2 > num6)
				{
					num6 = num2;
				}
			}
			float left = rect.width + 4f;
			num2 = num;
			for (int k = num3; k < this.displayNeeds.Count; k++)
			{
				Need need2 = this.displayNeeds[k];
				Rect rect3 = new Rect(left, rect.y + num2, rect.width, Mathf.Min(90f, rect.height / (float)num5));
				this.DrawNeed(rect3, need2);
				num2 = rect3.yMax - 10f;
				if (num2 > num6)
				{
					num6 = num2;
				}
			}
			return num6;
		}

		public void DrawNeed(Rect rect, Need need, List<float> threshPercents)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			TooltipHandler.TipRegion(rect, new TipSignal(() => need.GetTipString(), rect.GetHashCode()));
			float num2 = 14f;
			float num3 = num2 + 15f;
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = ((rect.height > 55f) ? GameFont.Small : GameFont.Tiny);
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
			Widgets.Label(rect2, need.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
			Widgets.FillableBar(rect3, need.CurLevel);
			Widgets.FillableBarChangeArrows(rect3, need.GUIChangeArrow);
			if (threshPercents != null)
			{
				for (int i = 0; i < threshPercents.Count; i++)
				{
					this.DrawBarThreshold(rect3, threshPercents[i], need);
				}
			}
			float curInstantLevel = need.CurInstantLevel;
			if (curInstantLevel >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect3, curInstantLevel, need);
			}
			Text.Font = GameFont.Small;
		}

		protected void DrawBarInstantMarkerAt(Rect barRect, float pct, Need need)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(need.def + " drawing bar percent > 1 : " + pct, 6932178);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			Rect position = new Rect(vector.x - num / 2f, vector.y, num, num);
			GUI.DrawTexture(position, ITab_Pawn_Needs_Alternate.BarInstantMarkerTex);
		}

		private void DrawBarThreshold(Rect barRect, float threshPct, Need need)
		{
			float num = (float)((barRect.width > 60f) ? 2 : 1);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < need.CurLevel)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		private bool DrawThoughtGroup(Rect rect, ThoughtDef def)
		{
			float num = 12f;
			float num2 = 4f;
			float num3 = rect.width - num - num2 - ITab_Pawn_Needs_Alternate.ThoughtValueWidth - 16f;
			float num4 = num + num3;
			try
			{
				List<Thought> list = base.SelPawn.needs.mood.thoughts.ThoughtsOfDef(def).ToList<Thought>();
				int index = 0;
				int num5 = -1;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].CurStageIndex > num5)
					{
						num5 = list[i].CurStageIndex;
						index = i;
					}
				}
				if (!list[index].Visible)
				{
					return false;
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (def.DurationTicks > 5)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(list[index].Description);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					Thought_Memory thought_Memory = list[index] as Thought_Memory;
					if (thought_Memory != null)
					{
						if (list.Count == 1)
						{
							stringBuilder.Append("ThoughtExpiresIn".Translate(new object[]
							{
								(def.DurationTicks - thought_Memory.age).TickstoDaysString()
							}));
						}
						else
						{
							Thought_Memory thought_Memory2 = (Thought_Memory)list[list.Count - 1];
							stringBuilder.Append("ThoughtStartsExpiringIn".Translate(new object[]
							{
								(def.DurationTicks - thought_Memory.age).TickstoDaysString()
							}));
							stringBuilder.AppendLine();
							stringBuilder.Append("ThoughtFinishesExpiringIn".Translate(new object[]
							{
								(def.DurationTicks - thought_Memory2.age).TickstoDaysString()
							}));
						}
					}
					TooltipHandler.TipRegion(rect, new TipSignal(stringBuilder.ToString(), 7291));
				}
				else
				{
					TooltipHandler.TipRegion(rect, new TipSignal(list[index].Description, 7141));
				}
				Text.WordWrap = false;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(rect.x + num, rect.y, num3, rect.height);
				rect2.yMin -= 3f;
				rect2.yMax += 3f;
				string text = list[index].LabelCap;
				if (list.Count > 1)
				{
					text = text + " x" + list.Count;
				}
				Widgets.Label(rect2, text);
				Text.Anchor = TextAnchor.MiddleCenter;
				float num6 = base.SelPawn.needs.mood.thoughts.MoodOffsetOfThoughtGroup(def);
				if (num6 == 0f)
				{
					GUI.color = ITab_Pawn_Needs_Alternate.NoEffectColor;
				}
				else if (num6 > 0f)
				{
					GUI.color = ITab_Pawn_Needs_Alternate.MoodColor;
				}
				else
				{
					GUI.color = ITab_Pawn_Needs_Alternate.MoodColorNegative;
				}
				Rect rect3 = new Rect(rect.x + num4, rect.y, ITab_Pawn_Needs_Alternate.ThoughtValueWidth, rect.height);
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect3, num6.ToString("##0"));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.WordWrap = true;
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Exception in DrawThoughtGroup for ",
					def,
					" on ",
					base.SelPawn,
					": ",
					ex.ToString()
				}), 3452698);
			}
			return true;
		}

		private void DrawThoughtListing(Rect listingRect)
		{
			Text.Font = GameFont.Small;
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Clear();
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent.AddRange(from th in base.SelPawn.needs.mood.thoughts.DistinctThoughtDefs
			orderby base.SelPawn.needs.mood.thoughts.MoodOffsetOfThoughtGroup(th) descending
			select th);
			float height = (float)ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Count * 24f;
			Widgets.BeginScrollView(listingRect, ref this.thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 16f, height));
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			for (int i = 0; i < ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Count; i++)
			{
				Rect rect = new Rect(0f, num, listingRect.width, 20f);
				if (this.DrawThoughtGroup(rect, ITab_Pawn_Needs_Alternate.thoughtTypesPresent[i]))
				{
					num += 24f;
				}
			}
			Widgets.EndScrollView();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		protected override void FillTab()
		{
			Rect position = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(1f);
			try
			{
				GUI.BeginGroup(position);
				Rect rect = new Rect(0f, 0f, position.width / 2f, this.size.y);
				float num = this.DoNeeds(rect);
				num += 10f;
				if (base.SelPawn.needs.mood != null)
				{
					Rect rect2 = new Rect(0f, num, position.width, position.height - num);
					this.DoMoodAndThoughts(rect2);
				}
			}
			finally
			{
				GUI.EndGroup();
				GUI.color = Color.white;
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value)
			{
				Pawn selPawn = base.SelPawn;
				if (selPawn != null)
				{
					BrowseButtonDrawer.DrawBrowseButtons(this.size, selPawn);
				}
			}
		}

		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
			this.size = TabDrawer.TabPanelSize;
		}

		private void UpdateDisplayNeeds()
		{
			this.displayNeeds.Clear();
			List<Need> allNeeds = base.SelPawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].def.showOnNeedList)
				{
					this.displayNeeds.Add(allNeeds[i]);
				}
			}
			this.displayNeeds.Sort((Need a, Need b) => b.def.listPriority.CompareTo(a.def.listPriority));
		}

		protected override void UpdateSize()
		{
			this.UpdateDisplayNeeds();
			if (base.SelPawn.needs.mood != null)
			{
				this.size = TabDrawer.TabPanelSize;
			}
			else
			{
				this.size = new Vector2(TabDrawer.TabPanelSize.x, (float)this.displayNeeds.Count * Mathf.Min(70f, ITab_Pawn_Needs_Alternate.FullSize.y / (float)this.displayNeeds.Count) + 16f);
			}
		}
	}
}
