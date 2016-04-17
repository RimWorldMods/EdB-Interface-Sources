using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryTab_Items : InventoryTab
	{
		protected static Vector2 ItemSlotSize = new Vector2(72f, 68f);

		protected static Vector2 WeaponSlotSize = new Vector2(76f, 78f);

		protected static Vector2 ResourceImageSize = new Vector2(48f, 48f);

		protected static Vector2 ResourceImageOffset;

		protected static Vector2 ApparelImageSize = new Vector2(52f, 52f);

		protected static Vector2 ApparelImageOffset;

		protected static Vector2 WeaponImageSize = new Vector2(64f, 64f);

		protected static Vector2 WeaponImageOffset;

		public InventoryTab_Items() : base("EdB.Inventory.Tab.ItemsAndResources", 1000)
		{
			InventoryTab_Items.WeaponImageOffset = new Vector2(InventoryTab_Items.ItemSlotSize.x / 2f - InventoryTab_Items.WeaponImageSize.x / 2f, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.WeaponImageSize.y);
			InventoryTab_Items.WeaponImageOffset.y = InventoryTab_Items.WeaponImageOffset.y + 6f;
			InventoryTab_Items.ResourceImageOffset = new Vector2(InventoryTab_Items.ItemSlotSize.x / 2f - InventoryTab_Items.ResourceImageSize.x / 2f, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.ResourceImageSize.y);
			InventoryTab_Items.ResourceImageOffset.y = InventoryTab_Items.ResourceImageOffset.y - 4f;
			InventoryTab_Items.ApparelImageOffset = new Vector2(InventoryTab_Items.ItemSlotSize.x / 2f - InventoryTab_Items.ApparelImageSize.x / 2f, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.ApparelImageSize.y);
			InventoryTab_Items.ApparelImageOffset.y = InventoryTab_Items.ApparelImageOffset.y - 6f;
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
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Resources".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_RESOURCE), position2, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Food".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_FOOD), position2, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Weapons".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_EQUIPMENT), position2, InventoryTab_Items.WeaponSlotSize, InventoryTab_Items.WeaponImageOffset, InventoryTab_Items.WeaponImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Apparel".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_APPAREL), position2, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ApparelImageOffset, InventoryTab_Items.ApparelImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Schematics".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_SCHEMATIC), position2, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position2 = base.DrawResourceSection(viewRect.width, "EdB.Inventory.Section.Other".Translate(), this.manager.GetInventoryRecord(InventoryType.ITEM_OTHER), position2, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
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
			if (this.manager.CompressedStorage)
			{
				string text = "EdB.Inventory.Prefs.CompressedStorage".Translate();
				float num = Text.CalcSize(text).x + 32f;
				float num2 = 22f;
				Rect rect2 = new Rect(fillRect.x + fillRect.width - num - num2, fillRect.y + fillRect.height - 38f, num, 30f);
				bool value = preferences.CompressedStorage.Value;
				Widgets.LabelCheckbox(rect2, text, ref value, false);
				preferences.CompressedStorage.Value = value;
			}
		}
	}
}
