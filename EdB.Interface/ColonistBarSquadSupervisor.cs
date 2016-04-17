using System;
using System.Collections.Generic;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class ColonistBarSquadSupervisor
	{
		public delegate void SelectedSquadChangedHandler(Squad Squad);

		private List<ColonistBarGroup> colonistBarGroups = new List<ColonistBarGroup>();

		private Squad selectedSquad;

		private ColonistBarGroup allColonistsGroup;

		private Dictionary<ColonistBarGroup, Squad> squadDictionary = new Dictionary<ColonistBarGroup, Squad>();

		private Dictionary<Squad, ColonistBarGroup> groupDictionary = new Dictionary<Squad, ColonistBarGroup>();

		private Dictionary<ColonistBarGroup, Squad> squadDictionaryScratch = new Dictionary<ColonistBarGroup, Squad>();

		private Dictionary<Squad, ColonistBarGroup> groupDictionaryScratch = new Dictionary<Squad, ColonistBarGroup>();

		private ColonistBar colonistBar;

		private bool enabled = true;

		protected List<ColonistBarGroup> scratchGroups = new List<ColonistBarGroup>();

		public event ColonistBarSquadSupervisor.SelectedSquadChangedHandler SelectedSquadChanged
		{
			add
			{
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler = this.SelectedSquadChanged;
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler2;
				do
				{
					selectedSquadChangedHandler2 = selectedSquadChangedHandler;
					selectedSquadChangedHandler = Interlocked.CompareExchange<ColonistBarSquadSupervisor.SelectedSquadChangedHandler>(ref this.SelectedSquadChanged, (ColonistBarSquadSupervisor.SelectedSquadChangedHandler)Delegate.Combine(selectedSquadChangedHandler2, value), selectedSquadChangedHandler);
				}
				while (selectedSquadChangedHandler != selectedSquadChangedHandler2);
			}
			remove
			{
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler = this.SelectedSquadChanged;
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler2;
				do
				{
					selectedSquadChangedHandler2 = selectedSquadChangedHandler;
					selectedSquadChangedHandler = Interlocked.CompareExchange<ColonistBarSquadSupervisor.SelectedSquadChangedHandler>(ref this.SelectedSquadChanged, (ColonistBarSquadSupervisor.SelectedSquadChangedHandler)Delegate.Remove(selectedSquadChangedHandler2, value), selectedSquadChangedHandler);
				}
				while (selectedSquadChangedHandler != selectedSquadChangedHandler2);
			}
		}

		public Squad SelectedSquad
		{
			get
			{
				return this.selectedSquad;
			}
			set
			{
				bool flag = this.selectedSquad != value;
				this.selectedSquad = value;
				if (flag && this.SelectedSquadChanged != null)
				{
					this.SelectedSquadChanged(this.selectedSquad);
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		public ColonistBarSquadSupervisor(ColonistBar colonistBar)
		{
			this.colonistBar = colonistBar;
		}

		public void SyncSquadsToColonistBar()
		{
			if (!this.enabled)
			{
				return;
			}
			SquadManager instance = SquadManager.Instance;
			ColonistTracker instance2 = ColonistTracker.Instance;
			AllColonistsSquad allColonistsSquad = SquadManager.Instance.AllColonistsSquad;
			this.groupDictionaryScratch.Clear();
			this.squadDictionaryScratch.Clear();
			this.colonistBarGroups.Clear();
			int count = instance.Squads.Count;
			for (int i = 0; i < count; i++)
			{
				Squad squad = instance.Squads[i];
				ColonistBarGroup colonistBarGroup = null;
				if (this.groupDictionary.TryGetValue(squad, out colonistBarGroup) && squad == allColonistsSquad)
				{
					this.allColonistsGroup = colonistBarGroup;
				}
				if (squad.Pawns.Count > 0 && squad.ShowInColonistBar)
				{
					bool flag = false;
					if (colonistBarGroup == null)
					{
						colonistBarGroup = new ColonistBarGroup(squad.Pawns.Count);
						flag = true;
					}
					else if (colonistBarGroup.OrderHash != squad.OrderHash)
					{
						flag = true;
					}
					if (flag)
					{
						colonistBarGroup.Clear();
						colonistBarGroup.Name = squad.Name;
						colonistBarGroup.Id = squad.Id;
						foreach (Pawn current in squad.Pawns)
						{
							TrackedColonist trackedColonist = instance2.FindTrackedColonist(current);
							if (trackedColonist != null)
							{
								colonistBarGroup.Add(trackedColonist);
							}
						}
					}
					this.colonistBarGroups.Add(colonistBarGroup);
					this.groupDictionaryScratch[squad] = colonistBarGroup;
					this.squadDictionaryScratch[colonistBarGroup] = squad;
				}
			}
			Dictionary<ColonistBarGroup, Squad> dictionary = this.squadDictionary;
			Dictionary<Squad, ColonistBarGroup> dictionary2 = this.groupDictionary;
			this.groupDictionary = this.groupDictionaryScratch;
			this.squadDictionary = this.squadDictionaryScratch;
			this.groupDictionaryScratch = dictionary2;
			this.squadDictionaryScratch = dictionary;
		}

		public void UpdateColonistBarGroups()
		{
			if (!this.enabled)
			{
				return;
			}
			if (this.colonistBarGroups.Count == 0)
			{
				this.colonistBar.UpdateGroups(this.colonistBarGroups, null);
				return;
			}
			if (this.selectedSquad != null)
			{
				ColonistBarGroup selected = null;
				if (this.groupDictionary.TryGetValue(this.selectedSquad, out selected))
				{
					this.colonistBar.UpdateGroups(this.colonistBarGroups, selected);
					return;
				}
			}
			if (SquadManager.Instance.AllColonistsSquad.ShowInColonistBar)
			{
				this.colonistBar.UpdateGroups(this.colonistBarGroups, this.allColonistsGroup);
			}
			else
			{
				this.colonistBar.UpdateGroups(this.colonistBarGroups, null);
			}
		}

		public void SelectedGroupChanged(ColonistBarGroup group)
		{
			if (!this.enabled)
			{
				return;
			}
			if (group != null)
			{
				Squad allColonistsSquad = SquadManager.Instance.AllColonistsSquad;
				this.squadDictionary.TryGetValue(group, out allColonistsSquad);
				this.SelectedSquad = allColonistsSquad;
			}
			else
			{
				this.SelectedSquad = null;
			}
		}

		public void SelectNextSquad(int direction)
		{
			this.colonistBar.SelectNextGroup(direction);
		}

		public void SelectFavorite(int index)
		{
			Squad favorite = SquadManager.Instance.GetFavorite(index);
			if (favorite != null && favorite.ShowInColonistBar)
			{
				ColonistBarGroup colonistBarGroup = this.groupDictionary[favorite];
				if (colonistBarGroup != null)
				{
					this.colonistBar.CurrentGroup = colonistBarGroup;
				}
			}
		}

		public bool SaveCurrentSquadAsFavorite(int index)
		{
			return this.selectedSquad != null && SquadManager.Instance.SetFavorite(index, this.selectedSquad);
		}

		public void SelectAllPawnsInFavorite(int index)
		{
			if (SquadManager.Instance.GetFavorite(index) != null)
			{
				this.colonistBar.SelectAllPawns();
			}
		}
	}
}
