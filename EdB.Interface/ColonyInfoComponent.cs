using System;
using Verse;

namespace EdB.Interface
{
	public class ColonyInfoComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "ColonyInfo";
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
			Find.ColonyInfo.ColonyInfoOnGUI();
		}
	}
}
