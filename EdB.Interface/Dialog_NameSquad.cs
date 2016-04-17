using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Dialog_NameSquad : Window
	{
		private const int MaxNameLength = 20;

		private Squad squad;

		private string currentName;

		private bool newSquad;

		private bool initializedFocus;

		public override Vector2 InitialWindowSize
		{
			get
			{
				return new Vector2(500f, 200f);
			}
		}

		public Dialog_NameSquad(Squad squad, bool newSquad)
		{
			this.squad = squad;
			this.currentName = squad.Name;
			this.newSquad = newSquad;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			if (this.newSquad)
			{
				Widgets.Label(new Rect(15f, 15f, 500f, 50f), "EdB.Squads.Window.NameDialog.New".Translate());
			}
			else
			{
				Widgets.Label(new Rect(15f, 15f, 500f, 50f), "EdB.Squads.Window.NameDialog.Rename".Translate());
			}
			Text.Font = GameFont.Small;
			bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
			Rect rect = new Rect(15f, 50f, inRect.width / 2f - 20f, 35f);
			GUI.SetNextControlName("NameSquad");
			string text = Widgets.TextField(rect, this.currentName);
			if (text.Length <= 20)
			{
				this.currentName = text;
			}
			string nameOfFocusedControl = GUI.GetNameOfFocusedControl();
			if (!this.initializedFocus)
			{
				if (nameOfFocusedControl == string.Empty)
				{
					GUI.FocusControl("NameSquad");
					this.initializedFocus = true;
				}
				else if (nameOfFocusedControl == "NameSquad")
				{
					TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
					textEditor.SelectAll();
					this.initializedFocus = true;
				}
			}
			else if (nameOfFocusedControl == "NameSquad" && flag && this.ChangeName())
			{
				this.Close(true);
				return;
			}
			if (!this.newSquad && Widgets.TextButton(new Rect(20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Cancel", true, false))
			{
				this.Close(true);
				return;
			}
			if (Widgets.TextButton(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "OK", true, false) && this.ChangeName())
			{
				this.Close(true);
				return;
			}
		}

		protected bool ChangeName()
		{
			if (this.currentName.Length > 0)
			{
				SquadManager.Instance.RenameSquad(this.squad, this.currentName);
				return true;
			}
			return false;
		}
	}
}
