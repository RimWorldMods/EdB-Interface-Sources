using System;

namespace EdB.Interface
{
	public class RoomOverlaysComponent : IUpdatedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "RoomOverlays";
			}
		}

		public void Update()
		{
			RoomStatsDrawer.DrawRoomOverlays();
		}
	}
}
