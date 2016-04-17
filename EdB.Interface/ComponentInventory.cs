using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentInventory : IInitializedComponent, IComponentWithPreferences
	{
		private InventoryManager inventoryManager;

		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceCompressedStorage preferenceCompressedStorage = new PreferenceCompressedStorage();

		protected PreferenceIncludeUnfinished preferenceIncludeUnfinished = new PreferenceIncludeUnfinished();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentInventory()
		{
			this.preferences.Add(this.preferenceCompressedStorage);
			this.preferences.Add(this.preferenceIncludeUnfinished);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			this.inventoryManager = new InventoryManager();
			this.inventoryManager.PreferenceCompressedStorage = this.preferenceCompressedStorage;
			this.inventoryManager.PreferenceIncludeUnfinished = this.preferenceIncludeUnfinished;
			MainTabDef mainTabDef = userInterface.MainTabsRoot.FindTabDef("EdB_Interface_Inventory");
			MainTabWindow_Inventory mainTabWindow_Inventory = mainTabDef.Window as MainTabWindow_Inventory;
			if (mainTabWindow_Inventory != null)
			{
				mainTabWindow_Inventory.InventoryManager = this.inventoryManager;
			}
		}

		public void Initialize(UserInterface userInterface)
		{
		}
	}
}
