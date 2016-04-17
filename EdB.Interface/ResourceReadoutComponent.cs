using RimWorld;
using System;

namespace EdB.Interface
{
	public class ResourceReadoutComponent : IRenderedComponent, INamedComponent
	{
		private ResourceReadout resourceReadout;

		public string Name
		{
			get
			{
				return "ResourceReadout";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public ResourceReadoutComponent(ResourceReadout resourceReadout)
		{
			this.resourceReadout = resourceReadout;
		}

		public void OnGUI()
		{
			this.resourceReadout.ResourceReadoutOnGUI();
		}
	}
}
