using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquadFiltering : BooleanPreference
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
				return "EdB.Squads.Prefs.EnableSquadFiltering";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.Squads.Prefs.EnableSquadFiltering.Tip";
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

		public override bool Indent
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
