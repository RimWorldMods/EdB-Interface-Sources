using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Inventory : MainTabWindow
	{
		private InventoryTab curTab;

		private List<InventoryTab> tabs = new List<InventoryTab>();

		protected InventoryManager inventoryManager;

		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		public InventoryManager InventoryManager
		{
			get
			{
				return this.inventoryManager;
			}
			set
			{
				this.inventoryManager = value;
				foreach (InventoryTab current in this.tabs)
				{
					current.InventoryManager = value;
				}
			}
		}

		public MainTabWindow_Inventory()
		{
			this.doCloseX = true;
			this.tabs.Add(new InventoryTab_Items());
			this.tabs.Add(new InventoryTab_Buildings());
			this.tabs = (from t in this.tabs
			orderby t.order descending
			select t).ToList<InventoryTab>();
			this.curTab = this.tabs[0];
		}

		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, -10f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "EdB.Inventory.Window.Header".Translate(new object[]
			{
				Find.ColonyInfo.ColonyName
			}));
			float num = 70f;
			Rect rect2 = new Rect(0f, num, inRect.width, inRect.height - num);
			Widgets.DrawMenuSection(rect2, true);
			this.curTab.InventoryTabOnGui(rect2);
			List<TabRecord> tabsEnum = (from panel in this.tabs
			select new TabRecord(panel.title, delegate
			{
				this.curTab = panel;
			}, panel == this.curTab)).ToList<TabRecord>();
			Verse.TabDrawer.DrawTabs(rect2, tabsEnum);
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (this.inventoryManager != null)
			{
				this.TakeInventory();
			}
		}

		public override void PreClose()
		{
			base.PreClose();
			Preferences.Instance.Save();
		}

		public void TakeInventory()
		{
			if (this.inventoryManager != null)
			{
				try
				{
					this.inventoryManager.TakeInventory();
				}
				catch (Exception ex)
				{
					Log.Error("EdB Interface inventory dialog failed to count all inventory");
					throw ex;
				}
			}
			else
			{
				Log.Warning("InventoryManager was null.  Could not take inventory");
			}
		}
	}
}
