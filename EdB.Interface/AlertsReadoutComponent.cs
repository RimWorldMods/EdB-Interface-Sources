using RimWorld;
using System;

namespace EdB.Interface
{
	public class AlertsReadoutComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		private AlertsReadout alertsReadout;

		public string Name
		{
			get
			{
				return "AlertsReadout";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public AlertsReadoutComponent(AlertsReadout alertsReadout)
		{
			this.alertsReadout = alertsReadout;
		}

		public void OnGUI()
		{
			this.alertsReadout.AlertsReadoutOnGUI();
		}

		public void Update()
		{
			this.alertsReadout.AlertsReadoutUpdate();
		}
	}
}
