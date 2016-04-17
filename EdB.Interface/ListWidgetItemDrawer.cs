using System;
using UnityEngine;

namespace EdB.Interface
{
	public interface ListWidgetItemDrawer<T>
	{
		Vector2 Draw(int index, T item, Vector2 cursor, float width, bool selected, bool disabled);

		float GetHeight(int index, T item, Vector2 cursor, float width, bool selected, bool disabled);
	}
}
