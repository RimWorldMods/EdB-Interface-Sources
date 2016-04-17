using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentColorCodedWorkPassions : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceColorCodedWorkPassions preference = new PreferenceColorCodedWorkPassions();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentColorCodedWorkPassions()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
		}

		public void Initialize(UserInterface userInterface)
		{
			WidgetsWork.PreferenceColorCodedWorkPassions = this.preference;
		}
	}
}
