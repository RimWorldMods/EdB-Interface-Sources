using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Health_Vanilla : ITab_Pawn_Health
	{
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Health_Vanilla(PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		protected override void FillTab()
		{
			base.FillTab();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value)
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
				if (pawn != null)
				{
					BrowseButtonDrawer.DrawBrowseButtons(this.size, pawn);
				}
			}
		}
	}
}
