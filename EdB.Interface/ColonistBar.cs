using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ColonistBar
	{
		public delegate void SelectedGroupChangedHandler(ColonistBarGroup group);

		protected static readonly bool LoggingEnabled = false;

		protected static Texture2D BrowseGroupsUp;

		protected static Texture2D BrowseGroupsDown;

		protected ColonistBarDrawer drawer;

		protected GameObject drawerGameObject;

		protected List<TrackedColonist> slots = new List<TrackedColonist>();

		protected ColonistBarGroup currentGroup;

		protected string currentGroupId = string.Empty;

		protected List<ColonistBarGroup> groups = new List<ColonistBarGroup>();

		private bool displayGroupName = true;

		protected float squadNameDisplayTimestamp;

		protected bool barVisible = true;

		protected List<KeyBindingDef> squadSelectionBindings = new List<KeyBindingDef>();

		protected KeyBindingDef nextGroupKeyBinding;

		protected KeyBindingDef previousGroupKeyBinding;

		protected bool enableGroups = true;

		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceEnabled preferenceEnabled = new PreferenceEnabled();

		protected PreferenceSmallIcons preferenceSmallIcons = new PreferenceSmallIcons();

		protected bool alwaysShowGroupName;

		protected static Color BrowseButtonColor = new Color(1f, 1f, 1f, 0.15f);

		protected static Color BrowseButtonHighlightColor = new Color(1f, 1f, 1f, 0.5f);

		protected static Color GroupNameColor = new Color(0.85f, 0.85f, 0.85f);

		protected static float GroupNameDisplayDuration = 1f;

		protected static float GroupNameEaseOutDuration = 0.4f;

		protected static float GroupNameEaseOutStart = ColonistBar.GroupNameDisplayDuration - ColonistBar.GroupNameEaseOutDuration;

		protected float lastKeyTime;

		protected KeyCode lastKey;

		public event ColonistBar.SelectedGroupChangedHandler SelectedGroupChanged
		{
			add
			{
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler = this.SelectedGroupChanged;
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler2;
				do
				{
					selectedGroupChangedHandler2 = selectedGroupChangedHandler;
					selectedGroupChangedHandler = Interlocked.CompareExchange<ColonistBar.SelectedGroupChangedHandler>(ref this.SelectedGroupChanged, (ColonistBar.SelectedGroupChangedHandler)Delegate.Combine(selectedGroupChangedHandler2, value), selectedGroupChangedHandler);
				}
				while (selectedGroupChangedHandler != selectedGroupChangedHandler2);
			}
			remove
			{
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler = this.SelectedGroupChanged;
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler2;
				do
				{
					selectedGroupChangedHandler2 = selectedGroupChangedHandler;
					selectedGroupChangedHandler = Interlocked.CompareExchange<ColonistBar.SelectedGroupChangedHandler>(ref this.SelectedGroupChanged, (ColonistBar.SelectedGroupChangedHandler)Delegate.Remove(selectedGroupChangedHandler2, value), selectedGroupChangedHandler);
				}
				while (selectedGroupChangedHandler != selectedGroupChangedHandler2);
			}
		}

		public ColonistBarDrawer Drawer
		{
			get
			{
				return this.drawer;
			}
		}

		public bool DisplayGroupName
		{
			get
			{
				return this.displayGroupName;
			}
			set
			{
				this.displayGroupName = value;
			}
		}

		public bool AlwaysShowGroupName
		{
			get
			{
				return this.alwaysShowGroupName;
			}
			set
			{
				this.alwaysShowGroupName = value;
			}
		}

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ColonistBarGroup CurrentGroup
		{
			get
			{
				return this.currentGroup;
			}
			set
			{
				bool flag = value != this.currentGroup;
				if (flag)
				{
					this.currentGroup = value;
					if (this.currentGroup != null)
					{
						if (!string.IsNullOrEmpty(this.currentGroupId) && this.currentGroup.Id != this.currentGroupId)
						{
							this.ResetGroupNameDisplay();
						}
						this.currentGroupId = this.currentGroup.Id;
					}
					else
					{
						this.currentGroupId = string.Empty;
					}
					if (this.SelectedGroupChanged != null)
					{
						this.SelectedGroupChanged(this.currentGroup);
					}
				}
				if (this.currentGroup != null)
				{
					this.drawer.Slots = this.currentGroup.Colonists;
				}
			}
		}

		public bool EnableGroups
		{
			get
			{
				return this.enableGroups;
			}
			set
			{
				this.enableGroups = value;
			}
		}

		public bool GroupsBrowsable
		{
			get
			{
				return this.groups.Count > 1;
			}
		}

		public ColonistBar()
		{
			this.preferences.Add(this.preferenceEnabled);
			this.preferences.Add(this.preferenceSmallIcons);
			this.Reset();
		}

		public static void ResetTextures()
		{
			ColonistBar.BrowseGroupsUp = ContentFinder<Texture2D>.Get("EdB/Interface/ColonistBar/BrowseGroupUp", true);
			ColonistBar.BrowseGroupsDown = ContentFinder<Texture2D>.Get("EdB/Interface/ColonistBar/BrowseGroupDown", true);
		}

		protected void Message(string message)
		{
			if (ColonistBar.LoggingEnabled)
			{
				Log.Message(message);
			}
		}

		public void Reset()
		{
			this.drawerGameObject = new GameObject("ColonistBarDrawer");
			this.drawer = this.drawerGameObject.AddComponent<ColonistBarDrawer>();
			this.barVisible = this.drawer.Visible;
		}

		public void UpdateGroups(List<ColonistBarGroup> groups, ColonistBarGroup selected)
		{
			this.Message(string.Concat(new object[]
			{
				"UpdateGroups(",
				groups.Count,
				", ",
				(selected != null) ? selected.Name : "null",
				")"
			}));
			foreach (ColonistBarGroup current in groups)
			{
				this.Message("group = " + ((current != null) ? current.Name : "null"));
			}
			this.Message("Already selected: " + ((this.currentGroup != null) ? this.currentGroup.Name : "null"));
			if (selected == null && groups.Count > 0)
			{
				selected = this.FindNextGroup(-1);
				this.Message("Previous group: " + ((selected != null) ? selected.Name : "null"));
			}
			this.groups.Clear();
			this.groups.AddRange(groups);
			if (selected == null && groups.Count > 0)
			{
				selected = groups[0];
			}
			this.CurrentGroup = selected;
		}

		protected void ResetGroupNameDisplay()
		{
			if (this.currentGroup != null && this.displayGroupName)
			{
				this.squadNameDisplayTimestamp = Time.time;
			}
			else
			{
				this.squadNameDisplayTimestamp = 0f;
			}
		}

		public void AddGroup(ColonistBarGroup group)
		{
			this.groups.Add(group);
		}

		public void Draw()
		{
			if (!this.preferenceEnabled.Value)
			{
				return;
			}
			if (this.currentGroup == null || this.currentGroup.Colonists.Count == 0)
			{
				return;
			}
			if (this.drawer != null)
			{
				this.drawer.Draw();
				this.drawer.DrawTexturesForSlots();
				this.drawer.DrawToggleButton();
			}
			if (this.enableGroups)
			{
				bool value = this.preferenceSmallIcons.Value;
				if (value && !this.drawer.SmallColonistIcons)
				{
					this.drawer.UseSmallIcons();
				}
				else if (!value && this.drawer.SmallColonistIcons)
				{
					this.drawer.UseLargeIcons();
				}
				if (this.GroupsBrowsable)
				{
					GUI.color = ColonistBar.BrowseButtonColor;
					Rect butRect = value ? new Rect(592f, 15f, 32f, 18f) : new Rect(592f, 25f, 32f, 18f);
					if (butRect.Contains(Event.current.mousePosition))
					{
						this.squadNameDisplayTimestamp = Time.time;
					}
					if (Button.ImageButton(butRect, ColonistBar.BrowseGroupsUp, ColonistBar.BrowseButtonHighlightColor))
					{
						this.SelectNextGroup(-1);
					}
					GUI.color = ColonistBar.BrowseButtonColor;
					butRect = (value ? new Rect(592f, 39f, 32f, 18f) : new Rect(592f, 49f, 32f, 18f));
					if (butRect.Contains(Event.current.mousePosition))
					{
						this.squadNameDisplayTimestamp = Time.time;
					}
					if (Button.ImageButton(butRect, ColonistBar.BrowseGroupsDown, ColonistBar.BrowseButtonHighlightColor))
					{
						this.SelectNextGroup(1);
					}
					GUI.color = Color.white;
				}
				bool flag = false;
				Color groupNameColor = ColonistBar.GroupNameColor;
				Color black = Color.black;
				if (this.alwaysShowGroupName)
				{
					flag = true;
				}
				else if (this.displayGroupName && this.squadNameDisplayTimestamp > 0f)
				{
					float time = Time.time;
					float num = time - this.squadNameDisplayTimestamp;
					if (num < ColonistBar.GroupNameDisplayDuration)
					{
						flag = true;
						if (num > ColonistBar.GroupNameEaseOutStart)
						{
							float num2 = num - ColonistBar.GroupNameEaseOutStart;
							float groupNameEaseOutDuration = ColonistBar.GroupNameEaseOutDuration;
							float num3 = 1f;
							float num4 = -1f;
							num2 /= groupNameEaseOutDuration;
							float a = num4 * num2 * num2 + num3;
							groupNameColor = new Color(ColonistBar.GroupNameColor.r, ColonistBar.GroupNameColor.g, ColonistBar.GroupNameColor.b, a);
							black.a = groupNameColor.a;
						}
					}
					else
					{
						this.squadNameDisplayTimestamp = 0f;
					}
				}
				if (flag)
				{
					Rect rect = value ? new Rect(348f, 20f, 225f, 36f) : new Rect(348f, 29f, 225f, 36f);
					if (!this.GroupsBrowsable)
					{
						rect.x += 48f;
					}
					Text.Anchor = TextAnchor.MiddleRight;
					Text.Font = GameFont.Small;
					GUI.color = black;
					Widgets.Label(new Rect(rect.x + 1f, rect.y + 1f, rect.width, rect.height), this.currentGroup.Name);
					if (rect.Contains(Event.current.mousePosition))
					{
						GUI.color = Color.white;
					}
					else
					{
						GUI.color = groupNameColor;
					}
					Widgets.Label(rect, this.currentGroup.Name);
					if (Widgets.InvisibleButton(rect))
					{
						this.drawer.SelectAllActive();
					}
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
					GUI.color = Color.white;
				}
			}
		}

		protected ColonistBarGroup FindNextGroup(int direction)
		{
			if (this.groups.Count == 0)
			{
				return null;
			}
			if (this.groups.Count == 1)
			{
				return this.groups[0];
			}
			int num = this.groups.IndexOf(this.currentGroup);
			if (num == -1)
			{
				return this.groups[0];
			}
			num += direction;
			if (num < 0)
			{
				num = this.groups.Count - 1;
			}
			else if (num > this.groups.Count - 1)
			{
				num = 0;
			}
			return this.groups[num];
		}

		public void SelectNextGroup(int direction)
		{
			this.CurrentGroup = this.FindNextGroup(direction);
		}

		public void UpdateScreenSize(int width, int height)
		{
			this.drawer.SizeCamera(width, height);
		}

		public void SelectAllPawns()
		{
			this.drawer.SelectAllActive();
		}
	}
}
