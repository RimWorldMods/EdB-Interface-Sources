using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentEmptyStockpile : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceEmptyStockpile preference = new PreferenceEmptyStockpile();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentEmptyStockpile()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
		}

		public void Initialize(UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			foreach (MainTabDef current in mainTabsRoot.AllTabs)
			{
				MainTabWindow window = current.Window;
				if (window != null)
				{
					MainTabWindow_Architect mainTabWindow_Architect = window as MainTabWindow_Architect;
					if (mainTabWindow_Architect != null)
					{
						Designator_ZoneAddStockpile_Resources designator_ZoneAddStockpile_Resources = new Designator_ZoneAddStockpile_Resources();
						designator_ZoneAddStockpile_Resources.EmptyZonePreference = this.preference;
						mainTabWindow_Architect.ReplaceDesignator(typeof(RimWorld.Designator_ZoneAddStockpile_Resources), designator_ZoneAddStockpile_Resources);
					}
				}
			}
		}
	}
}
