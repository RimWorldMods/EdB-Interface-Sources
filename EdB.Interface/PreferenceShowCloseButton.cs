using System;

namespace EdB.Interface
{
	public class PreferenceShowCloseButton : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.MainTabs.Prefs.ShowCloseButton";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.MainTabs.Prefs.ShowCloseButton.Tip";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.MainTabs.Prefs";
			}
		}

		public override bool DefaultValue
		{
			get
			{
				return false;
			}
		}
	}
}
