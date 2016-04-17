using System;
using UnityEngine;

namespace EdB.Interface
{
	public class ITab_Pawn_Prisoner_Alternate : ITab_Pawn_Visitor_Alternate
	{
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}

		public ITab_Pawn_Prisoner_Alternate(PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabPrisoner";
			this.tutorHighlightTag = "TabPrisoner";
			base.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = new Vector2(TabDrawer.TabPanelSize.x, 356f);
		}
	}
}
