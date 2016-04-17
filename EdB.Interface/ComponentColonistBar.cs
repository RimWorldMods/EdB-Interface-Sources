using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentColonistBar : IRenderedComponent, IInitializedComponent, INamedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		private ColonistBar colonistBar;

		private ColonistBarGroup defaultGroup = new ColonistBarGroup();

		private List<ColonistBarGroup> defaultGroups = new List<ColonistBarGroup>();

		public List<ColonistBarGroup> DefaultGroups
		{
			get
			{
				return this.defaultGroups;
			}
		}

		public ColonistBarGroup DefaultGroup
		{
			get
			{
				return this.defaultGroup;
			}
		}

		public string Name
		{
			get
			{
				return "ColonistBar";
			}
		}

		public ColonistBar ColonistBar
		{
			get
			{
				return this.colonistBar;
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return true;
			}
		}

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.colonistBar.Preferences;
			}
		}

		public ComponentColonistBar()
		{
			this.defaultGroups.Add(this.defaultGroup);
			this.colonistBar = new ColonistBar();
			this.colonistBar.AddGroup(this.defaultGroup);
			this.colonistBar.CurrentGroup = this.defaultGroup;
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			userInterface.ScreenSizeMonitor.Changed += new ScreenSizeMonitor.ScreenSizeChangeHandler(this.colonistBar.UpdateScreenSize);
			ColonistTracker.Instance.ColonistChanged += new ColonistNotificationHandler(this.ColonistNotificationHandler);
		}

		public void Initialize(UserInterface userInterface)
		{
		}

		public void OnGUI()
		{
			this.colonistBar.Draw();
		}

		public void ResetTextures()
		{
			ColonistBar.ResetTextures();
			ColonistBarDrawer.ResetTextures();
		}

		public void ColonistNotificationHandler(ColonistNotification notification)
		{
			if (notification.type == ColonistNotificationType.New)
			{
				this.defaultGroup.Add(notification.colonist);
			}
			else if (notification.type == ColonistNotificationType.Buried || notification.type == ColonistNotificationType.Lost || notification.type == ColonistNotificationType.Deleted)
			{
				this.defaultGroup.Remove(notification.colonist);
			}
		}
	}
}
