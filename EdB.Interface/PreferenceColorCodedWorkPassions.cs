using System;

namespace EdB.Interface
{
	public class PreferenceColorCodedWorkPassions : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.ColorCodedWorkPassions.Pref";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.ColorCodedWorkPassions.Pref.Tip";
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
