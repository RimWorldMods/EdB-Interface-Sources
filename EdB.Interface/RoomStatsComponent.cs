using System;

namespace EdB.Interface
{
	public class RoomStatsComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "RoomStats";
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
			RoomStatsDrawer.RoomStatsOnGUI();
		}
	}
}
