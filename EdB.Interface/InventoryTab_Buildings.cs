using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryTab_Buildings : InventoryTab
	{
		protected static Vector2 BuildingSlotSize = new Vector2(86f, 86f);

		protected static Vector2 BuildingImageSize = new Vector2(72f, 72f);

		protected static Vector2 BuildingImageOffset;

		protected Dictionary<string, string> labels = new Dictionary<string, string>();

		public InventoryTab_Buildings() : base("EdB.Inventory.Tab.Buildings", 500)
		{
			InventoryTab_Buildings.BuildingImageOffset = new Vector2(InventoryTab_Buildings.BuildingSlotSize.x / 2f - InventoryTab_Buildings.BuildingImageSize.x / 2f, InventoryTab_Buildings.BuildingSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Buildings.BuildingImageSize.y);
			InventoryTab_Buildings.BuildingImageOffset.y = InventoryTab_Buildings.BuildingImageOffset.y;
			this.InitializeCategoryLabels();
		}

		private void InitializeCategoryLabels()
		{
			string[] array = new string[]
			{
				"Furniture",
				"Structure",
				"Power",
				"Security",
				"Production",
				"Joy",
				"Temperature",
				"Ship",
				"FoodUtilities"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				foreach (DesignationCategoryDef current in DefDatabase<DesignationCategoryDef>.AllDefs)
				{
					if (text.Equals(current.defName))
					{
						this.labels.Add(text, current.LabelCap);
						break;
					}
				}
				if (!this.labels.ContainsKey(text))
				{
					this.labels.Add(text, text);
				}
			}
		}

		public override void InventoryTabOnGui(Rect fillRect)
		{
			InventoryPreferences preferences = this.manager.Preferences;
			Rect rect = new Rect(23f, 93f, 928f, 510f);
			GUI.color = new Color(0.08235f, 0.09804f, 0.1137f);
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = new Color(0.5394f, 0.5394f, 0.5394f);
			Widgets.DrawBox(rect, 1);
			try
			{
				Rect position = rect.ContractedBy(1f);
				GUI.BeginGroup(position);
				Rect outRect = new Rect(0f, 0f, position.width, position.height);
				Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - 16f, this.scrollViewHeight);
				try
				{
					Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);
					this.backgroundToggle = false;
					Vector2 position2 = new Vector2(InventoryTab.SectionPaddingSides, 0f);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Furniture"], this.manager.GetInventoryRecord(InventoryType.BUILDING_FURNITURE), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Production"], this.manager.GetInventoryRecord(InventoryType.BUILDING_PRODUCTION), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Power"], this.manager.GetInventoryRecord(InventoryType.BUILDING_POWER), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Security"], this.manager.GetInventoryRecord(InventoryType.BUILDING_SECURITY), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Joy"], this.manager.GetInventoryRecord(InventoryType.BUILDING_JOY), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Temperature"], this.manager.GetInventoryRecord(InventoryType.BUILDING_TEMPERATURE), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Ship"], this.manager.GetInventoryRecord(InventoryType.BUILDING_SHIP), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["Structure"], this.manager.GetInventoryRecord(InventoryType.BUILDING_STRUCTURE), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, this.labels["FoodUtilities"], this.manager.GetInventoryRecord(InventoryType.BUILDING_FOOD_UTILITIES), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Other".Translate(), this.manager.GetInventoryRecord(InventoryType.BUILDING_OTHER), position2, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					if (Event.current.type == EventType.Layout)
					{
						this.scrollViewHeight = position2.y - 1f;
					}
				}
				finally
				{
					Widgets.EndScrollView();
				}
			}
			finally
			{
				GUI.EndGroup();
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			string text = "EdB.Inventory.Prefs.IncludeUnfinished".Translate();
			float num = Text.CalcSize(text).x + 40f;
			float num2 = 22f;
			bool value = preferences.IncludeUnfinished.Value;
			Widgets.LabelCheckbox(new Rect(fillRect.x + fillRect.width - num - num2, fillRect.y + fillRect.height - 38f, num, 30f), text, ref value, false);
			preferences.IncludeUnfinished.Value = value;
		}
	}
}
