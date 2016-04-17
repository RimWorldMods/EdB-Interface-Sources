using RimWorld;
using System;

namespace EdB.Interface
{
	public class TargeterComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		private Targeter targeter;

		public string Name
		{
			get
			{
				return "Targeter";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public TargeterComponent(Targeter targeter)
		{
			this.targeter = targeter;
		}

		public void OnGUI()
		{
			this.targeter.TargeterOnGUI();
		}

		public void Update()
		{
			this.targeter.TargeterUpdate();
		}
	}
}
