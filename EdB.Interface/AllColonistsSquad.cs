using System;
using Verse;

namespace EdB.Interface
{
	public class AllColonistsSquad : Squad
	{
		public override string Name
		{
			get
			{
				return this.name;
			}
			set
			{
			}
		}

		public AllColonistsSquad()
		{
			this.name = "EdB.Squads.AllColonistsSquadName".Translate();
		}

		public override bool Remove(Pawn pawn)
		{
			return base.Remove(pawn);
		}
	}
}
