using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquads : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.Squads.Prefs.EnableSquads";
			}
		}

		public override string Tooltip
		{
			get
			{
				return null;
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.Squads.Prefs";
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
