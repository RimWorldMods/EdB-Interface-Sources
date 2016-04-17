using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class SquadManager
	{
		public static bool LoggingEnabled;

		public static readonly int MaxFavorites = 10;

		protected static SquadManager instance;

		protected List<Squad> squads = new List<Squad>();

		protected List<Squad> favorites = new List<Squad>();

		protected Squad currentSquad;

		protected Squad squadFilter;

		protected AllColonistsSquad allColonistsSquad;

		public event SquadNotificationHandler SquadChanged
		{
			add
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadChanged, (SquadNotificationHandler)Delegate.Combine(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadChanged, (SquadNotificationHandler)Delegate.Remove(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadNotificationHandler SquadAdded
		{
			add
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadAdded;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadAdded, (SquadNotificationHandler)Delegate.Combine(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadAdded;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadAdded, (SquadNotificationHandler)Delegate.Remove(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadRemovedHandler SquadRemoved
		{
			add
			{
				SquadRemovedHandler squadRemovedHandler = this.SquadRemoved;
				SquadRemovedHandler squadRemovedHandler2;
				do
				{
					squadRemovedHandler2 = squadRemovedHandler;
					squadRemovedHandler = Interlocked.CompareExchange<SquadRemovedHandler>(ref this.SquadRemoved, (SquadRemovedHandler)Delegate.Combine(squadRemovedHandler2, value), squadRemovedHandler);
				}
				while (squadRemovedHandler != squadRemovedHandler2);
			}
			remove
			{
				SquadRemovedHandler squadRemovedHandler = this.SquadRemoved;
				SquadRemovedHandler squadRemovedHandler2;
				do
				{
					squadRemovedHandler2 = squadRemovedHandler;
					squadRemovedHandler = Interlocked.CompareExchange<SquadRemovedHandler>(ref this.SquadRemoved, (SquadRemovedHandler)Delegate.Remove(squadRemovedHandler2, value), squadRemovedHandler);
				}
				while (squadRemovedHandler != squadRemovedHandler2);
			}
		}

		public event SquadNotificationHandler SquadDisplayPreferenceChanged
		{
			add
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadDisplayPreferenceChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadDisplayPreferenceChanged, (SquadNotificationHandler)Delegate.Combine(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove
			{
				SquadNotificationHandler squadNotificationHandler = this.SquadDisplayPreferenceChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do
				{
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler>(ref this.SquadDisplayPreferenceChanged, (SquadNotificationHandler)Delegate.Remove(squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadOrderChangedHandler SquadOrderChanged
		{
			add
			{
				SquadOrderChangedHandler squadOrderChangedHandler = this.SquadOrderChanged;
				SquadOrderChangedHandler squadOrderChangedHandler2;
				do
				{
					squadOrderChangedHandler2 = squadOrderChangedHandler;
					squadOrderChangedHandler = Interlocked.CompareExchange<SquadOrderChangedHandler>(ref this.SquadOrderChanged, (SquadOrderChangedHandler)Delegate.Combine(squadOrderChangedHandler2, value), squadOrderChangedHandler);
				}
				while (squadOrderChangedHandler != squadOrderChangedHandler2);
			}
			remove
			{
				SquadOrderChangedHandler squadOrderChangedHandler = this.SquadOrderChanged;
				SquadOrderChangedHandler squadOrderChangedHandler2;
				do
				{
					squadOrderChangedHandler2 = squadOrderChangedHandler;
					squadOrderChangedHandler = Interlocked.CompareExchange<SquadOrderChangedHandler>(ref this.SquadOrderChanged, (SquadOrderChangedHandler)Delegate.Remove(squadOrderChangedHandler2, value), squadOrderChangedHandler);
				}
				while (squadOrderChangedHandler != squadOrderChangedHandler2);
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads
		{
			get;
			set;
		}

		public static SquadManager Instance
		{
			get
			{
				if (SquadManager.instance == null)
				{
					SquadManager.instance = new SquadManager();
				}
				return SquadManager.instance;
			}
		}

		public AllColonistsSquad AllColonistsSquad
		{
			get
			{
				return this.allColonistsSquad;
			}
		}

		public List<Squad> Squads
		{
			get
			{
				return this.squads;
			}
		}

		public List<Squad> Favorites
		{
			get
			{
				return this.favorites;
			}
		}

		public int SquadCount
		{
			get
			{
				return this.squads.Count;
			}
		}

		public Squad CurrentSquad
		{
			get
			{
				return this.currentSquad;
			}
			set
			{
				this.currentSquad = value;
			}
		}

		public Squad SquadFilter
		{
			get
			{
				if (this.squadFilter == null || this.squadFilter.Pawns.Count == 0)
				{
					return this.allColonistsSquad;
				}
				return this.squadFilter;
			}
			set
			{
				this.squadFilter = value;
			}
		}

		public bool AllColonistsOrderMatches
		{
			get
			{
				int orderHash = this.GetOrderHash(this.AllColonistsSquad.Pawns);
				int orderHash2 = this.GetOrderHash(Find.ListerPawns.FreeColonists);
				return orderHash == orderHash2;
			}
		}

		protected SquadManager()
		{
			this.Message("SquadManager()");
			this.Reset();
		}

		public void Message(string message)
		{
			if (SquadManager.LoggingEnabled)
			{
				Log.Message(message);
			}
		}

		public void Warning(string message)
		{
			if (SquadManager.LoggingEnabled)
			{
				Log.Warning(message);
			}
		}

		public void Reset()
		{
			this.Message("SquadManager.Reset()");
			this.allColonistsSquad = new AllColonistsSquad();
			this.squads.Clear();
			this.squads.Add(this.allColonistsSquad);
			this.currentSquad = null;
			this.favorites.Clear();
			for (int i = 0; i < SquadManager.MaxFavorites; i++)
			{
				this.favorites.Add(null);
			}
		}

		public bool SyncWithMap()
		{
			this.Message("SquadManager.SyncWithMap()");
			SquadManagerThing squadManagerThing = SquadManagerThing.Instance;
			if (SquadManagerThing.Instance.InMap)
			{
				this.squads.Clear();
				this.squads.AddRange(squadManagerThing.Squads);
				this.favorites.Clear();
				this.favorites.AddRange(squadManagerThing.Favorites);
				foreach (Squad current in this.squads)
				{
					AllColonistsSquad allColonistsSquad = current as AllColonistsSquad;
					if (allColonistsSquad != null)
					{
						this.allColonistsSquad = allColonistsSquad;
					}
				}
				if (this.allColonistsSquad == null)
				{
					Log.Error("Could not find default all-colonists squad");
				}
				else
				{
					ColonistTracker.Instance.StartTrackingPawns(this.AllColonistsSquad.Pawns);
				}
				this.CurrentSquad = squadManagerThing.CurrentSquad;
				this.SquadFilter = squadManagerThing.SquadFilter;
				return true;
			}
			return false;
		}

		public void AddSquad(Squad squad)
		{
			this.Message("SquadManager.AddSquad()");
			this.squads.Add(squad);
			if (this.SquadAdded != null)
			{
				this.SquadAdded(squad);
			}
			this.SyncThingToMap();
		}

		public void RemoveSquad(Squad squad)
		{
			this.Message("SquadManager.RemoveSquad()");
			if (squad == this.allColonistsSquad)
			{
				return;
			}
			int num = this.squads.IndexOf(squad);
			if (num == -1)
			{
				return;
			}
			if (this.currentSquad == squad)
			{
				this.currentSquad = this.allColonistsSquad;
			}
			if (this.squadFilter == squad)
			{
				this.squadFilter = null;
			}
			this.RemoveFromFavorites(squad);
			if (this.squads.Remove(squad) && this.SquadRemoved != null)
			{
				this.SquadRemoved(squad, num);
			}
			this.SyncThingToMap();
		}

		public void SyncThingToMap()
		{
			this.Message("SyncThingToMap()");
			bool flag = false;
			if (this.PreferenceEnableSquads != null && this.PreferenceEnableSquads.Value)
			{
				if (this.squads.Count > 1)
				{
					flag = true;
				}
				else if (!this.AllColonistsOrderMatches)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (SquadManagerThing.Instance.AddToMap())
				{
					this.Message("Added SquadManagerThing to the map");
				}
			}
			else if (SquadManagerThing.Instance.RemoveFromMap())
			{
				this.Message("Removed SquadManagerThing from the map");
			}
		}

		public Squad GetFavorite(int index)
		{
			if (index < 0 || index >= this.favorites.Count)
			{
				return null;
			}
			return this.favorites[index];
		}

		public bool SetFavorite(int index, Squad squad)
		{
			if (index < 0 || index >= this.favorites.Count)
			{
				return false;
			}
			this.Message(string.Concat(new object[]
			{
				"Set favorite ",
				index,
				" to ",
				squad.Name
			}));
			this.favorites[index] = squad;
			return true;
		}

		public void RemoveFromFavorites(Squad squad)
		{
			for (int i = 0; i < this.favorites.Count; i++)
			{
				if (this.favorites[i] == squad)
				{
					this.favorites[i] = null;
				}
			}
		}

		public void ShowSquadInColonistBar(Squad squad, bool value)
		{
			if (squad.ShowInColonistBar != value)
			{
				squad.ShowInColonistBar = value;
				if (this.SquadDisplayPreferenceChanged != null)
				{
					this.SquadDisplayPreferenceChanged(squad);
				}
			}
		}

		public void RenameSquad(Squad squad, string name)
		{
			squad.Name = name;
			if (this.SquadChanged != null)
			{
				this.SquadChanged(squad);
			}
		}

		public void ReplaceSquadPawns(Squad squad, IEnumerable<Pawn> pawns)
		{
			this.Message("ReplaceSquadPawns");
			if (this.squads.Contains(squad))
			{
				squad.Pawns.Clear();
				squad.Pawns.AddRange(pawns);
				this.Message("Pawn count = " + squad.Pawns.Count);
				if (this.SquadChanged != null)
				{
					this.SquadChanged(squad);
				}
			}
			else
			{
				this.Message("Squad manager does not contain the specified squad");
			}
			this.SyncThingToMap();
		}

		public void ColonistChanged(ColonistNotification notification)
		{
			this.Message("SquadManager.ColonistChanged()");
			if (notification.type == ColonistNotificationType.New)
			{
				this.allColonistsSquad.Add(notification.colonist.Pawn);
				if (this.SquadChanged != null)
				{
					this.SquadChanged(this.allColonistsSquad);
				}
			}
			else if (notification.type == ColonistNotificationType.Buried)
			{
				this.RemovePawnFromAllSquads(notification.colonist.Pawn);
			}
			else if (notification.type == ColonistNotificationType.Lost)
			{
				this.RemovePawnFromAllSquads(notification.colonist.Pawn);
			}
			else if (notification.type == ColonistNotificationType.Deleted)
			{
				this.RemovePawnFromAllSquads(notification.colonist.Pawn);
			}
		}

		public void RemovePawnFromAllSquads(Pawn pawn)
		{
			foreach (Squad current in this.squads)
			{
				if (current.Pawns.Remove(pawn))
				{
					this.SquadChanged(current);
				}
			}
		}

		protected int GetOrderHash(IEnumerable<Pawn> colonists)
		{
			int num = 33;
			foreach (Pawn current in colonists)
			{
				TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist(current);
				if (trackedColonist == null || (!trackedColonist.Dead && !trackedColonist.Missing && !trackedColonist.Captured))
				{
					num = 17 * num + current.GetUniqueLoadID().GetHashCode();
				}
			}
			return num;
		}

		public void ReorderSquadList(List<Squad> reorderedSquads)
		{
			this.Message("ReorderSquadList()");
			if (this.squads.Except(reorderedSquads).Count<Squad>() == 0)
			{
				this.squads.Clear();
				this.squads.AddRange(reorderedSquads);
				if (this.SquadOrderChanged != null)
				{
					this.SquadOrderChanged();
				}
			}
		}
	}
}
