using System;

namespace EdB.Interface
{
	public class PreferenceEnableInventory : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.Inventory.Prefs.EnableInventory";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.Inventory.Prefs";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.Inventory.Prefs.EnableInventory.Tip";
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
