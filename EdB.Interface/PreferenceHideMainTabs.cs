using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class PreferenceHideMainTabs : MultipleSelectionStringOptionsPreference
	{
		protected List<string> options = new List<string>();

		protected HashSet<string> exclusions = new HashSet<string>();

		protected Dictionary<string, string> labels = new Dictionary<string, string>();

		public override string Name
		{
			get
			{
				return "EdB.MainTabs.Prefs.HideTabs";
			}
		}

		public override string Tooltip
		{
			get
			{
				return "EdB.MainTabs.Prefs.HideTabs.Tip";
			}
		}

		public override string Group
		{
			get
			{
				return "EdB.MainTabs.Prefs";
			}
		}

		public override string OptionValuePrefix
		{
			get
			{
				return string.Empty;
			}
		}

		public override string DefaultValue
		{
			get
			{
				return string.Empty;
			}
		}

		public override bool DisplayInOptions
		{
			get
			{
				return true;
			}
		}

		public override IEnumerable<string> OptionValues
		{
			get
			{
				return this.options;
			}
		}

		public PreferenceHideMainTabs()
		{
			this.exclusions.Add("Inspect");
			this.exclusions.Add("Architect");
			this.exclusions.Add("Work");
			this.exclusions.Add("EdB_Interface_Squads");
			this.exclusions.Add("Menu");
			IEnumerable<MainTabDef> allDefs = DefDatabase<MainTabDef>.AllDefs;
			foreach (MainTabDef current in from def in allDefs
			orderby def.order
			select def)
			{
				if (!this.exclusions.Contains(current.defName))
				{
					this.options.Add(current.defName);
					this.labels.Add(current.defName, current.LabelCap);
				}
			}
		}

		public bool IsTabExcluded(string name)
		{
			return this.exclusions.Contains(name);
		}

		public override string OptionTranslated(string optionValue)
		{
			string text;
			if (this.labels.TryGetValue(optionValue, out text))
			{
				return "EdB.MainTabs.Prefs.HideTabs.Option".Translate(new object[]
				{
					text
				});
			}
			return null;
		}
	}
}
