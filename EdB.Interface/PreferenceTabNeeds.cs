using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceTabNeeds : BooleanPreference
	{
		public override string Name
		{
			get
			{
				return "EdB.TabReplacement.Prefs.TabNeeds";
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

		public override bool Indent
		{
			get
			{
				return true;
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement
		{
			get;
			set;
		}

		public override bool Disabled
		{
			get
			{
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override bool Value
		{
			get
			{
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
