using System;

namespace EdB.Interface
{
	public class PreferenceEnableAlternateTimeDisplay : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.AlternateTimeDisplay.Prefs.Enabled";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.AlternateTimeDisplay.Prefs";
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
