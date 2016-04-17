using System;
using Verse;

namespace EdB.Interface
{
	public class LessonsComponent : IRenderedComponent, INamedComponent
	{
		public string Name
		{
			get
			{
				return "ActiveLessons";
			}
		}

		public bool RenderWithScreenshots
		{
			get
			{
				return false;
			}
		}

		public void OnGUI()
		{
			ActiveTutorNoteManager.ActiveLessonManagerOnGUI();
		}
	}
}
