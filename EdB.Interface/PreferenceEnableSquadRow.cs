using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquadRow : BooleanPreference
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
				return "EdB.Squads.Prefs.EnableSquadRow";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.Squads.Prefs.EnableSquadRow.Tip";
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
	}
}
