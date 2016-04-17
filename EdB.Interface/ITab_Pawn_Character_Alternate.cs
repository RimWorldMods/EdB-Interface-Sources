using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Character_Alternate : ITab_Pawn_Character
	{
		private static readonly Vector2 PaddingSize = new Vector2(26f, 20f);

		private static readonly Vector2 PanelSize = TabDrawer.TabPanelSize;

		private static readonly Vector2 ContentSize = new Vector2(ITab_Pawn_Character_Alternate.PanelSize.x - ITab_Pawn_Character_Alternate.PaddingSize.x * 2f, ITab_Pawn_Character_Alternate.PanelSize.y - ITab_Pawn_Character_Alternate.PaddingSize.y * 2f);

		protected SelectorUtility pawnSelector = new SelectorUtility();

		protected float StandardSkillCount = 12f;

		protected float HeightWithoutSkills = 362f;

		protected SkillDrawer skillDrawer = new SkillDrawer();

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (base.SelPawn != null)
				{
					pawn = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.innerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		public ITab_Pawn_Character_Alternate(PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.size = TabDrawer.TabPanelSize;
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		protected override void FillTab()
		{
			Pawn pawnToShowInfoAbout = this.PawnToShowInfoAbout;
			try
			{
				GUI.BeginGroup(new Rect(ITab_Pawn_Character_Alternate.PaddingSize.x, ITab_Pawn_Character_Alternate.PaddingSize.y, ITab_Pawn_Character_Alternate.ContentSize.x, ITab_Pawn_Character_Alternate.ContentSize.y));
				float num = 0f;
				float num2 = 12f;
				bool allowRename = !pawnToShowInfoAbout.Dead && !pawnToShowInfoAbout.Destroyed;
				num += TabDrawer.DrawNameAndBasicInfo(0f, num, this.PawnToShowInfoAbout, ITab_Pawn_Character_Alternate.ContentSize.x, allowRename);
				num += num2;
				num += TabDrawer.DrawHeader(0f, num, ITab_Pawn_Character_Alternate.ContentSize.x, "Backstory".Translate(), true, TextAnchor.UpperLeft);
				num += 2f;
				Text.Font = GameFont.Small;
				GUI.color = TabDrawer.TextColor;
				Vector2 vector = new Vector2(ITab_Pawn_Character_Alternate.ContentSize.x, 24f);
				Vector2 vector2 = new Vector2(90f, 24f);
				Vector2 vector3 = new Vector2(3f, 2f);
				int num3 = 0;
				IEnumerator enumerator = Enum.GetValues(typeof(BackstorySlot)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						BackstorySlot backstorySlot = (BackstorySlot)enumerator.Current;
						Rect rect = new Rect(0f, num, vector.x, vector.y);
						if (rect.Contains(Event.current.mousePosition))
						{
							Widgets.DrawHighlight(rect);
						}
						TooltipHandler.TipRegion(rect, pawnToShowInfoAbout.story.GetBackstory(backstorySlot).FullDescriptionFor(pawnToShowInfoAbout));
						rect.x += vector3.x;
						rect.width -= vector3.x * 2f;
						rect.y += vector3.y;
						GUI.skin.label.alignment = TextAnchor.MiddleLeft;
						string str = (backstorySlot == BackstorySlot.Adulthood) ? "Adulthood".Translate() : "Childhood".Translate();
						Widgets.Label(rect, str + ":");
						GUI.skin.label.alignment = TextAnchor.UpperLeft;
						Rect rect2 = new Rect(rect.x + vector2.x, rect.y, vector.x - vector2.x, vector.y);
						string title = pawnToShowInfoAbout.story.GetBackstory(backstorySlot).title;
						Widgets.Label(rect2, title);
						num += vector.y + 2f;
						num3++;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				num -= 6f;
				num += num2;
				num += TabDrawer.DrawHeader(0f, num, ITab_Pawn_Character_Alternate.ContentSize.x, "Skills".Translate(), true, TextAnchor.UpperLeft);
				num += 6f;
				float height = (float)DefDatabase<SkillDef>.AllDefs.Count<SkillDef>() * 24f;
				Rect position = new Rect(0f, num, 390f, height);
				GUI.color = TabDrawer.TextColor;
				GUI.BeginGroup(position);
				this.skillDrawer.DrawSkillsOf(pawnToShowInfoAbout, new Vector2(0f, 0f));
				GUI.EndGroup();
				num += position.height;
				num += num2;
				float num4 = num;
				float num5 = ITab_Pawn_Character_Alternate.ContentSize.x * 0.4f - ITab_Pawn_Character_Alternate.PaddingSize.x / 2f;
				float num6 = ITab_Pawn_Character_Alternate.ContentSize.x * 0.6f - ITab_Pawn_Character_Alternate.PaddingSize.x / 2f;
				float left = num5 + ITab_Pawn_Character_Alternate.PaddingSize.x;
				num += TabDrawer.DrawHeader(0f, num, num5, "IncapableOf".Translate(), true, TextAnchor.UpperLeft);
				num += 4f;
				Vector2 vector4 = new Vector2(num5, 22f);
				Text.Font = GameFont.Small;
				GUI.color = TabDrawer.TextColor;
				List<WorkTags> list = pawnToShowInfoAbout.story.DisabledWorkTags.ToList<WorkTags>();
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				if (list.Count == 0)
				{
					Rect rect3 = new Rect(0f, num, num5, vector4.y);
					GUI.color = TabDrawer.SeparatorColor;
					Widgets.Label(rect3, "NoneLower".Translate());
					GUI.color = TabDrawer.TextColor;
					num += rect3.height - 1f;
				}
				else
				{
					foreach (WorkTags current in list)
					{
						Rect rect4 = new Rect(0f, num, num5, vector4.y);
						Widgets.Label(rect4, current.LabelTranslated());
						num += vector4.y - 1f;
					}
				}
				num = num4;
				num += TabDrawer.DrawHeader(left, num, num6, "Traits".Translate(), true, TextAnchor.UpperLeft);
				num += 4f;
				Vector2 vector5 = new Vector2(num6, 22f);
				Text.Font = GameFont.Small;
				GUI.color = TabDrawer.TextColor;
				float num7 = 0f;
				foreach (Trait current2 in pawnToShowInfoAbout.story.traits.allTraits)
				{
					num7 += vector5.y + 2f;
					Rect rect5 = new Rect(left, num, num6, vector5.y);
					if (rect5.Contains(Event.current.mousePosition))
					{
						Widgets.DrawHighlight(rect5);
					}
					rect5.x += 2f;
					Widgets.Label(rect5, current2.LabelCap);
					TooltipHandler.TipRegion(rect5, current2.TipString(pawnToShowInfoAbout));
					num += vector5.y - 1f;
				}
			}
			finally
			{
				GUI.EndGroup();
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value && pawnToShowInfoAbout != null)
			{
				BrowseButtonDrawer.DrawBrowseButtons(this.size, pawnToShowInfoAbout);
			}
			GUI.color = Color.white;
		}
	}
}
