using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace EdB.Interface
{
	public class SelectorUtility
	{
		protected FieldInfo hostilePawnsField;

		protected FieldInfo allPawnsField;

		protected List<Pawn> emptyList = new List<Pawn>();

		protected List<Pawn> visitorPawns = new List<Pawn>(20);

		public int HostilePawnCount
		{
			get
			{
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue(Find.ListerPawns);
				if (dictionary == null)
				{
					return 0;
				}
				return dictionary[Faction.OfColony].Count;
			}
		}

		public IEnumerable<Pawn> HostilePawns
		{
			get
			{
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue(Find.ListerPawns);
				if (dictionary == null)
				{
					return this.emptyList;
				}
				return from p in dictionary[Faction.OfColony]
				where !p.InContainer
				select p;
			}
		}

		public bool MoreThanOneHostilePawn
		{
			get
			{
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue(Find.ListerPawns);
				if (dictionary == null)
				{
					return false;
				}
				int num = 0;
				foreach (Pawn current in from p in dictionary[Faction.OfColony]
				where !p.InContainer
				select p)
				{
					if (++num > 1)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool MoreThanOneVisitorPawn
		{
			get
			{
				List<Pawn> list = (List<Pawn>)this.allPawnsField.GetValue(Find.ListerPawns);
				if (list == null || list.Count < 2)
				{
					return false;
				}
				int num = 0;
				foreach (Pawn current in from p in list
				where p.Faction != null && p.Faction != Faction.OfColony && !p.IsPrisonerOfColony && !p.Faction.RelationWith(Faction.OfColony).hostile && !p.InContainer
				select p)
				{
					if (++num > 1)
					{
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<Pawn> VisitorPawns
		{
			get
			{
				List<Pawn> list = (List<Pawn>)this.allPawnsField.GetValue(Find.ListerPawns);
				if (list == null)
				{
					return this.emptyList;
				}
				return from p in list
				where p.Faction != null && p.Faction != Faction.OfColony && !p.IsPrisonerOfColony && !p.Faction.RelationWith(Faction.OfColony).hostile && !p.InContainer
				select p;
			}
		}

		public bool MoreThanOneColonyAnimal
		{
			get
			{
				int num = 0;
				foreach (Pawn current in from pawn in Find.ListerPawns.PawnsInFaction(Faction.OfColony)
				where !pawn.IsColonist
				select pawn)
				{
					if (++num > 1)
					{
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<Pawn> ColonyAnimals
		{
			get
			{
				return from pawn in Find.ListerPawns.PawnsInFaction(Faction.OfColony)
				where !pawn.IsColonist
				select pawn;
			}
		}

		public SelectorUtility()
		{
			this.hostilePawnsField = typeof(ListerPawns).GetField("pawnsHostileToFaction", BindingFlags.Instance | BindingFlags.NonPublic);
			this.allPawnsField = typeof(ListerPawns).GetField("allPawns", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public void SelectNextColonist()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction != Faction.OfColony)
			{
				this.SelectThing(Find.ListerPawns.FreeColonists.FirstOrDefault<Pawn>(), false);
			}
			else
			{
				bool flag = false;
				foreach (Pawn current in Find.ListerPawns.FreeColonists)
				{
					if (flag)
					{
						this.SelectThing(current, false);
						return;
					}
					if (current == selector.SingleSelectedThing)
					{
						flag = true;
					}
				}
				this.SelectThing(Find.ListerPawns.FreeColonists.FirstOrDefault<Pawn>(), false);
			}
		}

		public void SelectPreviousColonist()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction != Faction.OfColony)
			{
				this.SelectThing(Find.ListerPawns.FreeColonists.LastOrDefault<Pawn>(), false);
			}
			else
			{
				Pawn pawn = null;
				foreach (Pawn current in Find.ListerPawns.FreeColonists)
				{
					if (selector.SingleSelectedThing == current)
					{
						if (pawn != null)
						{
							this.SelectThing(pawn, false);
							break;
						}
						this.SelectThing(Find.ListerPawns.FreeColonists.LastOrDefault<Pawn>(), false);
						break;
					}
					else
					{
						pawn = current;
					}
				}
			}
		}

		public void SelectNextPrisoner()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn))
			{
				this.SelectThing(Find.ListerPawns.PrisonersOfColony.FirstOrDefault<Pawn>(), false);
			}
			else
			{
				bool flag = false;
				foreach (Pawn current in Find.ListerPawns.PrisonersOfColony)
				{
					if (flag)
					{
						this.SelectThing(current, false);
						return;
					}
					if (current == selector.SingleSelectedThing)
					{
						flag = true;
					}
				}
				this.SelectThing(Find.ListerPawns.PrisonersOfColony.FirstOrDefault<Pawn>(), false);
			}
		}

		public void SelectPreviousPrisoner()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn))
			{
				this.SelectThing(Find.ListerPawns.PrisonersOfColony.LastOrDefault<Pawn>(), false);
			}
			else
			{
				Pawn pawn = null;
				foreach (Pawn current in Find.ListerPawns.PrisonersOfColony)
				{
					if (selector.SingleSelectedThing == current)
					{
						if (pawn != null)
						{
							this.SelectThing(pawn, false);
							break;
						}
						this.SelectThing(Find.ListerPawns.PrisonersOfColony.LastOrDefault<Pawn>(), false);
						break;
					}
					else
					{
						pawn = current;
					}
				}
			}
		}

		public void SelectNextEnemy()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction == Faction.OfColony)
			{
				Pawn thing = this.HostilePawns.FirstOrDefault<Pawn>();
				this.SelectThing(thing, false);
			}
			else
			{
				bool flag = false;
				foreach (Pawn current in this.HostilePawns)
				{
					if (flag)
					{
						this.SelectThing(current, false);
						return;
					}
					if (current == selector.SingleSelectedThing)
					{
						flag = true;
					}
				}
				Pawn thing2 = this.HostilePawns.FirstOrDefault<Pawn>();
				this.SelectThing(thing2, false);
			}
		}

		public void SelectPreviousEnemy()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction == Faction.OfColony)
			{
				this.SelectThing(this.HostilePawns.LastOrDefault<Pawn>(), false);
			}
			else
			{
				Pawn pawn = null;
				foreach (Pawn current in this.HostilePawns)
				{
					if (selector.SingleSelectedThing == current)
					{
						if (pawn != null)
						{
							this.SelectThing(pawn, false);
							break;
						}
						this.SelectThing(this.HostilePawns.LastOrDefault<Pawn>(), false);
						break;
					}
					else
					{
						pawn = current;
					}
				}
			}
		}

		public void SelectNextVisitor()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction == Faction.OfColony)
			{
				this.SelectThing(this.VisitorPawns.FirstOrDefault<Pawn>(), false);
			}
			else
			{
				bool flag = false;
				foreach (Pawn current in this.VisitorPawns)
				{
					if (flag)
					{
						this.SelectThing(current, false);
						return;
					}
					if (current == selector.SingleSelectedThing)
					{
						flag = true;
					}
				}
				this.SelectThing(this.VisitorPawns.FirstOrDefault<Pawn>(), false);
			}
		}

		public void SelectPreviousVisitor()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction == Faction.OfColony)
			{
				this.SelectThing(this.VisitorPawns.LastOrDefault<Pawn>(), false);
			}
			else
			{
				Pawn pawn = null;
				foreach (Pawn current in this.VisitorPawns)
				{
					if (selector.SingleSelectedThing == current)
					{
						if (pawn != null)
						{
							this.SelectThing(pawn, false);
							break;
						}
						this.SelectThing(this.VisitorPawns.LastOrDefault<Pawn>(), false);
						break;
					}
					else
					{
						pawn = current;
					}
				}
			}
		}

		public void SelectThing(Thing thing, bool addToSelection = false)
		{
			if (thing == null)
			{
				return;
			}
			if (!addToSelection)
			{
				Find.Selector.ClearSelection();
			}
			Find.Selector.Select(thing, true, true);
			Find.MainTabsRoot.SetCurrentTab(MainTabDefOf.Inspect, true);
		}

		public void SelectAllColonists()
		{
			Selector selector = Find.Selector;
			selector.ClearSelection();
			foreach (Pawn current in Find.ListerPawns.FreeColonists)
			{
				Find.Selector.Select(current, false, true);
			}
			Find.MainTabsRoot.SetCurrentTab(MainTabDefOf.Inspect, true);
		}

		public void ClearSelection()
		{
			Find.Selector.ClearSelection();
		}

		public void AddToSelection(object o)
		{
			Find.Selector.Select(o, false, true);
		}

		public void SelectNextColonyAnimal()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction != Faction.OfColony || (selector.SingleSelectedThing as Pawn).IsColonist)
			{
				this.SelectThing(this.ColonyAnimals.FirstOrDefault<Pawn>(), false);
			}
			else
			{
				bool flag = false;
				foreach (Pawn current in this.ColonyAnimals)
				{
					if (flag)
					{
						this.SelectThing(current, false);
						return;
					}
					if (current == selector.SingleSelectedThing)
					{
						flag = true;
					}
				}
				this.SelectThing(this.ColonyAnimals.FirstOrDefault<Pawn>(), false);
			}
		}

		public void SelectPreviousColonyAnimal()
		{
			Selector selector = Find.Selector;
			if (selector.SingleSelectedThing == null || !(selector.SingleSelectedThing is Pawn) || selector.SingleSelectedThing.Faction != Faction.OfColony || (selector.SingleSelectedThing as Pawn).IsColonist)
			{
				this.SelectThing(this.ColonyAnimals.LastOrDefault<Pawn>(), false);
			}
			else
			{
				Pawn pawn = null;
				foreach (Pawn current in this.ColonyAnimals)
				{
					if (selector.SingleSelectedThing == current)
					{
						if (pawn != null)
						{
							this.SelectThing(pawn, false);
							break;
						}
						this.SelectThing(this.ColonyAnimals.LastOrDefault<Pawn>(), false);
						break;
					}
					else
					{
						pawn = current;
					}
				}
			}
		}
	}
}
