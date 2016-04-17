using System;
using Verse;

namespace EdB.Interface
{
	public class InventoryCount
	{
		public InventoryRecordKey key = new InventoryRecordKey();

		public int count;

		public int deepStorageCount;

		public InventoryCount(ThingDef thingDef, int count, bool isDeepStorage = false)
		{
			this.key.ThingDef = thingDef;
			this.key.StuffDef = null;
			this.count = (isDeepStorage ? 0 : count);
			this.deepStorageCount = ((!isDeepStorage) ? 0 : count);
		}

		public InventoryCount(ThingDef thingDef, ThingDef stuffDef, int count, bool isDeepStorage = false)
		{
			this.key.ThingDef = thingDef;
			this.key.StuffDef = stuffDef;
			this.count = (isDeepStorage ? 0 : count);
			this.deepStorageCount = ((!isDeepStorage) ? 0 : count);
		}
	}
}
