using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentAlternateTimeDisplay : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceEnableAlternateTimeDisplay preferenceEnableAlternateTimeDisplay = new PreferenceEnableAlternateTimeDisplay();

		protected PreferenceAmPm preferenceAmPm = new PreferenceAmPm();

		protected PreferenceMinuteInterval preferenceMinuteInterval = new PreferenceMinuteInterval();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentAlternateTimeDisplay()
		{
			this.preferences.Add(this.preferenceEnableAlternateTimeDisplay);
			this.preferences.Add(this.preferenceMinuteInterval);
			this.preferences.Add(this.preferenceAmPm);
			this.preferenceAmPm.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
			this.preferenceMinuteInterval.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			userInterface.globalControls.PreferenceAmPm = this.preferenceAmPm;
			userInterface.globalControls.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
			userInterface.globalControls.PreferenceMinuteInterval = this.preferenceMinuteInterval;
			DateReadout.Reinit();
		}

		public void Initialize(UserInterface userInterface)
		{
		}
	}
}
