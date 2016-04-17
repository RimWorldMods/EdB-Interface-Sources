using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class UserInterface : UIRoot_Map
	{
		public delegate void UIRootOnGUIDelegate();

		public delegate void UIRootUpdateDelegate();

		public new MainTabsRoot mainTabsRoot = new MainTabsRoot();

		private MouseoverReadout mouseoverReadout = new MouseoverReadout();

		public new GlobalControls globalControls = new GlobalControls();

		private UIRoot_MapIntermediary uiRootIntermediary;

		private UserInterface.UIRootOnGUIDelegate uiRootOnGUIDelegate;

		private UserInterface.UIRootUpdateDelegate uiRootUpdateDelegate;

		private List<IRenderedComponent> renderedComponents = new List<IRenderedComponent>();

		private List<IUpdatedComponent> updatedComponents = new List<IUpdatedComponent>();

		private List<IInitializedComponent> initializedComponents = new List<IInitializedComponent>();

		private HashSet<IComponentWithPreferences> componentsWithPreferences = new HashSet<IComponentWithPreferences>();

		private Dictionary<string, INamedComponent> componentDictionary = new Dictionary<string, INamedComponent>();

		private ScreenSizeMonitor screenSizeMonitor = new ScreenSizeMonitor();

		public ScreenSizeMonitor ScreenSizeMonitor
		{
			get
			{
				return this.screenSizeMonitor;
			}
		}

		public MainTabsRoot MainTabsRoot
		{
			get
			{
				return this.mainTabsRoot;
			}
		}

		public UserInterface()
		{
			Preferences.Instance.Reset();
			this.uiRootIntermediary = new UIRoot_MapIntermediary();
			this.uiRootOnGUIDelegate = (UserInterface.UIRootOnGUIDelegate)Delegate.CreateDelegate(typeof(UserInterface.UIRootOnGUIDelegate), this.uiRootIntermediary, "UIRootOnGUI");
			this.uiRootOnGUIDelegate.GetType().BaseType.BaseType.GetField("m_target", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this.uiRootOnGUIDelegate, this);
			this.uiRootUpdateDelegate = (UserInterface.UIRootUpdateDelegate)Delegate.CreateDelegate(typeof(UserInterface.UIRootUpdateDelegate), this.uiRootIntermediary, "UIRootUpdate");
			this.uiRootUpdateDelegate.GetType().BaseType.BaseType.GetField("m_target", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this.uiRootUpdateDelegate, this);
			this.AddUpdatedComponent(new ComponentColonistTracker());
			this.AddRenderedComponent(new ComponentColonistBar());
			this.AddUpdatedComponent(new ComponentSquadManager());
			this.AddRenderedComponent(new DebugWindowsOpenerComponent(this.debugWindowOpener));
			this.AddInitializedComponent(new ComponentInventory());
			this.AddUpdatedComponent(new ComponentTabReplacement());
			this.AddInitializedComponent(new ComponentMainTabCloseButton());
			this.AddInitializedComponent(new ComponentHideMainTabs());
			this.AddInitializedComponent(new ComponentAlternateTimeDisplay());
			this.AddInitializedComponent(new ComponentPauseOnStart());
			this.AddInitializedComponent(new ComponentAlternateMaterialSelection());
			this.AddInitializedComponent(new ComponentEmptyStockpile());
			this.AddInitializedComponent(new ComponentColorCodedWorkPassions());
			Preferences.Instance.Load();
			HashSet<ICustomTextureComponent> hashSet = new HashSet<ICustomTextureComponent>();
			foreach (IRenderedComponent current in this.renderedComponents)
			{
				ICustomTextureComponent customTextureComponent = current as ICustomTextureComponent;
				if (customTextureComponent != null && !hashSet.Contains(customTextureComponent))
				{
					customTextureComponent.ResetTextures();
					hashSet.Add(customTextureComponent);
				}
			}
			foreach (IUpdatedComponent current2 in this.updatedComponents)
			{
				ICustomTextureComponent customTextureComponent2 = current2 as ICustomTextureComponent;
				if (customTextureComponent2 != null && !hashSet.Contains(customTextureComponent2))
				{
					customTextureComponent2.ResetTextures();
					hashSet.Add(customTextureComponent2);
				}
			}
			foreach (IInitializedComponent current3 in this.initializedComponents)
			{
				ICustomTextureComponent customTextureComponent3 = current3 as ICustomTextureComponent;
				if (customTextureComponent3 != null && !hashSet.Contains(customTextureComponent3))
				{
					customTextureComponent3.ResetTextures();
					hashSet.Add(customTextureComponent3);
				}
			}
			foreach (IInitializedComponent current4 in this.initializedComponents)
			{
				current4.PrepareDependencies(this);
			}
			foreach (IInitializedComponent current5 in this.initializedComponents)
			{
				current5.Initialize(this);
			}
			hashSet.Clear();
			this.ResetTextures();
		}

		private void OpenMainMenuShortcut()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				Event.current.Use();
				this.mainTabsRoot.SetCurrentTab(MainTabDefOf.Menu, true);
			}
		}

		public void AddRenderedComponent(IRenderedComponent component)
		{
			this.InsertRenderedComponent(this.renderedComponents.Count, component);
		}

		public void AddInitializedComponent(IInitializedComponent component)
		{
			if (!this.initializedComponents.Contains(component))
			{
				this.initializedComponents.Add(component);
				if (component is IComponentWithPreferences)
				{
					this.AddComponentPreferences(component as IComponentWithPreferences);
				}
				if (component is INamedComponent)
				{
					this.AddNamedComponent(component as INamedComponent);
				}
			}
		}

		protected void AddNamedComponent(INamedComponent component)
		{
			if (!this.componentDictionary.ContainsKey(component.Name))
			{
				this.componentDictionary.Add(component.Name, component);
			}
		}

		public bool AddRenderedComponentAbove(IRenderedComponent component, string name)
		{
			return this.AddRenderedComponent(component, name, 1);
		}

		public bool AddRenderedComponentBelow(IRenderedComponent component, string name)
		{
			return this.AddRenderedComponent(component, name, 0);
		}

		protected void InsertRenderedComponent(int index, IRenderedComponent component)
		{
			this.renderedComponents.Insert(index, component);
			if (component is IInitializedComponent)
			{
				this.AddInitializedComponent(component as IInitializedComponent);
			}
			if (component is IComponentWithPreferences)
			{
				this.AddComponentPreferences(component as IComponentWithPreferences);
			}
			if (component is INamedComponent)
			{
				this.AddNamedComponent(component as INamedComponent);
			}
		}

		protected void AddComponentPreferences(IComponentWithPreferences component)
		{
			if (!this.componentsWithPreferences.Contains(component))
			{
				foreach (IPreference current in component.Preferences)
				{
					Preferences.Instance.Add(current);
					this.componentsWithPreferences.Add(component);
				}
			}
		}

		public INamedComponent FindNamedComponent(string name)
		{
			INamedComponent result = null;
			if (this.componentDictionary.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		public T FindNamedComponentAs<T>(string name)
		{
			INamedComponent namedComponent = null;
			if (this.componentDictionary.TryGetValue(name, out namedComponent) && namedComponent is T)
			{
				return (T)((object)namedComponent);
			}
			return default(T);
		}

		protected bool AddRenderedComponent(IRenderedComponent component, string name, int offset)
		{
			int num = this.renderedComponents.FindIndex(delegate(IRenderedComponent c)
			{
				INamedComponent namedComponent = c as INamedComponent;
				return namedComponent != null && namedComponent.Name == name;
			});
			if (num == -1)
			{
				return false;
			}
			if (num + offset < 0)
			{
				return false;
			}
			this.InsertRenderedComponent(num + offset, component);
			return true;
		}

		public void AddUpdatedComponent(IUpdatedComponent component)
		{
			this.updatedComponents.Add(component);
			if (component is IInitializedComponent)
			{
				this.AddInitializedComponent(component as IInitializedComponent);
			}
			if (component is IComponentWithPreferences)
			{
				this.AddComponentPreferences(component as IComponentWithPreferences);
			}
			if (component is INamedComponent)
			{
				this.AddNamedComponent(component as INamedComponent);
			}
		}

		public bool ReplaceComponent(object component, string name)
		{
			bool result = false;
			if (this.ReplaceComponentOfType<IRenderedComponent>(component, name, this.renderedComponents))
			{
				result = true;
			}
			if (this.ReplaceComponentOfType<IUpdatedComponent>(component, name, this.updatedComponents))
			{
				result = true;
			}
			if (this.ReplaceComponentOfType<IInitializedComponent>(component, name, this.initializedComponents))
			{
				result = true;
			}
			return result;
		}

		protected bool ReplaceComponentOfType<T>(object component, string name, List<T> list)
		{
			bool result = false;
			if (component is T)
			{
				int num = list.FindIndex(delegate(T c)
				{
					INamedComponent namedComponent = c as INamedComponent;
					return namedComponent != null && namedComponent.Name == name;
				});
				if (num != -1)
				{
					list[num] = (T)((object)component);
					result = true;
				}
			}
			return result;
		}

		public T FindMainTabOfType<T>() where T : MainTabWindow
		{
			foreach (MainTabDef current in this.mainTabsRoot.AllTabs)
			{
				MainTabWindow window = current.Window;
				if (window != null && window is T)
				{
					return (T)((object)window);
				}
			}
			return (T)((object)null);
		}

		public override void UIRootOnGUI()
		{
			this.CallAncestorUIRootOnGUI();
			this.screenSizeMonitor.Update();
			this.thingOverlays.ThingOverlaysOnGUI();
			for (int i = 0; i < Find.Map.components.Count; i++)
			{
				Find.Map.components[i].MapComponentOnGUI();
			}
			bool filtersCurrentEvent = this.screenshotMode.FiltersCurrentEvent;
			foreach (IRenderedComponent current in this.renderedComponents)
			{
				if (!filtersCurrentEvent || current.RenderWithScreenshots)
				{
					current.OnGUI();
				}
			}
			BeautyDrawer.BeautyOnGUI();
			this.selector.dragBox.DragBoxOnGUI();
			DesignatorManager.DesignationManagerOnGUI();
			this.targeter.TargeterOnGUI();
			Find.TooltipGiverList.DispenseAllThingTooltips();
			Find.ColonyInfo.ColonyInfoOnGUI();
			DebugTools.DebugToolsOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.globalControls.GlobalControlsOnGUI();
				this.resourceReadout.ResourceReadoutOnGUI();
				this.mainTabsRoot.MainTabsOnGUI();
				this.mouseoverReadout.MouseoverReadoutOnGUI();
				this.alerts.AlertsReadoutOnGUI();
				ActiveTutorNoteManager.ActiveLessonManagerOnGUI();
			}
			RoomStatsDrawer.RoomStatsOnGUI();
			Find.DebugDrawer.DebugDrawerOnGUI();
			this.windows.WindowStackOnGUI();
			DesignatorManager.ProcessInputEvents();
			this.targeter.ProcessInputEvents();
			this.mainTabsRoot.HandleLowPriorityShortcuts();
			this.selector.SelectorOnGUI();
			this.OpenMainMenuShortcut();
		}

		public override void UIRootUpdate()
		{
			this.CallAncestorUIRootUpdate();
			try
			{
				Messages.Update();
				this.targeter.TargeterUpdate();
				SelectionDrawer.DrawSelectionOverlays();
				RoomStatsDrawer.DrawRoomOverlays();
				DesignatorManager.DesignatorManagerUpdate();
				this.alerts.AlertsReadoutUpdate();
				ConceptDecider.ConceptDeciderUpdate();
				foreach (IUpdatedComponent current in this.updatedComponents)
				{
					current.Update();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception in UIRootUpdate: " + ex.ToString());
			}
		}

		protected void CallAncestorUIRootOnGUI()
		{
			this.uiRootOnGUIDelegate();
		}

		protected void CallAncestorUIRootUpdate()
		{
			this.uiRootUpdateDelegate();
		}

		protected void ResetTextures()
		{
			Button.ResetTextures();
		}
	}
}
