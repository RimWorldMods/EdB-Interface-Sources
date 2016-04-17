using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceEnableTabReplacement : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.TabReplacement.Prefs.Enable";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.TabReplacement.Prefs.Enable.Tip";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool DefaultValue
		{
			get
			{
				return Screen.height >= 900;
			}
		}
	}
}
