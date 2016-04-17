using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabsRoot
	{
		private List<MainTabDef> allTabs;

		public MainTabDef OpenTab
		{
			get
			{
				MainTabWindow mainTabWindow = Find.WindowStack.WindowOfType<MainTabWindow>();
				if (mainTabWindow == null)
				{
					return null;
				}
				return mainTabWindow.def;
			}
		}

		public List<MainTabDef> AllTabs
		{
			get
			{
				return this.allTabs;
			}
		}

		private int TabButtonsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allTabs.Count; i++)
				{
					if (this.allTabs[i].showTabButton)
					{
						num++;
					}
				}
				return num;
			}
		}

		public MainTabsRoot()
		{
			this.allTabs = (from x in DefDatabase<MainTabDef>.AllDefs
			orderby x.order
			select x).ToList<MainTabDef>();
		}

		private void DoTabButton(MainTabDef def, float posX, float width)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(posX, (float)(Screen.height - 35), width, 35f);
			SoundDef mouseoverButtonCategory = SoundDefOf.MouseoverButtonCategory;
			if (WidgetsSubtle.ButtonSubtle(rect, def.LabelCap, def.Window.TabButtonBarPercent, -1f, mouseoverButtonCategory))
			{
				this.ToggleTab(def, true);
			}
			if (!def.tutorHighlightTag.NullOrEmpty())
			{
				TutorUIHighlighter.HighlightOpportunity(def.tutorHighlightTag, rect);
			}
			if (!def.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, def.description);
			}
		}

		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		private float GetTabButtonPosition(MainTabDef tab)
		{
			int tabButtonsCount = this.TabButtonsCount;
			int num = (int)((float)Screen.width / (float)tabButtonsCount);
			int num2 = 0;
			for (int i = 0; i < this.allTabs.Count; i++)
			{
				if (this.allTabs[i].showTabButton)
				{
					if (this.allTabs[i] == tab)
					{
						return (float)num2;
					}
					num2 += num;
				}
			}
			return 0f;
		}

		private float GetTabButtonWidth(MainTabDef tab)
		{
			int tabButtonsCount = this.TabButtonsCount;
			int num = 0;
			for (int i = 0; i < this.allTabs.Count; i++)
			{
				if (this.allTabs[i].showTabButton)
				{
					num = i;
				}
			}
			int num2 = (int)((float)Screen.width / (float)tabButtonsCount);
			int num3 = 0;
			for (int j = 0; j < this.allTabs.Count; j++)
			{
				if (this.allTabs[j].showTabButton)
				{
					if (this.allTabs[j] == tab)
					{
						if (j == num)
						{
							return (float)(Screen.width - num3);
						}
						return (float)num2;
					}
					else
					{
						num3 += num2;
					}
				}
			}
			return 0f;
		}

		public void HandleLowPriorityShortcuts()
		{
			if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				Event.current.Use();
				this.ToggleTab(MainTabDefOf.Architect, true);
			}
			if (this.OpenTab != MainTabDefOf.Inspect && Event.current.type == EventType.MouseDown && Event.current.button != 2)
			{
				this.EscapeCurrentTab(true);
				Find.Selector.ClearSelection();
			}
		}

		public void MainTabsOnGUI()
		{
			GUI.color = Color.white;
			for (int i = 0; i < this.allTabs.Count; i++)
			{
				if (this.allTabs[i].showTabButton)
				{
					this.DoTabButton(this.allTabs[i], this.GetTabButtonPosition(this.allTabs[i]), this.GetTabButtonWidth(this.allTabs[i]));
				}
			}
			for (int j = 0; j < this.allTabs.Count; j++)
			{
				if (this.allTabs[j].toggleHotKey != null && this.allTabs[j].toggleHotKey.KeyDownEvent)
				{
					this.ToggleTab(this.allTabs[j], true);
					Event.current.Use();
					break;
				}
			}
			if (this.OpenTab == MainTabDefOf.Inspect && Find.Selector.NumSelected == 0)
			{
				this.EscapeCurrentTab(false);
			}
		}

		public void SetCurrentTab(MainTabDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab)
			{
				return;
			}
			this.ToggleTab(tab, playSound);
		}

		public void ToggleTab(MainTabDef newTab, bool playSound = true)
		{
			MainTabDef openTab = this.OpenTab;
			if (openTab == null && newTab == null)
			{
				return;
			}
			if (openTab == newTab)
			{
				Find.WindowStack.TryRemove(openTab.Window, true);
				if (playSound)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera();
				}
			}
			else
			{
				if (openTab != null)
				{
					Find.WindowStack.TryRemove(openTab.Window, true);
				}
				if (newTab != null)
				{
					Find.WindowStack.Add(newTab.Window);
				}
				if (playSound)
				{
					if (newTab == null)
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera();
					}
					else
					{
						SoundDefOf.TabOpen.PlayOneShotOnCamera();
					}
				}
			}
		}

		public MainTabWindow FindWindow(string defName)
		{
			MainTabDef mainTabDef = this.AllTabs.FirstOrDefault((MainTabDef d) => d.defName == defName);
			if (mainTabDef != null)
			{
				return mainTabDef.Window;
			}
			return null;
		}

		public T FindWindow<T>() where T : MainTabWindow
		{
			Type targetType = typeof(T);
			MainTabDef mainTabDef = this.AllTabs.FirstOrDefault((MainTabDef d) => d.Window != null && targetType.IsAssignableFrom(d.Window.GetType()));
			if (mainTabDef != null)
			{
				return (T)((object)mainTabDef.Window);
			}
			return (T)((object)null);
		}

		public IEnumerable<T> FindWindows<T>() where T : MainTabWindow
		{
			Type targetType = typeof(T);
			return (from d in this.AllTabs.FindAll((MainTabDef d) => d.Window != null && targetType.IsAssignableFrom(d.Window.GetType()))
			select d.Window).Cast<T>();
		}

		public MainTabDef FindTabDef(string defName)
		{
			return this.AllTabs.FirstOrDefault((MainTabDef d) => d.defName == defName);
		}
	}
}
