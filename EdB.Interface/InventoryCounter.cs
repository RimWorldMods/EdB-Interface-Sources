using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public interface InventoryCounter
	{
		InventoryRecord CountThing(Thing thing, Dictionary<InventoryRecordKey, InventoryRecord> recordLookup, InventoryState state, out bool equippable);
	}
}
