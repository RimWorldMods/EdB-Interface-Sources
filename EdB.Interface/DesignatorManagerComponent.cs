using System;
using Verse;

namespace EdB.Interface
{
	public class DesignatorManagerComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "DesignatorManager";
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
			DesignatorManager.DesignationManagerOnGUI();
		}

		public void Update()
		{
			DesignatorManager.DesignatorManagerUpdate();
		}
	}
}
