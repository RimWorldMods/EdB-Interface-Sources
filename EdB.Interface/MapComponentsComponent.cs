using System;
using Verse;

namespace EdB.Interface
{
	public class MapComponentsComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "MapComponents";
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
			for (int i = 0; i < Find.Map.components.Count; i++)
			{
				Find.Map.components[i].MapComponentOnGUI();
			}
		}
	}
}
