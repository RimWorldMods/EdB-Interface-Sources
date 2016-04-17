using RimWorld;
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class ITab_Pawn_Guest_Alternate : ITab_Pawn_Visitor_Alternate
	{
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfColony && !base.SelPawn.IsPrisoner;
			}
		}

		public ITab_Pawn_Guest_Alternate(PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabGuest";
			base.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = new Vector2(TabDrawer.TabPanelSize.x, 200f);
		}
	}
}
