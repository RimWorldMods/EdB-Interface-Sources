using System;

namespace EdB.Interface
{
	public class PreferenceRightClickOnly : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.MaterialSelection.Prefs.RightClickOnly";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.MaterialSelection.Prefs.RightClickOnly.Tip";
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
