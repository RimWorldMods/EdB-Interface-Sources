using System;

namespace EdB.Interface
{
	public class BeautyDrawerComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "BeautyDrawer";
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
			BeautyDrawer.BeautyOnGUI();
		}
	}
}
