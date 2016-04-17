using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class Squad : IExposable
	{
		private static int Count;

		protected List<Pawn> pawns = new List<Pawn>();

		protected string name;

		protected bool showInColonistBar = true;

		protected bool showInOverviewTabs = true;

		protected string id = Squad.GenerateId();

		public List<Pawn> Pawns
		{
			get
			{
				return this.pawns;
			}
			set
			{
				this.pawns = value;
			}
		}

		public bool ShowInColonistBar
		{
			get
			{
				return this.showInColonistBar;
			}
			set
			{
				this.showInColonistBar = value;
			}
		}

		public bool ShowInOverviewTabs
		{
			get
			{
				return this.showInOverviewTabs;
			}
			set
			{
				this.showInOverviewTabs = value;
			}
		}

		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public int OrderHash
		{
			get
			{
				int num = 33;
				foreach (Pawn current in this.pawns)
				{
					num = 17 * num + current.GetUniqueLoadID().GetHashCode();
				}
				num = 17 * num + this.name.GetHashCode();
				return num;
			}
		}

		public virtual void Add(Pawn pawn)
		{
			if (!this.pawns.Contains(pawn))
			{
				this.pawns.Add(pawn);
			}
		}

		public virtual bool Remove(Pawn pawn)
		{
			return this.pawns.Remove(pawn);
		}

		public void Replace(Pawn pawn, Pawn replacement)
		{
			int num = this.pawns.IndexOf(pawn);
			if (num > -1)
			{
				this.pawns[num] = replacement;
			}
		}

		public void Clear()
		{
			this.pawns.Clear();
		}

		public void ExposeData()
		{
			Scribe_Values.LookValue<string>(ref this.id, "id", null, true);
			Scribe_Values.LookValue<string>(ref this.name, "name", string.Empty, true);
			Scribe_Values.LookValue<bool>(ref this.showInColonistBar, "showInColonistBar", true, true);
			Scribe_Values.LookValue<bool>(ref this.showInOverviewTabs, "showInOverviewTabs", true, true);
			Scribe_Collections.LookList<Pawn>(ref this.pawns, "pawns", LookMode.MapReference, null);
		}

		public static string GenerateId()
		{
			return "Squad" + DateTime.Now.Ticks + ++Squad.Count;
		}
	}
}
