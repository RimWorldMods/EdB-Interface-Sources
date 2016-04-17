using System;

namespace EdB.Interface
{
	public class PreferenceAlwaysShowSquadName : BooleanPreference
	{
		protected PreferenceEnableSquads preferenceEnableSquads;

		public PreferenceEnableSquads PreferenceEnableSquads
		{
			set
			{
				this.preferenceEnableSquads = value;
			}
		}

		public override string Name
		{
			get
			{
				return "EdB.Squads.Prefs.AlwaysShowSquadName";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.Squads.Prefs.AlwaysShowSquadName.Tip";
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
				return false;
			}
		}

		public override bool Disabled
		{
			get
			{
				return this.preferenceEnableSquads != null && !this.preferenceEnableSquads.Value;
			}
		}

		public override bool Value
		{
			get
			{
				return (this.preferenceEnableSquads == null || this.preferenceEnableSquads.Value) && base.Value;
			}
		}
	}
}
