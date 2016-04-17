using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class ReplacementTabs
	{
		protected Dictionary<ThingDef, List<ITab>> thingDefDictionary = new Dictionary<ThingDef, List<ITab>>();

		protected Dictionary<Type, ITab> zoneTypeDictionary = new Dictionary<Type, ITab>();

		public bool Empty
		{
			get
			{
				return this.thingDefDictionary.Count == 0 && this.zoneTypeDictionary.Count == 0;
			}
		}

		public void Clear()
		{
			this.thingDefDictionary.Clear();
			this.zoneTypeDictionary.Clear();
		}

		public IEnumerable<ITab> GetTabs(Thing thing)
		{
			List<ITab> result;
			if (this.thingDefDictionary.TryGetValue(thing.def, out result))
			{
				return result;
			}
			if (thing.def.inspectorTabsResolved != null)
			{
				return thing.def.inspectorTabsResolved;
			}
			return Enumerable.Empty<ITab>();
		}

		public IEnumerable<ITab> GetTabs(Zone zone)
		{
			if (this.zoneTypeDictionary.Count == 0)
			{
				return zone.GetInspectionTabs();
			}
			return zone.GetInspectionTabs().Select(delegate(ITab tab)
			{
				ITab result;
				if (this.zoneTypeDictionary.TryGetValue(tab.GetType(), out result))
				{
					return result;
				}
				return tab;
			});
		}

		public void AddThingDef(ThingDef def, List<ITab> tabs)
		{
			this.thingDefDictionary.Add(def, tabs);
		}

		public void AddZoneType(Type type, ITab tab)
		{
			this.zoneTypeDictionary.Add(type, tab);
		}
	}
}
