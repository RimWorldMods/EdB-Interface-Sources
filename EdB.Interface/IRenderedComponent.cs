using System;

namespace EdB.Interface
{
	public interface IRenderedComponent
	{
		bool RenderWithScreenshots
		{
			get;
		}

		void OnGUI();
	}
}
