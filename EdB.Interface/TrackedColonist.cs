using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class TrackedColonist
	{
		private Pawn pawn;

		private bool dead;

		private bool missing;

		private bool cryptosleep;

		private int missingTimestamp;

		private Corpse corpse;

		private Faction capturingFaction;

		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
			set
			{
				this.pawn = value;
			}
		}

		public bool Dead
		{
			get
			{
				return this.dead;
			}
			set
			{
				this.dead = value;
			}
		}

		public bool Captured
		{
			get
			{
				return this.capturingFaction != null;
			}
		}

		public bool Cryptosleep
		{
			get
			{
				return this.cryptosleep;
			}
			set
			{
				this.cryptosleep = value;
			}
		}

		public Faction CapturingFaction
		{
			get
			{
				return this.capturingFaction;
			}
			set
			{
				this.capturingFaction = value;
			}
		}

		public int MissingTimestamp
		{
			get
			{
				return this.missingTimestamp;
			}
			set
			{
				this.missingTimestamp = value;
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

		public Pawn Carrier
		{
			get
			{
				if (this.pawn.holder != null && this.pawn.holder.owner != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						return pawn_CarryTracker.pawn;
					}
				}
				return null;
			}
		}

		public bool Drafted
		{
			get
			{
				return !this.dead && this.pawn.Drafted;
			}
		}

		public BrokenStateDef BrokenState
		{
			get
			{
				if (this.pawn.mindState != null && this.pawn.mindState.broken != null)
				{
					return this.pawn.mindState.broken.CurStateDef;
				}
				return null;
			}
		}

		public bool Broken
		{
			get
			{
				return this.BrokenState != null;
			}
		}

		public int MentalBreakWarningLevel
		{
			get
			{
				if (this.pawn.mindState != null && this.pawn.mindState.breaker != null && !this.pawn.Downed && !this.pawn.Dead)
				{
					if (this.pawn.mindState.breaker.HardBreakImminent)
					{
						return 2;
					}
					if (this.pawn.mindState.breaker.MentalBreakApproaching)
					{
						return 1;
					}
				}
				return 0;
			}
		}

		public float HealthPercent
		{
			get
			{
				if (this.pawn.health != null && this.pawn.health.summaryHealth != null)
				{
					return this.pawn.health.summaryHealth.SummaryHealthPercent;
				}
				return 0f;
			}
		}

		public bool Incapacitated
		{
			get
			{
				return this.pawn.health != null && this.pawn.health.Downed;
			}
		}

		public bool Controllable
		{
			get
			{
				return !this.Missing && !this.Dead && !this.Captured && !this.Incapacitated && !this.Broken && !this.Cryptosleep;
			}
		}

		public TrackedColonist()
		{
		}

		public TrackedColonist(Pawn pawn)
		{
			this.pawn = pawn;
			this.dead = false;
			this.missing = false;
			this.missingTimestamp = 0;
			this.corpse = null;
			this.capturingFaction = null;
			this.cryptosleep = false;
		}
	}
}
