using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Squads : MainTabWindow
	{
		public static Texture2D ButtonTexReorderUp;

		public static Texture2D ButtonTexReorderDown;

		public static Texture2D ButtonTexReorderTop;

		public static Texture2D ButtonTexReorderBottom;

		public static Texture2D RenameSquadButton;

		public static Texture2D BackgroundColorTexture;

		protected static Color ListHeaderColor = new Color(0.8f, 0.8f, 0.8f);

		protected static Color ListTextColor = new Color(0.7216f, 0.7647f, 0.902f);

		protected static Color SelectedRowColor = new Color(0.2588f, 0.2588f, 0.2588f);

		protected static Color SelectedTextColor = new Color(0.902f, 0.8314f, 0f);

		protected static Color AlternateRowColor = new Color(0.1095f, 0.125f, 0.1406f);

		protected static Color ArrowButtonColor = new Color(0.9137f, 0.9137f, 0.9137f);

		protected static Color ArrowButtonHighlightColor = Color.white;

		protected static Color InactiveButtonColor = new Color(1f, 1f, 1f, 0.5f);

		private ListWidget<Squad, ListWidgetLabelDrawer<Squad>> squadListWidget;

		private ListWidget<TrackedColonist, ColonistRowDrawer> availableColonistsWidget;

		private ListWidget<TrackedColonist, ColonistRowDrawer> squadMembersWidget;

		private bool reorderSquadUpButtonEnabled;

		private bool reorderSquadDownButtonEnabled;

		private bool moveSquadToTopButtonEnabled;

		private bool moveSquadToBottomButtonEnabled;

		private bool addMembersButtonEnabled;

		private bool removeMembersButtonEnabled;

		private bool deleteColonistButtonEnabled;

		private bool reorderMemberUpButtonEnabled;

		private bool reorderMemberDownButtonEnabled;

		private bool moveMemberToTopButtonEnabled;

		private bool moveMemberToBottomButtonEnabled;

		private int? visibleSquadCount;

		protected List<int> tempSelectedIndices = new List<int>();

		protected List<int> tempSortedSelectedList = new List<int>();

		protected List<int> tempNotSelectedList = new List<int>();

		protected List<int> tempAllIndices = new List<int>();

		private List<Pawn> tempPawnList = new List<Pawn>();

		private Squad selectedSquad;

		private SquadManager squadManager;

		public static readonly Vector2 SquadContentMargin = new Vector2(20f, 20f);

		public static readonly Vector2 MemberActionButtonSize = new Vector2(90f, 36f);

		public static readonly Vector2 MemberActionButtonMargin = new Vector2(24f, 24f);

		public static readonly Vector2 OrderButtonSize = new Vector2(32f, 32f);

		public static readonly Vector2 OrderButtonMargin = new Vector2(10f, 18f);

		public static readonly Rect SquadListRect = new Rect(22f, 70f, 230f, 514f);

		public static readonly Vector2 SquadListMargin = new Vector2(16f, 10f);

		public static readonly Vector2 SquadListActionButtonSize = new Vector2((MainTabWindow_Squads.SquadListRect.width - MainTabWindow_Squads.SquadListMargin.x) / 2f, 40f);

		public static readonly Rect NewSquadButtonRect = new Rect(MainTabWindow_Squads.SquadListRect.x, MainTabWindow_Squads.SquadListRect.y + MainTabWindow_Squads.SquadListRect.height + MainTabWindow_Squads.SquadListMargin.y, MainTabWindow_Squads.SquadListActionButtonSize.x, MainTabWindow_Squads.SquadListActionButtonSize.y);

		public static readonly Rect DeleteSquadButtonRect = new Rect(MainTabWindow_Squads.NewSquadButtonRect.x + MainTabWindow_Squads.NewSquadButtonRect.width + MainTabWindow_Squads.SquadListMargin.x, MainTabWindow_Squads.NewSquadButtonRect.y, MainTabWindow_Squads.SquadListActionButtonSize.x, MainTabWindow_Squads.SquadListActionButtonSize.y);

		public static readonly Rect SquadOrderMoveToTopButtonRect = new Rect(MainTabWindow_Squads.SquadListRect.x + MainTabWindow_Squads.SquadListRect.width + MainTabWindow_Squads.OrderButtonMargin.x, MainTabWindow_Squads.SquadListRect.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadOrderMoveUpButtonRect = new Rect(MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.x, MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.y + MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.height + MainTabWindow_Squads.OrderButtonMargin.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadOrderMoveToBottomButtonRect = new Rect(MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.x, MainTabWindow_Squads.SquadListRect.y + MainTabWindow_Squads.SquadListRect.height - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadOrderMoveDownButtonRect = new Rect(MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.x, MainTabWindow_Squads.SquadOrderMoveToBottomButtonRect.y - MainTabWindow_Squads.OrderButtonMargin.y - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadContentRect = new Rect(306f, 56f, 680f, 582f);

		public static readonly Rect AvailableListRect = new Rect(MainTabWindow_Squads.SquadContentMargin.x, 118f, 232f, 442f);

		public static readonly Rect MemberListRect = new Rect(MainTabWindow_Squads.AvailableListRect.x + MainTabWindow_Squads.MemberActionButtonMargin.x * 2f + MainTabWindow_Squads.MemberActionButtonSize.x + MainTabWindow_Squads.AvailableListRect.width, MainTabWindow_Squads.AvailableListRect.y, MainTabWindow_Squads.AvailableListRect.width, MainTabWindow_Squads.AvailableListRect.height);

		public static readonly Rect AddMemberButtonRect = new Rect(MainTabWindow_Squads.AvailableListRect.x + MainTabWindow_Squads.AvailableListRect.width + MainTabWindow_Squads.MemberActionButtonMargin.x, MainTabWindow_Squads.AvailableListRect.y + MainTabWindow_Squads.AvailableListRect.height / 2f - (MainTabWindow_Squads.MemberActionButtonSize.y + MainTabWindow_Squads.MemberActionButtonMargin.y / 2f), MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static readonly Rect RemoveMemberButtonRect = new Rect(MainTabWindow_Squads.AddMemberButtonRect.x, MainTabWindow_Squads.AddMemberButtonRect.y + MainTabWindow_Squads.AddMemberButtonRect.height + MainTabWindow_Squads.MemberActionButtonMargin.y, MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static readonly Rect DeleteMemberButtonRect = new Rect(MainTabWindow_Squads.AddMemberButtonRect.x, MainTabWindow_Squads.AvailableListRect.y + MainTabWindow_Squads.AvailableListRect.height - MainTabWindow_Squads.MemberActionButtonSize.y, MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static readonly Rect MemberOrderMoveToTopButtonRect = new Rect(MainTabWindow_Squads.MemberListRect.x + MainTabWindow_Squads.MemberListRect.width + MainTabWindow_Squads.OrderButtonMargin.x, MainTabWindow_Squads.MemberListRect.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveUpButtonRect = new Rect(MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.x, MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.y + MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.height + MainTabWindow_Squads.OrderButtonMargin.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveDownButtonRect = new Rect(MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.x, MainTabWindow_Squads.MemberListRect.y + MainTabWindow_Squads.MemberListRect.height - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveToBottomButtonRect = new Rect(MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.x, MainTabWindow_Squads.MemberOrderMoveDownButtonRect.y - MainTabWindow_Squads.OrderButtonMargin.y - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadHeaderRect = new Rect(MainTabWindow_Squads.SquadContentMargin.x, MainTabWindow_Squads.SquadContentMargin.y, 240f, 30f);

		public static readonly Rect MemberCountRect = new Rect(MainTabWindow_Squads.SquadHeaderRect.x, MainTabWindow_Squads.SquadHeaderRect.y + 23f, 200f, 28f);

		private List<Squad> tempSquadList = new List<Squad>();

		protected List<TrackedColonist> tempColonists = new List<TrackedColonist>();

		private HashSet<TrackedColonist> tempSelectedAvailable = new HashSet<TrackedColonist>();

		private HashSet<TrackedColonist> tempSelectedMembers = new HashSet<TrackedColonist>();

		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		protected int VisibleSquadCount
		{
			get
			{
				int? num = this.visibleSquadCount;
				if (!num.HasValue)
				{
					this.visibleSquadCount = new int?((from s in this.squadManager.Squads
					where s.ShowInColonistBar && s.Pawns.Count > 0
					select s).Count<Squad>());
				}
				return this.visibleSquadCount.Value;
			}
		}

		public MainTabWindow_Squads()
		{
			this.squadListWidget = new ListWidget<Squad, ListWidgetLabelDrawer<Squad>>(new ListWidgetLabelDrawer<Squad>((Squad s) => s.Name));
			this.squadListWidget.SingleSelectionChangedEvent += new ListWidgetSingleSelectionChangedHandler<Squad>(this.SelectSquad);
			this.availableColonistsWidget = new ListWidget<TrackedColonist, ColonistRowDrawer>(new ColonistRowDrawer());
			this.availableColonistsWidget.SupportsMultiSelect = true;
			this.availableColonistsWidget.MultiSelectionChangedEvent += new ListWidgetMultiSelectionChangedHandler<TrackedColonist>(this.SelectAvailableColonists);
			this.availableColonistsWidget.BackgroundColor = new Color(0.0664f, 0.082f, 0.0938f);
			this.squadMembersWidget = new ListWidget<TrackedColonist, ColonistRowDrawer>(new ColonistRowDrawer());
			this.squadMembersWidget.SupportsMultiSelect = true;
			this.squadMembersWidget.MultiSelectionChangedEvent += new ListWidgetMultiSelectionChangedHandler<TrackedColonist>(this.SelectSquadMembers);
			this.squadMembersWidget.BackgroundColor = new Color(0.0664f, 0.082f, 0.0938f);
		}

		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "EdB.Squads.Window.Header".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerLeft;
			GUI.color = MainTabWindow_Squads.ListHeaderColor;
			Widgets.Label(new Rect(MainTabWindow_Squads.SquadListRect.x, MainTabWindow_Squads.SquadListRect.y - 23f, 225f, 30f), "EdB.Squads.Window.SquadList.Header".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			this.squadListWidget.DrawWidget(MainTabWindow_Squads.SquadListRect);
			this.EnableSquadReorderButtons();
			if (Button.IconButton(MainTabWindow_Squads.SquadOrderMoveToTopButtonRect, MainTabWindow_Squads.ButtonTexReorderTop, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveSquadToTopButtonEnabled))
			{
				this.MoveSelectedSquadToTop();
			}
			if (Button.IconButton(MainTabWindow_Squads.SquadOrderMoveUpButtonRect, MainTabWindow_Squads.ButtonTexReorderUp, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderSquadUpButtonEnabled))
			{
				this.MoveSelectedSquadUp();
			}
			if (Button.IconButton(MainTabWindow_Squads.SquadOrderMoveDownButtonRect, MainTabWindow_Squads.ButtonTexReorderDown, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderSquadDownButtonEnabled))
			{
				this.MoveSelectedSquadDown();
			}
			if (Button.IconButton(MainTabWindow_Squads.SquadOrderMoveToBottomButtonRect, MainTabWindow_Squads.ButtonTexReorderBottom, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveSquadToBottomButtonEnabled))
			{
				this.MoveSelectedSquadToBottom();
			}
			GUI.color = Color.white;
			if (Widgets.TextButton(MainTabWindow_Squads.NewSquadButtonRect, "EdB.Squads.Window.NewSquad.Button".Translate(), true, false))
			{
				this.NewSquad();
			}
			bool enabled = this.selectedSquad != null && this.selectedSquad != this.squadManager.AllColonistsSquad;
			if (Button.TextButton(MainTabWindow_Squads.DeleteSquadButtonRect, "EdB.Squads.Window.DeleteSquad.Button".Translate(), true, false, enabled))
			{
				this.DeleteSquad();
			}
			GUI.DrawTexture(MainTabWindow_Squads.SquadContentRect, MainTabWindow_Squads.BackgroundColorTexture);
			try
			{
				GUI.BeginGroup(MainTabWindow_Squads.SquadContentRect);
				Text.Font = GameFont.Medium;
				Widgets.Label(MainTabWindow_Squads.SquadHeaderRect, this.selectedSquad.Name);
				Vector2 vector = Text.CalcSize(this.selectedSquad.Name);
				Text.Font = GameFont.Small;
				int count = this.selectedSquad.Pawns.Count;
				string label;
				if (count == 0)
				{
					label = "EdB.Squads.Window.SquadMemberCount.Zero".Translate();
				}
				else if (count == 1)
				{
					label = "EdB.Squads.Window.SquadMemberCount.One".Translate();
				}
				else
				{
					label = "EdB.Squads.Window.SquadMemberCount".Translate(new object[]
					{
						count
					});
				}
				GUI.color = MainTabWindow_Squads.ListHeaderColor;
				Widgets.Label(MainTabWindow_Squads.MemberCountRect, label);
				GUI.color = Color.white;
				Rect rect2 = new Rect(MainTabWindow_Squads.SquadHeaderRect.x + vector.x + 8f, MainTabWindow_Squads.SquadHeaderRect.y - 3f, 30f, 30f);
				if (this.selectedSquad != this.squadManager.AllColonistsSquad)
				{
					TooltipHandler.TipRegion(rect2, new TipSignal("EdB.Squads.Window.RenameSquad.Button.Tip".Translate()));
					if (Widgets.ImageButton(rect2, MainTabWindow_Squads.RenameSquadButton))
					{
						Find.WindowStack.Add(new Dialog_NameSquad(this.selectedSquad, false));
					}
				}
				else
				{
					TooltipHandler.TipRegion(rect2, new TipSignal("EdB.Squads.Window.RenameSquad.Disabled.Tip".Translate()));
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
					GUI.DrawTexture(rect2, MainTabWindow_Squads.RenameSquadButton);
					GUI.color = Color.white;
				}
				string text = "EdB.Squads.Window.SquadOption.ShowInBar".Translate();
				string text2 = "EdB.Squads.Window.SquadOption.ShowInFilters".Translate();
				Vector2 vector2 = Text.CalcSize(text);
				Vector2 vector3 = Text.CalcSize(text2);
				Vector2 vector4 = new Vector2(Math.Max(vector2.x, vector3.x), 28f);
				vector4.x += 48f;
				float num = 0f;
				Rect rect3 = new Rect(MainTabWindow_Squads.SquadContentRect.width - MainTabWindow_Squads.SquadContentMargin.x - vector4.x, MainTabWindow_Squads.SquadContentMargin.y, vector4.x, vector4.y);
				bool showInColonistBar = this.selectedSquad.ShowInColonistBar;
				bool flag = false;
				Widgets.LabelCheckbox(rect3, text, ref showInColonistBar, flag);
				this.squadManager.ShowSquadInColonistBar(this.selectedSquad, showInColonistBar);
				Rect rect4 = new Rect(MainTabWindow_Squads.SquadContentRect.width - MainTabWindow_Squads.SquadContentMargin.x - vector4.x, rect3.y + vector4.y + num, vector4.x, vector4.y);
				bool showInOverviewTabs = this.selectedSquad.ShowInOverviewTabs;
				flag = false;
				Widgets.LabelCheckbox(rect4, text2, ref showInOverviewTabs, flag);
				if (!flag)
				{
					this.selectedSquad.ShowInOverviewTabs = showInOverviewTabs;
				}
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.LowerLeft;
				GUI.color = MainTabWindow_Squads.ListHeaderColor;
				Widgets.Label(new Rect(MainTabWindow_Squads.AvailableListRect.x, MainTabWindow_Squads.AvailableListRect.y - 23f, 210f, 30f), "EdB.Squads.Window.AvailableList.Header".Translate());
				Widgets.Label(new Rect(MainTabWindow_Squads.MemberListRect.x, MainTabWindow_Squads.MemberListRect.y - 23f, 210f, 30f), "EdB.Squads.Window.MemberList.Header".Translate());
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				this.availableColonistsWidget.DrawWidget(MainTabWindow_Squads.AvailableListRect);
				this.squadMembersWidget.DrawWidget(MainTabWindow_Squads.MemberListRect);
				if (Button.TextButton(MainTabWindow_Squads.AddMemberButtonRect, "EdB.Squads.Window.AddSquadMember.Button".Translate(), true, false, this.addMembersButtonEnabled))
				{
					this.AddSelectedMembers();
				}
				if (Button.TextButton(MainTabWindow_Squads.RemoveMemberButtonRect, "EdB.Squads.Window.RemoveSquadMember.Button".Translate(), true, false, this.removeMembersButtonEnabled))
				{
					this.RemoveSelectedMembers();
				}
				if (!this.removeMembersButtonEnabled && this.selectedSquad == SquadManager.Instance.AllColonistsSquad)
				{
					TooltipHandler.TipRegion(MainTabWindow_Squads.RemoveMemberButtonRect, new TipSignal("EdB.Squads.Window.RemoveSquadMember.Disabled.Tip".Translate()));
				}
				if (Button.TextButton(MainTabWindow_Squads.DeleteMemberButtonRect, "EdB.Squads.Window.DeleteColonist.Button".Translate(), true, false, this.deleteColonistButtonEnabled))
				{
					this.DeleteSelectedColonists();
				}
				if (!this.deleteColonistButtonEnabled)
				{
					if (this.availableColonistsWidget.SelectedIndices.Count > 0 || this.squadMembersWidget.SelectedIndices.Count > 0)
					{
						TooltipHandler.TipRegion(MainTabWindow_Squads.DeleteMemberButtonRect, new TipSignal("EdB.Squads.Window.DeleteColonist.Disabled.Tip".Translate()));
					}
				}
				else
				{
					TooltipHandler.TipRegion(MainTabWindow_Squads.DeleteMemberButtonRect, new TipSignal("EdB.Squads.Window.DeleteColonist.Tip".Translate()));
				}
				if (Button.IconButton(MainTabWindow_Squads.MemberOrderMoveToTopButtonRect, MainTabWindow_Squads.ButtonTexReorderTop, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveMemberToTopButtonEnabled))
				{
					this.MoveSelectedMemberToTop();
				}
				if (Button.IconButton(MainTabWindow_Squads.MemberOrderMoveUpButtonRect, MainTabWindow_Squads.ButtonTexReorderUp, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderMemberUpButtonEnabled))
				{
					this.MoveSelectedMemberUp();
				}
				if (Button.IconButton(MainTabWindow_Squads.MemberOrderMoveDownButtonRect, MainTabWindow_Squads.ButtonTexReorderBottom, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveMemberToBottomButtonEnabled))
				{
					this.MoveSelectedMemberToBottom();
				}
				if (Button.IconButton(MainTabWindow_Squads.MemberOrderMoveToBottomButtonRect, MainTabWindow_Squads.ButtonTexReorderDown, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderMemberDownButtonEnabled))
				{
					this.MoveSelectedMemberDown();
				}
			}
			finally
			{
				GUI.EndGroup();
				GUI.color = Color.white;
			}
		}

		public static void ResetTextures()
		{
			MainTabWindow_Squads.ButtonTexReorderUp = ContentFinder<Texture2D>.Get("EdB/Interface/Squads/ArrowUp", true);
			MainTabWindow_Squads.ButtonTexReorderDown = ContentFinder<Texture2D>.Get("EdB/Interface/Squads/ArrowDown", true);
			MainTabWindow_Squads.ButtonTexReorderTop = ContentFinder<Texture2D>.Get("EdB/Interface/Squads/ArrowTop", true);
			MainTabWindow_Squads.ButtonTexReorderBottom = ContentFinder<Texture2D>.Get("EdB/Interface/Squads/ArrowBottom", true);
			MainTabWindow_Squads.RenameSquadButton = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);
			MainTabWindow_Squads.BackgroundColorTexture = SolidColorMaterials.NewSolidColorTexture(new Color(0.0508f, 0.0664f, 0.0742f));
		}

		public override void PostOpen()
		{
			base.PostOpen();
			this.squadManager = SquadManager.Instance;
			this.squadListWidget.Reset();
			this.availableColonistsWidget.Reset();
			this.squadMembersWidget.Reset();
			foreach (Squad current in this.squadManager.Squads)
			{
				this.squadListWidget.Add(current);
			}
			this.squadListWidget.Select(0);
		}

		public override void PostClose()
		{
			base.PostClose();
		}

		protected void MoveSelectedSquadUp()
		{
			this.tempSquadList.Clear();
			this.tempSquadList.AddRange(this.squadManager.Squads);
			this.tempSelectedIndices.Clear();
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList)
			{
				if (current - 1 < num)
				{
					num = current;
					this.tempSelectedIndices.Add(current);
				}
				else
				{
					Squad value = this.tempSquadList[current - 1];
					this.tempSquadList[current - 1] = this.tempSquadList[current];
					this.tempSquadList[current] = value;
					num = current;
					this.tempSelectedIndices.Add(current - 1);
				}
			}
			this.squadManager.ReorderSquadList(this.tempSquadList);
			this.squadListWidget.ResetItems(this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadDown()
		{
			this.tempSquadList.Clear();
			this.tempSquadList.AddRange(this.squadManager.Squads);
			this.tempSelectedIndices.Clear();
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempSortedSelectedList.Reverse();
			int num = this.squadManager.Squads.Count - 1;
			foreach (int current in this.tempSortedSelectedList)
			{
				if (current + 1 > num)
				{
					num = current;
					this.tempSelectedIndices.Add(current);
				}
				else
				{
					Squad value = this.tempSquadList[current + 1];
					this.tempSquadList[current + 1] = this.tempSquadList[current];
					this.tempSquadList[current] = value;
					num = current;
					this.tempSelectedIndices.Add(current + 1);
				}
			}
			this.squadManager.ReorderSquadList(this.tempSquadList);
			this.squadListWidget.ResetItems(this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadToTop()
		{
			this.tempSquadList.Clear();
			this.tempSquadList.AddRange(this.squadManager.Squads);
			this.tempSelectedIndices.Clear();
			this.tempAllIndices.Clear();
			int count = this.squadManager.Squads.Count;
			for (int i = 0; i < count; i++)
			{
				this.tempAllIndices.Add(i);
			}
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempNotSelectedList.Clear();
			this.tempNotSelectedList.AddRange(this.tempAllIndices.Except(this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList)
			{
				this.tempSquadList[num] = this.squadManager.Squads[current];
				this.tempSelectedIndices.Add(num);
				num++;
			}
			foreach (int current2 in this.tempNotSelectedList)
			{
				this.tempSquadList[num] = this.squadManager.Squads[current2];
				num++;
			}
			this.squadManager.ReorderSquadList(this.tempSquadList);
			this.squadListWidget.ResetItems(this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadToBottom()
		{
			this.tempSquadList.Clear();
			this.tempSquadList.AddRange(this.squadManager.Squads);
			this.tempSelectedIndices.Clear();
			this.tempAllIndices.Clear();
			for (int i = 0; i < this.squadManager.Squads.Count; i++)
			{
				this.tempAllIndices.Add(i);
			}
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempNotSelectedList.Clear();
			this.tempNotSelectedList.AddRange(this.tempAllIndices.Except(this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempNotSelectedList)
			{
				this.tempSquadList[num] = this.squadManager.Squads[current];
				num++;
			}
			foreach (int current2 in this.tempSortedSelectedList)
			{
				this.tempSquadList[num] = this.squadManager.Squads[current2];
				this.tempSelectedIndices.Add(num);
				num++;
			}
			this.squadManager.ReorderSquadList(this.tempSquadList);
			this.squadListWidget.ResetItems(this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void ReplaceSquadPawns(Squad squad, List<Pawn> pawns, List<int> selectedIndices)
		{
			this.squadManager.ReplaceSquadPawns(squad, pawns);
			this.ResetSquadMembers(squad);
			this.squadMembersWidget.SelectedIndices = selectedIndices;
		}

		protected void MoveSelectedMemberUp()
		{
			this.tempPawnList.Clear();
			this.tempPawnList.AddRange(this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear();
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList)
			{
				if (current - 1 < num)
				{
					num = current;
					this.tempSelectedIndices.Add(current);
				}
				else
				{
					Pawn value = this.tempPawnList[current - 1];
					this.tempPawnList[current - 1] = this.tempPawnList[current];
					this.tempPawnList[current] = value;
					num = current;
					this.tempSelectedIndices.Add(current - 1);
				}
			}
			this.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberDown()
		{
			this.tempPawnList.Clear();
			this.tempPawnList.AddRange(this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear();
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempSortedSelectedList.Reverse();
			int num = this.selectedSquad.Pawns.Count - 1;
			foreach (int current in this.tempSortedSelectedList)
			{
				if (current + 1 > num)
				{
					num = current;
					this.tempSelectedIndices.Add(current);
				}
				else
				{
					Pawn value = this.tempPawnList[current + 1];
					this.tempPawnList[current + 1] = this.tempPawnList[current];
					this.tempPawnList[current] = value;
					num = current;
					this.tempSelectedIndices.Add(current + 1);
				}
			}
			this.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberToTop()
		{
			this.tempPawnList.Clear();
			this.tempPawnList.AddRange(this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear();
			this.tempAllIndices.Clear();
			for (int i = 0; i < this.selectedSquad.Pawns.Count; i++)
			{
				this.tempAllIndices.Add(i);
			}
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempNotSelectedList.Clear();
			this.tempNotSelectedList.AddRange(this.tempAllIndices.Except(this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList)
			{
				this.tempPawnList[num] = this.selectedSquad.Pawns[current];
				this.tempSelectedIndices.Add(num);
				num++;
			}
			foreach (int current2 in this.tempNotSelectedList)
			{
				this.tempPawnList[num] = this.selectedSquad.Pawns[current2];
				num++;
			}
			this.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberToBottom()
		{
			this.tempPawnList.Clear();
			this.tempPawnList.AddRange(this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear();
			this.tempAllIndices.Clear();
			for (int i = 0; i < this.selectedSquad.Pawns.Count; i++)
			{
				this.tempAllIndices.Add(i);
			}
			this.tempSortedSelectedList.Clear();
			this.tempSortedSelectedList.AddRange(this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort();
			this.tempNotSelectedList.Clear();
			this.tempNotSelectedList.AddRange(this.tempAllIndices.Except(this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort();
			int num = 0;
			foreach (int current in this.tempNotSelectedList)
			{
				this.tempPawnList[num] = this.selectedSquad.Pawns[current];
				num++;
			}
			foreach (int current2 in this.tempSortedSelectedList)
			{
				this.tempPawnList[num] = this.selectedSquad.Pawns[current2];
				this.tempSelectedIndices.Add(num);
				num++;
			}
			this.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void NewSquad()
		{
			Squad squad = new Squad();
			squad.Name = this.CreateDefaultSquadName();
			this.squadManager.AddSquad(squad);
			this.squadListWidget.ResetItems(this.squadManager.Squads);
			this.squadListWidget.Select(squad);
			this.SelectSquad(squad);
			Find.WindowStack.Add(new Dialog_NameSquad(squad, true));
			this.ResetVisibleSquadCount();
		}

		protected string CreateDefaultSquadName()
		{
			int num = this.squadManager.Squads.Count;
			bool flag = false;
			int num2 = 10000;
			string name;
			do
			{
				name = "EdB.Squads.DefaultSquadName".Translate(new object[]
				{
					num
				});
				if ((from s in this.squadManager.Squads
				where name.Equals(s.Name)
				select s).Count<Squad>() == 0)
				{
					flag = true;
				}
				else
				{
					num++;
					if (num > num2)
					{
						flag = true;
					}
				}
			}
			while (!flag);
			return name;
		}

		protected void DeleteSquad()
		{
			Find.WindowStack.Add(new Dialog_Confirm("EdB.Squads.Window.DeleteSquad.Confirm".Translate(), delegate
			{
				int num = this.squadListWidget.SelectedIndices[0];
				this.squadManager.RemoveSquad(this.selectedSquad);
				if (num >= this.squadManager.Squads.Count)
				{
					num--;
				}
				this.squadListWidget.ResetItems(this.squadManager.Squads);
				this.squadListWidget.Select(num);
			}, true));
			this.ResetVisibleSquadCount();
		}

		protected void AddSelectedMembers()
		{
			this.tempColonists.Clear();
			this.tempColonists.AddRange(this.availableColonistsWidget.SelectedItems);
			if (this.tempColonists.Count == 0)
			{
				return;
			}
			this.tempPawnList.Clear();
			foreach (TrackedColonist current in this.squadMembersWidget.Items)
			{
				this.tempPawnList.Add(current.Pawn);
			}
			foreach (TrackedColonist current2 in this.tempColonists)
			{
				this.tempPawnList.Add(current2.Pawn);
			}
			this.squadManager.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList);
			this.ResetAvailableColonists(this.selectedSquad);
			this.ResetSquadMembers(this.selectedSquad);
			this.ResetVisibleSquadCount();
		}

		protected void RemoveSelectedMembers()
		{
			this.tempColonists.Clear();
			this.tempColonists.AddRange(this.squadMembersWidget.SelectedItems);
			if (this.tempColonists.Count == 0)
			{
				return;
			}
			this.tempPawnList.Clear();
			this.tempPawnList.AddRange(this.selectedSquad.Pawns);
			foreach (TrackedColonist current in this.tempColonists)
			{
				this.tempPawnList.Remove(current.Pawn);
			}
			this.squadManager.ReplaceSquadPawns(this.selectedSquad, this.tempPawnList);
			this.ResetAvailableColonists(this.selectedSquad);
			this.ResetSquadMembers(this.selectedSquad);
			this.ResetVisibleSquadCount();
		}

		protected void DeleteSelectedColonists()
		{
			this.tempColonists.Clear();
			if (this.availableColonistsWidget.SelectedIndices.Count > 0)
			{
				this.tempColonists.AddRange(this.availableColonistsWidget.SelectedItems);
			}
			else if (this.squadMembersWidget.SelectedIndices.Count > 0)
			{
				this.tempColonists.AddRange(this.squadMembersWidget.SelectedItems);
			}
			foreach (TrackedColonist current in this.tempColonists)
			{
				if (current.Dead || current.Missing)
				{
					ColonistTracker.Instance.StopTrackingPawn(current.Pawn);
				}
			}
		}

		public void SquadChanged(Squad squad)
		{
			if (this.selectedSquad != null && (squad == SquadManager.Instance.AllColonistsSquad || squad == this.selectedSquad) && this.availableColonistsWidget != null && this.squadMembersWidget != null)
			{
				this.tempSelectedAvailable.Clear();
				this.tempSelectedMembers.Clear();
				foreach (TrackedColonist current in this.availableColonistsWidget.SelectedItems)
				{
					this.tempSelectedAvailable.Add(current);
				}
				foreach (TrackedColonist current2 in this.squadMembersWidget.SelectedItems)
				{
					this.tempSelectedMembers.Add(current2);
				}
				this.ResetSquadMembers(this.selectedSquad);
				this.ResetAvailableColonists(this.selectedSquad);
				this.availableColonistsWidget.Select(this.tempSelectedAvailable);
				this.squadMembersWidget.Select(this.tempSelectedMembers);
			}
		}

		protected void ResetSquadMembers(Squad squad)
		{
			if (squad != null)
			{
				List<TrackedColonist> list = new List<TrackedColonist>();
				foreach (Pawn current in squad.Pawns)
				{
					TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist(current);
					if (trackedColonist != null)
					{
						list.Add(trackedColonist);
					}
				}
				this.squadMembersWidget.ResetItems(list);
			}
			else
			{
				this.squadMembersWidget.ResetItems(new List<TrackedColonist>());
			}
		}

		protected void ResetAvailableColonists(Squad squad)
		{
			List<TrackedColonist> list = new List<TrackedColonist>();
			foreach (Pawn current in this.squadManager.AllColonistsSquad.Pawns)
			{
				if (!squad.Pawns.Contains(current))
				{
					TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist(current);
					if (trackedColonist != null)
					{
						list.Add(trackedColonist);
					}
				}
			}
			this.availableColonistsWidget.ResetItems(list);
		}

		protected void SelectSquad(Squad squad)
		{
			this.selectedSquad = squad;
			if (squad == null)
			{
				this.ResetAvailableColonists(this.squadManager.AllColonistsSquad);
				this.ResetSquadMembers(null);
			}
			else
			{
				this.ResetAvailableColonists(squad);
				this.ResetSquadMembers(squad);
			}
			this.ResetVisibleSquadCount();
		}

		protected void ResetVisibleSquadCount()
		{
			this.visibleSquadCount = null;
		}

		protected void EnableSquadReorderButtons()
		{
			this.moveSquadToTopButtonEnabled = false;
			this.moveSquadToBottomButtonEnabled = false;
			this.reorderSquadDownButtonEnabled = false;
			this.reorderSquadUpButtonEnabled = false;
			if (this.selectedSquad != null)
			{
				int count = this.squadManager.Squads.Count;
				int num = this.squadManager.Squads.IndexOf(this.selectedSquad);
				if (num > -1 && count > 1)
				{
					this.moveSquadToTopButtonEnabled = true;
					this.reorderSquadUpButtonEnabled = true;
				}
				if (num > -1 && num < count - 1 && count > 1)
				{
					this.moveSquadToBottomButtonEnabled = true;
					this.reorderSquadDownButtonEnabled = true;
				}
			}
		}

		protected void SelectAvailableColonists(List<TrackedColonist> colonists)
		{
			if (colonists.Count > 0)
			{
				this.addMembersButtonEnabled = true;
				this.squadMembersWidget.ClearSelection();
				this.deleteColonistButtonEnabled = true;
				foreach (TrackedColonist current in this.squadMembersWidget.SelectedItems)
				{
					if (!current.Dead && !current.Missing)
					{
						this.deleteColonistButtonEnabled = false;
						break;
					}
				}
			}
			else
			{
				this.addMembersButtonEnabled = false;
				this.deleteColonistButtonEnabled = false;
			}
		}

		protected void SelectSquadMembers(List<TrackedColonist> colonists)
		{
			if (colonists.Count > 0)
			{
				this.removeMembersButtonEnabled = (this.selectedSquad != this.squadManager.AllColonistsSquad);
				this.availableColonistsWidget.ClearSelection();
				this.moveMemberToTopButtonEnabled = false;
				this.moveMemberToBottomButtonEnabled = false;
				this.reorderMemberDownButtonEnabled = false;
				this.reorderMemberUpButtonEnabled = false;
				int count = colonists.Count;
				foreach (int current in this.squadMembersWidget.SelectedIndices)
				{
					if (current >= count)
					{
						this.moveMemberToTopButtonEnabled = true;
						this.reorderMemberUpButtonEnabled = true;
						break;
					}
				}
				int num = this.selectedSquad.Pawns.Count - colonists.Count;
				foreach (int current2 in this.squadMembersWidget.SelectedIndices)
				{
					if (current2 < num)
					{
						this.moveMemberToBottomButtonEnabled = true;
						this.reorderMemberDownButtonEnabled = true;
						break;
					}
				}
				this.deleteColonistButtonEnabled = true;
				foreach (TrackedColonist current3 in this.squadMembersWidget.SelectedItems)
				{
					if (!current3.Dead && !current3.Missing)
					{
						this.deleteColonistButtonEnabled = false;
						break;
					}
				}
			}
			else
			{
				this.removeMembersButtonEnabled = false;
				this.deleteColonistButtonEnabled = false;
				this.moveMemberToTopButtonEnabled = false;
				this.moveMemberToBottomButtonEnabled = false;
				this.reorderMemberDownButtonEnabled = false;
				this.reorderMemberUpButtonEnabled = false;
			}
		}
	}
}
