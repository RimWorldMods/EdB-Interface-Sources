using System;
using Verse;

namespace EdB.Interface
{
	public class MessagesComponent : IUpdatedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "Messages";
			}
		}

		public void Update()
		{
			Messages.Update();
		}
	}
}
