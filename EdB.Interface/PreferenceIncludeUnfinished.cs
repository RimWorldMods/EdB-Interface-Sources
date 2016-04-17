using System;

namespace EdB.Interface
{
	public class PreferenceIncludeUnfinished : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.Inventory.Prefs.IncludeUnfinished";
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

		public override bool DisplayInOptions
		{
			get
			{
				return false;
			}
		}
	}
}
