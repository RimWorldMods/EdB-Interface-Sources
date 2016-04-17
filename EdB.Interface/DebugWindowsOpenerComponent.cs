using System;
using Verse;

namespace EdB.Interface
{
	public class DebugWindowsOpenerComponent : IRenderedComponent, INamedComponent
	{
		private DebugWindowsOpener debugWindowsOpener;

		public string Name
		{
			get
			{
				return "DebugWindowsOpener";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public DebugWindowsOpenerComponent(DebugWindowsOpener debugWindowsOpener)
		{
			this.debugWindowsOpener = debugWindowsOpener;
		}

		public void OnGUI()
		{
			this.debugWindowsOpener.DevToolStarterOnGUI();
		}
	}
}
