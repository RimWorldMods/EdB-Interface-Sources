using System;

namespace EdB.Interface
{
	public class GlobalControlsComponent : IRenderedComponent, INamedComponent
	{
		private GlobalControls globalControls;

		public string Name
		{
			get
			{
				return "GlobalControls";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public GlobalControlsComponent(GlobalControls globalControls)
		{
			this.globalControls = globalControls;
		}

		public void OnGUI()
		{
			this.globalControls.GlobalControlsOnGUI();
		}
	}
}
