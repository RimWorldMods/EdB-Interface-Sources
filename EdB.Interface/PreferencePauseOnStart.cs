using System;

namespace EdB.Interface
{
	public class PreferencePauseOnStart : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.PauseOnStart.Prefs.PauseOnStart";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.PauseOnStart.Prefs.PauseOnStart.Tip";
			}
		}

		public override string Group
		{
			get
			{
				return null;
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
