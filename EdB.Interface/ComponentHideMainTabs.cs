using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentHideMainTabs : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceHideMainTabs preference = new PreferenceHideMainTabs();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentHideMainTabs()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preference.ValueChanged += delegate(IEnumerable<string> selectedOptions)
			{
				this.UpdateMainTabVisibility(mainTabsRoot, selectedOptions);
			};
		}

		public void Initialize(UserInterface userInterface)
		{
			this.UpdateMainTabVisibility(userInterface.MainTabsRoot, this.preference.SelectedOptions);
		}

		public void UpdateMainTabVisibility(MainTabsRoot mainTabsRoot, IEnumerable<string> hiddenTabs)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string current in hiddenTabs)
			{
				hashSet.Add(current);
			}
			foreach (MainTabDef current2 in mainTabsRoot.AllTabs)
			{
				if (!this.preference.IsTabExcluded(current2.defName))
				{
					if (current2.Window != null)
					{
						if (hashSet.Contains(current2.defName))
						{
							current2.showTabButton = false;
						}
						else
						{
							current2.showTabButton = true;
						}
					}
				}
			}
		}
	}
}
