using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class InventoryState
	{
		public bool compressedStorage;

		public InventoryPreferences prefs = new InventoryPreferences();

		public Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>> categorizedRecords;
	}
}
