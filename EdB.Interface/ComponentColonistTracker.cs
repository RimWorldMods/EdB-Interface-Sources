using System;

namespace EdB.Interface
{
	public class ComponentColonistTracker : IUpdatedComponent, IInitializedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "ColonistTracker";
			}
		}

		public ComponentColonistTracker()
		{
			ColonistTracker.Instance.Reset();
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
		}

		public void Initialize(UserInterface userInterface)
		{
			ColonistTracker.Instance.InitializeWithDefaultColonists();
		}

		public void Update()
		{
			ColonistTracker.Instance.Update();
		}
	}
}
