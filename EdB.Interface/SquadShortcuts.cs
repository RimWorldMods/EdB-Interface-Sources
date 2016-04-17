using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class SquadShortcuts
	{
		protected List<KeyBindingDef> squadSelectionBindings = new List<KeyBindingDef>();

		protected KeyBindingDef nextSquadKeyBinding;

		protected KeyBindingDef previousSquadKeyBinding;

		protected int? pressedKey;

		protected float lastKeypressTimestamp = -1f;

		protected int keypressCount;

		public ColonistBarSquadSupervisor ColonistBarSquadSupervisor
		{
			get;
			set;
		}

		public SquadShortcuts()
		{
			this.squadSelectionBindings.Clear();
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad1"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad2"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad3"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad4"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad5"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad6"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad7"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad8"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad9"));
			this.squadSelectionBindings.Add(KeyBindingDef.Named("EdB_Interface_Squad10"));
			this.nextSquadKeyBinding = KeyBindingDef.Named("EdB_Interface_NextSquad");
			this.previousSquadKeyBinding = KeyBindingDef.Named("EdB_Interface_PreviousSquad");
		}

		public void Update()
		{
			for (int i = 0; i < this.squadSelectionBindings.Count; i++)
			{
				if (this.squadSelectionBindings[i] != null && this.squadSelectionBindings[i].JustPressed)
				{
					Event current = Event.current;
					if (current.shift || current.control)
					{
						if (this.ColonistBarSquadSupervisor.SaveCurrentSquadAsFavorite(i))
						{
							Messages.Message("EdB.Squads.Shortcuts.Assigned".Translate(new object[]
							{
								this.ColonistBarSquadSupervisor.SelectedSquad.Name,
								this.squadSelectionBindings[i].MainKeyLabel
							}), MessageSound.Standard);
						}
					}
					else
					{
						int? num = this.pressedKey;
						if (num.HasValue && this.pressedKey.Value == i && Time.time - this.lastKeypressTimestamp < 0.3f)
						{
							this.keypressCount++;
							if (this.keypressCount > 2)
							{
								this.keypressCount = 1;
							}
						}
						else
						{
							this.keypressCount = 1;
						}
						if (this.keypressCount == 1)
						{
							this.ColonistBarSquadSupervisor.SelectFavorite(i);
							this.pressedKey = new int?(i);
						}
						else if (this.keypressCount == 2)
						{
							this.ColonistBarSquadSupervisor.SelectAllPawnsInFavorite(i);
							this.pressedKey = null;
							this.keypressCount = 0;
						}
						this.lastKeypressTimestamp = Time.time;
					}
				}
			}
			if (this.nextSquadKeyBinding != null && this.nextSquadKeyBinding.JustPressed)
			{
				this.ColonistBarSquadSupervisor.SelectNextSquad(1);
			}
			if (this.previousSquadKeyBinding != null && this.previousSquadKeyBinding.JustPressed)
			{
				this.ColonistBarSquadSupervisor.SelectNextSquad(-1);
			}
		}
	}
}
