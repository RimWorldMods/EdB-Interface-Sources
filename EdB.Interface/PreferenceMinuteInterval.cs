using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class PreferenceMinuteInterval : IntegerOptionsPreference
	{
		protected static readonly int DefaultSelectedOption = 2;

		protected List<int> optionValues = new List<int>();

		protected string optionLabelPrefix = "EdB.AlternateTimeDisplay.Prefs.Interval";

		public PreferenceEnableAlternateTimeDisplay PreferenceEnableAlternateTimeDisplay
		{
			get;
			set;
		}

		public override string Name
		{
			get
			{
				return "EdB.AlternateTimeDisplay.Prefs.Interval";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.AlternateTimeDisplay.Prefs";
			}
		}

		public override int DefaultValue
		{
			get
			{
				return this.optionValues[PreferenceMinuteInterval.DefaultSelectedOption];
			}
		}

		public override IEnumerable<int> OptionValues
		{
			get
			{
				return this.optionValues;
			}
		}

		public override string OptionValuePrefix
		{
			get
			{
				return this.optionLabelPrefix;
			}
		}

		public override bool Disabled
		{
			get
			{
				return this.PreferenceEnableAlternateTimeDisplay == null || !this.PreferenceEnableAlternateTimeDisplay.Value;
			}
		}

		public override bool Indent
		{
			get
			{
				return false;
			}
		}

		public PreferenceMinuteInterval()
		{
			this.optionValues.Add(1);
			this.optionValues.Add(5);
			this.optionValues.Add(15);
		}
	}
}
