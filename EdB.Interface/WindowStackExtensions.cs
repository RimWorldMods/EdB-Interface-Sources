using System;
using Verse;

namespace EdB.Interface
{
	public static class WindowStackExtensions
	{
		public static Window Top(this WindowStack stack)
		{
			if (stack.Count > 0)
			{
				return stack.Windows[0];
			}
			return null;
		}
	}
}
