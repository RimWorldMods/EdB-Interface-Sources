using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public interface InventoryResolver
	{
		List<InventoryCount> GetInventoryCounts(Thing thing, InventoryPreferences prefs);
	}
}
