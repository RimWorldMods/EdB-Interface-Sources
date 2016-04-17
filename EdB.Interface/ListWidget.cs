using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ListWidget<T, D> where D : ListWidgetItemDrawer<T>
	{
		private Color borderColor = new Color(0.3593f, 0.3672f, 0.3789f);

		private List<T> items = new List<T>();

		private List<T> selectedItems = new List<T>();

		private List<int> selectedIndices = new List<int>();

		private D itemDrawer;

		private List<Texture> rowTextures = new List<Texture>();

		private Texture selectedTexture;

		private Texture backgroundColor;

		private bool supportsMultiSelect;

		protected Vector2 scrollableContentViewPosition = Vector2.zero;

		protected float scrollableContentHeight;

		public event ListWidgetMultiSelectionChangedHandler<T> MultiSelectionChangedEvent;

		public event ListWidgetSingleSelectionChangedHandler<T> SingleSelectionChangedEvent;

		public List<int> SelectedIndices
		{
			get
			{
				return this.selectedIndices;
			}
			set
			{
				this.selectedIndices.Clear();
				this.selectedIndices.AddRange(value);
				this.SendSelectionEvent();
			}
		}

		public List<T> SelectedItems
		{
			get
			{
				this.selectedItems.Clear();
				foreach (int current in this.selectedIndices)
				{
					this.selectedItems.Add(this.items[current]);
				}
				return this.selectedItems;
			}
		}

		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		public List<T> Items
		{
			get
			{
				return this.items;
			}
		}

		public bool SupportsMultiSelect
		{
			get
			{
				return this.supportsMultiSelect;
			}
			set
			{
				this.supportsMultiSelect = value;
			}
		}

		public Color BackgroundColor
		{
			set
			{
				this.backgroundColor = SolidColorMaterials.NewSolidColorTexture(value);
			}
		}

		public ListWidget(D itemDrawer)
		{
			this.itemDrawer = itemDrawer;
			this.selectedTexture = SolidColorMaterials.NewSolidColorTexture(new Color(0.2656f, 0.2773f, 0.2891f));
			this.rowTextures.Add(SolidColorMaterials.NewSolidColorTexture(new Color(0.1523f, 0.168f, 0.1836f)));
			this.rowTextures.Add(SolidColorMaterials.NewSolidColorTexture(new Color(0.1094f, 0.125f, 0.1406f)));
			this.backgroundColor = SolidColorMaterials.NewSolidColorTexture(new Color(0.0664f, 0.082f, 0.0938f));
		}

		public ListWidget(List<T> items, D itemDrawer) : this(itemDrawer)
		{
			this.items = new List<T>(items);
		}

		public void Reset()
		{
			this.items.Clear();
			this.selectedIndices.Clear();
		}

		public void DrawWidget(Rect bounds)
		{
			GUI.color = Color.white;
			GUI.DrawTexture(bounds, this.backgroundColor);
			GUI.color = this.borderColor;
			Widgets.DrawBox(bounds, 1);
			Rect position = bounds.ContractedBy(1f);
			try
			{
				GUI.BeginGroup(position);
				Rect outRect = new Rect(0f, 0f, position.width, position.height);
				Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - 16f, this.scrollableContentHeight);
				try
				{
					Widgets.BeginScrollView(outRect, ref this.scrollableContentViewPosition, viewRect);
					Vector2 cursor = new Vector2(0f, 0f);
					for (int i = 0; i < this.items.Count; i++)
					{
						bool flag = this.selectedIndices.Contains(i);
						T item = this.items[i];
						float height = this.itemDrawer.GetHeight(i, item, cursor, position.width, flag, false);
						Rect rect = new Rect(cursor.x, cursor.y, position.width, height);
						Texture image = null;
						if (flag)
						{
							image = this.selectedTexture;
						}
						else if (this.rowTextures.Count > 0)
						{
							image = this.rowTextures[i % this.rowTextures.Count];
						}
						if (this.backgroundColor != null)
						{
							GUI.color = Color.white;
							GUI.DrawTexture(rect, image);
						}
						cursor = this.itemDrawer.Draw(i, item, cursor, position.width, flag, false);
						if (Widgets.InvisibleButton(rect))
						{
							if (this.SupportsMultiSelect)
							{
								if (Event.current.control)
								{
									this.ToggleSelection(i);
								}
								else if (Event.current.shift)
								{
									this.SelectThrough(i);
								}
								else
								{
									this.Select(i);
								}
							}
							else
							{
								this.Select(i);
							}
						}
					}
					if (Event.current.type == EventType.Layout)
					{
						this.scrollableContentHeight = cursor.y;
					}
				}
				finally
				{
					Widgets.EndScrollView();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				GUI.EndGroup();
			}
			GUI.color = Color.white;
		}

		public void ResetItems(List<T> items)
		{
			this.items = new List<T>(items);
			this.ClearSelection();
		}

		public void Add(T item)
		{
			this.items.Add(item);
		}

		public void Insert(T item, int index)
		{
			this.items.Insert(index, item);
		}

		public bool Remove(T item)
		{
			int num = this.items.IndexOf(item);
			if (num > -1)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		public void RemoveAt(int index)
		{
			if (index > -1)
			{
				this.items.RemoveAt(index);
				this.selectedIndices.Remove(index);
				return;
			}
			throw new IndexOutOfRangeException();
		}

		public void ClearSelection()
		{
			if (this.selectedIndices.Count > 0)
			{
				this.selectedIndices.Clear();
				this.SendSelectionEvent();
			}
		}

		public void Select(T item)
		{
			int num = this.items.IndexOf(item);
			if (num > -1)
			{
				this.Select(num);
			}
		}

		public void Select(int index)
		{
			if (index < 0 || index >= this.items.Count)
			{
				return;
			}
			if (this.selectedIndices.Count != 1 || this.selectedIndices[0] != index)
			{
				this.selectedIndices.Clear();
				this.selectedIndices.Add(index);
				this.SendSelectionEvent();
			}
		}

		public void Select(HashSet<T> itemSet)
		{
			this.selectedIndices.Clear();
			int num = 0;
			foreach (T current in this.items)
			{
				if (itemSet.Contains(current))
				{
					this.selectedIndices.Add(num);
				}
				num++;
			}
			this.SendSelectionEvent();
		}

		public void SelectThrough(int index)
		{
			if (index < 0 || index >= this.items.Count)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (this.selectedIndices.Count == 0)
			{
				num = 0;
				num2 = index;
				num3 = 1;
			}
			else
			{
				int num4 = this.selectedIndices.Last<int>();
				if (index < num4)
				{
					num = index;
					num2 = num4;
					num3 = 1;
				}
				else if (index > num4)
				{
					num = index;
					num2 = num4;
					num3 = -1;
				}
			}
			List<int> list = new List<int>();
			if (num3 == 1)
			{
				for (int i = num; i <= num2; i++)
				{
					list.Add(i);
				}
			}
			else if (num3 == -1)
			{
				for (int j = num; j >= num2; j--)
				{
					list.Add(j);
				}
			}
			this.selectedIndices = list;
			this.SendSelectionEvent();
		}

		public void AddToSelection(int index)
		{
			if (index < 0 || index >= this.items.Count)
			{
				return;
			}
			if (!this.selectedIndices.Contains(index))
			{
				this.selectedIndices.Add(index);
				this.SendSelectionEvent();
			}
		}

		public void RemoveFromSelection(int index)
		{
			if (index < 0 || index >= this.items.Count)
			{
				return;
			}
			if (this.selectedIndices.Contains(index))
			{
				this.selectedIndices.Remove(index);
				this.SendSelectionEvent();
			}
		}

		public void ToggleSelection(int index)
		{
			if (index < 0 || index >= this.items.Count)
			{
				return;
			}
			if (this.selectedIndices.Contains(index))
			{
				this.selectedIndices.Remove(index);
			}
			else
			{
				this.selectedIndices.Add(index);
			}
			this.SendSelectionEvent();
		}

		private void SendSelectionEvent()
		{
			if (this.MultiSelectionChangedEvent != null)
			{
				this.MultiSelectionChangedEvent(this.SelectedItems);
			}
			if (this.SingleSelectionChangedEvent != null)
			{
				if (this.selectedIndices.Count == 1)
				{
					this.SingleSelectionChangedEvent(this.items[this.selectedIndices[0]]);
				}
				else if (this.selectedIndices.Count == 0)
				{
					this.SingleSelectionChangedEvent(default(T));
				}
			}
		}
	}
}
