using RimWorld;
using System;

namespace EdB.Interface
{
	public class SelectionOverlaysComponent : IUpdatedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "SelectionOverlays";
			}
		}

		public void Update()
		{
			SelectionDrawer.DrawSelectionOverlays();
		}
	}
}
