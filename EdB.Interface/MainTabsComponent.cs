using System;

namespace EdB.Interface
{
	public class MainTabsComponent : IRenderedComponent, INamedComponent
	{
		public static readonly string ComponentName = "MainTabs";

		private MainTabsRoot mainTabsRoot;

		public string Name
		{
			get
			{
				return MainTabsComponent.ComponentName;
			}
		}

		public MainTabsRoot MainTabsRoot
		{
			get
			{
				return this.mainTabsRoot;
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public MainTabsComponent(MainTabsRoot mainTabsRoot)
		{
			this.mainTabsRoot = mainTabsRoot;
		}

		public void OnGUI()
		{
			this.mainTabsRoot.MainTabsOnGUI();
		}
	}
}
