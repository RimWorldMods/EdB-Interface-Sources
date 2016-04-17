using System;

namespace EdB.Interface
{
	public class PreferenceTabGear : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.TabReplacement.Prefs.TabGear";
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

		public override bool Indent
		{
			get
			{
				return true;
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement
		{
			get;
			set;
		}

		public override bool Disabled
		{
			get
			{
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override bool Value
		{
			get
			{
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
