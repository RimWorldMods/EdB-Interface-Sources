using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class ComponentPauseOnStart : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferencePauseOnStart preference = new PreferencePauseOnStart();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentPauseOnStart()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
		}

		public void Initialize(UserInterface userInterface)
		{
			if (this.preference.Value)
			{
				try
				{
					if (MapInitData.startedFromEntry)
					{
						Find.TickManager.TogglePaused();
					}
					else if (!Prefs.PauseOnLoad)
					{
						Find.TickManager.TogglePaused();
					}
				}
				catch (Exception ex)
				{
					Log.Error("Failed to pause game on start as specified by your EdB interface preferences");
					throw ex;
				}
			}
		}
	}
}
