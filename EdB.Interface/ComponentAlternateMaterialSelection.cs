using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentAlternateMaterialSelection : IInitializedComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceRightClickOnly preference = new PreferenceRightClickOnly();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentAlternateMaterialSelection()
		{
			this.preferences.Add(this.preference);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			GizmoGridDrawer.RightClickMaterialPreference = this.preference;
		}

		public void Initialize(UserInterface userInterface)
		{
		}
	}
}
