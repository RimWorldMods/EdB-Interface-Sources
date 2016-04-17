using System;
using Verse;

namespace EdB.Interface
{
	public class EquippableThing
	{
		public Thing thing;

		public float distance;

		public EquippableThing()
		{
		}

		public EquippableThing(Thing thing)
		{
			this.thing = thing;
		}
	}
}
