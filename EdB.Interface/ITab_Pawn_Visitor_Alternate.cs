using RimWorld;
using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class ITab_Pawn_Visitor_Alternate : ITab_Pawn_Visitor
	{
		private const float CheckboxInterval = 30f;

		private const float CheckboxMargin = 50f;

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Visitor_Alternate()
		{
			this.size = new Vector2(TabDrawer.TabPanelSize.x, TabDrawer.TabPanelSize.y);
		}

		protected override void FillTab()
		{
			ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.GuiFrame);
			Text.Font = GameFont.Small;
			Rect position = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(20f);
			bool isPrisonerOfColony = base.SelPawn.IsPrisonerOfColony;
			try
			{
				GUI.BeginGroup(position);
				float num = 10f;
				Rect rect = new Rect(10f, num, position.width - 20f, 32f);
				MedicalCareUtility.MedicalCareSetter(rect, ref base.SelPawn.playerSettings.medCare);
				num += 32f;
				num += 18f;
				Rect rect2 = new Rect(10f, num, position.width - 28f, position.height - num);
				bool getsFood = base.SelPawn.guest.GetsFood;
				num += WidgetDrawer.DrawLabeledCheckbox(rect2, "GetsFood".Translate(), ref getsFood);
				base.SelPawn.guest.GetsFood = getsFood;
				if (isPrisonerOfColony)
				{
					num += 6f;
					int length = Enum.GetValues(typeof(PrisonerInteractionMode)).Length;
					float height = (float)(length * 28 + 20);
					Rect rect3 = new Rect(0f, num, position.width, height);
					TabDrawer.DrawBox(rect3);
					Rect rect4 = rect3.ContractedBy(10f);
					rect4.height = 28f;
					using (IEnumerator enumerator = Enum.GetValues(typeof(PrisonerInteractionMode)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PrisonerInteractionMode prisonerInteractionMode = (PrisonerInteractionMode)((byte)enumerator.Current);
							if (WidgetDrawer.DrawLabeledRadioButton(rect4, prisonerInteractionMode.GetLabel(), base.SelPawn.guest.interactionMode == prisonerInteractionMode, true))
							{
								base.SelPawn.guest.interactionMode = prisonerInteractionMode;
							}
							rect4.y += 28f;
						}
					}
					Rect rect5 = new Rect(rect3.x, rect3.y + rect3.height + 5f, rect3.width - 4f, 28f);
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect5, "RecruitmentDifficulty".Translate() + ": " + base.SelPawn.guest.RecruitDifficulty.ToString("##0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			finally
			{
				GUI.EndGroup();
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
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
	}
}
