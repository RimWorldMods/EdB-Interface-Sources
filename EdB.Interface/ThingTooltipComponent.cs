using System;
using Verse;

namespace EdB.Interface
{
	public class ThingTooltipComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "ThingTooltips";
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
			Find.TooltipGiverList.DispenseAllThingTooltips();
		}
	}
}
