using System;
using Verse;

namespace EdB.Interface
{
	public class InventoryRecordKey
	{
		public ThingDef ThingDef
		{
			get;
			set;
		}

		public ThingDef StuffDef
		{
			get;
			set;
		}

		public InventoryRecordKey()
		{
		}

		public InventoryRecordKey(ThingDef thingDef)
		{
			this.ThingDef = thingDef;
			this.StuffDef = null;
		}

		public InventoryRecordKey(ThingDef thingDef, ThingDef stuffDef)
		{
			this.ThingDef = thingDef;
			this.StuffDef = stuffDef;
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			InventoryRecordKey inventoryRecordKey = o as InventoryRecordKey;
			return inventoryRecordKey != null && this.ThingDef == inventoryRecordKey.ThingDef && this.StuffDef == inventoryRecordKey.StuffDef;
		}

		public override int GetHashCode()
		{
			int num = (this.ThingDef == null) ? 0 : this.ThingDef.GetHashCode();
			int num2 = (this.StuffDef == null) ? 0 : this.StuffDef.GetHashCode();
			return 31 * num + num2;
		}
	}
}
