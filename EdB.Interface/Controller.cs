using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Controller : MonoBehaviour
	{
		public static readonly string ModName = "EdB Interface";

		public static readonly string GameObjectName = "EdBInterfaceController";

		protected Window currentWindow;

		protected Window previousWindow;

		protected bool gameplay;

		protected RootMap replacementRootMap;

		protected Type uiRootType;

		public Window TopWindow
		{
			get
			{
				foreach (Window current in Find.WindowStack.Windows)
				{
					if (current.GetType().FullName != "Verse.EditWindow_Log")
					{
						return current;
					}
				}
				return null;
			}
		}

		public bool ModEnabled
		{
			get
			{
				InstalledMod installedMod = InstalledModLister.AllInstalledMods.First((InstalledMod m) => m.Name.Equals(Controller.ModName));
				return installedMod != null && installedMod.Active;
			}
		}

		public virtual void Start()
		{
			base.enabled = true;
		}

		public void OnLevelWasLoaded(int level)
		{
			if (level == 0)
			{
				this.gameplay = false;
				base.enabled = true;
			}
			else if (level == 1)
			{
				this.gameplay = true;
				base.enabled = true;
			}
		}

		public virtual void Update()
		{
			try
			{
				if (!this.gameplay)
				{
					this.MenusUpdate();
				}
				else
				{
					this.GameplayUpdate();
				}
			}
			catch (Exception ex)
			{
				base.enabled = false;
				Log.Error(ex.ToString());
			}
		}

		public virtual void GameplayUpdate()
		{
			Root rootRoot = Find.RootRoot;
			if (rootRoot != null)
			{
				UIRoot uiRoot = rootRoot.uiRoot;
				if (uiRoot != null)
				{
					if (!uiRoot.GetType().Equals(this.uiRootType))
					{
						this.uiRootType = uiRoot.GetType();
					}
					if (uiRoot.GetType().Equals(typeof(UIRoot_Map)))
					{
						if (this.ModEnabled)
						{
							try
							{
								this.ReplaceUIRoot();
								Log.Message("Replaced standard gameplay interface with EdB Interface");
							}
							catch (Exception ex)
							{
								Log.Error("Failed to replace gameplay interface with EdB Interface");
								Log.Error(ex.ToString());
							}
							base.enabled = false;
						}
					}
					else
					{
						Log.Message("EdB Interface mod not enabled.  Will not replace gameplay interface");
					}
				}
			}
		}

		public void ReplaceUIRoot()
		{
			UIRoot_Map uIRoot_Map = Find.UIRoot_Map;
			if (uIRoot_Map == null)
			{
				Log.Error("No user interface found.  Cannot replace with the EdB interface.");
				return;
			}
			UserInterface userInterface = new UserInterface();
			userInterface.windows = uIRoot_Map.windows;
			Root rootRoot = Find.RootRoot;
			rootRoot.uiRoot = userInterface;
		}

		public virtual void MenusUpdate()
		{
			bool flag = false;
			Window topWindow = this.TopWindow;
			if (topWindow != this.currentWindow)
			{
				this.previousWindow = this.currentWindow;
				this.currentWindow = topWindow;
				flag = true;
			}
			if (flag && this.previousWindow != null && (this.previousWindow.GetType().FullName.Equals("RimWorld.Page_ModsConfig") || this.previousWindow.GetType().FullName.Equals("EdB.ModOrder.Page_ModsConfig")) && this.currentWindow == null && !this.ModEnabled)
			{
				this.UnloadMod();
				return;
			}
		}

		protected void UnloadMod()
		{
			FieldInfo field = typeof(ITabManager).GetField("sharedInstances", BindingFlags.Static | BindingFlags.NonPublic);
			Dictionary<Type, ITab> dictionary = (Dictionary<Type, ITab>)field.GetValue(null);
			dictionary.Remove(typeof(Bootstrap));
			GameObject obj = GameObject.Find(Controller.GameObjectName);
			UnityEngine.Object.Destroy(obj);
			Log.Message("Unloaded " + Controller.ModName);
		}
	}
}
