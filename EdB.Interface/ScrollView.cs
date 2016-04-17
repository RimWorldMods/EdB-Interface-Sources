using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ScrollView
	{
		private float contentHeight;

		private Vector2 position = Vector2.zero;

		private Rect viewRect;

		private Rect contentRect;

		private bool consumeScrollEvents = true;

		public float ViewHeight
		{
			get
			{
				return this.viewRect.height;
			}
		}

		public float ViewWidth
		{
			get
			{
				return this.viewRect.width;
			}
		}

		public float ContentWidth
		{
			get
			{
				return this.contentRect.width;
			}
		}

		public float ContentHeight
		{
			get
			{
				return this.contentHeight;
			}
		}

		public Vector2 Position
		{
			get
			{
				return this.position;
			}
		}

		public ScrollView()
		{
		}

		public ScrollView(bool consumeScrollEvents)
		{
			this.consumeScrollEvents = consumeScrollEvents;
		}

		public void Begin(Rect viewRect)
		{
			this.viewRect = viewRect;
			this.contentRect = new Rect(0f, 0f, viewRect.width - 16f, this.contentHeight);
			if (this.consumeScrollEvents)
			{
				Widgets.BeginScrollView(viewRect, ref this.position, this.contentRect);
			}
			else
			{
				ScrollView.BeginScrollView(viewRect, ref this.position, this.contentRect);
			}
		}

		public void End(float yPosition)
		{
			if (Event.current.type == EventType.Layout)
			{
				this.contentHeight = yPosition;
			}
			Widgets.EndScrollView();
		}

		protected static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect)
		{
			Vector2 vector = scrollPosition;
			Vector2 vector2 = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			Vector2 vector3;
			if (Event.current.type == EventType.MouseDown)
			{
				vector3 = vector;
			}
			else
			{
				vector3 = vector2;
			}
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				vector3 += Event.current.delta * 40f;
			}
			scrollPosition = vector3;
		}
	}
}
