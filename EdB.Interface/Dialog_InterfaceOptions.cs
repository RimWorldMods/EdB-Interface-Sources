using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Dialog_InterfaceOptions : Window
	{
		private static readonly Vector2 WindowSize = new Vector2(500f, 720f);

		protected bool closed = true;

		private ScrollView optionListView = new ScrollView();

		public static readonly int LabelLineHeight = 30;

		public static readonly int SectionPadding = 14;

		public static Vector2 PreferencePadding = new Vector2(8f, 6f);

		public static float IndentSize = 16f;

		public static Color DisabledControlColor = new Color(1f, 1f, 1f, 0.5f);

		public override Vector2 InitialWindowSize
		{
			get
			{
				int num = 0;
				foreach (PreferenceGroup current in Preferences.Instance.Groups)
				{
					foreach (IPreference current2 in current.Preferences)
					{
						if (current2.DisplayInOptions)
						{
							num++;
						}
					}
				}
				if (num < 6)
				{
					return new Vector2(500f, 420f);
				}
				return new Vector2(Dialog_InterfaceOptions.WindowSize.x, Dialog_InterfaceOptions.WindowSize.y);
			}
		}

		public Dialog_InterfaceOptions()
		{
			this.closeOnEscapeKey = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.forcePause = true;
		}

		protected void SectionLabel(string message, ref float cursor, float width)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(Dialog_InterfaceOptions.PreferencePadding.x, cursor, width, (float)Dialog_InterfaceOptions.LabelLineHeight), message);
			cursor += (float)(Dialog_InterfaceOptions.LabelLineHeight + 6);
			Text.Font = GameFont.Small;
		}

		protected void SectionDivider(ref float cursor)
		{
			cursor += (float)Dialog_InterfaceOptions.SectionPadding;
		}

		public override void DoWindowContents(Rect inRect)
		{
			try
			{
				GUI.BeginGroup(inRect);
				Rect viewRect = new Rect(8f, 16f, inRect.width - 8f, inRect.height - 60f);
				float num = 0f;
				try
				{
					this.optionListView.Begin(viewRect);
					GUI.skin.label.alignment = TextAnchor.UpperLeft;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					float width = this.optionListView.ContentWidth - Dialog_InterfaceOptions.PreferencePadding.x - Dialog_InterfaceOptions.PreferencePadding.x;
					foreach (PreferenceGroup current in Preferences.Instance.Groups)
					{
						bool flag = false;
						foreach (IPreference current2 in current.Preferences)
						{
							if (current2.DisplayInOptions)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							this.SectionLabel(current.Name.Translate(), ref num, width);
							foreach (IPreference current3 in current.Preferences)
							{
								if (current3.DisplayInOptions)
								{
									current3.OnGUI(Dialog_InterfaceOptions.PreferencePadding.x, ref num, width);
									num += Dialog_InterfaceOptions.PreferencePadding.y;
								}
							}
							this.SectionDivider(ref num);
						}
					}
				}
				finally
				{
					this.optionListView.End(num);
				}
			}
			finally
			{
				GUI.EndGroup();
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		public override void PostClose()
		{
			base.PostClose();
			Preferences.Instance.Save();
		}
	}
}
