using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ColonistBarSlot
	{
		public Pawn pawn;

		public Corpse corpse;

		public bool missing;

		public bool incapacitated;

		public bool dead;

		public BrokenStateDef sanity;

		public int psychologyLevel;

		public bool kidnapped;

		public float health;

		public bool drafted;

		protected float missingTime;

		protected SelectorUtility pawnSelector = new SelectorUtility();

		protected bool remove;

		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		public Corpse Corpse
		{
			get
			{
				return this.corpse;
			}
			set
			{
				this.corpse = value;
			}
		}

		public bool Missing
		{
			get
			{
				return this.missing;
			}
			set
			{
				this.missing = value;
			}
		}

		public float MissingTime
		{
			get
			{
				return this.missingTime;
			}
			set
			{
				this.missingTime = value;
			}
		}

		public bool Remove
		{
			get
			{
				return this.remove;
			}
			set
			{
				this.remove = value;
			}
		}

		public ColonistBarSlot(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void Update()
		{
			if (this.pawn == null)
			{
				return;
			}
			this.incapacitated = false;
			if (this.pawn.health != null)
			{
				this.health = this.pawn.health.summaryHealth.SummaryHealthPercent;
				this.incapacitated = this.pawn.health.Downed;
			}
			else
			{
				this.health = 0f;
			}
			this.kidnapped = false;
			if (this.pawn.holder != null)
			{
				if (this.pawn.Destroyed)
				{
					this.missing = true;
				}
				else if (this.pawn.holder.owner != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null && pawn_CarryTracker.pawn.Faction != null && pawn_CarryTracker.pawn.Faction != Faction.OfColony && pawn_CarryTracker.pawn.Faction.RelationWith(Faction.OfColony).hostile)
					{
						this.kidnapped = true;
					}
				}
			}
			this.dead = this.pawn.Dead;
			if (this.dead && this.WasReplaced(this.pawn))
			{
				this.dead = false;
			}
			this.sanity = null;
			if (this.pawn.mindState != null && this.pawn.mindState.broken != null)
			{
				this.sanity = this.pawn.mindState.broken.CurStateDef;
			}
			this.drafted = (!this.dead && this.pawn.Drafted);
			this.psychologyLevel = 0;
			if (this.pawn.mindState != null && this.pawn.mindState.breaker != null && !this.pawn.Downed && !this.pawn.Dead)
			{
				if (this.pawn.mindState.breaker.HardBreakImminent)
				{
					this.psychologyLevel = 2;
				}
				else if (this.pawn.mindState.breaker.MentalBreakApproaching)
				{
					this.psychologyLevel = 1;
				}
			}
		}

		protected bool WasReplaced(Pawn pawn)
		{
			foreach (Pawn current in Find.ListerPawns.FreeColonists)
			{
				if (current.GetUniqueLoadID() == pawn.GetUniqueLoadID())
				{
					return true;
				}
			}
			return false;
		}

		public Pawn FindCarrier()
		{
			if (this.pawn.holder != null && this.pawn.holder.owner != null)
			{
				Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
				if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null)
				{
					return pawn_CarryTracker.pawn;
				}
			}
			return null;
		}
	}
}
