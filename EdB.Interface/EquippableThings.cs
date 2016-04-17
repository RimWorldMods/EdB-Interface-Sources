using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class EquippableThings
	{
		protected Dictionary<InventoryRecordKey, List<EquippableThing>> thingLookup = new Dictionary<InventoryRecordKey, List<EquippableThing>>();

		public List<EquippableThing> this[InventoryRecordKey key]
		{
			get
			{
				List<EquippableThing> result;
				if (this.thingLookup.TryGetValue(key, out result))
				{
					return result;
				}
				return null;
			}
			set
			{
				this.thingLookup.Add(key, value);
			}
		}

		public void Reset()
		{
			this.thingLookup.Clear();
		}

		public void Add(InventoryRecordKey key, Thing thing)
		{
			List<EquippableThing> list = this[key];
			if (list == null)
			{
				list = new List<EquippableThing>();
				this[key] = list;
			}
			list.Add(new EquippableThing(thing));
		}
	}
}
