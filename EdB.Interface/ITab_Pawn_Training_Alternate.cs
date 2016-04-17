using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Training_Alternate : ITab_Pawn_Training
	{
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Training_Alternate(PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
			this.size = new Vector2(TabDrawer.TabPanelSize.x, 356f);
		}

		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(20f);
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
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
