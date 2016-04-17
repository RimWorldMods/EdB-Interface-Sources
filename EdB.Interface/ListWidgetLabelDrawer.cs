using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ListWidgetLabelDrawer<T> : ListWidgetItemDrawer<T>
	{
		private Vector2 padding = new Vector2(16f, 5f);

		private Vector2 paddingTotal;

		private List<Texture> rowTextures = new List<Texture>();

		private bool wrap = true;

		private Func<T, string> labelDelegate;

		private Color textColor = new Color(0.85f, 0.85f, 0.85f);

		private Color selectedTextColor = new Color(1f, 1f, 1f);

		public ListWidgetLabelDrawer()
		{
			this.paddingTotal = new Vector2(this.padding.x * 2f, this.padding.y * 2f);
			this.labelDelegate = delegate(T item)
			{
				string text = item as string;
				return (text == null) ? "null" : text;
			};
		}

		public ListWidgetLabelDrawer(Func<T, string> labelDelegate)
		{
			this.paddingTotal = new Vector2(this.padding.x * 2f, this.padding.y * 2f);
			this.labelDelegate = labelDelegate;
		}

		public void AddRowColor(Color color)
		{
			this.rowTextures.Add(SolidColorMaterials.NewSolidColorTexture(color));
		}

		public void ClearRowColors()
		{
			this.rowTextures.Clear();
		}

		public float GetHeight(int index, T item, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string text = this.labelDelegate(item);
			float num;
			if (this.wrap)
			{
				num = Text.CalcHeight(text, width - this.paddingTotal.x);
			}
			else
			{
				num = Text.CalcSize(text).y;
			}
			return num + this.paddingTotal.y;
		}

		public Vector2 Draw(int index, T item, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string text = this.labelDelegate(item);
			Text.Anchor = TextAnchor.MiddleLeft;
			Vector2 result;
			try
			{
				Rect rect;
				Rect rect2;
				if (this.wrap)
				{
					float num = Text.CalcHeight(text, width - this.paddingTotal.x);
					rect = new Rect(cursor.x + this.padding.x, cursor.y + this.padding.y, width - this.paddingTotal.x, num);
					rect2 = new Rect(cursor.x, cursor.y, width, this.paddingTotal.y + num);
				}
				else
				{
					Vector2 vector = Text.CalcSize(text);
					rect = new Rect(cursor.x + this.padding.x, cursor.y + cursor.y, vector.x, vector.y);
					rect2 = new Rect(cursor.x, cursor.y, width, this.paddingTotal.y + vector.y);
				}
				if (selected)
				{
					GUI.color = this.selectedTextColor;
				}
				else
				{
					GUI.color = this.textColor;
				}
				Widgets.Label(rect, text);
				result = new Vector2(cursor.x, cursor.y + rect2.height);
			}
			finally
			{
				Text.Anchor = TextAnchor.UpperLeft;
			}
			return result;
		}
	}
}
