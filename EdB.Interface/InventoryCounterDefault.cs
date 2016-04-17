using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI;

namespace EdB.Interface
{
	public class InventoryCounterDefault : InventoryCounter
	{
		public HashSet<Thing> reservedThings = new HashSet<Thing>();

		protected FieldInfo reservationsField;

		public InventoryCounterDefault()
		{
			this.reservationsField = typeof(ReservationManager).GetField("reservations", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public void Prepare()
		{
			this.reservedThings.Clear();
			ReservationManager reservations = Find.Reservations;
			IList list = this.reservationsField.GetValue(reservations) as IList;
			if (list == null)
			{
				return;
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				object obj = list[i];
				TargetInfo a = (TargetInfo)obj.GetType().GetField("target", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
				if (a != null && a.HasThing)
				{
					this.reservedThings.Add(a.Thing);
				}
			}
		}

		public InventoryRecord CountThing(Thing thing, Dictionary<InventoryRecordKey, InventoryRecord> recordLookup, InventoryState state, out bool equippable)
		{
			equippable = false;
			if (thing == null)
			{
				Log.Warning("Tried to count a null thing");
				return null;
			}
			ThingDef thingDef = thing.def;
			if (thingDef == null)
			{
				Log.Warning("Failed to count thing.  Its definition is null.");
				return null;
			}
			ThingDef thingDef2 = null;
			if (thingDef.building != null && thingDef.entityDefToBuild != null)
			{
				thingDef2 = thingDef;
				thingDef = (thingDef.entityDefToBuild as ThingDef);
				if (thingDef == null)
				{
					return null;
				}
			}
			Thing thing2 = thing.GetInnerIfMinified();
			if (thing2 == thing)
			{
				thing2 = null;
			}
			if (thing2 != null)
			{
				thing = thing2;
				thingDef = thing2.def;
				if (thingDef == null)
				{
					return null;
				}
			}
			InventoryRecord inventoryRecord = null;
			if (!recordLookup.TryGetValue(new InventoryRecordKey(thingDef, thing.Stuff), out inventoryRecord))
			{
				if (thing.Stuff == null)
				{
					Log.Warning("Did not find record for " + thingDef.defName);
				}
				else
				{
					Log.Warning("Did not find record for " + thingDef.defName + " made out of " + thing.Stuff.defName);
				}
				return null;
			}
			if (inventoryRecord.type == InventoryType.UNKNOWN)
			{
				return null;
			}
			if (thingDef2 == null && thing2 == null)
			{
				inventoryRecord.count += thing.stackCount;
			}
			else
			{
				inventoryRecord.unfinishedCount += thing.stackCount;
			}
			if (thingDef.building == null)
			{
				TextureColorPair textureColorPair = MaterialResolver.Resolve(thing);
				inventoryRecord.texture = textureColorPair.texture;
				inventoryRecord.color = thing.DrawColor;
			}
			else
			{
				inventoryRecord.color = thing.DrawColor;
			}
			if (thingDef.equipmentType != EquipmentType.None || thingDef.apparel != null)
			{
				bool flag = thing.SelectableNow();
				bool flag2 = this.reservedThings.Contains(thing);
				if (flag && !flag2)
				{
					if (inventoryRecord.availableCount == -1)
					{
						inventoryRecord.availableCount = thing.stackCount;
					}
					else
					{
						inventoryRecord.availableCount += thing.stackCount;
					}
					equippable = true;
				}
				else if (inventoryRecord.availableCount == -1)
				{
					inventoryRecord.availableCount = 0;
				}
				if (flag2)
				{
					inventoryRecord.reservedCount++;
				}
			}
			return inventoryRecord;
		}
	}
}
