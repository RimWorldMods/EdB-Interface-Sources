using System;

namespace EdB.Interface
{
	public interface IInitializedComponent
	{
		void PrepareDependencies(UserInterface userInterface);

		void Initialize(UserInterface userInterface);
	}
}
