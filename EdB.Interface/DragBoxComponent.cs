using RimWorld;
using System;

namespace EdB.Interface
{
	public class DragBoxComponent : IRenderedComponent, INamedComponent
	{
		private Selector selector;

		public string Name
		{
			get
			{
				return "DragBox";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public DragBoxComponent(Selector selector)
		{
			this.selector = selector;
		}

		public void OnGUI()
		{
			this.selector.dragBox.DragBoxOnGUI();
		}
	}
}
