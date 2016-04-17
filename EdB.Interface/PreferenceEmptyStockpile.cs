using System;

namespace EdB.Interface
{
	public class PreferenceEmptyStockpile : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.EmptyStockpile.Prefs.EmptyStockpile";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.EmptyStockpile.Prefs.EmptyStockpile.Tip";
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
