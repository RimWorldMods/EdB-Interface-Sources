using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Inspect : RimWorld.MainTabWindow_Inspect
	{
		private const float PaneWidth = 432f;

		private const float PaneInnerMargin = 12f;

		private static readonly Texture2D TabButtonFillTex = SolidColorMaterials.NewSolidColorTexture(0.07450981f, 0.08627451f, 0.1058824f, 1f);

		private static IntVec3 lastSelectCell;

		private FieldInfo openTabTypeField;

		private FieldInfo recentHeightField;

		private FieldInfo sizeField;

		private MethodInfo updateSizeMethod;

		private MethodInfo fillTabMethod;

		private ReplacementTabs replacementTabs = new ReplacementTabs();

		protected float RecentHeight
		{
			get
			{
				return (float)this.recentHeightField.GetValue(this);
			}
			set
			{
				this.recentHeightField.SetValue(this, value);
			}
		}

		protected Type OpenTabType
		{
			get
			{
				return (Type)this.openTabTypeField.GetValue(this);
			}
			set
			{
				this.openTabTypeField.SetValue(this, value);
			}
		}

		public new IEnumerable<ITab> CurTabs
		{
			get
			{
				if (this.NumSelected == 1)
				{
					if (this.replacementTabs == null)
					{
						if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null)
						{
							return this.SelThing.def.inspectorTabsResolved;
						}
						if (this.SelZone != null)
						{
							return this.SelZone.GetInspectionTabs();
						}
					}
					else
					{
						if (this.SelThing != null)
						{
							return this.replacementTabs.GetTabs(this.SelThing);
						}
						if (this.SelZone != null)
						{
							return this.replacementTabs.GetTabs(this.SelZone);
						}
					}
				}
				return Enumerable.Empty<ITab>();
			}
		}

		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		private IEnumerable<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		protected override float WindowPadding
		{
			get
			{
				return 0f;
			}
		}

		public ReplacementTabs ReplacementTabs
		{
			get
			{
				return this.replacementTabs;
			}
			set
			{
				this.replacementTabs = value;
			}
		}

		public MainTabWindow_Inspect()
		{
			this.closeOnEscapeKey = false;
			this.recentHeightField = typeof(RimWorld.MainTabWindow_Inspect).GetField("recentHeight", BindingFlags.Instance | BindingFlags.NonPublic);
			this.openTabTypeField = typeof(RimWorld.MainTabWindow_Inspect).GetField("openTabType", BindingFlags.Instance | BindingFlags.NonPublic);
			this.sizeField = typeof(ITab).GetField("size", BindingFlags.Instance | BindingFlags.NonPublic);
			this.updateSizeMethod = typeof(ITab).GetMethod("UpdateSize", BindingFlags.Instance | BindingFlags.NonPublic);
			this.fillTabMethod = typeof(ITab).GetMethod("FillTab", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		private void DoTabs(IEnumerable<ITab> tabs)
		{
			try
			{
				Type openTabType = this.OpenTabType;
				float top = base.PaneTopY - 30f;
				float num = 360f;
				float width = 0f;
				bool flag = false;
				foreach (ITab current in tabs)
				{
					if (current.IsVisible)
					{
						Rect rect = new Rect(num, top, 72f, 30f);
						width = num;
						Text.Font = GameFont.Small;
						if (Widgets.TextButton(rect, current.labelKey.Translate(), true, false))
						{
							this.ToggleTab(current);
						}
						if (!current.tutorHighlightTag.NullOrEmpty())
						{
							TutorUIHighlighter.HighlightOpportunity(current.tutorHighlightTag, rect);
						}
						if (current.GetType() == openTabType)
						{
							this.DoTabGui(current);
							this.RecentHeight = 700f;
							flag = true;
						}
						num -= 72f;
					}
				}
				if (flag)
				{
					GUI.DrawTexture(new Rect(0f, top, width, 30f), MainTabWindow_Inspect.TabButtonFillTex);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 742783);
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.currentWindowRect.x = 0f;
			}
			else
			{
				this.currentWindowRect.x = (float)Screen.width - this.currentWindowRect.width;
			}
			this.currentWindowRect.y = (float)(Screen.height - 35) - this.currentWindowRect.height;
			if (this.def.concept != null)
			{
				ConceptDatabase.KnowledgeDemonstrated(this.def.concept, KnowledgeAmount.GuiFrame);
			}
			this.RecentHeight = RimWorld.MainTabWindow_Inspect.PaneSize.y;
			if (this.NumSelected > 0)
			{
				try
				{
					Rect rect = inRect.ContractedBy(12f);
					rect.yMin -= 4f;
					GUI.BeginGroup(rect);
					bool flag = true;
					if (this.NumSelected > 1)
					{
						flag = !(from t in this.Selected
						where !InspectPaneUtility.CanInspectTogether(this.Selected.First<object>(), t)
						select t).Any<object>();
					}
					else
					{
						Rect rect2 = new Rect(rect.width - 30f, 0f, 30f, 30f);
						if (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
						{
							if (Widgets.ImageButton(rect2, TexButton.SelectOverlappingNext))
							{
								this.SelectNextInCell();
							}
							TooltipHandler.TipRegion(rect2, "SelectNextInSquareTip".Translate(new object[]
							{
								KeyBindingDefOf.SelectNextInCell.MainKeyLabel
							}));
						}
						if (Find.Selector.SingleSelectedThing != null)
						{
							Widgets.InfoCardButton(rect.width - 60f, 0f, Find.Selector.SingleSelectedThing);
						}
					}
					Rect rect3 = new Rect(0f, 0f, rect.width - 60f, 50f);
					string label = InspectPaneUtility.AdjustedLabelFor(this.Selected, rect3);
					rect3.width += 300f;
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect3, label);
					if (flag && this.NumSelected == 1)
					{
						Rect rect4 = rect.AtZero();
						rect4.yMin += 26f;
						InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect4);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception doing inspect pane: " + ex.ToString());
				}
				finally
				{
					GUI.EndGroup();
				}
			}
		}

		public override void ExtraOnGUI()
		{
			if (this.NumSelected > 0)
			{
				if (KeyBindingDefOf.SelectNextInCell.KeyDownEvent)
				{
					this.SelectNextInCell();
				}
				if (DesignatorManager.SelectedDesignator != null)
				{
					DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, base.PaneTopY);
				}
				InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected);
				this.DoTabs(this.CurTabs);
			}
		}

		private void SelectNextInCell()
		{
			if (this.NumSelected == 0)
			{
				return;
			}
			if (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
			{
				if (Find.Selector.SelectedZone == null)
				{
					MainTabWindow_Inspect.lastSelectCell = Find.Selector.SingleSelectedThing.Position;
				}
				Find.Selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell);
			}
		}

		private void ToggleTab(ITab tab)
		{
			Type openTabType = this.OpenTabType;
			if ((tab == null && openTabType == null) || tab.GetType() == openTabType)
			{
				this.OpenTabType = null;
				SoundDefOf.TabClose.PlayOneShotOnCamera();
			}
			else
			{
				tab.OnOpen();
				this.OpenTabType = tab.GetType();
				SoundDefOf.TabOpen.PlayOneShotOnCamera();
			}
		}

		public override void WindowUpdate()
		{
			Type openTabType = this.OpenTabType;
			foreach (ITab current in this.CurTabs)
			{
				if (current.IsVisible && current.GetType() == openTabType)
				{
					current.TabUpdate();
				}
			}
		}

		public void DoTabGui(ITab tab)
		{
			MainTabWindow_Inspect inspectWorker = (MainTabWindow_Inspect)MainTabDefOf.Inspect.Window;
			this.updateSizeMethod.Invoke(tab, null);
			Vector2 vector = (Vector2)this.sizeField.GetValue(tab);
			float y = vector.y;
			float top = inspectWorker.PaneTopY - 30f - y;
			Rect outRect = new Rect(0f, top, vector.x, y);
			Find.WindowStack.ImmediateWindow(235086, outRect, WindowLayer.GameUI, delegate
			{
				if (Find.MainTabsRoot.OpenTab != MainTabDefOf.Inspect || !this.CurTabs.Contains(tab) || !tab.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(outRect.AtZero()))
				{
					inspectWorker.CloseOpenTab();
				}
				try
				{
					this.fillTabMethod.Invoke(tab, null);
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception filling tab ",
						this.GetType(),
						": ",
						ex.ToString()
					}), 49827);
				}
			}, true, false, 1f);
		}
	}
}
