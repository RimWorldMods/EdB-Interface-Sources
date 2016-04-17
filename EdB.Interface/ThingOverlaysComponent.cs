using System;
using Verse;

namespace EdB.Interface
{
	public class ThingOverlaysComponent : IRenderedComponent, INamedComponent
	{
		private ThingOverlays thingOverlays;

		public string Name
		{
			get
			{
				return "ThingOverlays";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public ThingOverlaysComponent(ThingOverlays thingOverlays)
		{
			this.thingOverlays = thingOverlays;
		}

		public void OnGUI()
		{
			this.thingOverlays.ThingOverlaysOnGUI();
		}
	}
}
