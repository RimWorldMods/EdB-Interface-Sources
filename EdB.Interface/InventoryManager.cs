using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryManager
	{
		protected Dictionary<InventoryRecordKey, InventoryRecord> allRecords = new Dictionary<InventoryRecordKey, InventoryRecord>();

		protected Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>> categorizedRecords = new Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>>();

		protected Dictionary<string, InventoryResolver> inventoryResolvers = new Dictionary<string, InventoryResolver>();

		protected Dictionary<ThingDef, InventoryCounter> thingCounters = new Dictionary<ThingDef, InventoryCounter>();

		protected InventoryState state = new InventoryState();

		protected InventoryCounterDefault defaultInventoryCounter = new InventoryCounterDefault();

		protected EquippableThings equippableThings = new EquippableThings();

		protected ThingCategoryDef schematicDef;

		protected bool initialized;

		public EquippableThings EquippableThings
		{
			get
			{
				return this.equippableThings;
			}
		}

		public bool CompressedStorage
		{
			get
			{
				return this.state.compressedStorage;
			}
			set
			{
				this.state.compressedStorage = value;
			}
		}

		public PreferenceCompressedStorage PreferenceCompressedStorage
		{
			set
			{
				this.state.prefs.CompressedStorage = value;
			}
		}

		public PreferenceIncludeUnfinished PreferenceIncludeUnfinished
		{
			set
			{
				this.state.prefs.IncludeUnfinished = value;
			}
		}

		public InventoryPreferences Preferences
		{
			get
			{
				return this.state.prefs;
			}
		}

		protected void Initialize()
		{
			this.PrepareAllDefinitions();
			this.CategorizeDefinitions();
			this.CreateCategories();
			this.initialized = true;
		}

		protected void PrepareRecord(InventoryRecord record)
		{
			if (record.thingDef.building != null)
			{
				this.PrepareBuildingRecord(record);
			}
			this.PrepareInventoryResolver(record);
			this.PrepareInventoryCounters(record);
		}

		protected void PrepareBuildingRecord(InventoryRecord record)
		{
			ThingDef thingDef = record.thingDef;
			if (thingDef.entityDefToBuild == null)
			{
				record.texture = thingDef.uiIcon;
			}
			else
			{
				record.texture = thingDef.entityDefToBuild.uiIcon;
			}
			if (thingDef.graphic != null)
			{
				record.proportions = new Vector2(thingDef.graphic.drawSize.x / thingDef.graphic.drawSize.y, 1f);
				float num = (thingDef.graphic.drawSize.x <= thingDef.graphic.drawSize.y) ? thingDef.graphic.drawSize.y : thingDef.graphic.drawSize.x;
				float num2 = (float)((thingDef.size.x <= thingDef.size.z) ? thingDef.size.z : thingDef.size.x);
				float num3 = 0.5f;
				float num4 = 1.75f;
				float num5 = num / num2;
				float buildingScale = (num5 - num3) / num4 + num3;
				record.buildingScale = buildingScale;
			}
			else
			{
				record.proportions = new Vector2(1f, 1f);
				record.buildingScale = 1f;
			}
			if (record.stuffDef != null)
			{
				record.color = ((record.stuffDef.graphic == null) ? Color.white : record.stuffDef.graphic.color);
			}
		}

		protected void PrepareInventoryResolver(InventoryRecord record)
		{
			ThingDef thingDef = record.thingDef;
			InventoryResolver resolver;
			if (this.inventoryResolvers.TryGetValue(thingDef.defName, out resolver))
			{
				record.resolver = resolver;
			}
		}

		protected void PrepareInventoryCounters(InventoryRecord record)
		{
		}

		protected void PrepareAllDefinitions()
		{
			this.schematicDef = DefDatabase<ThingCategoryDef>.GetNamedSilentFail("Schematic");
			Dictionary<StuffCategoryDef, HashSet<ThingDef>> dictionary = new Dictionary<StuffCategoryDef, HashSet<ThingDef>>();
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				if (current.IsStuff && current.stuffProps != null)
				{
					foreach (StuffCategoryDef current2 in current.stuffProps.categories)
					{
						HashSet<ThingDef> hashSet = null;
						if (!dictionary.TryGetValue(current2, out hashSet))
						{
							hashSet = new HashSet<ThingDef>();
							dictionary.Add(current2, hashSet);
						}
						hashSet.Add(current);
					}
				}
			}
			foreach (ThingDef current3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (current3.MadeFromStuff)
				{
					if (current3.stuffCategories != null)
					{
						foreach (StuffCategoryDef current4 in current3.stuffCategories)
						{
							HashSet<ThingDef> hashSet2;
							if (dictionary.TryGetValue(current4, out hashSet2))
							{
								foreach (ThingDef current5 in hashSet2)
								{
									try
									{
										InventoryRecord inventoryRecord = new InventoryRecord();
										inventoryRecord.thingDef = current3;
										inventoryRecord.stuffDef = current5;
										inventoryRecord.count = 0;
										inventoryRecord.color = new Color(1f, 1f, 1f);
										inventoryRecord.texture = null;
										this.PrepareBuildingRecord(inventoryRecord);
										this.allRecords[new InventoryRecordKey(current3, current5)] = inventoryRecord;
									}
									catch (Exception ex)
									{
										Log.Error(string.Concat(new string[]
										{
											"Failed to prepare thing definitions for EdB Interface inventory dialog.  Failed on ",
											current3.defName,
											" with ",
											current5.defName,
											" stuff"
										}));
										throw ex;
									}
								}
							}
						}
					}
				}
				else
				{
					try
					{
						InventoryRecord inventoryRecord2 = new InventoryRecord();
						inventoryRecord2.thingDef = current3;
						inventoryRecord2.stuffDef = null;
						inventoryRecord2.count = 0;
						inventoryRecord2.color = new Color(1f, 1f, 1f);
						inventoryRecord2.texture = null;
						this.PrepareBuildingRecord(inventoryRecord2);
						this.allRecords[new InventoryRecordKey(current3, null)] = inventoryRecord2;
					}
					catch (Exception ex2)
					{
						Log.Error("Failed to prepare thing definitions for EdB Interface inventory dialog.  Failed on " + current3.defName);
						throw ex2;
					}
				}
			}
		}

		public List<InventoryRecord> GetInventoryRecord(InventoryType type)
		{
			if (type != InventoryType.UNKNOWN)
			{
				try
				{
					List<InventoryRecord> result = this.categorizedRecords[type].Values.ToList<InventoryRecord>();
					return result;
				}
				catch (KeyNotFoundException)
				{
					List<InventoryRecord> result = null;
					return result;
				}
			}
			return null;
		}

		protected void CreateCategories()
		{
			this.categorizedRecords.Add(InventoryType.ITEM_RESOURCE, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.ITEM_FOOD, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.ITEM_OTHER, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.ITEM_APPAREL, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.ITEM_SCHEMATIC, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.ITEM_EQUIPMENT, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_OTHER, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_FURNITURE, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_SECURITY, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_STRUCTURE, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_POWER, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_SHIP, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_PRODUCTION, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_JOY, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_TEMPERATURE, new Dictionary<InventoryRecordKey, InventoryRecord>());
			this.categorizedRecords.Add(InventoryType.BUILDING_FOOD_UTILITIES, new Dictionary<InventoryRecordKey, InventoryRecord>());
		}

		protected void CategorizeDefinitions()
		{
			foreach (InventoryRecordKey current in this.allRecords.Keys)
			{
				try
				{
					InventoryType type = this.CategorizeDefinition(current.ThingDef);
					this.allRecords[current].type = type;
				}
				catch (Exception ex)
				{
					Log.Error("Failed to categorize thing definitions for EdB Interface inventory dialog.  Failed on " + current.ThingDef.defName);
					throw ex;
				}
			}
		}

		protected InventoryType CategorizeDefinition(ThingDef def)
		{
			if (def.category == ThingCategory.Projectile)
			{
				return InventoryType.UNKNOWN;
			}
			if (def.thingCategories != null)
			{
				if (def.thingCategories.FirstOrDefault((ThingCategoryDef d) => d.defName.StartsWith("Corpses")) != null)
				{
					return InventoryType.UNKNOWN;
				}
			}
			if (def.IsFrame)
			{
				return InventoryType.UNKNOWN;
			}
			if (def.building != null)
			{
				if ("Furniture".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_FURNITURE;
				}
				if ("Structure".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_STRUCTURE;
				}
				if ("Power".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_POWER;
				}
				if ("Production".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_PRODUCTION;
				}
				if ("Security".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_SECURITY;
				}
				if ("Ship".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_SHIP;
				}
				if ("Joy".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_JOY;
				}
				if ("Temperature".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_TEMPERATURE;
				}
				if ("FoodUtilities".Equals(def.designationCategory))
				{
					return InventoryType.BUILDING_FOOD_UTILITIES;
				}
				if (def.BelongsToCategory("Joy") || (def.graphic.path != null && def.graphic.path.IndexOf("/Joy/") != -1))
				{
					return InventoryType.BUILDING_JOY;
				}
				return InventoryType.BUILDING_OTHER;
			}
			else
			{
				if (def.apparel != null)
				{
					return InventoryType.ITEM_APPAREL;
				}
				if (def.weaponTags != null && def.weaponTags.Count > 0)
				{
					return InventoryType.ITEM_EQUIPMENT;
				}
				if (def.ingestible != null)
				{
					return InventoryType.ITEM_FOOD;
				}
				if (!def.CountAsResource)
				{
					return InventoryType.ITEM_OTHER;
				}
				if (this.schematicDef != null && def.thingCategories != null && def.thingCategories.Contains(this.schematicDef))
				{
					return InventoryType.ITEM_SCHEMATIC;
				}
				if (def.CountAsResource)
				{
					return InventoryType.ITEM_RESOURCE;
				}
				return InventoryType.UNKNOWN;
			}
		}

		protected void ClearInventory()
		{
			foreach (Dictionary<InventoryRecordKey, InventoryRecord> current in this.categorizedRecords.Values)
			{
				current.Clear();
			}
			foreach (InventoryRecord current2 in this.allRecords.Values)
			{
				current2.ResetCounts();
			}
			this.state.compressedStorage = false;
			this.state.categorizedRecords = this.categorizedRecords;
			this.equippableThings.Reset();
			this.defaultInventoryCounter.Prepare();
		}

		public void TakeInventory()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.ClearInventory();
			foreach (Building current in Find.ListerBuildings.allBuildingsColonist)
			{
				this.Count(current);
				Building_Storage building_Storage = current as Building_Storage;
				if (building_Storage != null && building_Storage.GetSlotGroup() != null)
				{
					foreach (Thing current2 in building_Storage.GetSlotGroup().HeldThings)
					{
						this.Count(current2);
					}
				}
				InventoryResolver inventoryResolver;
				if (this.inventoryResolvers.TryGetValue(current.def.defName, out inventoryResolver))
				{
					List<InventoryCount> inventoryCounts = inventoryResolver.GetInventoryCounts(current, this.state.prefs);
					if (inventoryCounts != null)
					{
						foreach (InventoryCount current3 in inventoryCounts)
						{
							this.Count(current3);
						}
					}
				}
			}
			using (List<Zone>.Enumerator enumerator4 = Find.ZoneManager.AllZones.FindAll((Zone zone) => zone is Zone_Stockpile).GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					Zone_Stockpile zone_Stockpile = (Zone_Stockpile)enumerator4.Current;
					if (zone_Stockpile.GetSlotGroup() != null)
					{
						foreach (Thing current4 in zone_Stockpile.GetSlotGroup().HeldThings)
						{
							this.Count(current4);
						}
					}
				}
			}
			foreach (Pawn current5 in Find.ListerPawns.FreeColonists)
			{
				if (current5.equipment != null)
				{
					foreach (Thing current6 in current5.equipment.AllEquipment)
					{
						this.Count(current6);
					}
				}
				if (current5.apparel != null)
				{
					foreach (Thing current7 in current5.apparel.WornApparel)
					{
						this.Count(current7);
					}
				}
				if (current5.inventory != null && current5.inventory.container != null)
				{
					ThingContainer container = current5.inventory.container;
					int count = container.Count;
					for (int i = 0; i < count; i++)
					{
						this.Count(container[i]);
					}
				}
			}
		}

		protected void LogInventory()
		{
			foreach (InventoryRecord current in this.allRecords.Values)
			{
				if (current.count != 0)
				{
					Log.Message(GenLabel.ThingLabel(current.thingDef, current.stuffDef, current.count));
				}
			}
		}

		protected void Count(Thing thing)
		{
			if (thing == null)
			{
				return;
			}
			InventoryRecord inventoryRecord = null;
			bool flag = false;
			InventoryCounter inventoryCounter;
			if (this.thingCounters.TryGetValue(thing.def, out inventoryCounter))
			{
				inventoryRecord = inventoryCounter.CountThing(thing, this.allRecords, this.state, out flag);
			}
			if (inventoryRecord == null)
			{
				inventoryRecord = this.defaultInventoryCounter.CountThing(thing, this.allRecords, this.state, out flag);
			}
			if (inventoryRecord == null)
			{
				return;
			}
			if (flag)
			{
				this.equippableThings.Add(new InventoryRecordKey(inventoryRecord.thingDef, inventoryRecord.stuffDef), thing);
			}
			Dictionary<InventoryRecordKey, InventoryRecord> dictionary;
			if (this.categorizedRecords.TryGetValue(inventoryRecord.type, out dictionary))
			{
				InventoryRecordKey key = new InventoryRecordKey(inventoryRecord.thingDef, inventoryRecord.stuffDef);
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, inventoryRecord);
				}
			}
		}

		protected void Count(InventoryCount count)
		{
			InventoryRecordKey key = count.key;
			ThingDef thingDef = key.ThingDef;
			ThingDef thingDef2 = null;
			if (thingDef.building != null && thingDef.entityDefToBuild != null)
			{
				thingDef2 = key.ThingDef;
				thingDef = (thingDef.entityDefToBuild as ThingDef);
				if (thingDef == null)
				{
					return;
				}
			}
			InventoryRecord inventoryRecord;
			if (!this.allRecords.TryGetValue(key, out inventoryRecord))
			{
				return;
			}
			if (inventoryRecord.type == InventoryType.UNKNOWN)
			{
				return;
			}
			if (thingDef2 == null)
			{
				inventoryRecord.count += count.count;
				inventoryRecord.compressedCount += count.deepStorageCount;
			}
			else
			{
				inventoryRecord.unfinishedCount += count.count;
			}
			if (thingDef.building == null && inventoryRecord.texture == null)
			{
				TextureColorPair textureColorPair = MaterialResolver.Resolve(thingDef);
				if (textureColorPair != null)
				{
					inventoryRecord.texture = textureColorPair.texture;
					inventoryRecord.color = textureColorPair.color;
				}
			}
			Dictionary<InventoryRecordKey, InventoryRecord> dictionary = this.categorizedRecords[inventoryRecord.type];
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, inventoryRecord);
			}
			if (count.deepStorageCount > 0)
			{
				this.state.compressedStorage = true;
			}
		}
	}
}
