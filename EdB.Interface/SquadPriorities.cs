using System;
using Verse;

namespace EdB.Interface
{
	public class SquadPriorities
	{
		protected DefMap<WorkTypeDef, int> priorities = new DefMap<WorkTypeDef, int>();

		public int GetPriority(WorkTypeDef w)
		{
			int num = this.priorities[w];
			if (num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				return 1;
			}
			return num;
		}

		public void SetPriority(WorkTypeDef w, int priority)
		{
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority);
			}
			this.priorities[w] = priority;
		}

		public void Reset()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			if (this.priorities.Count == 0)
			{
				foreach (WorkTypeDef current in DefDatabase<WorkTypeDef>.AllDefs)
				{
					this.SetPriority(current, 0);
				}
			}
			this.priorities.SetAll(0);
		}
	}
}
