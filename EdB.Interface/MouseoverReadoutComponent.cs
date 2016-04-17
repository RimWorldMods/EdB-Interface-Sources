using System;

namespace EdB.Interface
{
	public class MouseoverReadoutComponent : IRenderedComponent, INamedComponent
	{
		private MouseoverReadout mouseoverReadout;

		public string Name
		{
			get
			{
				return "MouseoverReadout";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public MouseoverReadoutComponent(MouseoverReadout mouseoverReadout)
		{
			this.mouseoverReadout = mouseoverReadout;
		}

		public void OnGUI()
		{
			this.mouseoverReadout.MouseoverReadoutOnGUI();
		}
	}
}
