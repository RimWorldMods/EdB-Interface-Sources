using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class ColonistTracker
	{
		protected static ColonistTracker instance;

		public static readonly bool LoggingEnabled;

		public static int MaxMissingDuration = 12000;

		protected HashSet<Pawn> pawnsInFaction = new HashSet<Pawn>();

		protected HashSet<Pawn> colonistsInFaction = new HashSet<Pawn>();

		protected Dictionary<Pawn, TrackedColonist> trackedColonists = new Dictionary<Pawn, TrackedColonist>();

		protected List<Pawn> removalList = new List<Pawn>();

		protected ThingRequest corpseThingRequest;

		public event ColonistNotificationHandler ColonistChanged
		{
			add
			{
				ColonistNotificationHandler colonistNotificationHandler = this.ColonistChanged;
				ColonistNotificationHandler colonistNotificationHandler2;
				do
				{
					colonistNotificationHandler2 = colonistNotificationHandler;
					colonistNotificationHandler = Interlocked.CompareExchange<ColonistNotificationHandler>(ref this.ColonistChanged, (ColonistNotificationHandler)Delegate.Combine(colonistNotificationHandler2, value), colonistNotificationHandler);
				}
				while (colonistNotificationHandler != colonistNotificationHandler2);
			}
			remove
			{
				ColonistNotificationHandler colonistNotificationHandler = this.ColonistChanged;
				ColonistNotificationHandler colonistNotificationHandler2;
				do
				{
					colonistNotificationHandler2 = colonistNotificationHandler;
					colonistNotificationHandler = Interlocked.CompareExchange<ColonistNotificationHandler>(ref this.ColonistChanged, (ColonistNotificationHandler)Delegate.Remove(colonistNotificationHandler2, value), colonistNotificationHandler);
				}
				while (colonistNotificationHandler != colonistNotificationHandler2);
			}
		}

		public event ColonistListSyncNeededHandler ColonistListSyncNeeded
		{
			add
			{
				ColonistListSyncNeededHandler colonistListSyncNeededHandler = this.ColonistListSyncNeeded;
				ColonistListSyncNeededHandler colonistListSyncNeededHandler2;
				do
				{
					colonistListSyncNeededHandler2 = colonistListSyncNeededHandler;
					colonistListSyncNeededHandler = Interlocked.CompareExchange<ColonistListSyncNeededHandler>(ref this.ColonistListSyncNeeded, (ColonistListSyncNeededHandler)Delegate.Combine(colonistListSyncNeededHandler2, value), colonistListSyncNeededHandler);
				}
				while (colonistListSyncNeededHandler != colonistListSyncNeededHandler2);
			}
			remove
			{
				ColonistListSyncNeededHandler colonistListSyncNeededHandler = this.ColonistListSyncNeeded;
				ColonistListSyncNeededHandler colonistListSyncNeededHandler2;
				do
				{
					colonistListSyncNeededHandler2 = colonistListSyncNeededHandler;
					colonistListSyncNeededHandler = Interlocked.CompareExchange<ColonistListSyncNeededHandler>(ref this.ColonistListSyncNeeded, (ColonistListSyncNeededHandler)Delegate.Remove(colonistListSyncNeededHandler2, value), colonistListSyncNeededHandler);
				}
				while (colonistListSyncNeededHandler != colonistListSyncNeededHandler2);
			}
		}

		public static ColonistTracker Instance
		{
			get
			{
				if (ColonistTracker.instance == null)
				{
					ColonistTracker.instance = new ColonistTracker();
				}
				return ColonistTracker.instance;
			}
		}

		public List<Pawn> SortedPawns
		{
			get
			{
				List<Pawn> list = new List<Pawn>(this.trackedColonists.Keys);
				list.Sort(delegate(Pawn a, Pawn b)
				{
					if (a.playerSettings == null || b.playerSettings == null || (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick == 0))
					{
						return b.GetUniqueLoadID().CompareTo(a.GetUniqueLoadID());
					}
					if (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick != 0)
					{
						return -1;
					}
					if (a.playerSettings.joinTick != 0 && b.playerSettings.joinTick == 0)
					{
						return 1;
					}
					return a.playerSettings.joinTick.CompareTo(b.playerSettings.joinTick);
				});
				return list;
			}
		}

		protected ColonistTracker()
		{
			this.corpseThingRequest = default(ThingRequest);
			this.corpseThingRequest.group = ThingRequestGroup.Corpse;
		}

		public void Reset()
		{
			this.colonistsInFaction.Clear();
			this.pawnsInFaction.Clear();
			this.trackedColonists.Clear();
			this.removalList.Clear();
		}

		public TrackedColonist FindTrackedColonist(Pawn pawn)
		{
			TrackedColonist result;
			if (this.trackedColonists.TryGetValue(pawn, out result))
			{
				return result;
			}
			return null;
		}

		private void Message(Pawn pawn, string message)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			string str = (nameTriple == null) ? pawn.Label : nameTriple.Nick;
			this.Message(str + ": " + message);
		}

		private void Message(string message)
		{
			if (ColonistTracker.LoggingEnabled)
			{
				Log.Message(message);
			}
		}

		public void Update()
		{
			if (Find.ListerPawns.PawnsInFaction(Faction.OfColony).Count != this.pawnsInFaction.Count)
			{
				this.Message("Free colonist list changed.  Re-syncing");
				this.SyncColonistLists();
			}
			foreach (KeyValuePair<Pawn, TrackedColonist> current in this.trackedColonists)
			{
				this.UpdateColonistState(current.Key, current.Value);
			}
			foreach (Pawn current2 in this.removalList)
			{
				this.trackedColonists.Remove(current2);
				this.Message(current2, "No longer tracking pawn");
			}
			this.removalList.Clear();
		}

		private void SyncColonistLists()
		{
			this.pawnsInFaction.Clear();
			this.colonistsInFaction.Clear();
			foreach (Pawn current in Find.ListerPawns.PawnsInFaction(Faction.OfColony))
			{
				this.pawnsInFaction.Add(current);
				if (current.IsColonist)
				{
					this.colonistsInFaction.Add(current);
				}
				if (!this.trackedColonists.ContainsKey(current))
				{
					this.StartTrackingPawn(current);
				}
			}
			if (this.colonistsInFaction.Count != this.trackedColonists.Count)
			{
				this.Message("Free colonist list count does not match tracked count.  Resolving.");
				foreach (TrackedColonist current2 in this.trackedColonists.Values)
				{
					Pawn pawn = current2.Pawn;
					if (!this.pawnsInFaction.Contains(pawn))
					{
						this.Message(pawn, "Tracked colonist not found in free list.  Resolving.");
						this.ResolveMissingPawn(pawn, current2);
					}
				}
			}
			if (this.ColonistListSyncNeeded != null)
			{
				this.ColonistListSyncNeeded();
			}
		}

		protected TrackedColonist StartTrackingPawn(Pawn pawn)
		{
			if (pawn == null || !pawn.IsColonist)
			{
				return null;
			}
			TrackedColonist trackedColonist = null;
			if (this.trackedColonists.TryGetValue(pawn, out trackedColonist))
			{
				this.Message(pawn, "Already tracking colonist");
				return trackedColonist;
			}
			trackedColonist = new TrackedColonist(pawn);
			if (!this.trackedColonists.ContainsKey(pawn))
			{
				this.trackedColonists.Add(pawn, trackedColonist);
				if (this.ColonistChanged != null)
				{
					this.ColonistChanged(new ColonistNotification(ColonistNotificationType.New, trackedColonist));
				}
				this.Message(pawn, "Tracking new colonist");
			}
			else
			{
				this.Message(pawn, "Already tracking colonist");
			}
			return trackedColonist;
		}

		public void StopTrackingPawn(Pawn pawn)
		{
			TrackedColonist trackedColonist = this.FindTrackedColonist(pawn);
			if (trackedColonist != null)
			{
				this.MarkColonistAsDeleted(trackedColonist);
			}
		}

		public void ResolveMissingPawn(Pawn pawn, TrackedColonist colonist)
		{
			if (pawn.Dead || pawn.Destroyed)
			{
				this.Message(pawn, "Tracked colonist is dead or destroyed.  Searching for corpse.");
				Corpse corpse = (Corpse)Find.ListerThings.ThingsMatching(this.corpseThingRequest).FirstOrDefault(delegate(Thing thing)
				{
					Corpse corpse2 = thing as Corpse;
					return corpse2 != null && corpse2.innerPawn == pawn;
				});
				if (corpse != null)
				{
					if (!colonist.Dead)
					{
						colonist.Dead = true;
						if (this.ColonistChanged != null)
						{
							this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Died, colonist));
						}
					}
					colonist.Corpse = corpse;
					this.Message(pawn, "Corpse found.  Colonist is dead.");
					return;
				}
				this.Message("Corpse not found.");
			}
			Pawn pawn2 = null;
			Faction faction = this.FindCarryingFaction(pawn, out pawn2);
			if (faction == null)
			{
				Pawn pawn3 = this.FindColonist(pawn);
				if (pawn3 == null)
				{
					if (!colonist.Missing)
					{
						this.MarkColonistAsMissing(colonist);
					}
				}
				else
				{
					this.ReplaceTrackedPawn(colonist, pawn3);
				}
				return;
			}
			if (faction != Faction.OfColony)
			{
				colonist.CapturingFaction = faction;
				this.Message(pawn, "Colonist is captured");
				return;
			}
			this.Message(pawn, "Colonist is being rescued");
		}

		protected Faction FindCarryingFaction(Pawn pawn, out Pawn carrier)
		{
			ThingContainer holder = pawn.holder;
			if (holder != null)
			{
				IThingContainerOwner owner = holder.owner;
				if (owner != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						Pawn pawn2 = pawn_CarryTracker.pawn;
						if (pawn2 != null)
						{
							carrier = pawn2;
							if (Find.ListerPawns.PawnsHostileToColony.Contains(pawn2))
							{
								this.Message(pawn, "Carried by pawn (" + pawn2.NameStringShort + ") in hostile faction");
								return pawn2.Faction;
							}
							this.Message(pawn, "Carried by pawn (" + pawn2.NameStringShort + ") in non-hostile faction");
							return Faction.OfColony;
						}
					}
				}
			}
			carrier = null;
			return null;
		}

		protected void ReplaceTrackedPawn(TrackedColonist colonist, Pawn replacement)
		{
			this.trackedColonists.Remove(colonist.Pawn);
			colonist.Pawn = replacement;
			this.trackedColonists.Add(colonist.Pawn, colonist);
			this.Message(colonist.Pawn, "Tracked colonist was found.  Pawn was replaced.");
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Replaced, colonist, replacement));
			}
		}

		protected Pawn FindColonist(Pawn pawn)
		{
			foreach (Pawn current in Find.ListerPawns.PawnsInFaction(Faction.OfColony))
			{
				if (current.GetUniqueLoadID() == pawn.GetUniqueLoadID())
				{
					return current;
				}
			}
			return null;
		}

		protected bool IsBuried(Thing thing)
		{
			return thing.holder != null && thing.holder.owner != null && thing.holder.owner is Building_Grave;
		}

		protected void UpdateColonistState(Pawn pawn, TrackedColonist colonist)
		{
			Faction faction = null;
			bool flag = false;
			Pawn pawn2 = null;
			if (pawn.holder != null)
			{
				if (pawn.Destroyed)
				{
					this.MarkColonistAsMissing(colonist);
				}
				else if (pawn.holder.owner != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null && pawn_CarryTracker.pawn.Faction != null && pawn_CarryTracker.pawn.Faction != Faction.OfColony && pawn_CarryTracker.pawn.Faction.RelationWith(Faction.OfColony).hostile)
					{
						pawn2 = pawn_CarryTracker.pawn;
						faction = pawn2.Faction;
					}
					Building_CryptosleepCasket building_CryptosleepCasket = pawn.holder.owner as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null)
					{
						flag = true;
						if (!colonist.Cryptosleep)
						{
							colonist.Cryptosleep = true;
							this.Message(pawn, "Colonist has entered cryptosleep.");
						}
					}
					else
					{
						colonist.Cryptosleep = false;
						if (colonist.Cryptosleep)
						{
							colonist.Cryptosleep = false;
							this.Message(pawn, "Colonist has woken from cryptosleep.");
						}
					}
				}
			}
			else
			{
				faction = null;
				colonist.Cryptosleep = false;
				if (colonist.Captured)
				{
					this.Message(pawn, "Captured colonist has been freed.");
					this.MarkColonistAsFreed(colonist);
				}
				if (colonist.Cryptosleep)
				{
					colonist.Cryptosleep = false;
					this.Message(pawn, "Colonist has woken from cryptosleep.");
				}
			}
			if (!colonist.Captured && faction != null)
			{
				this.MarkColonistAsCaptured(colonist, pawn2, faction);
			}
			else if (colonist.Captured && faction == null)
			{
				this.MarkColonistAsFreed(colonist);
			}
			else if (colonist.Captured && faction != colonist.CapturingFaction)
			{
				this.MarkColonistAsCaptured(colonist, pawn2, faction);
			}
			if (flag && !colonist.Cryptosleep)
			{
				this.MarkColonistAsEnteredCryptosleep(colonist);
			}
			else if (!flag && colonist.Cryptosleep)
			{
				this.MarkColonistAsWokenFromCryptosleep(colonist);
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (colonist.Dead && !colonist.Missing)
			{
				if (colonist.Corpse != null)
				{
					if (colonist.Corpse.Destroyed)
					{
						this.MarkColonistAsMissing(colonist);
					}
					else if (this.IsBuried(colonist.Corpse))
					{
						this.MarkColonistAsBuried(colonist);
					}
				}
			}
			else if (colonist.Missing)
			{
				int num = ticksGame - colonist.MissingTimestamp;
				if (num > ColonistTracker.MaxMissingDuration)
				{
					this.MarkColonistAsLost(colonist);
				}
			}
		}

		protected void MarkColonistAsMissing(TrackedColonist colonist)
		{
			if (!colonist.Missing)
			{
				if (colonist.Captured)
				{
					this.Message(colonist.Pawn, "Captured colonist has been removed from the map (by " + colonist.CapturingFaction + ")");
				}
				colonist.Missing = true;
				colonist.MissingTimestamp = Find.TickManager.TicksGame;
				if (this.ColonistChanged != null)
				{
					this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Missing, colonist));
				}
				this.Message(colonist.Pawn, "Tracked colonist is missing (since " + colonist.MissingTimestamp + ")");
			}
		}

		protected void MarkColonistAsBuried(TrackedColonist colonist)
		{
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Buried, colonist));
			}
			this.Message(colonist.Pawn, "Tracked colonist has been buried");
			this.removalList.Add(colonist.Pawn);
		}

		protected void MarkColonistAsLost(TrackedColonist colonist)
		{
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Lost, colonist));
			}
			this.Message("Tracked colonist has been missing for more than " + ColonistTracker.MaxMissingDuration + " ticks");
			this.removalList.Add(colonist.Pawn);
		}

		protected void MarkColonistAsCaptured(TrackedColonist colonist, Pawn carrier, Faction capturingFaction)
		{
			if (colonist.CapturingFaction != capturingFaction)
			{
				colonist.CapturingFaction = capturingFaction;
				if (this.ColonistChanged != null)
				{
					this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Captured, colonist));
				}
				this.Message(colonist.Pawn, "Colonist has been captured (by " + capturingFaction.name + ")");
			}
		}

		protected void MarkColonistAsDeleted(TrackedColonist colonist)
		{
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Deleted, colonist));
			}
			this.Message(colonist.Pawn, "Tracked colonist has been deleted");
			this.removalList.Add(colonist.Pawn);
		}

		protected void MarkColonistAsFreed(TrackedColonist colonist)
		{
			colonist.CapturingFaction = null;
			if (!colonist.Pawn.Destroyed)
			{
				if (this.ColonistChanged != null)
				{
					this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Freed, colonist));
				}
				this.Message(colonist.Pawn, "Captured colonist has been freed.");
			}
		}

		protected void MarkColonistAsEnteredCryptosleep(TrackedColonist colonist)
		{
			colonist.Cryptosleep = true;
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.Cryptosleep, colonist));
			}
			this.Message(colonist.Pawn, "Tracked colonist has entered cryptosleep.");
		}

		protected void MarkColonistAsWokenFromCryptosleep(TrackedColonist colonist)
		{
			colonist.Cryptosleep = false;
			if (this.ColonistChanged != null)
			{
				this.ColonistChanged(new ColonistNotification(ColonistNotificationType.WokeFromCryptosleep, colonist));
			}
			this.Message(colonist.Pawn, "Tracked colonist has woken from cryptosleep.");
		}

		public void InitializeWithDefaultColonists()
		{
			this.Message("InitializeWithDefaultColonists()");
			this.trackedColonists.Clear();
			this.pawnsInFaction.Clear();
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn current in Find.ListerPawns.PawnsInFaction(Faction.OfColony))
			{
				list.Add(current);
			}
			foreach (Pawn current2 in Find.ListerPawns.PawnsHostileToColony)
			{
				if (current2.carrier != null)
				{
					Pawn pawn = current2.carrier.CarriedThing as Pawn;
					if (pawn != null && pawn.Faction != null && pawn.Faction == Faction.OfColony)
					{
						list.Add(pawn);
					}
				}
			}
			foreach (Thing current3 in Find.ListerThings.AllThings)
			{
				Corpse corpse = current3 as Corpse;
				if (corpse != null && corpse.innerPawn != null && corpse.innerPawn.Faction == Faction.OfColony && !this.IsBuried(corpse))
				{
					list.Add(corpse.innerPawn);
				}
			}
			list.Sort(delegate(Pawn a, Pawn b)
			{
				if (a.playerSettings == null || b.playerSettings == null || (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick == 0))
				{
					return b.GetUniqueLoadID().CompareTo(a.GetUniqueLoadID());
				}
				if (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick != 0)
				{
					return -1;
				}
				if (a.playerSettings.joinTick != 0 && b.playerSettings.joinTick == 0)
				{
					return 1;
				}
				return a.playerSettings.joinTick.CompareTo(b.playerSettings.joinTick);
			});
			foreach (Pawn current4 in list)
			{
				this.StartTrackingPawn(current4);
			}
		}

		public void StartTrackingPawns(IEnumerable<Pawn> pawns)
		{
			this.Message("StartTrackingPawns(" + pawns.Count<Pawn>() + ")");
			foreach (Pawn current in pawns)
			{
				this.StartTrackingPawn(current);
			}
			this.SyncColonistLists();
		}
	}
}
