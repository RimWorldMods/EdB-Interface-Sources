using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Prisoner_Vanilla : ITab_Pawn_Prisoner
	{
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Prisoner_Vanilla(PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		protected override void FillTab()
		{
			base.FillTab();
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
