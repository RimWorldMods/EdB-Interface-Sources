using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Health_Alternate : ITab_Pawn_Health
	{
		private const float ThoughtLevelHeight = 25f;

		private const float ThoughtLevelSpacing = 20f;

		private const float topPadding = 20f;

		private const int HideBloodLossTicksThreshold = 60000;

		private static Texture2D TabAtlasTex;

		private static readonly Color highlightColor;

		private static readonly Color staticHighlightColor;

		private bool highlight = true;

		private bool operationsTabSelected;

		private static readonly Vector2 PanelSize;

		protected HealthCardUtility healthCardUtility = new HealthCardUtility();

		protected ScrollView operationsScrollView = new ScrollView(false);

		protected ScrollView injuriesScrollView = new ScrollView(false);

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		private bool HideBloodLoss
		{
			get
			{
				return this.SelCorpse != null && this.SelCorpse.Age > 60000;
			}
		}

		private Corpse SelCorpse
		{
			get
			{
				return base.SelThing as Corpse;
			}
		}

		private Pawn SelPawnForHealth
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.innerPawn;
				}
				throw new InvalidOperationException("Health tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		public ITab_Pawn_Health_Alternate(PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.size = ITab_Pawn_Health_Alternate.PanelSize;
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
		}

		static ITab_Pawn_Health_Alternate()
		{
			ITab_Pawn_Health_Alternate.TabAtlasTex = null;
			ITab_Pawn_Health_Alternate.highlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);
			ITab_Pawn_Health_Alternate.staticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);
			ITab_Pawn_Health_Alternate.PanelSize = TabDrawer.TabPanelSize;
			ITab_Pawn_Health_Alternate.ResetTextures();
		}

		public static void ResetTextures()
		{
			ITab_Pawn_Health_Alternate.TabAtlasTex = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/TabAtlas", true);
		}

		private void DoRightRowHighlight(Rect rowRect)
		{
			if (this.highlight)
			{
				GUI.color = ITab_Pawn_Health_Alternate.staticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			this.highlight = !this.highlight;
			if (rowRect.Contains(Event.current.mousePosition))
			{
				GUI.color = ITab_Pawn_Health_Alternate.highlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
		}

		protected override void FillTab()
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
				Log.Error("Health tab found no selected pawn to display.");
				return;
			}
			Corpse corpse2 = base.SelThing as Corpse;
			bool showBloodLoss = corpse2 == null || corpse2.Age < 60000;
			bool flag = !pawn.RaceProps.Humanlike && pawn.Downed;
			bool flag2 = base.SelThing.def.AllRecipes.Any<RecipeDef>();
			bool flag3 = flag2 && !pawn.Dead && (pawn.IsColonist || pawn.IsPrisonerOfColony || flag);
			TextAnchor anchor = Text.Anchor;
			Rect rect = new Rect(20f, 51f, ITab_Pawn_Health_Alternate.PanelSize.x - 40f, 345f);
			float height = this.size.y - rect.height - 109f;
			Rect position = new Rect(rect.x, rect.y + rect.height + 16f, rect.width, height);
			if (!flag3)
			{
				this.operationsTabSelected = false;
			}
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("HealthOverview".Translate(), delegate
			{
				this.operationsTabSelected = false;
			}, !this.operationsTabSelected));
			if (flag3)
			{
				string label;
				if (pawn.RaceProps.mechanoid)
				{
					label = "MedicalOperationsMechanoidsShort".Translate(new object[]
					{
						pawn.BillStack.Count
					});
				}
				else
				{
					label = "MedicalOperationsShort".Translate(new object[]
					{
						pawn.BillStack.Count
					});
				}
				list.Add(new TabRecord(label, delegate
				{
					this.operationsTabSelected = true;
				}, this.operationsTabSelected));
			}
			GUI.color = TabDrawer.BoxColor;
			GUI.DrawTexture(rect, TabDrawer.WhiteTexture);
			GUI.color = TabDrawer.BoxBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
			TabDrawer.DrawTabs(new Rect(rect.x, rect.y, rect.width - 90f, rect.height), list, ITab_Pawn_Health_Alternate.TabAtlasTex);
			float num = 0f;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (!this.operationsTabSelected)
			{
				Rect position2 = rect.ContractedBy(12f);
				if (pawn.playerSettings != null && !pawn.Dead)
				{
					Rect rect2 = new Rect(position2.x + 4f, position2.y + 8f, position2.width, 32f);
					MedicalCareUtility.MedicalCareSetter(rect2, ref pawn.playerSettings.medCare);
					position2.y += 50f;
					position2.height -= 50f;
				}
				try
				{
					GUI.BeginGroup(position2);
					Rect leftRect = new Rect(0f, num, position2.width, position2.height - num);
					num = this.healthCardUtility.DrawStatus(leftRect, pawn, num, showBloodLoss);
				}
				finally
				{
					GUI.EndGroup();
				}
			}
			else
			{
				Rect position3 = rect.ContractedBy(12f);
				try
				{
					GUI.BeginGroup(position3);
					Rect rect3 = new Rect(0f, 0f, position3.width, position3.height);
					this.operationsScrollView.Begin(rect3);
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.MedicalOperations, KnowledgeAmount.GuiFrame);
					num = this.healthCardUtility.DrawMedOperationsTab(rect3, pawn, base.SelThing, num);
					this.operationsScrollView.End(num);
				}
				finally
				{
					GUI.EndGroup();
				}
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.BeginGroup(position);
			Rect leftRect2 = new Rect(0f, 0f, position.width, position.height);
			this.healthCardUtility.DrawInjuries(leftRect2, pawn, showBloodLoss);
			GUI.EndGroup();
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value)
			{
				Pawn pawn2 = null;
				if (base.SelPawn != null)
				{
					pawn2 = base.SelPawn;
				}
				else
				{
					Corpse corpse3 = base.SelThing as Corpse;
					if (corpse3 != null)
					{
						pawn2 = corpse3.innerPawn;
					}
				}
				if (pawn2 != null)
				{
					BrowseButtonDrawer.DrawBrowseButtons(this.size, pawn2);
				}
			}
			GUI.color = Color.white;
			Text.Anchor = anchor;
			Rect rect4 = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(rect4))
			{
				Event.current.Use();
			}
		}
	}
}
