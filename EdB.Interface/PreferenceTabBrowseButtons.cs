using System;

namespace EdB.Interface
{
	public class PreferenceTabBrowseButtons : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.TabReplacement.Prefs.BrowseButtons";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.TabReplacement.Prefs.BrowseButtons.Tip";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool DefaultValue
		{
			get
			{
				return true;
			}
		}
	}
}
