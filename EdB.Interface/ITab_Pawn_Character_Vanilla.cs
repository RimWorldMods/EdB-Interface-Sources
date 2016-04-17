using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Character_Vanilla : ITab_Pawn_Character
	{
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

		public ITab_Pawn_Character_Vanilla(PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		protected override void FillTab()
		{
			base.FillTab();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value)
			{
				Pawn pawnToShowInfoAbout = this.PawnToShowInfoAbout;
				if (pawnToShowInfoAbout != null)
				{
					BrowseButtonDrawer.DrawBrowseButtons(this.size, pawnToShowInfoAbout);
				}
			}
		}
	}
}
