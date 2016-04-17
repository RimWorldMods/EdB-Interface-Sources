using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class ITab_Growing_Alternate : ITab_Growing
	{
		private static readonly Vector2 WinSize = new Vector2(TabDrawer.TabPanelSize.x, 480f);

		private static readonly Vector2 PaddingSize = new Vector2(26f, 30f);

		private static Vector2 ContentSize;

		protected ScrollView scrollView = new ScrollView();

		protected ThingDef rightClicked;

		protected FloatMenu infoFloatMenu;

		public ITab_Growing_Alternate()
		{
			this.size = ITab_Growing_Alternate.WinSize;
			ITab_Growing_Alternate.ContentSize = new Vector2(ITab_Growing_Alternate.WinSize.x - ITab_Growing_Alternate.PaddingSize.x * 2f, ITab_Growing_Alternate.WinSize.y - ITab_Growing_Alternate.PaddingSize.y * 2f);
			ITab_Growing_Alternate.ContentSize.y = ITab_Growing_Alternate.ContentSize.y - 2f;
		}

		protected override void FillTab()
		{
			if (this.rightClicked != null && this.infoFloatMenu != null && Find.WindowStack.Top() != this.infoFloatMenu)
			{
				this.rightClicked = null;
				this.infoFloatMenu = null;
			}
			Text.Font = GameFont.Small;
			IPlantToGrowSettable plantToGrowSettable = (IPlantToGrowSettable)Find.Selector.SelectedObjects.First<object>();
			Rect position = new Rect(ITab_Growing_Alternate.PaddingSize.x, ITab_Growing_Alternate.PaddingSize.y, ITab_Growing_Alternate.ContentSize.x, ITab_Growing_Alternate.ContentSize.y);
			GUI.BeginGroup(position);
			Rect viewRect = new Rect(0f, 0f, position.width, position.height);
			this.scrollView.Begin(viewRect);
			float num = 0f;
			int num2 = 0;
			foreach (ThingDef current in GenPlant.ValidPlantTypesForGrower(Find.Selector.SingleSelectedObject))
			{
				float num3 = Text.CalcHeight(current.LabelCap, ITab_Growing_Alternate.ContentSize.x - 32f);
				if (num3 < 30f)
				{
					num3 = 30f;
				}
				GUI.color = Color.white;
				Rect rect = new Rect(0f, num + 1f, ITab_Growing_Alternate.ContentSize.x - 28f, num3);
				Rect position2 = new Rect(0f, rect.y - 1f, rect.width, rect.height + 2f);
				Vector2 mousePosition = Event.current.mousePosition;
				if (position2.Contains(mousePosition) && mousePosition.y > this.scrollView.Position.y && mousePosition.y < this.scrollView.Position.y + this.scrollView.ViewHeight)
				{
					GUI.DrawTexture(position2, TexUI.HighlightTex);
				}
				else if (num2 % 2 == 0)
				{
					GUI.DrawTexture(position2, TabDrawer.AlternateRowTexture);
				}
				rect.x += 6f;
				rect.y += 3f;
				rect.width -= 4f;
				Widgets.InfoCardButton(rect.x, rect.y, current);
				rect.x += 34f;
				rect.width -= 34f;
				if ((Widgets.InvisibleButton(new Rect(rect.x, rect.y, rect.width - 36f, rect.height)) || WidgetDrawer.DrawLabeledRadioButton(rect, current.LabelCap, current == plantToGrowSettable.GetPlantDefToGrow(), false)) && plantToGrowSettable.GetPlantDefToGrow() != current)
				{
					plantToGrowSettable.SetPlantDefToGrow(current);
					SoundDefOf.RadioButtonClicked.PlayOneShotOnCamera();
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant, KnowledgeAmount.Total);
				}
				num += num3;
				num += 2f;
				num2++;
			}
			this.scrollView.End(num);
			TutorUIHighlighter.HighlightOpportunity("GrowingZoneSetPlant", new Rect(0f, 0f, ITab_Growing_Alternate.ContentSize.x, num));
			GUI.EndGroup();
		}
	}
}
