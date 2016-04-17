using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public interface IComponentWithPreferences
	{
		IEnumerable<IPreference> Preferences
		{
			get;
		}
	}
}
