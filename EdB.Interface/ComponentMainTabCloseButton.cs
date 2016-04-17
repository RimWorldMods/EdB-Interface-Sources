using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentMainTabCloseButton : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceShowCloseButton preference = new PreferenceShowCloseButton();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentMainTabCloseButton()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preference.ValueChanged += delegate(bool value)
			{
				this.UpdateButtonState(mainTabsRoot, value);
			};
		}

		public void Initialize(UserInterface userInterface)
		{
			this.UpdateButtonState(userInterface.MainTabsRoot, this.preference.Value);
		}

		public void UpdateButtonState(MainTabsRoot mainTabsRoot, bool value)
		{
			foreach (MainTabDef current in mainTabsRoot.AllTabs)
			{
				if (current.showTabButton)
				{
					MainTabWindow window = current.Window;
					if (window != null)
					{
						window.doCloseX = value;
					}
				}
			}
		}
	}
}
