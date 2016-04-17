using System;
using Verse;

namespace EdB.Interface
{
	public class ConceptDeciderComponent : IUpdatedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "ConceptDecider";
			}
		}

		public void Update()
		{
			ConceptDecider.ConceptDeciderUpdate();
		}
	}
}
