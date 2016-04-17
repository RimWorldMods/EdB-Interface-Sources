using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class MainTabWindow_PawnListWithSquads : MainTabWindow_PawnList
	{
		public static readonly Texture2D SquadRowBackground = SolidColorMaterials.NewSolidColorTexture(1f, 1f, 1f, 0.05f);

		public static readonly float SquadFilterButtonWidth = 228f;

		public static readonly float FooterButtonHeight = 32f;

		public static readonly float FooterExtraHeight = 22f;

		public static readonly float squadRowHeight = 33f;

		public static readonly float ScrollHeightReduction = 26f;

		protected PreferenceEnableSquads squadsEnabled;

		protected PreferenceEnableSquadFiltering squadFilteringEnabled;

		protected PreferenceEnableSquadRow squadRowEnabled;

		protected bool squadDirtyFlag;

		public PreferenceEnableSquadFiltering PreferenceEnableSquadFiltering
		{
			set
			{
				this.squadFilteringEnabled = value;
			}
		}

		public PreferenceEnableSquadRow PreferenceEnableSquadRow
		{
			set
			{
				this.squadRowEnabled = value;
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads
		{
			set
			{
				this.squadsEnabled = value;
			}
		}

		protected bool SquadFilteringEnabled
		{
			get
			{
				return this.squadFilteringEnabled != null && this.squadFilteringEnabled.Value;
			}
		}

		protected bool SquadRowEnabled
		{
			get
			{
				return this.squadRowEnabled != null && this.squadRowEnabled.Value;
			}
		}

		protected bool SquadsEnabled
		{
			get
			{
				return this.squadsEnabled != null && this.squadsEnabled.Value;
			}
		}

		protected float ExtraHeight
		{
			get
			{
				float num = 0f;
				if (this.SquadFilteringEnabled)
				{
					num += MainTabWindow_PawnListWithSquads.FooterExtraHeight;
				}
				if (this.SquadRowEnabled)
				{
					num += this.SquadRowHeight;
				}
				return num;
			}
		}

		protected float PawnListScrollHeightReduction
		{
			get
			{
				if (this.SquadFilteringEnabled || this.SquadRowEnabled)
				{
					return MainTabWindow_PawnListWithSquads.ScrollHeightReduction + this.ExtraHeight;
				}
				return 0f;
			}
		}

		protected float SquadRowHeight
		{
			get
			{
				if (this.SquadRowEnabled)
				{
					return MainTabWindow_PawnListWithSquads.squadRowHeight;
				}
				return 0f;
			}
		}

		protected IEnumerable<Pawn> Pawns
		{
			get
			{
				if (this.SquadFilteringEnabled)
				{
					return SquadManager.Instance.SquadFilter.Pawns.FindAll((Pawn p) => !p.Dead && !p.Destroyed && p.HostFaction == null && p.RaceProps.Humanlike);
				}
				if (this.SquadsEnabled)
				{
					return SquadManager.Instance.AllColonistsSquad.Pawns.FindAll((Pawn p) => !p.Dead && !p.Destroyed && p.HostFaction == null && p.RaceProps.Humanlike);
				}
				return Find.ListerPawns.FreeColonists;
			}
		}

		protected abstract float WindowHeight
		{
			get;
		}

		public MainTabWindow_PawnListWithSquads()
		{
		}

		protected virtual void DrawSquadSelectionDropdown(Rect rect)
		{
			Text.Font = GameFont.Small;
			Squad squadFilter = SquadManager.Instance.SquadFilter;
			List<Squad> list = SquadManager.Instance.Squads.FindAll((Squad s) => s.ShowInOverviewTabs && s.Pawns.Count > 0);
			if (list.Count > 0 && Button.TextButton(rect, squadFilter.Name, true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				list2.AddRange(list.ConvertAll<FloatMenuOption>((Squad s) => new FloatMenuOption(s.Name, delegate
				{
					if (SquadManager.Instance.SquadFilter != s)
					{
						SquadManager.Instance.SquadFilter = s;
						this.squadDirtyFlag = true;
					}
				}, MenuOptionPriority.Medium, null, null)));
				Find.WindowStack.Add(new FloatMenu(list2, false));
			}
		}

		public override void WindowUpdate()
		{
			if (this.squadDirtyFlag)
			{
				this.DeferredBuildPawnList();
				this.currentWindowRect.height = this.WindowHeight;
				this.scrollPosition = Vector2.zero;
				this.squadDirtyFlag = false;
			}
		}

		protected override void BuildPawnList()
		{
			this.squadDirtyFlag = true;
		}

		protected virtual void DeferredBuildPawnList()
		{
			this.pawns.Clear();
			this.pawns.AddRange(this.Pawns);
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.squadDirtyFlag = false;
			this.DeferredBuildPawnList();
		}
	}
}
