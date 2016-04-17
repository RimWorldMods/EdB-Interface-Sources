using System;

namespace EdB.Interface
{
	public class PreferenceCompressedStorage : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.Inventory.Prefs.CompressedStorage";
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
