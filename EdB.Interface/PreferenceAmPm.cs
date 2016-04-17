using System;

namespace EdB.Interface
{
	public class PreferenceAmPm : BooleanPreference
	{
		public PreferenceEnableAlternateTimeDisplay PreferenceEnableAlternateTimeDisplay
		{
			get;
			set;
		}

		public override string Name
		{
			get
			{
				return "EdB.AlternateTimeDisplay.Prefs.AmPm";
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

		public override bool Disabled
		{
			get
			{
				return this.PreferenceEnableAlternateTimeDisplay == null || !this.PreferenceEnableAlternateTimeDisplay.Value;
			}
		}

		public override bool Indent
		{
			get
			{
				return false;
			}
		}
	}
}
