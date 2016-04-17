using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace EdB.Interface
{
	public abstract class InventoryTab
	{
		protected InventoryManager manager;

		protected static Vector2 LabelOffset = new Vector2(0f, 12f);

		protected static float SlotRowPadding = 12f;

		protected static float SectionPaddingTop = 9f;

		protected static float SectionPaddingBottom = 18f;

		protected static float SectionPaddingSides = 12f;

		protected static float SectionPaddingBelowLabel = 0f;

		protected static float SectionLabelHorizontalPadding = 4f;

		protected static GameFont SectionLabelFont = GameFont.Small;

		protected bool backgroundToggle;

		protected Vector2 scrollPosition = Vector2.zero;

		protected float scrollViewHeight;

		public int order;

		public string title = "Untitled";

		protected FloatMenu equipmentAssignmentFloatMenu;

		public InventoryManager InventoryManager
		{
			get
			{
				return this.manager;
			}
			set
			{
				this.manager = value;
			}
		}

		public InventoryTab(string title, int order)
		{
			this.title = title.Translate();
			this.order = order;
		}

		public abstract void InventoryTabOnGui(Rect fillRect);

		protected Vector2 DrawResourceSection(float width, string label, List<InventoryRecord> records, Vector2 position, Vector2 slotSize, Vector2 offset, Vector2 iconSize, InventoryPreferences prefs)
		{
			float num = width - 20f;
			if (records == null)
			{
				return position;
			}
			if (prefs.IncludeUnfinished.Value)
			{
				if (records.Count == 0)
				{
					return position;
				}
			}
			else
			{
				bool flag = false;
				foreach (InventoryRecord current in records)
				{
					if (current.count > 0 || (current.compressedCount > 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount > 0 && prefs.IncludeUnfinished.Value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return position;
				}
			}
			Vector2 vector = new Vector2(position.x, position.y);
			if (vector.x != InventoryTab.SectionPaddingSides)
			{
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			float num2 = this.MeasureResourceSection(width, label, records, slotSize, prefs);
			if (this.backgroundToggle)
			{
				GUI.color = new Color(0.082f, 0.0977f, 0.1133f);
			}
			else
			{
				GUI.color = new Color(0.1094f, 0.125f, 0.1406f);
			}
			GUI.DrawTexture(new Rect(0f, vector.y, width + 16f, num2), BaseContent.WhiteTex);
			GUI.color = new Color(0.3294f, 0.3294f, 0.3294f);
			Widgets.DrawLineHorizontal(0f, vector.y + num2 - 1f, width + 16f);
			this.backgroundToggle = !this.backgroundToggle;
			vector.y += InventoryTab.SectionPaddingTop;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Text.Font = InventoryTab.SectionLabelFont;
			float num3 = Text.CalcHeight(label, num);
			Rect rect = new Rect(InventoryTab.SectionPaddingSides + InventoryTab.SectionLabelHorizontalPadding, vector.y, num - InventoryTab.SectionLabelHorizontalPadding, num3);
			Widgets.Label(rect, label);
			vector.y += num3 + InventoryTab.SectionPaddingBelowLabel;
			vector = this.DrawResources(num, records, vector, iconSize, offset, slotSize, prefs);
			if (vector.x != InventoryTab.SectionPaddingSides)
			{
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			vector.y += InventoryTab.SectionPaddingBottom;
			return vector;
		}

		protected float MeasureResourceSection(float width, string label, List<InventoryRecord> records, Vector2 slotSize, InventoryPreferences prefs)
		{
			float num = width - 20f;
			Vector2 vector = new Vector2(InventoryTab.SectionPaddingSides, InventoryTab.SectionPaddingTop);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = InventoryTab.SectionLabelFont;
			vector.y += Text.CalcHeight(label, num) + InventoryTab.SectionPaddingBelowLabel;
			foreach (InventoryRecord current in records)
			{
				if (current.count != 0 || (current.compressedCount != 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount != 0 && prefs.IncludeUnfinished.Value))
				{
					if (vector.x + slotSize.x > num)
					{
						vector.y += slotSize.y;
						vector.y += InventoryTab.SlotRowPadding;
						vector.x = InventoryTab.SectionPaddingSides;
					}
					vector.x += slotSize.x;
				}
			}
			if (vector.x != InventoryTab.SectionPaddingSides)
			{
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			vector.y += InventoryTab.SectionPaddingBottom;
			return vector.y;
		}

		protected Thing FindClosestValidThing(InventoryRecord record, Pawn pawn)
		{
			List<EquippableThing> list = this.manager.EquippableThings[new InventoryRecordKey(record.thingDef, record.stuffDef)];
			foreach (EquippableThing current in list)
			{
				Thing thing2 = current.thing;
				current.distance = (pawn.Position - thing2.Position).LengthHorizontalSquared;
			}
			IEnumerable<Thing> enumerable = from thing in list
			orderby thing.distance
			select thing.thing;
			foreach (Thing current2 in enumerable)
			{
				if (pawn.CanReach(current2, PathEndMode.ClosestTouch, Danger.Deadly, false))
				{
					if (pawn.CanReserveAndReach(new TargetInfo(current2), PathEndMode.ClosestTouch, Danger.Deadly, 1))
					{
						return current2;
					}
				}
			}
			return null;
		}

		protected void AssignEquipmentToPawn(InventoryRecord record, Pawn pawn)
		{
			Thing thing = this.FindClosestValidThing(record, pawn);
			if (thing == null)
			{
				Messages.Message("EdB.Inventory.Equipment.Error".Translate(), MessageSound.Negative);
				SoundDefOf.DesignateFailed.PlayOneShotOnCamera();
				return;
			}
			Thing thing2 = (thing.def.equipmentType == EquipmentType.None) ? null : thing;
			Apparel apparel = thing as Apparel;
			if (thing2 != null)
			{
				thing2.SetForbidden(false, true);
				Job job = new Job(JobDefOf.Equip, new TargetInfo(thing2));
				job.playerForced = true;
				pawn.drafter.TakeOrderedJob(job);
			}
			else if (apparel != null)
			{
				apparel.SetForbidden(false, true);
				Job job2 = new Job(JobDefOf.Wear, new TargetInfo(apparel));
				job2.playerForced = true;
				pawn.drafter.TakeOrderedJob(job2);
			}
			SoundDefOf.ColonistOrdered.PlayOneShotOnCamera();
			this.manager.TakeInventory();
		}

		protected Vector2 DrawResources(float width, List<InventoryRecord> records, Vector2 position, Vector2 iconSize, Vector2 offset, Vector2 slotSize, InventoryPreferences prefs)
		{
			if (this.equipmentAssignmentFloatMenu != null && Find.WindowStack[0] != this.equipmentAssignmentFloatMenu)
			{
				this.equipmentAssignmentFloatMenu = null;
			}
			Vector2 result = new Vector2(position.x, position.y);
			foreach (InventoryRecord current in records)
			{
				if (current.count != 0 || (current.compressedCount != 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount != 0 && prefs.IncludeUnfinished.Value))
				{
					if (result.x + slotSize.x > width)
					{
						result.y += slotSize.y;
						result.y += InventoryTab.SlotRowPadding;
						result.x = InventoryTab.SectionPaddingSides;
					}
					if (current.texture != null)
					{
						Vector2 vector = new Vector2(0f, 0f);
						if (current.thingDef.apparel != null && current.thingDef.apparel.LastLayer == ApparelLayer.Overhead)
						{
							vector.y += 10f;
						}
						Rect rect = new Rect(result.x + offset.x + vector.x, result.y + offset.y + vector.y, iconSize.x, iconSize.y);
						GUI.color = current.color;
						if (current.thingDef.building == null)
						{
							GUI.DrawTexture(rect, current.thingDef.uiIcon);
							if (current.availableCount > 0 && this.equipmentAssignmentFloatMenu == null && Widgets.InvisibleButton(rect) && Event.current.button == 1)
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								foreach (Pawn current2 in Find.ListerPawns.FreeColonists)
								{
									if (current2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
									{
										InventoryRecord assignedRecord = current;
										Pawn assignedPawn = current2;
										list.Add(new FloatMenuOption(current2.LabelBaseShort, delegate
										{
											this.AssignEquipmentToPawn(assignedRecord, assignedPawn);
										}, MenuOptionPriority.Medium, null, null));
									}
								}
								this.equipmentAssignmentFloatMenu = new FloatMenu(list, "EdB.Inventory.Equipment.Assign".Translate(), false, false);
								Find.WindowStack.Add(this.equipmentAssignmentFloatMenu);
							}
						}
						else
						{
							if (current.texture == null)
							{
								Log.Warning("No texture for building: " + current.thingDef.defName);
							}
							Widgets.DrawTextureFitted(rect, current.texture as Texture2D, current.buildingScale, current.proportions, new Rect(0f, 0f, 1f, 1f));
						}
						if (this.equipmentAssignmentFloatMenu == null)
						{
							string tooltipText = GenLabel.ThingLabel(current.thingDef, current.stuffDef, 1).CapitalizeFirst() + (string.IsNullOrEmpty(current.thingDef.description) ? string.Empty : ("\n\n" + current.thingDef.description));
							if (current.availableCount > -1)
							{
								tooltipText = string.Concat(new string[]
								{
									tooltipText,
									"\n\n",
									"EdB.Inventory.Equipment.Equipped".Translate(new object[]
									{
										current.count - current.reservedCount - current.availableCount
									}),
									"\n",
									"EdB.Inventory.Equipment.Reserved".Translate(new object[]
									{
										current.reservedCount
									}),
									"\n",
									"EdB.Inventory.Equipment.Available".Translate(new object[]
									{
										current.availableCount
									})
								});
							}
							TipSignal tip = new TipSignal(tooltipText, tooltipText.GetHashCode());
							TooltipHandler.TipRegion(rect, tip);
						}
						Text.Anchor = TextAnchor.UpperCenter;
						GUI.color = Color.white;
						Text.Font = GameFont.Tiny;
						string label;
						if (prefs.IncludeUnfinished.Value && current.thingDef.building != null)
						{
							label = string.Empty + current.count + ((current.unfinishedCount <= 0) ? string.Empty : (" / " + (current.count + current.unfinishedCount)));
						}
						else if (prefs.CompressedStorage.Value && current.compressedCount > 0)
						{
							label = string.Concat(new object[]
							{
								string.Empty,
								current.count,
								" / ",
								current.count + current.compressedCount
							});
						}
						else
						{
							label = string.Empty + current.count;
						}
						Widgets.Label(new Rect(result.x, result.y + slotSize.y - InventoryTab.LabelOffset.y, slotSize.x, slotSize.y), label);
						result.x += slotSize.x;
					}
				}
			}
			return result;
		}
	}
}
