using System;
using Verse;

namespace EdB.Interface
{
	public class DebugToolsComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "DebugTools";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public void OnGUI()
		{
			DebugTools.DebugToolsOnGUI();
		}
	}
}
