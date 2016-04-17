using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class SquadManagerThing : Thing
	{
		public static readonly string DefName = "EdBInterfaceSquadManager";

		protected List<Squad> squads = new List<Squad>();

		protected List<Squad> favorites = new List<Squad>();

		protected Squad currentSquad;

		protected Squad squadFilter;

		private List<Pawn> missingPawns = new List<Pawn>();

		private List<string> favoriteIds = new List<string>();

		private string currentSquadId = string.Empty;

		private string squadFilterId = string.Empty;

		protected bool inMap;

		protected static SquadManagerThing instance;

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

		public Squad CurrentSquad
		{
			get
			{
				return this.currentSquad;
			}
		}

		public Squad SquadFilter
		{
			get
			{
				return this.squadFilter;
			}
		}

		public static SquadManagerThing Instance
		{
			get
			{
				List<Thing> list = Find.Map.listerThings.ThingsOfDef(ThingDef.Named(SquadManagerThing.DefName));
				if (list != null && list.Count > 0)
				{
					SquadManagerThing.instance = (list[0] as SquadManagerThing);
					if (SquadManagerThing.instance != null)
					{
						SquadManagerThing.instance.inMap = true;
					}
				}
				if (SquadManagerThing.instance == null)
				{
					SquadManagerThing.instance = (ThingMaker.MakeThing(ThingDef.Named(SquadManagerThing.DefName), null) as SquadManagerThing);
					if (SquadManagerThing.instance != null)
					{
						SquadManagerThing.instance.Position = new IntVec3(0, 0, 0);
					}
					else
					{
						Log.Error("Could not create Squad Manager Thing.");
					}
				}
				return SquadManagerThing.instance;
			}
		}

		public override string LabelBase
		{
			get
			{
				return string.Empty;
			}
		}

		public bool InMap
		{
			get
			{
				return this.inMap;
			}
		}

		public static void Clear()
		{
			SquadManagerThing.instance = null;
		}

		public bool RemoveFromMap()
		{
			if (this.inMap)
			{
				Find.Map.listerThings.Remove(this);
				this.inMap = false;
				return true;
			}
			return false;
		}

		public bool AddToMap()
		{
			if (!this.inMap)
			{
				Find.Map.listerThings.Add(this);
				this.inMap = true;
				return true;
			}
			return false;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.squads.Clear();
				this.squads.AddRange(SquadManager.Instance.Squads);
				this.missingPawns.Clear();
				HashSet<Pawn> hashSet = new HashSet<Pawn>();
				foreach (Squad current in this.squads)
				{
					foreach (Pawn current2 in current.Pawns)
					{
						TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist(current2);
						if (trackedColonist.Missing)
						{
							hashSet.Add(current2);
						}
					}
				}
				foreach (Pawn current3 in hashSet)
				{
					this.missingPawns.Add(current3);
				}
				this.favorites.Clear();
				this.favorites.AddRange(SquadManager.Instance.Favorites);
				this.favoriteIds.Clear();
				foreach (Squad current4 in this.favorites)
				{
					if (current4 != null)
					{
						this.favoriteIds.Add(current4.Id);
					}
					else
					{
						this.favoriteIds.Add(string.Empty);
					}
				}
				this.currentSquad = SquadManager.Instance.CurrentSquad;
				if (this.currentSquad != null)
				{
					this.currentSquadId = this.currentSquad.Id;
				}
				else
				{
					this.currentSquadId = string.Empty;
				}
				this.squadFilter = SquadManager.Instance.SquadFilter;
				if (this.squadFilter != null)
				{
					this.squadFilterId = this.squadFilter.Id;
				}
				else
				{
					this.squadFilterId = string.Empty;
				}
			}
			Scribe_Values.LookValue<string>(ref this.currentSquadId, "currentSquad", null, false);
			Scribe_Values.LookValue<string>(ref this.squadFilterId, "squadFilter", null, false);
			Scribe_Collections.LookList<string>(ref this.favoriteIds, "favorites", LookMode.Value, null);
			Scribe_Collections.LookList<Pawn>(ref this.missingPawns, "missingPawns", LookMode.Deep, null);
			Scribe_Collections.LookList<Squad>(ref this.squads, "squads", LookMode.Deep, null);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.favorites == null)
				{
					this.favorites = new List<Squad>();
				}
				else
				{
					this.favorites.Clear();
				}
				for (int i = 0; i < SquadManager.MaxFavorites; i++)
				{
					this.favorites.Add(null);
				}
				if (this.favoriteIds == null)
				{
					this.favoriteIds = new List<string>();
					for (int j = 0; j < SquadManager.MaxFavorites; j++)
					{
						this.favoriteIds.Add(string.Empty);
					}
				}
				int num = 0;
				foreach (string current5 in this.favoriteIds)
				{
					this.favorites[num++] = this.FindSquadById(current5);
				}
				this.currentSquad = this.FindSquadById(this.currentSquadId);
				this.squadFilter = this.FindSquadById(this.squadFilterId);
			}
		}

		protected Squad FindSquadById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			return this.squads.FirstOrDefault((Squad squad) => squad.Id == id);
		}
	}
}
