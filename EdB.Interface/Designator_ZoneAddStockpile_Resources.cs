using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class Designator_ZoneAddStockpile_Resources : RimWorld.Designator_ZoneAddStockpile_Resources
	{
		protected List<ThingDef> thingDefs = new List<ThingDef>(100);

		private BooleanPreference emptyZonePreference;

		public BooleanPreference EmptyZonePreference
		{
			set
			{
				this.emptyZonePreference = value;
			}
		}

		protected override Zone MakeNewZone()
		{
			Zone_Stockpile zone_Stockpile = new Zone_Stockpile(StorageSettingsPreset.DefaultStockpile);
			if (this.emptyZonePreference != null && this.emptyZonePreference.Value)
			{
				if (Find.ZoneManager.AllZones.Count((Zone zone) => zone is Zone_Stockpile) > 1)
				{
					ThingFilter filter = zone_Stockpile.GetStoreSettings().filter;
					this.thingDefs.Clear();
					foreach (ThingDef current in filter.AllowedThingDefs)
					{
						this.thingDefs.Add(current);
					}
					foreach (ThingDef current2 in this.thingDefs)
					{
						filter.SetAllow(current2, false);
					}
					foreach (SpecialThingFilterDef current3 in DefDatabase<SpecialThingFilterDef>.AllDefs)
					{
						if (filter.Allowed(current3))
						{
							filter.SetAllow(current3, false);
						}
					}
				}
			}
			return zone_Stockpile;
		}
	}
}
