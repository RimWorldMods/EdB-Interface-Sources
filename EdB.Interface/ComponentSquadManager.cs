using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentSquadManager : IUpdatedComponent, IInitializedComponent, INamedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceEnableSquads preferenceEnableSquads = new PreferenceEnableSquads();

		protected PreferenceEnableSquadFiltering preferenceEnableSquadFiltering = new PreferenceEnableSquadFiltering();

		protected PreferenceEnableSquadRow preferenceEnableSquadRow = new PreferenceEnableSquadRow();

		protected PreferenceAlwaysShowSquadName preferenceAlwaysShowSquadName = new PreferenceAlwaysShowSquadName();

		protected ColonistBar colonistBar;

		protected Action initializeAction;

		private ColonistBarSquadSupervisor supervisor;

		private SquadShortcuts shortcuts = new SquadShortcuts();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public string Name
		{
			get
			{
				return "SquadManager";
			}
		}

		public ComponentSquadManager()
		{
			SquadManager.Instance.Reset();
			SquadManagerThing.Clear();
			this.preferences.Add(this.preferenceEnableSquads);
			this.preferences.Add(this.preferenceEnableSquadFiltering);
			this.preferences.Add(this.preferenceEnableSquadRow);
			this.preferences.Add(this.preferenceAlwaysShowSquadName);
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preferenceEnableSquadFiltering.PreferenceEnableSquads = this.preferenceEnableSquads;
			this.preferenceEnableSquadRow.PreferenceEnableSquads = this.preferenceEnableSquads;
			SquadManager.Instance.PreferenceEnableSquads = this.preferenceEnableSquads;
			ColonistTracker.Instance.ColonistChanged += new ColonistNotificationHandler(SquadManager.Instance.ColonistChanged);
			ComponentColonistBar colonistBarComponent = userInterface.FindNamedComponentAs<ComponentColonistBar>("ColonistBar");
			if (colonistBarComponent == null)
			{
				return;
			}
			colonistBarComponent.ColonistBar.AlwaysShowGroupName = this.preferenceAlwaysShowSquadName.Value;
			this.preferenceAlwaysShowSquadName.ValueChanged += delegate(bool value)
			{
				colonistBarComponent.ColonistBar.AlwaysShowGroupName = value;
			};
			MainTabDef squadsWindowTabDef = mainTabsRoot.FindTabDef("EdB_Interface_Squads");
			MainTabWindow_Squads mainTabWindow_Squads = squadsWindowTabDef.Window as MainTabWindow_Squads;
			if (squadsWindowTabDef != null)
			{
				this.preferenceEnableSquads.ValueChanged += delegate(bool value)
				{
					this.SquadsEnabledValueChanged(value, squadsWindowTabDef, colonistBarComponent);
				};
			}
			this.supervisor = new ColonistBarSquadSupervisor(colonistBarComponent.ColonistBar);
			SquadManager.Instance.SquadAdded += delegate(Squad Squad)
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			};
			SquadManager.Instance.SquadChanged += delegate(Squad Squad)
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			};
			SquadManager.Instance.SquadRemoved += delegate(Squad s, int i)
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			};
			SquadManager.Instance.SquadDisplayPreferenceChanged += delegate(Squad Squad)
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			};
			SquadManager.Instance.SquadOrderChanged += delegate
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			};
			colonistBarComponent.ColonistBar.SelectedGroupChanged += new ColonistBar.SelectedGroupChangedHandler(this.supervisor.SelectedGroupChanged);
			this.supervisor.SelectedSquadChanged += delegate(Squad squad)
			{
				SquadManager.Instance.CurrentSquad = squad;
			};
			if (mainTabWindow_Squads != null)
			{
				SquadManager.Instance.SquadChanged += new SquadNotificationHandler(mainTabWindow_Squads.SquadChanged);
			}
			this.initializeAction = delegate
			{
				this.SquadsEnabledValueChanged(this.preferenceEnableSquads.Value, squadsWindowTabDef, colonistBarComponent);
			};
			foreach (MainTabWindow_PawnListWithSquads current in mainTabsRoot.FindWindows<MainTabWindow_PawnListWithSquads>())
			{
				current.PreferenceEnableSquadFiltering = this.preferenceEnableSquadFiltering;
				current.PreferenceEnableSquadRow = this.preferenceEnableSquadRow;
				current.PreferenceEnableSquads = this.preferenceEnableSquads;
			}
			this.shortcuts.ColonistBarSquadSupervisor = this.supervisor;
		}

		public void Initialize(UserInterface userInterface)
		{
			if (SquadManager.Instance.SyncWithMap())
			{
				if (SquadManager.Instance.Squads.Count == 0)
				{
					SquadManager.Instance.Squads.Add(SquadManager.Instance.AllColonistsSquad);
				}
				this.supervisor.SelectedSquad = SquadManager.Instance.CurrentSquad;
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			}
			ColonistTracker.Instance.Update();
			this.initializeAction();
			SquadManager.Instance.SyncThingToMap();
		}

		public void Update()
		{
			if (this.preferenceEnableSquads.Value)
			{
				this.shortcuts.Update();
			}
		}

		public void ResetTextures()
		{
			MainTabWindow_Squads.ResetTextures();
		}

		public void SquadsEnabledValueChanged(bool value, MainTabDef squadsWindowTabDef, ComponentColonistBar colonistBarComponent)
		{
			this.supervisor.Enabled = value;
			squadsWindowTabDef.showTabButton = value;
			if (value)
			{
				this.supervisor.SyncSquadsToColonistBar();
				this.supervisor.UpdateColonistBarGroups();
			}
			else
			{
				colonistBarComponent.ColonistBar.UpdateGroups(colonistBarComponent.DefaultGroups, colonistBarComponent.DefaultGroup);
			}
			SquadManager.Instance.SyncThingToMap();
		}
	}
}
