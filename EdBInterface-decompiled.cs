using RimWorld;
using System;

namespace EdB.Interface
{
	public class AlertsReadoutComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		//
		// Fields
		//
		private AlertsReadout alertsReadout;

		//
		// Properties
		//
		public string Name {
			get {
				return "AlertsReadout";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Constructors
		//
		public AlertsReadoutComponent (AlertsReadout alertsReadout)
		{
			this.alertsReadout = alertsReadout;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.alertsReadout.AlertsReadoutOnGUI ();
		}

		public void Update ()
		{
			this.alertsReadout.AlertsReadoutUpdate ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class AllColonistsSquad : Squad
	{
		//
		// Properties
		//
		public override string Name {
			get {
				return this.name;
			}
			set {
			}
		}

		//
		// Constructors
		//
		public AllColonistsSquad ()
		{
			this.name = Translator.Translate ("EdB.Squads.AllColonistsSquadName");
		}

		//
		// Methods
		//
		public override bool Remove (Pawn pawn)
		{
			return base.Remove (pawn);
		}
	}
}
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ArchitectCategoryTab
	{
		//
		// Static Fields
		//
		public const float InfoRectHeight = 230;

		//
		// Fields
		//
		public DesignationCategoryDef def;

		//
		// Static Properties
		//
		public static Rect InfoRect {
			get {
				return new Rect (0, (float)(Screen.get_height () - 35) - ((MainTabWindow_Architect)MainTabDefOf.Architect.get_Window ()).get_WinHeight () - 230, 200, 230);
			}
		}

		//
		// Constructors
		//
		public ArchitectCategoryTab (DesignationCategoryDef def)
		{
			this.def = def;
		}

		//
		// Methods
		//
		public void DesignationTabOnGUI ()
		{
			if (DesignatorManager.get_SelectedDesignator () != null) {
				DesignatorManager.get_SelectedDesignator ().DoExtraGuiControls (0, (float)(Screen.get_height () - 35) - ((MainTabWindow_Architect)MainTabDefOf.Architect.get_Window ()).get_WinHeight () - 230);
			}
			float startX = 210;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid (this.def.resolvedDesignators.Cast<Gizmo> (), startX, out selectedDesignator);
			if (selectedDesignator == null && DesignatorManager.get_SelectedDesignator () != null) {
				selectedDesignator = DesignatorManager.get_SelectedDesignator ();
			}
			this.DoInfoBox (ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		protected void DoInfoBox (Rect infoRect, Designator designator)
		{
			Find.get_WindowStack ().ImmediateWindow (32520, infoRect, 0, delegate {
				if (designator != null) {
					Rect rect = GenUI.ContractedBy (GenUI.AtZero (infoRect), 7);
					GUI.BeginGroup (rect);
					Rect rect2 = new Rect (0, 0, rect.get_width (), 999);
					Text.set_Font (1);
					Widgets.Label (rect2, designator.get_LabelCap ());
					float num = 24;
					designator.DrawPanelReadout (ref num, rect.get_width ());
					Rect rect3 = new Rect (0, num, rect.get_width (), rect.get_height () - num);
					string desc = designator.get_Desc ();
					GenText.SetTextSizeToFit (desc, rect3);
					Widgets.Label (rect3, desc);
					GUI.EndGroup ();
				}
			}, true, false, 1);
		}

		public void PanelClosing ()
		{
			DesignatorManager.Deselect ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class AreaAllowedGUI
	{
		//
		// Static Methods
		//
		public static void DoAllowedAreaSelectors (Rect rect, Pawn p, AllowedAreaMode mode)
		{
			List<Area> allAreas = Find.get_AreaManager ().get_AllAreas ();
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++) {
				if (allAreas [i].AssignableAsAllowed (mode)) {
					num++;
				}
			}
			float num2 = rect.get_width () / (float)num;
			Text.set_WordWrap (false);
			Text.set_Font (0);
			Rect rect2 = new Rect (rect.get_x (), rect.get_y (), num2, rect.get_height ());
			AreaAllowedGUI.DoAreaSelector (rect2, p, null);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++) {
				if (allAreas [j].AssignableAsAllowed (mode)) {
					float num4 = (float)num3 * num2;
					Rect rect3 = new Rect (rect.get_x () + num4, rect.get_y (), num2, rect.get_height ());
					AreaAllowedGUI.DoAreaSelector (rect3, p, allAreas [j]);
					num3++;
				}
			}
			Text.set_WordWrap (true);
		}

		public static void DoAllowedAreaSelectors (Rect rect, ref Area areaRestriction, AllowedAreaMode mode, IEnumerable<Pawn> pawns)
		{
			List<Area> allAreas = Find.get_AreaManager ().get_AllAreas ();
			int num = 1;
			for (int i = 0; i < allAreas.Count; i++) {
				if (allAreas [i].AssignableAsAllowed (mode)) {
					num++;
				}
			}
			float num2 = rect.get_width () / (float)num;
			Text.set_WordWrap (false);
			Text.set_Font (0);
			Rect rect2 = new Rect (rect.get_x (), rect.get_y (), num2, rect.get_height ());
			AreaAllowedGUI.DoAreaSelector (rect2, ref areaRestriction, null, pawns);
			int num3 = 1;
			for (int j = 0; j < allAreas.Count; j++) {
				if (allAreas [j].AssignableAsAllowed (mode)) {
					float num4 = (float)num3 * num2;
					Rect rect3 = new Rect (rect.get_x () + num4, rect.get_y (), num2, rect.get_height ());
					AreaAllowedGUI.DoAreaSelector (rect3, ref areaRestriction, allAreas [j], pawns);
					num3++;
				}
			}
			Text.set_WordWrap (true);
		}

		private static void DoAreaSelector (Rect rect, ref Area areaRestriction, Area area, IEnumerable<Pawn> pawns)
		{
			rect = GenUI.ContractedBy (rect, 1);
			GUI.DrawTexture (rect, (area != null) ? area.get_ColorTexture () : BaseContent.GreyTex);
			Text.set_Anchor (3);
			string text = AreaUtility.AreaAllowedLabel_Area (area);
			Rect rect2 = rect;
			rect2.set_xMin (rect2.get_xMin () + 3);
			rect2.set_yMin (rect2.get_yMin () + 2);
			Widgets.Label (rect2, text);
			if (areaRestriction == area) {
				Widgets.DrawBox (rect, 2);
			}
			if (Mouse.IsOver (rect)) {
				if (area != null) {
					area.MarkForDraw ();
				}
				if (Input.GetMouseButton (0)) {
					areaRestriction = area;
					bool flag = false;
					foreach (Pawn current in pawns) {
						if (current.playerSettings.get_AreaRestriction () != area) {
							flag = true;
						}
						current.playerSettings.set_AreaRestriction (area);
					}
					if (flag) {
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.DesignateDragStandardChanged);
					}
				}
			}
			TooltipHandler.TipRegion (rect, text);
			Text.set_Anchor (0);
		}

		private static void DoAreaSelector (Rect rect, Pawn p, Area area)
		{
			rect = GenUI.ContractedBy (rect, 1);
			GUI.DrawTexture (rect, (area != null) ? area.get_ColorTexture () : BaseContent.GreyTex);
			Text.set_Anchor (3);
			string text = AreaUtility.AreaAllowedLabel_Area (area);
			Rect rect2 = rect;
			rect2.set_xMin (rect2.get_xMin () + 3);
			rect2.set_yMin (rect2.get_yMin () + 2);
			Widgets.Label (rect2, text);
			if (p.playerSettings.get_AreaRestriction () == area) {
				Widgets.DrawBox (rect, 2);
			}
			if (Mouse.IsOver (rect)) {
				if (area != null) {
					area.MarkForDraw ();
				}
				if (Input.GetMouseButton (0) && p.playerSettings.get_AreaRestriction () != area) {
					p.playerSettings.set_AreaRestriction (area);
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.DesignateDragStandardChanged);
				}
			}
			TooltipHandler.TipRegion (rect, text);
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal static class BeautyDrawer
	{
		//
		// Static Fields
		//
		private static Color ColorUgly = Color.get_red ();

		private static Color ColorBeautiful = Color.get_green ();

		private static List<Thing> tempCountedThings = new List<Thing> ();

		//
		// Static Methods
		//
		private static Color BeautyColor (float beauty)
		{
			float num = Mathf.InverseLerp (-40, 40, beauty);
			num = Mathf.Clamp01 (num);
			return Color.Lerp (BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num);
		}

		public static void BeautyOnGUI ()
		{
			if (!Find.get_PlaySettings ().showBeauty) {
				return;
			}
			if (!GenGrid.InBounds (Gen.MouseCell ()) || GridsUtility.Fogged (Gen.MouseCell ())) {
				return;
			}
			BeautyDrawer.tempCountedThings.Clear ();
			BeautyUtility.FillBeautyRelevantCells (Gen.MouseCell ());
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++) {
				IntVec3 intVec = BeautyUtility.beautyRelevantCells [i];
				float num = BeautyUtility.CellBeauty (intVec, BeautyDrawer.tempCountedThings);
				if (num != 0) {
					Vector3 vector = GenWorldUI.LabelDrawPosFor (intVec);
					GenWorldUI.DrawThingLabel (vector, GenString.ToStringCached (Mathf.RoundToInt (num)), BeautyDrawer.BeautyColor (num));
				}
			}
			Text.set_Font (2);
			Rect rect = new Rect (Event.get_current ().get_mousePosition ().x + 19, Event.get_current ().get_mousePosition ().y + 19, 100, 100);
			float beauty = BeautyUtility.AverageBeautyPerceptible (Gen.MouseCell ());
			GUI.set_color (BeautyDrawer.BeautyColor (beauty));
			Widgets.Label (rect, beauty.ToString ("F1"));
			GUI.set_color (Color.get_white ());
		}
	}
}
using System;

namespace EdB.Interface
{
	public class BeautyDrawerComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "BeautyDrawer";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			BeautyDrawer.BeautyOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class BillDrawer
	{
		//
		// Static Fields
		//
		private static MethodInfo statusStringGetter;

		public static readonly Texture2D ButtonBGAtlas;

		public static readonly Texture2D ButtonBGAtlasMouseover;

		public static readonly Texture2D ButtonBGAtlasClick;

		public static Color ButtonColor;

		public static Color ButtonColorDisabled;

		public static float ProductionBillPadding;

		public static readonly Texture2D ButtonTexPlus;

		private static MethodInfo drawConfigInterfaceMethod;

		public static Texture2D ButtonTexReorderUp;

		public static Texture2D ButtonTexReorderDown;

		public static Texture2D ButtonTexSuspend;

		public static readonly Texture2D ButtonTexDeleteX;

		public static readonly Texture2D ButtonTexMinus;

		//
		// Constructors
		//
		static BillDrawer ()
		{
			BillDrawer.ButtonTexDeleteX = ContentFinder<Texture2D>.Get ("UI/Buttons/Delete", true);
			BillDrawer.ButtonTexMinus = ContentFinder<Texture2D>.Get ("UI/Buttons/Minus", true);
			BillDrawer.ButtonTexPlus = ContentFinder<Texture2D>.Get ("UI/Buttons/Plus", true);
			BillDrawer.ButtonBGAtlas = ContentFinder<Texture2D>.Get ("UI/Widgets/ButtonBG", true);
			BillDrawer.ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get ("UI/Widgets/ButtonBGMouseover", true);
			BillDrawer.ButtonBGAtlasClick = ContentFinder<Texture2D>.Get ("UI/Widgets/ButtonBGClick", true);
			BillDrawer.ButtonColor = new Color (1, 0.8627, 0.2235);
			BillDrawer.ButtonColorDisabled = new Color (BillDrawer.ButtonColor.r, BillDrawer.ButtonColor.g, BillDrawer.ButtonColor.b, 0.0627);
			BillDrawer.ProductionBillPadding = 3;
			PropertyInfo property = typeof(Bill).GetProperty ("StatusString", BindingFlags.Instance | BindingFlags.NonPublic);
			BillDrawer.statusStringGetter = property.GetGetMethod (true);
			BillDrawer.drawConfigInterfaceMethod = typeof(Bill).GetMethod ("DrawConfigInterface", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Static Methods
		//
		public static Bill DrawListing (BillStack billStack, Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ScrollView scrollView)
		{
			Bill result = null;
			GUI.BeginGroup (rect);
			Text.set_Font (1);
			if (billStack.get_Count () < 10) {
				Rect rect2 = new Rect (0, 0, 150, 29);
				if (Widgets.TextButton (rect2, Translator.Translate ("AddBill"), true, false)) {
					Find.get_WindowStack ().Add (new FloatMenu (recipeOptionsMaker.Invoke (), false));
				}
			}
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Rect viewRect = new Rect (0, 35, rect.get_width (), rect.get_height () - 35);
			scrollView.Begin (viewRect);
			float num = 0;
			for (int i = 0; i < billStack.get_Count (); i++) {
				Bill bill = billStack.get_Item (i);
				Rect rect3 = BillDrawer.DrawProductionBill (billStack, bill, 0, num, scrollView.ViewWidth, i);
				if (!bill.get_DeletedOrDereferenced () && rect3.Contains (Event.get_current ().get_mousePosition ())) {
					result = bill;
				}
				num += rect3.get_height () + 6;
			}
			scrollView.End (num + 60);
			GUI.EndGroup ();
			return result;
		}

		public static Rect DrawMedicalBill (BillStack billStack, Bill bill, float x, float y, float width, int index)
		{
			string text = (string)BillDrawer.statusStringGetter.Invoke (bill, null);
			Rect rect = new Rect (x, y, width, 48);
			float num = rect.get_width () - 106;
			float num2 = Text.CalcHeight (bill.get_LabelCap (), num);
			float num3 = num2 + 10;
			rect.set_height ((num3 >= rect.get_height ()) ? num3 : rect.get_height ());
			if (!GenText.NullOrEmpty (text)) {
				rect.set_height (rect.get_height () + 17);
			}
			Color white = Color.get_white ();
			if (!bill.ShouldDoNow ()) {
				white = new Color (1, 0.7, 0.7, 0.7);
			}
			GUI.set_color (white);
			Text.set_Font (1);
			if (index % 2 == 0) {
				Widgets.DrawAltRect (rect);
			}
			if (Mouse.IsOver (rect)) {
				Widgets.DrawAltRect (rect);
				if (index % 2 == 1) {
					Widgets.DrawAltRect (rect);
				}
			}
			GUI.set_color (new Color (0.2969, 0.3359, 0.3789));
			Widgets.DrawBox (rect, 1);
			GUI.set_color (white);
			try {
				GUI.BeginGroup (rect);
				Rect rect2 = new Rect (10, 4, 24, 20);
				GUI.set_color (BillDrawer.ButtonColor);
				if (billStack.IndexOf (bill) > 0) {
					if (Widgets.ImageButton (rect2, BillDrawer.ButtonTexReorderUp, white)) {
						billStack.Reorder (bill, -1);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickHigh);
					}
				}
				else if (billStack.get_Count () > 1) {
					GUI.set_color (BillDrawer.ButtonColorDisabled);
					GUI.DrawTexture (rect2, BillDrawer.ButtonTexReorderUp);
				}
				Rect rect3 = new Rect (10, 26, 24, 20);
				if (billStack.IndexOf (bill) < billStack.get_Count () - 1) {
					GUI.set_color (BillDrawer.ButtonColor);
					if (Widgets.ImageButton (rect3, BillDrawer.ButtonTexReorderDown, white)) {
						billStack.Reorder (bill, 1);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickLow);
					}
				}
				else if (billStack.get_Count () > 1) {
					GUI.set_color (BillDrawer.ButtonColorDisabled);
					GUI.DrawTexture (rect3, BillDrawer.ButtonTexReorderDown);
				}
				GUI.set_color (white);
				Rect rect4 = new Rect (42, 1, num, rect.get_height ());
				Text.set_Anchor (3);
				if (bill.suspended) {
					GUI.set_color (new Color (white.r, white.g, white.b, 0.45 * white.a));
				}
				Widgets.Label (rect4, bill.get_LabelCap ());
				Text.set_Anchor (0);
				BillDrawer.drawConfigInterfaceMethod.Invoke (bill, new object[] {
					GenUI.AtZero (rect),
					white
				});
				Rect rect5 = new Rect (rect.get_width () - 28, 4, 24, 24);
				if (Widgets.ImageButton (rect5, TexButton.DeleteX, white)) {
					billStack.Delete (bill);
				}
				Rect rect6 = new Rect (rect5);
				rect6.set_x (rect6.get_x () - (rect6.get_width () + 4));
				GUI.set_color (BillDrawer.ButtonColor);
				if (Widgets.ImageButton (rect6, BillDrawer.ButtonTexSuspend, white)) {
					bill.suspended = !bill.suspended;
				}
				if (!GenText.NullOrEmpty (text)) {
					Text.set_Font (0);
					Rect rect7 = new Rect (24, rect.get_height () - 17, rect.get_width () - 24, 17);
					Widgets.Label (rect7, text);
				}
			}
			finally {
				GUI.EndGroup ();
			}
			if (bill.suspended) {
				Text.set_Font (2);
				Text.set_Anchor (4);
				Rect rect8 = new Rect (rect.get_x () + rect.get_width () / 2 - 70, rect.get_y () + rect.get_height () / 2 - 20, 140, 40);
				GUI.DrawTexture (rect8, TexUI.GrayTextBG);
				GUI.set_color (new Color (0.9, 0.9, 0.9, 1));
				Widgets.Label (rect8, Translator.Translate ("SuspendedCaps"));
				Text.set_Anchor (0);
				Text.set_Font (1);
			}
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			return rect;
		}

		public static Rect DrawProductionBill (BillStack billStack, Bill bill, float x, float y, float width, int index)
		{
			Rect rect = new Rect (x, y, width - 16, 60);
			float num = rect.get_width () - BillDrawer.ProductionBillPadding * 2 - 100;
			float num2 = Text.CalcHeight (bill.get_LabelCap (), num);
			string text = (string)BillDrawer.statusStringGetter.Invoke (bill, null);
			Text.set_Font (0);
			float num3 = rect.get_width () - BillDrawer.ProductionBillPadding * 2 - 48;
			float num4 = (!string.IsNullOrEmpty (text)) ? (Text.CalcHeight (text, num3) - 4) : 0;
			Text.set_Font (1);
			float height = 44 + num2 + num4;
			rect.set_height (height);
			Bill_Production productionBill = bill as Bill_Production;
			Color white = Color.get_white ();
			if (!bill.ShouldDoNow ()) {
				white = new Color (1, 0.7, 0.7, 0.7);
			}
			GUI.set_color (white);
			if (index % 2 == 0) {
				Widgets.DrawAltRect (rect);
			}
			if (Mouse.IsOver (rect)) {
				Widgets.DrawAltRect (rect);
				if (index % 2 == 1) {
					Widgets.DrawAltRect (rect);
				}
			}
			GUI.set_color (new Color (0.2969, 0.3359, 0.3789));
			Widgets.DrawBox (rect, 1);
			GUI.set_color (white);
			Rect rect2 = GenUI.ContractedBy (rect, BillDrawer.ProductionBillPadding);
			GUI.BeginGroup (rect2);
			Rect rect3 = new Rect (2, 3, 24, 24);
			if (billStack.IndexOf (bill) > 0) {
				GUI.set_color (BillDrawer.ButtonColor);
				if (Widgets.ImageButton (rect3, BillDrawer.ButtonTexReorderUp, white)) {
					billStack.Reorder (bill, -1);
					SoundStarter.PlayOneShotOnCamera (SoundDef.Named ("TickHigh"));
				}
			}
			else if (billStack.get_Count () > 1) {
				GUI.set_color (BillDrawer.ButtonColorDisabled);
				GUI.DrawTexture (rect3, BillDrawer.ButtonTexReorderUp);
			}
			Rect rect4 = new Rect (2, 28, 24, 24);
			if (billStack.IndexOf (bill) < billStack.get_Count () - 1) {
				GUI.set_color (BillDrawer.ButtonColor);
				if (Widgets.ImageButton (rect4, BillDrawer.ButtonTexReorderDown, white)) {
					billStack.Reorder (bill, 1);
					SoundStarter.PlayOneShotOnCamera (SoundDef.Named ("TickLow"));
				}
			}
			else if (billStack.get_Count () > 1) {
				GUI.set_color (BillDrawer.ButtonColorDisabled);
				GUI.DrawTexture (rect4, BillDrawer.ButtonTexReorderDown);
			}
			GUI.set_color (white);
			Rect rect5 = new Rect (36, 4, num, num2);
			Widgets.Label (rect5, bill.recipe.get_LabelCap ());
			float num5 = 26;
			float num6 = rect5.get_height () + 6;
			if (productionBill != null) {
				Rect rect6 = new Rect (32, num6, 180, num5);
				string text2 = null;
				if (productionBill.repeatMode == null) {
					text2 = Translator.Translate ("DoXTimes");
					text2 = text2.Replace ("X", string.Empty + productionBill.repeatCount);
					productionBill.targetCount = productionBill.repeatCount;
				}
				if (productionBill.repeatMode == 1) {
					text2 = Translator.Translate ("DoUntilYouHaveX");
					text2 = text2.Replace ("X", string.Empty + productionBill.targetCount);
					productionBill.repeatCount = productionBill.targetCount;
				}
				if (productionBill.repeatMode == 2) {
					text2 = Translator.Translate ("DoForever");
				}
				if (BillDrawer.TextButton (rect6, text2, white, 0)) {
					List<FloatMenuOption> list = new List<FloatMenuOption> ();
					list.Add (new FloatMenuOption (Translator.Translate ("DoXTimes"), delegate {
						productionBill.repeatMode = 0;
					}, 1, null, null));
					FloatMenuOption item = new FloatMenuOption (Translator.Translate ("DoUntilYouHaveX"), delegate {
						if (!productionBill.recipe.get_WorkerCounter ().CanCountProducts (productionBill)) {
							Messages.Message (Translator.Translate ("RecipeCannotHaveTargetCount"), 2);
						}
						else {
							productionBill.repeatMode = 1;
						}
					}, 1, null, null);
					list.Add (item);
					list.Add (new FloatMenuOption (Translator.Translate ("DoForever"), delegate {
						productionBill.repeatMode = 2;
					}, 1, null, null));
					Find.get_WindowStack ().Add (new FloatMenu (list, false));
				}
				if (!GenText.NullOrEmpty (text)) {
					Rect rect7 = new Rect (rect6.get_x () + 3, rect6.get_y () + rect6.get_height () + 4, num3, num4);
					Text.set_Font (0);
					Widgets.Label (rect7, text);
					Text.set_Font (1);
				}
				Rect rect8 = new Rect (213, num6, 27, num5);
				if (BillDrawer.TextButton (rect8, "-", white, -1)) {
					if (productionBill.repeatMode == 2) {
						productionBill.repeatMode = 0;
						productionBill.repeatCount = 1;
					}
					if (productionBill.repeatMode == 1) {
						productionBill.targetCount = Mathf.Max (1, productionBill.targetCount - 1);
					}
					if (productionBill.repeatMode == null) {
						productionBill.repeatCount = Mathf.Max (1, productionBill.repeatCount - 1);
					}
					SoundStarter.PlayOneShotOnCamera (SoundDef.Named ("TickLow"));
				}
				Rect rect9 = new Rect (243, num6, 27, num5);
				if (BillDrawer.TextButton (rect9, "+", white, -1)) {
					if (productionBill.repeatMode == 2) {
						productionBill.repeatMode = 0;
						productionBill.repeatCount = 1;
					}
					if (productionBill.repeatMode == 1) {
						productionBill.targetCount++;
					}
					if (productionBill.repeatMode == null) {
						productionBill.repeatCount++;
					}
					SoundStarter.PlayOneShotOnCamera (SoundDef.Named ("TickHigh"));
				}
				Rect rect10 = new Rect (276, num6, 35, num5);
				if (BillDrawer.TextButton (rect10, "...", white, 0)) {
					Find.get_WindowStack ().Add (new Dialog_BillConfig (productionBill, ((Thing)productionBill.billStack.billGiver).get_Position ()));
				}
			}
			GUI.set_color (white);
			Rect rect11 = new Rect (rect2.get_width () - 28, 1, 24, 24);
			if (Widgets.ImageButton (rect11, BillDrawer.ButtonTexDeleteX, white)) {
				billStack.Delete (bill);
			}
			Rect rect12 = new Rect (rect11);
			rect12.set_x (rect12.get_x () - (rect12.get_width () + 4));
			GUI.set_color (BillDrawer.ButtonColor);
			if (Widgets.ImageButton (rect12, BillDrawer.ButtonTexSuspend, white)) {
				bill.suspended = !bill.suspended;
			}
			GUI.EndGroup ();
			if (bill.suspended) {
				Text.set_Font (2);
				Text.set_Anchor (4);
				Rect rect13 = new Rect (rect.get_x () + rect.get_width () / 2 - 70, rect.get_y () + rect.get_height () / 2 - 20, 140, 40);
				GUI.DrawTexture (rect13, TexUI.GrayTextBG);
				Widgets.Label (rect13, Translator.Translate ("SuspendedCaps"));
				Text.set_Anchor (0);
				Text.set_Font (1);
			}
			return rect;
		}

		public static void ResetTextures ()
		{
			BillDrawer.ButtonTexReorderUp = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/ReorderUp", true);
			BillDrawer.ButtonTexReorderDown = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/ReorderDown", true);
			BillDrawer.ButtonTexSuspend = ContentFinder<Texture2D>.Get ("UI/Buttons/Suspend", true);
		}

		public static bool TextButton (Rect rect, string label, Color optionColor, float labelAdjustmentX = 0)
		{
			Texture2D texture2D = BillDrawer.ButtonBGAtlas;
			if (rect.Contains (Event.get_current ().get_mousePosition ())) {
				texture2D = BillDrawer.ButtonBGAtlasMouseover;
				if (Input.GetMouseButton (0)) {
					texture2D = BillDrawer.ButtonBGAtlasClick;
				}
			}
			Widgets.DrawAtlas (rect, texture2D);
			Text.set_Anchor (4);
			Rect rect2 = new Rect (rect);
			rect2.set_x (rect2.get_x () + labelAdjustmentX);
			rect2.set_y (rect2.get_y () + 1);
			Widgets.Label (rect2, label);
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			return Widgets.InvisibleButton (rect);
		}
	}
}
using System;

namespace EdB.Interface
{
	public class BillExtensions
	{
	}
}
using System;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class BooleanPreference : IPreference
	{
		public delegate void ValueChangedHandler (bool value);

		//
		// Static Fields
		//
		public static float LabelMargin = BooleanPreference.CheckboxWidth + BooleanPreference.CheckboxMargin;

		public static float CheckboxMargin = 18;

		public static float CheckboxWidth = 24;

		//
		// Fields
		//
		public int tooltipId;

		private bool? boolValue;

		private string stringValue;

		//
		// Properties
		//
		public abstract bool DefaultValue {
			get;
		}

		public virtual bool Disabled {
			get {
				return false;
			}
		}

		public virtual bool DisplayInOptions {
			get {
				return true;
			}
		}

		public abstract string Group {
			get;
		}

		public virtual bool Indent {
			get {
				return false;
			}
		}

		public virtual string Label {
			get {
				return Translator.Translate (this.Name);
			}
		}

		public abstract string Name {
			get;
		}

		public virtual string Tooltip {
			get {
				return null;
			}
		}

		protected virtual int TooltipId {
			get {
				if (this.tooltipId == 0) {
					this.tooltipId = Translator.Translate (this.Tooltip).GetHashCode ();
					return this.tooltipId;
				}
				return 0;
			}
		}

		public virtual bool Value {
			get {
				bool? flag = this.boolValue;
				if (flag.HasValue) {
					return this.boolValue.Value;
				}
				return this.DefaultValue;
			}
			set {
				bool? flag = this.boolValue;
				this.boolValue = new bool? (value);
				this.stringValue = ((!value) ? "false" : "true");
				if ((!flag.HasValue || flag.Value != this.boolValue) && this.ValueChanged != null) {
					this.ValueChanged (value);
				}
			}
		}

		public virtual bool ValueForDisplay {
			get {
				bool? flag = this.boolValue;
				if (flag.HasValue) {
					return this.boolValue.Value;
				}
				return this.DefaultValue;
			}
		}

		public string ValueForSerialization {
			get {
				return this.stringValue;
			}
			set {
				if ("true".Equals (value)) {
					this.boolValue = new bool? (true);
					this.stringValue = value;
				}
				else if ("false".Equals (value)) {
					this.boolValue = new bool? (false);
					this.stringValue = value;
				}
				else {
					if (!string.IsNullOrEmpty (value)) {
						this.boolValue = null;
						this.stringValue = null;
						throw new ArgumentException ("Cannot set this true/false preference to the specified non-boolean value.");
					}
					this.boolValue = null;
					this.stringValue = null;
				}
			}
		}

		//
		// Constructors
		//
		public BooleanPreference ()
		{
		}

		//
		// Methods
		//
		public void OnGUI (float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			float num = (!this.Indent) ? 0 : Dialog_InterfaceOptions.IndentSize;
			string label = this.Label;
			float num2 = Text.CalcHeight (label, width - BooleanPreference.LabelMargin - num);
			Rect rect = new Rect (positionX - 4 + num, positionY - 3, width + 6 - num, num2 + 5);
			if (Mouse.IsOver (rect)) {
				Widgets.DrawHighlight (rect);
			}
			Rect rect2 = new Rect (positionX + num, positionY, width - BooleanPreference.LabelMargin - num, num2);
			if (disabled) {
				GUI.set_color (Dialog_InterfaceOptions.DisabledControlColor);
			}
			GUI.Label (rect2, label);
			GUI.set_color (Color.get_white ());
			if (this.Tooltip != null) {
				TipSignal tipSignal = new TipSignal (() => Translator.Translate (this.Tooltip), this.TooltipId);
				TooltipHandler.TipRegion (rect2, tipSignal);
			}
			bool valueForDisplay = this.ValueForDisplay;
			Widgets.Checkbox (new Vector2 (positionX + width - BooleanPreference.CheckboxWidth, positionY - 2), ref valueForDisplay, 24, disabled);
			this.Value = valueForDisplay;
			positionY += num2;
		}

		//
		// Events
		//
		public event BooleanPreference.ValueChangedHandler ValueChanged {
			add {
				BooleanPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				BooleanPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<BooleanPreference.ValueChangedHandler> (ref this.ValueChanged, (BooleanPreference.ValueChangedHandler)Delegate.Combine (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove {
				BooleanPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				BooleanPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<BooleanPreference.ValueChangedHandler> (ref this.ValueChanged, (BooleanPreference.ValueChangedHandler)Delegate.Remove (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Bootstrap : ITab
	{
		//
		// Fields
		//
		protected GameObject gameObject;

		//
		// Constructors
		//
		public Bootstrap ()
		{
			Log.Message ("Initialized EdB Interface.");
			this.gameObject = new GameObject (Controller.GameObjectName);
			this.gameObject.AddComponent<Controller> ();
			Object.DontDestroyOnLoad (this.gameObject);
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
		}
	}
}
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class BrowseButtonDrawer
	{
		//
		// Static Fields
		//
		public static SelectorUtility selector = new SelectorUtility ();

		public static Texture2D ButtonTexturePrevious;

		public static Texture2D ButtonTextureNext;

		public static Color ButtonColor = new Color (0.75, 0.75, 0.75);

		public static Vector2 ButtonSize = new Vector2 (15, 17);

		//
		// Static Methods
		//
		public static void DrawBrowseButtons (Vector2 tabSize, Pawn currentPawn)
		{
			float num = 14;
			float padding = 30;
			float num2 = BrowseButtonDrawer.ButtonSize.y + num * 2;
			BrowseButtonDrawer.DrawBrowseButtons (tabSize.y - num2 + num, tabSize.x, padding, currentPawn);
		}

		public static void DrawBrowseButtons (float top, float width, float padding, Pawn currentPawn)
		{
			if (currentPawn != null) {
				Action action = null;
				Action action2 = null;
				if (currentPawn.get_IsColonist ()) {
					if (Find.get_ListerPawns ().get_FreeColonists ().Count<Pawn> () > 1) {
						action = delegate {
							BrowseButtonDrawer.selector.SelectNextColonist ();
						};
						action2 = delegate {
							BrowseButtonDrawer.selector.SelectPreviousColonist ();
						};
					}
				}
				else if (currentPawn.get_IsPrisonerOfColony ()) {
					if (Find.get_ListerPawns ().get_PrisonersOfColonyCount () > 1) {
						action = delegate {
							BrowseButtonDrawer.selector.SelectNextPrisoner ();
						};
						action2 = delegate {
							BrowseButtonDrawer.selector.SelectPreviousPrisoner ();
						};
					}
				}
				else {
					Faction faction = currentPawn.get_Faction ();
					if (faction != null) {
						if (faction != Faction.get_OfColony ()) {
							FactionRelation factionRelation = faction.RelationWith (Faction.get_OfColony ());
							if (factionRelation != null) {
								bool hostile = factionRelation.hostile;
								if (hostile) {
									if (BrowseButtonDrawer.selector.MoreThanOneHostilePawn) {
										action = delegate {
											BrowseButtonDrawer.selector.SelectNextEnemy ();
										};
										action2 = delegate {
											BrowseButtonDrawer.selector.SelectPreviousEnemy ();
										};
									}
								}
								else if (BrowseButtonDrawer.selector.MoreThanOneVisitorPawn) {
									action = delegate {
										BrowseButtonDrawer.selector.SelectNextVisitor ();
									};
									action2 = delegate {
										BrowseButtonDrawer.selector.SelectPreviousVisitor ();
									};
								}
							}
						}
						else if (BrowseButtonDrawer.selector.MoreThanOneColonyAnimal) {
							action = delegate {
								BrowseButtonDrawer.selector.SelectNextColonyAnimal ();
							};
							action2 = delegate {
								BrowseButtonDrawer.selector.SelectPreviousColonyAnimal ();
							};
						}
					}
				}
				Rect rect = new Rect (0, 0, BrowseButtonDrawer.ButtonSize.x, BrowseButtonDrawer.ButtonSize.y);
				if (action != null && action2 != null) {
					rect.set_x (padding - rect.get_width ());
					rect.set_y (top);
					if (rect.Contains (Event.get_current ().get_mousePosition ())) {
						GUI.set_color (Color.get_white ());
					}
					else {
						GUI.set_color (BrowseButtonDrawer.ButtonColor);
					}
					GUI.DrawTexture (rect, BrowseButtonDrawer.ButtonTexturePrevious);
					if (Widgets.InvisibleButton (rect)) {
						action2.Invoke ();
					}
					rect.set_x (width - padding);
					rect.set_y (top);
					if (rect.Contains (Event.get_current ().get_mousePosition ())) {
						GUI.set_color (Color.get_white ());
					}
					else {
						GUI.set_color (BrowseButtonDrawer.ButtonColor);
					}
					GUI.DrawTexture (rect, BrowseButtonDrawer.ButtonTextureNext);
					if (Widgets.InvisibleButton (rect)) {
						action.Invoke ();
					}
				}
			}
		}

		public static void ResetTextures ()
		{
			BrowseButtonDrawer.ButtonTextureNext = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/BrowseNext", true);
			BrowseButtonDrawer.ButtonTexturePrevious = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/BrowsePrevious", true);
			BrowseButtonDrawer.ButtonSize = new Vector2 ((float)BrowseButtonDrawer.ButtonTextureNext.get_width (), (float)BrowseButtonDrawer.ButtonTextureNext.get_height ());
		}
	}
}
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class Button
	{
		//
		// Static Fields
		//
		protected static Texture2D ButtonBGAtlas;

		protected static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get ("UI/Widgets/ButtonBGMouseover", true);

		protected static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get ("UI/Widgets/ButtonBGClick", true);

		protected static Color InactiveButtonColor = new Color (1, 1, 1, 0.5);

		protected static readonly Color MouseoverOptionColor = Color.get_yellow ();

		//
		// Static Methods
		//
		public static bool IconButton (Rect rect, Texture texture, Color baseColor, Color highlightColor, bool enabled)
		{
			if (texture == null) {
				return false;
			}
			if (!enabled) {
				GUI.set_color (Button.InactiveButtonColor);
			}
			else {
				GUI.set_color (Color.get_white ());
			}
			Texture2D texture2D = Button.ButtonBGAtlas;
			if (enabled && rect.Contains (Event.get_current ().get_mousePosition ())) {
				texture2D = Button.ButtonBGAtlasMouseover;
				if (Input.GetMouseButton (0)) {
					texture2D = Button.ButtonBGAtlasClick;
				}
			}
			Widgets.DrawAtlas (rect, texture2D);
			Rect rect2 = new Rect (rect.get_x () + rect.get_width () / 2 - (float)(texture.get_width () / 2), rect.get_y () + rect.get_height () / 2 - (float)(texture.get_height () / 2), (float)texture.get_width (), (float)texture.get_height ());
			if (!enabled) {
				GUI.set_color (Button.InactiveButtonColor);
			}
			else {
				GUI.set_color (baseColor);
			}
			if (enabled && rect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (highlightColor);
			}
			GUI.DrawTexture (rect2, texture);
			GUI.set_color (Color.get_white ());
			return enabled && Widgets.InvisibleButton (rect);
		}

		public static bool ImageButton (Rect butRect, Texture2D tex)
		{
			return Button.ImageButton (butRect, tex, GenUI.MouseoverColor);
		}

		public static bool ImageButton (Rect butRect, Texture2D tex, Color baseColor, Color mouseOverColor)
		{
			if (butRect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (mouseOverColor);
			}
			GUI.DrawTexture (butRect, tex);
			GUI.set_color (baseColor);
			return Widgets.InvisibleButton (butRect);
		}

		public static bool ImageButton (Rect butRect, Texture2D tex, Color highlightColor)
		{
			Color color = GUI.get_color ();
			if (butRect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (highlightColor);
			}
			GUI.DrawTexture (butRect, tex);
			GUI.set_color (color);
			return Widgets.InvisibleButton (butRect);
		}

		public static void ResetTextures ()
		{
			Button.ButtonBGAtlas = ContentFinder<Texture2D>.Get ("EdB/Interface/TextButton", true);
		}

		public static bool TextButton (Rect rect, string label, bool drawBackground, bool doMouseoverSound, bool enabled)
		{
			TextAnchor anchor = Text.get_Anchor ();
			Color color = GUI.get_color ();
			GUI.set_color ((!enabled) ? Button.InactiveButtonColor : Color.get_white ());
			if (drawBackground) {
				Texture2D texture2D = Button.ButtonBGAtlas;
				if (enabled && rect.Contains (Event.get_current ().get_mousePosition ())) {
					texture2D = Button.ButtonBGAtlasMouseover;
					if (Input.GetMouseButton (0)) {
						texture2D = Button.ButtonBGAtlasClick;
					}
				}
				Widgets.DrawAtlas (rect, texture2D);
			}
			if (doMouseoverSound) {
				MouseoverSounds.DoRegion (rect);
			}
			if (!drawBackground && enabled && rect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (Button.MouseoverOptionColor);
			}
			if (drawBackground) {
				Text.set_Anchor (4);
			}
			else {
				Text.set_Anchor (3);
			}
			Widgets.Label (rect, label);
			Text.set_Anchor (anchor);
			GUI.set_color (color);
			return enabled && Widgets.InvisibleButton (rect);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ColonistBar
	{
		public delegate void SelectedGroupChangedHandler (ColonistBarGroup group);

		//
		// Static Fields
		//
		protected static Color BrowseButtonColor = new Color (1, 1, 1, 0.15);

		protected static Color BrowseButtonHighlightColor = new Color (1, 1, 1, 0.5);

		protected static Color GroupNameColor = new Color (0.85, 0.85, 0.85);

		protected static float GroupNameDisplayDuration = 1;

		protected static float GroupNameEaseOutDuration = 0.4;

		protected static float GroupNameEaseOutStart = ColonistBar.GroupNameDisplayDuration - ColonistBar.GroupNameEaseOutDuration;

		protected static Texture2D BrowseGroupsDown;

		protected static readonly bool LoggingEnabled = false;

		protected static Texture2D BrowseGroupsUp;

		//
		// Fields
		//
		protected PreferenceSmallIcons preferenceSmallIcons = new PreferenceSmallIcons ();

		protected bool alwaysShowGroupName;

		protected List<TrackedColonist> slots = new List<TrackedColonist> ();

		protected GameObject drawerGameObject;

		protected ColonistBarDrawer drawer;

		protected ColonistBarGroup currentGroup;

		protected float lastKeyTime;

		protected KeyCode lastKey;

		protected PreferenceEnabled preferenceEnabled = new PreferenceEnabled ();

		protected string currentGroupId = string.Empty;

		protected List<ColonistBarGroup> groups = new List<ColonistBarGroup> ();

		private bool displayGroupName = true;

		protected float squadNameDisplayTimestamp;

		protected bool barVisible = true;

		protected List<IPreference> preferences = new List<IPreference> ();

		protected bool enableGroups = true;

		protected KeyBindingDef previousGroupKeyBinding;

		protected KeyBindingDef nextGroupKeyBinding;

		protected List<KeyBindingDef> squadSelectionBindings = new List<KeyBindingDef> ();

		//
		// Properties
		//
		public bool AlwaysShowGroupName {
			get {
				return this.alwaysShowGroupName;
			}
			set {
				this.alwaysShowGroupName = value;
			}
		}

		public ColonistBarGroup CurrentGroup {
			get {
				return this.currentGroup;
			}
			set {
				bool flag = value != this.currentGroup;
				if (flag) {
					this.currentGroup = value;
					if (this.currentGroup != null) {
						if (!string.IsNullOrEmpty (this.currentGroupId) && this.currentGroup.Id != this.currentGroupId) {
							this.ResetGroupNameDisplay ();
						}
						this.currentGroupId = this.currentGroup.Id;
					}
					else {
						this.currentGroupId = string.Empty;
					}
					if (this.SelectedGroupChanged != null) {
						this.SelectedGroupChanged (this.currentGroup);
					}
				}
				if (this.currentGroup != null) {
					this.drawer.Slots = this.currentGroup.Colonists;
				}
			}
		}

		public bool DisplayGroupName {
			get {
				return this.displayGroupName;
			}
			set {
				this.displayGroupName = value;
			}
		}

		public ColonistBarDrawer Drawer {
			get {
				return this.drawer;
			}
		}

		public bool EnableGroups {
			get {
				return this.enableGroups;
			}
			set {
				this.enableGroups = value;
			}
		}

		public bool GroupsBrowsable {
			get {
				return this.groups.Count > 1;
			}
		}

		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ColonistBar ()
		{
			this.preferences.Add (this.preferenceEnabled);
			this.preferences.Add (this.preferenceSmallIcons);
			this.Reset ();
		}

		//
		// Static Methods
		//
		public static void ResetTextures ()
		{
			ColonistBar.BrowseGroupsUp = ContentFinder<Texture2D>.Get ("EdB/Interface/ColonistBar/BrowseGroupUp", true);
			ColonistBar.BrowseGroupsDown = ContentFinder<Texture2D>.Get ("EdB/Interface/ColonistBar/BrowseGroupDown", true);
		}

		//
		// Methods
		//
		public void AddGroup (ColonistBarGroup group)
		{
			this.groups.Add (group);
		}

		public void Draw ()
		{
			if (!this.preferenceEnabled.Value) {
				return;
			}
			if (this.currentGroup == null || this.currentGroup.Colonists.Count == 0) {
				return;
			}
			if (this.drawer != null) {
				this.drawer.Draw ();
				this.drawer.DrawTexturesForSlots ();
				this.drawer.DrawToggleButton ();
			}
			if (this.enableGroups) {
				bool value = this.preferenceSmallIcons.Value;
				if (value && !this.drawer.SmallColonistIcons) {
					this.drawer.UseSmallIcons ();
				}
				else if (!value && this.drawer.SmallColonistIcons) {
					this.drawer.UseLargeIcons ();
				}
				if (this.GroupsBrowsable) {
					GUI.set_color (ColonistBar.BrowseButtonColor);
					Rect butRect = value ? new Rect (592, 15, 32, 18) : new Rect (592, 25, 32, 18);
					if (butRect.Contains (Event.get_current ().get_mousePosition ())) {
						this.squadNameDisplayTimestamp = Time.get_time ();
					}
					if (Button.ImageButton (butRect, ColonistBar.BrowseGroupsUp, ColonistBar.BrowseButtonHighlightColor)) {
						this.SelectNextGroup (-1);
					}
					GUI.set_color (ColonistBar.BrowseButtonColor);
					butRect = (value ? new Rect (592, 39, 32, 18) : new Rect (592, 49, 32, 18));
					if (butRect.Contains (Event.get_current ().get_mousePosition ())) {
						this.squadNameDisplayTimestamp = Time.get_time ();
					}
					if (Button.ImageButton (butRect, ColonistBar.BrowseGroupsDown, ColonistBar.BrowseButtonHighlightColor)) {
						this.SelectNextGroup (1);
					}
					GUI.set_color (Color.get_white ());
				}
				bool flag = false;
				Color groupNameColor = ColonistBar.GroupNameColor;
				Color black = Color.get_black ();
				if (this.alwaysShowGroupName) {
					flag = true;
				}
				else if (this.displayGroupName && this.squadNameDisplayTimestamp > 0) {
					float time = Time.get_time ();
					float num = time - this.squadNameDisplayTimestamp;
					if (num < ColonistBar.GroupNameDisplayDuration) {
						flag = true;
						if (num > ColonistBar.GroupNameEaseOutStart) {
							float num2 = num - ColonistBar.GroupNameEaseOutStart;
							float groupNameEaseOutDuration = ColonistBar.GroupNameEaseOutDuration;
							float num3 = 1;
							float num4 = -1;
							num2 /= groupNameEaseOutDuration;
							float num5 = num4 * num2 * num2 + num3;
							groupNameColor = new Color (ColonistBar.GroupNameColor.r, ColonistBar.GroupNameColor.g, ColonistBar.GroupNameColor.b, num5);
							black.a = groupNameColor.a;
						}
					}
					else {
						this.squadNameDisplayTimestamp = 0;
					}
				}
				if (flag) {
					Rect rect = value ? new Rect (348, 20, 225, 36) : new Rect (348, 29, 225, 36);
					if (!this.GroupsBrowsable) {
						rect.set_x (rect.get_x () + 48);
					}
					Text.set_Anchor (5);
					Text.set_Font (1);
					GUI.set_color (black);
					Widgets.Label (new Rect (rect.get_x () + 1, rect.get_y () + 1, rect.get_width (), rect.get_height ()), this.currentGroup.Name);
					if (rect.Contains (Event.get_current ().get_mousePosition ())) {
						GUI.set_color (Color.get_white ());
					}
					else {
						GUI.set_color (groupNameColor);
					}
					Widgets.Label (rect, this.currentGroup.Name);
					if (Widgets.InvisibleButton (rect)) {
						this.drawer.SelectAllActive ();
					}
					Text.set_Anchor (0);
					Text.set_Font (1);
					GUI.set_color (Color.get_white ());
				}
			}
		}

		protected ColonistBarGroup FindNextGroup (int direction)
		{
			if (this.groups.Count == 0) {
				return null;
			}
			if (this.groups.Count == 1) {
				return this.groups [0];
			}
			int num = this.groups.IndexOf (this.currentGroup);
			if (num == -1) {
				return this.groups [0];
			}
			num += direction;
			if (num < 0) {
				num = this.groups.Count - 1;
			}
			else if (num > this.groups.Count - 1) {
				num = 0;
			}
			return this.groups [num];
		}

		protected void Message (string message)
		{
			if (ColonistBar.LoggingEnabled) {
				Log.Message (message);
			}
		}

		public void Reset ()
		{
			this.drawerGameObject = new GameObject ("ColonistBarDrawer");
			this.drawer = this.drawerGameObject.AddComponent<ColonistBarDrawer> ();
			this.barVisible = this.drawer.Visible;
		}

		protected void ResetGroupNameDisplay ()
		{
			if (this.currentGroup != null && this.displayGroupName) {
				this.squadNameDisplayTimestamp = Time.get_time ();
			}
			else {
				this.squadNameDisplayTimestamp = 0;
			}
		}

		public void SelectAllPawns ()
		{
			this.drawer.SelectAllActive ();
		}

		public void SelectNextGroup (int direction)
		{
			this.CurrentGroup = this.FindNextGroup (direction);
		}

		public void UpdateGroups (List<ColonistBarGroup> groups, ColonistBarGroup selected)
		{
			this.Message (string.Concat (new object[] {
				"UpdateGroups(",
				groups.Count,
				", ",
				(selected != null) ? selected.Name : "null",
				")"
			}));
			foreach (ColonistBarGroup current in groups) {
				this.Message ("group = " + ((current != null) ? current.Name : "null"));
			}
			this.Message ("Already selected: " + ((this.currentGroup != null) ? this.currentGroup.Name : "null"));
			if (selected == null && groups.Count > 0) {
				selected = this.FindNextGroup (-1);
				this.Message ("Previous group: " + ((selected != null) ? selected.Name : "null"));
			}
			this.groups.Clear ();
			this.groups.AddRange (groups);
			if (selected == null && groups.Count > 0) {
				selected = groups [0];
			}
			this.CurrentGroup = selected;
		}

		public void UpdateScreenSize (int width, int height)
		{
			this.drawer.SizeCamera (width, height);
		}

		//
		// Events
		//
		public event ColonistBar.SelectedGroupChangedHandler SelectedGroupChanged {
			add {
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler = this.SelectedGroupChanged;
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler2;
				do {
					selectedGroupChangedHandler2 = selectedGroupChangedHandler;
					selectedGroupChangedHandler = Interlocked.CompareExchange<ColonistBar.SelectedGroupChangedHandler> (ref this.SelectedGroupChanged, (ColonistBar.SelectedGroupChangedHandler)Delegate.Combine (selectedGroupChangedHandler2, value), selectedGroupChangedHandler);
				}
				while (selectedGroupChangedHandler != selectedGroupChangedHandler2);
			}
			remove {
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler = this.SelectedGroupChanged;
				ColonistBar.SelectedGroupChangedHandler selectedGroupChangedHandler2;
				do {
					selectedGroupChangedHandler2 = selectedGroupChangedHandler;
					selectedGroupChangedHandler = Interlocked.CompareExchange<ColonistBar.SelectedGroupChangedHandler> (ref this.SelectedGroupChanged, (ColonistBar.SelectedGroupChangedHandler)Delegate.Remove (selectedGroupChangedHandler2, value), selectedGroupChangedHandler);
				}
				while (selectedGroupChangedHandler != selectedGroupChangedHandler2);
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class ColonistBarDrawer : MonoBehaviour
	{
		//
		// Static Fields
		//
		public static Material SlotBackgroundMat = null;

		public static Vector2 HealthOffsetLarge = new Vector2 (52, 6);

		public static Vector2 HealthSizeLarge = new Vector2 (5, 46);

		public static Vector2 SlotSizeSmall = new Vector2 (56, 38);

		public static Vector2 NormalSlotPaddingSmall = new Vector2 (12, 24);

		public static Vector2 WideSlotPaddingSmall = new Vector2 (36, 24);

		public static Vector2 BackgroundSizeSmall = new Vector2 (40, 36);

		public static Vector2 MentalHealthSizeLarge = new Vector2 (76, 76);

		public static Vector2 PortraitOffsetLarge = new Vector2 (5, 5);

		public static Vector2 PortraitSizeLarge = new Vector2 (46, 46);

		public static Vector2 BodySizeLarge = new Vector2 (76, 76);

		public static Vector2 BodyOffsetLarge = new Vector2 (-9, -11);

		public static Vector2 HeadSizeLarge = new Vector2 (76, 76);

		public static Vector2 HeadOffsetLarge = new Vector2 (-9, -26);

		public static Vector2 MentalHealthOffsetLarge = new Vector2 (-2, -22);

		public static Vector2 BackgroundOffsetSmall = new Vector2 (7, 0);

		public static Vector2 HealthOffsetSmall = new Vector2 (41, 4);

		public static Vector2 HealthSizeSmall = new Vector2 (3, 28);

		public static Color ColorBroken = new Color (0.65, 0.9, 0.93);

		public static Color ColorPsycho = new Color (0.9, 0.2, 0.5);

		public static Color ColorDead = new Color (0.5, 0.5, 0.5, 1);

		public static Color ColorFrozen = new Color (0.7, 0.7, 0.9, 1);

		public static Color ColorNameUnderlay = new Color (0, 0, 0, 0.6);

		public static Vector2 MentalHealthSizeSmall = new Vector2 (52, 52);

		public static Vector2 PortraitOffsetSmall = new Vector2 (11, 3);

		public static Vector2 PortraitSizeSmall = new Vector2 (28, 28);

		public static Vector2 BodySizeSmall = new Vector2 (50, 50);

		public static Vector2 BodyOffsetSmall = new Vector2 (1, -7);

		public static Vector2 HeadSizeSmall = new Vector2 (50, 50);

		public static Vector2 HeadOffsetSmall = new Vector2 (1, -20);

		public static Vector2 MentalHealthOffsetSmall = new Vector2 (7, -17);

		public static Vector2 BackgroundOffsetLarge = new Vector2 (0, 0);

		public static readonly Texture2D UnhappyTex = ContentFinder<Texture2D>.Get ("Things/Pawn/Effects/Unhappy", true);

		public static readonly Texture2D MentalBreakImminentTex = ContentFinder<Texture2D>.Get ("Things/Pawn/Effects/MentalBreakImminent", true);

		public static Texture2D ToggleButton = ContentFinder<Texture2D>.Get ("EdB/Interface/ColonistBar/ToggleBar", true);

		public static Vector2 StartingPosition = new Vector2 (640, 16);

		public static Vector2 SlotSize;

		public static Vector2 SlotPadding;

		public static Vector2 BackgroundSize;

		public static Material SlotSelectedMatSmall = null;

		public static Material SlotBordersMat = null;

		public static Material SlotSelectedMat = null;

		public static Material SlotBackgroundMatLarge = null;

		public static Material SlotBordersMatLarge = null;

		public static Material SlotSelectedMatLarge = null;

		public static Material SlotBackgroundMatSmall = null;

		public static Material SlotBordersMatSmall = null;

		public static Vector2 BackgroundOffset;

		public static Vector2 HealthOffset;

		public static Vector2 HealthSize;

		public static Vector2 MaxLabelSize;

		public static Vector2 SlotSizeLarge = new Vector2 (62, 56);

		public static Vector2 NormalSlotPaddingLarge = new Vector2 (16, 24);

		public static Vector2 WideSlotPaddingLarge = new Vector2 (36, 24);

		public static Vector2 BackgroundSizeLarge = new Vector2 (64, 64);

		public static Vector2 MentalHealthSize;

		public static Vector2 PortraitOffset;

		public static Vector2 PortraitSize;

		public static Vector2 BodySize;

		public static Vector2 BodyOffset;

		public static Vector2 HeadSize;

		public static Vector2 HeadOffset;

		public static Vector2 MentalHealthOffset;

		//
		// Fields
		//
		protected float doubleClickTime = -1;

		protected Camera camera;

		protected bool visible = true;

		protected Dictionary<Material, Material> deadMaterials = new Dictionary<Material, Material> ();

		protected Dictionary<Material, Material> cryptosleepMaterials = new Dictionary<Material, Material> ();

		protected List<TrackedColonist> slots;

		protected bool smallColonistIcons;

		protected Mesh backgroundMesh;

		protected Mesh bodyMesh;

		protected Mesh headMesh;

		protected SelectorUtility pawnSelector = new SelectorUtility ();

		protected MaterialPropertyBlock cryptosleepPropertyBlock = new MaterialPropertyBlock ();

		protected MaterialPropertyBlock deadPropertyBlock = new MaterialPropertyBlock ();

		//
		// Properties
		//
		public List<TrackedColonist> Slots {
			get {
				return this.slots;
			}
			set {
				this.slots = value;
			}
		}

		public bool SmallColonistIcons {
			get {
				return this.smallColonistIcons;
			}
		}

		public bool Visible {
			get {
				return this.visible;
			}
			set {
				this.visible = value;
			}
		}

		//
		// Constructors
		//
		public ColonistBarDrawer ()
		{
			ColonistBarDrawer.ResetTextures ();
		}

		//
		// Static Methods
		//
		public static void ResetTextures ()
		{
			ColonistBarDrawer.SlotBackgroundMatLarge = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitBackgroundLarge");
			ColonistBarDrawer.SlotBackgroundMatLarge.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.SlotBordersMatLarge = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitBordersLarge");
			ColonistBarDrawer.SlotBordersMatLarge.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.SlotSelectedMatLarge = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitSelectedLarge");
			ColonistBarDrawer.SlotSelectedMatLarge.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.SlotBackgroundMatSmall = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitBackgroundSmall");
			ColonistBarDrawer.SlotBackgroundMatSmall.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.SlotBordersMatSmall = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitBordersSmall");
			ColonistBarDrawer.SlotBordersMatSmall.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.SlotSelectedMatSmall = MaterialPool.MatFrom ("EdB/Interface/ColonistBar/PortraitSelectedSmall");
			ColonistBarDrawer.SlotSelectedMatSmall.get_mainTexture ().set_filterMode (0);
			ColonistBarDrawer.ToggleButton = ContentFinder<Texture2D>.Get ("EdB/Interface/ColonistBar/ToggleBar", true);
		}

		//
		// Methods
		//
		public void Draw ()
		{
			if (this.visible) {
				this.camera.Render ();
			}
		}

		protected void DrawTextureForSlot (TrackedColonist slot, Vector2 position)
		{
			Pawn pawn = slot.Pawn;
			if (Widgets.InvisibleButton (new Rect (position.x, position.y, ColonistBarDrawer.SlotSize.x, ColonistBarDrawer.SlotSize.y))) {
				int button = Event.get_current ().get_button ();
				if (button == 2 && slot.Carrier == null) {
					if (slot.Broken) {
						this.SelectAllNotSane ();
					}
					else if (slot.Controllable) {
						this.SelectAllActive ();
					}
					else {
						this.SelectAllDead ();
					}
				}
				if (button == 0) {
					if (Time.get_time () - this.doubleClickTime < 0.3) {
						if (!pawn.get_Dead ()) {
							Pawn carrier = slot.Carrier;
							if (carrier == null) {
								Find.get_CameraMap ().JumpTo (pawn.get_Position ());
							}
							else {
								Find.get_CameraMap ().JumpTo (carrier.get_Position ());
							}
						}
						else if (slot.Corpse != null) {
							Find.get_CameraMap ().JumpTo (slot.Corpse.get_Position ());
						}
						this.doubleClickTime = -1;
					}
					else {
						if (!pawn.get_Dead ()) {
							if ((Event.get_current ().get_shift () || Event.get_current ().get_control ()) && Find.get_Selector ().IsSelected (pawn)) {
								Find.get_Selector ().Deselect (pawn);
							}
							else if (slot.Carrier == null) {
								if (!Event.get_current ().get_alt ()) {
									this.pawnSelector.SelectThing (pawn, Event.get_current ().get_shift ());
								}
								else if (slot.Broken) {
									this.SelectAllNotSane ();
								}
								else {
									this.SelectAllActive ();
								}
							}
						}
						else {
							if (slot.Corpse == null || slot.Missing) {
								this.doubleClickTime = -1;
								return;
							}
							if (Event.get_current ().get_shift () && Find.get_Selector ().IsSelected (slot.Corpse)) {
								Find.get_Selector ().Deselect (slot.Corpse);
							}
							else if (Event.get_current ().get_alt ()) {
								this.SelectAllDead ();
							}
							else {
								this.pawnSelector.SelectThing (slot.Corpse, Event.get_current ().get_shift ());
							}
						}
						if (!Event.get_current ().get_shift ()) {
							this.doubleClickTime = Time.get_time ();
						}
					}
				}
				else {
					this.doubleClickTime = -1;
				}
				if (button == 1) {
					List<FloatMenuOption> list = new List<FloatMenuOption> ();
					if (slot.Missing || slot.Corpse != null) {
						string text = (!slot.Missing) ? Translator.Translate ("EdB.ColonistBar.RemoveDeadColonist") : Translator.Translate ("EdB.ColonistBar.RemoveMissingColonist");
						list.Add (new FloatMenuOption (text, delegate {
							ColonistTracker.Instance.StopTrackingPawn (slot.Pawn);
						}, 1, null, null));
					}
					list.Add (new FloatMenuOption (Translator.Translate ("EdB.ColonistBar.HideColonistBar"), delegate {
						this.visible = false;
					}, 1, null, null));
					FloatMenu floatMenu = new FloatMenu (list, string.Empty, false, false);
					Find.get_WindowStack ().Add (floatMenu);
				}
			}
			if (Event.get_current ().get_type () != 7) {
				return;
			}
			if (!slot.Dead) {
				if (slot.Incapacitated) {
					GUI.set_color (new Color (0.7843, 0, 0));
				}
				else if ((double)slot.HealthPercent < 0.95) {
					GUI.set_color (new Color (0.7843, 0.7843, 0));
				}
				else {
					GUI.set_color (new Color (0, 0.7843, 0));
				}
				if (slot.Missing) {
					GUI.set_color (new Color (0.4824, 0.4824, 0.4824));
				}
				float num = ColonistBarDrawer.HealthSize.y * slot.HealthPercent;
				GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.HealthOffset.x, position.y + ColonistBarDrawer.HealthOffset.y + ColonistBarDrawer.HealthSize.y - num, ColonistBarDrawer.HealthSize.x, num), BaseContent.WhiteTex);
			}
			Vector2 vector = Text.CalcSize (pawn.get_LabelBaseShort ());
			if (vector.x > ColonistBarDrawer.MaxLabelSize.x) {
				vector.x = ColonistBarDrawer.MaxLabelSize.x;
			}
			vector.x += 4;
			GUI.set_color (ColonistBarDrawer.ColorNameUnderlay);
			GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.SlotSize.x / 2 - vector.x / 2, position.y + ColonistBarDrawer.PortraitSize.y, vector.x, 12), BaseContent.BlackTex);
			Text.set_Font (0);
			GUI.get_skin ().get_label ().set_alignment (1);
			Text.set_Anchor (1);
			Color color = Color.get_white ();
			BrokenStateDef brokenState = slot.BrokenState;
			if (brokenState != null) {
				color = brokenState.nameColor;
			}
			GUI.set_color (color);
			Widgets.Label (new Rect (position.x + ColonistBarDrawer.SlotSize.x / 2 - vector.x / 2, position.y + ColonistBarDrawer.PortraitSize.y - 2, vector.x, 20), pawn.get_LabelBaseShort ());
			if (slot.Drafted) {
				vector.x -= 4;
				GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.SlotSize.x / 2 - vector.x / 2, position.y + ColonistBarDrawer.PortraitSize.y + 11, vector.x, 1), BaseContent.WhiteTex);
			}
			Text.set_Anchor (0);
			string text2 = null;
			if (slot.Missing) {
				text2 = Translator.Translate ("EdB.ColonistBar.Status.MISSING");
			}
			else if (slot.Corpse != null) {
				text2 = Translator.Translate ("EdB.ColonistBar.Status.DEAD");
			}
			else if (slot.Captured) {
				text2 = Translator.Translate ("EdB.ColonistBar.Status.KIDNAPPED");
			}
			else if (slot.Cryptosleep) {
				text2 = Translator.Translate ("EdB.ColonistBar.Status.CRYPTOSLEEP");
			}
			else if (brokenState != null) {
				if (brokenState == BrokenStateDefOf.Berserk) {
					text2 = Translator.Translate ("EdB.ColonistBar.Status.RAMPAGE");
				}
				else if (brokenState.defName.Contains ("Binging")) {
					text2 = Translator.Translate ("EdB.ColonistBar.Status.BINGING");
				}
				else {
					text2 = Translator.Translate ("EdB.ColonistBar.Status.BROKEN");
				}
			}
			if (text2 != null) {
				Vector2 vector2 = Text.CalcSize (text2);
				vector2.x += 4;
				GUI.set_color (new Color (0, 0, 0, 0.4));
				GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.SlotSize.x / 2 - vector2.x / 2, position.y + ColonistBarDrawer.PortraitSize.y + 12, vector2.x, 13), BaseContent.BlackTex);
				Text.set_Font (0);
				GUI.get_skin ().get_label ().set_alignment (1);
				Text.set_Anchor (1);
				GUI.set_color (color);
				Widgets.Label (new Rect (position.x + ColonistBarDrawer.SlotSize.x / 2 - vector2.x / 2, position.y + ColonistBarDrawer.PortraitSize.y + 10, vector2.x, 20), text2);
				Text.set_Anchor (0);
			}
			GUI.set_color (new Color (1, 1, 1));
			if (!slot.Cryptosleep) {
				if (slot.MentalBreakWarningLevel == 2 && (double)Time.get_time () % 1.2 < 0.4) {
					GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.PortraitOffset.x, position.y + ColonistBarDrawer.PortraitOffset.y, ColonistBarDrawer.MentalHealthSize.x, ColonistBarDrawer.MentalHealthSize.y), ColonistBarDrawer.MentalBreakImminentTex);
				}
				else if (slot.MentalBreakWarningLevel == 1 && (double)Time.get_time () % 1.2 < 0.4) {
					GUI.DrawTexture (new Rect (position.x + ColonistBarDrawer.MentalHealthOffset.x, position.y + ColonistBarDrawer.MentalHealthOffset.y, ColonistBarDrawer.MentalHealthSize.x, ColonistBarDrawer.MentalHealthSize.y), ColonistBarDrawer.UnhappyTex);
				}
			}
		}

		public void DrawTexturesForSlots ()
		{
			if (!this.visible || this.slots == null || this.slots.Count == 0) {
				return;
			}
			Vector2 startingPosition = ColonistBarDrawer.StartingPosition;
			float num = (float)Screen.get_width ();
			foreach (TrackedColonist current in this.slots) {
				this.DrawTextureForSlot (current, startingPosition);
				startingPosition.x += ColonistBarDrawer.SlotSize.x + ColonistBarDrawer.SlotPadding.x;
				if (startingPosition.x + ColonistBarDrawer.SlotSize.x + ColonistBarDrawer.SlotPadding.x > num) {
					startingPosition.y += ColonistBarDrawer.SlotSize.y + ColonistBarDrawer.SlotPadding.y;
					startingPosition.x = ColonistBarDrawer.StartingPosition.x;
				}
			}
		}

		public void DrawToggleButton ()
		{
			if (this.visible) {
				return;
			}
			Rect rect = new Rect ((float)(Screen.get_width () - ColonistBarDrawer.ToggleButton.get_width () - 16), ColonistBarDrawer.StartingPosition.y + 4, (float)ColonistBarDrawer.ToggleButton.get_width (), (float)ColonistBarDrawer.ToggleButton.get_height ());
			GUI.DrawTexture (rect, ColonistBarDrawer.ToggleButton);
			if (Widgets.InvisibleButton (rect)) {
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickTiny);
				this.visible = true;
			}
		}

		protected Material GetDeadMaterial (Material material)
		{
			Material material2;
			if (!this.deadMaterials.TryGetValue (material, out material2)) {
				material2 = new Material (material);
				this.deadMaterials [material] = material2;
			}
			return material2;
		}

		protected Material GetFrozenMaterial (Material material)
		{
			Material material2;
			if (!this.cryptosleepMaterials.TryGetValue (material, out material2)) {
				material2 = new Material (material);
				this.cryptosleepMaterials [material] = material2;
			}
			return material2;
		}

		public void OnGUI ()
		{
			if (this.slots == null || this.slots.Count == 0) {
				return;
			}
			Vector2 startingPosition = ColonistBarDrawer.StartingPosition;
			float num = (float)Screen.get_width ();
			foreach (TrackedColonist current in this.slots) {
				this.RenderSlot (current, startingPosition);
				startingPosition.x += ColonistBarDrawer.SlotSize.x + ColonistBarDrawer.SlotPadding.x;
				if (startingPosition.x + ColonistBarDrawer.SlotSize.x + ColonistBarDrawer.SlotPadding.x > num) {
					startingPosition.y += ColonistBarDrawer.SlotSize.y + ColonistBarDrawer.SlotPadding.y;
					startingPosition.x = ColonistBarDrawer.StartingPosition.x;
				}
			}
		}

		protected void RenderSlot (TrackedColonist slot, Vector2 position)
		{
			if (Event.get_current ().get_type () != 7) {
				return;
			}
			Rot4 south = Rot4.get_South ();
			Pawn pawn = slot.Pawn;
			PawnGraphicSet graphics = pawn.drawer.renderer.graphics;
			if (!graphics.get_AllResolved ()) {
				graphics.ResolveAllGraphics ();
			}
			bool flag = slot.Dead || slot.Missing;
			bool cryptosleep = slot.Cryptosleep;
			Quaternion identity = Quaternion.get_identity ();
			Vector3 one = Vector3.get_one ();
			Graphics.DrawMesh (this.backgroundMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.BackgroundOffset.x, position.y + ColonistBarDrawer.BackgroundOffset.y, 0), identity, one), ColonistBarDrawer.SlotBackgroundMat, 1, this.camera, 0, null);
			MaterialPropertyBlock materialPropertyBlock = null;
			if (flag) {
				materialPropertyBlock = this.deadPropertyBlock;
			}
			else if (slot.Cryptosleep) {
				materialPropertyBlock = this.cryptosleepPropertyBlock;
			}
			float num = 1;
			Material material;
			foreach (Material current in graphics.MatsBodyBaseAt (south, 0)) {
				material = current;
				if (flag) {
					material = this.GetDeadMaterial (current);
				}
				else if (cryptosleep) {
					material = this.GetFrozenMaterial (current);
				}
				Graphics.DrawMesh (this.bodyMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.PortraitOffset.x, position.y + ColonistBarDrawer.PortraitOffset.y, num), identity, one), material, 1, this.camera, 0, materialPropertyBlock);
				num += 1;
			}
			Material material2;
			for (int i = 0; i < graphics.apparelGraphics.Count; i++) {
				ApparelGraphicRecord apparelGraphicRecord = graphics.apparelGraphics [i];
				if (apparelGraphicRecord.sourceApparel.def.apparel.get_LastLayer () == 2) {
					material2 = apparelGraphicRecord.graphic.MatAt (south, null);
					material2 = graphics.flasher.GetDamagedMat (material2);
					material = material2;
					if (flag) {
						material = this.GetDeadMaterial (material2);
					}
					else if (cryptosleep) {
						material = this.GetFrozenMaterial (material2);
					}
					Graphics.DrawMesh (this.bodyMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.PortraitOffset.x, position.y + ColonistBarDrawer.PortraitOffset.y, num), identity, one), material, 1, this.camera, 0, materialPropertyBlock);
					num += 1;
				}
			}
			Graphics.DrawMesh (this.backgroundMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.BackgroundOffset.x, position.y + ColonistBarDrawer.BackgroundOffset.y, num), identity, one), ColonistBarDrawer.SlotBordersMat, 1, this.camera);
			num += 1;
			if (Find.get_Selector ().IsSelected ((slot.Corpse != null) ? slot.Corpse : pawn)) {
				Graphics.DrawMesh (this.backgroundMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.BackgroundOffset.x, position.y + ColonistBarDrawer.BackgroundOffset.y, num), identity, one), ColonistBarDrawer.SlotSelectedMat, 1, this.camera);
				num += 1;
			}
			material2 = pawn.drawer.renderer.graphics.HeadMatAt (south, 0);
			material = material2;
			if (flag) {
				material = this.GetDeadMaterial (material2);
			}
			else if (cryptosleep) {
				material = this.GetFrozenMaterial (material2);
			}
			Graphics.DrawMesh (this.headMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.HeadOffset.x, position.y + ColonistBarDrawer.HeadOffset.y, num), identity, one), material, 1, this.camera, 0, materialPropertyBlock);
			num += 1;
			bool flag2 = false;
			List<ApparelGraphicRecord> apparelGraphics = graphics.apparelGraphics;
			for (int j = 0; j < apparelGraphics.Count; j++) {
				if (apparelGraphics [j].sourceApparel.def.apparel.get_LastLayer () == 4) {
					flag2 = true;
					material2 = apparelGraphics [j].graphic.MatAt (south, null);
					material2 = graphics.flasher.GetDamagedMat (material2);
					material = material2;
					if (flag) {
						material = this.GetDeadMaterial (material2);
					}
					else if (cryptosleep) {
						material = this.GetFrozenMaterial (material2);
					}
					Graphics.DrawMesh (this.headMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.HeadOffset.x, position.y + ColonistBarDrawer.HeadOffset.y, num), identity, one), material, 1, this.camera, 0, materialPropertyBlock);
					num += 1;
				}
			}
			if (!flag2 && slot.Pawn.story.hairDef != null) {
				material2 = graphics.HairMatAt (south);
				material = material2;
				if (flag) {
					material = this.GetDeadMaterial (material2);
				}
				else if (cryptosleep) {
					material = this.GetFrozenMaterial (material2);
				}
				Graphics.DrawMesh (this.headMesh, Matrix4x4.TRS (new Vector3 (position.x + ColonistBarDrawer.HeadOffset.x, position.y + ColonistBarDrawer.HeadOffset.y, num), identity, one), material, 1, this.camera, 0, materialPropertyBlock);
				num += 1;
			}
		}

		protected void ResetMaxLabelSize ()
		{
			float num = ColonistBarDrawer.SlotSize.x + ColonistBarDrawer.SlotPadding.x - 8;
			ColonistBarDrawer.MaxLabelSize = new Vector2 (num, 12);
		}

		protected void ResizeMeshes ()
		{
			this.backgroundMesh = new Mesh ();
			this.backgroundMesh.set_vertices (new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (ColonistBarDrawer.BackgroundSize.x, 0, 0),
				new Vector3 (0, ColonistBarDrawer.BackgroundSize.y, 0),
				new Vector3 (ColonistBarDrawer.BackgroundSize.x, ColonistBarDrawer.BackgroundSize.y, 0)
			});
			this.backgroundMesh.set_uv (new Vector2[] {
				new Vector2 (0, 1),
				new Vector2 (1, 1),
				new Vector2 (0, 0),
				new Vector2 (1, 0)
			});
			this.backgroundMesh.set_triangles (new int[] {
				0,
				1,
				2,
				1,
				3,
				2
			});
			this.bodyMesh = new Mesh ();
			this.bodyMesh.set_vertices (new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (ColonistBarDrawer.PortraitSize.x, 0, 0),
				new Vector3 (0, ColonistBarDrawer.PortraitSize.y, 0),
				new Vector3 (ColonistBarDrawer.PortraitSize.x, ColonistBarDrawer.PortraitSize.y, 0)
			});
			Vector2 vector = new Vector2 ((ColonistBarDrawer.PortraitOffset.x - ColonistBarDrawer.BodyOffset.x) / ColonistBarDrawer.BodySize.x, (ColonistBarDrawer.PortraitOffset.y - ColonistBarDrawer.BodyOffset.y) / ColonistBarDrawer.BodySize.y);
			Vector2 vector2 = new Vector2 ((ColonistBarDrawer.PortraitOffset.x - ColonistBarDrawer.BodyOffset.x + ColonistBarDrawer.PortraitSize.x) / ColonistBarDrawer.BodySize.x, (ColonistBarDrawer.PortraitOffset.y - ColonistBarDrawer.BodyOffset.y + ColonistBarDrawer.PortraitSize.y) / ColonistBarDrawer.BodySize.y);
			this.bodyMesh.set_uv (new Vector2[] {
				new Vector2 (vector.x, vector2.y),
				new Vector2 (vector2.x, vector2.y),
				new Vector2 (vector.x, vector.y),
				new Vector2 (vector2.x, vector.y)
			});
			this.bodyMesh.set_triangles (new int[] {
				0,
				1,
				2,
				1,
				3,
				2
			});
			this.headMesh = new Mesh ();
			this.headMesh.set_vertices (new Vector3[] {
				new Vector3 (0, 0, 0),
				new Vector3 (ColonistBarDrawer.HeadSize.x, 0, 0),
				new Vector3 (0, ColonistBarDrawer.HeadSize.y, 0),
				new Vector3 (ColonistBarDrawer.HeadSize.x, ColonistBarDrawer.HeadSize.y, 0)
			});
			this.headMesh.set_uv (new Vector2[] {
				new Vector2 (0, 1),
				new Vector2 (1, 1),
				new Vector2 (0, 0),
				new Vector2 (1, 0)
			});
			this.headMesh.set_triangles (new int[] {
				0,
				1,
				2,
				1,
				3,
				2
			});
		}

		public void SelectAllActive ()
		{
			this.pawnSelector.ClearSelection ();
			foreach (TrackedColonist current in this.slots) {
				if (current.Controllable) {
					this.pawnSelector.AddToSelection (current.Pawn);
				}
			}
		}

		public void SelectAllDead ()
		{
			this.pawnSelector.ClearSelection ();
			foreach (TrackedColonist current in this.slots) {
				if (current.HealthPercent == 0 && !current.Missing && current.Corpse != null) {
					this.pawnSelector.AddToSelection (current.Corpse);
				}
			}
		}

		public void SelectAllNotSane ()
		{
			this.pawnSelector.ClearSelection ();
			foreach (TrackedColonist current in this.slots) {
				if (current.Broken) {
					this.pawnSelector.AddToSelection (current.Pawn);
				}
			}
		}

		public void SizeCamera (int width, int height)
		{
			float num = (float)width * 0.5;
			float num2 = (float)height * 0.5;
			this.camera.set_orthographicSize (num2);
			this.camera.get_transform ().set_position (new Vector3 (num, num2, 100));
			this.camera.get_transform ().LookAt (new Vector3 (num, num2, 0), new Vector3 (0, -1, 0));
			this.camera.set_aspect (num / num2);
		}

		public void Start ()
		{
			this.camera = base.get_gameObject ().AddComponent<Camera> ();
			this.camera.set_orthographic (true);
			this.camera.set_backgroundColor (new Color (0, 0, 0, 0));
			this.SizeCamera (Screen.get_width (), Screen.get_height ());
			this.camera.set_clearFlags (3);
			this.camera.set_nearClipPlane (1);
			this.camera.set_farClipPlane (200);
			this.camera.set_depth (-1);
			this.camera.set_enabled (false);
			this.UseLargeIcons ();
			this.deadPropertyBlock = new MaterialPropertyBlock ();
			this.deadPropertyBlock.Clear ();
			this.deadPropertyBlock.AddColor (Shader.PropertyToID ("_Color"), ColonistBarDrawer.ColorDead);
			this.cryptosleepPropertyBlock = new MaterialPropertyBlock ();
			this.cryptosleepPropertyBlock.Clear ();
			this.cryptosleepPropertyBlock.AddColor (Shader.PropertyToID ("_Color"), ColonistBarDrawer.ColorFrozen);
		}

		public void UseLargeIcons ()
		{
			ColonistBarDrawer.SlotSize = ColonistBarDrawer.SlotSizeLarge;
			ColonistBarDrawer.BackgroundSize = ColonistBarDrawer.BackgroundSizeLarge;
			ColonistBarDrawer.BackgroundOffset = ColonistBarDrawer.BackgroundOffsetLarge;
			ColonistBarDrawer.SlotPadding = ColonistBarDrawer.NormalSlotPaddingLarge;
			ColonistBarDrawer.PortraitOffset = ColonistBarDrawer.PortraitOffsetLarge;
			ColonistBarDrawer.PortraitSize = ColonistBarDrawer.PortraitSizeLarge;
			ColonistBarDrawer.BodySize = ColonistBarDrawer.BodySizeLarge;
			ColonistBarDrawer.BodyOffset = ColonistBarDrawer.BodyOffsetLarge;
			ColonistBarDrawer.HeadSize = ColonistBarDrawer.HeadSizeLarge;
			ColonistBarDrawer.HeadOffset = ColonistBarDrawer.HeadOffsetLarge;
			ColonistBarDrawer.MentalHealthOffset = ColonistBarDrawer.MentalHealthOffsetLarge;
			ColonistBarDrawer.MentalHealthSize = ColonistBarDrawer.MentalHealthSizeLarge;
			ColonistBarDrawer.HealthOffset = ColonistBarDrawer.HealthOffsetLarge;
			ColonistBarDrawer.HealthSize = ColonistBarDrawer.HealthSizeLarge;
			ColonistBarDrawer.SlotBackgroundMat = ColonistBarDrawer.SlotBackgroundMatLarge;
			ColonistBarDrawer.SlotBordersMat = ColonistBarDrawer.SlotBordersMatLarge;
			ColonistBarDrawer.SlotSelectedMat = ColonistBarDrawer.SlotSelectedMatLarge;
			this.smallColonistIcons = false;
			this.ResizeMeshes ();
			this.ResetMaxLabelSize ();
		}

		public void UseSmallIcons ()
		{
			ColonistBarDrawer.SlotSize = ColonistBarDrawer.SlotSizeSmall;
			ColonistBarDrawer.BackgroundSize = ColonistBarDrawer.BackgroundSizeSmall;
			ColonistBarDrawer.BackgroundOffset = ColonistBarDrawer.BackgroundOffsetSmall;
			ColonistBarDrawer.SlotPadding = ColonistBarDrawer.NormalSlotPaddingSmall;
			ColonistBarDrawer.PortraitOffset = ColonistBarDrawer.PortraitOffsetSmall;
			ColonistBarDrawer.PortraitSize = ColonistBarDrawer.PortraitSizeSmall;
			ColonistBarDrawer.BodySize = ColonistBarDrawer.BodySizeSmall;
			ColonistBarDrawer.BodyOffset = ColonistBarDrawer.BodyOffsetSmall;
			ColonistBarDrawer.HeadSize = ColonistBarDrawer.HeadSizeSmall;
			ColonistBarDrawer.HeadOffset = ColonistBarDrawer.HeadOffsetSmall;
			ColonistBarDrawer.MentalHealthOffset = ColonistBarDrawer.MentalHealthOffsetSmall;
			ColonistBarDrawer.MentalHealthSize = ColonistBarDrawer.MentalHealthSizeSmall;
			ColonistBarDrawer.HealthOffset = ColonistBarDrawer.HealthOffsetSmall;
			ColonistBarDrawer.HealthSize = ColonistBarDrawer.HealthSizeSmall;
			ColonistBarDrawer.SlotBackgroundMat = ColonistBarDrawer.SlotBackgroundMatSmall;
			ColonistBarDrawer.SlotBordersMat = ColonistBarDrawer.SlotBordersMatSmall;
			ColonistBarDrawer.SlotSelectedMat = ColonistBarDrawer.SlotSelectedMatSmall;
			this.smallColonistIcons = true;
			this.ResizeMeshes ();
			this.ResetMaxLabelSize ();
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class ColonistBarGroup
	{
		//
		// Fields
		//
		private List<TrackedColonist> colonists;

		private string name;

		private bool visible;

		private string id = string.Empty;

		//
		// Properties
		//
		public List<TrackedColonist> Colonists {
			get {
				return this.colonists;
			}
		}

		public string Id {
			get {
				return this.id;
			}
			set {
				this.id = value;
			}
		}

		public string Name {
			get {
				return this.name;
			}
			set {
				this.name = value;
			}
		}

		public int OrderHash {
			get {
				int num = 33;
				foreach (TrackedColonist current in this.colonists) {
					num = 17 * num + current.Pawn.GetUniqueLoadID ().GetHashCode ();
				}
				num = 17 * num + this.name.GetHashCode ();
				return num;
			}
		}

		public bool Visible {
			get {
				return this.visible;
			}
			set {
				this.visible = value;
			}
		}

		//
		// Constructors
		//
		public ColonistBarGroup (string name, List<TrackedColonist> colonists)
		{
			this.colonists = colonists;
			this.name = name;
		}

		public ColonistBarGroup (int reserve)
		{
			if (reserve > 0) {
				this.colonists = new List<TrackedColonist> (reserve);
			}
			else {
				this.colonists = new List<TrackedColonist> ();
			}
		}

		public ColonistBarGroup ()
		{
			this.colonists = new List<TrackedColonist> ();
		}

		//
		// Methods
		//
		public void Add (TrackedColonist colonist)
		{
			if (!this.colonists.Contains (colonist)) {
				this.colonists.Add (colonist);
			}
		}

		public void Clear ()
		{
			this.colonists.Clear ();
		}

		public bool Remove (TrackedColonist colonist)
		{
			return this.colonists.Remove (colonist);
		}

		public bool Remove (Pawn pawn)
		{
			int num = this.colonists.FindIndex ((TrackedColonist c) => c.Pawn == pawn);
			if (num != -1) {
				this.colonists.RemoveAt (num);
				return true;
			}
			return false;
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ColonistBarSlot
	{
		//
		// Fields
		//
		public Pawn pawn;

		protected bool remove;

		protected SelectorUtility pawnSelector = new SelectorUtility ();

		protected float missingTime;

		public bool drafted;

		public float health;

		public bool kidnapped;

		public Corpse corpse;

		public bool missing;

		public bool incapacitated;

		public bool dead;

		public BrokenStateDef sanity;

		public int psychologyLevel;

		//
		// Properties
		//
		public Corpse Corpse {
			get {
				return this.corpse;
			}
			set {
				this.corpse = value;
			}
		}

		public bool Missing {
			get {
				return this.missing;
			}
			set {
				this.missing = value;
			}
		}

		public float MissingTime {
			get {
				return this.missingTime;
			}
			set {
				this.missingTime = value;
			}
		}

		public Pawn Pawn {
			get {
				return this.pawn;
			}
		}

		public bool Remove {
			get {
				return this.remove;
			}
			set {
				this.remove = value;
			}
		}

		//
		// Constructors
		//
		public ColonistBarSlot (Pawn pawn)
		{
			this.pawn = pawn;
		}

		//
		// Methods
		//
		public Pawn FindCarrier ()
		{
			if (this.pawn.holder != null && this.pawn.holder.owner != null) {
				Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
				if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null) {
					return pawn_CarryTracker.pawn;
				}
			}
			return null;
		}

		public void Update ()
		{
			if (this.pawn == null) {
				return;
			}
			this.incapacitated = false;
			if (this.pawn.health != null) {
				this.health = this.pawn.health.summaryHealth.get_SummaryHealthPercent ();
				this.incapacitated = this.pawn.health.get_Downed ();
			}
			else {
				this.health = 0;
			}
			this.kidnapped = false;
			if (this.pawn.holder != null) {
				if (this.pawn.get_Destroyed ()) {
					this.missing = true;
				}
				else if (this.pawn.holder.owner != null) {
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null && pawn_CarryTracker.pawn.get_Faction () != null && pawn_CarryTracker.pawn.get_Faction () != Faction.get_OfColony () && pawn_CarryTracker.pawn.get_Faction ().RelationWith (Faction.get_OfColony ()).hostile) {
						this.kidnapped = true;
					}
				}
			}
			this.dead = this.pawn.get_Dead ();
			if (this.dead && this.WasReplaced (this.pawn)) {
				this.dead = false;
			}
			this.sanity = null;
			if (this.pawn.mindState != null && this.pawn.mindState.broken != null) {
				this.sanity = this.pawn.mindState.broken.get_CurStateDef ();
			}
			this.drafted = (!this.dead && this.pawn.get_Drafted ());
			this.psychologyLevel = 0;
			if (this.pawn.mindState != null && this.pawn.mindState.breaker != null && !this.pawn.get_Downed () && !this.pawn.get_Dead ()) {
				if (this.pawn.mindState.breaker.get_HardBreakImminent ()) {
					this.psychologyLevel = 2;
				}
				else if (this.pawn.mindState.breaker.get_MentalBreakApproaching ()) {
					this.psychologyLevel = 1;
				}
			}
		}

		protected bool WasReplaced (Pawn pawn)
		{
			foreach (Pawn current in Find.get_ListerPawns ().get_FreeColonists ()) {
				if (current.GetUniqueLoadID () == pawn.GetUniqueLoadID ()) {
					return true;
				}
			}
			return false;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class ColonistBarSquadSupervisor
	{
		public delegate void SelectedSquadChangedHandler (Squad Squad);

		//
		// Fields
		//
		protected List<ColonistBarGroup> scratchGroups = new List<ColonistBarGroup> ();

		private bool enabled = true;

		private ColonistBar colonistBar;

		private Dictionary<Squad, ColonistBarGroup> groupDictionaryScratch = new Dictionary<Squad, ColonistBarGroup> ();

		private Dictionary<ColonistBarGroup, Squad> squadDictionaryScratch = new Dictionary<ColonistBarGroup, Squad> ();

		private List<ColonistBarGroup> colonistBarGroups = new List<ColonistBarGroup> ();

		private Squad selectedSquad;

		private ColonistBarGroup allColonistsGroup;

		private Dictionary<ColonistBarGroup, Squad> squadDictionary = new Dictionary<ColonistBarGroup, Squad> ();

		private Dictionary<Squad, ColonistBarGroup> groupDictionary = new Dictionary<Squad, ColonistBarGroup> ();

		//
		// Properties
		//
		public bool Enabled {
			get {
				return this.enabled;
			}
			set {
				this.enabled = value;
			}
		}

		public Squad SelectedSquad {
			get {
				return this.selectedSquad;
			}
			set {
				bool flag = this.selectedSquad != value;
				this.selectedSquad = value;
				if (flag && this.SelectedSquadChanged != null) {
					this.SelectedSquadChanged (this.selectedSquad);
				}
			}
		}

		//
		// Constructors
		//
		public ColonistBarSquadSupervisor (ColonistBar colonistBar)
		{
			this.colonistBar = colonistBar;
		}

		//
		// Methods
		//
		public bool SaveCurrentSquadAsFavorite (int index)
		{
			return this.selectedSquad != null && SquadManager.Instance.SetFavorite (index, this.selectedSquad);
		}

		public void SelectAllPawnsInFavorite (int index)
		{
			if (SquadManager.Instance.GetFavorite (index) != null) {
				this.colonistBar.SelectAllPawns ();
			}
		}

		public void SelectedGroupChanged (ColonistBarGroup group)
		{
			if (!this.enabled) {
				return;
			}
			if (group != null) {
				Squad allColonistsSquad = SquadManager.Instance.AllColonistsSquad;
				this.squadDictionary.TryGetValue (group, out allColonistsSquad);
				this.SelectedSquad = allColonistsSquad;
			}
			else {
				this.SelectedSquad = null;
			}
		}

		public void SelectFavorite (int index)
		{
			Squad favorite = SquadManager.Instance.GetFavorite (index);
			if (favorite != null && favorite.ShowInColonistBar) {
				ColonistBarGroup colonistBarGroup = this.groupDictionary [favorite];
				if (colonistBarGroup != null) {
					this.colonistBar.CurrentGroup = colonistBarGroup;
				}
			}
		}

		public void SelectNextSquad (int direction)
		{
			this.colonistBar.SelectNextGroup (direction);
		}

		public void SyncSquadsToColonistBar ()
		{
			if (!this.enabled) {
				return;
			}
			SquadManager instance = SquadManager.Instance;
			ColonistTracker instance2 = ColonistTracker.Instance;
			AllColonistsSquad allColonistsSquad = SquadManager.Instance.AllColonistsSquad;
			this.groupDictionaryScratch.Clear ();
			this.squadDictionaryScratch.Clear ();
			this.colonistBarGroups.Clear ();
			int count = instance.Squads.Count;
			for (int i = 0; i < count; i++) {
				Squad squad = instance.Squads [i];
				ColonistBarGroup colonistBarGroup = null;
				if (this.groupDictionary.TryGetValue (squad, out colonistBarGroup) && squad == allColonistsSquad) {
					this.allColonistsGroup = colonistBarGroup;
				}
				if (squad.Pawns.Count > 0 && squad.ShowInColonistBar) {
					bool flag = false;
					if (colonistBarGroup == null) {
						colonistBarGroup = new ColonistBarGroup (squad.Pawns.Count);
						flag = true;
					}
					else if (colonistBarGroup.OrderHash != squad.OrderHash) {
						flag = true;
					}
					if (flag) {
						colonistBarGroup.Clear ();
						colonistBarGroup.Name = squad.Name;
						colonistBarGroup.Id = squad.Id;
						foreach (Pawn current in squad.Pawns) {
							TrackedColonist trackedColonist = instance2.FindTrackedColonist (current);
							if (trackedColonist != null) {
								colonistBarGroup.Add (trackedColonist);
							}
						}
					}
					this.colonistBarGroups.Add (colonistBarGroup);
					this.groupDictionaryScratch [squad] = colonistBarGroup;
					this.squadDictionaryScratch [colonistBarGroup] = squad;
				}
			}
			Dictionary<ColonistBarGroup, Squad> dictionary = this.squadDictionary;
			Dictionary<Squad, ColonistBarGroup> dictionary2 = this.groupDictionary;
			this.groupDictionary = this.groupDictionaryScratch;
			this.squadDictionary = this.squadDictionaryScratch;
			this.groupDictionaryScratch = dictionary2;
			this.squadDictionaryScratch = dictionary;
		}

		public void UpdateColonistBarGroups ()
		{
			if (!this.enabled) {
				return;
			}
			if (this.colonistBarGroups.Count == 0) {
				this.colonistBar.UpdateGroups (this.colonistBarGroups, null);
				return;
			}
			if (this.selectedSquad != null) {
				ColonistBarGroup selected = null;
				if (this.groupDictionary.TryGetValue (this.selectedSquad, out selected)) {
					this.colonistBar.UpdateGroups (this.colonistBarGroups, selected);
					return;
				}
			}
			if (SquadManager.Instance.AllColonistsSquad.ShowInColonistBar) {
				this.colonistBar.UpdateGroups (this.colonistBarGroups, this.allColonistsGroup);
			}
			else {
				this.colonistBar.UpdateGroups (this.colonistBarGroups, null);
			}
		}

		//
		// Events
		//
		public event ColonistBarSquadSupervisor.SelectedSquadChangedHandler SelectedSquadChanged {
			add {
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler = this.SelectedSquadChanged;
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler2;
				do {
					selectedSquadChangedHandler2 = selectedSquadChangedHandler;
					selectedSquadChangedHandler = Interlocked.CompareExchange<ColonistBarSquadSupervisor.SelectedSquadChangedHandler> (ref this.SelectedSquadChanged, (ColonistBarSquadSupervisor.SelectedSquadChangedHandler)Delegate.Combine (selectedSquadChangedHandler2, value), selectedSquadChangedHandler);
				}
				while (selectedSquadChangedHandler != selectedSquadChangedHandler2);
			}
			remove {
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler = this.SelectedSquadChanged;
				ColonistBarSquadSupervisor.SelectedSquadChangedHandler selectedSquadChangedHandler2;
				do {
					selectedSquadChangedHandler2 = selectedSquadChangedHandler;
					selectedSquadChangedHandler = Interlocked.CompareExchange<ColonistBarSquadSupervisor.SelectedSquadChangedHandler> (ref this.SelectedSquadChanged, (ColonistBarSquadSupervisor.SelectedSquadChangedHandler)Delegate.Remove (selectedSquadChangedHandler2, value), selectedSquadChangedHandler);
				}
				while (selectedSquadChangedHandler != selectedSquadChangedHandler2);
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public delegate void ColonistListSyncNeededHandler ();
}
using System;
using Verse;

namespace EdB.Interface
{
	public class ColonistNotification
	{
		//
		// Fields
		//
		public TrackedColonist colonist;

		public ColonistNotificationType type;

		public Pawn relatedPawn;

		//
		// Constructors
		//
		public ColonistNotification (ColonistNotificationType type, TrackedColonist colonist)
		{
			this.type = type;
			this.colonist = colonist;
			this.relatedPawn = null;
		}

		public ColonistNotification (ColonistNotificationType type, TrackedColonist colonist, Pawn relatedPawn)
		{
			this.type = type;
			this.colonist = colonist;
			this.relatedPawn = relatedPawn;
		}

		//
		// Methods
		//
		public override string ToString ()
		{
			NameTriple nameTriple = this.colonist.Pawn.get_Name () as NameTriple;
			return string.Concat (new object[] {
				"ColonistNotification, ",
				this.type,
				": ",
				nameTriple.get_Nick ()
			});
		}
	}
}
using System;

namespace EdB.Interface
{
	public delegate void ColonistNotificationHandler (ColonistNotification notification);
}
using System;

namespace EdB.Interface
{
	public enum ColonistNotificationType
	{
		New,
		Died,
		Captured,
		Missing,
		Replaced,
		Freed,
		Buried,
		Lost,
		Deleted,
		Cryptosleep,
		WokeFromCryptosleep
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ColonistRowDrawer : ListWidgetItemDrawer<TrackedColonist>
	{
		//
		// Fields
		//
		private Vector2 padding = new Vector2 (9, 8);

		private Texture blackTexture;

		private Color deadColor = new Color (0.5, 0.5, 0.5);

		private Color selectedTextColor = new Color (1, 1, 1);

		private Color textColor = new Color (0.85, 0.85, 0.85);

		private Vector2 iconSize = new Vector2 (41, 40);

		private Vector2 iconPadding = new Vector2 (16, 8);

		private Vector2 paddingTotal;

		private List<Texture> rowTextures = new List<Texture> ();

		//
		// Constructors
		//
		public ColonistRowDrawer ()
		{
			this.paddingTotal = new Vector2 (this.padding.x * 2, this.padding.y * 2);
			this.blackTexture = SolidColorMaterials.NewSolidColorTexture (new Color (0.1523, 0.168, 0.1836));
		}

		//
		// Methods
		//
		public void AddRowColor (Color color)
		{
			this.rowTextures.Add (SolidColorMaterials.NewSolidColorTexture (color));
		}

		public Vector2 Draw (int index, TrackedColonist colonist, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string nameStringShort = colonist.Pawn.get_NameStringShort ();
			Text.set_Anchor (3);
			float num = 56;
			Rect rect = new Rect (cursor.x, cursor.y, width, num);
			Rect rect2 = new Rect (cursor.x + this.padding.x, cursor.y + this.padding.y, this.iconSize.x, this.iconSize.y);
			GUI.DrawTexture (rect2, this.blackTexture);
			GUI.set_color (new Color (0.3, 0.3, 0.3));
			Widgets.DrawBox (rect2, 1);
			Rect rect3 = new Rect (rect2.get_x () + this.iconSize.x - 1, rect2.get_y (), 7, this.iconSize.y);
			bool flag = colonist.Dead || colonist.Missing;
			bool cryptosleep = colonist.Cryptosleep;
			if (!flag) {
				if (colonist.Incapacitated) {
					GUI.set_color (new Color (0.7843, 0, 0));
				}
				else if ((double)colonist.HealthPercent < 0.95) {
					GUI.set_color (new Color (0.7843, 0.7843, 0));
				}
				else {
					GUI.set_color (new Color (0, 0.7843, 0));
				}
				if (colonist.Missing) {
					GUI.set_color (new Color (0.4824, 0.4824, 0.4824));
				}
				float num2 = rect3.get_height () * colonist.HealthPercent;
				GUI.DrawTexture (new Rect (rect3.get_x (), rect3.get_y () + rect3.get_height () - num2, rect3.get_width (), num2), BaseContent.WhiteTex);
			}
			GUI.set_color (new Color (0.3, 0.3, 0.3));
			Widgets.DrawBox (rect3, 1);
			Rect rect4 = new Rect (cursor.x + this.padding.x + 1, cursor.y, this.iconSize.x, this.iconSize.y + this.padding.y - 1);
			try {
				GUI.BeginGroup (rect4);
				Vector2 vector = new Vector2 (64, 64);
				Vector2 vector2 = new Vector2 (-12, 5);
				Vector2 vector3 = new Vector2 (-12, -9);
				bool flag2 = true;
				if (flag) {
					GUI.set_color (this.deadColor);
					flag2 = false;
				}
				else if (cryptosleep) {
					GUI.set_color (ColonistBarDrawer.ColorFrozen);
					flag2 = false;
				}
				if (flag2) {
					GUI.set_color (colonist.Pawn.story.skinColor);
				}
				Rect rect5 = new Rect (vector2.x, vector2.y, vector.x, vector.y);
				GUI.DrawTexture (rect5, colonist.Pawn.drawer.renderer.graphics.nakedGraphic.get_MatFront ().get_mainTexture ());
				bool flag3 = false;
				foreach (ApparelGraphicRecord current in colonist.Pawn.drawer.renderer.graphics.apparelGraphics) {
					if (current.sourceApparel.def.apparel.get_LastLayer () != 4) {
						if (flag2) {
							GUI.set_color (current.sourceApparel.get_DrawColor ());
						}
						GUI.DrawTexture (rect5, current.graphic.get_MatFront ().get_mainTexture ());
					}
					else {
						flag3 = true;
					}
				}
				if (flag2) {
					GUI.set_color (colonist.Pawn.story.skinColor);
				}
				Rect rect6 = new Rect (vector3.x, vector3.y, vector.x, vector.y);
				GUI.DrawTexture (rect6, colonist.Pawn.drawer.renderer.graphics.headGraphic.get_MatFront ().get_mainTexture ());
				if (!flag3) {
					if (flag2) {
						GUI.set_color (colonist.Pawn.story.hairColor);
					}
					GUI.DrawTexture (rect6, colonist.Pawn.drawer.renderer.graphics.hairGraphic.get_MatFront ().get_mainTexture ());
				}
				else {
					foreach (ApparelGraphicRecord current2 in colonist.Pawn.drawer.renderer.graphics.apparelGraphics) {
						if (current2.sourceApparel.def.apparel.get_LastLayer () == 4) {
							if (flag2) {
								GUI.set_color (current2.sourceApparel.get_DrawColor ());
							}
							GUI.DrawTexture (rect6, current2.graphic.get_MatFront ().get_mainTexture ());
						}
					}
				}
			}
			finally {
				GUI.EndGroup ();
			}
			GUI.set_color (Color.get_white ());
			try {
				if (selected) {
					GUI.set_color (this.selectedTextColor);
				}
				else {
					GUI.set_color (this.textColor);
				}
				string text = null;
				if (flag) {
					if (colonist.Missing) {
						text = Translator.Translate ("EdB.Squads.Window.SquadMemberStatus.Missing");
					}
					else {
						text = Translator.Translate ("EdB.Squads.Window.SquadMemberStatus.Dead");
					}
				}
				else if (colonist.Cryptosleep) {
					text = Translator.Translate ("EdB.Squads.Window.SquadMemberStatus.Cryptosleep");
				}
				if (text == null) {
					Rect rect7 = new Rect (cursor.x + this.padding.x + this.iconSize.x + this.iconPadding.x, cursor.y + this.padding.y + 2, width - this.paddingTotal.x - this.iconSize.x - this.iconPadding.x, num - this.paddingTotal.y);
					Widgets.Label (rect7, nameStringShort);
				}
				else {
					Rect rect8 = new Rect (cursor.x + this.padding.x + this.iconSize.x + this.iconPadding.x, cursor.y + this.padding.y + 5, width - this.paddingTotal.x - this.iconSize.x - this.iconPadding.x, (num - this.paddingTotal.y) / 2);
					Text.set_Anchor (6);
					Widgets.Label (rect8, nameStringShort);
					rect8.set_y (rect8.get_y () + rect8.get_height () - 3);
					Text.set_Anchor (0);
					Text.set_Font (0);
					Widgets.Label (rect8, text);
				}
			}
			finally {
				Text.set_Anchor (0);
				Text.set_Font (1);
				GUI.set_color (Color.get_white ());
			}
			if (!colonist.Dead && !colonist.Missing) {
				string tooltipText = this.GetTooltipText (colonist);
				TooltipHandler.TipRegion (rect, new TipSignal (tooltipText, tooltipText.GetHashCode ()));
			}
			return new Vector2 (cursor.x, cursor.y + rect.get_height ());
		}

		public float GetHeight (int index, TrackedColonist colonist, Vector2 cursor, float width, bool selected, bool disabled)
		{
			return 56;
		}

		protected string GetTooltipText (TrackedColonist colonist)
		{
			StringBuilder stringBuilder = new StringBuilder ();
			Pawn pawn = colonist.Pawn;
			foreach (SkillRecord current in pawn.skills.skills) {
				stringBuilder.AppendLine (current.def.skillLabel + ": " + current.level);
			}
			List<WorkTags> list = pawn.story.get_DisabledWorkTags ().ToList<WorkTags> ();
			if (list.Count > 0) {
				stringBuilder.AppendLine ();
				stringBuilder.Append (Translator.Translate ("IncapableOf"));
				stringBuilder.AppendLine (": ");
				foreach (WorkTags current2 in list) {
					stringBuilder.Append ("   ");
					stringBuilder.AppendLine (WorkTypeDefsUtility.LabelTranslated (current2));
				}
			}
			if (pawn.story != null && pawn.story.traits != null && pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count > 0) {
				stringBuilder.AppendLine ();
				stringBuilder.Append (Translator.Translate ("Traits"));
				stringBuilder.AppendLine (": ");
				for (int i = 0; i < pawn.story.traits.allTraits.Count; i++) {
					Trait trait = pawn.story.traits.allTraits [i];
					stringBuilder.Append ("   ");
					stringBuilder.AppendLine (trait.get_LabelCap ());
				}
			}
			if (pawn.equipment != null) {
				ThingWithComps primary = pawn.equipment.get_Primary ();
				if (primary != null) {
					stringBuilder.AppendLine ();
					stringBuilder.AppendLine (primary.get_LabelBaseCap ());
				}
			}
			return stringBuilder.ToString ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class ColonistTracker
	{
		//
		// Static Fields
		//
		protected static ColonistTracker instance;

		public static readonly bool LoggingEnabled;

		public static int MaxMissingDuration = 12000;

		//
		// Fields
		//
		protected ThingRequest corpseThingRequest;

		protected List<Pawn> removalList = new List<Pawn> ();

		protected Dictionary<Pawn, TrackedColonist> trackedColonists = new Dictionary<Pawn, TrackedColonist> ();

		protected HashSet<Pawn> colonistsInFaction = new HashSet<Pawn> ();

		protected HashSet<Pawn> pawnsInFaction = new HashSet<Pawn> ();

		//
		// Static Properties
		//
		public static ColonistTracker Instance {
			get {
				if (ColonistTracker.instance == null) {
					ColonistTracker.instance = new ColonistTracker ();
				}
				return ColonistTracker.instance;
			}
		}

		//
		// Properties
		//
		public List<Pawn> SortedPawns {
			get {
				List<Pawn> list = new List<Pawn> (this.trackedColonists.Keys);
				list.Sort (delegate (Pawn a, Pawn b) {
					if (a.playerSettings == null || b.playerSettings == null || (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick == 0)) {
						return b.GetUniqueLoadID ().CompareTo (a.GetUniqueLoadID ());
					}
					if (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick != 0) {
						return -1;
					}
					if (a.playerSettings.joinTick != 0 && b.playerSettings.joinTick == 0) {
						return 1;
					}
					return a.playerSettings.joinTick.CompareTo (b.playerSettings.joinTick);
				});
				return list;
			}
		}

		//
		// Constructors
		//
		protected ColonistTracker ()
		{
			this.corpseThingRequest = default(ThingRequest);
			this.corpseThingRequest.group = 8;
		}

		//
		// Methods
		//
		protected Faction FindCarryingFaction (Pawn pawn, out Pawn carrier)
		{
			ThingContainer holder = pawn.holder;
			if (holder != null) {
				IThingContainerOwner owner = holder.owner;
				if (owner != null) {
					Pawn_CarryTracker pawn_CarryTracker = owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null) {
						Pawn pawn2 = pawn_CarryTracker.pawn;
						if (pawn2 != null) {
							carrier = pawn2;
							if (Find.get_ListerPawns ().get_PawnsHostileToColony ().Contains (pawn2)) {
								this.Message (pawn, "Carried by pawn (" + pawn2.get_NameStringShort () + ") in hostile faction");
								return pawn2.get_Faction ();
							}
							this.Message (pawn, "Carried by pawn (" + pawn2.get_NameStringShort () + ") in non-hostile faction");
							return Faction.get_OfColony ();
						}
					}
				}
			}
			carrier = null;
			return null;
		}

		protected Pawn FindColonist (Pawn pawn)
		{
			foreach (Pawn current in Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ())) {
				if (current.GetUniqueLoadID () == pawn.GetUniqueLoadID ()) {
					return current;
				}
			}
			return null;
		}

		public TrackedColonist FindTrackedColonist (Pawn pawn)
		{
			TrackedColonist result;
			if (this.trackedColonists.TryGetValue (pawn, out result)) {
				return result;
			}
			return null;
		}

		public void InitializeWithDefaultColonists ()
		{
			this.Message ("InitializeWithDefaultColonists()");
			this.trackedColonists.Clear ();
			this.pawnsInFaction.Clear ();
			List<Pawn> list = new List<Pawn> ();
			foreach (Pawn current in Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ())) {
				list.Add (current);
			}
			foreach (Pawn current2 in Find.get_ListerPawns ().get_PawnsHostileToColony ()) {
				if (current2.carrier != null) {
					Pawn pawn = current2.carrier.get_CarriedThing () as Pawn;
					if (pawn != null && pawn.get_Faction () != null && pawn.get_Faction () == Faction.get_OfColony ()) {
						list.Add (pawn);
					}
				}
			}
			foreach (Thing current3 in Find.get_ListerThings ().get_AllThings ()) {
				Corpse corpse = current3 as Corpse;
				if (corpse != null && corpse.innerPawn != null && corpse.innerPawn.get_Faction () == Faction.get_OfColony () && !this.IsBuried (corpse)) {
					list.Add (corpse.innerPawn);
				}
			}
			list.Sort (delegate (Pawn a, Pawn b) {
				if (a.playerSettings == null || b.playerSettings == null || (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick == 0)) {
					return b.GetUniqueLoadID ().CompareTo (a.GetUniqueLoadID ());
				}
				if (a.playerSettings.joinTick == 0 && b.playerSettings.joinTick != 0) {
					return -1;
				}
				if (a.playerSettings.joinTick != 0 && b.playerSettings.joinTick == 0) {
					return 1;
				}
				return a.playerSettings.joinTick.CompareTo (b.playerSettings.joinTick);
			});
			foreach (Pawn current4 in list) {
				this.StartTrackingPawn (current4);
			}
		}

		protected bool IsBuried (Thing thing)
		{
			return thing.holder != null && thing.holder.owner != null && thing.holder.owner is Building_Grave;
		}

		protected void MarkColonistAsBuried (TrackedColonist colonist)
		{
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Buried, colonist));
			}
			this.Message (colonist.Pawn, "Tracked colonist has been buried");
			this.removalList.Add (colonist.Pawn);
		}

		protected void MarkColonistAsCaptured (TrackedColonist colonist, Pawn carrier, Faction capturingFaction)
		{
			if (colonist.CapturingFaction != capturingFaction) {
				colonist.CapturingFaction = capturingFaction;
				if (this.ColonistChanged != null) {
					this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Captured, colonist));
				}
				this.Message (colonist.Pawn, "Colonist has been captured (by " + capturingFaction.name + ")");
			}
		}

		protected void MarkColonistAsDeleted (TrackedColonist colonist)
		{
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Deleted, colonist));
			}
			this.Message (colonist.Pawn, "Tracked colonist has been deleted");
			this.removalList.Add (colonist.Pawn);
		}

		protected void MarkColonistAsEnteredCryptosleep (TrackedColonist colonist)
		{
			colonist.Cryptosleep = true;
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Cryptosleep, colonist));
			}
			this.Message (colonist.Pawn, "Tracked colonist has entered cryptosleep.");
		}

		protected void MarkColonistAsFreed (TrackedColonist colonist)
		{
			colonist.CapturingFaction = null;
			if (!colonist.Pawn.get_Destroyed ()) {
				if (this.ColonistChanged != null) {
					this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Freed, colonist));
				}
				this.Message (colonist.Pawn, "Captured colonist has been freed.");
			}
		}

		protected void MarkColonistAsLost (TrackedColonist colonist)
		{
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Lost, colonist));
			}
			this.Message ("Tracked colonist has been missing for more than " + ColonistTracker.MaxMissingDuration + " ticks");
			this.removalList.Add (colonist.Pawn);
		}

		protected void MarkColonistAsMissing (TrackedColonist colonist)
		{
			if (!colonist.Missing) {
				if (colonist.Captured) {
					this.Message (colonist.Pawn, "Captured colonist has been removed from the map (by " + colonist.CapturingFaction + ")");
				}
				colonist.Missing = true;
				colonist.MissingTimestamp = Find.get_TickManager ().get_TicksGame ();
				if (this.ColonistChanged != null) {
					this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Missing, colonist));
				}
				this.Message (colonist.Pawn, "Tracked colonist is missing (since " + colonist.MissingTimestamp + ")");
			}
		}

		protected void MarkColonistAsWokenFromCryptosleep (TrackedColonist colonist)
		{
			colonist.Cryptosleep = false;
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.WokeFromCryptosleep, colonist));
			}
			this.Message (colonist.Pawn, "Tracked colonist has woken from cryptosleep.");
		}

		private void Message (Pawn pawn, string message)
		{
			NameTriple nameTriple = pawn.get_Name () as NameTriple;
			string str = (nameTriple == null) ? pawn.get_Label () : nameTriple.get_Nick ();
			this.Message (str + ": " + message);
		}

		private void Message (string message)
		{
			if (ColonistTracker.LoggingEnabled) {
				Log.Message (message);
			}
		}

		protected void ReplaceTrackedPawn (TrackedColonist colonist, Pawn replacement)
		{
			this.trackedColonists.Remove (colonist.Pawn);
			colonist.Pawn = replacement;
			this.trackedColonists.Add (colonist.Pawn, colonist);
			this.Message (colonist.Pawn, "Tracked colonist was found.  Pawn was replaced.");
			if (this.ColonistChanged != null) {
				this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Replaced, colonist, replacement));
			}
		}

		public void Reset ()
		{
			this.colonistsInFaction.Clear ();
			this.pawnsInFaction.Clear ();
			this.trackedColonists.Clear ();
			this.removalList.Clear ();
		}

		public void ResolveMissingPawn (Pawn pawn, TrackedColonist colonist)
		{
			if (pawn.get_Dead () || pawn.get_Destroyed ()) {
				this.Message (pawn, "Tracked colonist is dead or destroyed.  Searching for corpse.");
				Corpse corpse = (Corpse)Find.get_ListerThings ().ThingsMatching (this.corpseThingRequest).FirstOrDefault (delegate (Thing thing) {
					Corpse corpse2 = thing as Corpse;
					return corpse2 != null && corpse2.innerPawn == pawn;
				});
				if (corpse != null) {
					if (!colonist.Dead) {
						colonist.Dead = true;
						if (this.ColonistChanged != null) {
							this.ColonistChanged (new ColonistNotification (ColonistNotificationType.Died, colonist));
						}
					}
					colonist.Corpse = corpse;
					this.Message (pawn, "Corpse found.  Colonist is dead.");
					return;
				}
				this.Message ("Corpse not found.");
			}
			Pawn pawn2 = null;
			Faction faction = this.FindCarryingFaction (pawn, out pawn2);
			if (faction == null) {
				Pawn pawn3 = this.FindColonist (pawn);
				if (pawn3 == null) {
					if (!colonist.Missing) {
						this.MarkColonistAsMissing (colonist);
					}
				}
				else {
					this.ReplaceTrackedPawn (colonist, pawn3);
				}
				return;
			}
			if (faction != Faction.get_OfColony ()) {
				colonist.CapturingFaction = faction;
				this.Message (pawn, "Colonist is captured");
				return;
			}
			this.Message (pawn, "Colonist is being rescued");
		}

		protected TrackedColonist StartTrackingPawn (Pawn pawn)
		{
			if (pawn == null || !pawn.get_IsColonist ()) {
				return null;
			}
			TrackedColonist trackedColonist = null;
			if (this.trackedColonists.TryGetValue (pawn, out trackedColonist)) {
				this.Message (pawn, "Already tracking colonist");
				return trackedColonist;
			}
			trackedColonist = new TrackedColonist (pawn);
			if (!this.trackedColonists.ContainsKey (pawn)) {
				this.trackedColonists.Add (pawn, trackedColonist);
				if (this.ColonistChanged != null) {
					this.ColonistChanged (new ColonistNotification (ColonistNotificationType.New, trackedColonist));
				}
				this.Message (pawn, "Tracking new colonist");
			}
			else {
				this.Message (pawn, "Already tracking colonist");
			}
			return trackedColonist;
		}

		public void StartTrackingPawns (IEnumerable<Pawn> pawns)
		{
			this.Message ("StartTrackingPawns(" + pawns.Count<Pawn> () + ")");
			foreach (Pawn current in pawns) {
				this.StartTrackingPawn (current);
			}
			this.SyncColonistLists ();
		}

		public void StopTrackingPawn (Pawn pawn)
		{
			TrackedColonist trackedColonist = this.FindTrackedColonist (pawn);
			if (trackedColonist != null) {
				this.MarkColonistAsDeleted (trackedColonist);
			}
		}

		private void SyncColonistLists ()
		{
			this.pawnsInFaction.Clear ();
			this.colonistsInFaction.Clear ();
			foreach (Pawn current in Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ())) {
				this.pawnsInFaction.Add (current);
				if (current.get_IsColonist ()) {
					this.colonistsInFaction.Add (current);
				}
				if (!this.trackedColonists.ContainsKey (current)) {
					this.StartTrackingPawn (current);
				}
			}
			if (this.colonistsInFaction.Count != this.trackedColonists.Count) {
				this.Message ("Free colonist list count does not match tracked count.  Resolving.");
				foreach (TrackedColonist current2 in this.trackedColonists.Values) {
					Pawn pawn = current2.Pawn;
					if (!this.pawnsInFaction.Contains (pawn)) {
						this.Message (pawn, "Tracked colonist not found in free list.  Resolving.");
						this.ResolveMissingPawn (pawn, current2);
					}
				}
			}
			if (this.ColonistListSyncNeeded != null) {
				this.ColonistListSyncNeeded ();
			}
		}

		public void Update ()
		{
			if (Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ()).Count != this.pawnsInFaction.Count) {
				this.Message ("Free colonist list changed.  Re-syncing");
				this.SyncColonistLists ();
			}
			foreach (KeyValuePair<Pawn, TrackedColonist> current in this.trackedColonists) {
				this.UpdateColonistState (current.Key, current.Value);
			}
			foreach (Pawn current2 in this.removalList) {
				this.trackedColonists.Remove (current2);
				this.Message (current2, "No longer tracking pawn");
			}
			this.removalList.Clear ();
		}

		protected void UpdateColonistState (Pawn pawn, TrackedColonist colonist)
		{
			Faction faction = null;
			bool flag = false;
			Pawn pawn2 = null;
			if (pawn.holder != null) {
				if (pawn.get_Destroyed ()) {
					this.MarkColonistAsMissing (colonist);
				}
				else if (pawn.holder.owner != null) {
					Pawn_CarryTracker pawn_CarryTracker = pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null && pawn_CarryTracker.pawn != null && pawn_CarryTracker.pawn.get_Faction () != null && pawn_CarryTracker.pawn.get_Faction () != Faction.get_OfColony () && pawn_CarryTracker.pawn.get_Faction ().RelationWith (Faction.get_OfColony ()).hostile) {
						pawn2 = pawn_CarryTracker.pawn;
						faction = pawn2.get_Faction ();
					}
					Building_CryptosleepCasket building_CryptosleepCasket = pawn.holder.owner as Building_CryptosleepCasket;
					if (building_CryptosleepCasket != null) {
						flag = true;
						if (!colonist.Cryptosleep) {
							colonist.Cryptosleep = true;
							this.Message (pawn, "Colonist has entered cryptosleep.");
						}
					}
					else {
						colonist.Cryptosleep = false;
						if (colonist.Cryptosleep) {
							colonist.Cryptosleep = false;
							this.Message (pawn, "Colonist has woken from cryptosleep.");
						}
					}
				}
			}
			else {
				faction = null;
				colonist.Cryptosleep = false;
				if (colonist.Captured) {
					this.Message (pawn, "Captured colonist has been freed.");
					this.MarkColonistAsFreed (colonist);
				}
				if (colonist.Cryptosleep) {
					colonist.Cryptosleep = false;
					this.Message (pawn, "Colonist has woken from cryptosleep.");
				}
			}
			if (!colonist.Captured && faction != null) {
				this.MarkColonistAsCaptured (colonist, pawn2, faction);
			}
			else if (colonist.Captured && faction == null) {
				this.MarkColonistAsFreed (colonist);
			}
			else if (colonist.Captured && faction != colonist.CapturingFaction) {
				this.MarkColonistAsCaptured (colonist, pawn2, faction);
			}
			if (flag && !colonist.Cryptosleep) {
				this.MarkColonistAsEnteredCryptosleep (colonist);
			}
			else if (!flag && colonist.Cryptosleep) {
				this.MarkColonistAsWokenFromCryptosleep (colonist);
			}
			int ticksGame = Find.get_TickManager ().get_TicksGame ();
			if (colonist.Dead && !colonist.Missing) {
				if (colonist.Corpse != null) {
					if (colonist.Corpse.get_Destroyed ()) {
						this.MarkColonistAsMissing (colonist);
					}
					else if (this.IsBuried (colonist.Corpse)) {
						this.MarkColonistAsBuried (colonist);
					}
				}
			}
			else if (colonist.Missing) {
				int num = ticksGame - colonist.MissingTimestamp;
				if (num > ColonistTracker.MaxMissingDuration) {
					this.MarkColonistAsLost (colonist);
				}
			}
		}

		//
		// Events
		//
		public event ColonistNotificationHandler ColonistChanged {
			add {
				ColonistNotificationHandler colonistNotificationHandler = this.ColonistChanged;
				ColonistNotificationHandler colonistNotificationHandler2;
				do {
					colonistNotificationHandler2 = colonistNotificationHandler;
					colonistNotificationHandler = Interlocked.CompareExchange<ColonistNotificationHandler> (ref this.ColonistChanged, (ColonistNotificationHandler)Delegate.Combine (colonistNotificationHandler2, value), colonistNotificationHandler);
				}
				while (colonistNotificationHandler != colonistNotificationHandler2);
			}
			remove {
				ColonistNotificationHandler colonistNotificationHandler = this.ColonistChanged;
				ColonistNotificationHandler colonistNotificationHandler2;
				do {
					colonistNotificationHandler2 = colonistNotificationHandler;
					colonistNotificationHandler = Interlocked.CompareExchange<ColonistNotificationHandler> (ref this.ColonistChanged, (ColonistNotificationHandler)Delegate.Remove (colonistNotificationHandler2, value), colonistNotificationHandler);
				}
				while (colonistNotificationHandler != colonistNotificationHandler2);
			}
		}

		public event ColonistListSyncNeededHandler ColonistListSyncNeeded {
			add {
				ColonistListSyncNeededHandler colonistListSyncNeededHandler = this.ColonistListSyncNeeded;
				ColonistListSyncNeededHandler colonistListSyncNeededHandler2;
				do {
					colonistListSyncNeededHandler2 = colonistListSyncNeededHandler;
					colonistListSyncNeededHandler = Interlocked.CompareExchange<ColonistListSyncNeededHandler> (ref this.ColonistListSyncNeeded, (ColonistListSyncNeededHandler)Delegate.Combine (colonistListSyncNeededHandler2, value), colonistListSyncNeededHandler);
				}
				while (colonistListSyncNeededHandler != colonistListSyncNeededHandler2);
			}
			remove {
				ColonistListSyncNeededHandler colonistListSyncNeededHandler = this.ColonistListSyncNeeded;
				ColonistListSyncNeededHandler colonistListSyncNeededHandler2;
				do {
					colonistListSyncNeededHandler2 = colonistListSyncNeededHandler;
					colonistListSyncNeededHandler = Interlocked.CompareExchange<ColonistListSyncNeededHandler> (ref this.ColonistListSyncNeeded, (ColonistListSyncNeededHandler)Delegate.Remove (colonistListSyncNeededHandler2, value), colonistListSyncNeededHandler);
				}
				while (colonistListSyncNeededHandler != colonistListSyncNeededHandler2);
			}
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class ColonyInfoComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "ColonyInfo";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			Find.get_ColonyInfo ().ColonyInfoOnGUI ();
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentAlternateMaterialSelection : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceRightClickOnly preference = new PreferenceRightClickOnly ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentAlternateMaterialSelection ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			GizmoGridDrawer.RightClickMaterialPreference = this.preference;
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentAlternateTimeDisplay : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceMinuteInterval preferenceMinuteInterval = new PreferenceMinuteInterval ();

		protected PreferenceAmPm preferenceAmPm = new PreferenceAmPm ();

		protected PreferenceEnableAlternateTimeDisplay preferenceEnableAlternateTimeDisplay = new PreferenceEnableAlternateTimeDisplay ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentAlternateTimeDisplay ()
		{
			this.preferences.Add (this.preferenceEnableAlternateTimeDisplay);
			this.preferences.Add (this.preferenceMinuteInterval);
			this.preferences.Add (this.preferenceAmPm);
			this.preferenceAmPm.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
			this.preferenceMinuteInterval.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			userInterface.globalControls.PreferenceAmPm = this.preferenceAmPm;
			userInterface.globalControls.PreferenceEnableAlternateTimeDisplay = this.preferenceEnableAlternateTimeDisplay;
			userInterface.globalControls.PreferenceMinuteInterval = this.preferenceMinuteInterval;
			DateReadout.Reinit ();
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentColonistBar : IRenderedComponent, IInitializedComponent, INamedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		private ColonistBar colonistBar;

		private ColonistBarGroup defaultGroup = new ColonistBarGroup ();

		private List<ColonistBarGroup> defaultGroups = new List<ColonistBarGroup> ();

		//
		// Properties
		//
		public ColonistBar ColonistBar {
			get {
				return this.colonistBar;
			}
		}

		public ColonistBarGroup DefaultGroup {
			get {
				return this.defaultGroup;
			}
		}

		public List<ColonistBarGroup> DefaultGroups {
			get {
				return this.defaultGroups;
			}
		}

		public string Name {
			get {
				return "ColonistBar";
			}
		}

		public IEnumerable<IPreference> Preferences {
			get {
				return this.colonistBar.Preferences;
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Constructors
		//
		public ComponentColonistBar ()
		{
			this.defaultGroups.Add (this.defaultGroup);
			this.colonistBar = new ColonistBar ();
			this.colonistBar.AddGroup (this.defaultGroup);
			this.colonistBar.CurrentGroup = this.defaultGroup;
		}

		//
		// Methods
		//
		public void ColonistNotificationHandler (ColonistNotification notification)
		{
			if (notification.type == ColonistNotificationType.New) {
				this.defaultGroup.Add (notification.colonist);
			}
			else if (notification.type == ColonistNotificationType.Buried || notification.type == ColonistNotificationType.Lost || notification.type == ColonistNotificationType.Deleted) {
				this.defaultGroup.Remove (notification.colonist);
			}
		}

		public void Initialize (UserInterface userInterface)
		{
		}

		public void OnGUI ()
		{
			this.colonistBar.Draw ();
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			userInterface.ScreenSizeMonitor.Changed += new ScreenSizeMonitor.ScreenSizeChangeHandler (this.colonistBar.UpdateScreenSize);
			ColonistTracker.Instance.ColonistChanged += new ColonistNotificationHandler (this.ColonistNotificationHandler);
		}

		public void ResetTextures ()
		{
			ColonistBar.ResetTextures ();
			ColonistBarDrawer.ResetTextures ();
		}
	}
}
using System;

namespace EdB.Interface
{
	public class ComponentColonistTracker : IUpdatedComponent, IInitializedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "ColonistTracker";
			}
		}

		//
		// Constructors
		//
		public ComponentColonistTracker ()
		{
			ColonistTracker.Instance.Reset ();
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			ColonistTracker.Instance.InitializeWithDefaultColonists ();
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
		}

		public void Update ()
		{
			ColonistTracker.Instance.Update ();
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentColorCodedWorkPassions : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceColorCodedWorkPassions preference = new PreferenceColorCodedWorkPassions ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentColorCodedWorkPassions ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			WidgetsWork.PreferenceColorCodedWorkPassions = this.preference;
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentEmptyStockpile : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceEmptyStockpile preference = new PreferenceEmptyStockpile ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentEmptyStockpile ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			foreach (MainTabDef current in mainTabsRoot.AllTabs) {
				MainTabWindow window = current.get_Window ();
				if (window != null) {
					MainTabWindow_Architect mainTabWindow_Architect = window as MainTabWindow_Architect;
					if (mainTabWindow_Architect != null) {
						Designator_ZoneAddStockpile_Resources designator_ZoneAddStockpile_Resources = new Designator_ZoneAddStockpile_Resources ();
						designator_ZoneAddStockpile_Resources.EmptyZonePreference = this.preference;
						mainTabWindow_Architect.ReplaceDesignator (typeof(Designator_ZoneAddStockpile_Resources), designator_ZoneAddStockpile_Resources);
					}
				}
			}
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentHideMainTabs : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceHideMainTabs preference = new PreferenceHideMainTabs ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentHideMainTabs ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			this.UpdateMainTabVisibility (userInterface.MainTabsRoot, this.preference.SelectedOptions);
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preference.ValueChanged += delegate (IEnumerable<string> selectedOptions) {
				this.UpdateMainTabVisibility (mainTabsRoot, selectedOptions);
			};
		}

		public void UpdateMainTabVisibility (MainTabsRoot mainTabsRoot, IEnumerable<string> hiddenTabs)
		{
			HashSet<string> hashSet = new HashSet<string> ();
			foreach (string current in hiddenTabs) {
				hashSet.Add (current);
			}
			foreach (MainTabDef current2 in mainTabsRoot.AllTabs) {
				if (!this.preference.IsTabExcluded (current2.defName)) {
					if (current2.get_Window () != null) {
						if (hashSet.Contains (current2.defName)) {
							current2.showTabButton = false;
						}
						else {
							current2.showTabButton = true;
						}
					}
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentInventory : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		private InventoryManager inventoryManager;

		protected PreferenceIncludeUnfinished preferenceIncludeUnfinished = new PreferenceIncludeUnfinished ();

		protected PreferenceCompressedStorage preferenceCompressedStorage = new PreferenceCompressedStorage ();

		protected List<IPreference> preferences = new List<IPreference> ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentInventory ()
		{
			this.preferences.Add (this.preferenceCompressedStorage);
			this.preferences.Add (this.preferenceIncludeUnfinished);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			this.inventoryManager = new InventoryManager ();
			this.inventoryManager.PreferenceCompressedStorage = this.preferenceCompressedStorage;
			this.inventoryManager.PreferenceIncludeUnfinished = this.preferenceIncludeUnfinished;
			MainTabDef mainTabDef = userInterface.MainTabsRoot.FindTabDef ("EdB_Interface_Inventory");
			MainTabWindow_Inventory mainTabWindow_Inventory = mainTabDef.get_Window () as MainTabWindow_Inventory;
			if (mainTabWindow_Inventory != null) {
				mainTabWindow_Inventory.InventoryManager = this.inventoryManager;
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentMainTabCloseButton : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferenceShowCloseButton preference = new PreferenceShowCloseButton ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentMainTabCloseButton ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			this.UpdateButtonState (userInterface.MainTabsRoot, this.preference.Value);
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preference.ValueChanged += delegate (bool value) {
				this.UpdateButtonState (mainTabsRoot, value);
			};
		}

		public void UpdateButtonState (MainTabsRoot mainTabsRoot, bool value)
		{
			foreach (MainTabDef current in mainTabsRoot.AllTabs) {
				if (current.showTabButton) {
					MainTabWindow window = current.get_Window ();
					if (window != null) {
						window.doCloseX = value;
					}
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class ComponentPauseOnStart : IInitializedComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected PreferencePauseOnStart preference = new PreferencePauseOnStart ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentPauseOnStart ()
		{
			this.preferences.Add (this.preference);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			if (this.preference.Value) {
				try {
					if (MapInitData.startedFromEntry) {
						Find.get_TickManager ().TogglePaused ();
					}
					else if (!Prefs.get_PauseOnLoad ()) {
						Find.get_TickManager ().TogglePaused ();
					}
				}
				catch (Exception ex) {
					Log.Error ("Failed to pause game on start as specified by your EdB interface preferences");
					throw ex;
				}
			}
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class ComponentSquadManager : IUpdatedComponent, IInitializedComponent, INamedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		private SquadShortcuts shortcuts = new SquadShortcuts ();

		private ColonistBarSquadSupervisor supervisor;

		protected Action initializeAction;

		protected ColonistBar colonistBar;

		protected PreferenceAlwaysShowSquadName preferenceAlwaysShowSquadName = new PreferenceAlwaysShowSquadName ();

		protected PreferenceEnableSquadRow preferenceEnableSquadRow = new PreferenceEnableSquadRow ();

		protected PreferenceEnableSquadFiltering preferenceEnableSquadFiltering = new PreferenceEnableSquadFiltering ();

		protected PreferenceEnableSquads preferenceEnableSquads = new PreferenceEnableSquads ();

		//
		// Properties
		//
		public string Name {
			get {
				return "SquadManager";
			}
		}

		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentSquadManager ()
		{
			SquadManager.Instance.Reset ();
			SquadManagerThing.Clear ();
			this.preferences.Add (this.preferenceEnableSquads);
			this.preferences.Add (this.preferenceEnableSquadFiltering);
			this.preferences.Add (this.preferenceEnableSquadRow);
			this.preferences.Add (this.preferenceAlwaysShowSquadName);
		}

		//
		// Methods
		//
		public void Initialize (UserInterface userInterface)
		{
			if (SquadManager.Instance.SyncWithMap ()) {
				if (SquadManager.Instance.Squads.Count == 0) {
					SquadManager.Instance.Squads.Add (SquadManager.Instance.AllColonistsSquad);
				}
				this.supervisor.SelectedSquad = SquadManager.Instance.CurrentSquad;
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			}
			ColonistTracker.Instance.Update ();
			this.initializeAction.Invoke ();
			SquadManager.Instance.SyncThingToMap ();
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			MainTabsRoot mainTabsRoot = userInterface.MainTabsRoot;
			this.preferenceEnableSquadFiltering.PreferenceEnableSquads = this.preferenceEnableSquads;
			this.preferenceEnableSquadRow.PreferenceEnableSquads = this.preferenceEnableSquads;
			SquadManager.Instance.PreferenceEnableSquads = this.preferenceEnableSquads;
			ColonistTracker.Instance.ColonistChanged += new ColonistNotificationHandler (SquadManager.Instance.ColonistChanged);
			ComponentColonistBar colonistBarComponent = userInterface.FindNamedComponentAs<ComponentColonistBar> ("ColonistBar");
			if (colonistBarComponent == null) {
				return;
			}
			colonistBarComponent.ColonistBar.AlwaysShowGroupName = this.preferenceAlwaysShowSquadName.Value;
			this.preferenceAlwaysShowSquadName.ValueChanged += delegate (bool value) {
				colonistBarComponent.ColonistBar.AlwaysShowGroupName = value;
			};
			MainTabDef squadsWindowTabDef = mainTabsRoot.FindTabDef ("EdB_Interface_Squads");
			MainTabWindow_Squads mainTabWindow_Squads = squadsWindowTabDef.get_Window () as MainTabWindow_Squads;
			if (squadsWindowTabDef != null) {
				this.preferenceEnableSquads.ValueChanged += delegate (bool value) {
					this.SquadsEnabledValueChanged (value, squadsWindowTabDef, colonistBarComponent);
				};
			}
			this.supervisor = new ColonistBarSquadSupervisor (colonistBarComponent.ColonistBar);
			SquadManager.Instance.SquadAdded += delegate (Squad Squad) {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			};
			SquadManager.Instance.SquadChanged += delegate (Squad Squad) {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			};
			SquadManager.Instance.SquadRemoved += delegate (Squad s, int i) {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			};
			SquadManager.Instance.SquadDisplayPreferenceChanged += delegate (Squad Squad) {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			};
			SquadManager.Instance.SquadOrderChanged += delegate {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			};
			colonistBarComponent.ColonistBar.SelectedGroupChanged += new ColonistBar.SelectedGroupChangedHandler (this.supervisor.SelectedGroupChanged);
			this.supervisor.SelectedSquadChanged += delegate (Squad squad) {
				SquadManager.Instance.CurrentSquad = squad;
			};
			if (mainTabWindow_Squads != null) {
				SquadManager.Instance.SquadChanged += new SquadNotificationHandler (mainTabWindow_Squads.SquadChanged);
			}
			this.initializeAction = delegate {
				this.SquadsEnabledValueChanged (this.preferenceEnableSquads.Value, squadsWindowTabDef, colonistBarComponent);
			};
			foreach (MainTabWindow_PawnListWithSquads current in mainTabsRoot.FindWindows<MainTabWindow_PawnListWithSquads> ()) {
				current.PreferenceEnableSquadFiltering = this.preferenceEnableSquadFiltering;
				current.PreferenceEnableSquadRow = this.preferenceEnableSquadRow;
				current.PreferenceEnableSquads = this.preferenceEnableSquads;
			}
			this.shortcuts.ColonistBarSquadSupervisor = this.supervisor;
		}

		public void ResetTextures ()
		{
			MainTabWindow_Squads.ResetTextures ();
		}

		public void SquadsEnabledValueChanged (bool value, MainTabDef squadsWindowTabDef, ComponentColonistBar colonistBarComponent)
		{
			this.supervisor.Enabled = value;
			squadsWindowTabDef.showTabButton = value;
			if (value) {
				this.supervisor.SyncSquadsToColonistBar ();
				this.supervisor.UpdateColonistBarGroups ();
			}
			else {
				colonistBarComponent.ColonistBar.UpdateGroups (colonistBarComponent.DefaultGroups, colonistBarComponent.DefaultGroup);
			}
			SquadManager.Instance.SyncThingToMap ();
		}

		public void Update ()
		{
			if (this.preferenceEnableSquads.Value) {
				this.shortcuts.Update ();
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class ComponentTabReplacement : IUpdatedComponent, IInitializedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		//
		// Fields
		//
		protected List<IPreference> preferences = new List<IPreference> ();

		protected ITab_Pawn_Health_Vanilla tabHealthVanilla;

		protected ITab_Pawn_Health_Alternate tabHealth;

		protected ITab_Pawn_Needs_Vanilla tabNeedsVanilla;

		protected ITab_Pawn_Needs_Alternate tabNeeds;

		protected ITab_Pawn_Gear_Vanilla tabGearVanilla;

		protected ITab_Pawn_Gear_Alternate tabGear;

		protected ITab_Pawn_Training_Alternate tabTraining;

		protected ReplacementTabs replacementTabs = new ReplacementTabs ();

		protected ITab_Art_Alternate tabArt;

		protected ITab_Pawn_Prisoner_Vanilla tabPrisonerVanilla;

		protected ITab_Pawn_Prisoner_Alternate tabPrisoner;

		protected ITab_Pawn_Guest_Vanilla tabGuestVanilla;

		protected ITab_Pawn_Guest_Alternate tabGuest;

		protected ITab_Pawn_Training_Vanilla tabTrainingVanilla;

		protected ITab_Pawn_Character_Vanilla tabCharacterVanilla;

		protected PreferenceTabGuestAndPrisoner preferenceTabGuestAndPrisoner = new PreferenceTabGuestAndPrisoner ();

		protected PreferenceTabHealth preferenceTabHealth = new PreferenceTabHealth ();

		protected PreferenceTabNeeds preferenceTabNeeds = new PreferenceTabNeeds ();

		protected PreferenceTabGear preferenceTabGear = new PreferenceTabGear ();

		protected PreferenceTabCharacter preferenceTabCharacter = new PreferenceTabCharacter ();

		protected PreferenceTabBrowseButtons preferenceTabBrowseButtons = new PreferenceTabBrowseButtons ();

		protected PreferenceEnableTabReplacement preferenceEnableTabReplacement = new PreferenceEnableTabReplacement ();

		protected PreferenceTabTraining preferenceTabTraining = new PreferenceTabTraining ();

		protected ITab_Pawn_Character_Alternate tabCharacter;

		protected ITab_Growing_Alternate tabGrowing;

		protected ITab_Bills_Alternate tabBills;

		protected bool dirtyFlag;

		protected PreferenceTabArt preferenceTabArt = new PreferenceTabArt ();

		protected PreferenceTabBills preferenceTabBills = new PreferenceTabBills ();

		protected PreferenceTabGrowing preferenceTabGrowing = new PreferenceTabGrowing ();

		//
		// Properties
		//
		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public ComponentTabReplacement ()
		{
			this.preferences.Add (this.preferenceTabBrowseButtons);
			this.preferences.Add (this.preferenceEnableTabReplacement);
			this.preferences.Add (this.preferenceTabCharacter);
			this.preferences.Add (this.preferenceTabGear);
			this.preferences.Add (this.preferenceTabNeeds);
			this.preferences.Add (this.preferenceTabHealth);
			this.preferences.Add (this.preferenceTabGuestAndPrisoner);
			this.preferences.Add (this.preferenceTabTraining);
			this.preferences.Add (this.preferenceTabGrowing);
			this.preferences.Add (this.preferenceTabBills);
			this.preferences.Add (this.preferenceTabArt);
			this.preferenceTabCharacter.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGear.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabNeeds.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabHealth.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGuestAndPrisoner.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabTraining.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGrowing.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabBills.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabArt.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.tabBills = new ITab_Bills_Alternate ();
			this.tabGrowing = new ITab_Growing_Alternate ();
			this.tabArt = new ITab_Art_Alternate ();
			this.tabCharacter = new ITab_Pawn_Character_Alternate (this.preferenceTabBrowseButtons);
			this.tabCharacterVanilla = new ITab_Pawn_Character_Vanilla (this.preferenceTabBrowseButtons);
			this.tabGear = new ITab_Pawn_Gear_Alternate (this.preferenceTabBrowseButtons);
			this.tabGearVanilla = new ITab_Pawn_Gear_Vanilla (this.preferenceTabBrowseButtons);
			this.tabNeeds = new ITab_Pawn_Needs_Alternate (this.preferenceTabBrowseButtons);
			this.tabNeedsVanilla = new ITab_Pawn_Needs_Vanilla (this.preferenceTabBrowseButtons);
			this.tabHealth = new ITab_Pawn_Health_Alternate (this.preferenceTabBrowseButtons);
			this.tabHealthVanilla = new ITab_Pawn_Health_Vanilla (this.preferenceTabBrowseButtons);
			this.tabTraining = new ITab_Pawn_Training_Alternate (this.preferenceTabBrowseButtons);
			this.tabTrainingVanilla = new ITab_Pawn_Training_Vanilla (this.preferenceTabBrowseButtons);
			this.tabGuest = new ITab_Pawn_Guest_Alternate (this.preferenceTabBrowseButtons);
			this.tabGuestVanilla = new ITab_Pawn_Guest_Vanilla (this.preferenceTabBrowseButtons);
			this.tabPrisoner = new ITab_Pawn_Prisoner_Alternate (this.preferenceTabBrowseButtons);
			this.tabPrisonerVanilla = new ITab_Pawn_Prisoner_Vanilla (this.preferenceTabBrowseButtons);
		}

		//
		// Methods
		//
		public ReplacementTabs CreateReplacementTabs ()
		{
			this.replacementTabs.Clear ();
			Dictionary<Type, ITab> dictionary = new Dictionary<Type, ITab> ();
			if (this.preferenceTabCharacter.Value) {
				dictionary [typeof(ITab_Pawn_Character)] = this.tabCharacter;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Character)] = this.tabCharacterVanilla;
			}
			if (this.preferenceTabGear.Value) {
				dictionary [typeof(ITab_Pawn_Gear)] = this.tabGear;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Gear)] = this.tabGearVanilla;
			}
			if (this.preferenceTabNeeds.Value) {
				dictionary [typeof(ITab_Pawn_Needs)] = this.tabNeeds;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Needs)] = this.tabNeedsVanilla;
			}
			if (this.preferenceTabHealth.Value) {
				dictionary [typeof(ITab_Pawn_Health)] = this.tabHealth;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Health)] = this.tabHealthVanilla;
			}
			if (this.preferenceTabGuestAndPrisoner.Value) {
				dictionary [typeof(ITab_Pawn_Guest)] = this.tabGuest;
				dictionary [typeof(ITab_Pawn_Prisoner)] = this.tabPrisoner;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Guest)] = this.tabGuestVanilla;
				dictionary [typeof(ITab_Pawn_Prisoner)] = this.tabPrisonerVanilla;
			}
			if (this.preferenceTabTraining.Value) {
				dictionary [typeof(ITab_Pawn_Training)] = this.tabTraining;
			}
			else if (this.preferenceTabBrowseButtons.Value) {
				dictionary [typeof(ITab_Pawn_Training)] = this.tabTrainingVanilla;
			}
			if (this.preferenceTabBills.Value) {
				dictionary [typeof(ITab_Bills)] = this.tabBills;
			}
			if (this.preferenceTabArt.Value) {
				dictionary [typeof(ITab_Art)] = this.tabArt;
			}
			foreach (ThingDef current in DefDatabase<ThingDef>.get_AllDefs ()) {
				if (current.inspectorTabsResolved != null) {
					bool flag = false;
					foreach (ITab current2 in current.inspectorTabsResolved) {
						if (dictionary.ContainsKey (current2.GetType ())) {
							flag = true;
							break;
						}
					}
					if (flag) {
						List<ITab> list = new List<ITab> ();
						foreach (ITab current3 in current.inspectorTabsResolved) {
							ITab item;
							if (dictionary.TryGetValue (current3.GetType (), out item)) {
								list.Add (item);
							}
							else {
								list.Add (current3);
							}
						}
						this.replacementTabs.AddThingDef (current, list);
					}
				}
			}
			if (this.preferenceTabGrowing.Value) {
				this.replacementTabs.AddZoneType (typeof(ITab_Growing), this.tabGrowing);
			}
			return this.replacementTabs;
		}

		public void Initialize (UserInterface userInterface)
		{
			MainTabWindow_Inspect mainTabWindow_Inspect = userInterface.FindMainTabOfType<MainTabWindow_Inspect> ();
			if (mainTabWindow_Inspect != null) {
				this.ResetReplacementTabs (mainTabWindow_Inspect);
			}
		}

		public void PrepareDependencies (UserInterface userInterface)
		{
			this.preferenceEnableTabReplacement.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabBrowseButtons.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabCharacter.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabGear.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabNeeds.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabHealth.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabGuestAndPrisoner.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabTraining.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabGrowing.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabBills.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
			this.preferenceTabArt.ValueChanged += delegate (bool value) {
				this.dirtyFlag = true;
			};
		}

		public void ResetReplacementTabs (MainTabWindow_Inspect window)
		{
			ReplacementTabs replacementTabs = this.CreateReplacementTabs ();
			if (!replacementTabs.Empty) {
				window.ReplacementTabs = this.CreateReplacementTabs ();
			}
			else {
				window.ReplacementTabs = null;
			}
		}

		public void ResetTextures ()
		{
			BrowseButtonDrawer.ResetTextures ();
			ITab_Pawn_Health_Alternate.ResetTextures ();
			MedicalCareUtility.Reset ();
			BillDrawer.ResetTextures ();
			TabDrawer.ResetTextures ();
		}

		public void Update ()
		{
			if (this.dirtyFlag) {
				UserInterface userInterface = Find.get_UIRoot_Map () as UserInterface;
				if (userInterface != null) {
					MainTabWindow_Inspect mainTabWindow_Inspect = userInterface.FindMainTabOfType<MainTabWindow_Inspect> ();
					if (mainTabWindow_Inspect != null) {
						this.ResetReplacementTabs (mainTabWindow_Inspect);
					}
				}
				this.dirtyFlag = false;
			}
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class ConceptDeciderComponent : IUpdatedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "ConceptDecider";
			}
		}

		//
		// Methods
		//
		public void Update ()
		{
			ConceptDecider.ConceptDeciderUpdate ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Controller : MonoBehaviour
	{
		//
		// Static Fields
		//
		public static readonly string ModName = "EdB Interface";

		public static readonly string GameObjectName = "EdBInterfaceController";

		//
		// Fields
		//
		protected Type uiRootType;

		protected RootMap replacementRootMap;

		protected bool gameplay;

		protected Window previousWindow;

		protected Window currentWindow;

		//
		// Properties
		//
		public bool ModEnabled {
			get {
				InstalledMod installedMod = InstalledModLister.get_AllInstalledMods ().First ((InstalledMod m) => m.get_Name ().Equals (Controller.ModName));
				return installedMod != null && installedMod.get_Active ();
			}
		}

		public Window TopWindow {
			get {
				foreach (Window current in Find.get_WindowStack ().get_Windows ()) {
					if (current.GetType ().FullName != "Verse.EditWindow_Log") {
						return current;
					}
				}
				return null;
			}
		}

		//
		// Methods
		//
		public virtual void GameplayUpdate ()
		{
			Root rootRoot = Find.get_RootRoot ();
			if (rootRoot != null) {
				UIRoot uiRoot = rootRoot.uiRoot;
				if (uiRoot != null) {
					if (!uiRoot.GetType ().Equals (this.uiRootType)) {
						this.uiRootType = uiRoot.GetType ();
					}
					if (uiRoot.GetType ().Equals (typeof(UIRoot_Map))) {
						if (this.ModEnabled) {
							try {
								this.ReplaceUIRoot ();
								Log.Message ("Replaced standard gameplay interface with EdB Interface");
							}
							catch (Exception ex) {
								Log.Error ("Failed to replace gameplay interface with EdB Interface");
								Log.Error (ex.ToString ());
							}
							base.set_enabled (false);
						}
					}
					else {
						Log.Message ("EdB Interface mod not enabled.  Will not replace gameplay interface");
					}
				}
			}
		}

		public virtual void MenusUpdate ()
		{
			bool flag = false;
			Window topWindow = this.TopWindow;
			if (topWindow != this.currentWindow) {
				this.previousWindow = this.currentWindow;
				this.currentWindow = topWindow;
				flag = true;
			}
			if (flag && this.previousWindow != null && (this.previousWindow.GetType ().FullName.Equals ("RimWorld.Page_ModsConfig") || this.previousWindow.GetType ().FullName.Equals ("EdB.ModOrder.Page_ModsConfig")) && this.currentWindow == null && !this.ModEnabled) {
				this.UnloadMod ();
				return;
			}
		}

		public void OnLevelWasLoaded (int level)
		{
			if (level == 0) {
				this.gameplay = false;
				base.set_enabled (true);
			}
			else if (level == 1) {
				this.gameplay = true;
				base.set_enabled (true);
			}
		}

		public void ReplaceUIRoot ()
		{
			UIRoot_Map uIRoot_Map = Find.get_UIRoot_Map ();
			if (uIRoot_Map == null) {
				Log.Error ("No user interface found.  Cannot replace with the EdB interface.");
				return;
			}
			UserInterface userInterface = new UserInterface ();
			userInterface.windows = uIRoot_Map.windows;
			Root rootRoot = Find.get_RootRoot ();
			rootRoot.uiRoot = userInterface;
		}

		public virtual void Start ()
		{
			base.set_enabled (true);
		}

		protected void UnloadMod ()
		{
			FieldInfo field = typeof(ITabManager).GetField ("sharedInstances", BindingFlags.Static | BindingFlags.NonPublic);
			Dictionary<Type, ITab> dictionary = (Dictionary<Type, ITab>)field.GetValue (null);
			dictionary.Remove (typeof(Bootstrap));
			GameObject gameObject = GameObject.Find (Controller.GameObjectName);
			Object.Destroy (gameObject);
			Log.Message ("Unloaded " + Controller.ModName);
		}

		public virtual void Update ()
		{
			try {
				if (!this.gameplay) {
					this.MenusUpdate ();
				}
				else {
					this.GameplayUpdate ();
				}
			}
			catch (Exception ex) {
				base.set_enabled (false);
				Log.Error (ex.ToString ());
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public static class DateReadout
	{
		//
		// Static Fields
		//
		public const float Height = 22;

		private static readonly float TicksPerMinute = 20.83333;

		private static readonly List<string> fastHourStrings = new List<string> ();

		private static int dateStringDay = -1;

		private static string dateString;

		//
		// Static Methods
		//
		public static void AlternateDateOnGUI (Rect dateRect, bool amPm, int minuteInterval)
		{
			if (Mouse.IsOver (dateRect)) {
				Widgets.DrawHighlight (dateRect);
			}
			GUI.BeginGroup (dateRect);
			Text.set_Font (1);
			Text.set_Anchor (5);
			float num = 9;
			float num2 = dateRect.get_width () * 0 / num;
			Text.set_Font (1);
			float num3 = dateRect.get_width () / num;
			Rect rect = new Rect (num2, 0, num3 * 4.25, 22);
			int num4 = GenDate.get_HourInt ();
			int num5 = (Find.get_TickManager ().get_TicksGame () + Find.get_TickManager ().gameStartAbsTick) % 30000;
			int num6 = (int)((float)(num5 % 1250) / DateReadout.TicksPerMinute) / minuteInterval * minuteInterval;
			string text = string.Empty;
			if (amPm) {
				text = " " + ((num4 >= 12) ? "PM" : "AM");
				if (num4 == 0) {
					num4 = 12;
				}
				else if (num4 > 12) {
					num4 -= 12;
				}
			}
			Widgets.Label (rect, string.Concat (new object[] {
				(num4 >= 10) ? null : "0",
				num4,
				":",
				(num6 >= 10) ? null : "0",
				num6,
				text
			}));
			float num7 = dateRect.get_width () * 3 / num;
			Text.set_Font (1);
			Rect rect2 = new Rect (num7 - 8, 0, num3 * 4, 22);
			if (GenDate.get_DayOfMonth () != DateReadout.dateStringDay) {
				DateReadout.dateString = GenDate.get_CurrentMonthDateShortString ();
				DateReadout.dateStringDay = GenDate.get_DayOfMonth ();
			}
			Widgets.Label (rect2, DateReadout.dateString);
			float num8 = dateRect.get_width () * 7 / num;
			Text.set_Font (1);
			Rect rect3 = new Rect (num8 - 8, 0, num3 * 2, 22);
			Widgets.Label (rect3, GenString.ToStringCached (GenDate.get_CurrentYear ()));
			Text.set_Anchor (0);
			GUI.EndGroup ();
			TooltipHandler.TipRegion (dateRect, new TipSignal (() => Translator.Translate ("DateReadoutTip", new object[] {
				GenDate.get_DaysPassed (),
				10,
				DateUtility.Label (GenDate.get_CurrentSeason ())
			}), 86423));
			Text.set_Anchor (0);
		}

		public static void DateOnGUI (Rect dateRect)
		{
			if (Mouse.IsOver (dateRect)) {
				Widgets.DrawHighlight (dateRect);
			}
			GUI.BeginGroup (dateRect);
			Text.set_Font (1);
			Text.set_Anchor (4);
			float num = dateRect.get_width () * 1 / 6;
			Text.set_Font (1);
			Rect rect = new Rect (num - 50, 0, 100, 22);
			Widgets.Label (rect, DateReadout.fastHourStrings [GenDate.get_HourInt ()]);
			float num2 = dateRect.get_width () * 3 / 6;
			Text.set_Font (1);
			Rect rect2 = new Rect (num2 - 50, 0, 100, 22);
			if (GenDate.get_DayOfMonth () != DateReadout.dateStringDay) {
				DateReadout.dateString = GenDate.get_CurrentMonthDateShortString ();
				DateReadout.dateStringDay = GenDate.get_DayOfMonth ();
			}
			Widgets.Label (rect2, DateReadout.dateString);
			float num3 = dateRect.get_width () * 5 / 6;
			Text.set_Font (1);
			Rect rect3 = new Rect (num3 - 50, 0, 100, 22);
			Widgets.Label (rect3, GenString.ToStringCached (GenDate.get_CurrentYear ()));
			Text.set_Anchor (0);
			GUI.EndGroup ();
			TooltipHandler.TipRegion (dateRect, new TipSignal (() => Translator.Translate ("DateReadoutTip", new object[] {
				GenDate.get_DaysPassed (),
				10,
				DateUtility.Label (GenDate.get_CurrentSeason ())
			}), 86423));
		}

		public static void Reinit ()
		{
			DateReadout.dateString = null;
			DateReadout.dateStringDay = -1;
			DateReadout.fastHourStrings.Clear ();
			for (int i = 0; i < 24; i++) {
				DateReadout.fastHourStrings.Add (i + Translator.Translate ("LetterHour"));
			}
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class DebugToolsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "DebugTools";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			DebugTools.DebugToolsOnGUI ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class DebugWindowsOpenerComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private DebugWindowsOpener debugWindowsOpener;

		//
		// Properties
		//
		public string Name {
			get {
				return "DebugWindowsOpener";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Constructors
		//
		public DebugWindowsOpenerComponent (DebugWindowsOpener debugWindowsOpener)
		{
			this.debugWindowsOpener = debugWindowsOpener;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.debugWindowsOpener.DevToolStarterOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class Designator_ZoneAddStockpile_Resources : Designator_ZoneAddStockpile_Resources
	{
		//
		// Fields
		//
		protected List<ThingDef> thingDefs = new List<ThingDef> (100);

		private BooleanPreference emptyZonePreference;

		//
		// Properties
		//
		public BooleanPreference EmptyZonePreference {
			set {
				this.emptyZonePreference = value;
			}
		}

		//
		// Methods
		//
		protected override Zone MakeNewZone ()
		{
			Zone_Stockpile zone_Stockpile = new Zone_Stockpile (0);
			if (this.emptyZonePreference != null && this.emptyZonePreference.Value) {
				if (Find.get_ZoneManager ().get_AllZones ().Count ((Zone zone) => zone is Zone_Stockpile) > 1) {
					ThingFilter filter = zone_Stockpile.GetStoreSettings ().filter;
					this.thingDefs.Clear ();
					foreach (ThingDef current in filter.get_AllowedThingDefs ()) {
						this.thingDefs.Add (current);
					}
					foreach (ThingDef current2 in this.thingDefs) {
						filter.SetAllow (current2, false);
					}
					foreach (SpecialThingFilterDef current3 in DefDatabase<SpecialThingFilterDef>.get_AllDefs ()) {
						if (filter.Allowed (current3)) {
							filter.SetAllow (current3, false);
						}
					}
				}
			}
			return zone_Stockpile;
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class DesignatorManagerComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "DesignatorManager";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			DesignatorManager.DesignationManagerOnGUI ();
		}

		public void Update ()
		{
			DesignatorManager.DesignatorManagerUpdate ();
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Dialog_InterfaceOptions : Window
	{
		//
		// Static Fields
		//
		private static readonly Vector2 WindowSize = new Vector2 (500, 720);

		public static Color DisabledControlColor = new Color (1, 1, 1, 0.5);

		public static float IndentSize = 16;

		public static Vector2 PreferencePadding = new Vector2 (8, 6);

		public static readonly int SectionPadding = 14;

		public static readonly int LabelLineHeight = 30;

		//
		// Fields
		//
		private ScrollView optionListView = new ScrollView ();

		protected bool closed = true;

		//
		// Properties
		//
		public override Vector2 InitialWindowSize {
			get {
				int num = 0;
				foreach (PreferenceGroup current in Preferences.Instance.Groups) {
					foreach (IPreference current2 in current.Preferences) {
						if (current2.DisplayInOptions) {
							num++;
						}
					}
				}
				if (num < 6) {
					return new Vector2 (500, 420);
				}
				return new Vector2 (Dialog_InterfaceOptions.WindowSize.x, Dialog_InterfaceOptions.WindowSize.y);
			}
		}

		//
		// Constructors
		//
		public Dialog_InterfaceOptions ()
		{
			this.closeOnEscapeKey = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.forcePause = true;
		}

		//
		// Methods
		//
		public override void DoWindowContents (Rect inRect)
		{
			try {
				GUI.BeginGroup (inRect);
				Rect viewRect = new Rect (8, 16, inRect.get_width () - 8, inRect.get_height () - 60);
				float num = 0;
				try {
					this.optionListView.Begin (viewRect);
					GUI.get_skin ().get_label ().set_alignment (0);
					Text.set_Anchor (0);
					GUI.set_color (Color.get_white ());
					float width = this.optionListView.ContentWidth - Dialog_InterfaceOptions.PreferencePadding.x - Dialog_InterfaceOptions.PreferencePadding.x;
					foreach (PreferenceGroup current in Preferences.Instance.Groups) {
						bool flag = false;
						foreach (IPreference current2 in current.Preferences) {
							if (current2.DisplayInOptions) {
								flag = true;
								break;
							}
						}
						if (flag) {
							this.SectionLabel (Translator.Translate (current.Name), ref num, width);
							foreach (IPreference current3 in current.Preferences) {
								if (current3.DisplayInOptions) {
									current3.OnGUI (Dialog_InterfaceOptions.PreferencePadding.x, ref num, width);
									num += Dialog_InterfaceOptions.PreferencePadding.y;
								}
							}
							this.SectionDivider (ref num);
						}
					}
				}
				finally {
					this.optionListView.End (num);
				}
			}
			finally {
				GUI.EndGroup ();
				GUI.set_color (Color.get_white ());
				Text.set_Anchor (0);
			}
		}

		public override void PostClose ()
		{
			base.PostClose ();
			Preferences.Instance.Save ();
		}

		protected void SectionDivider (ref float cursor)
		{
			cursor += (float)Dialog_InterfaceOptions.SectionPadding;
		}

		protected void SectionLabel (string message, ref float cursor, float width)
		{
			Text.set_Font (2);
			Widgets.Label (new Rect (Dialog_InterfaceOptions.PreferencePadding.x, cursor, width, (float)Dialog_InterfaceOptions.LabelLineHeight), message);
			cursor += (float)(Dialog_InterfaceOptions.LabelLineHeight + 6);
			Text.set_Font (1);
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Dialog_NameSquad : Window
	{
		//
		// Static Fields
		//
		private const int MaxNameLength = 20;

		//
		// Fields
		//
		private bool initializedFocus;

		private bool newSquad;

		private string currentName;

		private Squad squad;

		//
		// Properties
		//
		public override Vector2 InitialWindowSize {
			get {
				return new Vector2 (500, 200);
			}
		}

		//
		// Constructors
		//
		public Dialog_NameSquad (Squad squad, bool newSquad)
		{
			this.squad = squad;
			this.currentName = squad.Name;
			this.newSquad = newSquad;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		//
		// Methods
		//
		protected bool ChangeName ()
		{
			if (this.currentName.Length > 0) {
				SquadManager.Instance.RenameSquad (this.squad, this.currentName);
				return true;
			}
			return false;
		}

		public override void DoWindowContents (Rect inRect)
		{
			Text.set_Font (2);
			if (this.newSquad) {
				Widgets.Label (new Rect (15, 15, 500, 50), Translator.Translate ("EdB.Squads.Window.NameDialog.New"));
			}
			else {
				Widgets.Label (new Rect (15, 15, 500, 50), Translator.Translate ("EdB.Squads.Window.NameDialog.Rename"));
			}
			Text.set_Font (1);
			bool flag = Event.get_current ().get_type () == 4 && Event.get_current ().get_keyCode () == 13;
			Rect rect = new Rect (15, 50, inRect.get_width () / 2 - 20, 35);
			GUI.SetNextControlName ("NameSquad");
			string text = Widgets.TextField (rect, this.currentName);
			if (text.Length <= 20) {
				this.currentName = text;
			}
			string nameOfFocusedControl = GUI.GetNameOfFocusedControl ();
			if (!this.initializedFocus) {
				if (nameOfFocusedControl == string.Empty) {
					GUI.FocusControl ("NameSquad");
					this.initializedFocus = true;
				}
				else if (nameOfFocusedControl == "NameSquad") {
					TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject (typeof(TextEditor), GUIUtility.get_keyboardControl ());
					textEditor.SelectAll ();
					this.initializedFocus = true;
				}
			}
			else if (nameOfFocusedControl == "NameSquad" && flag && this.ChangeName ()) {
				this.Close (true);
				return;
			}
			if (!this.newSquad && Widgets.TextButton (new Rect (20, inRect.get_height () - 35, inRect.get_width () / 2 - 20, 35), "Cancel", true, false)) {
				this.Close (true);
				return;
			}
			if (Widgets.TextButton (new Rect (inRect.get_width () / 2 + 20, inRect.get_height () - 35, inRect.get_width () / 2 - 20, 35), "OK", true, false) && this.ChangeName ()) {
				this.Close (true);
				return;
			}
		}
	}
}
using RimWorld;
using System;

namespace EdB.Interface
{
	public class DragBoxComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private Selector selector;

		//
		// Properties
		//
		public string Name {
			get {
				return "DragBox";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Constructors
		//
		public DragBoxComponent (Selector selector)
		{
			this.selector = selector;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.selector.dragBox.DragBoxOnGUI ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class EquippableThing
	{
		//
		// Fields
		//
		public Thing thing;

		public float distance;

		//
		// Constructors
		//
		public EquippableThing ()
		{
		}

		public EquippableThing (Thing thing)
		{
			this.thing = thing;
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class EquippableThings
	{
		//
		// Fields
		//
		protected Dictionary<InventoryRecordKey, List<EquippableThing>> thingLookup = new Dictionary<InventoryRecordKey, List<EquippableThing>> ();

		//
		// Indexer
		//
		public List<EquippableThing> this [InventoryRecordKey key] {
			get {
				List<EquippableThing> result;
				if (this.thingLookup.TryGetValue (key, out result)) {
					return result;
				}
				return null;
			}
			set {
				this.thingLookup.Add (key, value);
			}
		}

		//
		// Methods
		//
		public void Add (InventoryRecordKey key, Thing thing)
		{
			List<EquippableThing> list = this [key];
			if (list == null) {
				list = new List<EquippableThing> ();
				this [key] = list;
			}
			list.Add (new EquippableThing (thing));
		}

		public void Reset ()
		{
			this.thingLookup.Clear ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class GizmoGridDrawer
	{
		//
		// Static Fields
		//
		private static float heightDrawn = 0;

		private static BooleanPreference rightClickMaterialPreference;

		private static List<Gizmo> firstGizmos = new List<Gizmo> ();

		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>> ();

		private static readonly Vector2 GizmoSpacing = new Vector2 (5, 14);

		private static int heightDrawnFrame;

		//
		// Static Properties
		//
		public static float HeightDrawnRecently {
			get {
				if (Time.get_frameCount () > GizmoGridDrawer.heightDrawnFrame + 2) {
					return 0;
				}
				return GizmoGridDrawer.heightDrawn;
			}
		}

		public static BooleanPreference RightClickMaterialPreference {
			set {
				GizmoGridDrawer.rightClickMaterialPreference = value;
			}
		}

		//
		// Static Methods
		//
		public static void DrawGizmoGrid (IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
		{
			GizmoGridDrawer.gizmoGroups.Clear ();
			foreach (Gizmo current in gizmos) {
				bool flag = false;
				for (int i = 0; i < GizmoGridDrawer.gizmoGroups.Count; i++) {
					if (GizmoGridDrawer.gizmoGroups [i] [0].GroupsWith (current)) {
						flag = true;
						GizmoGridDrawer.gizmoGroups [i].Add (current);
						break;
					}
				}
				if (!flag) {
					List<Gizmo> list = new List<Gizmo> ();
					list.Add (current);
					GizmoGridDrawer.gizmoGroups.Add (list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear ();
			for (int j = 0; j < GizmoGridDrawer.gizmoGroups.Count; j++) {
				List<Gizmo> source = GizmoGridDrawer.gizmoGroups [j];
				Gizmo gizmo = source.FirstOrDefault ((Gizmo opt) => !opt.disabled);
				if (gizmo == null) {
					gizmo = source.FirstOrDefault<Gizmo> ();
				}
				GizmoGridDrawer.firstGizmos.Add (gizmo);
			}
			GizmoGridDrawer.drawnHotKeys.Clear ();
			float num = (float)(Screen.get_width () - 140);
			Text.set_Font (0);
			Vector2 vector = new Vector2 (startX, (float)(Screen.get_height () - 35) - GizmoGridDrawer.GizmoSpacing.y - 75);
			mouseoverGizmo = null;
			Gizmo interactedGiz = null;
			Event @event = null;
			for (int k = 0; k < GizmoGridDrawer.firstGizmos.Count; k++) {
				Gizmo gizmo2 = GizmoGridDrawer.firstGizmos [k];
				if (gizmo2.get_Visible ()) {
					if (vector.x + gizmo2.get_Width () + GizmoGridDrawer.GizmoSpacing.x > num) {
						vector.x = startX;
						vector.y -= 75 + GizmoGridDrawer.GizmoSpacing.x;
					}
					GizmoGridDrawer.heightDrawnFrame = Time.get_frameCount ();
					GizmoGridDrawer.heightDrawn = (float)Screen.get_height () - vector.y;
					GizmoResult gizmoResult = gizmo2.GizmoOnGUI (vector);
					if (gizmoResult.get_State () == 2) {
						@event = gizmoResult.get_InteractEvent ();
						interactedGiz = gizmo2;
					}
					if (gizmoResult.get_State () >= 1) {
						mouseoverGizmo = gizmo2;
					}
					Rect rect = new Rect (vector.x, vector.y, gizmo2.get_Width (), 75 + GizmoGridDrawer.GizmoSpacing.y);
					rect = GenUI.ContractedBy (rect, -12);
					GenUI.AbsorbClicksInRect (rect);
					vector.x += gizmo2.get_Width () + GizmoGridDrawer.GizmoSpacing.x;
				}
			}
			if (interactedGiz != null) {
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups.First ((List<Gizmo> group) => group.Contains (interactedGiz));
				for (int l = 0; l < list2.Count; l++) {
					Gizmo gizmo3 = list2 [l];
					if (gizmo3 != interactedGiz && !gizmo3.disabled && gizmo3.InheritInteractionsFrom (interactedGiz)) {
						gizmo3.ProcessInput (@event);
					}
				}
				if (interactedGiz is Designator_Build) {
					if (@event.get_button () != 1 && GizmoGridDrawer.rightClickMaterialPreference != null && GizmoGridDrawer.rightClickMaterialPreference.Value) {
						Command command = interactedGiz as Command;
						if (command != null && command.get_CurActivateSound () != null) {
							SoundStarter.PlayOneShotOnCamera (command.get_CurActivateSound ());
						}
						Designator designator = interactedGiz as Designator;
						if (designator != null) {
							DesignatorManager.Select (designator);
						}
					}
					else {
						interactedGiz.ProcessInput (@event);
					}
				}
				else {
					interactedGiz.ProcessInput (@event);
				}
				Event.get_current ().Use ();
			}
		}
	}
}
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class GlobalControls
	{
		//
		// Static Fields
		//
		public const float Width = 134;

		private const int VisibilityControlsPerRow = 5;

		private static readonly int TempSearchNumRadialCells = GenRadial.NumCellsInRadius (2.9);

		//
		// Fields
		//
		protected PreferenceMinuteInterval preferenceMinuteInterval;

		protected PreferenceAmPm preferenceAmPm;

		protected PreferenceEnableAlternateTimeDisplay preferenceEnableAlternateTimeDisplay;

		private WidgetRow rowVisibility = new WidgetRow ();

		//
		// Properties
		//
		public PreferenceAmPm PreferenceAmPm {
			set {
				this.preferenceAmPm = value;
			}
		}

		public PreferenceEnableAlternateTimeDisplay PreferenceEnableAlternateTimeDisplay {
			set {
				this.preferenceEnableAlternateTimeDisplay = value;
			}
		}

		public PreferenceMinuteInterval PreferenceMinuteInterval {
			set {
				this.preferenceMinuteInterval = value;
			}
		}

		//
		// Static Methods
		//
		private static string TemperatureString ()
		{
			IntVec3 intVec = Gen.MouseCell ();
			IntVec3 intVec2 = IntVec3.get_Invalid ();
			Room room = null;
			for (int i = 0; i < GlobalControls.TempSearchNumRadialCells; i++) {
				intVec2 = intVec + GenRadial.RadialPattern [i];
				if (GenGrid.InBounds (intVec2)) {
					room = GridsUtility.GetRoom (intVec2);
					if (room != null) {
						break;
					}
				}
			}
			if (room == null && GenGrid.InBounds (intVec)) {
				Building edifice = GridsUtility.GetEdifice (intVec);
				if (edifice != null) {
					CellRect.CellRectIterator iterator = GenAdj.OccupiedRect (edifice).ExpandedBy (1).ClipInsideMap ().GetIterator ();
					while (!iterator.Done ()) {
						IntVec3 current = iterator.get_Current ();
						room = GridsUtility.GetRoom (current);
						if (room != null && !room.get_PsychologicallyOutdoors ()) {
							intVec2 = current;
							break;
						}
						iterator.MoveNext ();
					}
				}
			}
			string str;
			if (GenGrid.InBounds (intVec2) && !GridsUtility.Fogged (intVec2) && room != null && !room.get_PsychologicallyOutdoors ()) {
				if (!room.get_UsesOutdoorTemperature ()) {
					str = Translator.Translate ("Indoors");
				}
				else {
					str = Translator.Translate ("IndoorsUnroofed");
				}
			}
			else {
				str = Translator.Translate ("Outdoors");
			}
			float num = (room == null || GridsUtility.Fogged (intVec2)) ? GenTemperature.get_OutdoorTemp () : room.get_Temperature ();
			return str + " " + GenText.ToStringTemperature (num, "F0");
		}

		//
		// Methods
		//
		public void GlobalControlsOnGUI ()
		{
			float num = (float)Screen.get_width () - 134;
			float num2 = (float)Screen.get_height ();
			num2 -= 35;
			GenUI.DrawTextWinterShadow (new Rect ((float)(Screen.get_width () - 270), (float)(Screen.get_height () - 450), 270, 450));
			num2 -= 4;
			float arg_5E_0 = num2;
			Vector2 timeButSize = TimeControls.TimeButSize;
			float num3 = arg_5E_0 - timeButSize.y;
			this.rowVisibility.Init ((float)Screen.get_width (), num3, 0, 141, 29);
			Find.get_PlaySettings ().DoPlaySettingsGlobalControls (this.rowVisibility);
			num2 = this.rowVisibility.get_FinalY ();
			num2 -= 4;
			Vector2 timeButSize2 = TimeControls.TimeButSize;
			float y = timeButSize2.y;
			Rect rect = new Rect (num, num2 - y, 134, y);
			TimeControls.DoTimeControlsGUI (rect);
			num2 -= rect.get_height ();
			num2 -= 4;
			if (this.preferenceEnableAlternateTimeDisplay == null || !this.preferenceEnableAlternateTimeDisplay.Value) {
				Rect dateRect = new Rect (num, num2 - 22, 134, 22);
				DateReadout.DateOnGUI (dateRect);
				num2 -= dateRect.get_height ();
			}
			else {
				Rect dateRect2 = new Rect (num - 48, num2 - 22, 182, 22);
				DateReadout.AlternateDateOnGUI (dateRect2, this.preferenceAmPm.Value, this.preferenceMinuteInterval.Value);
				num2 -= dateRect2.get_height ();
			}
			Rect rect2 = new Rect (num - 30, num2 - 26, 164, 26);
			Find.get_WeatherManager ().DoWeatherGUI (rect2);
			num2 -= rect2.get_height ();
			Rect rect3 = new Rect (num - 100, num2 - 26, 227, 26);
			Text.set_Anchor (5);
			Widgets.Label (rect3, GlobalControls.TemperatureString ());
			Text.set_Anchor (0);
			num2 -= 26;
			float num4 = 164;
			float num5 = Find.get_MapConditionManager ().TotalHeightAt (num4 - 15);
			Rect rect4 = new Rect (num - 30, num2 - num5, num4, num5);
			Find.get_MapConditionManager ().DoConditionsUI (rect4);
			num2 -= rect4.get_height ();
			num2 -= 10;
			Find.get_LetterStack ().LettersOnGUI (num2);
		}
	}
}
using System;

namespace EdB.Interface
{
	public class GlobalControlsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private GlobalControls globalControls;

		//
		// Properties
		//
		public string Name {
			get {
				return "GlobalControls";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Constructors
		//
		public GlobalControlsComponent (GlobalControls globalControls)
		{
			this.globalControls = globalControls;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.globalControls.GlobalControlsOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class HealthCardUtility
	{
		//
		// Static Fields
		//
		private const float ThoughtLevelHeight = 25;

		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get ("UI/Icons/Medical/Bleeding", true);

		private static bool showHediffsDebugInfo = false;

		private static Vector2 scrollPosition = Vector2.get_zero ();

		private static float scrollViewHeight = 0;

		private static bool highlight = true;

		private static Vector2 billsScrollPosition = Vector2.get_zero ();

		private static float billsScrollHeight = 1000;

		private static readonly Texture2D TreatedPoorIcon = ContentFinder<Texture2D>.Get ("UI/Icons/Medical/TreatedPoorly", true);

		private const float IconSize = 20;

		private const float ThoughtLevelSpacing = 4;

		private static readonly Color SeverePainColor = new Color (0.9, 0.5, 0);

		private static readonly Color MediumPainColor = new Color (0.9, 0.9, 0);

		private static readonly Color StaticHighlightColor = new Color (0.75, 0.75, 0.85, 1);

		private static readonly Color HighlightColor = new Color (0.5, 0.5, 0.5, 1);

		private static readonly Texture2D TreatedWellIcon = ContentFinder<Texture2D>.Get ("UI/Icons/Medical/TreatedWell", true);

		//
		// Fields
		//
		protected MethodInfo visibleHediffGroupsInOrderMethod;

		protected MethodInfo visibleHediffsMethod;

		//
		// Constructors
		//
		public HealthCardUtility ()
		{
			this.visibleHediffGroupsInOrderMethod = typeof(HealthCardUtility).GetMethod ("VisibleHediffGroupsInOrder", BindingFlags.Static | BindingFlags.NonPublic);
			this.visibleHediffsMethod = typeof(HealthCardUtility).GetMethod ("VisibleHediffs", BindingFlags.Static | BindingFlags.NonPublic);
		}

		//
		// Static Methods
		//
		private static void DoRightRowHighlight (Rect rowRect)
		{
			if (HealthCardUtility.highlight) {
				GUI.set_color (HealthCardUtility.StaticHighlightColor);
				GUI.DrawTexture (rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
			if (rowRect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (HealthCardUtility.HighlightColor);
				GUI.DrawTexture (rowRect, TexUI.HighlightTex);
			}
		}

		public static Color GetBleedingRateColor (float rate)
		{
			Color result = Color.get_white ();
			if ((double)rate < 0.15) {
				result = HealthCardUtility.MediumPainColor;
			}
			else if ((double)rate < 0.4) {
				result = HealthCardUtility.MediumPainColor;
			}
			else if ((double)rate < 0.8) {
				result = HealthCardUtility.SeverePainColor;
			}
			else {
				result = HealthUtility.DarkRedColor;
			}
			return result;
		}

		//
		// Methods
		//
		public void DrawHediffRow (Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			float num = 6;
			float num2 = rect.get_width () * 0.4;
			float num3 = rect.get_width () - num2 - 20;
			num2 -= num;
			float num4;
			if (diffs.First<Hediff> ().get_Part () == null) {
				num4 = Text.CalcHeight (Translator.Translate ("WholeBody"), num2);
			}
			else {
				num4 = Text.CalcHeight (diffs.First<Hediff> ().get_Part ().def.get_LabelCap (), num2);
			}
			float num5 = 0;
			float num6 = curY;
			float num7 = 0;
			foreach (IGrouping<int, Hediff> current in from x in diffs
			group x by x.get_UIGroupKey ()) {
				int num8 = current.Count<Hediff> ();
				string text = current.First<Hediff> ().get_LabelCap ();
				if (num8 != 1) {
					text = text + " x" + num8.ToString ();
				}
				num7 += Text.CalcHeight (text, num3);
			}
			num5 = num7;
			Rect rect2 = new Rect (0, curY, rect.get_width (), Mathf.Max (num4, num5));
			HealthCardUtility.DoRightRowHighlight (rect2);
			StringBuilder stringBuilder = new StringBuilder ();
			if (diffs.First<Hediff> ().get_Part () != null) {
				stringBuilder.Append (diffs.First<Hediff> ().get_Part ().def.get_LabelCap () + ": ");
				GUI.set_color (HealthUtility.GetPartConditionLabel (pawn, diffs.First<Hediff> ().get_Part ()).get_Second ());
				Widgets.Label (new Rect (num, curY + 1, num2, 100), new GUIContent (diffs.First<Hediff> ().get_Part ().def.get_LabelCap ()));
				stringBuilder.Append (" " + pawn.health.hediffSet.GetPartHealth (diffs.First<Hediff> ().get_Part ()).ToString () + " / " + diffs.First<Hediff> ().get_Part ().def.GetMaxHealth (pawn).ToString ());
			}
			else {
				stringBuilder.Append (Translator.Translate ("WholeBody"));
				GUI.set_color (HealthUtility.DarkRedColor);
				Widgets.Label (new Rect (num, curY + 1, num2, 100), new GUIContent (Translator.Translate ("WholeBody")));
			}
			GUI.set_color (Color.get_white ());
			stringBuilder.AppendLine ();
			stringBuilder.AppendLine ("------------------");
			foreach (IGrouping<int, Hediff> current2 in from x in diffs
			group x by x.get_UIGroupKey ()) {
				Hediff hediff = current2.First<Hediff> ();
				int num9 = 0;
				bool flag = false;
				Texture2D texture2D = null;
				foreach (Hediff current3 in current2) {
					num9++;
					Hediff_Injury hediff_Injury = current3 as Hediff_Injury;
					if (hediff_Injury != null && HediffUtility.IsTended (hediff_Injury) && !HediffUtility.IsOld (hediff_Injury)) {
						if (HediffUtility.IsTendedWell (hediff_Injury)) {
							texture2D = HealthCardUtility.TreatedWellIcon;
						}
						else {
							texture2D = HealthCardUtility.TreatedPoorIcon;
						}
					}
					float bleedRate = current3.get_BleedRate ();
					if ((double)bleedRate > 1E-05) {
						flag = true;
					}
					bool flag2 = HealthCardUtility.showHediffsDebugInfo && !GenText.NullOrEmpty (current3.get_DebugString ());
					string damageLabel = current3.get_DamageLabel ();
					if (!GenText.NullOrEmpty (current3.get_Label ()) || !GenText.NullOrEmpty (damageLabel) || !GenList.NullOrEmpty<PawnCapacityModifier> (current3.get_CapMods ()) || flag2) {
						stringBuilder.Append (current3.get_LabelCap ());
						if (!GenText.NullOrEmpty (damageLabel)) {
							stringBuilder.Append (": " + damageLabel);
						}
						stringBuilder.AppendLine ();
						stringBuilder.AppendLine (GenText.Indented (GenText.TrimEndNewlines (current3.get_TipString ())));
						if (flag2) {
							stringBuilder.AppendLine (GenText.Indented (GenText.TrimEndNewlines (current3.get_DebugString ())));
						}
					}
				}
				string text2 = hediff.get_LabelCap ();
				if (num9 != 1) {
					text2 = text2 + " x" + num9.ToString ();
				}
				GUI.set_color (hediff.get_LabelColor ());
				float num10 = Text.CalcHeight (text2, num3);
				Rect rect3 = new Rect (num + num2, curY + 1, num3, num10);
				Widgets.Label (rect3, text2);
				GUI.set_color (Color.get_white ());
				Rect rect4 = new Rect (rect2.get_xMax () - 20, curY, 20, 20);
				if (flag) {
					GUI.DrawTexture (rect4, HealthCardUtility.BleedingIcon);
				}
				if (texture2D != null) {
					GUI.DrawTexture (rect4, texture2D);
				}
				curY += num10;
			}
			GUI.set_color (Color.get_white ());
			curY = num6 + Mathf.Max (num4, num5);
			TooltipHandler.TipRegion (rect2, new TipSignal (stringBuilder.ToString ().TrimEnd (new char[0]), (int)curY + 7857));
		}

		public void DrawInjuries (Rect leftRect, Pawn pawn, bool showBloodLoss)
		{
			float num = 0;
			Rect rect = new Rect (leftRect.get_x (), num, leftRect.get_width (), leftRect.get_height () - num);
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (0, 0, rect.get_width (), rect.get_height ());
			Rect rect3 = new Rect (0, 0, rect.get_width () - 16, HealthCardUtility.scrollViewHeight);
			Rect rect4 = new Rect (rect.get_x (), rect.get_y (), rect.get_width () - 16, rect.get_height ());
			GUI.set_color (Color.get_white ());
			Widgets.BeginScrollView (rect2, ref HealthCardUtility.scrollPosition, rect3);
			float num2 = 0;
			HealthCardUtility.highlight = true;
			bool flag = false;
			foreach (IGrouping<BodyPartRecord, Hediff> current in ((IEnumerable<IGrouping<BodyPartRecord, Hediff>>)this.visibleHediffGroupsInOrderMethod.Invoke (null, new object[] {
				pawn,
				showBloodLoss
			}))) {
				flag = true;
				this.DrawHediffRow (rect4, pawn, current, ref num2);
			}
			if (!flag) {
				GUI.set_color (Color.get_gray ());
				Text.set_Anchor (1);
				Widgets.Label (new Rect (0, 0, rect.get_width (), 30), new GUIContent (Translator.Translate ("NoInjuries")));
				Text.set_Anchor (0);
			}
			if (Event.get_current ().get_type () == 8) {
				HealthCardUtility.scrollViewHeight = num2;
			}
			Widgets.EndScrollView ();
			GUI.EndGroup ();
			GUI.set_color (Color.get_white ());
		}

		private float DrawLeftRow (Rect leftRect, float curY, string leftLabel, string rightLabel, Color rightLabelColor, string tipLabel)
		{
			Rect rect = new Rect (0, curY, leftRect.get_width (), 20);
			if (rect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (HealthCardUtility.HighlightColor);
				GUI.DrawTexture (rect, TexUI.HighlightTex);
			}
			float num = leftRect.get_width () * 0.5;
			float num2 = leftRect.get_width () - num;
			GUI.set_color (TabDrawer.TextColor);
			float num3 = 4;
			Widgets.Label (new Rect (num3, curY, num - num3, 30), new GUIContent (leftLabel));
			GUI.set_color (rightLabelColor);
			TextAnchor anchor = Text.get_Anchor ();
			Text.set_Anchor (2);
			Widgets.Label (new Rect (num, curY, num2 - num3, 30), new GUIContent (rightLabel));
			if (tipLabel != null) {
				TooltipHandler.TipRegion (new Rect (0, curY, leftRect.get_width (), 20), new TipSignal (tipLabel));
			}
			Text.set_Anchor (anchor);
			curY += 20;
			return curY;
		}

		public Bill DrawListing (BillStack billStack, Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			GUI.BeginGroup (rect);
			Text.set_Font (1);
			Rect rect2 = new Rect (0, 0, 150, 29);
			if (billStack.get_Count () < 10) {
				if (Widgets.TextButton (rect2, Translator.Translate ("AddBill"), true, false)) {
					Find.get_WindowStack ().Add (new FloatMenu (recipeOptionsMaker.Invoke (), false));
				}
			}
			else {
				GUI.set_color (new Color (1, 1, 1, 0.3));
				Button.TextButton (rect2, Translator.Translate ("AddBill"), true, false, false);
			}
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Rect rect3 = new Rect (0, 35, rect.get_width (), rect.get_height () - 35);
			Rect rect4 = new Rect (0, 0, rect3.get_width () - 16, viewHeight);
			Widgets.BeginScrollView (rect3, ref scrollPosition, rect4);
			float num = 0;
			for (int i = 0; i < billStack.get_Count (); i++) {
				Bill bill = billStack.get_Item (i);
				Rect rect5 = BillDrawer.DrawMedicalBill (billStack, bill, 0, num, rect4.get_width (), i);
				if (!bill.get_DeletedOrDereferenced () && rect5.Contains (Event.get_current ().get_mousePosition ())) {
					result = bill;
				}
				num += rect5.get_height ();
				if (i < billStack.get_Count () - 1) {
					num += 6;
				}
			}
			if (Event.get_current ().get_type () == 8) {
				viewHeight = num;
			}
			Widgets.EndScrollView ();
			GUI.EndGroup ();
			return result;
		}

		public float DrawMedOperationsTab (Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate {
				List<FloatMenuOption> list = new List<FloatMenuOption> ();
				foreach (RecipeDef current in thingForMedBills.def.get_AllRecipes ()) {
					if (current.get_AvailableNow ()) {
						IEnumerable<ThingDef> enumerable = current.PotentiallyMissingIngredients (null);
						if (!enumerable.Any ((ThingDef x) => x.isBodyPartOrImplant)) {
							IEnumerable<BodyPartRecord> partsToApplyOn = current.get_Worker ().GetPartsToApplyOn (pawn, current);
							if (partsToApplyOn.Any<BodyPartRecord> ()) {
								foreach (BodyPartRecord current2 in partsToApplyOn) {
									RecipeDef localRecipe = current;
									BodyPartRecord localPart = current2;
									string text;
									if (localRecipe == RecipeDefOf.RemoveBodyPart) {
										text = HealthCardUtility.RemoveBodyPartSpecialLabel (pawn, current2);
									}
									else {
										text = localRecipe.get_LabelCap ();
									}
									if (!current.hideBodyPartNames) {
										text = text + " (" + current2.def.label + ")";
									}
									Action action = null;
									if (enumerable.Any<ThingDef> ()) {
										text += " (";
										bool flag = true;
										foreach (ThingDef current3 in enumerable) {
											if (!flag) {
												text += ", ";
											}
											flag = false;
											text += Translator.Translate ("MissingMedicalBillIngredient", new object[] {
												current3.label
											});
										}
										text += ")";
									}
									else {
										action = delegate {
											if (!Find.get_ListerPawns ().get_FreeColonists ().Any ((Pawn col) => localRecipe.PawnSatisfiesSkillRequirements (col))) {
												Bill.CreateNoPawnsWithSkillDialog (localRecipe);
											}
											Pawn pawn2 = thingForMedBills as Pawn;
											if (pawn2 != null && !RestUtility.InBed (pawn) && pawn.get_RaceProps ().get_Humanlike ()) {
												if (!Find.get_ListerBuildings ().allBuildingsColonist.Any ((Building x) => x is Building_Bed && (x as Building_Bed).get_Medical ())) {
													Messages.Message (Translator.Translate ("MessageNoMedicalBeds"), 4);
												}
											}
											Bill_Medical bill_Medical = new Bill_Medical (localRecipe);
											pawn2.get_BillStack ().AddBill (bill_Medical);
											bill_Medical.set_Part (localPart);
											if (pawn2.get_Faction () != null && !pawn2.get_Faction ().def.hidden && !FactionUtility.HostileTo (pawn2.get_Faction (), Faction.get_OfColony ()) && localRecipe.get_Worker ().IsViolationOnPawn (pawn2, localPart, Faction.get_OfColony ())) {
												Messages.Message (Translator.Translate ("MessageMedicalOperationWillAngerFaction", new object[] {
													pawn2.get_Faction ()
												}), 4);
											}
										};
									}
									list.Add (new FloatMenuOption (text, action, 1, null, null));
								}
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect (leftRect.get_x (), curY, leftRect.get_width (), leftRect.get_height () - curY - 3);
			this.DrawListing (pawn.get_BillStack (), rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		public float DrawOverviewTab (Rect leftRect, Pawn pawn, float curY, bool showBloodLoss)
		{
			curY += TabDrawer.DrawHeader (0, curY, leftRect.get_width (), Translator.Translate ("EdB.Status"), true, 0);
			curY += 4;
			Text.set_Font (1);
			float bleedingRate = pawn.health.hediffSet.get_BleedingRate ();
			if ((double)bleedingRate > 0.01) {
				string rightLabel = string.Concat (new string[] {
					GenText.ToStringPercent (bleedingRate),
					"/",
					Translator.Translate ("Day")
				});
				curY = this.DrawLeftRow (leftRect, curY, Translator.Translate ("BleedingRate"), rightLabel, HealthCardUtility.GetBleedingRateColor (bleedingRate), null);
			}
			if (pawn.def.race.isFlesh) {
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel (pawn);
				string tipLabel = Translator.Translate ("PainLevel") + ": " + (pawn.health.hediffSet.get_Pain () * 100).ToString ("F0") + "%";
				curY = this.DrawLeftRow (leftRect, curY, Translator.Translate ("PainLevel"), painLabel.get_First (), painLabel.get_Second (), tipLabel);
			}
			if (!pawn.get_Dead ()) {
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.get_Humanlike ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.get_Animal ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnAnimals
					select x;
				}
				else {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act) {
					if (PawnCapacityUtility.BodyCanEverDoActivity (pawn.get_RaceProps ().body, current)) {
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel (pawn, current);
						string tipLabel2 = Translator.Translate ("Efficiency") + ": " + (pawn.health.capacities.GetEfficiency (current) * 100).ToString ("F0") + "%";
						curY = this.DrawLeftRow (leftRect, curY, GenText.CapitalizeFirst (current.GetLabelFor (pawn.get_RaceProps ().isFlesh, pawn.get_RaceProps ().get_Humanlike ())), efficiencyLabel.get_First (), efficiencyLabel.get_Second (), tipLabel2);
					}
				}
			}
			curY += 16;
			curY += TabDrawer.DrawHeader (0, curY, leftRect.get_width (), Translator.Translate ("EdB.Injuries"), true, 0);
			curY += 4;
			Rect rect = new Rect (leftRect.get_x (), curY, leftRect.get_width (), leftRect.get_height () - curY);
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (0, 0, rect.get_width (), rect.get_height ());
			Rect rect3 = new Rect (0, 0, rect.get_width () - 16, HealthCardUtility.scrollViewHeight);
			Rect rect4 = new Rect (rect.get_x (), rect.get_y (), rect.get_width () - 16, rect.get_height ());
			GUI.set_color (Color.get_white ());
			Widgets.BeginScrollView (rect2, ref HealthCardUtility.scrollPosition, rect3);
			float num = 0;
			HealthCardUtility.highlight = true;
			bool flag = false;
			foreach (IGrouping<BodyPartRecord, Hediff> current2 in ((IEnumerable<IGrouping<BodyPartRecord, Hediff>>)this.visibleHediffGroupsInOrderMethod.Invoke (null, new object[] {
				pawn,
				showBloodLoss
			}))) {
				flag = true;
				this.DrawHediffRow (rect4, pawn, current2, ref num);
			}
			if (!flag) {
				GUI.set_color (Color.get_gray ());
				Text.set_Anchor (1);
				Widgets.Label (new Rect (0, 0, rect.get_width (), 30), new GUIContent (Translator.Translate ("NoInjuries")));
				Text.set_Anchor (0);
			}
			if (Event.get_current ().get_type () == 8) {
				HealthCardUtility.scrollViewHeight = num;
			}
			Widgets.EndScrollView ();
			GUI.EndGroup ();
			GUI.set_color (Color.get_white ());
			return curY;
		}

		public float DrawOverviewTab (Rect leftRect, Pawn pawn, float curY)
		{
			curY += 4;
			if (pawn.playerSettings != null && !pawn.get_Dead ()) {
				Rect rect = new Rect (0, curY, 140, 28);
				MedicalCareUtility.MedicalCareSetter (rect, ref pawn.playerSettings.medCare);
				curY += 32;
			}
			Text.set_Font (1);
			if (pawn.def.race.isFlesh) {
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel (pawn);
				string tipLabel = Translator.Translate ("PainLevel") + ": " + (pawn.health.hediffSet.get_Pain () * 100).ToString ("F0") + "%";
				curY = this.DrawLeftRow (leftRect, curY, Translator.Translate ("PainLevel"), painLabel.get_First (), painLabel.get_Second (), tipLabel);
			}
			if (!pawn.get_Dead ()) {
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.get_Humanlike ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.get_Animal ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnAnimals
					select x;
				}
				else {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act) {
					if (PawnCapacityUtility.BodyCanEverDoActivity (pawn.get_RaceProps ().body, current)) {
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel (pawn, current);
						string tipLabel2 = Translator.Translate ("Efficiency") + ": " + (pawn.health.capacities.GetEfficiency (current) * 100).ToString ("F0") + "%";
						curY = this.DrawLeftRow (leftRect, curY, GenText.CapitalizeFirst (current.GetLabelFor (pawn.get_RaceProps ().isFlesh, pawn.get_RaceProps ().get_Humanlike ())), efficiencyLabel.get_First (), efficiencyLabel.get_Second (), tipLabel2);
					}
				}
			}
			return curY;
		}

		public float DrawStatus (Rect leftRect, Pawn pawn, float curY, bool showBloodLoss)
		{
			Text.set_Font (1);
			float bleedingRate = pawn.health.hediffSet.get_BleedingRate ();
			if ((double)bleedingRate > 0.01) {
				string rightLabel = string.Concat (new string[] {
					GenText.ToStringPercent (bleedingRate),
					"/",
					Translator.Translate ("Day")
				});
				curY = this.DrawLeftRow (leftRect, curY, Translator.Translate ("BleedingRate"), rightLabel, HealthCardUtility.GetBleedingRateColor (bleedingRate), null);
			}
			if (pawn.def.race.isFlesh) {
				Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel (pawn);
				string tipLabel = Translator.Translate ("PainLevel") + ": " + (pawn.health.hediffSet.get_Pain () * 100).ToString ("F0") + "%";
				curY = this.DrawLeftRow (leftRect, curY, Translator.Translate ("PainLevel"), painLabel.get_First (), painLabel.get_Second (), tipLabel);
			}
			if (!pawn.get_Dead ()) {
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.get_Humanlike ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.get_Animal ()) {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnAnimals
					select x;
				}
				else {
					source = from x in DefDatabase<PawnCapacityDef>.get_AllDefs ()
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act) {
					if (PawnCapacityUtility.BodyCanEverDoActivity (pawn.get_RaceProps ().body, current)) {
						Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel (pawn, current);
						string tipLabel2 = Translator.Translate ("Efficiency") + ": " + (pawn.health.capacities.GetEfficiency (current) * 100).ToString ("F0") + "%";
						curY = this.DrawLeftRow (leftRect, curY, GenText.CapitalizeFirst (current.GetLabelFor (pawn.get_RaceProps ().isFlesh, pawn.get_RaceProps ().get_Humanlike ())), efficiencyLabel.get_First (), efficiencyLabel.get_Second (), tipLabel2);
					}
				}
			}
			return curY;
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public interface IComponentWithPreferences
	{
		//
		// Properties
		//
		IEnumerable<IPreference> Preferences {
			get;
		}
	}
}
using System;

namespace EdB.Interface
{
	public interface ICustomTextureComponent
	{
		//
		// Methods
		//
		void ResetTextures ();
	}
}
using System;

namespace EdB.Interface
{
	public interface IInitializedComponent
	{
		//
		// Methods
		//
		void Initialize (UserInterface userInterface);

		void PrepareDependencies (UserInterface userInterface);
	}
}
using System;

namespace EdB.Interface
{
	public interface INamedComponent
	{
		//
		// Properties
		//
		string Name {
			get;
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal static class InspectGizmoGrid
	{
		//
		// Static Fields
		//
		private static List<object> objList = new List<object> ();

		private static List<Gizmo> gizmoList = new List<Gizmo> ();

		//
		// Static Methods
		//
		public static void DrawInspectGizmoGridFor (IEnumerable<object> selectedObjects)
		{
			try {
				InspectGizmoGrid.objList.Clear ();
				InspectGizmoGrid.objList.AddRange (selectedObjects);
				InspectGizmoGrid.gizmoList.Clear ();
				for (int i = 0; i < InspectGizmoGrid.objList.Count; i++) {
					ISelectable selectable = InspectGizmoGrid.objList [i] as ISelectable;
					if (selectable != null) {
						foreach (Gizmo current in selectable.GetGizmos ()) {
							InspectGizmoGrid.gizmoList.Add (current);
						}
					}
				}
				for (int j = 0; j < InspectGizmoGrid.objList.Count; j++) {
					Thing t = InspectGizmoGrid.objList [j] as Thing;
					if (t != null) {
						List<Designator> allDesignators = ReverseDesignatorDatabase.get_AllDesignators ();
						for (int k = 0; k < allDesignators.Count; k++) {
							Designator des = allDesignators [k];
							if (des.CanDesignateThing (t).get_Accepted ()) {
								Command_Action command_Action = new Command_Action ();
								command_Action.defaultLabel = des.LabelCapReverseDesignating (t);
								command_Action.icon = des.IconReverseDesignating (t);
								command_Action.defaultDesc = des.DescReverseDesignating (t);
								command_Action.action = delegate {
									des.DesignateThing (t);
									des.Finalize (true);
								};
								command_Action.hotKey = des.hotKey;
								command_Action.groupKey = des.groupKey;
								InspectGizmoGrid.gizmoList.Add (command_Action);
							}
						}
					}
				}
				IEnumerable<Gizmo> arg_1F5_0 = InspectGizmoGrid.gizmoList;
				Vector2 paneSize = MainTabWindow_Inspect.PaneSize;
				Gizmo gizmo;
				GizmoGridDrawer.DrawGizmoGrid (arg_1F5_0, paneSize.x + 20, out gizmo);
			}
			catch (Exception ex) {
				Log.ErrorOnce (ex.ToString (), 3427734);
			}
		}
	}
}
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal class InspectPaneFiller
	{
		//
		// Static Fields
		//
		private const float BarHeight = 16;

		private static bool debug_inspectStringExceptionErrored;

		private static bool debug_inspectLengthWarned;

		private static readonly Texture2D MoodTex;

		private static readonly Texture2D HealthTex;

		private static readonly Texture2D BarBGTex;

		private const float BarSpacing = 6;

		private const float BarWidth = 93;

		//
		// Constructors
		//
		static InspectPaneFiller ()
		{
			ColorInt colorInt = new ColorInt (10, 10, 10);
			InspectPaneFiller.BarBGTex = SolidColorMaterials.NewSolidColorTexture (colorInt.get_ToColor ());
			ColorInt colorInt2 = new ColorInt (35, 35, 35);
			InspectPaneFiller.HealthTex = SolidColorMaterials.NewSolidColorTexture (colorInt2.get_ToColor ());
			ColorInt colorInt3 = new ColorInt (26, 52, 52);
			InspectPaneFiller.MoodTex = SolidColorMaterials.NewSolidColorTexture (colorInt3.get_ToColor ());
			InspectPaneFiller.debug_inspectLengthWarned = false;
			InspectPaneFiller.debug_inspectStringExceptionErrored = false;
		}

		//
		// Static Methods
		//
		public static void DoPaneContentsFor (ISelectable sel, Rect rect)
		{
			try {
				GUI.BeginGroup (rect);
				float num = 0;
				Thing thing = sel as Thing;
				Pawn pawn = sel as Pawn;
				if (thing != null) {
					num += 3;
					WidgetRow row = new WidgetRow (0, num, 2, 2000, 29);
					InspectPaneFiller.DrawHealth (row, thing);
					if (pawn != null) {
						InspectPaneFiller.DrawMood (row, pawn);
						if (pawn.timetable != null) {
							InspectPaneFiller.DrawTimetableSetting (row, pawn);
						}
						InspectPaneFiller.DrawAreaAllowed (row, pawn);
					}
					num += 18;
				}
				InspectPaneFiller.DrawInspectStringFor (sel, ref num);
			}
			catch (Exception ex) {
				Log.ErrorOnce (string.Concat (new object[] {
					"Error in DoPaneContentsFor ",
					Find.get_Selector ().get_FirstSelectedObject (),
					": ",
					ex.ToString ()
				}), 754672);
			}
			finally {
				GUI.EndGroup ();
			}
		}

		private static void DrawAreaAllowed (WidgetRow row, Pawn pawn)
		{
			if (pawn.playerSettings == null || pawn.get_HostFaction () != null) {
				return;
			}
			row.DoGap (6);
			bool flag = pawn.playerSettings != null && pawn.playerSettings.get_AreaRestriction () != null;
			Texture2D texture2D;
			if (flag) {
				texture2D = pawn.playerSettings.get_AreaRestriction ().get_ColorTexture ();
			}
			else {
				texture2D = BaseContent.GreyTex;
			}
			Rect rect = row.DoBar (93, 16, 1, AreaUtility.AreaAllowedLabel (pawn), texture2D, null);
			if (Mouse.IsOver (rect)) {
				if (flag) {
					pawn.playerSettings.get_AreaRestriction ().MarkForDraw ();
				}
				Rect rect2 = GenUI.ContractedBy (rect, -1);
				Widgets.DrawBox (rect2, 1);
			}
			if (Widgets.InvisibleButton (rect)) {
				AllowedAreaMode allowedAreaMode = pawn.get_RaceProps ().get_Humanlike () ? 1 : 2;
				AreaUtility.MakeAllowedAreaListFloatMenu (delegate (Area a) {
					pawn.playerSettings.set_AreaRestriction (a);
				}, allowedAreaMode, true, true);
			}
		}

		public static void DrawHealth (WidgetRow row, Thing t)
		{
			Pawn pawn = t as Pawn;
			float num;
			string text;
			if (pawn == null) {
				if (!t.def.useHitPoints) {
					return;
				}
				if (t.get_HitPoints () >= t.get_MaxHitPoints ()) {
					GUI.set_color (Color.get_white ());
				}
				else if ((double)((float)t.get_HitPoints ()) > (double)((float)t.get_MaxHitPoints ()) * 0.5) {
					GUI.set_color (Color.get_yellow ());
				}
				else if (t.get_HitPoints () > 0) {
					GUI.set_color (Color.get_red ());
				}
				else {
					GUI.set_color (Color.get_grey ());
				}
				num = (float)t.get_HitPoints () / (float)t.get_MaxHitPoints ();
				text = GenString.ToStringCached (t.get_HitPoints ()) + " / " + GenString.ToStringCached (t.get_MaxHitPoints ());
			}
			else {
				GUI.set_color (Color.get_white ());
				num = pawn.health.summaryHealth.get_SummaryHealthPercent ();
				text = HealthUtility.GetGeneralConditionLabel (pawn);
			}
			row.DoBar (93, 16, num, text, InspectPaneFiller.HealthTex, InspectPaneFiller.BarBGTex);
			GUI.set_color (Color.get_white ());
		}

		public static void DrawInspectStringFor (ISelectable t, ref float y)
		{
			string text;
			try {
				text = t.GetInspectString ();
			}
			catch (Exception ex) {
				text = "GetInspectString exception on " + t.ToString () + ":
" + ex.ToString ();
				if (!InspectPaneFiller.debug_inspectStringExceptionErrored) {
					Log.Error (text);
					InspectPaneFiller.debug_inspectStringExceptionErrored = true;
				}
			}
			Text.set_Font (1);
			float arg_65_1 = 0;
			float arg_65_2 = y;
			Vector2 paneInnerSize = MainTabWindow_Inspect.PaneInnerSize;
			Rect rect = new Rect (arg_65_1, arg_65_2, paneInnerSize.x, 100);
			Widgets.Label (rect, text);
			if (Prefs.get_DevMode ()) {
				text = text.Trim ();
				if (!InspectPaneFiller.debug_inspectLengthWarned) {
					if (text.Count ((char f) => f == '
') > 5) {
						Log.ErrorOnce (string.Concat (new object[] {
							t,
							" gave an inspect string over six lines (some may be empty):
",
							text,
							"END"
						}), 778772);
						InspectPaneFiller.debug_inspectLengthWarned = true;
					}
				}
			}
		}

		private static void DrawMood (WidgetRow row, Pawn pawn)
		{
			if (pawn.needs.mood == null) {
				return;
			}
			row.DoGap (6);
			row.DoBar (93, 16, pawn.needs.mood.get_CurLevel (), GenText.CapitalizeFirst (pawn.needs.mood.get_MoodString ()), InspectPaneFiller.MoodTex, InspectPaneFiller.BarBGTex);
		}

		private static void DrawTimetableSetting (WidgetRow row, Pawn pawn)
		{
			row.DoGap (6);
			row.DoBar (93, 16, 1, pawn.timetable.get_CurrentAssignment ().get_LabelCap (), pawn.timetable.get_CurrentAssignment ().get_ColorTexture (), null);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class IntegerOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler (int value);

		//
		// Static Fields
		//
		public static float LabelMargin = IntegerOptionsPreference.RadioButtonWidth + IntegerOptionsPreference.RadioButtonMargin;

		public static float RadioButtonMargin = 18;

		public static float RadioButtonWidth = 24;

		//
		// Fields
		//
		private string stringValue;

		private int? intValue;

		public int tooltipId;

		//
		// Properties
		//
		public abstract int DefaultValue {
			get;
		}

		public virtual bool Disabled {
			get {
				return false;
			}
		}

		public virtual bool DisplayInOptions {
			get {
				return true;
			}
		}

		public abstract string Group {
			get;
		}

		public virtual bool Indent {
			get {
				return false;
			}
		}

		public virtual string Label {
			get {
				return Translator.Translate (this.Name);
			}
		}

		public abstract string Name {
			get;
		}

		public abstract string OptionValuePrefix {
			get;
		}

		public abstract IEnumerable<int> OptionValues {
			get;
		}

		public virtual string Tooltip {
			get {
				return null;
			}
		}

		protected virtual int TooltipId {
			get {
				if (this.tooltipId == 0) {
					this.tooltipId = Translator.Translate (this.Tooltip).GetHashCode ();
					return this.tooltipId;
				}
				return 0;
			}
		}

		public virtual int Value {
			get {
				int? num = this.intValue;
				if (num.HasValue) {
					return this.intValue.Value;
				}
				return this.DefaultValue;
			}
			set {
				int? num = this.intValue;
				this.intValue = new int? (value);
				this.stringValue = value.ToString ();
				if ((!num.HasValue || num.Value != this.intValue) && this.ValueChanged != null) {
					this.ValueChanged (value);
				}
			}
		}

		public virtual int ValueForDisplay {
			get {
				int? num = this.intValue;
				if (num.HasValue) {
					return this.intValue.Value;
				}
				return this.DefaultValue;
			}
		}

		public string ValueForSerialization {
			get {
				return this.stringValue;
			}
			set {
				int value2;
				if (int.TryParse (value, out value2) && this.OptionValues.Contains (value2)) {
					this.stringValue = value;
					this.intValue = new int? (value2);
					return;
				}
				this.intValue = null;
				this.stringValue = null;
			}
		}

		//
		// Constructors
		//
		public IntegerOptionsPreference ()
		{
		}

		//
		// Methods
		//
		public void OnGUI (float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			if (disabled) {
				GUI.set_color (Dialog_InterfaceOptions.DisabledControlColor);
			}
			float num = (!this.Indent) ? 0 : Dialog_InterfaceOptions.IndentSize;
			foreach (int current in this.OptionValues) {
				string text = Translator.Translate (this.OptionValuePrefix + "." + current);
				float num2 = Text.CalcHeight (text, width - IntegerOptionsPreference.LabelMargin - num);
				Rect rect = new Rect (positionX - 4 + num, positionY - 3, width + 6 - num, num2 + 5);
				if (Mouse.IsOver (rect)) {
					Widgets.DrawHighlight (rect);
				}
				Rect rect2 = new Rect (positionX + num, positionY, width - IntegerOptionsPreference.LabelMargin - num, num2);
				GUI.Label (rect2, text);
				if (this.Tooltip != null) {
					TipSignal tipSignal = new TipSignal (() => Translator.Translate (this.Tooltip), this.TooltipId);
					TooltipHandler.TipRegion (rect2, tipSignal);
				}
				int valueForDisplay = this.ValueForDisplay;
				bool flag = valueForDisplay == current;
				if (Widgets.RadioButton (new Vector2 (positionX + width - IntegerOptionsPreference.RadioButtonWidth, positionY - 3), flag) && !disabled) {
					this.Value = current;
				}
				positionY += num2 + Dialog_InterfaceOptions.PreferencePadding.y;
			}
			positionY -= Dialog_InterfaceOptions.PreferencePadding.y;
			GUI.set_color (Color.get_white ());
		}

		//
		// Events
		//
		public event IntegerOptionsPreference.ValueChangedHandler ValueChanged {
			add {
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<IntegerOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (IntegerOptionsPreference.ValueChangedHandler)Delegate.Combine (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove {
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<IntegerOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (IntegerOptionsPreference.ValueChangedHandler)Delegate.Remove (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class InventoryCount
	{
		//
		// Fields
		//
		public InventoryRecordKey key = new InventoryRecordKey ();

		public int count;

		public int deepStorageCount;

		//
		// Constructors
		//
		public InventoryCount (ThingDef thingDef, int count, bool isDeepStorage = false)
		{
			this.key.ThingDef = thingDef;
			this.key.StuffDef = null;
			this.count = (isDeepStorage ? 0 : count);
			this.deepStorageCount = ((!isDeepStorage) ? 0 : count);
		}

		public InventoryCount (ThingDef thingDef, ThingDef stuffDef, int count, bool isDeepStorage = false)
		{
			this.key.ThingDef = thingDef;
			this.key.StuffDef = stuffDef;
			this.count = (isDeepStorage ? 0 : count);
			this.deepStorageCount = ((!isDeepStorage) ? 0 : count);
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public interface InventoryCounter
	{
		//
		// Methods
		//
		InventoryRecord CountThing (Thing thing, Dictionary<InventoryRecordKey, InventoryRecord> recordLookup, InventoryState state, out bool equippable);
	}
}
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI;

namespace EdB.Interface
{
	public class InventoryCounterDefault : InventoryCounter
	{
		//
		// Fields
		//
		public HashSet<Thing> reservedThings = new HashSet<Thing> ();

		protected FieldInfo reservationsField;

		//
		// Constructors
		//
		public InventoryCounterDefault ()
		{
			this.reservationsField = typeof(ReservationManager).GetField ("reservations", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Methods
		//
		public InventoryRecord CountThing (Thing thing, Dictionary<InventoryRecordKey, InventoryRecord> recordLookup, InventoryState state, out bool equippable)
		{
			equippable = false;
			if (thing == null) {
				Log.Warning ("Tried to count a null thing");
				return null;
			}
			ThingDef thingDef = thing.def;
			if (thingDef == null) {
				Log.Warning ("Failed to count thing.  Its definition is null.");
				return null;
			}
			ThingDef thingDef2 = null;
			if (thingDef.building != null && thingDef.entityDefToBuild != null) {
				thingDef2 = thingDef;
				thingDef = (thingDef.entityDefToBuild as ThingDef);
				if (thingDef == null) {
					return null;
				}
			}
			Thing thing2 = MinifyUtility.GetInnerIfMinified (thing);
			if (thing2 == thing) {
				thing2 = null;
			}
			if (thing2 != null) {
				thing = thing2;
				thingDef = thing2.def;
				if (thingDef == null) {
					return null;
				}
			}
			InventoryRecord inventoryRecord = null;
			if (!recordLookup.TryGetValue (new InventoryRecordKey (thingDef, thing.get_Stuff ()), out inventoryRecord)) {
				if (thing.get_Stuff () == null) {
					Log.Warning ("Did not find record for " + thingDef.defName);
				}
				else {
					Log.Warning ("Did not find record for " + thingDef.defName + " made out of " + thing.get_Stuff ().defName);
				}
				return null;
			}
			if (inventoryRecord.type == InventoryType.UNKNOWN) {
				return null;
			}
			if (thingDef2 == null && thing2 == null) {
				inventoryRecord.count += thing.stackCount;
			}
			else {
				inventoryRecord.unfinishedCount += thing.stackCount;
			}
			if (thingDef.building == null) {
				TextureColorPair textureColorPair = MaterialResolver.Resolve (thing);
				inventoryRecord.texture = textureColorPair.texture;
				inventoryRecord.color = thing.get_DrawColor ();
			}
			else {
				inventoryRecord.color = thing.get_DrawColor ();
			}
			if (thingDef.equipmentType != null || thingDef.apparel != null) {
				bool flag = ThingSelectionUtility.SelectableNow (thing);
				bool flag2 = this.reservedThings.Contains (thing);
				if (flag && !flag2) {
					if (inventoryRecord.availableCount == -1) {
						inventoryRecord.availableCount = thing.stackCount;
					}
					else {
						inventoryRecord.availableCount += thing.stackCount;
					}
					equippable = true;
				}
				else if (inventoryRecord.availableCount == -1) {
					inventoryRecord.availableCount = 0;
				}
				if (flag2) {
					inventoryRecord.reservedCount++;
				}
			}
			return inventoryRecord;
		}

		public void Prepare ()
		{
			this.reservedThings.Clear ();
			ReservationManager reservations = Find.get_Reservations ();
			IList list = this.reservationsField.GetValue (reservations) as IList;
			if (list == null) {
				return;
			}
			int count = list.Count;
			for (int i = 0; i < count; i++) {
				object obj = list [i];
				TargetInfo targetInfo = (TargetInfo)obj.GetType ().GetField ("target", BindingFlags.Instance | BindingFlags.NonPublic).GetValue (obj);
				if (targetInfo != null && targetInfo.get_HasThing ()) {
					this.reservedThings.Add (targetInfo.get_Thing ());
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryManager
	{
		//
		// Fields
		//
		protected Dictionary<InventoryRecordKey, InventoryRecord> allRecords = new Dictionary<InventoryRecordKey, InventoryRecord> ();

		protected bool initialized;

		protected ThingCategoryDef schematicDef;

		protected EquippableThings equippableThings = new EquippableThings ();

		protected InventoryCounterDefault defaultInventoryCounter = new InventoryCounterDefault ();

		protected InventoryState state = new InventoryState ();

		protected Dictionary<ThingDef, InventoryCounter> thingCounters = new Dictionary<ThingDef, InventoryCounter> ();

		protected Dictionary<string, InventoryResolver> inventoryResolvers = new Dictionary<string, InventoryResolver> ();

		protected Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>> categorizedRecords = new Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>> ();

		//
		// Properties
		//
		public bool CompressedStorage {
			get {
				return this.state.compressedStorage;
			}
			set {
				this.state.compressedStorage = value;
			}
		}

		public EquippableThings EquippableThings {
			get {
				return this.equippableThings;
			}
		}

		public PreferenceCompressedStorage PreferenceCompressedStorage {
			set {
				this.state.prefs.CompressedStorage = value;
			}
		}

		public PreferenceIncludeUnfinished PreferenceIncludeUnfinished {
			set {
				this.state.prefs.IncludeUnfinished = value;
			}
		}

		public InventoryPreferences Preferences {
			get {
				return this.state.prefs;
			}
		}

		//
		// Methods
		//
		protected InventoryType CategorizeDefinition (ThingDef def)
		{
			if (def.category == 6) {
				return InventoryType.UNKNOWN;
			}
			if (def.thingCategories != null) {
				if (def.thingCategories.FirstOrDefault ((ThingCategoryDef d) => d.defName.StartsWith ("Corpses")) != null) {
					return InventoryType.UNKNOWN;
				}
			}
			if (def.get_IsFrame ()) {
				return InventoryType.UNKNOWN;
			}
			if (def.building != null) {
				if ("Furniture".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_FURNITURE;
				}
				if ("Structure".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_STRUCTURE;
				}
				if ("Power".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_POWER;
				}
				if ("Production".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_PRODUCTION;
				}
				if ("Security".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_SECURITY;
				}
				if ("Ship".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_SHIP;
				}
				if ("Joy".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_JOY;
				}
				if ("Temperature".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_TEMPERATURE;
				}
				if ("FoodUtilities".Equals (def.designationCategory)) {
					return InventoryType.BUILDING_FOOD_UTILITIES;
				}
				if (def.BelongsToCategory ("Joy") || (def.graphic.path != null && def.graphic.path.IndexOf ("/Joy/") != -1)) {
					return InventoryType.BUILDING_JOY;
				}
				return InventoryType.BUILDING_OTHER;
			}
			else {
				if (def.apparel != null) {
					return InventoryType.ITEM_APPAREL;
				}
				if (def.weaponTags != null && def.weaponTags.Count > 0) {
					return InventoryType.ITEM_EQUIPMENT;
				}
				if (def.ingestible != null) {
					return InventoryType.ITEM_FOOD;
				}
				if (!def.get_CountAsResource ()) {
					return InventoryType.ITEM_OTHER;
				}
				if (this.schematicDef != null && def.thingCategories != null && def.thingCategories.Contains (this.schematicDef)) {
					return InventoryType.ITEM_SCHEMATIC;
				}
				if (def.get_CountAsResource ()) {
					return InventoryType.ITEM_RESOURCE;
				}
				return InventoryType.UNKNOWN;
			}
		}

		protected void CategorizeDefinitions ()
		{
			foreach (InventoryRecordKey current in this.allRecords.Keys) {
				try {
					InventoryType type = this.CategorizeDefinition (current.ThingDef);
					this.allRecords [current].type = type;
				}
				catch (Exception ex) {
					Log.Error ("Failed to categorize thing definitions for EdB Interface inventory dialog.  Failed on " + current.ThingDef.defName);
					throw ex;
				}
			}
		}

		protected void ClearInventory ()
		{
			foreach (Dictionary<InventoryRecordKey, InventoryRecord> current in this.categorizedRecords.Values) {
				current.Clear ();
			}
			foreach (InventoryRecord current2 in this.allRecords.Values) {
				current2.ResetCounts ();
			}
			this.state.compressedStorage = false;
			this.state.categorizedRecords = this.categorizedRecords;
			this.equippableThings.Reset ();
			this.defaultInventoryCounter.Prepare ();
		}

		protected void Count (Thing thing)
		{
			if (thing == null) {
				return;
			}
			InventoryRecord inventoryRecord = null;
			bool flag = false;
			InventoryCounter inventoryCounter;
			if (this.thingCounters.TryGetValue (thing.def, out inventoryCounter)) {
				inventoryRecord = inventoryCounter.CountThing (thing, this.allRecords, this.state, out flag);
			}
			if (inventoryRecord == null) {
				inventoryRecord = this.defaultInventoryCounter.CountThing (thing, this.allRecords, this.state, out flag);
			}
			if (inventoryRecord == null) {
				return;
			}
			if (flag) {
				this.equippableThings.Add (new InventoryRecordKey (inventoryRecord.thingDef, inventoryRecord.stuffDef), thing);
			}
			Dictionary<InventoryRecordKey, InventoryRecord> dictionary;
			if (this.categorizedRecords.TryGetValue (inventoryRecord.type, out dictionary)) {
				InventoryRecordKey key = new InventoryRecordKey (inventoryRecord.thingDef, inventoryRecord.stuffDef);
				if (!dictionary.ContainsKey (key)) {
					dictionary.Add (key, inventoryRecord);
				}
			}
		}

		protected void Count (InventoryCount count)
		{
			InventoryRecordKey key = count.key;
			ThingDef thingDef = key.ThingDef;
			ThingDef thingDef2 = null;
			if (thingDef.building != null && thingDef.entityDefToBuild != null) {
				thingDef2 = key.ThingDef;
				thingDef = (thingDef.entityDefToBuild as ThingDef);
				if (thingDef == null) {
					return;
				}
			}
			InventoryRecord inventoryRecord;
			if (!this.allRecords.TryGetValue (key, out inventoryRecord)) {
				return;
			}
			if (inventoryRecord.type == InventoryType.UNKNOWN) {
				return;
			}
			if (thingDef2 == null) {
				inventoryRecord.count += count.count;
				inventoryRecord.compressedCount += count.deepStorageCount;
			}
			else {
				inventoryRecord.unfinishedCount += count.count;
			}
			if (thingDef.building == null && inventoryRecord.texture == null) {
				TextureColorPair textureColorPair = MaterialResolver.Resolve (thingDef);
				if (textureColorPair != null) {
					inventoryRecord.texture = textureColorPair.texture;
					inventoryRecord.color = textureColorPair.color;
				}
			}
			Dictionary<InventoryRecordKey, InventoryRecord> dictionary = this.categorizedRecords [inventoryRecord.type];
			if (!dictionary.ContainsKey (key)) {
				dictionary.Add (key, inventoryRecord);
			}
			if (count.deepStorageCount > 0) {
				this.state.compressedStorage = true;
			}
		}

		protected void CreateCategories ()
		{
			this.categorizedRecords.Add (InventoryType.ITEM_RESOURCE, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.ITEM_FOOD, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.ITEM_OTHER, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.ITEM_APPAREL, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.ITEM_SCHEMATIC, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.ITEM_EQUIPMENT, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_OTHER, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_FURNITURE, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_SECURITY, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_STRUCTURE, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_POWER, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_SHIP, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_PRODUCTION, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_JOY, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_TEMPERATURE, new Dictionary<InventoryRecordKey, InventoryRecord> ());
			this.categorizedRecords.Add (InventoryType.BUILDING_FOOD_UTILITIES, new Dictionary<InventoryRecordKey, InventoryRecord> ());
		}

		public List<InventoryRecord> GetInventoryRecord (InventoryType type)
		{
			if (type != InventoryType.UNKNOWN) {
				try {
					List<InventoryRecord> result = this.categorizedRecords [type].Values.ToList<InventoryRecord> ();
					return result;
				}
				catch (KeyNotFoundException) {
					List<InventoryRecord> result = null;
					return result;
				}
			}
			return null;
		}

		protected void Initialize ()
		{
			this.PrepareAllDefinitions ();
			this.CategorizeDefinitions ();
			this.CreateCategories ();
			this.initialized = true;
		}

		protected void LogInventory ()
		{
			foreach (InventoryRecord current in this.allRecords.Values) {
				if (current.count != 0) {
					Log.Message (GenLabel.ThingLabel (current.thingDef, current.stuffDef, current.count));
				}
			}
		}

		protected void PrepareAllDefinitions ()
		{
			this.schematicDef = DefDatabase<ThingCategoryDef>.GetNamedSilentFail ("Schematic");
			Dictionary<StuffCategoryDef, HashSet<ThingDef>> dictionary = new Dictionary<StuffCategoryDef, HashSet<ThingDef>> ();
			foreach (ThingDef current in DefDatabase<ThingDef>.get_AllDefs ()) {
				if (current.get_IsStuff () && current.stuffProps != null) {
					foreach (StuffCategoryDef current2 in current.stuffProps.categories) {
						HashSet<ThingDef> hashSet = null;
						if (!dictionary.TryGetValue (current2, out hashSet)) {
							hashSet = new HashSet<ThingDef> ();
							dictionary.Add (current2, hashSet);
						}
						hashSet.Add (current);
					}
				}
			}
			foreach (ThingDef current3 in DefDatabase<ThingDef>.get_AllDefs ()) {
				if (current3.get_MadeFromStuff ()) {
					if (current3.stuffCategories != null) {
						foreach (StuffCategoryDef current4 in current3.stuffCategories) {
							HashSet<ThingDef> hashSet2;
							if (dictionary.TryGetValue (current4, out hashSet2)) {
								foreach (ThingDef current5 in hashSet2) {
									try {
										InventoryRecord inventoryRecord = new InventoryRecord ();
										inventoryRecord.thingDef = current3;
										inventoryRecord.stuffDef = current5;
										inventoryRecord.count = 0;
										inventoryRecord.color = new Color (1, 1, 1);
										inventoryRecord.texture = null;
										this.PrepareBuildingRecord (inventoryRecord);
										this.allRecords [new InventoryRecordKey (current3, current5)] = inventoryRecord;
									}
									catch (Exception ex) {
										Log.Error (string.Concat (new string[] {
											"Failed to prepare thing definitions for EdB Interface inventory dialog.  Failed on ",
											current3.defName,
											" with ",
											current5.defName,
											" stuff"
										}));
										throw ex;
									}
								}
							}
						}
					}
				}
				else {
					try {
						InventoryRecord inventoryRecord2 = new InventoryRecord ();
						inventoryRecord2.thingDef = current3;
						inventoryRecord2.stuffDef = null;
						inventoryRecord2.count = 0;
						inventoryRecord2.color = new Color (1, 1, 1);
						inventoryRecord2.texture = null;
						this.PrepareBuildingRecord (inventoryRecord2);
						this.allRecords [new InventoryRecordKey (current3, null)] = inventoryRecord2;
					}
					catch (Exception ex2) {
						Log.Error ("Failed to prepare thing definitions for EdB Interface inventory dialog.  Failed on " + current3.defName);
						throw ex2;
					}
				}
			}
		}

		protected void PrepareBuildingRecord (InventoryRecord record)
		{
			ThingDef thingDef = record.thingDef;
			if (thingDef.entityDefToBuild == null) {
				record.texture = thingDef.uiIcon;
			}
			else {
				record.texture = thingDef.entityDefToBuild.uiIcon;
			}
			if (thingDef.graphic != null) {
				record.proportions = new Vector2 (thingDef.graphic.drawSize.x / thingDef.graphic.drawSize.y, 1);
				float num = (thingDef.graphic.drawSize.x <= thingDef.graphic.drawSize.y) ? thingDef.graphic.drawSize.y : thingDef.graphic.drawSize.x;
				float num2 = (float)((thingDef.size.x <= thingDef.size.z) ? thingDef.size.z : thingDef.size.x);
				float num3 = 0.5;
				float num4 = 1.75;
				float num5 = num / num2;
				float buildingScale = (num5 - num3) / num4 + num3;
				record.buildingScale = buildingScale;
			}
			else {
				record.proportions = new Vector2 (1, 1);
				record.buildingScale = 1;
			}
			if (record.stuffDef != null) {
				record.color = ((record.stuffDef.graphic == null) ? Color.get_white () : record.stuffDef.graphic.color);
			}
		}

		protected void PrepareInventoryCounters (InventoryRecord record)
		{
		}

		protected void PrepareInventoryResolver (InventoryRecord record)
		{
			ThingDef thingDef = record.thingDef;
			InventoryResolver resolver;
			if (this.inventoryResolvers.TryGetValue (thingDef.defName, out resolver)) {
				record.resolver = resolver;
			}
		}

		protected void PrepareRecord (InventoryRecord record)
		{
			if (record.thingDef.building != null) {
				this.PrepareBuildingRecord (record);
			}
			this.PrepareInventoryResolver (record);
			this.PrepareInventoryCounters (record);
		}

		public void TakeInventory ()
		{
			if (!this.initialized) {
				this.Initialize ();
			}
			this.ClearInventory ();
			foreach (Building current in Find.get_ListerBuildings ().allBuildingsColonist) {
				this.Count (current);
				Building_Storage building_Storage = current as Building_Storage;
				if (building_Storage != null && building_Storage.GetSlotGroup () != null) {
					foreach (Thing current2 in building_Storage.GetSlotGroup ().get_HeldThings ()) {
						this.Count (current2);
					}
				}
				InventoryResolver inventoryResolver;
				if (this.inventoryResolvers.TryGetValue (current.def.defName, out inventoryResolver)) {
					List<InventoryCount> inventoryCounts = inventoryResolver.GetInventoryCounts (current, this.state.prefs);
					if (inventoryCounts != null) {
						foreach (InventoryCount current3 in inventoryCounts) {
							this.Count (current3);
						}
					}
				}
			}
			using (List<Zone>.Enumerator enumerator4 = Find.get_ZoneManager ().get_AllZones ().FindAll ((Zone zone) => zone is Zone_Stockpile).GetEnumerator ()) {
				while (enumerator4.MoveNext ()) {
					Zone_Stockpile zone_Stockpile = (Zone_Stockpile)enumerator4.Current;
					if (zone_Stockpile.GetSlotGroup () != null) {
						foreach (Thing current4 in zone_Stockpile.GetSlotGroup ().get_HeldThings ()) {
							this.Count (current4);
						}
					}
				}
			}
			foreach (Pawn current5 in Find.get_ListerPawns ().get_FreeColonists ()) {
				if (current5.equipment != null) {
					foreach (Thing current6 in current5.equipment.get_AllEquipment ()) {
						this.Count (current6);
					}
				}
				if (current5.apparel != null) {
					foreach (Thing current7 in current5.apparel.get_WornApparel ()) {
						this.Count (current7);
					}
				}
				if (current5.inventory != null && current5.inventory.container != null) {
					ThingContainer container = current5.inventory.container;
					int count = container.get_Count ();
					for (int i = 0; i < count; i++) {
						this.Count (container.get_Item (i));
					}
				}
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class InventoryPreferences
	{
		//
		// Fields
		//
		private PreferenceIncludeUnfinished includeUnfinished;

		private PreferenceCompressedStorage compressedStorage;

		//
		// Properties
		//
		public PreferenceCompressedStorage CompressedStorage {
			get {
				return this.compressedStorage;
			}
			set {
				this.compressedStorage = value;
			}
		}

		public PreferenceIncludeUnfinished IncludeUnfinished {
			get {
				return this.includeUnfinished;
			}
			set {
				this.includeUnfinished = value;
			}
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryRecord
	{
		//
		// Fields
		//
		public ThingDef thingDef;

		public int reservedCount;

		public int availableCount;

		public InventoryResolver resolver;

		public float buildingScale;

		public Vector2 proportions;

		public Texture texture;

		public ThingDef stuffDef;

		public InventoryType type;

		public int count;

		public int unfinishedCount;

		public int compressedCount;

		public Color color;

		//
		// Methods
		//
		public void ResetCounts ()
		{
			this.count = 0;
			this.unfinishedCount = 0;
			this.compressedCount = 0;
			this.availableCount = -1;
			this.reservedCount = 0;
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class InventoryRecordKey
	{
		//
		// Properties
		//
		public ThingDef StuffDef {
			get;
			set;
		}

		public ThingDef ThingDef {
			get;
			set;
		}

		//
		// Constructors
		//
		public InventoryRecordKey ()
		{
		}

		public InventoryRecordKey (ThingDef thingDef)
		{
			this.ThingDef = thingDef;
			this.StuffDef = null;
		}

		public InventoryRecordKey (ThingDef thingDef, ThingDef stuffDef)
		{
			this.ThingDef = thingDef;
			this.StuffDef = stuffDef;
		}

		//
		// Methods
		//
		public override bool Equals (object o)
		{
			if (o == null) {
				return false;
			}
			InventoryRecordKey inventoryRecordKey = o as InventoryRecordKey;
			return inventoryRecordKey != null && this.ThingDef == inventoryRecordKey.ThingDef && this.StuffDef == inventoryRecordKey.StuffDef;
		}

		public override int GetHashCode ()
		{
			int num = (this.ThingDef == null) ? 0 : this.ThingDef.GetHashCode ();
			int num2 = (this.StuffDef == null) ? 0 : this.StuffDef.GetHashCode ();
			return 31 * num + num2;
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public interface InventoryResolver
	{
		//
		// Methods
		//
		List<InventoryCount> GetInventoryCounts (Thing thing, InventoryPreferences prefs);
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class InventoryState
	{
		//
		// Fields
		//
		public bool compressedStorage;

		public InventoryPreferences prefs = new InventoryPreferences ();

		public Dictionary<InventoryType, Dictionary<InventoryRecordKey, InventoryRecord>> categorizedRecords;
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace EdB.Interface
{
	public abstract class InventoryTab
	{
		//
		// Static Fields
		//
		protected static float SectionPaddingSides = 12;

		protected static GameFont SectionLabelFont = 1;

		protected static float SectionLabelHorizontalPadding = 4;

		protected static float SectionPaddingBelowLabel = 0;

		protected static float SectionPaddingBottom = 18;

		protected static float SectionPaddingTop = 9;

		protected static float SlotRowPadding = 12;

		protected static Vector2 LabelOffset = new Vector2 (0, 12);

		//
		// Fields
		//
		public string title = "Untitled";

		protected FloatMenu equipmentAssignmentFloatMenu;

		protected InventoryManager manager;

		public int order;

		protected float scrollViewHeight;

		protected Vector2 scrollPosition = Vector2.get_zero ();

		protected bool backgroundToggle;

		//
		// Properties
		//
		public InventoryManager InventoryManager {
			get {
				return this.manager;
			}
			set {
				this.manager = value;
			}
		}

		//
		// Constructors
		//
		public InventoryTab (string title, int order)
		{
			this.title = Translator.Translate (title);
			this.order = order;
		}

		//
		// Methods
		//
		protected void AssignEquipmentToPawn (InventoryRecord record, Pawn pawn)
		{
			Thing thing = this.FindClosestValidThing (record, pawn);
			if (thing == null) {
				Messages.Message (Translator.Translate ("EdB.Inventory.Equipment.Error"), 4);
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.DesignateFailed);
				return;
			}
			Thing thing2 = (thing.def.equipmentType == null) ? null : thing;
			Apparel apparel = thing as Apparel;
			if (thing2 != null) {
				ForbidUtility.SetForbidden (thing2, false, true);
				Job job = new Job (JobDefOf.Equip, new TargetInfo (thing2));
				job.playerForced = true;
				pawn.drafter.TakeOrderedJob (job);
			}
			else if (apparel != null) {
				ForbidUtility.SetForbidden (apparel, false, true);
				Job job2 = new Job (JobDefOf.Wear, new TargetInfo (apparel));
				job2.playerForced = true;
				pawn.drafter.TakeOrderedJob (job2);
			}
			SoundStarter.PlayOneShotOnCamera (SoundDefOf.ColonistOrdered);
			this.manager.TakeInventory ();
		}

		protected Vector2 DrawResources (float width, List<InventoryRecord> records, Vector2 position, Vector2 iconSize, Vector2 offset, Vector2 slotSize, InventoryPreferences prefs)
		{
			if (this.equipmentAssignmentFloatMenu != null && Find.get_WindowStack ().get_Item (0) != this.equipmentAssignmentFloatMenu) {
				this.equipmentAssignmentFloatMenu = null;
			}
			Vector2 result = new Vector2 (position.x, position.y);
			foreach (InventoryRecord current in records) {
				if (current.count != 0 || (current.compressedCount != 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount != 0 && prefs.IncludeUnfinished.Value)) {
					if (result.x + slotSize.x > width) {
						result.y += slotSize.y;
						result.y += InventoryTab.SlotRowPadding;
						result.x = InventoryTab.SectionPaddingSides;
					}
					if (current.texture != null) {
						Vector2 vector = new Vector2 (0, 0);
						if (current.thingDef.apparel != null && current.thingDef.apparel.get_LastLayer () == 4) {
							vector.y += 10;
						}
						Rect rect = new Rect (result.x + offset.x + vector.x, result.y + offset.y + vector.y, iconSize.x, iconSize.y);
						GUI.set_color (current.color);
						if (current.thingDef.building == null) {
							GUI.DrawTexture (rect, current.thingDef.uiIcon);
							if (current.availableCount > 0 && this.equipmentAssignmentFloatMenu == null && Widgets.InvisibleButton (rect) && Event.get_current ().get_button () == 1) {
								List<FloatMenuOption> list = new List<FloatMenuOption> ();
								foreach (Pawn current2 in Find.get_ListerPawns ().get_FreeColonists ()) {
									if (current2.health.capacities.CapableOf (PawnCapacityDefOf.Manipulation)) {
										InventoryRecord assignedRecord = current;
										Pawn assignedPawn = current2;
										list.Add (new FloatMenuOption (current2.get_LabelBaseShort (), delegate {
											this.AssignEquipmentToPawn (assignedRecord, assignedPawn);
										}, 1, null, null));
									}
								}
								this.equipmentAssignmentFloatMenu = new FloatMenu (list, Translator.Translate ("EdB.Inventory.Equipment.Assign"), false, false);
								Find.get_WindowStack ().Add (this.equipmentAssignmentFloatMenu);
							}
						}
						else {
							if (current.texture == null) {
								Log.Warning ("No texture for building: " + current.thingDef.defName);
							}
							Widgets.DrawTextureFitted (rect, current.texture as Texture2D, current.buildingScale, current.proportions, new Rect (0, 0, 1, 1));
						}
						if (this.equipmentAssignmentFloatMenu == null) {
							InventoryTab.<DrawResources>c__AnonStoreyF <DrawResources>c__AnonStoreyF = new InventoryTab.<DrawResources>c__AnonStoreyF ();
							<DrawResources>c__AnonStoreyF.tooltipText = GenText.CapitalizeFirst (GenLabel.ThingLabel (current.thingDef, current.stuffDef, 1)) + (string.IsNullOrEmpty (current.thingDef.description) ? string.Empty : ("

" + current.thingDef.description));
							if (current.availableCount > -1) {
								InventoryTab.<DrawResources>c__AnonStoreyF arg_432_0 = <DrawResources>c__AnonStoreyF;
								string tooltipText = <DrawResources>c__AnonStoreyF.tooltipText;
								arg_432_0.tooltipText = string.Concat (new string[] {
									tooltipText,
									"

",
									Translator.Translate ("EdB.Inventory.Equipment.Equipped", new object[] {
										current.count - current.reservedCount - current.availableCount
									}),
									"
",
									Translator.Translate ("EdB.Inventory.Equipment.Reserved", new object[] {
										current.reservedCount
									}),
									"
",
									Translator.Translate ("EdB.Inventory.Equipment.Available", new object[] {
										current.availableCount
									})
								});
							}
							TipSignal tipSignal = new TipSignal (() => <DrawResources>c__AnonStoreyF.tooltipText, <DrawResources>c__AnonStoreyF.tooltipText.GetHashCode ());
							TooltipHandler.TipRegion (rect, tipSignal);
						}
						Text.set_Anchor (1);
						GUI.set_color (Color.get_white ());
						Text.set_Font (0);
						string text;
						if (prefs.IncludeUnfinished.Value && current.thingDef.building != null) {
							text = string.Empty + current.count + ((current.unfinishedCount <= 0) ? string.Empty : (" / " + (current.count + current.unfinishedCount)));
						}
						else if (prefs.CompressedStorage.Value && current.compressedCount > 0) {
							text = string.Concat (new object[] {
								string.Empty,
								current.count,
								" / ",
								current.count + current.compressedCount
							});
						}
						else {
							text = string.Empty + current.count;
						}
						Widgets.Label (new Rect (result.x, result.y + slotSize.y - InventoryTab.LabelOffset.y, slotSize.x, slotSize.y), text);
						result.x += slotSize.x;
					}
				}
			}
			return result;
		}

		protected Vector2 DrawResourceSection (float width, string label, List<InventoryRecord> records, Vector2 position, Vector2 slotSize, Vector2 offset, Vector2 iconSize, InventoryPreferences prefs)
		{
			float num = width - 20;
			if (records == null) {
				return position;
			}
			if (prefs.IncludeUnfinished.Value) {
				if (records.Count == 0) {
					return position;
				}
			}
			else {
				bool flag = false;
				foreach (InventoryRecord current in records) {
					if (current.count > 0 || (current.compressedCount > 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount > 0 && prefs.IncludeUnfinished.Value)) {
						flag = true;
						break;
					}
				}
				if (!flag) {
					return position;
				}
			}
			Vector2 vector = new Vector2 (position.x, position.y);
			if (vector.x != InventoryTab.SectionPaddingSides) {
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			float num2 = this.MeasureResourceSection (width, label, records, slotSize, prefs);
			if (this.backgroundToggle) {
				GUI.set_color (new Color (0.082, 0.0977, 0.1133));
			}
			else {
				GUI.set_color (new Color (0.1094, 0.125, 0.1406));
			}
			GUI.DrawTexture (new Rect (0, vector.y, width + 16, num2), BaseContent.WhiteTex);
			GUI.set_color (new Color (0.3294, 0.3294, 0.3294));
			Widgets.DrawLineHorizontal (0, vector.y + num2 - 1, width + 16);
			this.backgroundToggle = !this.backgroundToggle;
			vector.y += InventoryTab.SectionPaddingTop;
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Text.set_Font (InventoryTab.SectionLabelFont);
			float num3 = Text.CalcHeight (label, num);
			Rect rect = new Rect (InventoryTab.SectionPaddingSides + InventoryTab.SectionLabelHorizontalPadding, vector.y, num - InventoryTab.SectionLabelHorizontalPadding, num3);
			Widgets.Label (rect, label);
			vector.y += num3 + InventoryTab.SectionPaddingBelowLabel;
			vector = this.DrawResources (num, records, vector, iconSize, offset, slotSize, prefs);
			if (vector.x != InventoryTab.SectionPaddingSides) {
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			vector.y += InventoryTab.SectionPaddingBottom;
			return vector;
		}

		protected Thing FindClosestValidThing (InventoryRecord record, Pawn pawn)
		{
			List<EquippableThing> list = this.manager.EquippableThings [new InventoryRecordKey (record.thingDef, record.stuffDef)];
			foreach (EquippableThing current in list) {
				Thing thing2 = current.thing;
				current.distance = (pawn.get_Position () - thing2.get_Position ()).get_LengthHorizontalSquared ();
			}
			IEnumerable<Thing> enumerable = from thing in list
			orderby thing.distance
			select thing.thing;
			foreach (Thing current2 in enumerable) {
				if (Reachability.CanReach (pawn, current2, 3, 2, false)) {
					if (ReservationUtility.CanReserveAndReach (pawn, new TargetInfo (current2), 3, 2, 1)) {
						return current2;
					}
				}
			}
			return null;
		}

		public abstract void InventoryTabOnGui (Rect fillRect);

		protected float MeasureResourceSection (float width, string label, List<InventoryRecord> records, Vector2 slotSize, InventoryPreferences prefs)
		{
			float num = width - 20;
			Vector2 vector = new Vector2 (InventoryTab.SectionPaddingSides, InventoryTab.SectionPaddingTop);
			Text.set_Anchor (0);
			Text.set_Font (InventoryTab.SectionLabelFont);
			vector.y += Text.CalcHeight (label, num) + InventoryTab.SectionPaddingBelowLabel;
			foreach (InventoryRecord current in records) {
				if (current.count != 0 || (current.compressedCount != 0 && prefs.CompressedStorage.Value) || (current.unfinishedCount != 0 && prefs.IncludeUnfinished.Value)) {
					if (vector.x + slotSize.x > num) {
						vector.y += slotSize.y;
						vector.y += InventoryTab.SlotRowPadding;
						vector.x = InventoryTab.SectionPaddingSides;
					}
					vector.x += slotSize.x;
				}
			}
			if (vector.x != InventoryTab.SectionPaddingSides) {
				vector.x = InventoryTab.SectionPaddingSides;
				vector.y += slotSize.y;
			}
			vector.y += InventoryTab.SectionPaddingBottom;
			return vector.y;
		}
	}
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryTab_Buildings : InventoryTab
	{
		//
		// Static Fields
		//
		protected static Vector2 BuildingSlotSize = new Vector2 (86, 86);

		protected static Vector2 BuildingImageSize = new Vector2 (72, 72);

		protected static Vector2 BuildingImageOffset;

		//
		// Fields
		//
		protected Dictionary<string, string> labels = new Dictionary<string, string> ();

		//
		// Constructors
		//
		public InventoryTab_Buildings () : base ("EdB.Inventory.Tab.Buildings", 500)
		{
			InventoryTab_Buildings.BuildingImageOffset = new Vector2 (InventoryTab_Buildings.BuildingSlotSize.x / 2 - InventoryTab_Buildings.BuildingImageSize.x / 2, InventoryTab_Buildings.BuildingSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Buildings.BuildingImageSize.y);
			InventoryTab_Buildings.BuildingImageOffset.y = InventoryTab_Buildings.BuildingImageOffset.y;
			this.InitializeCategoryLabels ();
		}

		//
		// Methods
		//
		private void InitializeCategoryLabels ()
		{
			string[] array = new string[] {
				"Furniture",
				"Structure",
				"Power",
				"Security",
				"Production",
				"Joy",
				"Temperature",
				"Ship",
				"FoodUtilities"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++) {
				string text = array2 [i];
				foreach (DesignationCategoryDef current in DefDatabase<DesignationCategoryDef>.get_AllDefs ()) {
					if (text.Equals (current.defName)) {
						this.labels.Add (text, current.get_LabelCap ());
						break;
					}
				}
				if (!this.labels.ContainsKey (text)) {
					this.labels.Add (text, text);
				}
			}
		}

		public override void InventoryTabOnGui (Rect fillRect)
		{
			InventoryPreferences preferences = this.manager.Preferences;
			Rect rect = new Rect (23, 93, 928, 510);
			GUI.set_color (new Color (0.08235, 0.09804, 0.1137));
			GUI.DrawTexture (rect, BaseContent.WhiteTex);
			GUI.set_color (new Color (0.5394, 0.5394, 0.5394));
			Widgets.DrawBox (rect, 1);
			try {
				Rect rect2 = GenUI.ContractedBy (rect, 1);
				GUI.BeginGroup (rect2);
				Rect rect3 = new Rect (0, 0, rect2.get_width (), rect2.get_height ());
				Rect rect4 = new Rect (rect3.get_x (), rect3.get_y (), rect3.get_width () - 16, this.scrollViewHeight);
				try {
					Widgets.BeginScrollView (rect3, ref this.scrollPosition, rect4);
					this.backgroundToggle = false;
					Vector2 position = new Vector2 (InventoryTab.SectionPaddingSides, 0);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Furniture"], this.manager.GetInventoryRecord (InventoryType.BUILDING_FURNITURE), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Production"], this.manager.GetInventoryRecord (InventoryType.BUILDING_PRODUCTION), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Power"], this.manager.GetInventoryRecord (InventoryType.BUILDING_POWER), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Security"], this.manager.GetInventoryRecord (InventoryType.BUILDING_SECURITY), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Joy"], this.manager.GetInventoryRecord (InventoryType.BUILDING_JOY), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Temperature"], this.manager.GetInventoryRecord (InventoryType.BUILDING_TEMPERATURE), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Ship"], this.manager.GetInventoryRecord (InventoryType.BUILDING_SHIP), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["Structure"], this.manager.GetInventoryRecord (InventoryType.BUILDING_STRUCTURE), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), this.labels ["FoodUtilities"], this.manager.GetInventoryRecord (InventoryType.BUILDING_FOOD_UTILITIES), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Other"), this.manager.GetInventoryRecord (InventoryType.BUILDING_OTHER), position, InventoryTab_Buildings.BuildingSlotSize, InventoryTab_Buildings.BuildingImageOffset, InventoryTab_Buildings.BuildingImageSize, preferences);
					if (Event.get_current ().get_type () == 8) {
						this.scrollViewHeight = position.y - 1;
					}
				}
				finally {
					Widgets.EndScrollView ();
				}
			}
			finally {
				GUI.EndGroup ();
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
			}
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Text.set_Font (1);
			string text = Translator.Translate ("EdB.Inventory.Prefs.IncludeUnfinished");
			float num = Text.CalcSize (text).x + 40;
			float num2 = 22;
			bool value = preferences.IncludeUnfinished.Value;
			Widgets.LabelCheckbox (new Rect (fillRect.get_x () + fillRect.get_width () - num - num2, fillRect.get_y () + fillRect.get_height () - 38, num, 30), text, ref value, false);
			preferences.IncludeUnfinished.Value = value;
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryTab_Items : InventoryTab
	{
		//
		// Static Fields
		//
		protected static Vector2 ItemSlotSize = new Vector2 (72, 68);

		protected static Vector2 WeaponImageOffset;

		protected static Vector2 WeaponImageSize = new Vector2 (64, 64);

		protected static Vector2 ApparelImageOffset;

		protected static Vector2 ApparelImageSize = new Vector2 (52, 52);

		protected static Vector2 ResourceImageOffset;

		protected static Vector2 ResourceImageSize = new Vector2 (48, 48);

		protected static Vector2 WeaponSlotSize = new Vector2 (76, 78);

		//
		// Constructors
		//
		public InventoryTab_Items () : base ("EdB.Inventory.Tab.ItemsAndResources", 1000)
		{
			InventoryTab_Items.WeaponImageOffset = new Vector2 (InventoryTab_Items.ItemSlotSize.x / 2 - InventoryTab_Items.WeaponImageSize.x / 2, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.WeaponImageSize.y);
			InventoryTab_Items.WeaponImageOffset.y = InventoryTab_Items.WeaponImageOffset.y + 6;
			InventoryTab_Items.ResourceImageOffset = new Vector2 (InventoryTab_Items.ItemSlotSize.x / 2 - InventoryTab_Items.ResourceImageSize.x / 2, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.ResourceImageSize.y);
			InventoryTab_Items.ResourceImageOffset.y = InventoryTab_Items.ResourceImageOffset.y - 4;
			InventoryTab_Items.ApparelImageOffset = new Vector2 (InventoryTab_Items.ItemSlotSize.x / 2 - InventoryTab_Items.ApparelImageSize.x / 2, InventoryTab_Items.ItemSlotSize.y - InventoryTab.LabelOffset.y - InventoryTab_Items.ApparelImageSize.y);
			InventoryTab_Items.ApparelImageOffset.y = InventoryTab_Items.ApparelImageOffset.y - 6;
		}

		//
		// Methods
		//
		public override void InventoryTabOnGui (Rect fillRect)
		{
			InventoryPreferences preferences = this.manager.Preferences;
			Rect rect = new Rect (23, 93, 928, 510);
			GUI.set_color (new Color (0.08235, 0.09804, 0.1137));
			GUI.DrawTexture (rect, BaseContent.WhiteTex);
			GUI.set_color (new Color (0.5394, 0.5394, 0.5394));
			Widgets.DrawBox (rect, 1);
			try {
				Rect rect2 = GenUI.ContractedBy (rect, 1);
				GUI.BeginGroup (rect2);
				Rect rect3 = new Rect (0, 0, rect2.get_width (), rect2.get_height ());
				Rect rect4 = new Rect (rect3.get_x (), rect3.get_y (), rect3.get_width () - 16, this.scrollViewHeight);
				try {
					Widgets.BeginScrollView (rect3, ref this.scrollPosition, rect4);
					this.backgroundToggle = false;
					Vector2 position = new Vector2 (InventoryTab.SectionPaddingSides, 0);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Resources"), this.manager.GetInventoryRecord (InventoryType.ITEM_RESOURCE), position, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Food"), this.manager.GetInventoryRecord (InventoryType.ITEM_FOOD), position, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Weapons"), this.manager.GetInventoryRecord (InventoryType.ITEM_EQUIPMENT), position, InventoryTab_Items.WeaponSlotSize, InventoryTab_Items.WeaponImageOffset, InventoryTab_Items.WeaponImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Apparel"), this.manager.GetInventoryRecord (InventoryType.ITEM_APPAREL), position, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ApparelImageOffset, InventoryTab_Items.ApparelImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Schematics"), this.manager.GetInventoryRecord (InventoryType.ITEM_SCHEMATIC), position, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					position = base.DrawResourceSection (rect4.get_width (), Translator.Translate ("EdB.Inventory.Section.Other"), this.manager.GetInventoryRecord (InventoryType.ITEM_OTHER), position, InventoryTab_Items.ItemSlotSize, InventoryTab_Items.ResourceImageOffset, InventoryTab_Items.ResourceImageSize, preferences);
					if (Event.get_current ().get_type () == 8) {
						this.scrollViewHeight = position.y - 1;
					}
				}
				finally {
					Widgets.EndScrollView ();
				}
			}
			finally {
				GUI.EndGroup ();
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
			}
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Text.set_Font (1);
			if (this.manager.CompressedStorage) {
				string text = Translator.Translate ("EdB.Inventory.Prefs.CompressedStorage");
				float num = Text.CalcSize (text).x + 32;
				float num2 = 22;
				Rect rect5 = new Rect (fillRect.get_x () + fillRect.get_width () - num - num2, fillRect.get_y () + fillRect.get_height () - 38, num, 30);
				bool value = preferences.CompressedStorage.Value;
				Widgets.LabelCheckbox (rect5, text, ref value, false);
				preferences.CompressedStorage.Value = value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public enum InventoryType
	{
		UNKNOWN,
		ITEM_RESOURCE,
		ITEM_FOOD,
		ITEM_OTHER,
		ITEM_APPAREL,
		ITEM_EQUIPMENT,
		ITEM_SCHEMATIC,
		BUILDING_OTHER,
		BUILDING_FURNITURE,
		BUILDING_SECURITY,
		BUILDING_STRUCTURE,
		BUILDING_POWER,
		BUILDING_SHIP,
		BUILDING_PRODUCTION,
		BUILDING_JOY,
		BUILDING_TEMPERATURE,
		BUILDING_FOOD_UTILITIES
	}
}
using System;

namespace EdB.Interface
{
	public interface IPreference
	{
		//
		// Properties
		//
		bool DisplayInOptions {
			get;
		}

		string Group {
			get;
		}

		string Name {
			get;
		}

		string ValueForSerialization {
			get;
			set;
		}

		//
		// Methods
		//
		void OnGUI (float positionX, ref float positionY, float width);
	}
}
using System;

namespace EdB.Interface
{
	public interface IRenderedComponent
	{
		//
		// Properties
		//
		bool RenderWithScreenshots {
			get;
		}

		//
		// Methods
		//
		void OnGUI ();
	}
}
using RimWorld;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Art_Alternate : ITab_Art
	{
		//
		// Static Fields
		//
		protected static string cachedImageDescription;

		protected static CompArt cachedImageSource;

		protected static TaleReference cachedTaleRef;

		//
		// Fields
		//
		protected MethodInfo selectedCompArtGetter;

		//
		// Constructors
		//
		public ITab_Art_Alternate ()
		{
			this.size = new Vector2 (TabDrawer.TabPanelSize.x, 320);
			PropertyInfo property = typeof(ITab_Art).GetProperty ("SelectedCompArt", BindingFlags.Instance | BindingFlags.NonPublic);
			this.selectedCompArtGetter = property.GetGetMethod (true);
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			Rect rect = GenUI.ContractedBy (new Rect (0, 0, this.size.x, this.size.y), 20);
			Rect rect2 = rect;
			Text.set_Font (2);
			CompArt compArt = (CompArt)this.selectedCompArtGetter.Invoke (this, null);
			Widgets.Label (rect2, compArt.get_Title ());
			if (ITab_Art_Alternate.cachedImageSource != compArt || ITab_Art_Alternate.cachedTaleRef != compArt.get_TaleRef ()) {
				ITab_Art_Alternate.cachedImageDescription = compArt.ImageDescription ();
				ITab_Art_Alternate.cachedImageSource = compArt;
				ITab_Art_Alternate.cachedTaleRef = compArt.get_TaleRef ();
			}
			Rect rect3 = rect;
			rect3.set_yMin (rect3.get_yMin () + 35);
			Text.set_Font (1);
			Widgets.Label (rect3, ITab_Art_Alternate.cachedImageDescription);
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Bills_Alternate : ITab_Bills
	{
		//
		// Static Fields
		//
		private static readonly Vector2 WinSize = new Vector2 (TabDrawer.TabPanelSize.x, 480);

		//
		// Fields
		//
		private ScrollView scrollView = new ScrollView ();

		protected FieldInfo mouseoverBillField;

		//
		// Constructors
		//
		public ITab_Bills_Alternate ()
		{
			this.size = ITab_Bills_Alternate.WinSize;
			this.mouseoverBillField = typeof(ITab_Bills).GetField ("mouseoverBill", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.BillsTab, 1);
			Rect rect = GenUI.ContractedBy (new Rect (0, 0, ITab_Bills_Alternate.WinSize.x, ITab_Bills_Alternate.WinSize.y), 10);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate {
				List<FloatMenuOption> list = new List<FloatMenuOption> ();
				for (int i = 0; i < base.get_SelTable ().def.get_AllRecipes ().Count; i++) {
					RecipeDef recipe = base.get_SelTable ().def.get_AllRecipes () [i];
					list.Add (new FloatMenuOption (recipe.get_LabelCap (), delegate {
						if (!Find.get_ListerPawns ().get_FreeColonists ().Any ((Pawn col) => recipe.PawnSatisfiesSkillRequirements (col))) {
							Bill.CreateNoPawnsWithSkillDialog (recipe);
						}
						Bill bill = BillUtility.MakeNewBill (recipe);
						this.get_SelTable ().billStack.AddBill (bill);
					}, 1, null, null));
				}
				return list;
			};
			Bill value = BillDrawer.DrawListing (base.get_SelTable ().billStack, rect, recipeOptionsMaker, this.scrollView);
			this.mouseoverBillField.SetValue (this, value);
		}
	}
}
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class ITab_Growing_Alternate : ITab_Growing
	{
		//
		// Static Fields
		//
		private static readonly Vector2 WinSize = new Vector2 (TabDrawer.TabPanelSize.x, 480);

		private static readonly Vector2 PaddingSize = new Vector2 (26, 30);

		private static Vector2 ContentSize;

		//
		// Fields
		//
		protected FloatMenu infoFloatMenu;

		protected ThingDef rightClicked;

		protected ScrollView scrollView = new ScrollView ();

		//
		// Constructors
		//
		public ITab_Growing_Alternate ()
		{
			this.size = ITab_Growing_Alternate.WinSize;
			ITab_Growing_Alternate.ContentSize = new Vector2 (ITab_Growing_Alternate.WinSize.x - ITab_Growing_Alternate.PaddingSize.x * 2, ITab_Growing_Alternate.WinSize.y - ITab_Growing_Alternate.PaddingSize.y * 2);
			ITab_Growing_Alternate.ContentSize.y = ITab_Growing_Alternate.ContentSize.y - 2;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			if (this.rightClicked != null && this.infoFloatMenu != null && Find.get_WindowStack ().Top () != this.infoFloatMenu) {
				this.rightClicked = null;
				this.infoFloatMenu = null;
			}
			Text.set_Font (1);
			IPlantToGrowSettable plantToGrowSettable = (IPlantToGrowSettable)Find.get_Selector ().get_SelectedObjects ().First<object> ();
			Rect rect = new Rect (ITab_Growing_Alternate.PaddingSize.x, ITab_Growing_Alternate.PaddingSize.y, ITab_Growing_Alternate.ContentSize.x, ITab_Growing_Alternate.ContentSize.y);
			GUI.BeginGroup (rect);
			Rect viewRect = new Rect (0, 0, rect.get_width (), rect.get_height ());
			this.scrollView.Begin (viewRect);
			float num = 0;
			int num2 = 0;
			foreach (ThingDef current in GenPlant.ValidPlantTypesForGrower (Find.get_Selector ().get_SingleSelectedObject ())) {
				float num3 = Text.CalcHeight (current.get_LabelCap (), ITab_Growing_Alternate.ContentSize.x - 32);
				if (num3 < 30) {
					num3 = 30;
				}
				GUI.set_color (Color.get_white ());
				Rect rect2 = new Rect (0, num + 1, ITab_Growing_Alternate.ContentSize.x - 28, num3);
				Rect rect3 = new Rect (0, rect2.get_y () - 1, rect2.get_width (), rect2.get_height () + 2);
				Vector2 mousePosition = Event.get_current ().get_mousePosition ();
				if (rect3.Contains (mousePosition) && mousePosition.y > this.scrollView.Position.y && mousePosition.y < this.scrollView.Position.y + this.scrollView.ViewHeight) {
					GUI.DrawTexture (rect3, TexUI.HighlightTex);
				}
				else if (num2 % 2 == 0) {
					GUI.DrawTexture (rect3, TabDrawer.AlternateRowTexture);
				}
				rect2.set_x (rect2.get_x () + 6);
				rect2.set_y (rect2.get_y () + 3);
				rect2.set_width (rect2.get_width () - 4);
				Widgets.InfoCardButton (rect2.get_x (), rect2.get_y (), current);
				rect2.set_x (rect2.get_x () + 34);
				rect2.set_width (rect2.get_width () - 34);
				if ((Widgets.InvisibleButton (new Rect (rect2.get_x (), rect2.get_y (), rect2.get_width () - 36, rect2.get_height ())) || WidgetDrawer.DrawLabeledRadioButton (rect2, current.get_LabelCap (), current == plantToGrowSettable.GetPlantDefToGrow (), false)) && plantToGrowSettable.GetPlantDefToGrow () != current) {
					plantToGrowSettable.SetPlantDefToGrow (current);
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.RadioButtonClicked);
					ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.SetGrowingZonePlant, 6);
				}
				num += num3;
				num += 2;
				num2++;
			}
			this.scrollView.End (num);
			TutorUIHighlighter.HighlightOpportunity ("GrowingZoneSetPlant", new Rect (0, 0, ITab_Growing_Alternate.ContentSize.x, num));
			GUI.EndGroup ();
		}
	}
}
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Character_Alternate : ITab_Pawn_Character
	{
		//
		// Static Fields
		//
		private static readonly Vector2 PaddingSize = new Vector2 (26, 20);

		private static readonly Vector2 PanelSize = TabDrawer.TabPanelSize;

		private static readonly Vector2 ContentSize = new Vector2 (ITab_Pawn_Character_Alternate.PanelSize.x - ITab_Pawn_Character_Alternate.PaddingSize.x * 2, ITab_Pawn_Character_Alternate.PanelSize.y - ITab_Pawn_Character_Alternate.PaddingSize.y * 2);

		//
		// Fields
		//
		protected SkillDrawer skillDrawer = new SkillDrawer ();

		protected float HeightWithoutSkills = 362;

		protected float StandardSkillCount = 12;

		protected SelectorUtility pawnSelector = new SelectorUtility ();

		//
		// Properties
		//
		private Pawn PawnToShowInfoAbout {
			get {
				Pawn pawn = null;
				if (base.get_SelPawn () != null) {
					pawn = base.get_SelPawn ();
				}
				else {
					Corpse corpse = base.get_SelThing () as Corpse;
					if (corpse != null) {
						pawn = corpse.innerPawn;
					}
				}
				if (pawn == null) {
					Log.Error ("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Character_Alternate (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.size = TabDrawer.TabPanelSize;
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			Pawn pawnToShowInfoAbout = this.PawnToShowInfoAbout;
			try {
				GUI.BeginGroup (new Rect (ITab_Pawn_Character_Alternate.PaddingSize.x, ITab_Pawn_Character_Alternate.PaddingSize.y, ITab_Pawn_Character_Alternate.ContentSize.x, ITab_Pawn_Character_Alternate.ContentSize.y));
				float num = 0;
				float num2 = 12;
				bool allowRename = !pawnToShowInfoAbout.get_Dead () && !pawnToShowInfoAbout.get_Destroyed ();
				num += TabDrawer.DrawNameAndBasicInfo (0, num, this.PawnToShowInfoAbout, ITab_Pawn_Character_Alternate.ContentSize.x, allowRename);
				num += num2;
				num += TabDrawer.DrawHeader (0, num, ITab_Pawn_Character_Alternate.ContentSize.x, Translator.Translate ("Backstory"), true, 0);
				num += 2;
				Text.set_Font (1);
				GUI.set_color (TabDrawer.TextColor);
				Vector2 vector = new Vector2 (ITab_Pawn_Character_Alternate.ContentSize.x, 24);
				Vector2 vector2 = new Vector2 (90, 24);
				Vector2 vector3 = new Vector2 (3, 2);
				int num3 = 0;
				IEnumerator enumerator = Enum.GetValues (typeof(BackstorySlot)).GetEnumerator ();
				try {
					while (enumerator.MoveNext ()) {
						BackstorySlot backstorySlot = (BackstorySlot)enumerator.Current;
						Rect rect = new Rect (0, num, vector.x, vector.y);
						if (rect.Contains (Event.get_current ().get_mousePosition ())) {
							Widgets.DrawHighlight (rect);
						}
						TooltipHandler.TipRegion (rect, pawnToShowInfoAbout.story.GetBackstory (backstorySlot).FullDescriptionFor (pawnToShowInfoAbout));
						rect.set_x (rect.get_x () + vector3.x);
						rect.set_width (rect.get_width () - vector3.x * 2);
						rect.set_y (rect.get_y () + vector3.y);
						GUI.get_skin ().get_label ().set_alignment (3);
						string str = (backstorySlot == 1) ? Translator.Translate ("Adulthood") : Translator.Translate ("Childhood");
						Widgets.Label (rect, str + ":");
						GUI.get_skin ().get_label ().set_alignment (0);
						Rect rect2 = new Rect (rect.get_x () + vector2.x, rect.get_y (), vector.x - vector2.x, vector.y);
						string title = pawnToShowInfoAbout.story.GetBackstory (backstorySlot).title;
						Widgets.Label (rect2, title);
						num += vector.y + 2;
						num3++;
					}
				}
				finally {
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null) {
						disposable.Dispose ();
					}
				}
				num -= 6;
				num += num2;
				num += TabDrawer.DrawHeader (0, num, ITab_Pawn_Character_Alternate.ContentSize.x, Translator.Translate ("Skills"), true, 0);
				num += 6;
				float num4 = (float)DefDatabase<SkillDef>.get_AllDefs ().Count<SkillDef> () * 24;
				Rect rect3 = new Rect (0, num, 390, num4);
				GUI.set_color (TabDrawer.TextColor);
				GUI.BeginGroup (rect3);
				this.skillDrawer.DrawSkillsOf (pawnToShowInfoAbout, new Vector2 (0, 0));
				GUI.EndGroup ();
				num += rect3.get_height ();
				num += num2;
				float num5 = num;
				float num6 = ITab_Pawn_Character_Alternate.ContentSize.x * 0.4 - ITab_Pawn_Character_Alternate.PaddingSize.x / 2;
				float num7 = ITab_Pawn_Character_Alternate.ContentSize.x * 0.6 - ITab_Pawn_Character_Alternate.PaddingSize.x / 2;
				float num8 = num6 + ITab_Pawn_Character_Alternate.PaddingSize.x;
				num += TabDrawer.DrawHeader (0, num, num6, Translator.Translate ("IncapableOf"), true, 0);
				num += 4;
				Vector2 vector4 = new Vector2 (num6, 22);
				Text.set_Font (1);
				GUI.set_color (TabDrawer.TextColor);
				List<WorkTags> list = pawnToShowInfoAbout.story.get_DisabledWorkTags ().ToList<WorkTags> ();
				GUI.get_skin ().get_label ().set_alignment (0);
				if (list.Count == 0) {
					Rect rect4 = new Rect (0, num, num6, vector4.y);
					GUI.set_color (TabDrawer.SeparatorColor);
					Widgets.Label (rect4, Translator.Translate ("NoneLower"));
					GUI.set_color (TabDrawer.TextColor);
					num += rect4.get_height () - 1;
				}
				else {
					foreach (WorkTags current in list) {
						Rect rect5 = new Rect (0, num, num6, vector4.y);
						Widgets.Label (rect5, WorkTypeDefsUtility.LabelTranslated (current));
						num += vector4.y - 1;
					}
				}
				num = num5;
				num += TabDrawer.DrawHeader (num8, num, num7, Translator.Translate ("Traits"), true, 0);
				num += 4;
				Vector2 vector5 = new Vector2 (num7, 22);
				Text.set_Font (1);
				GUI.set_color (TabDrawer.TextColor);
				float num9 = 0;
				foreach (Trait current2 in pawnToShowInfoAbout.story.traits.allTraits) {
					num9 += vector5.y + 2;
					Rect rect6 = new Rect (num8, num, num7, vector5.y);
					if (rect6.Contains (Event.get_current ().get_mousePosition ())) {
						Widgets.DrawHighlight (rect6);
					}
					rect6.set_x (rect6.get_x () + 2);
					Widgets.Label (rect6, current2.get_LabelCap ());
					TooltipHandler.TipRegion (rect6, current2.TipString (pawnToShowInfoAbout));
					num += vector5.y - 1;
				}
			}
			finally {
				GUI.EndGroup ();
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value && pawnToShowInfoAbout != null) {
				BrowseButtonDrawer.DrawBrowseButtons (this.size, pawnToShowInfoAbout);
			}
			GUI.set_color (Color.get_white ());
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Character_Vanilla : ITab_Pawn_Character
	{
		//
		// Properties
		//
		private Pawn PawnToShowInfoAbout {
			get {
				Pawn pawn = null;
				if (base.get_SelPawn () != null) {
					pawn = base.get_SelPawn ();
				}
				else {
					Corpse corpse = base.get_SelThing () as Corpse;
					if (corpse != null) {
						pawn = corpse.innerPawn;
					}
				}
				if (pawn == null) {
					Log.Error ("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Character_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn pawnToShowInfoAbout = this.PawnToShowInfoAbout;
				if (pawnToShowInfoAbout != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, pawnToShowInfoAbout);
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Gear_Alternate : ITab_Pawn_Gear
	{
		//
		// Static Fields
		//
		private const float TopPadding = 20;

		public static float LineHeight = 25;

		private static readonly Vector2 PanelSize = new Vector2 (TabDrawer.TabPanelSize.x, 480);

		private static readonly Color HighlightColor = new Color (0.5, 0.5, 0.5, 1);

		private static readonly Color ThingLabelColor = new Color (0.9, 0.9, 0.9, 1);

		private const float SeparatorLabelHeight = 20;

		private const float ThingIconSize = 28;

		private const float ThingRowHeight = 30;

		private const float ThingLeftX = 36;

		private const float InfoRectHeight = 100;

		//
		// Fields
		//
		private Vector2 scrollPosition = Vector2.get_zero ();

		private float scrollViewHeight;

		//
		// Properties
		//
		private bool CanEdit {
			get {
				return this.SelPawnForGear.get_IsColonistPlayerControlled ();
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		private Pawn SelPawnForGear {
			get {
				if (base.get_SelPawn () != null) {
					return base.get_SelPawn ();
				}
				Corpse corpse = base.get_SelThing () as Corpse;
				if (corpse != null) {
					return corpse.innerPawn;
				}
				throw new InvalidOperationException ("Gear tab on non-pawn non-corpse " + base.get_SelThing ());
			}
		}

		//
		// Constructors
		//
		public ITab_Pawn_Gear_Alternate (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.size = ITab_Pawn_Gear_Alternate.PanelSize;
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
		}

		//
		// Methods
		//
		private void DrawThingRow (ref float y, float width, Thing thing)
		{
			Rect rect = new Rect (36, y + 2, width - 38, 28);
			string text = thing.get_LabelCap ();
			if (thing is Apparel && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced ((Apparel)thing)) {
				text = text + ", " + Translator.Translate ("ApparelForcedLower");
			}
			float num = Text.CalcHeight (text, rect.get_width ());
			rect.set_height ((num >= 28) ? num : 28);
			Rect rect2 = new Rect (0, y, width, rect.get_height () + 4);
			if (rect2.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (ITab_Pawn_Gear_Alternate.HighlightColor);
				GUI.DrawTexture (rect2, TexUI.HighlightTex);
			}
			if (Widgets.InvisibleButton (rect2) && Event.get_current ().get_button () == 1) {
				List<FloatMenuOption> list = new List<FloatMenuOption> ();
				list.Add (new FloatMenuOption (Translator.Translate ("ThingInfo"), delegate {
					Find.get_WindowStack ().Add (new Dialog_InfoCard (thing));
				}, 1, null, null));
				if (this.CanEdit) {
					Action action = null;
					ThingWithComps eq = thing as ThingWithComps;
					Apparel ap = thing as Apparel;
					if (ap != null) {
						Apparel unused;
						action = delegate {
							this.SelPawnForGear.apparel.TryDrop (ap, ref unused, this.SelPawnForGear.get_Position (), true);
						};
					}
					else if (eq != null && this.SelPawnForGear.equipment.get_AllEquipment ().Contains (eq)) {
						ThingWithComps unused;
						action = delegate {
							this.SelPawnForGear.equipment.TryDropEquipment (eq, ref unused, this.SelPawnForGear.get_Position (), true);
						};
					}
					else if (!thing.def.destroyOnDrop) {
						Thing unused;
						action = delegate {
							this.SelPawnForGear.inventory.container.TryDrop (thing, this.SelPawnForGear.get_Position (), 1, ref unused);
						};
					}
					list.Add (new FloatMenuOption (Translator.Translate ("DropThing"), action, 1, null, null));
				}
				FloatMenu floatMenu = new FloatMenu (list, thing.get_LabelCap (), false, false);
				Find.get_WindowStack ().Add (floatMenu);
			}
			if (thing.def.get_DrawMatSingle () != null && thing.def.get_DrawMatSingle ().get_mainTexture () != null) {
				Widgets.ThingIcon (new Rect (2, y + 2, 28, 28), thing);
			}
			Text.set_Anchor (3);
			GUI.set_color (ITab_Pawn_Gear_Alternate.ThingLabelColor);
			rect.set_y (rect.get_y () + 1);
			Widgets.Label (rect, text);
			y += rect.get_height () + 4;
		}

		protected override void FillTab ()
		{
			Text.set_Font (1);
			Rect rect = new Rect (0, 8, this.size.x, this.size.y - 8);
			Rect rect2 = GenUI.ContractedBy (rect, 24);
			Rect rect3 = new Rect (rect2.get_x (), rect2.get_y (), rect2.get_width (), rect2.get_height () - 24);
			try {
				GUI.BeginGroup (rect3);
				Text.set_Font (1);
				GUI.set_color (Color.get_white ());
				Rect rect4 = new Rect (0, 0, rect3.get_width (), rect3.get_height ());
				Rect rect5 = new Rect (0, 0, rect3.get_width () - 16, this.scrollViewHeight);
				Widgets.BeginScrollView (rect4, ref this.scrollPosition, rect5);
				float num = 0;
				bool flag = false;
				if (this.SelPawnForGear.equipment != null) {
					num += TabDrawer.DrawHeader (0, num, rect5.get_width (), Translator.Translate ("Equipment"), true, 0);
					foreach (ThingWithComps current in this.SelPawnForGear.equipment.get_AllEquipment ()) {
						this.DrawThingRow (ref num, rect5.get_width (), current);
						flag = true;
					}
					if (!flag) {
						num += 4;
						Rect rect6 = new Rect (0, num, rect5.get_width (), 24);
						GUI.set_color (TabDrawer.SeparatorColor);
						Widgets.Label (rect6, Translator.Translate ("NoneLower"));
						GUI.set_color (TabDrawer.TextColor);
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				flag = false;
				if (this.SelPawnForGear.apparel != null) {
					num += 10;
					num += TabDrawer.DrawHeader (0, num, rect5.get_width (), Translator.Translate ("Apparel"), true, 0);
					num += 4;
					foreach (Apparel current2 in from ap in this.SelPawnForGear.apparel.get_WornApparel ()
					orderby ap.def.apparel.bodyPartGroups [0].listOrder descending
					select ap) {
						this.DrawThingRow (ref num, rect5.get_width (), current2);
						flag = true;
					}
					if (!flag) {
						num += 4;
						Rect rect7 = new Rect (0, num, rect5.get_width (), 24);
						GUI.set_color (TabDrawer.SeparatorColor);
						Widgets.Label (rect7, Translator.Translate ("NoneLower"));
						GUI.set_color (TabDrawer.TextColor);
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				flag = false;
				if (this.SelPawnForGear.inventory != null) {
					num += 10;
					num += TabDrawer.DrawHeader (0, num, rect5.get_width (), Translator.Translate ("Inventory"), true, 0);
					foreach (Thing current3 in this.SelPawnForGear.inventory.container) {
						this.DrawThingRow (ref num, rect5.get_width (), current3);
						flag = true;
					}
					if (!flag) {
						num += 4;
						Rect rect8 = new Rect (0, num, rect5.get_width (), 24);
						GUI.set_color (TabDrawer.SeparatorColor);
						Widgets.Label (rect8, Translator.Translate ("NoneLower"));
						GUI.set_color (TabDrawer.TextColor);
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				if (Event.get_current ().get_type () == 8) {
					this.scrollViewHeight = num + 8;
				}
				Widgets.EndScrollView ();
			}
			finally {
				GUI.EndGroup ();
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawnForGear = this.SelPawnForGear;
				if (selPawnForGear != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawnForGear);
				}
			}
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Gear_Vanilla : ITab_Pawn_Gear
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		private Pawn SelPawnForGear {
			get {
				if (base.get_SelPawn () != null) {
					return base.get_SelPawn ();
				}
				Corpse corpse = base.get_SelThing () as Corpse;
				if (corpse != null) {
					return corpse.innerPawn;
				}
				throw new InvalidOperationException ("Gear tab on non-pawn non-corpse " + base.get_SelThing ());
			}
		}

		//
		// Constructors
		//
		public ITab_Pawn_Gear_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawnForGear = this.SelPawnForGear;
				if (selPawnForGear != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawnForGear);
				}
			}
		}
	}
}
using RimWorld;
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class ITab_Pawn_Guest_Alternate : ITab_Pawn_Visitor_Alternate
	{
		//
		// Properties
		//
		public override bool IsVisible {
			get {
				return base.get_SelPawn ().get_HostFaction () == Faction.get_OfColony () && !base.get_SelPawn ().get_IsPrisoner ();
			}
		}

		//
		// Constructors
		//
		public ITab_Pawn_Guest_Alternate (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabGuest";
			base.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = new Vector2 (TabDrawer.TabPanelSize.x, 200);
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Guest_Vanilla : ITab_Pawn_Guest
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Guest_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Health_Alternate : ITab_Pawn_Health
	{
		//
		// Static Fields
		//
		private const float ThoughtLevelHeight = 25;

		private static readonly Vector2 PanelSize;

		private static readonly Color staticHighlightColor;

		private static readonly Color highlightColor;

		private static Texture2D TabAtlasTex;

		private const int HideBloodLossTicksThreshold = 60000;

		private const float topPadding = 20;

		private const float ThoughtLevelSpacing = 20;

		//
		// Fields
		//
		protected ScrollView injuriesScrollView = new ScrollView (false);

		protected ScrollView operationsScrollView = new ScrollView (false);

		protected HealthCardUtility healthCardUtility = new HealthCardUtility ();

		private bool operationsTabSelected;

		private bool highlight = true;

		//
		// Properties
		//
		private bool HideBloodLoss {
			get {
				return this.SelCorpse != null && this.SelCorpse.get_Age () > 60000;
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		private Corpse SelCorpse {
			get {
				return base.get_SelThing () as Corpse;
			}
		}

		private Pawn SelPawnForHealth {
			get {
				if (base.get_SelPawn () != null) {
					return base.get_SelPawn ();
				}
				Corpse corpse = base.get_SelThing () as Corpse;
				if (corpse != null) {
					return corpse.innerPawn;
				}
				throw new InvalidOperationException ("Health tab on non-pawn non-corpse " + base.get_SelThing ());
			}
		}

		//
		// Constructors
		//
		public ITab_Pawn_Health_Alternate (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.size = ITab_Pawn_Health_Alternate.PanelSize;
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
		}

		static ITab_Pawn_Health_Alternate ()
		{
			ITab_Pawn_Health_Alternate.TabAtlasTex = null;
			ITab_Pawn_Health_Alternate.highlightColor = new Color (0.5, 0.5, 0.5, 1);
			ITab_Pawn_Health_Alternate.staticHighlightColor = new Color (0.75, 0.75, 0.85, 1);
			ITab_Pawn_Health_Alternate.PanelSize = TabDrawer.TabPanelSize;
			ITab_Pawn_Health_Alternate.ResetTextures ();
		}

		//
		// Static Methods
		//
		public static void ResetTextures ()
		{
			ITab_Pawn_Health_Alternate.TabAtlasTex = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/TabAtlas", true);
		}

		//
		// Methods
		//
		private void DoRightRowHighlight (Rect rowRect)
		{
			if (this.highlight) {
				GUI.set_color (ITab_Pawn_Health_Alternate.staticHighlightColor);
				GUI.DrawTexture (rowRect, TexUI.HighlightTex);
			}
			this.highlight = !this.highlight;
			if (rowRect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (ITab_Pawn_Health_Alternate.highlightColor);
				GUI.DrawTexture (rowRect, TexUI.HighlightTex);
			}
		}

		protected override void FillTab ()
		{
			Pawn pawn = null;
			if (base.get_SelPawn () != null) {
				pawn = base.get_SelPawn ();
			}
			else {
				Corpse corpse = base.get_SelThing () as Corpse;
				if (corpse != null) {
					pawn = corpse.innerPawn;
				}
			}
			if (pawn == null) {
				Log.Error ("Health tab found no selected pawn to display.");
				return;
			}
			Corpse corpse2 = base.get_SelThing () as Corpse;
			bool showBloodLoss = corpse2 == null || corpse2.get_Age () < 60000;
			bool flag = !pawn.get_RaceProps ().get_Humanlike () && pawn.get_Downed ();
			bool flag2 = base.get_SelThing ().def.get_AllRecipes ().Any<RecipeDef> ();
			bool flag3 = flag2 && !pawn.get_Dead () && (pawn.get_IsColonist () || pawn.get_IsPrisonerOfColony () || flag);
			TextAnchor anchor = Text.get_Anchor ();
			Rect rect = new Rect (20, 51, ITab_Pawn_Health_Alternate.PanelSize.x - 40, 345);
			float num = this.size.y - rect.get_height () - 109;
			Rect rect2 = new Rect (rect.get_x (), rect.get_y () + rect.get_height () + 16, rect.get_width (), num);
			if (!flag3) {
				this.operationsTabSelected = false;
			}
			List<TabRecord> list = new List<TabRecord> ();
			list.Add (new TabRecord (Translator.Translate ("HealthOverview"), delegate {
				this.operationsTabSelected = false;
			}, !this.operationsTabSelected));
			if (flag3) {
				string text;
				if (pawn.get_RaceProps ().mechanoid) {
					text = Translator.Translate ("MedicalOperationsMechanoidsShort", new object[] {
						pawn.get_BillStack ().get_Count ()
					});
				}
				else {
					text = Translator.Translate ("MedicalOperationsShort", new object[] {
						pawn.get_BillStack ().get_Count ()
					});
				}
				list.Add (new TabRecord (text, delegate {
					this.operationsTabSelected = true;
				}, this.operationsTabSelected));
			}
			GUI.set_color (TabDrawer.BoxColor);
			GUI.DrawTexture (rect, TabDrawer.WhiteTexture);
			GUI.set_color (TabDrawer.BoxBorderColor);
			Widgets.DrawBox (rect, 1);
			GUI.set_color (Color.get_white ());
			TabDrawer.DrawTabs (new Rect (rect.get_x (), rect.get_y (), rect.get_width () - 90, rect.get_height ()), list, ITab_Pawn_Health_Alternate.TabAtlasTex);
			float num2 = 0;
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
			if (!this.operationsTabSelected) {
				Rect rect3 = GenUI.ContractedBy (rect, 12);
				if (pawn.playerSettings != null && !pawn.get_Dead ()) {
					Rect rect4 = new Rect (rect3.get_x () + 4, rect3.get_y () + 8, rect3.get_width (), 32);
					MedicalCareUtility.MedicalCareSetter (rect4, ref pawn.playerSettings.medCare);
					rect3.set_y (rect3.get_y () + 50);
					rect3.set_height (rect3.get_height () - 50);
				}
				try {
					GUI.BeginGroup (rect3);
					Rect leftRect = new Rect (0, num2, rect3.get_width (), rect3.get_height () - num2);
					num2 = this.healthCardUtility.DrawStatus (leftRect, pawn, num2, showBloodLoss);
				}
				finally {
					GUI.EndGroup ();
				}
			}
			else {
				Rect rect5 = GenUI.ContractedBy (rect, 12);
				try {
					GUI.BeginGroup (rect5);
					Rect rect6 = new Rect (0, 0, rect5.get_width (), rect5.get_height ());
					this.operationsScrollView.Begin (rect6);
					ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.MedicalOperations, 1);
					num2 = this.healthCardUtility.DrawMedOperationsTab (rect6, pawn, base.get_SelThing (), num2);
					this.operationsScrollView.End (num2);
				}
				finally {
					GUI.EndGroup ();
				}
			}
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
			GUI.BeginGroup (rect2);
			Rect leftRect2 = new Rect (0, 0, rect2.get_width (), rect2.get_height ());
			this.healthCardUtility.DrawInjuries (leftRect2, pawn, showBloodLoss);
			GUI.EndGroup ();
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn pawn2 = null;
				if (base.get_SelPawn () != null) {
					pawn2 = base.get_SelPawn ();
				}
				else {
					Corpse corpse3 = base.get_SelThing () as Corpse;
					if (corpse3 != null) {
						pawn2 = corpse3.innerPawn;
					}
				}
				if (pawn2 != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, pawn2);
				}
			}
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (anchor);
			Rect rect7 = new Rect (0, 0, this.size.x, this.size.y);
			if (Event.get_current ().get_type () == 6 && Mouse.IsOver (rect7)) {
				Event.get_current ().Use ();
			}
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Health_Vanilla : ITab_Pawn_Health
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Health_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn pawn = null;
				if (base.get_SelPawn () != null) {
					pawn = base.get_SelPawn ();
				}
				else {
					Corpse corpse = base.get_SelThing () as Corpse;
					if (corpse != null) {
						pawn = corpse.innerPawn;
					}
				}
				if (pawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, pawn);
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Needs_Alternate : ITab_Pawn_Needs
	{
		//
		// Static Fields
		//
		private const float ThoughtHeight = 20;

		public static float ThoughtValueWidth;

		public static Texture2D BarInstantMarkerTex;

		private static readonly Vector2 FullSize;

		private static readonly Color MoodColor;

		private static readonly Color MoodColorNegative;

		private static readonly Color NoEffectColor;

		private const float MoodNumberWidth = 32;

		private const float MoodX = 235;

		private const float ThoughtIntervalY = 24;

		private const float NeedsColumnWidth = 225;

		private const float ThoughtSpacing = 4;

		private static List<ThoughtDef> thoughtTypesPresent;

		//
		// Fields
		//
		private List<float> threshPercentsForMood = new List<float> ();

		private List<Need> displayNeeds = new List<Need> ();

		private Vector2 thoughtScrollPosition = default(Vector2);

		protected FieldInfo threshPercentsField;

		//
		// Properties
		//
		public override bool IsVisible {
			get {
				return base.get_SelPawn ().needs != null && base.get_SelPawn ().needs.get_AllNeeds ().Count > 0;
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		static ITab_Pawn_Needs_Alternate ()
		{
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent = new List<ThoughtDef> ();
			ITab_Pawn_Needs_Alternate.NoEffectColor = new Color (0.5, 0.5, 0.5, 0.75);
			ITab_Pawn_Needs_Alternate.MoodColorNegative = new Color (0.8, 0.4, 0.4);
			ITab_Pawn_Needs_Alternate.MoodColor = new Color (0.1, 1, 0.1);
			ITab_Pawn_Needs_Alternate.FullSize = new Vector2 (580, 520);
			ITab_Pawn_Needs_Alternate.ThoughtValueWidth = 40;
			ITab_Pawn_Needs_Alternate.BarInstantMarkerTex = ContentFinder<Texture2D>.Get ("UI/Misc/BarInstantMarker", true);
		}

		public ITab_Pawn_Needs_Alternate (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabNeeds";
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = TabDrawer.TabPanelSize;
			this.threshPercentsField = typeof(Need).GetField ("threshPercents", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Methods
		//
		protected void DoMoodAndThoughts (Rect rect)
		{
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (0, 0, rect.get_width (), 70);
			this.DrawMood (rect2, base.get_SelPawn (), base.get_SelPawn ().needs.mood);
			Rect rect3 = new Rect (0, 80, rect.get_width (), rect.get_height () - 120);
			rect3 = GenUI.ContractedBy (rect3, 10);
			rect3.set_x (rect3.get_x () + 10);
			rect3.set_width (rect3.get_width () - 20);
			this.DrawThoughtListing (rect3);
			GUI.EndGroup ();
		}

		private float DoNeeds (Rect rect)
		{
			this.displayNeeds.Clear ();
			List<Need> allNeeds = base.get_SelPawn ().needs.get_AllNeeds ();
			for (int i = 0; i < allNeeds.Count; i++) {
				if (allNeeds [i].def.showOnNeedList) {
					this.displayNeeds.Add (allNeeds [i]);
				}
			}
			this.displayNeeds.Sort ((Need a, Need b) => b.def.listPriority.CompareTo (a.def.listPriority));
			float num = 14;
			float num2 = num;
			int num3 = this.displayNeeds.Count / 2;
			int num4 = this.displayNeeds.Count - num3;
			int num5 = (num3 <= num4) ? num4 : num3;
			float num6 = 0;
			for (int j = 0; j < num3; j++) {
				Need need = this.displayNeeds [j];
				Rect rect2 = new Rect (rect.get_x (), rect.get_y () + num2, rect.get_width (), Mathf.Min (90, rect.get_height () / (float)num5));
				this.DrawNeed (rect2, need);
				num2 = rect2.get_yMax () - 10;
				if (num2 > num6) {
					num6 = num2;
				}
			}
			float num7 = rect.get_width () + 4;
			num2 = num;
			for (int k = num3; k < this.displayNeeds.Count; k++) {
				Need need2 = this.displayNeeds [k];
				Rect rect3 = new Rect (num7, rect.get_y () + num2, rect.get_width (), Mathf.Min (90, rect.get_height () / (float)num5));
				this.DrawNeed (rect3, need2);
				num2 = rect3.get_yMax () - 10;
				if (num2 > num6) {
					num6 = num2;
				}
			}
			return num6;
		}

		protected void DrawBarInstantMarkerAt (Rect barRect, float pct, Need need)
		{
			if (pct > 1) {
				Log.ErrorOnce (need.def + " drawing bar percent > 1 : " + pct, 6932178);
			}
			float num = 12;
			if (barRect.get_width () < 150) {
				num /= 2;
			}
			Vector2 vector = new Vector2 (barRect.get_x () + barRect.get_width () * pct, barRect.get_y () + barRect.get_height ());
			Rect rect = new Rect (vector.x - num / 2, vector.y, num, num);
			GUI.DrawTexture (rect, ITab_Pawn_Needs_Alternate.BarInstantMarkerTex);
		}

		private void DrawBarThreshold (Rect barRect, float threshPct, Need need)
		{
			float num = (float)((barRect.get_width () > 60) ? 2 : 1);
			Rect rect = new Rect (barRect.get_x () + barRect.get_width () * threshPct - (num - 1), barRect.get_y () + barRect.get_height () / 2, num, barRect.get_height () / 2);
			Texture2D texture2D;
			if (threshPct < need.get_CurLevel ()) {
				texture2D = BaseContent.BlackTex;
				GUI.set_color (new Color (1, 1, 1, 0.9));
			}
			else {
				texture2D = BaseContent.GreyTex;
				GUI.set_color (new Color (1, 1, 1, 0.5));
			}
			GUI.DrawTexture (rect, texture2D);
			GUI.set_color (Color.get_white ());
		}

		protected void DrawMood (Rect rect, Pawn pawn, Need_Mood mood)
		{
			this.threshPercentsForMood.Clear ();
			this.threshPercentsForMood.Add (pawn.mindState.breaker.get_HardBreakThreshold ());
			this.threshPercentsForMood.Add (pawn.mindState.breaker.get_SoftBreakThreshold ());
			this.DrawNeed (rect, mood, this.threshPercentsForMood);
		}

		public void DrawNeed (Rect rect, Need need, List<float> threshPercents)
		{
			if (rect.get_height () > 70) {
				float num = (rect.get_height () - 70) / 2;
				rect.set_height (70);
				rect.set_y (rect.get_y () + num);
			}
			if (Mouse.IsOver (rect)) {
				Widgets.DrawHighlight (rect);
			}
			TooltipHandler.TipRegion (rect, new TipSignal (() => need.GetTipString (), rect.GetHashCode ()));
			float num2 = 14;
			float num3 = num2 + 15;
			if (rect.get_height () < 50) {
				num2 *= Mathf.InverseLerp (0, 50, rect.get_height ());
			}
			Text.set_Font ((rect.get_height () > 55) ? 1 : 0);
			Text.set_Anchor (7);
			Rect rect2 = new Rect (rect.get_x (), rect.get_y (), rect.get_width (), rect.get_height () / 2);
			Widgets.Label (rect2, need.get_LabelCap ());
			Text.set_Anchor (0);
			Rect rect3 = new Rect (rect.get_x (), rect.get_y () + rect.get_height () / 2, rect.get_width (), rect.get_height () / 2);
			rect3 = new Rect (rect3.get_x () + num3, rect3.get_y (), rect3.get_width () - num3 * 2, rect3.get_height () - num2);
			Widgets.FillableBar (rect3, need.get_CurLevel ());
			Widgets.FillableBarChangeArrows (rect3, need.get_GUIChangeArrow ());
			if (threshPercents != null) {
				for (int i = 0; i < threshPercents.Count; i++) {
					this.DrawBarThreshold (rect3, threshPercents [i], need);
				}
			}
			float curInstantLevel = need.get_CurInstantLevel ();
			if (curInstantLevel >= 0) {
				this.DrawBarInstantMarkerAt (rect3, curInstantLevel, need);
			}
			Text.set_Font (1);
		}

		protected void DrawNeed (Rect rect, Need need)
		{
			List<float> threshPercents = (List<float>)this.threshPercentsField.GetValue (need);
			this.DrawNeed (rect, need, threshPercents);
		}

		private bool DrawThoughtGroup (Rect rect, ThoughtDef def)
		{
			float num = 12;
			float num2 = 4;
			float num3 = rect.get_width () - num - num2 - ITab_Pawn_Needs_Alternate.ThoughtValueWidth - 16;
			float num4 = num + num3;
			try {
				List<Thought> list = base.get_SelPawn ().needs.mood.thoughts.ThoughtsOfDef (def).ToList<Thought> ();
				int index = 0;
				int num5 = -1;
				for (int i = 0; i < list.Count; i++) {
					if (list [i].get_CurStageIndex () > num5) {
						num5 = list [i].get_CurStageIndex ();
						index = i;
					}
				}
				if (!list [index].get_Visible ()) {
					return false;
				}
				if (Mouse.IsOver (rect)) {
					Widgets.DrawHighlight (rect);
				}
				if (def.get_DurationTicks () > 5) {
					StringBuilder stringBuilder = new StringBuilder ();
					stringBuilder.Append (list [index].get_Description ());
					stringBuilder.AppendLine ();
					stringBuilder.AppendLine ();
					Thought_Memory thought_Memory = list [index] as Thought_Memory;
					if (thought_Memory != null) {
						if (list.Count == 1) {
							stringBuilder.Append (Translator.Translate ("ThoughtExpiresIn", new object[] {
								GenDate.TickstoDaysString (def.get_DurationTicks () - thought_Memory.age)
							}));
						}
						else {
							Thought_Memory thought_Memory2 = (Thought_Memory)list [list.Count - 1];
							stringBuilder.Append (Translator.Translate ("ThoughtStartsExpiringIn", new object[] {
								GenDate.TickstoDaysString (def.get_DurationTicks () - thought_Memory.age)
							}));
							stringBuilder.AppendLine ();
							stringBuilder.Append (Translator.Translate ("ThoughtFinishesExpiringIn", new object[] {
								GenDate.TickstoDaysString (def.get_DurationTicks () - thought_Memory2.age)
							}));
						}
					}
					TooltipHandler.TipRegion (rect, new TipSignal (stringBuilder.ToString (), 7291));
				}
				else {
					TooltipHandler.TipRegion (rect, new TipSignal (list [index].get_Description (), 7141));
				}
				Text.set_WordWrap (false);
				Text.set_Anchor (3);
				Rect rect2 = new Rect (rect.get_x () + num, rect.get_y (), num3, rect.get_height ());
				rect2.set_yMin (rect2.get_yMin () - 3);
				rect2.set_yMax (rect2.get_yMax () + 3);
				string text = list [index].get_LabelCap ();
				if (list.Count > 1) {
					text = text + " x" + list.Count;
				}
				Widgets.Label (rect2, text);
				Text.set_Anchor (4);
				float num6 = base.get_SelPawn ().needs.mood.thoughts.MoodOffsetOfThoughtGroup (def);
				if (num6 == 0) {
					GUI.set_color (ITab_Pawn_Needs_Alternate.NoEffectColor);
				}
				else if (num6 > 0) {
					GUI.set_color (ITab_Pawn_Needs_Alternate.MoodColor);
				}
				else {
					GUI.set_color (ITab_Pawn_Needs_Alternate.MoodColorNegative);
				}
				Rect rect3 = new Rect (rect.get_x () + num4, rect.get_y (), ITab_Pawn_Needs_Alternate.ThoughtValueWidth, rect.get_height ());
				Text.set_Anchor (5);
				Widgets.Label (rect3, num6.ToString ("##0"));
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
				Text.set_WordWrap (true);
			}
			catch (Exception ex) {
				Log.ErrorOnce (string.Concat (new object[] {
					"Exception in DrawThoughtGroup for ",
					def,
					" on ",
					base.get_SelPawn (),
					": ",
					ex.ToString ()
				}), 3452698);
			}
			return true;
		}

		private void DrawThoughtListing (Rect listingRect)
		{
			Text.set_Font (1);
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Clear ();
			ITab_Pawn_Needs_Alternate.thoughtTypesPresent.AddRange (from th in base.get_SelPawn ().needs.mood.thoughts.get_DistinctThoughtDefs ()
			orderby base.get_SelPawn ().needs.mood.thoughts.MoodOffsetOfThoughtGroup (th) descending
			select th);
			float num = (float)ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Count * 24;
			Widgets.BeginScrollView (listingRect, ref this.thoughtScrollPosition, new Rect (0, 0, listingRect.get_width () - 16, num));
			Text.set_Anchor (3);
			float num2 = 0;
			for (int i = 0; i < ITab_Pawn_Needs_Alternate.thoughtTypesPresent.Count; i++) {
				Rect rect = new Rect (0, num2, listingRect.get_width (), 20);
				if (this.DrawThoughtGroup (rect, ITab_Pawn_Needs_Alternate.thoughtTypesPresent [i])) {
					num2 += 24;
				}
			}
			Widgets.EndScrollView ();
			Text.set_Anchor (0);
		}

		protected override void FillTab ()
		{
			Rect rect = GenUI.ContractedBy (new Rect (0, 0, this.size.x, this.size.y), 1);
			try {
				GUI.BeginGroup (rect);
				Rect rect2 = new Rect (0, 0, rect.get_width () / 2, this.size.y);
				float num = this.DoNeeds (rect2);
				num += 10;
				if (base.get_SelPawn ().needs.mood != null) {
					Rect rect3 = new Rect (0, num, rect.get_width (), rect.get_height () - num);
					this.DoMoodAndThoughts (rect3);
				}
			}
			finally {
				GUI.EndGroup ();
				GUI.set_color (Color.get_white ());
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}

		public override void OnOpen ()
		{
			this.thoughtScrollPosition = default(Vector2);
			this.size = TabDrawer.TabPanelSize;
		}

		private void UpdateDisplayNeeds ()
		{
			this.displayNeeds.Clear ();
			List<Need> allNeeds = base.get_SelPawn ().needs.get_AllNeeds ();
			for (int i = 0; i < allNeeds.Count; i++) {
				if (allNeeds [i].def.showOnNeedList) {
					this.displayNeeds.Add (allNeeds [i]);
				}
			}
			this.displayNeeds.Sort ((Need a, Need b) => b.def.listPriority.CompareTo (a.def.listPriority));
		}

		protected override void UpdateSize ()
		{
			this.UpdateDisplayNeeds ();
			if (base.get_SelPawn ().needs.mood != null) {
				this.size = TabDrawer.TabPanelSize;
			}
			else {
				this.size = new Vector2 (TabDrawer.TabPanelSize.x, (float)this.displayNeeds.Count * Mathf.Min (70, ITab_Pawn_Needs_Alternate.FullSize.y / (float)this.displayNeeds.Count) + 16);
			}
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Needs_Vanilla : ITab_Pawn_Needs
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Needs_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class ITab_Pawn_Prisoner_Alternate : ITab_Pawn_Visitor_Alternate
	{
		//
		// Properties
		//
		public override bool IsVisible {
			get {
				return base.get_SelPawn ().get_IsPrisonerOfColony ();
			}
		}

		//
		// Constructors
		//
		public ITab_Pawn_Prisoner_Alternate (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.labelKey = "TabPrisoner";
			this.tutorHighlightTag = "TabPrisoner";
			base.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
			this.size = new Vector2 (TabDrawer.TabPanelSize.x, 356);
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Prisoner_Vanilla : ITab_Pawn_Prisoner
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Prisoner_Vanilla (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Training_Alternate : ITab_Pawn_Training
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Training_Alternate (PreferenceTabBrowseButtons prefBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = prefBrowseButtons;
			this.size = new Vector2 (TabDrawer.TabPanelSize.x, 356);
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			Rect rect = GenUI.ContractedBy (new Rect (0, 0, this.size.x, this.size.y), 20);
			TrainingCardUtility.DrawTrainingCard (rect, base.get_SelPawn ());
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class ITab_Pawn_Training_Vanilla : ITab_Pawn_Training
	{
		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Training_Vanilla (PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			base.FillTab ();
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class ITab_Pawn_Visitor_Alternate : ITab_Pawn_Visitor
	{
		//
		// Static Fields
		//
		private const float CheckboxInterval = 30;

		private const float CheckboxMargin = 50;

		//
		// Properties
		//
		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons {
			get;
			set;
		}

		//
		// Constructors
		//
		public ITab_Pawn_Visitor_Alternate ()
		{
			this.size = new Vector2 (TabDrawer.TabPanelSize.x, TabDrawer.TabPanelSize.y);
		}

		//
		// Methods
		//
		protected override void FillTab ()
		{
			ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.PrisonerTab, 1);
			Text.set_Font (1);
			Rect rect = GenUI.ContractedBy (new Rect (0, 0, this.size.x, this.size.y), 20);
			bool isPrisonerOfColony = base.get_SelPawn ().get_IsPrisonerOfColony ();
			try {
				GUI.BeginGroup (rect);
				float num = 10;
				Rect rect2 = new Rect (10, num, rect.get_width () - 20, 32);
				MedicalCareUtility.MedicalCareSetter (rect2, ref base.get_SelPawn ().playerSettings.medCare);
				num += 32;
				num += 18;
				Rect rect3 = new Rect (10, num, rect.get_width () - 28, rect.get_height () - num);
				bool getsFood = base.get_SelPawn ().guest.get_GetsFood ();
				num += WidgetDrawer.DrawLabeledCheckbox (rect3, Translator.Translate ("GetsFood"), ref getsFood);
				base.get_SelPawn ().guest.set_GetsFood (getsFood);
				if (isPrisonerOfColony) {
					num += 6;
					int length = Enum.GetValues (typeof(PrisonerInteractionMode)).Length;
					float num2 = (float)(length * 28 + 20);
					Rect rect4 = new Rect (0, num, rect.get_width (), num2);
					TabDrawer.DrawBox (rect4);
					Rect rect5 = GenUI.ContractedBy (rect4, 10);
					rect5.set_height (28);
					foreach (PrisonerInteractionMode prisonerInteractionMode in Enum.GetValues (typeof(PrisonerInteractionMode))) {
						if (WidgetDrawer.DrawLabeledRadioButton (rect5, PrisonerInteractionModeUtility.GetLabel (prisonerInteractionMode), base.get_SelPawn ().guest.interactionMode == prisonerInteractionMode, true)) {
							base.get_SelPawn ().guest.interactionMode = prisonerInteractionMode;
						}
						rect5.set_y (rect5.get_y () + 28);
					}
					Rect rect6 = new Rect (rect4.get_x (), rect4.get_y () + rect4.get_height () + 5, rect4.get_width () - 4, 28);
					Text.set_Anchor (2);
					Widgets.Label (rect6, Translator.Translate ("RecruitmentDifficulty") + ": " + base.get_SelPawn ().guest.get_RecruitDifficulty ().ToString ("##0"));
					Text.set_Anchor (0);
				}
			}
			finally {
				GUI.EndGroup ();
				GUI.set_color (Color.get_white ());
				Text.set_Anchor (0);
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value) {
				Pawn selPawn = base.get_SelPawn ();
				if (selPawn != null) {
					BrowseButtonDrawer.DrawBrowseButtons (this.size, selPawn);
				}
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public interface IUpdatedComponent
	{
		//
		// Methods
		//
		void Update ();
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class LessonsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "ActiveLessons";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			ActiveTutorNoteManager.ActiveLessonManagerOnGUI ();
		}
	}
}
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
		//
		// Fields
		//
		private Color borderColor = new Color (0.3593, 0.3672, 0.3789);

		protected float scrollableContentHeight;

		protected Vector2 scrollableContentViewPosition = Vector2.get_zero ();

		private bool supportsMultiSelect;

		private Texture backgroundColor;

		private Texture selectedTexture;

		private List<T> items = new List<T> ();

		private List<T> selectedItems = new List<T> ();

		private List<int> selectedIndices = new List<int> ();

		private D itemDrawer;

		private List<Texture> rowTextures = new List<Texture> ();

		//
		// Properties
		//
		public Color BackgroundColor {
			set {
				this.backgroundColor = SolidColorMaterials.NewSolidColorTexture (value);
			}
		}

		public int Count {
			get {
				return this.items.Count;
			}
		}

		public List<T> Items {
			get {
				return this.items;
			}
		}

		public List<int> SelectedIndices {
			get {
				return this.selectedIndices;
			}
			set {
				this.selectedIndices.Clear ();
				this.selectedIndices.AddRange (value);
				this.SendSelectionEvent ();
			}
		}

		public List<T> SelectedItems {
			get {
				this.selectedItems.Clear ();
				foreach (int current in this.selectedIndices) {
					this.selectedItems.Add (this.items [current]);
				}
				return this.selectedItems;
			}
		}

		public bool SupportsMultiSelect {
			get {
				return this.supportsMultiSelect;
			}
			set {
				this.supportsMultiSelect = value;
			}
		}

		//
		// Constructors
		//
		public ListWidget (List<T> items, D itemDrawer) : this (itemDrawer)
		{
			this.items = new List<T> (items);
		}

		public ListWidget (D itemDrawer)
		{
			this.itemDrawer = itemDrawer;
			this.selectedTexture = SolidColorMaterials.NewSolidColorTexture (new Color (0.2656, 0.2773, 0.2891));
			this.rowTextures.Add (SolidColorMaterials.NewSolidColorTexture (new Color (0.1523, 0.168, 0.1836)));
			this.rowTextures.Add (SolidColorMaterials.NewSolidColorTexture (new Color (0.1094, 0.125, 0.1406)));
			this.backgroundColor = SolidColorMaterials.NewSolidColorTexture (new Color (0.0664, 0.082, 0.0938));
		}

		//
		// Methods
		//
		public void Add (T item)
		{
			this.items.Add (item);
		}

		public void AddToSelection (int index)
		{
			if (index < 0 || index >= this.items.Count) {
				return;
			}
			if (!this.selectedIndices.Contains (index)) {
				this.selectedIndices.Add (index);
				this.SendSelectionEvent ();
			}
		}

		public void ClearSelection ()
		{
			if (this.selectedIndices.Count > 0) {
				this.selectedIndices.Clear ();
				this.SendSelectionEvent ();
			}
		}

		public void DrawWidget (Rect bounds)
		{
			GUI.set_color (Color.get_white ());
			GUI.DrawTexture (bounds, this.backgroundColor);
			GUI.set_color (this.borderColor);
			Widgets.DrawBox (bounds, 1);
			Rect rect = GenUI.ContractedBy (bounds, 1);
			try {
				GUI.BeginGroup (rect);
				Rect rect2 = new Rect (0, 0, rect.get_width (), rect.get_height ());
				Rect rect3 = new Rect (rect2.get_x (), rect2.get_y (), rect2.get_width () - 16, this.scrollableContentHeight);
				try {
					Widgets.BeginScrollView (rect2, ref this.scrollableContentViewPosition, rect3);
					Vector2 cursor = new Vector2 (0, 0);
					for (int i = 0; i < this.items.Count; i++) {
						bool flag = this.selectedIndices.Contains (i);
						T item = this.items [i];
						float height = this.itemDrawer.GetHeight (i, item, cursor, rect.get_width (), flag, false);
						Rect rect4 = new Rect (cursor.x, cursor.y, rect.get_width (), height);
						Texture texture = null;
						if (flag) {
							texture = this.selectedTexture;
						}
						else if (this.rowTextures.Count > 0) {
							texture = this.rowTextures [i % this.rowTextures.Count];
						}
						if (this.backgroundColor != null) {
							GUI.set_color (Color.get_white ());
							GUI.DrawTexture (rect4, texture);
						}
						cursor = this.itemDrawer.Draw (i, item, cursor, rect.get_width (), flag, false);
						if (Widgets.InvisibleButton (rect4)) {
							if (this.SupportsMultiSelect) {
								if (Event.get_current ().get_control ()) {
									this.ToggleSelection (i);
								}
								else if (Event.get_current ().get_shift ()) {
									this.SelectThrough (i);
								}
								else {
									this.Select (i);
								}
							}
							else {
								this.Select (i);
							}
						}
					}
					if (Event.get_current ().get_type () == 8) {
						this.scrollableContentHeight = cursor.y;
					}
				}
				finally {
					Widgets.EndScrollView ();
				}
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				GUI.EndGroup ();
			}
			GUI.set_color (Color.get_white ());
		}

		public void Insert (T item, int index)
		{
			this.items.Insert (index, item);
		}

		public bool Remove (T item)
		{
			int num = this.items.IndexOf (item);
			if (num > -1) {
				this.RemoveAt (num);
				return true;
			}
			return false;
		}

		public void RemoveAt (int index)
		{
			if (index > -1) {
				this.items.RemoveAt (index);
				this.selectedIndices.Remove (index);
				return;
			}
			throw new IndexOutOfRangeException ();
		}

		public void RemoveFromSelection (int index)
		{
			if (index < 0 || index >= this.items.Count) {
				return;
			}
			if (this.selectedIndices.Contains (index)) {
				this.selectedIndices.Remove (index);
				this.SendSelectionEvent ();
			}
		}

		public void Reset ()
		{
			this.items.Clear ();
			this.selectedIndices.Clear ();
		}

		public void ResetItems (List<T> items)
		{
			this.items = new List<T> (items);
			this.ClearSelection ();
		}

		public void Select (T item)
		{
			int num = this.items.IndexOf (item);
			if (num > -1) {
				this.Select (num);
			}
		}

		public void Select (int index)
		{
			if (index < 0 || index >= this.items.Count) {
				return;
			}
			if (this.selectedIndices.Count != 1 || this.selectedIndices [0] != index) {
				this.selectedIndices.Clear ();
				this.selectedIndices.Add (index);
				this.SendSelectionEvent ();
			}
		}

		public void Select (HashSet<T> itemSet)
		{
			this.selectedIndices.Clear ();
			int num = 0;
			foreach (T current in this.items) {
				if (itemSet.Contains (current)) {
					this.selectedIndices.Add (num);
				}
				num++;
			}
			this.SendSelectionEvent ();
		}

		public void SelectThrough (int index)
		{
			if (index < 0 || index >= this.items.Count) {
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (this.selectedIndices.Count == 0) {
				num = 0;
				num2 = index;
				num3 = 1;
			}
			else {
				int num4 = this.selectedIndices.Last<int> ();
				if (index < num4) {
					num = index;
					num2 = num4;
					num3 = 1;
				}
				else if (index > num4) {
					num = index;
					num2 = num4;
					num3 = -1;
				}
			}
			List<int> list = new List<int> ();
			if (num3 == 1) {
				for (int i = num; i <= num2; i++) {
					list.Add (i);
				}
			}
			else if (num3 == -1) {
				for (int j = num; j >= num2; j--) {
					list.Add (j);
				}
			}
			this.selectedIndices = list;
			this.SendSelectionEvent ();
		}

		private void SendSelectionEvent ()
		{
			if (this.MultiSelectionChangedEvent != null) {
				this.MultiSelectionChangedEvent (this.SelectedItems);
			}
			if (this.SingleSelectionChangedEvent != null) {
				if (this.selectedIndices.Count == 1) {
					this.SingleSelectionChangedEvent (this.items [this.selectedIndices [0]]);
				}
				else if (this.selectedIndices.Count == 0) {
					this.SingleSelectionChangedEvent (default(T));
				}
			}
		}

		public void ToggleSelection (int index)
		{
			if (index < 0 || index >= this.items.Count) {
				return;
			}
			if (this.selectedIndices.Contains (index)) {
				this.selectedIndices.Remove (index);
			}
			else {
				this.selectedIndices.Add (index);
			}
			this.SendSelectionEvent ();
		}

		//
		// Events
		//
		public event ListWidgetMultiSelectionChangedHandler<T> MultiSelectionChangedEvent {
			add {
				ListWidgetMultiSelectionChangedHandler<T> listWidgetMultiSelectionChangedHandler = this.MultiSelectionChangedEvent;
				ListWidgetMultiSelectionChangedHandler<T> listWidgetMultiSelectionChangedHandler2;
				do {
					listWidgetMultiSelectionChangedHandler2 = listWidgetMultiSelectionChangedHandler;
					listWidgetMultiSelectionChangedHandler = Interlocked.CompareExchange<ListWidgetMultiSelectionChangedHandler<T>> (ref this.MultiSelectionChangedEvent, (ListWidgetMultiSelectionChangedHandler<T>)Delegate.Combine (listWidgetMultiSelectionChangedHandler2, value), listWidgetMultiSelectionChangedHandler);
				}
				while (listWidgetMultiSelectionChangedHandler != listWidgetMultiSelectionChangedHandler2);
			}
			remove {
				ListWidgetMultiSelectionChangedHandler<T> listWidgetMultiSelectionChangedHandler = this.MultiSelectionChangedEvent;
				ListWidgetMultiSelectionChangedHandler<T> listWidgetMultiSelectionChangedHandler2;
				do {
					listWidgetMultiSelectionChangedHandler2 = listWidgetMultiSelectionChangedHandler;
					listWidgetMultiSelectionChangedHandler = Interlocked.CompareExchange<ListWidgetMultiSelectionChangedHandler<T>> (ref this.MultiSelectionChangedEvent, (ListWidgetMultiSelectionChangedHandler<T>)Delegate.Remove (listWidgetMultiSelectionChangedHandler2, value), listWidgetMultiSelectionChangedHandler);
				}
				while (listWidgetMultiSelectionChangedHandler != listWidgetMultiSelectionChangedHandler2);
			}
		}

		public event ListWidgetSingleSelectionChangedHandler<T> SingleSelectionChangedEvent {
			add {
				ListWidgetSingleSelectionChangedHandler<T> listWidgetSingleSelectionChangedHandler = this.SingleSelectionChangedEvent;
				ListWidgetSingleSelectionChangedHandler<T> listWidgetSingleSelectionChangedHandler2;
				do {
					listWidgetSingleSelectionChangedHandler2 = listWidgetSingleSelectionChangedHandler;
					listWidgetSingleSelectionChangedHandler = Interlocked.CompareExchange<ListWidgetSingleSelectionChangedHandler<T>> (ref this.SingleSelectionChangedEvent, (ListWidgetSingleSelectionChangedHandler<T>)Delegate.Combine (listWidgetSingleSelectionChangedHandler2, value), listWidgetSingleSelectionChangedHandler);
				}
				while (listWidgetSingleSelectionChangedHandler != listWidgetSingleSelectionChangedHandler2);
			}
			remove {
				ListWidgetSingleSelectionChangedHandler<T> listWidgetSingleSelectionChangedHandler = this.SingleSelectionChangedEvent;
				ListWidgetSingleSelectionChangedHandler<T> listWidgetSingleSelectionChangedHandler2;
				do {
					listWidgetSingleSelectionChangedHandler2 = listWidgetSingleSelectionChangedHandler;
					listWidgetSingleSelectionChangedHandler = Interlocked.CompareExchange<ListWidgetSingleSelectionChangedHandler<T>> (ref this.SingleSelectionChangedEvent, (ListWidgetSingleSelectionChangedHandler<T>)Delegate.Remove (listWidgetSingleSelectionChangedHandler2, value), listWidgetSingleSelectionChangedHandler);
				}
				while (listWidgetSingleSelectionChangedHandler != listWidgetSingleSelectionChangedHandler2);
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public interface ListWidgetItemDrawer<T>
	{
		//
		// Methods
		//
		Vector2 Draw (int index, T item, Vector2 cursor, float width, bool selected, bool disabled);

		float GetHeight (int index, T item, Vector2 cursor, float width, bool selected, bool disabled);
	}
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ListWidgetLabelDrawer<T> : ListWidgetItemDrawer<T>
	{
		//
		// Fields
		//
		private Vector2 padding = new Vector2 (16, 5);

		private Color selectedTextColor = new Color (1, 1, 1);

		private Color textColor = new Color (0.85, 0.85, 0.85);

		private Func<T, string> labelDelegate;

		private bool wrap = true;

		private List<Texture> rowTextures = new List<Texture> ();

		private Vector2 paddingTotal;

		//
		// Constructors
		//
		public ListWidgetLabelDrawer (Func<T, string> labelDelegate)
		{
			this.paddingTotal = new Vector2 (this.padding.x * 2, this.padding.y * 2);
			this.labelDelegate = labelDelegate;
		}

		public ListWidgetLabelDrawer ()
		{
			this.paddingTotal = new Vector2 (this.padding.x * 2, this.padding.y * 2);
			this.labelDelegate = delegate (T item) {
				string text = item as string;
				return (text == null) ? "null" : text;
			};
		}

		//
		// Methods
		//
		public void AddRowColor (Color color)
		{
			this.rowTextures.Add (SolidColorMaterials.NewSolidColorTexture (color));
		}

		public void ClearRowColors ()
		{
			this.rowTextures.Clear ();
		}

		public Vector2 Draw (int index, T item, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string text = this.labelDelegate.Invoke (item);
			Text.set_Anchor (3);
			Vector2 result;
			try {
				Rect rect;
				Rect rect2;
				if (this.wrap) {
					float num = Text.CalcHeight (text, width - this.paddingTotal.x);
					rect = new Rect (cursor.x + this.padding.x, cursor.y + this.padding.y, width - this.paddingTotal.x, num);
					rect2 = new Rect (cursor.x, cursor.y, width, this.paddingTotal.y + num);
				}
				else {
					Vector2 vector = Text.CalcSize (text);
					rect = new Rect (cursor.x + this.padding.x, cursor.y + cursor.y, vector.x, vector.y);
					rect2 = new Rect (cursor.x, cursor.y, width, this.paddingTotal.y + vector.y);
				}
				if (selected) {
					GUI.set_color (this.selectedTextColor);
				}
				else {
					GUI.set_color (this.textColor);
				}
				Widgets.Label (rect, text);
				result = new Vector2 (cursor.x, cursor.y + rect2.get_height ());
			}
			finally {
				Text.set_Anchor (0);
			}
			return result;
		}

		public float GetHeight (int index, T item, Vector2 cursor, float width, bool selected, bool disabled)
		{
			string text = this.labelDelegate.Invoke (item);
			float num;
			if (this.wrap) {
				num = Text.CalcHeight (text, width - this.paddingTotal.x);
			}
			else {
				num = Text.CalcSize (text).y;
			}
			return num + this.paddingTotal.y;
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public delegate void ListWidgetMultiSelectionChangedHandler<T> (List<T> selected);
}
using System;

namespace EdB.Interface
{
	public delegate void ListWidgetSingleSelectionChangedHandler<T> (T selected);
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public static class MainMenuDrawer
	{
		//
		// Static Fields
		//
		private const float GameRectWidth = 200;

		private static readonly Texture2D IconTwitter = ContentFinder<Texture2D>.Get ("UI/HeroArt/WebIcons/Twitter", true);

		private static readonly Texture2D IconBook = ContentFinder<Texture2D>.Get ("UI/HeroArt/WebIcons/Book", true);

		private static readonly Texture2D IconBlog = ContentFinder<Texture2D>.Get ("UI/HeroArt/WebIcons/Blog", true);

		private static bool anyMapFiles;

		private static bool anyWorldFiles;

		private static readonly Texture2D IconForums = ContentFinder<Texture2D>.Get ("UI/HeroArt/WebIcons/Forums", true);

		private static readonly Vector2 PaneSize = new Vector2 (450, 450);

		private const float TitleShift = 50;

		private const int ButCount = 3;

		private const float NewsRectWidth = 350;

		private static readonly Vector2 LudeonLogoSize = new Vector2 (200, 58);

		private static readonly Texture2D TexLudeonLogo = ContentFinder<Texture2D>.Get ("UI/HeroArt/LudeonLogoSmall", true);

		private static readonly Vector2 TitleSize = new Vector2 (1034, 158);

		private static readonly Texture2D TexTitle = ContentFinder<Texture2D>.Get ("UI/HeroArt/GameTitle", true);

		//
		// Static Methods
		//
		private static void CloseMainTab ()
		{
			if (Game.get_Mode () == 2) {
				Find.get_MainTabsRoot ().EscapeCurrentTab (false);
			}
		}

		public static void DoMainMenuButtons (Rect rect, bool anyWorldFiles, bool anyMapFiles, Action backToGameButtonAction = null)
		{
			Rect rect2 = new Rect (0, 0, 200, rect.get_height ());
			Rect rect3 = new Rect (rect2.get_xMax () + 17, 0, -1, rect.get_height ());
			rect3.set_xMax (rect.get_width ());
			Text.set_Font (1);
			List<ListableOption> list = new List<ListableOption> ();
			ListableOption item;
			if (Game.get_Mode () == null) {
				item = new ListableOption (Translator.Translate ("CreateWorld"), delegate {
					MapInitData.Reset ();
					Find.get_WindowStack ().Add (new Page_CreateWorldParams ());
				});
				list.Add (item);
				if (anyWorldFiles) {
					item = new ListableOption (Translator.Translate ("NewColony"), delegate {
						MapInitData.Reset ();
						Find.get_WindowStack ().Add (new Page_SelectStoryteller ());
					});
					list.Add (item);
				}
			}
			if (Game.get_Mode () == 2) {
				if (backToGameButtonAction != null) {
					item = new ListableOption (Translator.Translate ("BackToGame"), backToGameButtonAction);
					list.Add (item);
				}
				item = new ListableOption (Translator.Translate ("Save"), delegate {
					MainMenuDrawer.CloseMainTab ();
					Find.get_WindowStack ().Add (new Dialog_MapList_Save ());
				});
				list.Add (item);
			}
			if (anyMapFiles) {
				item = new ListableOption (Translator.Translate ("Load"), delegate {
					MainMenuDrawer.CloseMainTab ();
					Find.get_WindowStack ().Add (new Dialog_MapList_Load ());
				});
				list.Add (item);
			}
			item = new ListableOption (Translator.Translate ("EdB.InterfaceOptions.Button.GameOptions"), delegate {
				MainMenuDrawer.CloseMainTab ();
				Find.get_WindowStack ().Add (new Dialog_Options ());
			});
			list.Add (item);
			if (Preferences.Instance.AtLeastOne) {
				item = new ListableOption (Translator.Translate ("EdB.InterfaceOptions.Button.InterfaceOptions"), delegate {
					MainMenuDrawer.CloseMainTab ();
					Find.get_WindowStack ().Add (new Dialog_InterfaceOptions ());
				});
				list.Add (item);
			}
			if (Game.get_Mode () == null) {
				item = new ListableOption (Translator.Translate ("Mods"), delegate {
					Find.get_WindowStack ().Add (new Page_ModsConfig ());
				});
				list.Add (item);
				item = new ListableOption (Translator.Translate ("Credits"), delegate {
					Find.get_WindowStack ().Add (new Page_Credits ());
				});
				list.Add (item);
			}
			if (Game.get_Mode () == 2) {
				Action action = delegate {
					Find.get_WindowStack ().Add (new Dialog_Confirm (Translator.Translate ("ConfirmQuit"), delegate {
						Application.LoadLevel ("Entry");
					}, true));
				};
				item = new ListableOption (Translator.Translate ("QuitToMainMenu"), action);
				list.Add (item);
				Action action2 = delegate {
					Find.get_WindowStack ().Add (new Dialog_Confirm (Translator.Translate ("ConfirmQuit"), delegate {
						Root.Shutdown ();
					}, true));
				};
				item = new ListableOption (Translator.Translate ("QuitToOS"), action2);
				list.Add (item);
			}
			else {
				item = new ListableOption (Translator.Translate ("QuitToOS"), delegate {
					Root.Shutdown ();
				});
				list.Add (item);
			}
			Rect rect4 = GenUI.ContractedBy (rect2, 17);
			OptionListingUtility.DrawOptionListing (rect4, list);
			Text.set_Font (1);
			List<ListableOption> list2 = new List<ListableOption> ();
			ListableOption item2 = new ListableOption_WebLink (Translator.Translate ("FictionPrimer"), "https://docs.google.com/document/d/1pIZyKif0bFbBWten4drrm7kfSSfvBoJPgG9-ywfN8j8/pub", MainMenuDrawer.IconBlog);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("LudeonBlog"), "http://ludeon.com/blog", MainMenuDrawer.IconBlog);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("Forums"), "http://ludeon.com/forums", MainMenuDrawer.IconForums);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("OfficialWiki"), "http://rimworldwiki.com", MainMenuDrawer.IconBlog);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("TynansTwitter"), "https://twitter.com/TynanSylvester", MainMenuDrawer.IconTwitter);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("TynansDesignBook"), "http://tynansylvester.com/book", MainMenuDrawer.IconBook);
			list2.Add (item2);
			item2 = new ListableOption_WebLink (Translator.Translate ("HelpTranslate"), "http://ludeon.com/forums/index.php?topic=2933.0", MainMenuDrawer.IconForums);
			list2.Add (item2);
			Rect rect5 = GenUI.ContractedBy (rect3, 17);
			float num = OptionListingUtility.DrawOptionListing (rect5, list2);
			GUI.BeginGroup (rect5);
			if (Game.get_Mode () == null && Widgets.ImageButton (new Rect (0, num + 10, 64, 32), LanguageDatabase.activeLanguage.icon)) {
				List<FloatMenuOption> list3 = new List<FloatMenuOption> ();
				foreach (LoadedLanguage current in LanguageDatabase.get_AllLoadedLanguages ()) {
					LoadedLanguage localLang = current;
					list3.Add (new FloatMenuOption (localLang.get_FriendlyNameNative (), delegate {
						LanguageDatabase.SelectLanguage (localLang);
						Prefs.Save ();
					}, 1, null, null));
				}
				Find.get_WindowStack ().Add (new FloatMenu (list3, false));
			}
			GUI.EndGroup ();
		}

		public static void Init ()
		{
			if (!PlayDataLoader.loaded) {
				PlayDataLoader.LoadAllPlayData (false);
			}
			ConceptDatabase.Save ();
			ShipCountdown.CancelCountdown ();
			MainMenuDrawer.anyWorldFiles = SavedWorldsDatabase.get_AllWorldFiles ().Any<FileInfo> ();
			MainMenuDrawer.anyMapFiles = MapFilesUtility.get_AllMapFiles ().Any<FileInfo> ();
		}

		public static void MainMenuOnGUI ()
		{
			VersionControl.DrawInfoInCorner ();
			Rect rect = new Rect ((float)(Screen.get_width () / 2) - MainMenuDrawer.PaneSize.x / 2, (float)(Screen.get_height () / 2) - MainMenuDrawer.PaneSize.y / 2, MainMenuDrawer.PaneSize.x, MainMenuDrawer.PaneSize.y);
			rect.set_y (rect.get_y () + 50);
			rect.set_x ((float)Screen.get_width () - rect.get_width () - 30);
			Vector2 vector = MainMenuDrawer.TitleSize;
			if (vector.x > (float)Screen.get_width ()) {
				vector *= (float)Screen.get_width () / vector.x;
			}
			vector *= 0.7;
			Rect rect2 = new Rect ((float)(Screen.get_width () / 2) - vector.x / 2, rect.get_y () - vector.y - 10, vector.x, vector.y);
			rect2.set_x ((float)Screen.get_width () - vector.x - 50);
			GUI.DrawTexture (rect2, MainMenuDrawer.TexTitle, 0, true);
			Rect rect3 = rect2;
			rect3.set_y (rect3.get_y () + rect2.get_height ());
			rect3.set_xMax (rect3.get_xMax () - 55);
			rect3.set_height (30);
			rect3.set_y (rect3.get_y () + 3);
			string text = Translator.Translate ("MainPageCredit");
			Text.set_Font (2);
			Text.set_Anchor (2);
			if (Screen.get_width () < 990) {
				Rect rect4 = rect3;
				rect4.set_xMin (rect4.get_xMax () - Text.CalcSize (text).x);
				rect4.set_xMin (rect4.get_xMin () - 4);
				rect4.set_xMax (rect4.get_xMax () + 4);
				GUI.set_color (new Color (0.2, 0.2, 0.2, 0.5));
				GUI.DrawTexture (rect4, BaseContent.WhiteTex);
				GUI.set_color (Color.get_white ());
			}
			Widgets.Label (rect3, text);
			Text.set_Anchor (0);
			Text.set_Font (1);
			GUI.set_color (new Color (1, 1, 1, 0.5));
			Rect rect5 = new Rect ((float)(Screen.get_width () - 8) - MainMenuDrawer.LudeonLogoSize.x, 8, MainMenuDrawer.LudeonLogoSize.x, MainMenuDrawer.LudeonLogoSize.y);
			GUI.DrawTexture (rect5, MainMenuDrawer.TexLudeonLogo, 0, true);
			GUI.set_color (Color.get_white ());
			Rect rect6 = GenUI.ContractedBy (rect, 17);
			GUI.BeginGroup (rect6);
			MainMenuDrawer.DoMainMenuButtons (rect6, MainMenuDrawer.anyWorldFiles, MainMenuDrawer.anyMapFiles, null);
			GUI.EndGroup ();
		}

		public static void Notify_WorldFilesChanged ()
		{
			MainMenuDrawer.anyWorldFiles = SavedWorldsDatabase.get_AllWorldFiles ().Any<FileInfo> ();
		}
	}
}
using System;

namespace EdB.Interface
{
	public class MainTabsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Static Fields
		//
		public static readonly string ComponentName = "MainTabs";

		//
		// Fields
		//
		private MainTabsRoot mainTabsRoot;

		//
		// Properties
		//
		public MainTabsRoot MainTabsRoot {
			get {
				return this.mainTabsRoot;
			}
		}

		public string Name {
			get {
				return MainTabsComponent.ComponentName;
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Constructors
		//
		public MainTabsComponent (MainTabsRoot mainTabsRoot)
		{
			this.mainTabsRoot = mainTabsRoot;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.mainTabsRoot.MainTabsOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabsRoot
	{
		//
		// Fields
		//
		private List<MainTabDef> allTabs;

		//
		// Properties
		//
		public List<MainTabDef> AllTabs {
			get {
				return this.allTabs;
			}
		}

		public MainTabDef OpenTab {
			get {
				MainTabWindow mainTabWindow = Find.get_WindowStack ().WindowOfType<MainTabWindow> ();
				if (mainTabWindow == null) {
					return null;
				}
				return mainTabWindow.def;
			}
		}

		private int TabButtonsCount {
			get {
				int num = 0;
				for (int i = 0; i < this.allTabs.Count; i++) {
					if (this.allTabs [i].showTabButton) {
						num++;
					}
				}
				return num;
			}
		}

		//
		// Constructors
		//
		public MainTabsRoot ()
		{
			this.allTabs = (from x in DefDatabase<MainTabDef>.get_AllDefs ()
			orderby x.order
			select x).ToList<MainTabDef> ();
		}

		//
		// Methods
		//
		private void DoTabButton (MainTabDef def, float posX, float width)
		{
			Text.set_Font (1);
			Rect rect = new Rect (posX, (float)(Screen.get_height () - 35), width, 35);
			SoundDef mouseoverButtonCategory = SoundDefOf.MouseoverButtonCategory;
			if (WidgetsSubtle.ButtonSubtle (rect, def.get_LabelCap (), def.get_Window ().get_TabButtonBarPercent (), -1, mouseoverButtonCategory)) {
				this.ToggleTab (def, true);
			}
			if (!GenText.NullOrEmpty (def.tutorHighlightTag)) {
				TutorUIHighlighter.HighlightOpportunity (def.tutorHighlightTag, rect);
			}
			if (!GenText.NullOrEmpty (def.description)) {
				TooltipHandler.TipRegion (rect, def.description);
			}
		}

		public void EscapeCurrentTab (bool playSound = true)
		{
			this.SetCurrentTab (null, playSound);
		}

		public MainTabDef FindTabDef (string defName)
		{
			return this.AllTabs.FirstOrDefault ((MainTabDef d) => d.defName == defName);
		}

		public T FindWindow<T> () where T : MainTabWindow
		{
			Type targetType = typeof(T);
			MainTabDef mainTabDef = this.AllTabs.FirstOrDefault ((MainTabDef d) => d.get_Window () != null && targetType.IsAssignableFrom (d.get_Window ().GetType ()));
			if (mainTabDef != null) {
				return (T)((object)mainTabDef.get_Window ());
			}
			return (T)((object)null);
		}

		public MainTabWindow FindWindow (string defName)
		{
			MainTabDef mainTabDef = this.AllTabs.FirstOrDefault ((MainTabDef d) => d.defName == defName);
			if (mainTabDef != null) {
				return mainTabDef.get_Window ();
			}
			return null;
		}

		public IEnumerable<T> FindWindows<T> () where T : MainTabWindow
		{
			Type targetType = typeof(T);
			return (from d in this.AllTabs.FindAll ((MainTabDef d) => d.get_Window () != null && targetType.IsAssignableFrom (d.get_Window ().GetType ()))
			select d.get_Window ()).Cast<T> ();
		}

		private float GetTabButtonPosition (MainTabDef tab)
		{
			int tabButtonsCount = this.TabButtonsCount;
			int num = (int)((float)Screen.get_width () / (float)tabButtonsCount);
			int num2 = 0;
			for (int i = 0; i < this.allTabs.Count; i++) {
				if (this.allTabs [i].showTabButton) {
					if (this.allTabs [i] == tab) {
						return (float)num2;
					}
					num2 += num;
				}
			}
			return 0;
		}

		private float GetTabButtonWidth (MainTabDef tab)
		{
			int tabButtonsCount = this.TabButtonsCount;
			int num = 0;
			for (int i = 0; i < this.allTabs.Count; i++) {
				if (this.allTabs [i].showTabButton) {
					num = i;
				}
			}
			int num2 = (int)((float)Screen.get_width () / (float)tabButtonsCount);
			int num3 = 0;
			for (int j = 0; j < this.allTabs.Count; j++) {
				if (this.allTabs [j].showTabButton) {
					if (this.allTabs [j] == tab) {
						if (j == num) {
							return (float)(Screen.get_width () - num3);
						}
						return (float)num2;
					}
					else {
						num3 += num2;
					}
				}
			}
			return 0;
		}

		public void HandleLowPriorityShortcuts ()
		{
			if (Find.get_Selector ().get_NumSelected () == 0 && Event.get_current ().get_type () == null && Event.get_current ().get_button () == 1) {
				Event.get_current ().Use ();
				this.ToggleTab (MainTabDefOf.Architect, true);
			}
			if (this.OpenTab != MainTabDefOf.Inspect && Event.get_current ().get_type () == null && Event.get_current ().get_button () != 2) {
				this.EscapeCurrentTab (true);
				Find.get_Selector ().ClearSelection ();
			}
		}

		public void MainTabsOnGUI ()
		{
			GUI.set_color (Color.get_white ());
			for (int i = 0; i < this.allTabs.Count; i++) {
				if (this.allTabs [i].showTabButton) {
					this.DoTabButton (this.allTabs [i], this.GetTabButtonPosition (this.allTabs [i]), this.GetTabButtonWidth (this.allTabs [i]));
				}
			}
			for (int j = 0; j < this.allTabs.Count; j++) {
				if (this.allTabs [j].toggleHotKey != null && this.allTabs [j].toggleHotKey.get_KeyDownEvent ()) {
					this.ToggleTab (this.allTabs [j], true);
					Event.get_current ().Use ();
					break;
				}
			}
			if (this.OpenTab == MainTabDefOf.Inspect && Find.get_Selector ().get_NumSelected () == 0) {
				this.EscapeCurrentTab (false);
			}
		}

		public void SetCurrentTab (MainTabDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab) {
				return;
			}
			this.ToggleTab (tab, playSound);
		}

		public void ToggleTab (MainTabDef newTab, bool playSound = true)
		{
			MainTabDef openTab = this.OpenTab;
			if (openTab == null && newTab == null) {
				return;
			}
			if (openTab == newTab) {
				Find.get_WindowStack ().TryRemove (openTab.get_Window (), true);
				if (playSound) {
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.TabClose);
				}
			}
			else {
				if (openTab != null) {
					Find.get_WindowStack ().TryRemove (openTab.get_Window (), true);
				}
				if (newTab != null) {
					Find.get_WindowStack ().Add (newTab.get_Window ());
				}
				if (playSound) {
					if (newTab == null) {
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.TabClose);
					}
					else {
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.TabOpen);
					}
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Architect : MainTabWindow_Architect
	{
		//
		// Static Fields
		//
		private const float ButHeight = 32;

		//
		// Fields
		//
		private List<ArchitectCategoryTab> desPanelsCached;

		public ArchitectCategoryTab selectedDesPanel;

		//
		// Constructors
		//
		public MainTabWindow_Architect ()
		{
			this.CacheDesPanels ();
		}

		//
		// Methods
		//
		protected void BaseDoWindowContents (Rect inRect)
		{
			if (this.get_Anchor () == null) {
				this.currentWindowRect.set_x (0);
			}
			else {
				this.currentWindowRect.set_x ((float)Screen.get_width () - this.currentWindowRect.get_width ());
			}
			this.currentWindowRect.set_y ((float)(Screen.get_height () - 35) - this.currentWindowRect.get_height ());
			if (this.def.concept != null) {
				ConceptDatabase.KnowledgeDemonstrated (this.def.concept, 1);
			}
		}

		private void CacheDesPanels ()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab> ();
			foreach (DesignationCategoryDef current in from dc in DefDatabase<DesignationCategoryDef>.get_AllDefs ()
			orderby dc.order descending
			select dc) {
				this.desPanelsCached.Add (new ArchitectCategoryTab (current));
			}
		}

		protected void ClickedCategory (ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan) {
				this.selectedDesPanel = null;
			}
			else {
				this.selectedDesPanel = Pan;
			}
			SoundStarter.PlayOneShotOnCamera (SoundDefOf.ArchitectCategorySelect);
		}

		public override void DoWindowContents (Rect inRect)
		{
			this.BaseDoWindowContents (inRect);
			Text.set_Font (1);
			float num = inRect.get_width () / 2;
			float num2 = 0;
			float num3 = 0;
			for (int i = 0; i < this.desPanelsCached.Count; i++) {
				Rect rect = new Rect (num2 * num, num3 * 32, num, 32);
				rect.set_height (rect.get_height () + 1);
				if (num2 == 0) {
					rect.set_width (rect.get_width () + 1);
				}
				if (WidgetsSubtle.ButtonSubtle (rect, this.desPanelsCached [i].def.get_LabelCap (), 0, 8, SoundDefOf.MouseoverButtonCategory)) {
					this.ClickedCategory (this.desPanelsCached [i]);
				}
				num2 += 1;
				if (num2 > 1) {
					num2 = 0;
					num3 += 1;
				}
			}
		}

		public override void ExtraOnGUI ()
		{
			base.ExtraOnGUI ();
			if (this.selectedDesPanel != null) {
				this.selectedDesPanel.DesignationTabOnGUI ();
			}
		}

		public Designator ReplaceDesignator (Type searchingFor, Designator replacement)
		{
			DesignationCategoryDef designationCategoryDef = null;
			Designator designator = null;
			foreach (DesignationCategoryDef current in from dc in DefDatabase<DesignationCategoryDef>.get_AllDefs ()
			orderby dc.order descending
			select dc) {
				foreach (Designator current2 in current.resolvedDesignators) {
					if (current2.GetType ().Equals (searchingFor)) {
						designator = current2;
						designationCategoryDef = current;
						break;
					}
				}
				if (designator != null) {
					break;
				}
			}
			if (designator == null) {
				return null;
			}
			int index = designationCategoryDef.resolvedDesignators.IndexOf (designator);
			designationCategoryDef.resolvedDesignators [index] = replacement;
			return designator;
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Inspect : MainTabWindow_Inspect
	{
		//
		// Static Fields
		//
		private const float PaneWidth = 432;

		private const float PaneInnerMargin = 12;

		private static readonly Texture2D TabButtonFillTex = SolidColorMaterials.NewSolidColorTexture (0.07450981, 0.08627451, 0.1058824, 1);

		private static IntVec3 lastSelectCell;

		//
		// Fields
		//
		private ReplacementTabs replacementTabs = new ReplacementTabs ();

		private MethodInfo fillTabMethod;

		private MethodInfo updateSizeMethod;

		private FieldInfo sizeField;

		private FieldInfo recentHeightField;

		private FieldInfo openTabTypeField;

		//
		// Properties
		//
		public IEnumerable<ITab> CurTabs {
			get {
				if (this.NumSelected == 1) {
					if (this.replacementTabs == null) {
						if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null) {
							return this.SelThing.def.inspectorTabsResolved;
						}
						if (this.SelZone != null) {
							return this.SelZone.GetInspectionTabs ();
						}
					}
					else {
						if (this.SelThing != null) {
							return this.replacementTabs.GetTabs (this.SelThing);
						}
						if (this.SelZone != null) {
							return this.replacementTabs.GetTabs (this.SelZone);
						}
					}
				}
				return Enumerable.Empty<ITab> ();
			}
		}

		private int NumSelected {
			get {
				return Find.get_Selector ().get_NumSelected ();
			}
		}

		protected Type OpenTabType {
			get {
				return (Type)this.openTabTypeField.GetValue (this);
			}
			set {
				this.openTabTypeField.SetValue (this, value);
			}
		}

		protected float RecentHeight {
			get {
				return (float)this.recentHeightField.GetValue (this);
			}
			set {
				this.recentHeightField.SetValue (this, value);
			}
		}

		public ReplacementTabs ReplacementTabs {
			get {
				return this.replacementTabs;
			}
			set {
				this.replacementTabs = value;
			}
		}

		private IEnumerable<object> Selected {
			get {
				return Find.get_Selector ().get_SelectedObjects ();
			}
		}

		private Thing SelThing {
			get {
				return Find.get_Selector ().get_SingleSelectedThing ();
			}
		}

		private Zone SelZone {
			get {
				return Find.get_Selector ().get_SelectedZone ();
			}
		}

		protected override float WindowPadding {
			get {
				return 0;
			}
		}

		//
		// Constructors
		//
		public MainTabWindow_Inspect ()
		{
			this.closeOnEscapeKey = false;
			this.recentHeightField = typeof(MainTabWindow_Inspect).GetField ("recentHeight", BindingFlags.Instance | BindingFlags.NonPublic);
			this.openTabTypeField = typeof(MainTabWindow_Inspect).GetField ("openTabType", BindingFlags.Instance | BindingFlags.NonPublic);
			this.sizeField = typeof(ITab).GetField ("size", BindingFlags.Instance | BindingFlags.NonPublic);
			this.updateSizeMethod = typeof(ITab).GetMethod ("UpdateSize", BindingFlags.Instance | BindingFlags.NonPublic);
			this.fillTabMethod = typeof(ITab).GetMethod ("FillTab", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Methods
		//
		public void DoTabGui (ITab tab)
		{
			MainTabWindow_Inspect inspectWorker = (MainTabWindow_Inspect)MainTabDefOf.Inspect.get_Window ();
			this.updateSizeMethod.Invoke (tab, null);
			Vector2 vector = (Vector2)this.sizeField.GetValue (tab);
			float y = vector.y;
			float num = inspectWorker.get_PaneTopY () - 30 - y;
			Rect outRect = new Rect (0, num, vector.x, y);
			Find.get_WindowStack ().ImmediateWindow (235086, outRect, 0, delegate {
				if (Find.get_MainTabsRoot ().get_OpenTab () != MainTabDefOf.Inspect || !this.CurTabs.Contains (tab) || !tab.get_IsVisible ()) {
					return;
				}
				if (Widgets.CloseButtonFor (GenUI.AtZero (outRect))) {
					inspectWorker.CloseOpenTab ();
				}
				try {
					this.fillTabMethod.Invoke (tab, null);
				}
				catch (Exception ex) {
					Log.ErrorOnce (string.Concat (new object[] {
						"Exception filling tab ",
						this.GetType (),
						": ",
						ex.ToString ()
					}), 49827);
				}
			}, true, false, 1);
		}

		private void DoTabs (IEnumerable<ITab> tabs)
		{
			try {
				Type openTabType = this.OpenTabType;
				float num = base.get_PaneTopY () - 30;
				float num2 = 360;
				float num3 = 0;
				bool flag = false;
				foreach (ITab current in tabs) {
					if (current.get_IsVisible ()) {
						Rect rect = new Rect (num2, num, 72, 30);
						num3 = num2;
						Text.set_Font (1);
						if (Widgets.TextButton (rect, Translator.Translate (current.labelKey), true, false)) {
							this.ToggleTab (current);
						}
						if (!GenText.NullOrEmpty (current.tutorHighlightTag)) {
							TutorUIHighlighter.HighlightOpportunity (current.tutorHighlightTag, rect);
						}
						if (current.GetType () == openTabType) {
							this.DoTabGui (current);
							this.RecentHeight = 700;
							flag = true;
						}
						num2 -= 72;
					}
				}
				if (flag) {
					GUI.DrawTexture (new Rect (0, num, num3, 30), MainTabWindow_Inspect.TabButtonFillTex);
				}
			}
			catch (Exception ex) {
				Log.ErrorOnce (ex.ToString (), 742783);
			}
		}

		public override void DoWindowContents (Rect inRect)
		{
			if (this.get_Anchor () == null) {
				this.currentWindowRect.set_x (0);
			}
			else {
				this.currentWindowRect.set_x ((float)Screen.get_width () - this.currentWindowRect.get_width ());
			}
			this.currentWindowRect.set_y ((float)(Screen.get_height () - 35) - this.currentWindowRect.get_height ());
			if (this.def.concept != null) {
				ConceptDatabase.KnowledgeDemonstrated (this.def.concept, 1);
			}
			Vector2 paneSize = MainTabWindow_Inspect.PaneSize;
			this.RecentHeight = paneSize.y;
			if (this.NumSelected > 0) {
				try {
					Rect rect = GenUI.ContractedBy (inRect, 12);
					rect.set_yMin (rect.get_yMin () - 4);
					GUI.BeginGroup (rect);
					bool flag = true;
					if (this.NumSelected > 1) {
						flag = !(from t in this.Selected
						where !InspectPaneUtility.CanInspectTogether (this.Selected.First<object> (), t)
						select t).Any<object> ();
					}
					else {
						Rect rect2 = new Rect (rect.get_width () - 30, 0, 30, 30);
						if (Find.get_Selector ().get_SelectedZone () == null || Find.get_Selector ().get_SelectedZone ().ContainsCell (MainTabWindow_Inspect.lastSelectCell)) {
							if (Widgets.ImageButton (rect2, TexButton.SelectOverlappingNext)) {
								this.SelectNextInCell ();
							}
							TooltipHandler.TipRegion (rect2, Translator.Translate ("SelectNextInSquareTip", new object[] {
								KeyBindingDefOf.SelectNextInCell.get_MainKeyLabel ()
							}));
						}
						if (Find.get_Selector ().get_SingleSelectedThing () != null) {
							Widgets.InfoCardButton (rect.get_width () - 60, 0, Find.get_Selector ().get_SingleSelectedThing ());
						}
					}
					Rect rect3 = new Rect (0, 0, rect.get_width () - 60, 50);
					string text = InspectPaneUtility.AdjustedLabelFor (this.Selected, rect3);
					rect3.set_width (rect3.get_width () + 300);
					Text.set_Font (2);
					Text.set_Anchor (0);
					Widgets.Label (rect3, text);
					if (flag && this.NumSelected == 1) {
						Rect rect4 = GenUI.AtZero (rect);
						rect4.set_yMin (rect4.get_yMin () + 26);
						InspectPaneFiller.DoPaneContentsFor ((ISelectable)Find.get_Selector ().get_FirstSelectedObject (), rect4);
					}
				}
				catch (Exception ex) {
					Log.Error ("Exception doing inspect pane: " + ex.ToString ());
				}
				finally {
					GUI.EndGroup ();
				}
			}
		}

		public override void ExtraOnGUI ()
		{
			if (this.NumSelected > 0) {
				if (KeyBindingDefOf.SelectNextInCell.get_KeyDownEvent ()) {
					this.SelectNextInCell ();
				}
				if (DesignatorManager.get_SelectedDesignator () != null) {
					DesignatorManager.get_SelectedDesignator ().DoExtraGuiControls (0, base.get_PaneTopY ());
				}
				InspectGizmoGrid.DrawInspectGizmoGridFor (this.Selected);
				this.DoTabs (this.CurTabs);
			}
		}

		private void SelectNextInCell ()
		{
			if (this.NumSelected == 0) {
				return;
			}
			if (Find.get_Selector ().get_SelectedZone () == null || Find.get_Selector ().get_SelectedZone ().ContainsCell (MainTabWindow_Inspect.lastSelectCell)) {
				if (Find.get_Selector ().get_SelectedZone () == null) {
					MainTabWindow_Inspect.lastSelectCell = Find.get_Selector ().get_SingleSelectedThing ().get_Position ();
				}
				Find.get_Selector ().SelectNextAt (MainTabWindow_Inspect.lastSelectCell);
			}
		}

		private void ToggleTab (ITab tab)
		{
			Type openTabType = this.OpenTabType;
			if ((tab == null && openTabType == null) || tab.GetType () == openTabType) {
				this.OpenTabType = null;
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.TabClose);
			}
			else {
				tab.OnOpen ();
				this.OpenTabType = tab.GetType ();
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.TabOpen);
			}
		}

		public override void WindowUpdate ()
		{
			Type openTabType = this.OpenTabType;
			foreach (ITab current in this.CurTabs) {
				if (current.get_IsVisible () && current.GetType () == openTabType) {
					current.TabUpdate ();
				}
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Inventory : MainTabWindow
	{
		//
		// Fields
		//
		private InventoryTab curTab;

		private List<InventoryTab> tabs = new List<InventoryTab> ();

		protected InventoryManager inventoryManager;

		//
		// Properties
		//
		public InventoryManager InventoryManager {
			get {
				return this.inventoryManager;
			}
			set {
				this.inventoryManager = value;
				foreach (InventoryTab current in this.tabs) {
					current.InventoryManager = value;
				}
			}
		}

		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (1010, 684);
			}
		}

		//
		// Constructors
		//
		public MainTabWindow_Inventory ()
		{
			this.doCloseX = true;
			this.tabs.Add (new InventoryTab_Items ());
			this.tabs.Add (new InventoryTab_Buildings ());
			this.tabs = (from t in this.tabs
			orderby t.order descending
			select t).ToList<InventoryTab> ();
			this.curTab = this.tabs [0];
		}

		//
		// Methods
		//
		public override void DoWindowContents (Rect inRect)
		{
			base.DoWindowContents (inRect);
			Text.set_Font (1);
			Rect rect = new Rect (0, -10, inRect.get_width (), 40);
			Text.set_Font (2);
			Text.set_Anchor (4);
			Widgets.Label (rect, Translator.Translate ("EdB.Inventory.Window.Header", new object[] {
				Find.get_ColonyInfo ().get_ColonyName ()
			}));
			float num = 70;
			Rect rect2 = new Rect (0, num, inRect.get_width (), inRect.get_height () - num);
			Widgets.DrawMenuSection (rect2, true);
			this.curTab.InventoryTabOnGui (rect2);
			List<TabRecord> list = (from panel in this.tabs
			select new TabRecord (panel.title, delegate {
				this.curTab = panel;
			}, panel == this.curTab)).ToList<TabRecord> ();
			TabDrawer.DrawTabs (rect2, list);
		}

		public override void ExtraOnGUI ()
		{
			base.ExtraOnGUI ();
		}

		public override void PreClose ()
		{
			base.PreClose ();
			Preferences.Instance.Save ();
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			if (this.inventoryManager != null) {
				this.TakeInventory ();
			}
		}

		public void TakeInventory ()
		{
			if (this.inventoryManager != null) {
				try {
					this.inventoryManager.TakeInventory ();
				}
				catch (Exception ex) {
					Log.Error ("EdB Interface inventory dialog failed to count all inventory");
					throw ex;
				}
			}
			else {
				Log.Warning ("InventoryManager was null.  Could not take inventory");
			}
		}
	}
}
using RimWorld;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Menu : MainTabWindow
	{
		//
		// Fields
		//
		private bool anyWorldFiles;

		private bool anyMapFiles;

		//
		// Properties
		//
		public override MainTabWindowAnchor Anchor {
			get {
				return 1;
			}
		}

		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (450, 372);
			}
		}

		//
		// Constructors
		//
		public MainTabWindow_Menu ()
		{
			this.forcePause = true;
		}

		//
		// Methods
		//
		public override void DoWindowContents (Rect rect)
		{
			base.DoWindowContents (rect);
			MainMenuDrawer.DoMainMenuButtons (rect, this.anyWorldFiles, this.anyMapFiles, null);
		}

		public override void ExtraOnGUI ()
		{
			base.ExtraOnGUI ();
			VersionControl.DrawInfoInCorner ();
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			ConceptDatabase.Save ();
			ShipCountdown.CancelCountdown ();
			this.anyWorldFiles = SavedWorldsDatabase.get_AllWorldFiles ().Any<FileInfo> ();
			this.anyMapFiles = MapFilesUtility.get_AllMapFiles ().Any<FileInfo> ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Outfits : MainTabWindow_PawnListWithSquads
	{
		//
		// Static Fields
		//
		private const float TopAreaHeight = 45;

		//
		// Fields
		//
		private Outfit outfit;

		private bool anyForcedApparel;

		//
		// Properties
		//
		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (1010, this.WindowHeight);
			}
		}

		protected override float WindowHeight {
			get {
				return 45 + (float)base.get_PawnsCount () * 30 + 65 + base.ExtraHeight;
			}
		}

		//
		// Methods
		//
		public override void DoWindowContents (Rect fillRect)
		{
			base.DoWindowContents (fillRect);
			Rect rect = new Rect (0, 0, fillRect.get_width (), 45);
			GUI.BeginGroup (rect);
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
			Rect rect2 = new Rect (5, 5, 160, 35);
			if (Widgets.TextButton (rect2, Translator.Translate ("ManageOutfits"), true, false)) {
				Find.get_WindowStack ().Add (new Dialog_ManageOutfits (null));
			}
			Text.set_Anchor (7);
			Rect rect3 = new Rect (175, 0, rect.get_width () - 175, rect.get_height ());
			Rect rect4 = new Rect (rect3.get_x (), rect3.get_y (), rect3.get_width () / 2, rect3.get_height ());
			Widgets.Label (rect4, Translator.Translate ("CurrentOutfit"));
			Text.set_Anchor (0);
			GUI.EndGroup ();
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			float squadRowHeight = base.SquadRowHeight;
			Rect rect5 = new Rect (0, 45 + squadRowHeight, fillRect.get_width (), fillRect.get_height () - 45 - base.PawnListScrollHeightReduction);
			this.anyForcedApparel = false;
			base.DrawRows (rect5);
			if (base.SquadRowEnabled) {
				this.DrawSquadRow (new Rect (0, 45, fillRect.get_width () - 16, squadRowHeight));
			}
			if (base.SquadFilteringEnabled) {
				Text.set_Font (1);
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
				this.DrawSquadSelectionDropdown (new Rect (fillRect.get_x (), fillRect.get_y () + fillRect.get_height () - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
		}

		protected override void DrawPawnRow (Rect rect, Pawn p)
		{
			Rect rect2 = new Rect (rect.get_x () + 175, rect.get_y (), rect.get_width () - 175, rect.get_height ());
			Rect rect3 = new Rect (rect2.get_x (), rect2.get_y () + 2, rect2.get_width () * 0.333, rect2.get_height () - 4);
			if (Widgets.TextButton (rect3, p.outfits.get_CurrentOutfit ().label, true, false)) {
				List<FloatMenuOption> list = new List<FloatMenuOption> ();
				foreach (Outfit current in Find.get_Map ().outfitDatabase.get_AllOutfits ()) {
					Outfit localOut = current;
					list.Add (new FloatMenuOption (localOut.label, delegate {
						p.outfits.set_CurrentOutfit (localOut);
					}, 1, null, null));
				}
				Find.get_WindowStack ().Add (new FloatMenu (list, false));
			}
			Rect rect4 = new Rect (rect3.get_xMax () + 4, rect.get_y () + 2, 100, rect.get_height () - 4);
			if (Widgets.TextButton (rect4, Translator.Translate ("OutfitEdit"), true, false)) {
				Find.get_WindowStack ().Add (new Dialog_ManageOutfits (p.outfits.get_CurrentOutfit ()));
			}
			Rect rect5 = new Rect (rect4.get_xMax () + 4, rect.get_y () + 2, 100, rect.get_height () - 4);
			if (p.outfits.forcedHandler.get_SomethingIsForced ()) {
				this.anyForcedApparel = true;
				if (Widgets.TextButton (rect5, Translator.Translate ("ClearForcedApparel"), true, false)) {
					p.outfits.forcedHandler.Reset ();
				}
				TooltipHandler.TipRegion (rect5, new TipSignal (delegate {
					string text = Translator.Translate ("ForcedApparel") + ":
";
					foreach (Apparel current2 in p.outfits.forcedHandler.get_ForcedApparel ()) {
						text = text + "
   " + current2.get_LabelCap ();
					}
					return text;
				}, p.GetHashCode () * 612));
			}
		}

		protected void DrawSquadRow (Rect rect)
		{
			float num = 3;
			GUI.DrawTexture (rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			float num2 = rect.get_height () - num - 4;
			Rect rect2 = new Rect (rect.get_x () + 175, rect.get_y (), rect.get_width () - 175, rect.get_height () - num);
			Rect rect3 = new Rect (rect2.get_x (), rect2.get_y () + 2, rect2.get_width () * 0.333, num2);
			if (Widgets.TextButton (rect3, this.outfit.label, true, false)) {
				List<FloatMenuOption> list = new List<FloatMenuOption> ();
				foreach (Outfit current in Find.get_Map ().outfitDatabase.get_AllOutfits ()) {
					Outfit localOut = current;
					list.Add (new FloatMenuOption (localOut.label, delegate {
						this.outfit = localOut;
						foreach (Pawn current3 in this.pawns) {
							current3.outfits.set_CurrentOutfit (localOut);
						}
					}, 1, null, null));
				}
				Find.get_WindowStack ().Add (new FloatMenu (list, false));
			}
			Rect rect4 = new Rect (rect3.get_xMax () + 4, rect.get_y () + 2, 100, num2);
			if (Widgets.TextButton (rect4, Translator.Translate ("OutfitEdit"), true, false)) {
				Find.get_WindowStack ().Add (new Dialog_ManageOutfits (this.outfit));
			}
			Rect rect5 = new Rect (rect4.get_xMax () + 4, rect.get_y () + 2, 100, num2);
			if (this.anyForcedApparel && Widgets.TextButton (rect5, Translator.Translate ("ClearForcedApparel"), true, false)) {
				foreach (Pawn current2 in this.pawns) {
					current2.outfits.forcedHandler.Reset ();
				}
			}
			GUI.BeginGroup (rect);
			GUI.set_color (new Color (1, 1, 1, 0.2));
			Widgets.DrawLineHorizontal (0, 0, rect.get_width ());
			GUI.set_color (new Color (1, 1, 1, 0.35));
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 3, rect.get_width ());
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 2, rect.get_width ());
			Text.set_Font (1);
			Text.set_Anchor (3);
			Text.set_WordWrap (false);
			Rect rect6 = new Rect (0, 0, 175, 30);
			rect6.set_xMin (rect6.get_xMin () + 15);
			GUI.set_color (new Color (1, 1, 1, 1));
			if (base.SquadFilteringEnabled && SquadManager.Instance.SquadFilter != null) {
				Widgets.Label (rect6, SquadManager.Instance.SquadFilter.Name);
			}
			else {
				Widgets.Label (rect6, Translator.Translate ("EdB.Squads.AllColonistsSquadName"));
			}
			Text.set_Anchor (0);
			Text.set_WordWrap (true);
			GUI.set_color (Color.get_white ());
			GUI.EndGroup ();
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			this.outfit = Find.get_Map ().outfitDatabase.get_AllOutfits ().FirstOrDefault<Outfit> ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class MainTabWindow_PawnListWithSquads : MainTabWindow_PawnList
	{
		//
		// Static Fields
		//
		public static readonly Texture2D SquadRowBackground = SolidColorMaterials.NewSolidColorTexture (1, 1, 1, 0.05);

		public static readonly float ScrollHeightReduction = 26;

		public static readonly float squadRowHeight = 33;

		public static readonly float FooterExtraHeight = 22;

		public static readonly float FooterButtonHeight = 32;

		public static readonly float SquadFilterButtonWidth = 228;

		//
		// Fields
		//
		protected bool squadDirtyFlag;

		protected PreferenceEnableSquadRow squadRowEnabled;

		protected PreferenceEnableSquadFiltering squadFilteringEnabled;

		protected PreferenceEnableSquads squadsEnabled;

		//
		// Properties
		//
		protected float ExtraHeight {
			get {
				float num = 0;
				if (this.SquadFilteringEnabled) {
					num += MainTabWindow_PawnListWithSquads.FooterExtraHeight;
				}
				if (this.SquadRowEnabled) {
					num += this.SquadRowHeight;
				}
				return num;
			}
		}

		protected float PawnListScrollHeightReduction {
			get {
				if (this.SquadFilteringEnabled || this.SquadRowEnabled) {
					return MainTabWindow_PawnListWithSquads.ScrollHeightReduction + this.ExtraHeight;
				}
				return 0;
			}
		}

		protected IEnumerable<Pawn> Pawns {
			get {
				if (this.SquadFilteringEnabled) {
					return SquadManager.Instance.SquadFilter.Pawns.FindAll ((Pawn p) => !p.get_Dead () && !p.get_Destroyed () && p.get_HostFaction () == null && p.get_RaceProps ().get_Humanlike ());
				}
				if (this.SquadsEnabled) {
					return SquadManager.Instance.AllColonistsSquad.Pawns.FindAll ((Pawn p) => !p.get_Dead () && !p.get_Destroyed () && p.get_HostFaction () == null && p.get_RaceProps ().get_Humanlike ());
				}
				return Find.get_ListerPawns ().get_FreeColonists ();
			}
		}

		public PreferenceEnableSquadFiltering PreferenceEnableSquadFiltering {
			set {
				this.squadFilteringEnabled = value;
			}
		}

		public PreferenceEnableSquadRow PreferenceEnableSquadRow {
			set {
				this.squadRowEnabled = value;
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads {
			set {
				this.squadsEnabled = value;
			}
		}

		protected bool SquadFilteringEnabled {
			get {
				return this.squadFilteringEnabled != null && this.squadFilteringEnabled.Value;
			}
		}

		protected bool SquadRowEnabled {
			get {
				return this.squadRowEnabled != null && this.squadRowEnabled.Value;
			}
		}

		protected float SquadRowHeight {
			get {
				if (this.SquadRowEnabled) {
					return MainTabWindow_PawnListWithSquads.squadRowHeight;
				}
				return 0;
			}
		}

		protected bool SquadsEnabled {
			get {
				return this.squadsEnabled != null && this.squadsEnabled.Value;
			}
		}

		protected abstract float WindowHeight {
			get;
		}

		//
		// Constructors
		//
		public MainTabWindow_PawnListWithSquads ()
		{
		}

		//
		// Methods
		//
		protected override void BuildPawnList ()
		{
			this.squadDirtyFlag = true;
		}

		protected virtual void DeferredBuildPawnList ()
		{
			this.pawns.Clear ();
			this.pawns.AddRange (this.Pawns);
		}

		protected virtual void DrawSquadSelectionDropdown (Rect rect)
		{
			Text.set_Font (1);
			Squad squadFilter = SquadManager.Instance.SquadFilter;
			List<Squad> list = SquadManager.Instance.Squads.FindAll ((Squad s) => s.ShowInOverviewTabs && s.Pawns.Count > 0);
			if (list.Count > 0 && Button.TextButton (rect, squadFilter.Name, true, false, true)) {
				List<FloatMenuOption> list2 = new List<FloatMenuOption> ();
				list2.AddRange (list.ConvertAll<FloatMenuOption> ((Squad s) => new FloatMenuOption (s.Name, delegate {
					if (SquadManager.Instance.SquadFilter != s) {
						SquadManager.Instance.SquadFilter = s;
						this.squadDirtyFlag = true;
					}
				}, 1, null, null)));
				Find.get_WindowStack ().Add (new FloatMenu (list2, false));
			}
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			this.squadDirtyFlag = false;
			this.DeferredBuildPawnList ();
		}

		public override void WindowUpdate ()
		{
			if (this.squadDirtyFlag) {
				this.DeferredBuildPawnList ();
				this.currentWindowRect.set_height (this.WindowHeight);
				this.scrollPosition = Vector2.get_zero ();
				this.squadDirtyFlag = false;
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Restrictions : MainTabWindow_PawnListWithSquads
	{
		//
		// Static Fields
		//
		private const float TopAreaHeight = 65;

		private const float AAGapWidth = 6;

		private const float TimeTablesWidth = 500;

		private const float CopyPasteIconSize = 24;

		private const float CopyPasteColumnWidth = 52;

		//
		// Fields
		//
		private Area areaRestriction;

		private Pawn_TimetableTracker timetable = new Pawn_TimetableTracker ();

		private float hourWidth;

		private List<TimeAssignmentDef> clipboard;

		private TimeAssignmentDef selectedAssignment = TimeAssignmentDefOf.Work;

		//
		// Properties
		//
		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (1010, this.WindowHeight);
			}
		}

		protected override float WindowHeight {
			get {
				return 65 + (float)base.get_PawnsCount () * 30 + 65 + base.ExtraHeight;
			}
		}

		//
		// Methods
		//
		private void CopyFrom (Pawn p)
		{
			this.clipboard = p.timetable.times.ToList<TimeAssignmentDef> ();
		}

		private void DoTimeAssignment (Rect rect, int hour)
		{
			rect = GenUI.ContractedBy (rect, 1);
			TimeAssignmentDef assignment = this.timetable.GetAssignment (hour);
			GUI.DrawTexture (rect, assignment.get_ColorTexture ());
			if (Mouse.IsOver (rect)) {
				Widgets.DrawBox (rect, 2);
				bool flag = false;
				if (Input.GetMouseButton (0)) {
					if (assignment != this.selectedAssignment) {
						this.timetable.SetAssignment (hour, this.selectedAssignment);
					}
					foreach (Pawn current in this.pawns) {
						if (current.timetable.GetAssignment (hour) != assignment) {
							current.timetable.SetAssignment (hour, this.selectedAssignment);
							flag = true;
						}
					}
				}
				if (flag) {
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.DesignateDragStandardChanged);
				}
			}
		}

		private void DoTimeAssignment (Rect rect, Pawn p, int hour)
		{
			rect = GenUI.ContractedBy (rect, 1);
			TimeAssignmentDef assignment = p.timetable.GetAssignment (hour);
			GUI.DrawTexture (rect, assignment.get_ColorTexture ());
			if (Mouse.IsOver (rect)) {
				Widgets.DrawBox (rect, 2);
				if (assignment != this.selectedAssignment && Input.GetMouseButton (0)) {
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.DesignateDragStandardChanged);
					p.timetable.SetAssignment (hour, this.selectedAssignment);
				}
			}
		}

		public override void DoWindowContents (Rect fillRect)
		{
			base.DoWindowContents (fillRect);
			Rect rect = new Rect (0, 0, fillRect.get_width (), 65);
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (0, 0, 217, rect.get_height ());
			this.DrawTimeAssignmentSelectorGrid (rect2);
			float num = 227;
			Text.set_Font (0);
			Text.set_Anchor (6);
			for (int i = 0; i < 24; i++) {
				Rect rect3 = new Rect (num + 4, 0, this.hourWidth, rect.get_height () + 3);
				Widgets.Label (rect3, i.ToString ());
				num += this.hourWidth;
			}
			num += 6;
			Rect rect4 = new Rect (num, 0, rect.get_width () - num - 16, Mathf.Round (rect.get_height () / 2));
			Text.set_Font (1);
			if (Widgets.TextButton (rect4, Translator.Translate ("ManageAreas"), true, false)) {
				Find.get_WindowStack ().Add (new Dialog_ManageAreas ());
			}
			Text.set_Font (0);
			Text.set_Anchor (7);
			Rect rect5 = new Rect (num, 0, rect.get_width () - num, rect.get_height () + 3);
			Widgets.Label (rect5, Translator.Translate ("AllowedArea"));
			GUI.EndGroup ();
			if (base.SquadFilteringEnabled) {
				Text.set_Font (1);
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
				this.DrawSquadSelectionDropdown (new Rect (fillRect.get_x (), fillRect.get_y () + fillRect.get_height () - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
			Text.set_Font (1);
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			float squadRowHeight = base.SquadRowHeight;
			Rect rect6 = new Rect (0, rect.get_height () + squadRowHeight, fillRect.get_width (), fillRect.get_height () - rect.get_height () - base.PawnListScrollHeightReduction);
			base.DrawRows (rect6);
			if (base.SquadRowEnabled) {
				this.DrawSquadRow (new Rect (0, rect.get_height (), fillRect.get_width () - 16, squadRowHeight));
			}
		}

		protected override void DrawPawnRow (Rect rect, Pawn p)
		{
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (175, 4, 24, 24);
			if (Widgets.ImageButton (rect2, TexButton.Copy)) {
				this.CopyFrom (p);
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickHigh);
			}
			TooltipHandler.TipRegion (rect2, Translator.Translate ("Copy"));
			if (this.clipboard != null) {
				Rect rect3 = rect2;
				rect3.set_x (rect2.get_xMax () + 2);
				if (Widgets.ImageButton (rect3, TexButton.Paste)) {
					this.PasteTo (p);
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickLow);
				}
				TooltipHandler.TipRegion (rect3, Translator.Translate ("Paste"));
			}
			float num = 227;
			this.hourWidth = 20.83333;
			for (int i = 0; i < 24; i++) {
				Rect rect4 = new Rect (num, 0, this.hourWidth, rect.get_height ());
				this.DoTimeAssignment (rect4, p, i);
				num += this.hourWidth;
			}
			GUI.set_color (Color.get_white ());
			num += 6;
			Rect rect5 = new Rect (num, 0, rect.get_width () - num, rect.get_height ());
			AreaAllowedGUI.DoAllowedAreaSelectors (rect5, p, 1);
			GUI.EndGroup ();
		}

		protected void DrawSquadRow (Rect rect)
		{
			float num = 3;
			GUI.DrawTexture (rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			GUI.BeginGroup (rect);
			Rect rect2 = new Rect (175, 4, 24, 24);
			if (this.clipboard != null) {
				Rect rect3 = rect2;
				rect3.set_x (rect2.get_xMax () + 2);
				if (Widgets.ImageButton (rect3, TexButton.Paste)) {
					foreach (Pawn current in this.pawns) {
						this.PasteTo (current);
					}
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickLow);
				}
				TooltipHandler.TipRegion (rect3, Translator.Translate ("Paste"));
			}
			float num2 = 227;
			this.hourWidth = 20.83333;
			for (int i = 0; i < 24; i++) {
				Rect rect4 = new Rect (num2, 0, this.hourWidth, rect.get_height () - num);
				this.DoTimeAssignment (rect4, i);
				num2 += this.hourWidth;
			}
			GUI.set_color (Color.get_white ());
			num2 += 6;
			Rect rect5 = new Rect (num2, 0, rect.get_width () - num2, rect.get_height () - num);
			AreaAllowedGUI.DoAllowedAreaSelectors (rect5, ref this.areaRestriction, 1, this.pawns);
			GUI.set_color (new Color (1, 1, 1, 0.2));
			Widgets.DrawLineHorizontal (0, 0, rect.get_width ());
			GUI.set_color (new Color (1, 1, 1, 0.35));
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 3, rect.get_width ());
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 2, rect.get_width ());
			Text.set_Font (1);
			Text.set_Anchor (3);
			Text.set_WordWrap (false);
			Rect rect6 = new Rect (0, 0, 175, 30);
			rect6.set_xMin (rect6.get_xMin () + 15);
			GUI.set_color (new Color (1, 1, 1, 1));
			if (base.SquadFilteringEnabled && SquadManager.Instance.SquadFilter != null) {
				Widgets.Label (rect6, SquadManager.Instance.SquadFilter.Name);
			}
			else {
				Widgets.Label (rect6, Translator.Translate ("EdB.Squads.AllColonistsSquadName"));
			}
			Text.set_Anchor (0);
			Text.set_WordWrap (true);
			GUI.set_color (Color.get_white ());
			GUI.EndGroup ();
		}

		private void DrawTimeAssignmentSelectorFor (Rect rect, TimeAssignmentDef ta)
		{
			rect = GenUI.ContractedBy (rect, 2);
			GUI.DrawTexture (rect, ta.get_ColorTexture ());
			if (Widgets.InvisibleButton (rect)) {
				this.selectedAssignment = ta;
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickHigh);
			}
			GUI.set_color (Color.get_white ());
			if (Mouse.IsOver (rect)) {
				Widgets.DrawHighlight (rect);
			}
			Text.set_Font (1);
			Text.set_Anchor (4);
			GUI.set_color (Color.get_white ());
			Widgets.Label (rect, ta.get_LabelCap ());
			Text.set_Anchor (0);
			if (this.selectedAssignment == ta) {
				Widgets.DrawBox (rect, 2);
			}
		}

		private void DrawTimeAssignmentSelectorGrid (Rect rect)
		{
			rect.set_yMax (rect.get_yMax () - 2);
			Rect rect2 = rect;
			rect2.set_xMax (rect2.get_center ().x);
			rect2.set_yMax (rect2.get_center ().y);
			this.DrawTimeAssignmentSelectorFor (rect2, TimeAssignmentDefOf.Anything);
			rect2.set_x (rect2.get_x () + rect2.get_width ());
			this.DrawTimeAssignmentSelectorFor (rect2, TimeAssignmentDefOf.Work);
			rect2.set_y (rect2.get_y () + rect2.get_height ());
			rect2.set_x (rect2.get_x () - rect2.get_width ());
			this.DrawTimeAssignmentSelectorFor (rect2, TimeAssignmentDefOf.Joy);
			rect2.set_x (rect2.get_x () + rect2.get_width ());
			this.DrawTimeAssignmentSelectorFor (rect2, TimeAssignmentDefOf.Sleep);
		}

		private void PasteTo (Pawn p)
		{
			for (int i = 0; i < 24; i++) {
				p.timetable.times [i] = this.clipboard [i];
			}
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			for (int i = 0; i < 24; i++) {
				this.timetable.SetAssignment (i, TimeAssignmentDefOf.Anything);
			}
			this.areaRestriction = null;
			this.clipboard = null;
		}
	}
}
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
		//
		// Static Fields
		//
		public static Texture2D ButtonTexReorderUp;

		public static readonly Rect SquadOrderMoveUpButtonRect = new Rect (MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.get_y () + MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.get_height () + MainTabWindow_Squads.OrderButtonMargin.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadOrderMoveToTopButtonRect = new Rect (MainTabWindow_Squads.SquadListRect.get_x () + MainTabWindow_Squads.SquadListRect.get_width () + MainTabWindow_Squads.OrderButtonMargin.x, MainTabWindow_Squads.SquadListRect.get_y (), MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect DeleteSquadButtonRect = new Rect (MainTabWindow_Squads.NewSquadButtonRect.get_x () + MainTabWindow_Squads.NewSquadButtonRect.get_width () + MainTabWindow_Squads.SquadListMargin.x, MainTabWindow_Squads.NewSquadButtonRect.get_y (), MainTabWindow_Squads.SquadListActionButtonSize.x, MainTabWindow_Squads.SquadListActionButtonSize.y);

		public static readonly Rect NewSquadButtonRect = new Rect (MainTabWindow_Squads.SquadListRect.get_x (), MainTabWindow_Squads.SquadListRect.get_y () + MainTabWindow_Squads.SquadListRect.get_height () + MainTabWindow_Squads.SquadListMargin.y, MainTabWindow_Squads.SquadListActionButtonSize.x, MainTabWindow_Squads.SquadListActionButtonSize.y);

		public static readonly Vector2 SquadListActionButtonSize = new Vector2 ((MainTabWindow_Squads.SquadListRect.get_width () - MainTabWindow_Squads.SquadListMargin.x) / 2, 40);

		public static readonly Vector2 SquadListMargin = new Vector2 (16, 10);

		public static readonly Vector2 SquadContentMargin = new Vector2 (20, 20);

		public static readonly Vector2 MemberActionButtonSize = new Vector2 (90, 36);

		public static readonly Vector2 MemberActionButtonMargin = new Vector2 (24, 24);

		public static readonly Vector2 OrderButtonSize = new Vector2 (32, 32);

		public static readonly Vector2 OrderButtonMargin = new Vector2 (10, 18);

		public static readonly Rect SquadListRect = new Rect (22, 70, 230, 514);

		public static readonly Rect SquadOrderMoveToBottomButtonRect = new Rect (MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.SquadListRect.get_y () + MainTabWindow_Squads.SquadListRect.get_height () - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveToTopButtonRect = new Rect (MainTabWindow_Squads.MemberListRect.get_x () + MainTabWindow_Squads.MemberListRect.get_width () + MainTabWindow_Squads.OrderButtonMargin.x, MainTabWindow_Squads.MemberListRect.get_y (), MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveUpButtonRect = new Rect (MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.get_y () + MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.get_height () + MainTabWindow_Squads.OrderButtonMargin.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveDownButtonRect = new Rect (MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.MemberListRect.get_y () + MainTabWindow_Squads.MemberListRect.get_height () - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect MemberOrderMoveToBottomButtonRect = new Rect (MainTabWindow_Squads.MemberOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.MemberOrderMoveDownButtonRect.get_y () - MainTabWindow_Squads.OrderButtonMargin.y - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadHeaderRect = new Rect (MainTabWindow_Squads.SquadContentMargin.x, MainTabWindow_Squads.SquadContentMargin.y, 240, 30);

		public static readonly Rect MemberCountRect = new Rect (MainTabWindow_Squads.SquadHeaderRect.get_x (), MainTabWindow_Squads.SquadHeaderRect.get_y () + 23, 200, 28);

		public static readonly Rect DeleteMemberButtonRect = new Rect (MainTabWindow_Squads.AddMemberButtonRect.get_x (), MainTabWindow_Squads.AvailableListRect.get_y () + MainTabWindow_Squads.AvailableListRect.get_height () - MainTabWindow_Squads.MemberActionButtonSize.y, MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static readonly Rect SquadOrderMoveDownButtonRect = new Rect (MainTabWindow_Squads.SquadOrderMoveToTopButtonRect.get_x (), MainTabWindow_Squads.SquadOrderMoveToBottomButtonRect.get_y () - MainTabWindow_Squads.OrderButtonMargin.y - MainTabWindow_Squads.OrderButtonSize.y, MainTabWindow_Squads.OrderButtonSize.x, MainTabWindow_Squads.OrderButtonSize.y);

		public static readonly Rect SquadContentRect = new Rect (306, 56, 680, 582);

		public static readonly Rect AvailableListRect = new Rect (MainTabWindow_Squads.SquadContentMargin.x, 118, 232, 442);

		public static readonly Rect MemberListRect = new Rect (MainTabWindow_Squads.AvailableListRect.get_x () + MainTabWindow_Squads.MemberActionButtonMargin.x * 2 + MainTabWindow_Squads.MemberActionButtonSize.x + MainTabWindow_Squads.AvailableListRect.get_width (), MainTabWindow_Squads.AvailableListRect.get_y (), MainTabWindow_Squads.AvailableListRect.get_width (), MainTabWindow_Squads.AvailableListRect.get_height ());

		public static readonly Rect AddMemberButtonRect = new Rect (MainTabWindow_Squads.AvailableListRect.get_x () + MainTabWindow_Squads.AvailableListRect.get_width () + MainTabWindow_Squads.MemberActionButtonMargin.x, MainTabWindow_Squads.AvailableListRect.get_y () + MainTabWindow_Squads.AvailableListRect.get_height () / 2 - (MainTabWindow_Squads.MemberActionButtonSize.y + MainTabWindow_Squads.MemberActionButtonMargin.y / 2), MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static readonly Rect RemoveMemberButtonRect = new Rect (MainTabWindow_Squads.AddMemberButtonRect.get_x (), MainTabWindow_Squads.AddMemberButtonRect.get_y () + MainTabWindow_Squads.AddMemberButtonRect.get_height () + MainTabWindow_Squads.MemberActionButtonMargin.y, MainTabWindow_Squads.MemberActionButtonSize.x, MainTabWindow_Squads.MemberActionButtonSize.y);

		public static Texture2D ButtonTexReorderDown;

		protected static Color ListHeaderColor = new Color (0.8, 0.8, 0.8);

		protected static Color ListTextColor = new Color (0.7216, 0.7647, 0.902);

		protected static Color SelectedRowColor = new Color (0.2588, 0.2588, 0.2588);

		protected static Color SelectedTextColor = new Color (0.902, 0.8314, 0);

		protected static Color AlternateRowColor = new Color (0.1095, 0.125, 0.1406);

		protected static Color ArrowButtonColor = new Color (0.9137, 0.9137, 0.9137);

		protected static Color ArrowButtonHighlightColor = Color.get_white ();

		protected static Color InactiveButtonColor = new Color (1, 1, 1, 0.5);

		public static Texture2D BackgroundColorTexture;

		public static Texture2D RenameSquadButton;

		public static Texture2D ButtonTexReorderBottom;

		public static Texture2D ButtonTexReorderTop;

		//
		// Fields
		//
		private ListWidget<Squad, ListWidgetLabelDrawer<Squad>> squadListWidget;

		private HashSet<TrackedColonist> tempSelectedMembers = new HashSet<TrackedColonist> ();

		private HashSet<TrackedColonist> tempSelectedAvailable = new HashSet<TrackedColonist> ();

		protected List<TrackedColonist> tempColonists = new List<TrackedColonist> ();

		private List<Squad> tempSquadList = new List<Squad> ();

		private bool moveMemberToBottomButtonEnabled;

		private ListWidget<TrackedColonist, ColonistRowDrawer> availableColonistsWidget;

		private bool moveMemberToTopButtonEnabled;

		private SquadManager squadManager;

		private Squad selectedSquad;

		private List<Pawn> tempPawnList = new List<Pawn> ();

		protected List<int> tempAllIndices = new List<int> ();

		protected List<int> tempNotSelectedList = new List<int> ();

		protected List<int> tempSortedSelectedList = new List<int> ();

		protected List<int> tempSelectedIndices = new List<int> ();

		private int? visibleSquadCount;

		private bool reorderMemberDownButtonEnabled;

		private ListWidget<TrackedColonist, ColonistRowDrawer> squadMembersWidget;

		private bool reorderSquadUpButtonEnabled;

		private bool reorderSquadDownButtonEnabled;

		private bool moveSquadToTopButtonEnabled;

		private bool moveSquadToBottomButtonEnabled;

		private bool reorderMemberUpButtonEnabled;

		private bool deleteColonistButtonEnabled;

		private bool removeMembersButtonEnabled;

		private bool addMembersButtonEnabled;

		//
		// Properties
		//
		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (1010, 684);
			}
		}

		protected int VisibleSquadCount {
			get {
				int? num = this.visibleSquadCount;
				if (!num.HasValue) {
					this.visibleSquadCount = new int? ((from s in this.squadManager.Squads
					where s.ShowInColonistBar && s.Pawns.Count > 0
					select s).Count<Squad> ());
				}
				return this.visibleSquadCount.Value;
			}
		}

		//
		// Constructors
		//
		public MainTabWindow_Squads ()
		{
			this.squadListWidget = new ListWidget<Squad, ListWidgetLabelDrawer<Squad>> (new ListWidgetLabelDrawer<Squad> ((Squad s) => s.Name));
			this.squadListWidget.SingleSelectionChangedEvent += new ListWidgetSingleSelectionChangedHandler<Squad> (this.SelectSquad);
			this.availableColonistsWidget = new ListWidget<TrackedColonist, ColonistRowDrawer> (new ColonistRowDrawer ());
			this.availableColonistsWidget.SupportsMultiSelect = true;
			this.availableColonistsWidget.MultiSelectionChangedEvent += new ListWidgetMultiSelectionChangedHandler<TrackedColonist> (this.SelectAvailableColonists);
			this.availableColonistsWidget.BackgroundColor = new Color (0.0664, 0.082, 0.0938);
			this.squadMembersWidget = new ListWidget<TrackedColonist, ColonistRowDrawer> (new ColonistRowDrawer ());
			this.squadMembersWidget.SupportsMultiSelect = true;
			this.squadMembersWidget.MultiSelectionChangedEvent += new ListWidgetMultiSelectionChangedHandler<TrackedColonist> (this.SelectSquadMembers);
			this.squadMembersWidget.BackgroundColor = new Color (0.0664, 0.082, 0.0938);
		}

		//
		// Static Methods
		//
		public static void ResetTextures ()
		{
			MainTabWindow_Squads.ButtonTexReorderUp = ContentFinder<Texture2D>.Get ("EdB/Interface/Squads/ArrowUp", true);
			MainTabWindow_Squads.ButtonTexReorderDown = ContentFinder<Texture2D>.Get ("EdB/Interface/Squads/ArrowDown", true);
			MainTabWindow_Squads.ButtonTexReorderTop = ContentFinder<Texture2D>.Get ("EdB/Interface/Squads/ArrowTop", true);
			MainTabWindow_Squads.ButtonTexReorderBottom = ContentFinder<Texture2D>.Get ("EdB/Interface/Squads/ArrowBottom", true);
			MainTabWindow_Squads.RenameSquadButton = ContentFinder<Texture2D>.Get ("UI/Buttons/Rename", true);
			MainTabWindow_Squads.BackgroundColorTexture = SolidColorMaterials.NewSolidColorTexture (new Color (0.0508, 0.0664, 0.0742));
		}

		//
		// Methods
		//
		protected void AddSelectedMembers ()
		{
			this.tempColonists.Clear ();
			this.tempColonists.AddRange (this.availableColonistsWidget.SelectedItems);
			if (this.tempColonists.Count == 0) {
				return;
			}
			this.tempPawnList.Clear ();
			foreach (TrackedColonist current in this.squadMembersWidget.Items) {
				this.tempPawnList.Add (current.Pawn);
			}
			foreach (TrackedColonist current2 in this.tempColonists) {
				this.tempPawnList.Add (current2.Pawn);
			}
			this.squadManager.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList);
			this.ResetAvailableColonists (this.selectedSquad);
			this.ResetSquadMembers (this.selectedSquad);
			this.ResetVisibleSquadCount ();
		}

		protected string CreateDefaultSquadName ()
		{
			int num = this.squadManager.Squads.Count;
			bool flag = false;
			int num2 = 10000;
			string name;
			do {
				name = Translator.Translate ("EdB.Squads.DefaultSquadName", new object[] {
					num
				});
				if ((from s in this.squadManager.Squads
				where name.Equals (s.Name)
				select s).Count<Squad> () == 0) {
					flag = true;
				}
				else {
					num++;
					if (num > num2) {
						flag = true;
					}
				}
			}
			while (!flag);
			return name;
		}

		protected void DeleteSelectedColonists ()
		{
			this.tempColonists.Clear ();
			if (this.availableColonistsWidget.SelectedIndices.Count > 0) {
				this.tempColonists.AddRange (this.availableColonistsWidget.SelectedItems);
			}
			else if (this.squadMembersWidget.SelectedIndices.Count > 0) {
				this.tempColonists.AddRange (this.squadMembersWidget.SelectedItems);
			}
			foreach (TrackedColonist current in this.tempColonists) {
				if (current.Dead || current.Missing) {
					ColonistTracker.Instance.StopTrackingPawn (current.Pawn);
				}
			}
		}

		protected void DeleteSquad ()
		{
			Find.get_WindowStack ().Add (new Dialog_Confirm (Translator.Translate ("EdB.Squads.Window.DeleteSquad.Confirm"), delegate {
				int num = this.squadListWidget.SelectedIndices [0];
				this.squadManager.RemoveSquad (this.selectedSquad);
				if (num >= this.squadManager.Squads.Count) {
					num--;
				}
				this.squadListWidget.ResetItems (this.squadManager.Squads);
				this.squadListWidget.Select (num);
			}, true));
			this.ResetVisibleSquadCount ();
		}

		public override void DoWindowContents (Rect inRect)
		{
			base.DoWindowContents (inRect);
			Text.set_Font (1);
			Rect rect = new Rect (0, 0, inRect.get_width (), 40);
			Text.set_Font (2);
			Text.set_Anchor (4);
			Widgets.Label (rect, Translator.Translate ("EdB.Squads.Window.Header"));
			Text.set_Anchor (0);
			Text.set_Font (1);
			Text.set_Font (0);
			Text.set_Anchor (6);
			GUI.set_color (MainTabWindow_Squads.ListHeaderColor);
			Widgets.Label (new Rect (MainTabWindow_Squads.SquadListRect.get_x (), MainTabWindow_Squads.SquadListRect.get_y () - 23, 225, 30), Translator.Translate ("EdB.Squads.Window.SquadList.Header"));
			Text.set_Anchor (0);
			GUI.set_color (Color.get_white ());
			Text.set_Font (1);
			this.squadListWidget.DrawWidget (MainTabWindow_Squads.SquadListRect);
			this.EnableSquadReorderButtons ();
			if (Button.IconButton (MainTabWindow_Squads.SquadOrderMoveToTopButtonRect, MainTabWindow_Squads.ButtonTexReorderTop, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveSquadToTopButtonEnabled)) {
				this.MoveSelectedSquadToTop ();
			}
			if (Button.IconButton (MainTabWindow_Squads.SquadOrderMoveUpButtonRect, MainTabWindow_Squads.ButtonTexReorderUp, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderSquadUpButtonEnabled)) {
				this.MoveSelectedSquadUp ();
			}
			if (Button.IconButton (MainTabWindow_Squads.SquadOrderMoveDownButtonRect, MainTabWindow_Squads.ButtonTexReorderDown, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderSquadDownButtonEnabled)) {
				this.MoveSelectedSquadDown ();
			}
			if (Button.IconButton (MainTabWindow_Squads.SquadOrderMoveToBottomButtonRect, MainTabWindow_Squads.ButtonTexReorderBottom, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveSquadToBottomButtonEnabled)) {
				this.MoveSelectedSquadToBottom ();
			}
			GUI.set_color (Color.get_white ());
			if (Widgets.TextButton (MainTabWindow_Squads.NewSquadButtonRect, Translator.Translate ("EdB.Squads.Window.NewSquad.Button"), true, false)) {
				this.NewSquad ();
			}
			bool enabled = this.selectedSquad != null && this.selectedSquad != this.squadManager.AllColonistsSquad;
			if (Button.TextButton (MainTabWindow_Squads.DeleteSquadButtonRect, Translator.Translate ("EdB.Squads.Window.DeleteSquad.Button"), true, false, enabled)) {
				this.DeleteSquad ();
			}
			GUI.DrawTexture (MainTabWindow_Squads.SquadContentRect, MainTabWindow_Squads.BackgroundColorTexture);
			try {
				GUI.BeginGroup (MainTabWindow_Squads.SquadContentRect);
				Text.set_Font (2);
				Widgets.Label (MainTabWindow_Squads.SquadHeaderRect, this.selectedSquad.Name);
				Vector2 vector = Text.CalcSize (this.selectedSquad.Name);
				Text.set_Font (1);
				int count = this.selectedSquad.Pawns.Count;
				string text;
				if (count == 0) {
					text = Translator.Translate ("EdB.Squads.Window.SquadMemberCount.Zero");
				}
				else if (count == 1) {
					text = Translator.Translate ("EdB.Squads.Window.SquadMemberCount.One");
				}
				else {
					text = Translator.Translate ("EdB.Squads.Window.SquadMemberCount", new object[] {
						count
					});
				}
				GUI.set_color (MainTabWindow_Squads.ListHeaderColor);
				Widgets.Label (MainTabWindow_Squads.MemberCountRect, text);
				GUI.set_color (Color.get_white ());
				Rect rect2 = new Rect (MainTabWindow_Squads.SquadHeaderRect.get_x () + vector.x + 8, MainTabWindow_Squads.SquadHeaderRect.get_y () - 3, 30, 30);
				if (this.selectedSquad != this.squadManager.AllColonistsSquad) {
					TooltipHandler.TipRegion (rect2, new TipSignal (Translator.Translate ("EdB.Squads.Window.RenameSquad.Button.Tip")));
					if (Widgets.ImageButton (rect2, MainTabWindow_Squads.RenameSquadButton)) {
						Find.get_WindowStack ().Add (new Dialog_NameSquad (this.selectedSquad, false));
					}
				}
				else {
					TooltipHandler.TipRegion (rect2, new TipSignal (Translator.Translate ("EdB.Squads.Window.RenameSquad.Disabled.Tip")));
					GUI.set_color (new Color (1, 1, 1, 0.5));
					GUI.DrawTexture (rect2, MainTabWindow_Squads.RenameSquadButton);
					GUI.set_color (Color.get_white ());
				}
				string text2 = Translator.Translate ("EdB.Squads.Window.SquadOption.ShowInBar");
				string text3 = Translator.Translate ("EdB.Squads.Window.SquadOption.ShowInFilters");
				Vector2 vector2 = Text.CalcSize (text2);
				Vector2 vector3 = Text.CalcSize (text3);
				Vector2 vector4 = new Vector2 (Math.Max (vector2.x, vector3.x), 28);
				vector4.x += 48;
				float num = 0;
				Rect rect3 = new Rect (MainTabWindow_Squads.SquadContentRect.get_width () - MainTabWindow_Squads.SquadContentMargin.x - vector4.x, MainTabWindow_Squads.SquadContentMargin.y, vector4.x, vector4.y);
				bool showInColonistBar = this.selectedSquad.ShowInColonistBar;
				bool flag = false;
				Widgets.LabelCheckbox (rect3, text2, ref showInColonistBar, flag);
				this.squadManager.ShowSquadInColonistBar (this.selectedSquad, showInColonistBar);
				Rect rect4 = new Rect (MainTabWindow_Squads.SquadContentRect.get_width () - MainTabWindow_Squads.SquadContentMargin.x - vector4.x, rect3.get_y () + vector4.y + num, vector4.x, vector4.y);
				bool showInOverviewTabs = this.selectedSquad.ShowInOverviewTabs;
				flag = false;
				Widgets.LabelCheckbox (rect4, text3, ref showInOverviewTabs, flag);
				if (!flag) {
					this.selectedSquad.ShowInOverviewTabs = showInOverviewTabs;
				}
				Text.set_Font (0);
				Text.set_Anchor (6);
				GUI.set_color (MainTabWindow_Squads.ListHeaderColor);
				Widgets.Label (new Rect (MainTabWindow_Squads.AvailableListRect.get_x (), MainTabWindow_Squads.AvailableListRect.get_y () - 23, 210, 30), Translator.Translate ("EdB.Squads.Window.AvailableList.Header"));
				Widgets.Label (new Rect (MainTabWindow_Squads.MemberListRect.get_x (), MainTabWindow_Squads.MemberListRect.get_y () - 23, 210, 30), Translator.Translate ("EdB.Squads.Window.MemberList.Header"));
				GUI.set_color (Color.get_white ());
				Text.set_Anchor (0);
				Text.set_Font (1);
				this.availableColonistsWidget.DrawWidget (MainTabWindow_Squads.AvailableListRect);
				this.squadMembersWidget.DrawWidget (MainTabWindow_Squads.MemberListRect);
				if (Button.TextButton (MainTabWindow_Squads.AddMemberButtonRect, Translator.Translate ("EdB.Squads.Window.AddSquadMember.Button"), true, false, this.addMembersButtonEnabled)) {
					this.AddSelectedMembers ();
				}
				if (Button.TextButton (MainTabWindow_Squads.RemoveMemberButtonRect, Translator.Translate ("EdB.Squads.Window.RemoveSquadMember.Button"), true, false, this.removeMembersButtonEnabled)) {
					this.RemoveSelectedMembers ();
				}
				if (!this.removeMembersButtonEnabled && this.selectedSquad == SquadManager.Instance.AllColonistsSquad) {
					TooltipHandler.TipRegion (MainTabWindow_Squads.RemoveMemberButtonRect, new TipSignal (Translator.Translate ("EdB.Squads.Window.RemoveSquadMember.Disabled.Tip")));
				}
				if (Button.TextButton (MainTabWindow_Squads.DeleteMemberButtonRect, Translator.Translate ("EdB.Squads.Window.DeleteColonist.Button"), true, false, this.deleteColonistButtonEnabled)) {
					this.DeleteSelectedColonists ();
				}
				if (!this.deleteColonistButtonEnabled) {
					if (this.availableColonistsWidget.SelectedIndices.Count > 0 || this.squadMembersWidget.SelectedIndices.Count > 0) {
						TooltipHandler.TipRegion (MainTabWindow_Squads.DeleteMemberButtonRect, new TipSignal (Translator.Translate ("EdB.Squads.Window.DeleteColonist.Disabled.Tip")));
					}
				}
				else {
					TooltipHandler.TipRegion (MainTabWindow_Squads.DeleteMemberButtonRect, new TipSignal (Translator.Translate ("EdB.Squads.Window.DeleteColonist.Tip")));
				}
				if (Button.IconButton (MainTabWindow_Squads.MemberOrderMoveToTopButtonRect, MainTabWindow_Squads.ButtonTexReorderTop, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveMemberToTopButtonEnabled)) {
					this.MoveSelectedMemberToTop ();
				}
				if (Button.IconButton (MainTabWindow_Squads.MemberOrderMoveUpButtonRect, MainTabWindow_Squads.ButtonTexReorderUp, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderMemberUpButtonEnabled)) {
					this.MoveSelectedMemberUp ();
				}
				if (Button.IconButton (MainTabWindow_Squads.MemberOrderMoveDownButtonRect, MainTabWindow_Squads.ButtonTexReorderBottom, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.moveMemberToBottomButtonEnabled)) {
					this.MoveSelectedMemberToBottom ();
				}
				if (Button.IconButton (MainTabWindow_Squads.MemberOrderMoveToBottomButtonRect, MainTabWindow_Squads.ButtonTexReorderDown, MainTabWindow_Squads.ArrowButtonColor, MainTabWindow_Squads.ArrowButtonHighlightColor, this.reorderMemberDownButtonEnabled)) {
					this.MoveSelectedMemberDown ();
				}
			}
			finally {
				GUI.EndGroup ();
				GUI.set_color (Color.get_white ());
			}
		}

		protected void EnableSquadReorderButtons ()
		{
			this.moveSquadToTopButtonEnabled = false;
			this.moveSquadToBottomButtonEnabled = false;
			this.reorderSquadDownButtonEnabled = false;
			this.reorderSquadUpButtonEnabled = false;
			if (this.selectedSquad != null) {
				int count = this.squadManager.Squads.Count;
				int num = this.squadManager.Squads.IndexOf (this.selectedSquad);
				if (num > -1 && count > 1) {
					this.moveSquadToTopButtonEnabled = true;
					this.reorderSquadUpButtonEnabled = true;
				}
				if (num > -1 && num < count - 1 && count > 1) {
					this.moveSquadToBottomButtonEnabled = true;
					this.reorderSquadDownButtonEnabled = true;
				}
			}
		}

		protected void MoveSelectedMemberDown ()
		{
			this.tempPawnList.Clear ();
			this.tempPawnList.AddRange (this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear ();
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempSortedSelectedList.Reverse ();
			int num = this.selectedSquad.Pawns.Count - 1;
			foreach (int current in this.tempSortedSelectedList) {
				if (current + 1 > num) {
					num = current;
					this.tempSelectedIndices.Add (current);
				}
				else {
					Pawn value = this.tempPawnList [current + 1];
					this.tempPawnList [current + 1] = this.tempPawnList [current];
					this.tempPawnList [current] = value;
					num = current;
					this.tempSelectedIndices.Add (current + 1);
				}
			}
			this.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberToBottom ()
		{
			this.tempPawnList.Clear ();
			this.tempPawnList.AddRange (this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear ();
			this.tempAllIndices.Clear ();
			for (int i = 0; i < this.selectedSquad.Pawns.Count; i++) {
				this.tempAllIndices.Add (i);
			}
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempNotSelectedList.Clear ();
			this.tempNotSelectedList.AddRange (this.tempAllIndices.Except (this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempNotSelectedList) {
				this.tempPawnList [num] = this.selectedSquad.Pawns [current];
				num++;
			}
			foreach (int current2 in this.tempSortedSelectedList) {
				this.tempPawnList [num] = this.selectedSquad.Pawns [current2];
				this.tempSelectedIndices.Add (num);
				num++;
			}
			this.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberToTop ()
		{
			this.tempPawnList.Clear ();
			this.tempPawnList.AddRange (this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear ();
			this.tempAllIndices.Clear ();
			for (int i = 0; i < this.selectedSquad.Pawns.Count; i++) {
				this.tempAllIndices.Add (i);
			}
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempNotSelectedList.Clear ();
			this.tempNotSelectedList.AddRange (this.tempAllIndices.Except (this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList) {
				this.tempPawnList [num] = this.selectedSquad.Pawns [current];
				this.tempSelectedIndices.Add (num);
				num++;
			}
			foreach (int current2 in this.tempNotSelectedList) {
				this.tempPawnList [num] = this.selectedSquad.Pawns [current2];
				num++;
			}
			this.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedMemberUp ()
		{
			this.tempPawnList.Clear ();
			this.tempPawnList.AddRange (this.selectedSquad.Pawns);
			this.tempSelectedIndices.Clear ();
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadMembersWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList) {
				if (current - 1 < num) {
					num = current;
					this.tempSelectedIndices.Add (current);
				}
				else {
					Pawn value = this.tempPawnList [current - 1];
					this.tempPawnList [current - 1] = this.tempPawnList [current];
					this.tempPawnList [current] = value;
					num = current;
					this.tempSelectedIndices.Add (current - 1);
				}
			}
			this.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList, this.tempSelectedIndices);
		}

		protected void MoveSelectedSquadDown ()
		{
			this.tempSquadList.Clear ();
			this.tempSquadList.AddRange (this.squadManager.Squads);
			this.tempSelectedIndices.Clear ();
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempSortedSelectedList.Reverse ();
			int num = this.squadManager.Squads.Count - 1;
			foreach (int current in this.tempSortedSelectedList) {
				if (current + 1 > num) {
					num = current;
					this.tempSelectedIndices.Add (current);
				}
				else {
					Squad value = this.tempSquadList [current + 1];
					this.tempSquadList [current + 1] = this.tempSquadList [current];
					this.tempSquadList [current] = value;
					num = current;
					this.tempSelectedIndices.Add (current + 1);
				}
			}
			this.squadManager.ReorderSquadList (this.tempSquadList);
			this.squadListWidget.ResetItems (this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadToBottom ()
		{
			this.tempSquadList.Clear ();
			this.tempSquadList.AddRange (this.squadManager.Squads);
			this.tempSelectedIndices.Clear ();
			this.tempAllIndices.Clear ();
			for (int i = 0; i < this.squadManager.Squads.Count; i++) {
				this.tempAllIndices.Add (i);
			}
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempNotSelectedList.Clear ();
			this.tempNotSelectedList.AddRange (this.tempAllIndices.Except (this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempNotSelectedList) {
				this.tempSquadList [num] = this.squadManager.Squads [current];
				num++;
			}
			foreach (int current2 in this.tempSortedSelectedList) {
				this.tempSquadList [num] = this.squadManager.Squads [current2];
				this.tempSelectedIndices.Add (num);
				num++;
			}
			this.squadManager.ReorderSquadList (this.tempSquadList);
			this.squadListWidget.ResetItems (this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadToTop ()
		{
			this.tempSquadList.Clear ();
			this.tempSquadList.AddRange (this.squadManager.Squads);
			this.tempSelectedIndices.Clear ();
			this.tempAllIndices.Clear ();
			int count = this.squadManager.Squads.Count;
			for (int i = 0; i < count; i++) {
				this.tempAllIndices.Add (i);
			}
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			this.tempNotSelectedList.Clear ();
			this.tempNotSelectedList.AddRange (this.tempAllIndices.Except (this.tempSortedSelectedList));
			this.tempNotSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList) {
				this.tempSquadList [num] = this.squadManager.Squads [current];
				this.tempSelectedIndices.Add (num);
				num++;
			}
			foreach (int current2 in this.tempNotSelectedList) {
				this.tempSquadList [num] = this.squadManager.Squads [current2];
				num++;
			}
			this.squadManager.ReorderSquadList (this.tempSquadList);
			this.squadListWidget.ResetItems (this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void MoveSelectedSquadUp ()
		{
			this.tempSquadList.Clear ();
			this.tempSquadList.AddRange (this.squadManager.Squads);
			this.tempSelectedIndices.Clear ();
			this.tempSortedSelectedList.Clear ();
			this.tempSortedSelectedList.AddRange (this.squadListWidget.SelectedIndices);
			this.tempSortedSelectedList.Sort ();
			int num = 0;
			foreach (int current in this.tempSortedSelectedList) {
				if (current - 1 < num) {
					num = current;
					this.tempSelectedIndices.Add (current);
				}
				else {
					Squad value = this.tempSquadList [current - 1];
					this.tempSquadList [current - 1] = this.tempSquadList [current];
					this.tempSquadList [current] = value;
					num = current;
					this.tempSelectedIndices.Add (current - 1);
				}
			}
			this.squadManager.ReorderSquadList (this.tempSquadList);
			this.squadListWidget.ResetItems (this.tempSquadList);
			this.squadListWidget.SelectedIndices = this.tempSelectedIndices;
		}

		protected void NewSquad ()
		{
			Squad squad = new Squad ();
			squad.Name = this.CreateDefaultSquadName ();
			this.squadManager.AddSquad (squad);
			this.squadListWidget.ResetItems (this.squadManager.Squads);
			this.squadListWidget.Select (squad);
			this.SelectSquad (squad);
			Find.get_WindowStack ().Add (new Dialog_NameSquad (squad, true));
			this.ResetVisibleSquadCount ();
		}

		public override void PostClose ()
		{
			base.PostClose ();
		}

		public override void PostOpen ()
		{
			base.PostOpen ();
			this.squadManager = SquadManager.Instance;
			this.squadListWidget.Reset ();
			this.availableColonistsWidget.Reset ();
			this.squadMembersWidget.Reset ();
			foreach (Squad current in this.squadManager.Squads) {
				this.squadListWidget.Add (current);
			}
			this.squadListWidget.Select (0);
		}

		protected void RemoveSelectedMembers ()
		{
			this.tempColonists.Clear ();
			this.tempColonists.AddRange (this.squadMembersWidget.SelectedItems);
			if (this.tempColonists.Count == 0) {
				return;
			}
			this.tempPawnList.Clear ();
			this.tempPawnList.AddRange (this.selectedSquad.Pawns);
			foreach (TrackedColonist current in this.tempColonists) {
				this.tempPawnList.Remove (current.Pawn);
			}
			this.squadManager.ReplaceSquadPawns (this.selectedSquad, this.tempPawnList);
			this.ResetAvailableColonists (this.selectedSquad);
			this.ResetSquadMembers (this.selectedSquad);
			this.ResetVisibleSquadCount ();
		}

		protected void ReplaceSquadPawns (Squad squad, List<Pawn> pawns, List<int> selectedIndices)
		{
			this.squadManager.ReplaceSquadPawns (squad, pawns);
			this.ResetSquadMembers (squad);
			this.squadMembersWidget.SelectedIndices = selectedIndices;
		}

		protected void ResetAvailableColonists (Squad squad)
		{
			List<TrackedColonist> list = new List<TrackedColonist> ();
			foreach (Pawn current in this.squadManager.AllColonistsSquad.Pawns) {
				if (!squad.Pawns.Contains (current)) {
					TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist (current);
					if (trackedColonist != null) {
						list.Add (trackedColonist);
					}
				}
			}
			this.availableColonistsWidget.ResetItems (list);
		}

		protected void ResetSquadMembers (Squad squad)
		{
			if (squad != null) {
				List<TrackedColonist> list = new List<TrackedColonist> ();
				foreach (Pawn current in squad.Pawns) {
					TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist (current);
					if (trackedColonist != null) {
						list.Add (trackedColonist);
					}
				}
				this.squadMembersWidget.ResetItems (list);
			}
			else {
				this.squadMembersWidget.ResetItems (new List<TrackedColonist> ());
			}
		}

		protected void ResetVisibleSquadCount ()
		{
			this.visibleSquadCount = null;
		}

		protected void SelectAvailableColonists (List<TrackedColonist> colonists)
		{
			if (colonists.Count > 0) {
				this.addMembersButtonEnabled = true;
				this.squadMembersWidget.ClearSelection ();
				this.deleteColonistButtonEnabled = true;
				foreach (TrackedColonist current in this.squadMembersWidget.SelectedItems) {
					if (!current.Dead && !current.Missing) {
						this.deleteColonistButtonEnabled = false;
						break;
					}
				}
			}
			else {
				this.addMembersButtonEnabled = false;
				this.deleteColonistButtonEnabled = false;
			}
		}

		protected void SelectSquad (Squad squad)
		{
			this.selectedSquad = squad;
			if (squad == null) {
				this.ResetAvailableColonists (this.squadManager.AllColonistsSquad);
				this.ResetSquadMembers (null);
			}
			else {
				this.ResetAvailableColonists (squad);
				this.ResetSquadMembers (squad);
			}
			this.ResetVisibleSquadCount ();
		}

		protected void SelectSquadMembers (List<TrackedColonist> colonists)
		{
			if (colonists.Count > 0) {
				this.removeMembersButtonEnabled = (this.selectedSquad != this.squadManager.AllColonistsSquad);
				this.availableColonistsWidget.ClearSelection ();
				this.moveMemberToTopButtonEnabled = false;
				this.moveMemberToBottomButtonEnabled = false;
				this.reorderMemberDownButtonEnabled = false;
				this.reorderMemberUpButtonEnabled = false;
				int count = colonists.Count;
				foreach (int current in this.squadMembersWidget.SelectedIndices) {
					if (current >= count) {
						this.moveMemberToTopButtonEnabled = true;
						this.reorderMemberUpButtonEnabled = true;
						break;
					}
				}
				int num = this.selectedSquad.Pawns.Count - colonists.Count;
				foreach (int current2 in this.squadMembersWidget.SelectedIndices) {
					if (current2 < num) {
						this.moveMemberToBottomButtonEnabled = true;
						this.reorderMemberDownButtonEnabled = true;
						break;
					}
				}
				this.deleteColonistButtonEnabled = true;
				foreach (TrackedColonist current3 in this.squadMembersWidget.SelectedItems) {
					if (!current3.Dead && !current3.Missing) {
						this.deleteColonistButtonEnabled = false;
						break;
					}
				}
			}
			else {
				this.removeMembersButtonEnabled = false;
				this.deleteColonistButtonEnabled = false;
				this.moveMemberToTopButtonEnabled = false;
				this.moveMemberToBottomButtonEnabled = false;
				this.reorderMemberDownButtonEnabled = false;
				this.reorderMemberUpButtonEnabled = false;
			}
		}

		public void SquadChanged (Squad squad)
		{
			if (this.selectedSquad != null && (squad == SquadManager.Instance.AllColonistsSquad || squad == this.selectedSquad) && this.availableColonistsWidget != null && this.squadMembersWidget != null) {
				this.tempSelectedAvailable.Clear ();
				this.tempSelectedMembers.Clear ();
				foreach (TrackedColonist current in this.availableColonistsWidget.SelectedItems) {
					this.tempSelectedAvailable.Add (current);
				}
				foreach (TrackedColonist current2 in this.squadMembersWidget.SelectedItems) {
					this.tempSelectedMembers.Add (current2);
				}
				this.ResetSquadMembers (this.selectedSquad);
				this.ResetAvailableColonists (this.selectedSquad);
				this.availableColonistsWidget.Select (this.tempSelectedAvailable);
				this.squadMembersWidget.Select (this.tempSelectedMembers);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MainTabWindow_Work : MainTabWindow_PawnListWithSquads
	{
		//
		// Static Fields
		//
		private const float TopAreaHeight = 40;

		protected const float LabelRowHeight = 50;

		private static List<WorkTypeDef> VisibleWorkTypeDefsInPriorityOrder;

		//
		// Fields
		//
		private float workColumnSpacing = -1;

		protected SquadPriorities squadPriorities = new SquadPriorities ();

		//
		// Properties
		//
		public override Vector2 RequestedTabSize {
			get {
				return new Vector2 (1010, this.WindowHeight);
			}
		}

		protected override float WindowHeight {
			get {
				return 90 + (float)base.get_PawnsCount () * 30 + 65 + base.ExtraHeight;
			}
		}

		//
		// Static Methods
		//
		public static void Reinit ()
		{
			MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder = (from def in WorkTypeDefsUtility.get_WorkTypeDefsInPriorityOrder ()
			where def.visible
			select def).ToList<WorkTypeDef> ();
		}

		//
		// Methods
		//
		public override void DoWindowContents (Rect rect)
		{
			base.DoWindowContents (rect);
			Rect rect2 = new Rect (0, 0, rect.get_width (), 40);
			GUI.BeginGroup (rect2);
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			Text.set_Anchor (0);
			Rect rect3 = new Rect (5, 5, 140, 30);
			bool useWorkPriorities = Find.get_Map ().playSettings.useWorkPriorities;
			Widgets.LabelCheckbox (rect3, Translator.Translate ("ManualPriorities"), ref Find.get_Map ().playSettings.useWorkPriorities, false);
			if (useWorkPriorities != Find.get_Map ().playSettings.useWorkPriorities) {
				foreach (Pawn current in Find.get_ListerPawns ().get_FreeColonists ()) {
					current.workSettings.Notify_UseWorkPrioritiesChanged ();
				}
			}
			float num = rect2.get_width () / 3;
			float num2 = rect2.get_width () * 2 / 3;
			Rect rect4 = new Rect (num - 50, 5, 160, 30);
			Rect rect5 = new Rect (num2 - 50, 5, 160, 30);
			GUI.set_color (new Color (1, 1, 1, 0.5));
			Text.set_Anchor (1);
			Text.set_Font (0);
			Widgets.Label (rect4, "<= " + Translator.Translate ("HigherPriority"));
			Widgets.Label (rect5, Translator.Translate ("LowerPriority") + " =>");
			Text.set_Font (1);
			Text.set_Anchor (0);
			GUI.EndGroup ();
			Rect rect6 = new Rect (0, 40, rect.get_width (), rect.get_height () - 40);
			GUI.BeginGroup (rect6);
			Text.set_Font (1);
			GUI.set_color (Color.get_white ());
			float squadRowHeight = base.SquadRowHeight;
			Rect rect7 = new Rect (0, 50 + squadRowHeight, rect6.get_width (), rect6.get_height () - 50 - base.PawnListScrollHeightReduction);
			this.workColumnSpacing = (rect6.get_width () - 16 - 175) / (float)MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count;
			float num3 = 175;
			int num4 = 0;
			foreach (WorkTypeDef current2 in MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder) {
				Vector2 vector = Text.CalcSize (current2.labelShort);
				float num5 = num3 + 15;
				Rect rect8 = new Rect (num5 - vector.x / 2, 0, vector.x, vector.y);
				if (num4 % 2 == 1) {
					rect8.set_y (rect8.get_y () + 20);
				}
				if (Mouse.IsOver (rect8)) {
					Widgets.DrawHighlight (rect8);
				}
				Text.set_Anchor (4);
				Widgets.Label (rect8, current2.labelShort);
				WorkTypeDef localDef = current2;
				TooltipHandler.TipRegion (rect8, new TipSignal (() => localDef.gerundLabel + "

" + localDef.description, localDef.GetHashCode ()));
				GUI.set_color (new Color (1, 1, 1, 0.3));
				Widgets.DrawLineVertical (num5, rect8.get_yMax () - 3, 50 - rect8.get_yMax () + 3);
				Widgets.DrawLineVertical (num5 + 1, rect8.get_yMax () - 3, 50 - rect8.get_yMax () + 3);
				GUI.set_color (Color.get_white ());
				num3 += this.workColumnSpacing;
				num4++;
			}
			base.DrawRows (rect7);
			if (base.SquadRowEnabled) {
				this.DrawSquadRow (new Rect (0, 50, rect6.get_width () - 16, squadRowHeight));
			}
			GUI.EndGroup ();
			if (base.SquadFilteringEnabled) {
				Text.set_Font (1);
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
				this.DrawSquadSelectionDropdown (new Rect (rect.get_x (), rect.get_y () + rect.get_height () - MainTabWindow_PawnListWithSquads.FooterButtonHeight, MainTabWindow_PawnListWithSquads.SquadFilterButtonWidth, MainTabWindow_PawnListWithSquads.FooterButtonHeight));
			}
		}

		protected override void DrawPawnRow (Rect rect, Pawn p)
		{
			float num = 175;
			Text.set_Font (2);
			for (int i = 0; i < MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count; i++) {
				WorkTypeDef workTypeDef = MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder [i];
				Vector2 topLeft = new Vector2 (num, rect.get_y () + 2.5);
				WidgetsWork.DrawWorkBoxFor (topLeft, p, workTypeDef);
				Rect rect2 = new Rect (topLeft.x, topLeft.y, 25, 25);
				TooltipHandler.TipRegion (rect2, WidgetsWork.TipForPawnWorker (p, workTypeDef));
				num += this.workColumnSpacing;
			}
		}

		protected void DrawSquadRow (Rect rect)
		{
			GUI.DrawTexture (rect, MainTabWindow_PawnListWithSquads.SquadRowBackground);
			float num = 175;
			Text.set_Font (2);
			for (int i = 0; i < MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder.Count; i++) {
				WorkTypeDef wType = MainTabWindow_Work.VisibleWorkTypeDefsInPriorityOrder [i];
				Vector2 topLeft = new Vector2 (num, rect.get_y () + 2.5);
				WidgetsWork.DrawWorkBoxForSquad (topLeft, wType, this.squadPriorities, this.pawns);
				num += this.workColumnSpacing;
			}
			GUI.BeginGroup (rect);
			GUI.set_color (new Color (1, 1, 1, 0.2));
			Widgets.DrawLineHorizontal (0, 0, rect.get_width ());
			GUI.set_color (new Color (1, 1, 1, 0.35));
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 3, rect.get_width ());
			Widgets.DrawLineHorizontal (0, base.SquadRowHeight - 2, rect.get_width ());
			Text.set_Font (1);
			Text.set_Anchor (3);
			Text.set_WordWrap (false);
			Rect rect2 = new Rect (0, 0, 175, 30);
			rect2.set_xMin (rect2.get_xMin () + 15);
			GUI.set_color (new Color (1, 1, 1, 1));
			if (base.SquadFilteringEnabled && SquadManager.Instance.SquadFilter != null) {
				Widgets.Label (rect2, SquadManager.Instance.SquadFilter.Name);
			}
			else {
				Widgets.Label (rect2, Translator.Translate ("EdB.Squads.AllColonistsSquadName"));
			}
			Text.set_Anchor (0);
			Text.set_WordWrap (true);
			GUI.set_color (Color.get_white ());
			GUI.EndGroup ();
		}

		public override void PreOpen ()
		{
			base.PreOpen ();
			MainTabWindow_Work.Reinit ();
			this.squadPriorities.Reset ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class MapComponentsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "MapComponents";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			for (int i = 0; i < Find.get_Map ().components.Count; i++) {
				Find.get_Map ().components [i].MapComponentOnGUI ();
			}
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MaterialResolver
	{
		//
		// Static Methods
		//
		public static TextureColorPair Resolve (Thing thing)
		{
			if (thing.def.apparel != null) {
				return new TextureColorPair (thing.def.uiIcon, thing.get_DrawColor ());
			}
			return new TextureColorPair (thing.def.uiIcon, thing.get_DrawColor ());
		}

		public static TextureColorPair Resolve (ThingDef def)
		{
			return new TextureColorPair (def.uiIcon, Color.get_white ());
		}
	}
}
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class MedicalCareUtility
	{
		//
		// Static Fields
		//
		public const float CareSetterHeight = 28;

		public static float ButtonPadding = 8;

		public static Vector2 ButtonSize = new Vector2 (32, 32);

		public static Texture2D HealthOptionInactive = null;

		public const float CareSetterWidth = 140;

		public static Texture2D[] careTextures;

		public static Texture2D HealthOptionActive = null;

		//
		// Static Methods
		//
		public static bool AllowsMedicine (this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat) {
			case 0:
				return false;
			case 1:
				return false;
			case 2:
				return (double)StatExtension.GetStatValueAbstract (meds, StatDefOf.MedicalPotency, null) <= 0.99;
			case 3:
				return (double)StatExtension.GetStatValueAbstract (meds, StatDefOf.MedicalPotency, null) <= 1.01;
			case 4:
				return true;
			default:
				throw new InvalidOperationException ();
			}
		}

		public static string GetLabel (this MedicalCareCategory cat)
		{
			return Translator.Translate ("MedicalCareCategory_" + cat);
		}

		public static void MedicalCareSetter (Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect (rect.get_x (), rect.get_y (), MedicalCareUtility.ButtonSize.x, MedicalCareUtility.ButtonSize.y);
			for (int i = 0; i < 5; i++) {
				MedicalCareCategory mc = (byte)i;
				if (medCare == mc) {
					GUI.DrawTexture (rect2, MedicalCareUtility.HealthOptionActive);
				}
				else {
					GUI.DrawTexture (rect2, MedicalCareUtility.HealthOptionInactive);
				}
				if (Mouse.IsOver (rect)) {
				}
				Rect rect3 = GenUI.ContractedBy (rect2, 2);
				GUI.DrawTexture (rect3, MedicalCareUtility.careTextures [i]);
				if (Widgets.InvisibleButton (rect2)) {
					medCare = mc;
					SoundStarter.PlayOneShotOnCamera (SoundDefOf.TickHigh);
				}
				TooltipHandler.TipRegion (rect2, () => mc.GetLabel (), 632165 + i * 17);
				rect2.set_x (rect2.get_x () + (MedicalCareUtility.ButtonSize.x + MedicalCareUtility.ButtonPadding));
			}
		}

		public static void Reset ()
		{
			MedicalCareUtility.careTextures = new Texture2D[5];
			MedicalCareUtility.careTextures [0] = ContentFinder<Texture2D>.Get ("UI/Icons/Medical/NoCare", true);
			MedicalCareUtility.careTextures [1] = ContentFinder<Texture2D>.Get ("UI/Icons/Medical/NoMeds", true);
			MedicalCareUtility.careTextures [2] = ThingDefOf.HerbalMedicine.uiIcon;
			MedicalCareUtility.careTextures [3] = ThingDefOf.Medicine.uiIcon;
			MedicalCareUtility.careTextures [4] = ThingDefOf.GlitterworldMedicine.uiIcon;
			MedicalCareUtility.HealthOptionActive = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/HealthOptionActive", true);
			MedicalCareUtility.HealthOptionInactive = ContentFinder<Texture2D>.Get ("EdB/Interface/TabReplacement/HealthOptionInactive", true);
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class MessagesComponent : IUpdatedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "Messages";
			}
		}

		//
		// Methods
		//
		public void Update ()
		{
			Messages.Update ();
		}
	}
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MouseoverReadout
	{
		//
		// Static Fields
		//
		private const float YInterval = 19;

		private static readonly Vector2 BotLeft = new Vector2 (15, 65);

		//
		// Fields
		//
		private TerrainDef cachedTerrain;

		private string cachedTerrainString;

		//
		// Methods
		//
		public void MouseoverReadoutOnGUI ()
		{
			if (Find.get_MainTabsRoot ().get_OpenTab () != null) {
				return;
			}
			GenUI.DrawTextWinterShadow (new Rect (256, (float)(Screen.get_height () - 256), -256, 256));
			Text.set_Font (1);
			GUI.set_color (new Color (1, 1, 1, 0.8));
			IntVec3 intVec = Gen.MouseCell ();
			if (!GenGrid.InBounds (intVec)) {
				return;
			}
			float num = 0;
			Rect rect;
			if (GridsUtility.Fogged (intVec)) {
				rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
				Widgets.Label (rect, Translator.Translate ("Undiscovered"));
				GUI.set_color (Color.get_white ());
				return;
			}
			rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
			Widgets.Label (rect, PsychGlowUtility.GetLabel (Find.get_GlowGrid ().PsychGlowAt (intVec)) + " (" + GenText.ToStringPercent (Find.get_GlowGrid ().GameGlowAt (intVec)) + ")");
			num += 19;
			rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
			TerrainDef terrain = GridsUtility.GetTerrain (intVec);
			if (terrain != this.cachedTerrain) {
				this.cachedTerrainString = terrain.get_LabelCap () + ((terrain.passability != 2) ? (" (" + Translator.Translate ("WalkSpeed", new object[] {
					this.SpeedPercentString ((float)terrain.pathCost)
				}) + ")") : null);
				this.cachedTerrain = terrain;
			}
			Widgets.Label (rect, this.cachedTerrainString);
			num += 19;
			Zone zone = GridsUtility.GetZone (intVec);
			if (zone != null) {
				rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
				string label = zone.label;
				Widgets.Label (rect, label);
				num += 19;
			}
			float depth = Find.get_SnowGrid ().GetDepth (intVec);
			if ((double)depth > 0.03) {
				rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
				SnowCategory snowCategory = SnowUtility.GetSnowCategory (depth);
				string text = SnowUtility.GetDescription (snowCategory) + " (" + Translator.Translate ("WalkSpeed", new object[] {
					this.SpeedPercentString ((float)SnowUtility.MovementTicksAddOn (snowCategory))
				}) + ")";
				Widgets.Label (rect, text);
				num += 19;
			}
			List<Thing> list = Find.get_ThingGrid ().ThingsListAt (intVec);
			for (int i = 0; i < list.Count; i++) {
				Thing thing = list [i];
				if (thing.def.category != 7) {
					rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
					string labelMouseover = thing.get_LabelMouseover ();
					Widgets.Label (rect, labelMouseover);
					num += 19;
				}
			}
			RoofDef roof = GridsUtility.GetRoof (intVec);
			if (roof != null) {
				rect = new Rect (MouseoverReadout.BotLeft.x, (float)Screen.get_height () - MouseoverReadout.BotLeft.y - num, 999, 999);
				Widgets.Label (rect, roof.get_LabelCap ());
				num += 19;
			}
			GUI.set_color (Color.get_white ());
		}

		private string SpeedPercentString (float extraPathTicks)
		{
			float num = 13 / (extraPathTicks + 13);
			return GenText.ToStringPercent (num);
		}
	}
}
using System;

namespace EdB.Interface
{
	public class MouseoverReadoutComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private MouseoverReadout mouseoverReadout;

		//
		// Properties
		//
		public string Name {
			get {
				return "MouseoverReadout";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Constructors
		//
		public MouseoverReadoutComponent (MouseoverReadout mouseoverReadout)
		{
			this.mouseoverReadout = mouseoverReadout;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.mouseoverReadout.MouseoverReadoutOnGUI ();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class MultipleSelectionStringOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler (IEnumerable<string> selectedOptions);

		//
		// Static Fields
		//
		public static float LabelMargin = MultipleSelectionStringOptionsPreference.RadioButtonWidth + MultipleSelectionStringOptionsPreference.RadioButtonMargin;

		public static float RadioButtonMargin = 18;

		public static float RadioButtonWidth = 24;

		//
		// Fields
		//
		public HashSet<string> selectedOptions = new HashSet<string> ();

		public int tooltipId;

		private string setValue;

		private string stringValue;

		//
		// Properties
		//
		public abstract string DefaultValue {
			get;
		}

		public virtual bool Disabled {
			get {
				return false;
			}
		}

		public virtual bool DisplayInOptions {
			get {
				return true;
			}
		}

		public abstract string Group {
			get;
		}

		public virtual bool Indent {
			get {
				return true;
			}
		}

		public virtual string Label {
			get {
				return Translator.Translate (this.Name);
			}
		}

		public abstract string Name {
			get;
		}

		public abstract string OptionValuePrefix {
			get;
		}

		public abstract IEnumerable<string> OptionValues {
			get;
		}

		public IEnumerable<string> SelectedOptions {
			get {
				return this.selectedOptions;
			}
		}

		public virtual string Tooltip {
			get {
				return null;
			}
		}

		protected virtual int TooltipId {
			get {
				if (this.tooltipId == 0) {
					this.tooltipId = Translator.Translate (this.Tooltip).GetHashCode ();
					return this.tooltipId;
				}
				return 0;
			}
		}

		public virtual string ValueForDisplay {
			get {
				if (this.setValue != null) {
					return this.setValue;
				}
				return this.DefaultValue;
			}
		}

		public string ValueForSerialization {
			get {
				return this.stringValue;
			}
			set {
				this.stringValue = value;
				this.setValue = value;
				this.selectedOptions.Clear ();
				string[] array = this.stringValue.Split (new char[] {
					','
				});
				for (int i = 0; i < array.Length; i++) {
					string text = array [i];
					if (this.OptionValues.Contains (text)) {
						this.selectedOptions.Add (text);
					}
				}
			}
		}

		//
		// Constructors
		//
		public MultipleSelectionStringOptionsPreference ()
		{
		}

		//
		// Methods
		//
		public bool IsOptionSelected (string option)
		{
			return this.selectedOptions.Contains (option);
		}

		public void OnGUI (float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			if (disabled) {
				GUI.set_color (Dialog_InterfaceOptions.DisabledControlColor);
			}
			if (!string.IsNullOrEmpty (this.Name)) {
				string text = Translator.Translate (this.Name);
				float num = Text.CalcHeight (text, width);
				Rect rect = new Rect (positionX, positionY, width, num);
				Widgets.Label (rect, text);
				if (this.Tooltip != null) {
					TipSignal tipSignal = new TipSignal (() => Translator.Translate (this.Tooltip), this.TooltipId);
					TooltipHandler.TipRegion (rect, tipSignal);
				}
				positionY += num + Dialog_InterfaceOptions.PreferencePadding.y;
			}
			float num2 = (!this.Indent) ? 0 : Dialog_InterfaceOptions.IndentSize;
			foreach (string current in this.OptionValues) {
				string text2 = this.OptionTranslated (current);
				if (!string.IsNullOrEmpty (text2)) {
					float num3 = Text.CalcHeight (text2, width - MultipleSelectionStringOptionsPreference.LabelMargin - num2);
					Rect rect2 = new Rect (positionX - 4 + num2, positionY - 3, width + 6 - num2, num3 + 5);
					if (Mouse.IsOver (rect2)) {
						Widgets.DrawHighlight (rect2);
					}
					Rect rect3 = new Rect (positionX + num2, positionY, width - num2, num3);
					bool flag = this.IsOptionSelected (current);
					bool flag2 = flag;
					WidgetDrawer.DrawLabeledCheckbox (rect3, text2, ref flag);
					if (flag != flag2) {
						this.UpdateOption (current, flag);
					}
					positionY += num3 + Dialog_InterfaceOptions.PreferencePadding.y;
				}
			}
			positionY -= Dialog_InterfaceOptions.PreferencePadding.y;
			GUI.set_color (Color.get_white ());
		}

		public virtual string OptionTranslated (string optionValue)
		{
			return Translator.Translate (this.OptionValuePrefix + "." + optionValue);
		}

		public void UpdateOption (string option, bool value)
		{
			if (!value) {
				if (this.selectedOptions.Contains (option)) {
					this.selectedOptions.Remove (option);
					this.UpdateSerializedValue ();
					if (this.ValueChanged != null) {
						this.ValueChanged (this.selectedOptions);
					}
				}
			}
			else if (!this.selectedOptions.Contains (option)) {
				this.selectedOptions.Add (option);
				this.UpdateSerializedValue ();
				if (this.ValueChanged != null) {
					this.ValueChanged (this.selectedOptions);
				}
			}
		}

		protected void UpdateSerializedValue ()
		{
			this.stringValue = string.Join (",", this.selectedOptions.ToArray<string> ());
		}

		//
		// Events
		//
		public event MultipleSelectionStringOptionsPreference.ValueChangedHandler ValueChanged {
			add {
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<MultipleSelectionStringOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (MultipleSelectionStringOptionsPreference.ValueChangedHandler)Delegate.Combine (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove {
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<MultipleSelectionStringOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (MultipleSelectionStringOptionsPreference.ValueChangedHandler)Delegate.Remove (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceAlwaysShowSquadName : BooleanPreference
	{
		//
		// Fields
		//
		protected PreferenceEnableSquads preferenceEnableSquads;

		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override bool Disabled {
			get {
				return this.preferenceEnableSquads != null && !this.preferenceEnableSquads.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.Squads.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.Squads.Prefs.AlwaysShowSquadName";
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads {
			set {
				this.preferenceEnableSquads = value;
			}
		}

		public override string Tooltip {
			get {
				return "EdB.Squads.Prefs.AlwaysShowSquadName.Tip";
			}
		}

		public override bool Value {
			get {
				return (this.preferenceEnableSquads == null || this.preferenceEnableSquads.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceAmPm : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableAlternateTimeDisplay == null || !this.PreferenceEnableAlternateTimeDisplay.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.AlternateTimeDisplay.Prefs";
			}
		}

		public override bool Indent {
			get {
				return false;
			}
		}

		public override string Name {
			get {
				return "EdB.AlternateTimeDisplay.Prefs.AmPm";
			}
		}

		public PreferenceEnableAlternateTimeDisplay PreferenceEnableAlternateTimeDisplay {
			get;
			set;
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceColorCodedWorkPassions : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.ColorCodedWorkPassions.Pref";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.ColorCodedWorkPassions.Pref.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceCompressedStorage : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override bool DisplayInOptions {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.Inventory.Prefs.CompressedStorage";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEmptyStockpile : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.EmptyStockpile.Prefs.EmptyStockpile";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.EmptyStockpile.Prefs.EmptyStockpile.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnableAlternateTimeDisplay : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return "EdB.AlternateTimeDisplay.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.AlternateTimeDisplay.Prefs.Enabled";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnabled : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.ColonistBar.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.ColonistBar.Prefs.Enable";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnableInventory : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.Inventory.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.Inventory.Prefs.EnableInventory";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.Inventory.Prefs.EnableInventory.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquadFiltering : BooleanPreference
	{
		//
		// Fields
		//
		protected PreferenceEnableSquads preferenceEnableSquads;

		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.preferenceEnableSquads != null && !this.preferenceEnableSquads.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.Squads.Prefs";
			}
		}

		public override bool Indent {
			get {
				return false;
			}
		}

		public override string Name {
			get {
				return "EdB.Squads.Prefs.EnableSquadFiltering";
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads {
			set {
				this.preferenceEnableSquads = value;
			}
		}

		public override string Tooltip {
			get {
				return "EdB.Squads.Prefs.EnableSquadFiltering.Tip";
			}
		}

		public override bool Value {
			get {
				return (this.preferenceEnableSquads == null || this.preferenceEnableSquads.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquadRow : BooleanPreference
	{
		//
		// Fields
		//
		protected PreferenceEnableSquads preferenceEnableSquads;

		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.Squads.Prefs";
			}
		}

		public override bool Indent {
			get {
				return false;
			}
		}

		public override string Name {
			get {
				return "EdB.Squads.Prefs.EnableSquadRow";
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads {
			set {
				this.preferenceEnableSquads = value;
			}
		}

		public override string Tooltip {
			get {
				return "EdB.Squads.Prefs.EnableSquadRow.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceEnableSquads : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.Squads.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.Squads.Prefs.EnableSquads";
			}
		}

		public override string Tooltip {
			get {
				return null;
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceEnableTabReplacement : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return Screen.get_height () >= 900;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.Enable";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.TabReplacement.Prefs.Enable.Tip";
			}
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class PreferenceGroup
	{
		//
		// Fields
		//
		private string name;

		private List<IPreference> preferences = new List<IPreference> ();

		//
		// Properties
		//
		public string Name {
			get {
				return this.name;
			}
			set {
				this.name = value;
			}
		}

		public int PreferenceCount {
			get {
				return this.preferences.Count;
			}
		}

		public IEnumerable<IPreference> Preferences {
			get {
				return this.preferences;
			}
		}

		//
		// Constructors
		//
		public PreferenceGroup (string name)
		{
			this.name = name;
		}

		//
		// Methods
		//
		public void Add (IPreference preference)
		{
			this.preferences.Add (preference);
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class PreferenceHideMainTabs : MultipleSelectionStringOptionsPreference
	{
		//
		// Fields
		//
		protected List<string> options = new List<string> ();

		protected HashSet<string> exclusions = new HashSet<string> ();

		protected Dictionary<string, string> labels = new Dictionary<string, string> ();

		//
		// Properties
		//
		public override string DefaultValue {
			get {
				return string.Empty;
			}
		}

		public override bool DisplayInOptions {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.MainTabs.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.MainTabs.Prefs.HideTabs";
			}
		}

		public override string OptionValuePrefix {
			get {
				return string.Empty;
			}
		}

		public override IEnumerable<string> OptionValues {
			get {
				return this.options;
			}
		}

		public override string Tooltip {
			get {
				return "EdB.MainTabs.Prefs.HideTabs.Tip";
			}
		}

		//
		// Constructors
		//
		public PreferenceHideMainTabs ()
		{
			this.exclusions.Add ("Inspect");
			this.exclusions.Add ("Architect");
			this.exclusions.Add ("Work");
			this.exclusions.Add ("EdB_Interface_Squads");
			this.exclusions.Add ("Menu");
			IEnumerable<MainTabDef> allDefs = DefDatabase<MainTabDef>.get_AllDefs ();
			foreach (MainTabDef current in from def in allDefs
			orderby def.order
			select def) {
				if (!this.exclusions.Contains (current.defName)) {
					this.options.Add (current.defName);
					this.labels.Add (current.defName, current.get_LabelCap ());
				}
			}
		}

		//
		// Methods
		//
		public bool IsTabExcluded (string name)
		{
			return this.exclusions.Contains (name);
		}

		public override string OptionTranslated (string optionValue)
		{
			string text;
			if (this.labels.TryGetValue (optionValue, out text)) {
				return Translator.Translate ("EdB.MainTabs.Prefs.HideTabs.Option", new object[] {
					text
				});
			}
			return null;
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceIncludeUnfinished : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override bool DisplayInOptions {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.Inventory.Prefs.IncludeUnfinished";
			}
		}
	}
}
using System;
using System.Collections.Generic;

namespace EdB.Interface
{
	public class PreferenceMinuteInterval : IntegerOptionsPreference
	{
		//
		// Static Fields
		//
		protected static readonly int DefaultSelectedOption = 2;

		//
		// Fields
		//
		protected List<int> optionValues = new List<int> ();

		protected string optionLabelPrefix = "EdB.AlternateTimeDisplay.Prefs.Interval";

		//
		// Properties
		//
		public override int DefaultValue {
			get {
				return this.optionValues [PreferenceMinuteInterval.DefaultSelectedOption];
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableAlternateTimeDisplay == null || !this.PreferenceEnableAlternateTimeDisplay.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.AlternateTimeDisplay.Prefs";
			}
		}

		public override bool Indent {
			get {
				return false;
			}
		}

		public override string Name {
			get {
				return "EdB.AlternateTimeDisplay.Prefs.Interval";
			}
		}

		public override string OptionValuePrefix {
			get {
				return this.optionLabelPrefix;
			}
		}

		public override IEnumerable<int> OptionValues {
			get {
				return this.optionValues;
			}
		}

		public PreferenceEnableAlternateTimeDisplay PreferenceEnableAlternateTimeDisplay {
			get;
			set;
		}

		//
		// Constructors
		//
		public PreferenceMinuteInterval ()
		{
			this.optionValues.Add (1);
			this.optionValues.Add (5);
			this.optionValues.Add (15);
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferencePauseOnStart : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.PauseOnStart.Prefs.PauseOnStart";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.PauseOnStart.Prefs.PauseOnStart.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceRightClickOnly : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return null;
			}
		}

		public override string Name {
			get {
				return "EdB.MaterialSelection.Prefs.RightClickOnly";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.MaterialSelection.Prefs.RightClickOnly.Tip";
			}
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Verse;

namespace EdB.Interface
{
	public class Preferences
	{
		//
		// Static Fields
		//
		private static Preferences instance;

		//
		// Fields
		//
		protected bool atLeastOne;

		protected List<PreferenceGroup> miscellaneousGroup = new List<PreferenceGroup> ();

		protected Dictionary<string, IPreference> preferenceDictionary = new Dictionary<string, IPreference> ();

		protected Dictionary<string, PreferenceGroup> groupDictionary = new Dictionary<string, PreferenceGroup> ();

		protected List<PreferenceGroup> groups = new List<PreferenceGroup> ();

		//
		// Static Properties
		//
		public static Preferences Instance {
			get {
				if (Preferences.instance == null) {
					Preferences.instance = new Preferences ();
				}
				return Preferences.instance;
			}
		}

		//
		// Properties
		//
		public bool AtLeastOne {
			get {
				return this.atLeastOne;
			}
		}

		protected string FilePath {
			get {
				return Path.Combine (GenFilePaths.get_ConfigFolderPath (), "EdBInterface.xml");
			}
		}

		public IEnumerable<PreferenceGroup> Groups {
			get {
				if (this.miscellaneousGroup [0].PreferenceCount > 0) {
					return this.groups.Concat (this.miscellaneousGroup);
				}
				return this.groups;
			}
		}

		//
		// Constructors
		//
		public Preferences ()
		{
			this.Reset ();
		}

		//
		// Methods
		//
		public void Add (IPreference preference)
		{
			if (this.preferenceDictionary.ContainsKey (preference.Name)) {
				Log.Warning ("Preference already added to EdB.Interface.Preferences: " + preference.Name);
				return;
			}
			string group = preference.Group;
			PreferenceGroup preferenceGroup;
			if (group == null) {
				preferenceGroup = this.miscellaneousGroup [0];
			}
			else if (this.groupDictionary.ContainsKey (group)) {
				preferenceGroup = this.groupDictionary [group];
			}
			else {
				preferenceGroup = new PreferenceGroup (group);
				this.groups.Add (preferenceGroup);
				this.groupDictionary.Add (group, preferenceGroup);
			}
			preferenceGroup.Add (preference);
			this.preferenceDictionary.Add (preference.Name, preference);
			this.atLeastOne = true;
		}

		public void Load ()
		{
			try {
				XmlDocument xmlDocument = new XmlDocument ();
				try {
					xmlDocument.LoadXml (File.ReadAllText (this.FilePath));
				}
				catch (FileNotFoundException) {
					return;
				}
				IEnumerator enumerator = xmlDocument.ChildNodes.GetEnumerator ();
				try {
					while (enumerator.MoveNext ()) {
						object current = enumerator.Current;
						if (current is XmlElement) {
							XmlElement xmlElement = current as XmlElement;
							if ("Preferences".Equals (xmlElement.Name)) {
								IEnumerator enumerator2 = xmlElement.ChildNodes.GetEnumerator ();
								try {
									while (enumerator2.MoveNext ()) {
										object current2 = enumerator2.Current;
										if (current2 is XmlElement) {
											XmlElement xmlElement2 = current2 as XmlElement;
											string name = xmlElement2.Name;
											if (this.preferenceDictionary.ContainsKey (name)) {
												IPreference preference = this.preferenceDictionary [name];
												preference.ValueForSerialization = xmlElement2.InnerText;
											}
											else {
												Log.Warning ("Unrecognized EdB Interface preference: " + name);
											}
										}
									}
								}
								finally {
									IDisposable disposable;
									if ((disposable = (enumerator2 as IDisposable)) != null) {
										disposable.Dispose ();
									}
								}
							}
						}
					}
				}
				finally {
					IDisposable disposable2;
					if ((disposable2 = (enumerator as IDisposable)) != null) {
						disposable2.Dispose ();
					}
				}
			}
			catch (Exception arg) {
				Log.Warning ("Exception loading EdB Interface preferences: " + arg);
			}
		}

		public void Reset ()
		{
			this.groups.Clear ();
			this.groupDictionary.Clear ();
			this.preferenceDictionary.Clear ();
			this.miscellaneousGroup.Clear ();
			this.miscellaneousGroup.Add (new PreferenceGroup ("EdB.InterfaceOptions.Prefs.Miscellaneous"));
			this.atLeastOne = false;
		}

		public void Save ()
		{
			try {
				XDocument xDocument = new XDocument ();
				XElement xElement = new XElement ("Preferences");
				xDocument.Add (xElement);
				foreach (PreferenceGroup current in this.Groups) {
					foreach (IPreference current2 in current.Preferences) {
						if (!string.IsNullOrEmpty (current2.ValueForSerialization)) {
							XElement content = new XElement (current2.Name, current2.ValueForSerialization);
							xElement.Add (content);
						}
					}
				}
				xDocument.Save (this.FilePath);
			}
			catch (Exception arg) {
				Log.Warning ("Exception saving EdB Interface preferences: " + arg);
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceShowCloseButton : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return "EdB.MainTabs.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.MainTabs.Prefs.ShowCloseButton";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.MainTabs.Prefs.ShowCloseButton.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceSmallIcons : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return false;
			}
		}

		public override string Group {
			get {
				return "EdB.ColonistBar.Prefs";
			}
		}

		public override bool Indent {
			get {
				return false;
			}
		}

		public override string Name {
			get {
				return "EdB.ColonistBar.Prefs.SmallIcons";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.ColonistBar.Prefs.SmallIcons.Tip";
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabArt : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabArt";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabBills : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabBills";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabBrowseButtons : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.BrowseButtons";
			}
		}

		public override string Tooltip {
			get {
				return "EdB.TabReplacement.Prefs.BrowseButtons.Tip";
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceTabCharacter : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return Screen.get_height () >= 900;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabCharacter";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabGear : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabGear";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabGrowing : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabGrowing";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabGuestAndPrisoner : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabGuestAndPrisoner";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceTabHealth : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return Screen.get_height () >= 900;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabHealth";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class PreferenceTabNeeds : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return Screen.get_height () >= 900;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabNeeds";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public class PreferenceTabTraining : BooleanPreference
	{
		//
		// Properties
		//
		public override bool DefaultValue {
			get {
				return true;
			}
		}

		public override bool Disabled {
			get {
				return this.PreferenceEnableTabReplacement != null && !this.PreferenceEnableTabReplacement.Value;
			}
		}

		public override string Group {
			get {
				return "EdB.TabReplacement.Prefs";
			}
		}

		public override bool Indent {
			get {
				return true;
			}
		}

		public override string Name {
			get {
				return "EdB.TabReplacement.Prefs.TabTraining";
			}
		}

		public PreferenceEnableTabReplacement PreferenceEnableTabReplacement {
			get;
			set;
		}

		public override bool Value {
			get {
				return (this.PreferenceEnableTabReplacement == null || this.PreferenceEnableTabReplacement.Value) && base.Value;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class ReplacementTabs
	{
		//
		// Fields
		//
		protected Dictionary<ThingDef, List<ITab>> thingDefDictionary = new Dictionary<ThingDef, List<ITab>> ();

		protected Dictionary<Type, ITab> zoneTypeDictionary = new Dictionary<Type, ITab> ();

		//
		// Properties
		//
		public bool Empty {
			get {
				return this.thingDefDictionary.Count == 0 && this.zoneTypeDictionary.Count == 0;
			}
		}

		//
		// Methods
		//
		public void AddThingDef (ThingDef def, List<ITab> tabs)
		{
			this.thingDefDictionary.Add (def, tabs);
		}

		public void AddZoneType (Type type, ITab tab)
		{
			this.zoneTypeDictionary.Add (type, tab);
		}

		public void Clear ()
		{
			this.thingDefDictionary.Clear ();
			this.zoneTypeDictionary.Clear ();
		}

		public IEnumerable<ITab> GetTabs (Thing thing)
		{
			List<ITab> result;
			if (this.thingDefDictionary.TryGetValue (thing.def, out result)) {
				return result;
			}
			if (thing.def.inspectorTabsResolved != null) {
				return thing.def.inspectorTabsResolved;
			}
			return Enumerable.Empty<ITab> ();
		}

		public IEnumerable<ITab> GetTabs (Zone zone)
		{
			if (this.zoneTypeDictionary.Count == 0) {
				return zone.GetInspectionTabs ();
			}
			return zone.GetInspectionTabs ().Select (delegate (ITab tab) {
				ITab result;
				if (this.zoneTypeDictionary.TryGetValue (tab.GetType (), out result)) {
					return result;
				}
				return tab;
			});
		}
	}
}
using RimWorld;
using System;

namespace EdB.Interface
{
	public class ResourceReadoutComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private ResourceReadout resourceReadout;

		//
		// Properties
		//
		public string Name {
			get {
				return "ResourceReadout";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return false;
			}
		}

		//
		// Constructors
		//
		public ResourceReadoutComponent (ResourceReadout resourceReadout)
		{
			this.resourceReadout = resourceReadout;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.resourceReadout.ResourceReadoutOnGUI ();
		}
	}
}
using System;

namespace EdB.Interface
{
	public class RoomOverlaysComponent : IUpdatedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "RoomOverlays";
			}
		}

		//
		// Methods
		//
		public void Update ()
		{
			RoomStatsDrawer.DrawRoomOverlays ();
		}
	}
}
using System;

namespace EdB.Interface
{
	public class RoomStatsComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "RoomStats";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			RoomStatsDrawer.RoomStatsOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal static class RoomStatsDrawer
	{
		//
		// Static Fields
		//
		private static float scoreStageLabelColumnWidth = 0;

		private static readonly Color RelatedStatColor = new Color (0.85, 0.85, 0.85);

		private static float scoreColumnWidth = 0;

		private static float statLabelColumnWidth = 0;

		private const float SpaceBetweenColumns = 35;

		private const int WindowPadding = 19;

		private const int LineHeight = 23;

		private const int SpaceBetweenLines = 2;

		//
		// Static Properties
		//
		private static int DisplayedRoomStatsCount {
			get {
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.get_AllDefsListForReading ();
				for (int i = 0; i < allDefsListForReading.Count; i++) {
					if (!allDefsListForReading [i].isHidden) {
						num++;
					}
				}
				return num;
			}
		}

		private static bool ShouldShowRoomStats {
			get {
				if (!Find.get_PlaySettings ().showRoomStats) {
					return false;
				}
				if (Mouse.get_IsInputBlockedNow ()) {
					return false;
				}
				if (!GenGrid.InBounds (Gen.MouseCell ()) || GridsUtility.Fogged (Gen.MouseCell ())) {
					return false;
				}
				Room room = GridsUtility.GetRoom (Gen.MouseCell ());
				return room != null && room.get_Role () != RoomRoleDefOf.None;
			}
		}

		//
		// Static Methods
		//
		private static void CalculateColumnsSizes (Room room)
		{
			RoomStatsDrawer.statLabelColumnWidth = 0;
			RoomStatsDrawer.scoreColumnWidth = 0;
			RoomStatsDrawer.scoreStageLabelColumnWidth = 0;
			for (int i = 0; i < DefDatabase<RoomStatDef>.get_AllDefsListForReading ().Count; i++) {
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.get_AllDefsListForReading () [i];
				if (!roomStatDef.isHidden) {
					RoomStatsDrawer.statLabelColumnWidth = Mathf.Max (RoomStatsDrawer.statLabelColumnWidth, Text.CalcSize (roomStatDef.get_LabelCap ()).x);
					float stat = room.GetStat (roomStatDef);
					string label = roomStatDef.GetScoreStage (stat).label;
					RoomStatsDrawer.scoreStageLabelColumnWidth = Mathf.Max (RoomStatsDrawer.scoreStageLabelColumnWidth, Text.CalcSize (label).x);
					string text;
					if (roomStatDef.displayRounded) {
						text = Mathf.RoundToInt (stat).ToString ();
					}
					else {
						text = stat.ToString ("0.##");
					}
					RoomStatsDrawer.scoreColumnWidth = Mathf.Max (RoomStatsDrawer.scoreColumnWidth, Text.CalcSize (text).x);
				}
			}
			RoomStatsDrawer.scoreColumnWidth = Mathf.Max (RoomStatsDrawer.scoreColumnWidth, 40);
		}

		public static void DrawRoomOverlays ()
		{
			if (!RoomStatsDrawer.ShouldShowRoomStats) {
				return;
			}
			Room room = GridsUtility.GetRoom (Gen.MouseCell ());
			room.DrawFieldEdges ();
		}

		private static string GetRoomRoleLabel (Room room)
		{
			Pawn owner = room.get_Owner ();
			string result;
			if (owner != null) {
				result = Translator.Translate ("SomeonesSomething", new object[] {
					owner.get_NameStringShort (),
					room.get_Role ().label
				});
			}
			else {
				result = room.get_Role ().get_LabelCap ();
			}
			return result;
		}

		public static void RoomStatsOnGUI ()
		{
			RoomStatsDrawer.Anonymous temp = new RoomStatsDrawer.Anonymous ();
			if (!RoomStatsDrawer.ShouldShowRoomStats) {
				return;
			}
			temp.room = GridsUtility.GetRoom (Gen.MouseCell ());
			Text.set_Font (1);
			RoomStatsDrawer.CalculateColumnsSizes (temp.room);
			temp.windowRect = new Rect (Event.get_current ().get_mousePosition ().x, Event.get_current ().get_mousePosition ().y, 108 + RoomStatsDrawer.statLabelColumnWidth + RoomStatsDrawer.scoreColumnWidth + RoomStatsDrawer.scoreStageLabelColumnWidth, (float)(65 + RoomStatsDrawer.DisplayedRoomStatsCount * 25));
			RoomStatsDrawer.Anonymous temp5 = temp;
			temp5.windowRect.set_x (temp5.windowRect.get_x () + 5);
			RoomStatsDrawer.Anonymous temp2 = temp;
			temp2.windowRect.set_y (temp2.windowRect.get_y () + 5);
			if (temp.windowRect.get_xMax () > (float)Screen.get_width ()) {
				RoomStatsDrawer.Anonymous temp3 = temp;
				temp3.windowRect.set_x (temp3.windowRect.get_x () - (temp.windowRect.get_width () + 10));
			}
			if (temp.windowRect.get_yMax () > (float)Screen.get_height ()) {
				RoomStatsDrawer.Anonymous temp4 = temp;
				temp4.windowRect.set_y (temp4.windowRect.get_y () - (temp.windowRect.get_height () + 10));
			}
			Find.get_WindowStack ().ImmediateWindow (74975, temp.windowRect, 2, delegate {
				ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.InspectRoomStats, 1);
				Text.set_Font (1);
				float num = 19;
				Rect rect = new Rect (19, num, temp.windowRect.get_width () - 38, 100);
				GUI.set_color (Color.get_white ());
				Widgets.Label (rect, RoomStatsDrawer.GetRoomRoleLabel (temp.room));
				num += 25;
				for (int i = 0; i < DefDatabase<RoomStatDef>.get_AllDefsListForReading ().Count; i++) {
					RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.get_AllDefsListForReading () [i];
					if (!roomStatDef.isHidden) {
						float stat = temp.room.GetStat (roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage (stat);
						if (temp.room.get_Role ().IsStatRelated (roomStatDef)) {
							GUI.set_color (RoomStatsDrawer.RelatedStatColor);
						}
						else {
							GUI.set_color (Color.get_gray ());
						}
						Rect rect2 = new Rect (rect.get_x (), num, RoomStatsDrawer.statLabelColumnWidth, 23);
						Widgets.Label (rect2, roomStatDef.get_LabelCap ());
						Rect rect3 = new Rect (rect2.get_xMax () + 35, num, RoomStatsDrawer.scoreColumnWidth, 23);
						string text;
						if (roomStatDef.displayRounded) {
							text = Mathf.RoundToInt (stat).ToString ();
						}
						else {
							text = stat.ToString ("0.##");
						}
						Widgets.Label (rect3, text);
						Rect rect4 = new Rect (rect3.get_xMax () + 35, num, RoomStatsDrawer.scoreStageLabelColumnWidth, 23);
						Widgets.Label (rect4, scoreStage.label);
						num += 25;
					}
				}
				GUI.set_color (Color.get_white ());
			}, true, false, 1);
		}

		//
		// Nested Types
		//
		private sealed class Anonymous
		{
			internal Rect windowRect;

			internal Room room;

			internal void Iterate ()
			{
				ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.InspectRoomStats, 1);
				Text.set_Font (1);
				float num = 19;
				Rect rect = new Rect (19, num, this.windowRect.get_width () - 38, 100);
				GUI.set_color (Color.get_white ());
				Widgets.Label (rect, RoomStatsDrawer.GetRoomRoleLabel (this.room));
				num += 25;
				for (int i = 0; i < DefDatabase<RoomStatDef>.get_AllDefsListForReading ().Count; i++) {
					RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.get_AllDefsListForReading () [i];
					if (!roomStatDef.isHidden) {
						float stat = this.room.GetStat (roomStatDef);
						RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage (stat);
						if (this.room.get_Role ().IsStatRelated (roomStatDef)) {
							GUI.set_color (RoomStatsDrawer.RelatedStatColor);
						}
						else {
							GUI.set_color (Color.get_gray ());
						}
						Rect rect2 = new Rect (rect.get_x (), num, RoomStatsDrawer.statLabelColumnWidth, 23);
						Widgets.Label (rect2, roomStatDef.get_LabelCap ());
						Rect rect3 = new Rect (rect2.get_xMax () + 35, num, RoomStatsDrawer.scoreColumnWidth, 23);
						string text;
						if (roomStatDef.displayRounded) {
							text = Mathf.RoundToInt (stat).ToString ();
						}
						else {
							text = stat.ToString ("0.##");
						}
						Widgets.Label (rect3, text);
						Rect rect4 = new Rect (rect3.get_xMax () + 35, num, RoomStatsDrawer.scoreStageLabelColumnWidth, 23);
						Widgets.Label (rect4, scoreStage.label);
						num += 25;
					}
				}
				GUI.set_color (Color.get_white ());
			}
		}
	}
}
using System;
using System.Threading;
using UnityEngine;

namespace EdB.Interface
{
	public class ScreenSizeMonitor
	{
		public delegate void ScreenSizeChangeHandler (int width, int height);

		//
		// Fields
		//
		protected int width;

		protected int height;

		//
		// Properties
		//
		public int Height {
			get {
				return this.height;
			}
		}

		public int Width {
			get {
				return this.width;
			}
		}

		//
		// Constructors
		//
		public ScreenSizeMonitor ()
		{
			this.width = Screen.get_width ();
			this.height = Screen.get_height ();
		}

		//
		// Methods
		//
		public void Update ()
		{
			int num = Screen.get_width ();
			int num2 = Screen.get_height ();
			if (num != this.width || num2 != this.height) {
				this.width = num;
				this.height = num2;
				if (this.Changed != null) {
					this.Changed (num, num2);
				}
			}
		}

		//
		// Events
		//
		public event ScreenSizeMonitor.ScreenSizeChangeHandler Changed {
			add {
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler = this.Changed;
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler2;
				do {
					screenSizeChangeHandler2 = screenSizeChangeHandler;
					screenSizeChangeHandler = Interlocked.CompareExchange<ScreenSizeMonitor.ScreenSizeChangeHandler> (ref this.Changed, (ScreenSizeMonitor.ScreenSizeChangeHandler)Delegate.Combine (screenSizeChangeHandler2, value), screenSizeChangeHandler);
				}
				while (screenSizeChangeHandler != screenSizeChangeHandler2);
			}
			remove {
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler = this.Changed;
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler2;
				do {
					screenSizeChangeHandler2 = screenSizeChangeHandler;
					screenSizeChangeHandler = Interlocked.CompareExchange<ScreenSizeMonitor.ScreenSizeChangeHandler> (ref this.Changed, (ScreenSizeMonitor.ScreenSizeChangeHandler)Delegate.Remove (screenSizeChangeHandler2, value), screenSizeChangeHandler);
				}
				while (screenSizeChangeHandler != screenSizeChangeHandler2);
			}
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ScrollView
	{
		//
		// Fields
		//
		private float contentHeight;

		private Vector2 position = Vector2.get_zero ();

		private Rect viewRect;

		private Rect contentRect;

		private bool consumeScrollEvents = true;

		//
		// Properties
		//
		public float ContentHeight {
			get {
				return this.contentHeight;
			}
		}

		public float ContentWidth {
			get {
				return this.contentRect.get_width ();
			}
		}

		public Vector2 Position {
			get {
				return this.position;
			}
		}

		public float ViewHeight {
			get {
				return this.viewRect.get_height ();
			}
		}

		public float ViewWidth {
			get {
				return this.viewRect.get_width ();
			}
		}

		//
		// Constructors
		//
		public ScrollView (bool consumeScrollEvents)
		{
			this.consumeScrollEvents = consumeScrollEvents;
		}

		public ScrollView ()
		{
		}

		//
		// Static Methods
		//
		protected static void BeginScrollView (Rect outRect, ref Vector2 scrollPosition, Rect viewRect)
		{
			Vector2 vector = scrollPosition;
			Vector2 vector2 = GUI.BeginScrollView (outRect, scrollPosition, viewRect);
			Vector2 vector3;
			if (Event.get_current ().get_type () == null) {
				vector3 = vector;
			}
			else {
				vector3 = vector2;
			}
			if (Event.get_current ().get_type () == 6 && Mouse.IsOver (outRect)) {
				vector3 += Event.get_current ().get_delta () * 40;
			}
			scrollPosition = vector3;
		}

		//
		// Methods
		//
		public void Begin (Rect viewRect)
		{
			this.viewRect = viewRect;
			this.contentRect = new Rect (0, 0, viewRect.get_width () - 16, this.contentHeight);
			if (this.consumeScrollEvents) {
				Widgets.BeginScrollView (viewRect, ref this.position, this.contentRect);
			}
			else {
				ScrollView.BeginScrollView (viewRect, ref this.position, this.contentRect);
			}
		}

		public void End (float yPosition)
		{
			if (Event.get_current ().get_type () == 8) {
				this.contentHeight = yPosition;
			}
			Widgets.EndScrollView ();
		}
	}
}
using RimWorld;
using System;

namespace EdB.Interface
{
	public class SelectionOverlaysComponent : IUpdatedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "SelectionOverlays";
			}
		}

		//
		// Methods
		//
		public void Update ()
		{
			SelectionDrawer.DrawSelectionOverlays ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace EdB.Interface
{
	public class SelectorUtility
	{
		//
		// Fields
		//
		protected FieldInfo hostilePawnsField;

		protected FieldInfo allPawnsField;

		protected List<Pawn> emptyList = new List<Pawn> ();

		protected List<Pawn> visitorPawns = new List<Pawn> (20);

		//
		// Properties
		//
		public IEnumerable<Pawn> ColonyAnimals {
			get {
				return from pawn in Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ())
				where !pawn.get_IsColonist ()
				select pawn;
			}
		}

		public int HostilePawnCount {
			get {
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue (Find.get_ListerPawns ());
				if (dictionary == null) {
					return 0;
				}
				return dictionary [Faction.get_OfColony ()].Count;
			}
		}

		public IEnumerable<Pawn> HostilePawns {
			get {
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue (Find.get_ListerPawns ());
				if (dictionary == null) {
					return this.emptyList;
				}
				return from p in dictionary [Faction.get_OfColony ()]
				where !p.get_InContainer ()
				select p;
			}
		}

		public bool MoreThanOneColonyAnimal {
			get {
				int num = 0;
				foreach (Pawn current in from pawn in Find.get_ListerPawns ().PawnsInFaction (Faction.get_OfColony ())
				where !pawn.get_IsColonist ()
				select pawn) {
					if (++num > 1) {
						return true;
					}
				}
				return false;
			}
		}

		public bool MoreThanOneHostilePawn {
			get {
				Dictionary<Faction, List<Pawn>> dictionary = (Dictionary<Faction, List<Pawn>>)this.hostilePawnsField.GetValue (Find.get_ListerPawns ());
				if (dictionary == null) {
					return false;
				}
				int num = 0;
				foreach (Pawn current in from p in dictionary [Faction.get_OfColony ()]
				where !p.get_InContainer ()
				select p) {
					if (++num > 1) {
						return true;
					}
				}
				return false;
			}
		}

		public bool MoreThanOneVisitorPawn {
			get {
				List<Pawn> list = (List<Pawn>)this.allPawnsField.GetValue (Find.get_ListerPawns ());
				if (list == null || list.Count < 2) {
					return false;
				}
				int num = 0;
				foreach (Pawn current in from p in list
				where p.get_Faction () != null && p.get_Faction () != Faction.get_OfColony () && !p.get_IsPrisonerOfColony () && !p.get_Faction ().RelationWith (Faction.get_OfColony ()).hostile && !p.get_InContainer ()
				select p) {
					if (++num > 1) {
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<Pawn> VisitorPawns {
			get {
				List<Pawn> list = (List<Pawn>)this.allPawnsField.GetValue (Find.get_ListerPawns ());
				if (list == null) {
					return this.emptyList;
				}
				return from p in list
				where p.get_Faction () != null && p.get_Faction () != Faction.get_OfColony () && !p.get_IsPrisonerOfColony () && !p.get_Faction ().RelationWith (Faction.get_OfColony ()).hostile && !p.get_InContainer ()
				select p;
			}
		}

		//
		// Constructors
		//
		public SelectorUtility ()
		{
			this.hostilePawnsField = typeof(ListerPawns).GetField ("pawnsHostileToFaction", BindingFlags.Instance | BindingFlags.NonPublic);
			this.allPawnsField = typeof(ListerPawns).GetField ("allPawns", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Methods
		//
		public void AddToSelection (object o)
		{
			Find.get_Selector ().Select (o, false, true);
		}

		public void ClearSelection ()
		{
			Find.get_Selector ().ClearSelection ();
		}

		public void SelectAllColonists ()
		{
			Selector selector = Find.get_Selector ();
			selector.ClearSelection ();
			foreach (Pawn current in Find.get_ListerPawns ().get_FreeColonists ()) {
				Find.get_Selector ().Select (current, false, true);
			}
			Find.get_MainTabsRoot ().SetCurrentTab (MainTabDefOf.Inspect, true);
		}

		public void SelectNextColonist ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () != Faction.get_OfColony ()) {
				this.SelectThing (Find.get_ListerPawns ().get_FreeColonists ().FirstOrDefault<Pawn> (), false);
			}
			else {
				bool flag = false;
				foreach (Pawn current in Find.get_ListerPawns ().get_FreeColonists ()) {
					if (flag) {
						this.SelectThing (current, false);
						return;
					}
					if (current == selector.get_SingleSelectedThing ()) {
						flag = true;
					}
				}
				this.SelectThing (Find.get_ListerPawns ().get_FreeColonists ().FirstOrDefault<Pawn> (), false);
			}
		}

		public void SelectNextColonyAnimal ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () != Faction.get_OfColony () || (selector.get_SingleSelectedThing () as Pawn).get_IsColonist ()) {
				this.SelectThing (this.ColonyAnimals.FirstOrDefault<Pawn> (), false);
			}
			else {
				bool flag = false;
				foreach (Pawn current in this.ColonyAnimals) {
					if (flag) {
						this.SelectThing (current, false);
						return;
					}
					if (current == selector.get_SingleSelectedThing ()) {
						flag = true;
					}
				}
				this.SelectThing (this.ColonyAnimals.FirstOrDefault<Pawn> (), false);
			}
		}

		public void SelectNextEnemy ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () == Faction.get_OfColony ()) {
				Pawn thing = this.HostilePawns.FirstOrDefault<Pawn> ();
				this.SelectThing (thing, false);
			}
			else {
				bool flag = false;
				foreach (Pawn current in this.HostilePawns) {
					if (flag) {
						this.SelectThing (current, false);
						return;
					}
					if (current == selector.get_SingleSelectedThing ()) {
						flag = true;
					}
				}
				Pawn thing2 = this.HostilePawns.FirstOrDefault<Pawn> ();
				this.SelectThing (thing2, false);
			}
		}

		public void SelectNextPrisoner ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn)) {
				this.SelectThing (Find.get_ListerPawns ().get_PrisonersOfColony ().FirstOrDefault<Pawn> (), false);
			}
			else {
				bool flag = false;
				foreach (Pawn current in Find.get_ListerPawns ().get_PrisonersOfColony ()) {
					if (flag) {
						this.SelectThing (current, false);
						return;
					}
					if (current == selector.get_SingleSelectedThing ()) {
						flag = true;
					}
				}
				this.SelectThing (Find.get_ListerPawns ().get_PrisonersOfColony ().FirstOrDefault<Pawn> (), false);
			}
		}

		public void SelectNextVisitor ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () == Faction.get_OfColony ()) {
				this.SelectThing (this.VisitorPawns.FirstOrDefault<Pawn> (), false);
			}
			else {
				bool flag = false;
				foreach (Pawn current in this.VisitorPawns) {
					if (flag) {
						this.SelectThing (current, false);
						return;
					}
					if (current == selector.get_SingleSelectedThing ()) {
						flag = true;
					}
				}
				this.SelectThing (this.VisitorPawns.FirstOrDefault<Pawn> (), false);
			}
		}

		public void SelectPreviousColonist ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () != Faction.get_OfColony ()) {
				this.SelectThing (Find.get_ListerPawns ().get_FreeColonists ().LastOrDefault<Pawn> (), false);
			}
			else {
				Pawn pawn = null;
				foreach (Pawn current in Find.get_ListerPawns ().get_FreeColonists ()) {
					if (selector.get_SingleSelectedThing () == current) {
						if (pawn != null) {
							this.SelectThing (pawn, false);
							break;
						}
						this.SelectThing (Find.get_ListerPawns ().get_FreeColonists ().LastOrDefault<Pawn> (), false);
						break;
					}
					else {
						pawn = current;
					}
				}
			}
		}

		public void SelectPreviousColonyAnimal ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () != Faction.get_OfColony () || (selector.get_SingleSelectedThing () as Pawn).get_IsColonist ()) {
				this.SelectThing (this.ColonyAnimals.LastOrDefault<Pawn> (), false);
			}
			else {
				Pawn pawn = null;
				foreach (Pawn current in this.ColonyAnimals) {
					if (selector.get_SingleSelectedThing () == current) {
						if (pawn != null) {
							this.SelectThing (pawn, false);
							break;
						}
						this.SelectThing (this.ColonyAnimals.LastOrDefault<Pawn> (), false);
						break;
					}
					else {
						pawn = current;
					}
				}
			}
		}

		public void SelectPreviousEnemy ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () == Faction.get_OfColony ()) {
				this.SelectThing (this.HostilePawns.LastOrDefault<Pawn> (), false);
			}
			else {
				Pawn pawn = null;
				foreach (Pawn current in this.HostilePawns) {
					if (selector.get_SingleSelectedThing () == current) {
						if (pawn != null) {
							this.SelectThing (pawn, false);
							break;
						}
						this.SelectThing (this.HostilePawns.LastOrDefault<Pawn> (), false);
						break;
					}
					else {
						pawn = current;
					}
				}
			}
		}

		public void SelectPreviousPrisoner ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn)) {
				this.SelectThing (Find.get_ListerPawns ().get_PrisonersOfColony ().LastOrDefault<Pawn> (), false);
			}
			else {
				Pawn pawn = null;
				foreach (Pawn current in Find.get_ListerPawns ().get_PrisonersOfColony ()) {
					if (selector.get_SingleSelectedThing () == current) {
						if (pawn != null) {
							this.SelectThing (pawn, false);
							break;
						}
						this.SelectThing (Find.get_ListerPawns ().get_PrisonersOfColony ().LastOrDefault<Pawn> (), false);
						break;
					}
					else {
						pawn = current;
					}
				}
			}
		}

		public void SelectPreviousVisitor ()
		{
			Selector selector = Find.get_Selector ();
			if (selector.get_SingleSelectedThing () == null || !(selector.get_SingleSelectedThing () is Pawn) || selector.get_SingleSelectedThing ().get_Faction () == Faction.get_OfColony ()) {
				this.SelectThing (this.VisitorPawns.LastOrDefault<Pawn> (), false);
			}
			else {
				Pawn pawn = null;
				foreach (Pawn current in this.VisitorPawns) {
					if (selector.get_SingleSelectedThing () == current) {
						if (pawn != null) {
							this.SelectThing (pawn, false);
							break;
						}
						this.SelectThing (this.VisitorPawns.LastOrDefault<Pawn> (), false);
						break;
					}
					else {
						pawn = current;
					}
				}
			}
		}

		public void SelectThing (Thing thing, bool addToSelection = false)
		{
			if (thing == null) {
				return;
			}
			if (!addToSelection) {
				Find.get_Selector ().ClearSelection ();
			}
			Find.get_Selector ().Select (thing, true, true);
			Find.get_MainTabsRoot ().SetCurrentTab (MainTabDefOf.Inspect, true);
		}
	}
}
using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class SkillDrawer
	{
		//
		// Static Fields
		//
		private const float IncButSpacing = 10;

		private const float SkillWidth = 380;

		private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get ("UI/Icons/PassionMajor", true);

		private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get ("UI/Icons/PassionMinor", true);

		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture (new Color (1, 1, 1, 0.1));

		private static Color DisabledSkillColor = new Color (1, 1, 1, 0.5);

		private static float levelLabelWidth = -1;

		public const float LeftEdgeMargin = 6;

		public const float ProgBarHeight = 8;

		public const float SkillYSpacing = 3;

		public const float SkillRowHeight = 24;

		public const float SkillBarHeight = 22;

		public const float IncButX = 205;

		public const float ProgBarX = 180;

		public const float LevelNumberX = 140;

		//
		// Static Methods
		//
		private static string GetSkillDescription (SkillRecord sk)
		{
			StringBuilder stringBuilder = new StringBuilder ();
			if (sk.get_TotallyDisabled ()) {
				stringBuilder.Append (GenText.CapitalizeFirst (Translator.Translate ("DisabledLower")));
			}
			else {
				stringBuilder.AppendLine (string.Concat (new object[] {
					Translator.Translate ("Level"),
					" ",
					sk.level,
					": ",
					sk.get_LevelDescriptor ()
				}));
				if (Game.get_Mode () == 2) {
					string text = (sk.level == 20) ? Translator.Translate ("Experience") : Translator.Translate ("ProgressToNextLevel");
					stringBuilder.AppendLine (string.Concat (new object[] {
						text,
						": ",
						sk.xpSinceLastLevel.ToString ("########0"),
						" / ",
						sk.get_XpRequiredForLevelUp ()
					}));
				}
				stringBuilder.Append (Translator.Translate ("Passion") + ": ");
				switch (sk.passion) {
				case 0:
					stringBuilder.Append (Translator.Translate ("PassionNone", new object[] {
						"0.3"
					}));
					break;
				case 1:
					stringBuilder.Append (Translator.Translate ("PassionMinor", new object[] {
						"1.0"
					}));
					break;
				case 2:
					stringBuilder.Append (Translator.Translate ("PassionMajor", new object[] {
						"1.5"
					}));
					break;
				}
			}
			stringBuilder.AppendLine ();
			stringBuilder.AppendLine ();
			stringBuilder.Append (sk.def.description);
			return stringBuilder.ToString ();
		}

		//
		// Methods
		//
		private void DrawSkill (SkillRecord skill, Vector2 topLeft)
		{
			Rect rect = new Rect (topLeft.x, topLeft.y, 380, 20);
			if (rect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.DrawTexture (rect, TexUI.HighlightTex);
			}
			try {
				GUI.BeginGroup (rect);
				Text.set_Anchor (0);
				Rect rect2 = new Rect (6, -3, SkillDrawer.levelLabelWidth + 6, rect.get_height () + 5);
				rect2.set_yMin (rect2.get_yMin () + 3);
				GUI.set_color (TabDrawer.TextColor);
				Widgets.Label (rect2, skill.def.skillLabel);
				Rect rect3 = new Rect (rect2.get_xMax (), 0, 24, 24);
				if (skill.passion > 0) {
					Texture2D texture2D = (skill.passion == 2) ? SkillDrawer.PassionMajorIcon : SkillDrawer.PassionMinorIcon;
					GUI.DrawTexture (rect3, texture2D);
				}
				if (!skill.get_TotallyDisabled ()) {
					Rect rect4 = new Rect (rect3.get_xMax (), 0, rect.get_width () - rect3.get_xMax (), rect.get_height ());
					Widgets.FillableBar (rect4, (float)skill.level / 20, SkillDrawer.SkillBarFillTex, null, false);
				}
				Rect rect5 = new Rect (rect3.get_xMax () + 4, -2, 999, rect2.get_height ());
				rect5.set_yMin (rect5.get_yMin () + 3);
				string text;
				if (skill.get_TotallyDisabled ()) {
					GUI.set_color (SkillDrawer.DisabledSkillColor);
					text = "-";
				}
				else {
					text = GenString.ToStringCached (skill.level);
				}
				Text.set_Anchor (3);
				Widgets.Label (rect5, text);
			}
			finally {
				GUI.EndGroup ();
				Text.set_Anchor (0);
				GUI.set_color (Color.get_white ());
			}
			TooltipHandler.TipRegion (rect, new TipSignal (SkillDrawer.GetSkillDescription (skill), skill.def.GetHashCode () * 397945));
		}

		public void DrawSkillsOf (Pawn p, Vector2 Offset)
		{
			Text.set_Font (1);
			foreach (SkillDef current in DefDatabase<SkillDef>.get_AllDefs ()) {
				float x = Text.CalcSize (current.skillLabel).x;
				if (x > SkillDrawer.levelLabelWidth) {
					SkillDrawer.levelLabelWidth = x;
				}
			}
			int num = 0;
			foreach (SkillRecord current2 in p.skills.skills) {
				float num2 = (float)num * 24 + Offset.y;
				this.DrawSkill (current2, new Vector2 (Offset.x, num2));
				num++;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class Squad : IExposable
	{
		//
		// Static Fields
		//
		private static int Count;

		//
		// Fields
		//
		protected string id = Squad.GenerateId ();

		protected bool showInOverviewTabs = true;

		protected bool showInColonistBar = true;

		protected string name;

		protected List<Pawn> pawns = new List<Pawn> ();

		//
		// Properties
		//
		public string Id {
			get {
				return this.id;
			}
			set {
				this.id = value;
			}
		}

		public virtual string Name {
			get {
				return this.name;
			}
			set {
				this.name = value;
			}
		}

		public int OrderHash {
			get {
				int num = 33;
				foreach (Pawn current in this.pawns) {
					num = 17 * num + current.GetUniqueLoadID ().GetHashCode ();
				}
				num = 17 * num + this.name.GetHashCode ();
				return num;
			}
		}

		public List<Pawn> Pawns {
			get {
				return this.pawns;
			}
			set {
				this.pawns = value;
			}
		}

		public bool ShowInColonistBar {
			get {
				return this.showInColonistBar;
			}
			set {
				this.showInColonistBar = value;
			}
		}

		public bool ShowInOverviewTabs {
			get {
				return this.showInOverviewTabs;
			}
			set {
				this.showInOverviewTabs = value;
			}
		}

		//
		// Static Methods
		//
		public static string GenerateId ()
		{
			return "Squad" + DateTime.Now.Ticks + ++Squad.Count;
		}

		//
		// Methods
		//
		public virtual void Add (Pawn pawn)
		{
			if (!this.pawns.Contains (pawn)) {
				this.pawns.Add (pawn);
			}
		}

		public void Clear ()
		{
			this.pawns.Clear ();
		}

		public void ExposeData ()
		{
			Scribe_Values.LookValue<string> (ref this.id, "id", null, true);
			Scribe_Values.LookValue<string> (ref this.name, "name", string.Empty, true);
			Scribe_Values.LookValue<bool> (ref this.showInColonistBar, "showInColonistBar", true, true);
			Scribe_Values.LookValue<bool> (ref this.showInOverviewTabs, "showInOverviewTabs", true, true);
			Scribe_Collections.LookList<Pawn> (ref this.pawns, "pawns", 3, null);
		}

		public virtual bool Remove (Pawn pawn)
		{
			return this.pawns.Remove (pawn);
		}

		public void Replace (Pawn pawn, Pawn replacement)
		{
			int num = this.pawns.IndexOf (pawn);
			if (num > -1) {
				this.pawns [num] = replacement;
			}
		}
	}
}
using System;

namespace EdB.Interface
{
	public delegate void SquadAddedHandler (Squad squad);
}
using System;

namespace EdB.Interface
{
	public delegate void SquadChangedHandler (Squad squad);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verse;

namespace EdB.Interface
{
	public class SquadManager
	{
		//
		// Static Fields
		//
		public static bool LoggingEnabled;

		public static readonly int MaxFavorites = 10;

		protected static SquadManager instance;

		//
		// Fields
		//
		protected AllColonistsSquad allColonistsSquad;

		protected Squad squadFilter;

		protected Squad currentSquad;

		protected List<Squad> favorites = new List<Squad> ();

		protected List<Squad> squads = new List<Squad> ();

		//
		// Static Properties
		//
		public static SquadManager Instance {
			get {
				if (SquadManager.instance == null) {
					SquadManager.instance = new SquadManager ();
				}
				return SquadManager.instance;
			}
		}

		//
		// Properties
		//
		public bool AllColonistsOrderMatches {
			get {
				int orderHash = this.GetOrderHash (this.AllColonistsSquad.Pawns);
				int orderHash2 = this.GetOrderHash (Find.get_ListerPawns ().get_FreeColonists ());
				return orderHash == orderHash2;
			}
		}

		public AllColonistsSquad AllColonistsSquad {
			get {
				return this.allColonistsSquad;
			}
		}

		public Squad CurrentSquad {
			get {
				return this.currentSquad;
			}
			set {
				this.currentSquad = value;
			}
		}

		public List<Squad> Favorites {
			get {
				return this.favorites;
			}
		}

		public PreferenceEnableSquads PreferenceEnableSquads {
			get;
			set;
		}

		public int SquadCount {
			get {
				return this.squads.Count;
			}
		}

		public Squad SquadFilter {
			get {
				if (this.squadFilter == null || this.squadFilter.Pawns.Count == 0) {
					return this.allColonistsSquad;
				}
				return this.squadFilter;
			}
			set {
				this.squadFilter = value;
			}
		}

		public List<Squad> Squads {
			get {
				return this.squads;
			}
		}

		//
		// Constructors
		//
		protected SquadManager ()
		{
			this.Message ("SquadManager()");
			this.Reset ();
		}

		//
		// Methods
		//
		public void AddSquad (Squad squad)
		{
			this.Message ("SquadManager.AddSquad()");
			this.squads.Add (squad);
			if (this.SquadAdded != null) {
				this.SquadAdded (squad);
			}
			this.SyncThingToMap ();
		}

		public void ColonistChanged (ColonistNotification notification)
		{
			this.Message ("SquadManager.ColonistChanged()");
			if (notification.type == ColonistNotificationType.New) {
				this.allColonistsSquad.Add (notification.colonist.Pawn);
				if (this.SquadChanged != null) {
					this.SquadChanged (this.allColonistsSquad);
				}
			}
			else if (notification.type == ColonistNotificationType.Buried) {
				this.RemovePawnFromAllSquads (notification.colonist.Pawn);
			}
			else if (notification.type == ColonistNotificationType.Lost) {
				this.RemovePawnFromAllSquads (notification.colonist.Pawn);
			}
			else if (notification.type == ColonistNotificationType.Deleted) {
				this.RemovePawnFromAllSquads (notification.colonist.Pawn);
			}
		}

		public Squad GetFavorite (int index)
		{
			if (index < 0 || index >= this.favorites.Count) {
				return null;
			}
			return this.favorites [index];
		}

		protected int GetOrderHash (IEnumerable<Pawn> colonists)
		{
			int num = 33;
			foreach (Pawn current in colonists) {
				TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist (current);
				if (trackedColonist == null || (!trackedColonist.Dead && !trackedColonist.Missing && !trackedColonist.Captured)) {
					num = 17 * num + current.GetUniqueLoadID ().GetHashCode ();
				}
			}
			return num;
		}

		public void Message (string message)
		{
			if (SquadManager.LoggingEnabled) {
				Log.Message (message);
			}
		}

		public void RemoveFromFavorites (Squad squad)
		{
			for (int i = 0; i < this.favorites.Count; i++) {
				if (this.favorites [i] == squad) {
					this.favorites [i] = null;
				}
			}
		}

		public void RemovePawnFromAllSquads (Pawn pawn)
		{
			foreach (Squad current in this.squads) {
				if (current.Pawns.Remove (pawn)) {
					this.SquadChanged (current);
				}
			}
		}

		public void RemoveSquad (Squad squad)
		{
			this.Message ("SquadManager.RemoveSquad()");
			if (squad == this.allColonistsSquad) {
				return;
			}
			int num = this.squads.IndexOf (squad);
			if (num == -1) {
				return;
			}
			if (this.currentSquad == squad) {
				this.currentSquad = this.allColonistsSquad;
			}
			if (this.squadFilter == squad) {
				this.squadFilter = null;
			}
			this.RemoveFromFavorites (squad);
			if (this.squads.Remove (squad) && this.SquadRemoved != null) {
				this.SquadRemoved (squad, num);
			}
			this.SyncThingToMap ();
		}

		public void RenameSquad (Squad squad, string name)
		{
			squad.Name = name;
			if (this.SquadChanged != null) {
				this.SquadChanged (squad);
			}
		}

		public void ReorderSquadList (List<Squad> reorderedSquads)
		{
			this.Message ("ReorderSquadList()");
			if (this.squads.Except (reorderedSquads).Count<Squad> () == 0) {
				this.squads.Clear ();
				this.squads.AddRange (reorderedSquads);
				if (this.SquadOrderChanged != null) {
					this.SquadOrderChanged ();
				}
			}
		}

		public void ReplaceSquadPawns (Squad squad, IEnumerable<Pawn> pawns)
		{
			this.Message ("ReplaceSquadPawns");
			if (this.squads.Contains (squad)) {
				squad.Pawns.Clear ();
				squad.Pawns.AddRange (pawns);
				this.Message ("Pawn count = " + squad.Pawns.Count);
				if (this.SquadChanged != null) {
					this.SquadChanged (squad);
				}
			}
			else {
				this.Message ("Squad manager does not contain the specified squad");
			}
			this.SyncThingToMap ();
		}

		public void Reset ()
		{
			this.Message ("SquadManager.Reset()");
			this.allColonistsSquad = new AllColonistsSquad ();
			this.squads.Clear ();
			this.squads.Add (this.allColonistsSquad);
			this.currentSquad = null;
			this.favorites.Clear ();
			for (int i = 0; i < SquadManager.MaxFavorites; i++) {
				this.favorites.Add (null);
			}
		}

		public bool SetFavorite (int index, Squad squad)
		{
			if (index < 0 || index >= this.favorites.Count) {
				return false;
			}
			this.Message (string.Concat (new object[] {
				"Set favorite ",
				index,
				" to ",
				squad.Name
			}));
			this.favorites [index] = squad;
			return true;
		}

		public void ShowSquadInColonistBar (Squad squad, bool value)
		{
			if (squad.ShowInColonistBar != value) {
				squad.ShowInColonistBar = value;
				if (this.SquadDisplayPreferenceChanged != null) {
					this.SquadDisplayPreferenceChanged (squad);
				}
			}
		}

		public void SyncThingToMap ()
		{
			this.Message ("SyncThingToMap()");
			bool flag = false;
			if (this.PreferenceEnableSquads != null && this.PreferenceEnableSquads.Value) {
				if (this.squads.Count > 1) {
					flag = true;
				}
				else if (!this.AllColonistsOrderMatches) {
					flag = true;
				}
			}
			if (flag) {
				if (SquadManagerThing.Instance.AddToMap ()) {
					this.Message ("Added SquadManagerThing to the map");
				}
			}
			else if (SquadManagerThing.Instance.RemoveFromMap ()) {
				this.Message ("Removed SquadManagerThing from the map");
			}
		}

		public bool SyncWithMap ()
		{
			this.Message ("SquadManager.SyncWithMap()");
			SquadManagerThing squadManagerThing = SquadManagerThing.Instance;
			if (SquadManagerThing.Instance.InMap) {
				this.squads.Clear ();
				this.squads.AddRange (squadManagerThing.Squads);
				this.favorites.Clear ();
				this.favorites.AddRange (squadManagerThing.Favorites);
				foreach (Squad current in this.squads) {
					AllColonistsSquad allColonistsSquad = current as AllColonistsSquad;
					if (allColonistsSquad != null) {
						this.allColonistsSquad = allColonistsSquad;
					}
				}
				if (this.allColonistsSquad == null) {
					Log.Error ("Could not find default all-colonists squad");
				}
				else {
					ColonistTracker.Instance.StartTrackingPawns (this.AllColonistsSquad.Pawns);
				}
				this.CurrentSquad = squadManagerThing.CurrentSquad;
				this.SquadFilter = squadManagerThing.SquadFilter;
				return true;
			}
			return false;
		}

		public void Warning (string message)
		{
			if (SquadManager.LoggingEnabled) {
				Log.Warning (message);
			}
		}

		//
		// Events
		//
		public event SquadNotificationHandler SquadAdded {
			add {
				SquadNotificationHandler squadNotificationHandler = this.SquadAdded;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadAdded, (SquadNotificationHandler)Delegate.Combine (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove {
				SquadNotificationHandler squadNotificationHandler = this.SquadAdded;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadAdded, (SquadNotificationHandler)Delegate.Remove (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadNotificationHandler SquadChanged {
			add {
				SquadNotificationHandler squadNotificationHandler = this.SquadChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadChanged, (SquadNotificationHandler)Delegate.Combine (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove {
				SquadNotificationHandler squadNotificationHandler = this.SquadChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadChanged, (SquadNotificationHandler)Delegate.Remove (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadNotificationHandler SquadDisplayPreferenceChanged {
			add {
				SquadNotificationHandler squadNotificationHandler = this.SquadDisplayPreferenceChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadDisplayPreferenceChanged, (SquadNotificationHandler)Delegate.Combine (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
			remove {
				SquadNotificationHandler squadNotificationHandler = this.SquadDisplayPreferenceChanged;
				SquadNotificationHandler squadNotificationHandler2;
				do {
					squadNotificationHandler2 = squadNotificationHandler;
					squadNotificationHandler = Interlocked.CompareExchange<SquadNotificationHandler> (ref this.SquadDisplayPreferenceChanged, (SquadNotificationHandler)Delegate.Remove (squadNotificationHandler2, value), squadNotificationHandler);
				}
				while (squadNotificationHandler != squadNotificationHandler2);
			}
		}

		public event SquadOrderChangedHandler SquadOrderChanged {
			add {
				SquadOrderChangedHandler squadOrderChangedHandler = this.SquadOrderChanged;
				SquadOrderChangedHandler squadOrderChangedHandler2;
				do {
					squadOrderChangedHandler2 = squadOrderChangedHandler;
					squadOrderChangedHandler = Interlocked.CompareExchange<SquadOrderChangedHandler> (ref this.SquadOrderChanged, (SquadOrderChangedHandler)Delegate.Combine (squadOrderChangedHandler2, value), squadOrderChangedHandler);
				}
				while (squadOrderChangedHandler != squadOrderChangedHandler2);
			}
			remove {
				SquadOrderChangedHandler squadOrderChangedHandler = this.SquadOrderChanged;
				SquadOrderChangedHandler squadOrderChangedHandler2;
				do {
					squadOrderChangedHandler2 = squadOrderChangedHandler;
					squadOrderChangedHandler = Interlocked.CompareExchange<SquadOrderChangedHandler> (ref this.SquadOrderChanged, (SquadOrderChangedHandler)Delegate.Remove (squadOrderChangedHandler2, value), squadOrderChangedHandler);
				}
				while (squadOrderChangedHandler != squadOrderChangedHandler2);
			}
		}

		public event SquadRemovedHandler SquadRemoved {
			add {
				SquadRemovedHandler squadRemovedHandler = this.SquadRemoved;
				SquadRemovedHandler squadRemovedHandler2;
				do {
					squadRemovedHandler2 = squadRemovedHandler;
					squadRemovedHandler = Interlocked.CompareExchange<SquadRemovedHandler> (ref this.SquadRemoved, (SquadRemovedHandler)Delegate.Combine (squadRemovedHandler2, value), squadRemovedHandler);
				}
				while (squadRemovedHandler != squadRemovedHandler2);
			}
			remove {
				SquadRemovedHandler squadRemovedHandler = this.SquadRemoved;
				SquadRemovedHandler squadRemovedHandler2;
				do {
					squadRemovedHandler2 = squadRemovedHandler;
					squadRemovedHandler = Interlocked.CompareExchange<SquadRemovedHandler> (ref this.SquadRemoved, (SquadRemovedHandler)Delegate.Remove (squadRemovedHandler2, value), squadRemovedHandler);
				}
				while (squadRemovedHandler != squadRemovedHandler2);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public class SquadManagerThing : Thing
	{
		//
		// Static Fields
		//
		public static readonly string DefName = "EdBInterfaceSquadManager";

		protected static SquadManagerThing instance;

		//
		// Fields
		//
		protected bool inMap;

		private string squadFilterId = string.Empty;

		private string currentSquadId = string.Empty;

		private List<string> favoriteIds = new List<string> ();

		protected List<Squad> squads = new List<Squad> ();

		protected List<Squad> favorites = new List<Squad> ();

		protected Squad currentSquad;

		protected Squad squadFilter;

		private List<Pawn> missingPawns = new List<Pawn> ();

		//
		// Static Properties
		//
		public static SquadManagerThing Instance {
			get {
				List<Thing> list = Find.get_Map ().listerThings.ThingsOfDef (ThingDef.Named (SquadManagerThing.DefName));
				if (list != null && list.Count > 0) {
					SquadManagerThing.instance = (list [0] as SquadManagerThing);
					if (SquadManagerThing.instance != null) {
						SquadManagerThing.instance.inMap = true;
					}
				}
				if (SquadManagerThing.instance == null) {
					SquadManagerThing.instance = (ThingMaker.MakeThing (ThingDef.Named (SquadManagerThing.DefName), null) as SquadManagerThing);
					if (SquadManagerThing.instance != null) {
						SquadManagerThing.instance.set_Position (new IntVec3 (0, 0, 0));
					}
					else {
						Log.Error ("Could not create Squad Manager Thing.");
					}
				}
				return SquadManagerThing.instance;
			}
		}

		//
		// Properties
		//
		public Squad CurrentSquad {
			get {
				return this.currentSquad;
			}
		}

		public List<Squad> Favorites {
			get {
				return this.favorites;
			}
		}

		public bool InMap {
			get {
				return this.inMap;
			}
		}

		public override string LabelBase {
			get {
				return string.Empty;
			}
		}

		public Squad SquadFilter {
			get {
				return this.squadFilter;
			}
		}

		public List<Squad> Squads {
			get {
				return this.squads;
			}
		}

		//
		// Static Methods
		//
		public static void Clear ()
		{
			SquadManagerThing.instance = null;
		}

		//
		// Methods
		//
		public bool AddToMap ()
		{
			if (!this.inMap) {
				Find.get_Map ().listerThings.Add (this);
				this.inMap = true;
				return true;
			}
			return false;
		}

		public override void ExposeData ()
		{
			base.ExposeData ();
			if (Scribe.mode == 1) {
				this.squads.Clear ();
				this.squads.AddRange (SquadManager.Instance.Squads);
				this.missingPawns.Clear ();
				HashSet<Pawn> hashSet = new HashSet<Pawn> ();
				foreach (Squad current in this.squads) {
					foreach (Pawn current2 in current.Pawns) {
						TrackedColonist trackedColonist = ColonistTracker.Instance.FindTrackedColonist (current2);
						if (trackedColonist.Missing) {
							hashSet.Add (current2);
						}
					}
				}
				foreach (Pawn current3 in hashSet) {
					this.missingPawns.Add (current3);
				}
				this.favorites.Clear ();
				this.favorites.AddRange (SquadManager.Instance.Favorites);
				this.favoriteIds.Clear ();
				foreach (Squad current4 in this.favorites) {
					if (current4 != null) {
						this.favoriteIds.Add (current4.Id);
					}
					else {
						this.favoriteIds.Add (string.Empty);
					}
				}
				this.currentSquad = SquadManager.Instance.CurrentSquad;
				if (this.currentSquad != null) {
					this.currentSquadId = this.currentSquad.Id;
				}
				else {
					this.currentSquadId = string.Empty;
				}
				this.squadFilter = SquadManager.Instance.SquadFilter;
				if (this.squadFilter != null) {
					this.squadFilterId = this.squadFilter.Id;
				}
				else {
					this.squadFilterId = string.Empty;
				}
			}
			Scribe_Values.LookValue<string> (ref this.currentSquadId, "currentSquad", null, false);
			Scribe_Values.LookValue<string> (ref this.squadFilterId, "squadFilter", null, false);
			Scribe_Collections.LookList<string> (ref this.favoriteIds, "favorites", 1, null);
			Scribe_Collections.LookList<Pawn> (ref this.missingPawns, "missingPawns", 2, null);
			Scribe_Collections.LookList<Squad> (ref this.squads, "squads", 2, null);
			if (Scribe.mode == 4) {
				if (this.favorites == null) {
					this.favorites = new List<Squad> ();
				}
				else {
					this.favorites.Clear ();
				}
				for (int i = 0; i < SquadManager.MaxFavorites; i++) {
					this.favorites.Add (null);
				}
				if (this.favoriteIds == null) {
					this.favoriteIds = new List<string> ();
					for (int j = 0; j < SquadManager.MaxFavorites; j++) {
						this.favoriteIds.Add (string.Empty);
					}
				}
				int num = 0;
				foreach (string current5 in this.favoriteIds) {
					this.favorites [num++] = this.FindSquadById (current5);
				}
				this.currentSquad = this.FindSquadById (this.currentSquadId);
				this.squadFilter = this.FindSquadById (this.squadFilterId);
			}
		}

		protected Squad FindSquadById (string id)
		{
			if (string.IsNullOrEmpty (id)) {
				return null;
			}
			return this.squads.FirstOrDefault ((Squad squad) => squad.Id == id);
		}

		public bool RemoveFromMap ()
		{
			if (this.inMap) {
				Find.get_Map ().listerThings.Remove (this);
				this.inMap = false;
				return true;
			}
			return false;
		}
	}
}
using System;

namespace EdB.Interface
{
	public delegate void SquadNotificationHandler (Squad squad);
}
using System;

namespace EdB.Interface
{
	public delegate void SquadOrderChangedHandler ();
}
using System;
using Verse;

namespace EdB.Interface
{
	public class SquadPriorities
	{
		//
		// Fields
		//
		protected DefMap<WorkTypeDef, int> priorities = new DefMap<WorkTypeDef, int> ();

		//
		// Methods
		//
		public int GetPriority (WorkTypeDef w)
		{
			int num = this.priorities.get_Item (w);
			if (num > 0 && !Find.get_PlaySettings ().useWorkPriorities) {
				return 1;
			}
			return num;
		}

		public void Reset ()
		{
			if (this.priorities == null) {
				this.priorities = new DefMap<WorkTypeDef, int> ();
			}
			if (this.priorities.get_Count () == 0) {
				foreach (WorkTypeDef current in DefDatabase<WorkTypeDef>.get_AllDefs ()) {
					this.SetPriority (current, 0);
				}
			}
			this.priorities.SetAll (0);
		}

		public void SetPriority (WorkTypeDef w, int priority)
		{
			if (priority < 0 || priority > 4) {
				Log.Message ("Trying to set work to invalid priority " + priority);
			}
			this.priorities.set_Item (w, priority);
		}
	}
}
using System;

namespace EdB.Interface
{
	public delegate void SquadRemovedHandler (Squad squad, int index);
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class SquadShortcuts
	{
		//
		// Fields
		//
		protected List<KeyBindingDef> squadSelectionBindings = new List<KeyBindingDef> ();

		protected int keypressCount;

		protected float lastKeypressTimestamp = -1;

		protected int? pressedKey;

		protected KeyBindingDef previousSquadKeyBinding;

		protected KeyBindingDef nextSquadKeyBinding;

		//
		// Properties
		//
		public ColonistBarSquadSupervisor ColonistBarSquadSupervisor {
			get;
			set;
		}

		//
		// Constructors
		//
		public SquadShortcuts ()
		{
			this.squadSelectionBindings.Clear ();
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad1"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad2"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad3"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad4"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad5"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad6"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad7"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad8"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad9"));
			this.squadSelectionBindings.Add (KeyBindingDef.Named ("EdB_Interface_Squad10"));
			this.nextSquadKeyBinding = KeyBindingDef.Named ("EdB_Interface_NextSquad");
			this.previousSquadKeyBinding = KeyBindingDef.Named ("EdB_Interface_PreviousSquad");
		}

		//
		// Methods
		//
		public void Update ()
		{
			for (int i = 0; i < this.squadSelectionBindings.Count; i++) {
				if (this.squadSelectionBindings [i] != null && this.squadSelectionBindings [i].get_JustPressed ()) {
					Event current = Event.get_current ();
					if (current.get_shift () || current.get_control ()) {
						if (this.ColonistBarSquadSupervisor.SaveCurrentSquadAsFavorite (i)) {
							Messages.Message (Translator.Translate ("EdB.Squads.Shortcuts.Assigned", new object[] {
								this.ColonistBarSquadSupervisor.SelectedSquad.Name,
								this.squadSelectionBindings [i].get_MainKeyLabel ()
							}), 1);
						}
					}
					else {
						int? num = this.pressedKey;
						if (num.HasValue && this.pressedKey.Value == i && Time.get_time () - this.lastKeypressTimestamp < 0.3) {
							this.keypressCount++;
							if (this.keypressCount > 2) {
								this.keypressCount = 1;
							}
						}
						else {
							this.keypressCount = 1;
						}
						if (this.keypressCount == 1) {
							this.ColonistBarSquadSupervisor.SelectFavorite (i);
							this.pressedKey = new int? (i);
						}
						else if (this.keypressCount == 2) {
							this.ColonistBarSquadSupervisor.SelectAllPawnsInFavorite (i);
							this.pressedKey = null;
							this.keypressCount = 0;
						}
						this.lastKeypressTimestamp = Time.get_time ();
					}
				}
			}
			if (this.nextSquadKeyBinding != null && this.nextSquadKeyBinding.get_JustPressed ()) {
				this.ColonistBarSquadSupervisor.SelectNextSquad (1);
			}
			if (this.previousSquadKeyBinding != null && this.previousSquadKeyBinding.get_JustPressed ()) {
				this.ColonistBarSquadSupervisor.SelectNextSquad (-1);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class StringOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler (string value);

		//
		// Static Fields
		//
		public static float LabelMargin = StringOptionsPreference.RadioButtonWidth + StringOptionsPreference.RadioButtonMargin;

		public static float RadioButtonMargin = 18;

		public static float RadioButtonWidth = 24;

		//
		// Fields
		//
		private string stringValue;

		private string setValue;

		public int tooltipId;

		//
		// Properties
		//
		public abstract string DefaultValue {
			get;
		}

		public virtual bool Disabled {
			get {
				return false;
			}
		}

		public virtual bool DisplayInOptions {
			get {
				return true;
			}
		}

		public abstract string Group {
			get;
		}

		public virtual bool Indent {
			get {
				return false;
			}
		}

		public virtual string Label {
			get {
				return Translator.Translate (this.Name);
			}
		}

		public abstract string Name {
			get;
		}

		public abstract string OptionValuePrefix {
			get;
		}

		public abstract IEnumerable<string> OptionValues {
			get;
		}

		public virtual string Tooltip {
			get {
				return null;
			}
		}

		protected virtual int TooltipId {
			get {
				if (this.tooltipId == 0) {
					this.tooltipId = Translator.Translate (this.Tooltip).GetHashCode ();
					return this.tooltipId;
				}
				return 0;
			}
		}

		public virtual string Value {
			get {
				if (this.setValue != null) {
					return this.setValue;
				}
				return this.DefaultValue;
			}
			set {
				string text = this.setValue;
				this.setValue = value;
				this.stringValue = value.ToString ();
				if ((text == null || text != value) && this.ValueChanged != null) {
					this.ValueChanged (value);
				}
			}
		}

		public virtual string ValueForDisplay {
			get {
				if (this.setValue != null) {
					return this.setValue;
				}
				return this.DefaultValue;
			}
		}

		public string ValueForSerialization {
			get {
				return this.stringValue;
			}
			set {
				this.stringValue = value;
				this.setValue = value;
			}
		}

		//
		// Constructors
		//
		public StringOptionsPreference ()
		{
		}

		//
		// Methods
		//
		public void OnGUI (float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			if (disabled) {
				GUI.set_color (Dialog_InterfaceOptions.DisabledControlColor);
			}
			float num = (!this.Indent) ? 0 : Dialog_InterfaceOptions.IndentSize;
			foreach (string current in this.OptionValues) {
				string text = Translator.Translate (this.OptionValuePrefix + "." + current);
				float num2 = Text.CalcHeight (text, width - StringOptionsPreference.LabelMargin - num);
				Rect rect = new Rect (positionX - 4 + num, positionY - 3, width + 6 - num, num2 + 5);
				if (Mouse.IsOver (rect)) {
					Widgets.DrawHighlight (rect);
				}
				Rect rect2 = new Rect (positionX + num, positionY, width - StringOptionsPreference.LabelMargin - num, num2);
				GUI.Label (rect2, text);
				if (this.Tooltip != null) {
					TipSignal tipSignal = new TipSignal (() => Translator.Translate (this.Tooltip), this.TooltipId);
					TooltipHandler.TipRegion (rect2, tipSignal);
				}
				string valueForDisplay = this.ValueForDisplay;
				bool flag = valueForDisplay == current;
				if (Widgets.RadioButton (new Vector2 (positionX + width - StringOptionsPreference.RadioButtonWidth, positionY - 3), flag) && !disabled) {
					this.Value = current;
				}
				positionY += num2 + Dialog_InterfaceOptions.PreferencePadding.y;
			}
			positionY -= Dialog_InterfaceOptions.PreferencePadding.y;
			GUI.set_color (Color.get_white ());
		}

		//
		// Events
		//
		public event StringOptionsPreference.ValueChangedHandler ValueChanged {
			add {
				StringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				StringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<StringOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (StringOptionsPreference.ValueChangedHandler)Delegate.Combine (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove {
				StringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				StringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do {
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<StringOptionsPreference.ValueChangedHandler> (ref this.ValueChanged, (StringOptionsPreference.ValueChangedHandler)Delegate.Remove (valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class TabDrawer
	{
		//
		// Static Fields
		//
		private const float TabHoriztonalOverlap = 10;

		public static Color HeaderColor = new Color (1, 1, 1);

		public static Color SeparatorColor = new Color (0.3, 0.3, 0.3);

		public static Color NoneColor = new Color (0.4, 0.4, 0.4);

		public static Color BoxColor = new Color (0.1172, 0.1328, 0.1484);

		public static Color BoxBorderColor = new Color (0.52157, 0.52157, 0.52157);

		private static List<TabRecord> tabList = new List<TabRecord> ();

		public static Color TextColor = new Color (0.8, 0.8, 0.8);

		private const float MaxTabWidth = 200;

		private const float TabHeight = 32;

		public static readonly Texture2D AlternateRowTexture = SolidColorMaterials.NewSolidColorTexture (new Color (1, 1, 1, 0.05));

		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get ("UI/Buttons/Rename", true);

		public static Texture2D WhiteTexture = null;

		public static Vector2 TabPanelSize = new Vector2 (360, 670);

		//
		// Static Methods
		//
		public static void DrawBox (Rect rect)
		{
			GUI.set_color (TabDrawer.BoxColor);
			GUI.DrawTexture (rect, TabDrawer.WhiteTexture);
			GUI.set_color (TabDrawer.BoxBorderColor);
			Widgets.DrawBox (rect, 1);
			GUI.set_color (Color.get_white ());
		}

		public static float DrawHeader (float left, float top, float contentWidth, string labelText, bool drawSeparator = true, TextAnchor alignment = 0)
		{
			TextAnchor alignment2 = GUI.get_skin ().get_label ().get_alignment ();
			float num = 2;
			GUI.get_skin ().get_label ().set_alignment (alignment);
			Rect rect = new Rect (left, top, contentWidth, 22);
			rect.set_y (top);
			Text.set_Font (1);
			GUI.set_color (TabDrawer.HeaderColor);
			Widgets.Label (rect, labelText);
			if (drawSeparator) {
				GUI.set_color (TabDrawer.SeparatorColor);
				Widgets.DrawLineHorizontal (left, rect.get_y () + rect.get_height () - num, rect.get_width ());
				GUI.set_color (TabDrawer.TextColor);
			}
			float num2 = top + (rect.get_height () - num);
			GUI.get_skin ().get_label ().set_alignment (alignment2);
			return num2 - top;
		}

		public static float DrawNameAndBasicInfo (float left, float top, Pawn pawn, float contentWidth, bool allowRename = false)
		{
			float num = top;
			float num2 = 5;
			GUI.get_skin ().get_label ().set_alignment (0);
			Rect rect = new Rect (left, num, contentWidth, 30);
			Text.set_Font (2);
			if (pawn != null) {
				string text = (pawn.story == null) ? pawn.get_Label () : pawn.get_Name ().get_ToStringFull ();
				Vector2 vector = Text.CalcSize (text);
				GUI.set_color (TabDrawer.HeaderColor);
				Widgets.Label (rect, text);
				float num3 = rect.get_height () - num2;
				if (allowRename && pawn.get_IsColonist ()) {
					Rect rect2 = new Rect (left + vector.x + 7, num - 2, 30, 30);
					TooltipHandler.TipRegion (rect2, new TipSignal (Translator.Translate ("RenameColonist")));
					if (Widgets.ImageButton (rect2, TabDrawer.Rename)) {
						Find.get_WindowStack ().Add (new Dialog_ChangeNameTriple (pawn));
					}
				}
				num += num3;
			}
			Vector2 vector2 = new Vector2 (contentWidth, 24);
			Text.set_Font (1);
			GUI.set_color (TabDrawer.HeaderColor);
			Rect rect3 = new Rect (left, num, vector2.x, vector2.y);
			string text2 = (pawn.gender == 1) ? GenText.CapitalizeFirst (Translator.Translate ("Male")) : GenText.CapitalizeFirst (Translator.Translate ("Female"));
			if (!pawn.def.label.Equals (pawn.get_KindLabel ())) {
				Widgets.Label (rect3, string.Concat (new string[] {
					text2,
					" ",
					pawn.def.get_LabelCap (),
					" ",
					pawn.get_KindLabel (),
					", ",
					Translator.Translate ("AgeIndicator", new object[] {
						pawn.ageTracker.get_AgeNumberString ()
					})
				}));
				TooltipHandler.TipRegion (rect3, () => pawn.ageTracker.get_AgeTooltipString (), 6873641);
			}
			else {
				Widgets.Label (rect3, string.Concat (new string[] {
					text2,
					" ",
					pawn.def.get_LabelCap (),
					", ",
					Translator.Translate ("AgeIndicator", new object[] {
						pawn.ageTracker.get_AgeNumberString ()
					})
				}));
			}
			num += vector2.y;
			return num - top - 3;
		}

		public static void DrawTab (TabRecord record, Rect rect, Texture2D atlas)
		{
			Rect rect2 = new Rect (rect);
			rect2.set_width (30);
			Rect rect3 = new Rect (rect);
			rect3.set_width (30);
			rect3.set_x (rect.get_x () + rect.get_width () - 30);
			Rect rect4 = new Rect (0.53125, 0, 0.46875, 1);
			Rect rect5 = new Rect (rect);
			rect5.set_x (rect5.get_x () + rect2.get_width ());
			rect5.set_width (rect5.get_width () - 60);
			Rect rect6 = Widgets.ToUVRect (new Rect (30, 0, 4, (float)atlas.get_height ()), new Vector2 ((float)atlas.get_width (), (float)atlas.get_height ()));
			Widgets.DrawTexturePart (rect2, new Rect (0, 0, 0.46875, 1), atlas);
			Widgets.DrawTexturePart (rect5, rect6, atlas);
			Widgets.DrawTexturePart (rect3, rect4, atlas);
			Rect rect7 = rect;
			if (rect.Contains (Event.get_current ().get_mousePosition ())) {
				GUI.set_color (Color.get_yellow ());
				rect7.set_x (rect7.get_x () + 2);
				rect7.set_y (rect7.get_y () - 2);
			}
			Widgets.Label (rect7, record.label);
			GUI.set_color (Color.get_white ());
			if (!record.selected) {
				Rect rect8 = new Rect (rect);
				rect8.set_y (rect8.get_y () + rect.get_height ());
				rect8.set_y (rect8.get_y () - 1);
				rect8.set_height (1);
				Rect rect9 = new Rect (0.5, 0.01, 0.01, 0.01);
				Widgets.DrawTexturePart (rect8, rect9, atlas);
			}
		}

		public static TabRecord DrawTabs (Rect baseRect, IEnumerable<TabRecord> tabsEnum, Texture2D atlas)
		{
			TabDrawer.tabList.Clear ();
			foreach (TabRecord current in tabsEnum) {
				TabDrawer.tabList.Add (current);
			}
			TabRecord tabRecord = null;
			TabRecord tabRecord2 = (from t in TabDrawer.tabList
			where t.selected
			select t).FirstOrDefault<TabRecord> ();
			if (tabRecord2 == null) {
				Debug.LogWarning ("Drew tabs without any being selected.");
				return TabDrawer.tabList [0];
			}
			float num = baseRect.get_width () + (float)(TabDrawer.tabList.Count - 1) * 10;
			float tabWidth = num / (float)TabDrawer.tabList.Count;
			if (tabWidth > 200) {
				tabWidth = 200;
			}
			Rect rect = new Rect (baseRect);
			rect.set_y (rect.get_y () - 32);
			rect.set_height (9999);
			GUI.BeginGroup (rect);
			Text.set_Anchor (4);
			Text.set_Font (1);
			Func<TabRecord, Rect> func = delegate (TabRecord tab) {
				int num2 = TabDrawer.tabList.IndexOf (tab);
				float num3 = (float)num2 * (tabWidth - 10);
				return new Rect (num3, 1, tabWidth, 32);
			};
			List<TabRecord> list = GenList.ListFullCopy<TabRecord> (TabDrawer.tabList);
			list.Remove (tabRecord2);
			list.Add (tabRecord2);
			TabRecord tabRecord3 = null;
			List<TabRecord> list2 = GenList.ListFullCopy<TabRecord> (list);
			list2.Reverse ();
			for (int i = 0; i < list2.Count; i++) {
				TabRecord tabRecord4 = list2 [i];
				Rect rect2 = func.Invoke (tabRecord4);
				if (tabRecord3 == null && rect2.Contains (Event.get_current ().get_mousePosition ())) {
					tabRecord3 = tabRecord4;
				}
				MouseoverSounds.DoRegion (rect2);
				if (Widgets.InvisibleButton (rect2)) {
					tabRecord = tabRecord4;
				}
			}
			foreach (TabRecord current2 in list) {
				Rect rect3 = func.Invoke (current2);
				TabDrawer.DrawTab (current2, rect3, atlas);
			}
			Text.set_Anchor (0);
			GUI.EndGroup ();
			if (tabRecord != null) {
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.SelectDesignator);
				if (tabRecord.clickedAction != null) {
					tabRecord.clickedAction.Invoke ();
				}
			}
			return tabRecord;
		}

		public static void ResetTextures ()
		{
			TabDrawer.WhiteTexture = SolidColorMaterials.NewSolidColorTexture (new Color (1, 1, 1));
		}
	}
}
using RimWorld;
using System;

namespace EdB.Interface
{
	public class TargeterComponent : IRenderedComponent, IUpdatedComponent, INamedComponent
	{
		//
		// Fields
		//
		private Targeter targeter;

		//
		// Properties
		//
		public string Name {
			get {
				return "Targeter";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Constructors
		//
		public TargeterComponent (Targeter targeter)
		{
			this.targeter = targeter;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.targeter.TargeterOnGUI ();
		}

		public void Update ()
		{
			this.targeter.TargeterUpdate ();
		}
	}
}
using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	internal class TexButton
	{
		//
		// Static Fields
		//
		public static readonly Texture2D CloseXBig = ContentFinder<Texture2D>.Get ("UI/Widgets/CloseX", true);

		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get ("UI/Buttons/Minus", true);

		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get ("UI/Buttons/Plus", true);

		public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get ("UI/Buttons/ReorderDown", true);

		public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get ("UI/Buttons/ReorderUp", true);

		public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get ("UI/Buttons/Delete", true);

		public static readonly Texture2D NextBig = ContentFinder<Texture2D>.Get ("UI/Widgets/NextArrow", true);

		public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get ("UI/Widgets/CloseXSmall", true);

		public static readonly Texture2D OpenDebugActionsMenu = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);

		public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get ("UI/Buttons/Suspend", true);

		public static readonly Texture2D OpenInspector = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/OpenInspector", true);

		public static readonly Texture2D ToggleLog = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/ToggleLog", true);

		public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get ("UI/Buttons/Paste", true);

		public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get ("UI/Buttons/Copy", true);

		public static readonly Texture2D OpenStatsReport = ContentFinder<Texture2D>.Get ("UI/Buttons/OpenStatsReport", true);

		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get ("UI/Buttons/Rename", true);

		public static readonly Texture2D Info = ContentFinder<Texture2D>.Get ("UI/Buttons/InfoButton", true);

		public static readonly Texture2D SelectOverlappingNext = ContentFinder<Texture2D>.Get ("UI/Buttons/SelectNextOverlapping", true);

		public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/OpenInspectSettings", true);

		public static readonly Texture2D Save = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Save", true);

		public static readonly Texture2D Empty = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Empty", true);

		public static readonly Texture2D Collapse = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Collapse", true);

		public static readonly Texture2D Reveal = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Reveal", true);

		public static readonly Texture2D NewItem = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/NewItem", true);

		public static readonly Texture2D Add = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Add", true);

		public static readonly Texture2D TogglePauseOnError = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/TogglePauseOnError", true);

		public static readonly Texture2D OpenPackageEditor = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/OpenPackageEditor", true);

		public static readonly Texture2D NewFile = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/NewFile", true);

		public static readonly Texture2D CenterOnPointsTex = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/CenterOnPoints", true);

		public static readonly Texture2D ToggleGodMode = ContentFinder<Texture2D>.Get ("UI/Buttons/DevRoot/ToggleGodMode", true);

		public static readonly Texture2D InspectModeToggle = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/InspectModeToggle", true);

		public static readonly Texture2D RangeMatch = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/RangeMatch", true);

		public static readonly Texture2D Stop = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Stop", true);

		public static readonly Texture2D Play = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Play", true);

		public static readonly Texture2D Reload = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Reload", true);

		public static readonly Texture2D RenameDev = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/Rename", true);

		public static readonly Texture2D CurveResetTex = ContentFinder<Texture2D>.Get ("UI/Buttons/Dev/CurveReset", true);
	}
}
using System;
using UnityEngine;

namespace EdB.Interface
{
	public class TextureColorPair
	{
		//
		// Fields
		//
		public Texture texture;

		public Color color;

		//
		// Constructors
		//
		public TextureColorPair (Texture texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}
	}
}
using RimWorld;
using System;
using System.Linq;
using Verse;

namespace EdB.Interface
{
	public static class ThingDefExtensions
	{
		//
		// Static Methods
		//
		public static bool BelongsToCategory (this ThingDef def, string category)
		{
			return def.thingCategories != null && def.thingCategories.FirstOrDefault ((ThingCategoryDef d) => category.Equals (d.defName)) != null;
		}

		public static bool DeploysFromEscapePod (this ThingDef def)
		{
			return def.apparel != null || (def.weaponTags != null && def.weaponTags.Count > 0) || def.BelongsToCategory ("FoodMeals") || (def == ThingDefOf.Medicine || def.defName.Equals ("GlitterworldMedicine"));
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class ThingOverlaysComponent : IRenderedComponent, INamedComponent
	{
		//
		// Fields
		//
		private ThingOverlays thingOverlays;

		//
		// Properties
		//
		public string Name {
			get {
				return "ThingOverlays";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Constructors
		//
		public ThingOverlaysComponent (ThingOverlays thingOverlays)
		{
			this.thingOverlays = thingOverlays;
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			this.thingOverlays.ThingOverlaysOnGUI ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class ThingTooltipComponent : IRenderedComponent, INamedComponent
	{
		//
		// Properties
		//
		public string Name {
			get {
				return "ThingTooltips";
			}
		}

		public bool RenderWithScreenshots {
			get {
				return true;
			}
		}

		//
		// Methods
		//
		public void OnGUI ()
		{
			Find.get_TooltipGiverList ().DispenseAllThingTooltips ();
		}
	}
}
using RimWorld;
using System;
using Verse;

namespace EdB.Interface
{
	public class TrackedColonist
	{
		//
		// Fields
		//
		private Pawn pawn;

		private Faction capturingFaction;

		private Corpse corpse;

		private int missingTimestamp;

		private bool cryptosleep;

		private bool missing;

		private bool dead;

		//
		// Properties
		//
		public bool Broken {
			get {
				return this.BrokenState != null;
			}
		}

		public BrokenStateDef BrokenState {
			get {
				if (this.pawn.mindState != null && this.pawn.mindState.broken != null) {
					return this.pawn.mindState.broken.get_CurStateDef ();
				}
				return null;
			}
		}

		public bool Captured {
			get {
				return this.capturingFaction != null;
			}
		}

		public Faction CapturingFaction {
			get {
				return this.capturingFaction;
			}
			set {
				this.capturingFaction = value;
			}
		}

		public Pawn Carrier {
			get {
				if (this.pawn.holder != null && this.pawn.holder.owner != null) {
					Pawn_CarryTracker pawn_CarryTracker = this.pawn.holder.owner as Pawn_CarryTracker;
					if (pawn_CarryTracker != null) {
						return pawn_CarryTracker.pawn;
					}
				}
				return null;
			}
		}

		public bool Controllable {
			get {
				return !this.Missing && !this.Dead && !this.Captured && !this.Incapacitated && !this.Broken && !this.Cryptosleep;
			}
		}

		public Corpse Corpse {
			get {
				return this.corpse;
			}
			set {
				this.corpse = value;
			}
		}

		public bool Cryptosleep {
			get {
				return this.cryptosleep;
			}
			set {
				this.cryptosleep = value;
			}
		}

		public bool Dead {
			get {
				return this.dead;
			}
			set {
				this.dead = value;
			}
		}

		public bool Drafted {
			get {
				return !this.dead && this.pawn.get_Drafted ();
			}
		}

		public float HealthPercent {
			get {
				if (this.pawn.health != null && this.pawn.health.summaryHealth != null) {
					return this.pawn.health.summaryHealth.get_SummaryHealthPercent ();
				}
				return 0;
			}
		}

		public bool Incapacitated {
			get {
				return this.pawn.health != null && this.pawn.health.get_Downed ();
			}
		}

		public int MentalBreakWarningLevel {
			get {
				if (this.pawn.mindState != null && this.pawn.mindState.breaker != null && !this.pawn.get_Downed () && !this.pawn.get_Dead ()) {
					if (this.pawn.mindState.breaker.get_HardBreakImminent ()) {
						return 2;
					}
					if (this.pawn.mindState.breaker.get_MentalBreakApproaching ()) {
						return 1;
					}
				}
				return 0;
			}
		}

		public bool Missing {
			get {
				return this.missing;
			}
			set {
				this.missing = value;
			}
		}

		public int MissingTimestamp {
			get {
				return this.missingTimestamp;
			}
			set {
				this.missingTimestamp = value;
			}
		}

		public Pawn Pawn {
			get {
				return this.pawn;
			}
			set {
				this.pawn = value;
			}
		}

		//
		// Constructors
		//
		public TrackedColonist ()
		{
		}

		public TrackedColonist (Pawn pawn)
		{
			this.pawn = pawn;
			this.dead = false;
			this.missing = false;
			this.missingTimestamp = 0;
			this.corpse = null;
			this.capturingFaction = null;
			this.cryptosleep = false;
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public static class TrainingCardUtility
	{
		//
		// Static Fields
		//
		private const float RowHeight = 28;

		private const float InfoHeaderHeight = 50;

		private static FieldInfo stepsField;

		//
		// Constructors
		//
		static TrainingCardUtility ()
		{
			TrainingCardUtility.stepsField = typeof(Pawn_TrainingTracker).GetField ("steps", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		//
		// Static Methods
		//
		public static void DrawTrainingCard (Rect rect, Pawn pawn)
		{
			try {
				GUI.BeginGroup (rect);
				Rect rect2 = new Rect (0, 0, rect.get_width (), 25);
				string text = Translator.Translate ("Master") + ": ";
				Vector2 vector = Text.CalcSize (text);
				Widgets.Label (rect2, text);
				Rect rect3 = new Rect (rect2.get_x () + vector.x + 6, rect2.get_y (), 200, rect2.get_height ());
				if (pawn.training.IsCompleted (TrainableDefOf.Obedience)) {
					rect3.set_y (rect3.get_y () - 1);
					string text2 = TrainableUtility.MasterString (pawn);
					if (Widgets.TextButton (rect3, text2, true, false)) {
						TrainableUtility.OpenMasterSelectMenu (pawn);
					}
				}
				else {
					GUI.set_color (new Color (0.7, 0.7, 0.7));
					Widgets.Label (rect3, Translator.Translate ("None"));
					GUI.set_color (Color.get_white ());
				}
				List<TrainableDef> trainableDefsInListOrder = TrainableUtility.get_TrainableDefsInListOrder ();
				int count = trainableDefsInListOrder.Count;
				float num = (float)(count * 28 + 20);
				Rect rect4 = new Rect (0, rect2.get_y () + 35, rect.get_width (), num);
				TabDrawer.DrawBox (rect4);
				Rect rect5 = GenUI.ContractedBy (rect4, 10);
				rect5.set_height (28);
				for (int i = 0; i < trainableDefsInListOrder.Count; i++) {
					if (TrainingCardUtility.TryDrawTrainableRow (rect5, pawn, trainableDefsInListOrder [i])) {
						rect5.set_y (rect5.get_y () + 28);
					}
				}
				Text.set_Anchor (2);
				string text3 = Translator.Translate ("TrainableIntelligence") + ": " + TrainableIntelligenceExtension.GetLabel (pawn.get_RaceProps ().trainableIntelligence);
				Widgets.Label (new Rect (0, rect4.get_y () + rect4.get_height () + 5, rect4.get_width () - 2, 25), text3);
				Text.set_Anchor (0);
			}
			finally {
				GUI.EndGroup ();
			}
		}

		public static int GetSteps (Pawn_TrainingTracker training, TrainableDef td)
		{
			DefMap<TrainableDef, int> defMap = (DefMap<TrainableDef, int>)TrainingCardUtility.stepsField.GetValue (training);
			return defMap.get_Item (td);
		}

		private static void SetWantedRecursive (TrainableDef td, Pawn pawn, bool checkOn)
		{
			pawn.training.SetWanted (td, checkOn);
			if (checkOn) {
				if (td.prerequisites != null) {
					for (int i = 0; i < td.prerequisites.Count; i++) {
						TrainingCardUtility.SetWantedRecursive (td.prerequisites [i], pawn, true);
					}
				}
			}
			else {
				IEnumerable<TrainableDef> enumerable = from t in DefDatabase<TrainableDef>.get_AllDefsListForReading ()
				where t.prerequisites != null && t.prerequisites.Contains (td)
				select t;
				foreach (TrainableDef current in enumerable) {
					TrainingCardUtility.SetWantedRecursive (current, pawn, false);
				}
			}
		}

		private static bool TryDrawTrainableRow (Rect rect, Pawn pawn, TrainableDef td)
		{
			bool flag = pawn.training.IsCompleted (td);
			bool flag2;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain (td, ref flag2);
			if (!flag2) {
				return false;
			}
			Widgets.DrawHighlightIfMouseover (rect);
			Rect rect2 = rect;
			rect2.set_width (rect2.get_width () - 50);
			rect2.set_xMin (rect2.get_xMin () + (float)td.indent * 15);
			Rect rect3 = rect;
			rect3.set_xMin (rect3.get_xMax () - 50 + 17);
			if (!flag) {
				bool wanted = pawn.training.GetWanted (td);
				bool flag3 = wanted;
				Widgets.LabelCheckbox (rect2, td.get_LabelCap (), ref wanted, !canTrain.get_Accepted ());
				if (wanted != flag3) {
					ConceptDatabase.KnowledgeDemonstrated (ConceptDefOf.AnimalTraining, 6);
					TrainingCardUtility.SetWantedRecursive (td, pawn, wanted);
				}
			}
			else {
				Text.set_Anchor (3);
				Widgets.Label (rect2, td.get_LabelCap ());
				Text.set_Anchor (0);
			}
			if (flag) {
				GUI.set_color (Color.get_green ());
			}
			Text.set_Anchor (3);
			Widgets.Label (rect3, TrainingCardUtility.GetSteps (pawn.training, td) + " / " + td.steps);
			Text.set_Anchor (0);
			if (Game.get_GodMode () && !pawn.training.IsCompleted (td)) {
				Rect rect4 = rect3;
				rect4.set_yMin (rect4.get_yMax () - 10);
				rect4.set_xMin (rect4.get_xMax () - 10);
				if (Widgets.TextButton (rect4, "+", true, false)) {
					pawn.training.Train (td, GenCollection.RandomElement<Pawn> (Find.get_ListerPawns ().get_FreeColonistsSpawned ()));
				}
			}
			TooltipHandler.TipRegion (rect, delegate {
				string text = td.get_LabelCap () + "

" + td.description;
				if (!canTrain.get_Accepted ()) {
					text = text + "

" + canTrain.get_Reason ();
				}
				else if (!GenList.NullOrEmpty<TrainableDef> (td.prerequisites)) {
					text += "
";
					for (int i = 0; i < td.prerequisites.Count; i++) {
						if (!pawn.training.IsCompleted (td.prerequisites [i])) {
							text = text + "
" + Translator.Translate ("TrainingNeedsPrerequisite", new object[] {
								td.prerequisites [i].get_LabelCap ()
							});
						}
					}
				}
				return text;
			}, (int)(rect.get_y () * 612));
			GUI.set_color (Color.get_white ());
			return true;
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public class UIRoot_MapIntermediary : UIRoot
	{
		//
		// Methods
		//
		public override void UIRootOnGUI ()
		{
			base.UIRootOnGUI ();
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class UserInterface : UIRoot_Map
	{
		public delegate void UIRootOnGUIDelegate ();

		public delegate void UIRootUpdateDelegate ();

		//
		// Fields
		//
		private List<IUpdatedComponent> updatedComponents = new List<IUpdatedComponent> ();

		private List<IInitializedComponent> initializedComponents = new List<IInitializedComponent> ();

		private HashSet<IComponentWithPreferences> componentsWithPreferences = new HashSet<IComponentWithPreferences> ();

		private Dictionary<string, INamedComponent> componentDictionary = new Dictionary<string, INamedComponent> ();

		private ScreenSizeMonitor screenSizeMonitor = new ScreenSizeMonitor ();

		private List<IRenderedComponent> renderedComponents = new List<IRenderedComponent> ();

		public MainTabsRoot mainTabsRoot = new MainTabsRoot ();

		private MouseoverReadout mouseoverReadout = new MouseoverReadout ();

		public GlobalControls globalControls = new GlobalControls ();

		private UIRoot_MapIntermediary uiRootIntermediary;

		private UserInterface.UIRootOnGUIDelegate uiRootOnGUIDelegate;

		private UserInterface.UIRootUpdateDelegate uiRootUpdateDelegate;

		//
		// Properties
		//
		public MainTabsRoot MainTabsRoot {
			get {
				return this.mainTabsRoot;
			}
		}

		public ScreenSizeMonitor ScreenSizeMonitor {
			get {
				return this.screenSizeMonitor;
			}
		}

		//
		// Constructors
		//
		public UserInterface ()
		{
			Preferences.Instance.Reset ();
			this.uiRootIntermediary = new UIRoot_MapIntermediary ();
			this.uiRootOnGUIDelegate = (UserInterface.UIRootOnGUIDelegate)Delegate.CreateDelegate (typeof(UserInterface.UIRootOnGUIDelegate), this.uiRootIntermediary, "UIRootOnGUI");
			this.uiRootOnGUIDelegate.GetType ().BaseType.BaseType.GetField ("m_target", BindingFlags.Instance | BindingFlags.NonPublic).SetValue (this.uiRootOnGUIDelegate, this);
			this.uiRootUpdateDelegate = (UserInterface.UIRootUpdateDelegate)Delegate.CreateDelegate (typeof(UserInterface.UIRootUpdateDelegate), this.uiRootIntermediary, "UIRootUpdate");
			this.uiRootUpdateDelegate.GetType ().BaseType.BaseType.GetField ("m_target", BindingFlags.Instance | BindingFlags.NonPublic).SetValue (this.uiRootUpdateDelegate, this);
			this.AddUpdatedComponent (new ComponentColonistTracker ());
			this.AddRenderedComponent (new ComponentColonistBar ());
			this.AddUpdatedComponent (new ComponentSquadManager ());
			this.AddRenderedComponent (new DebugWindowsOpenerComponent (this.debugWindowOpener));
			this.AddInitializedComponent (new ComponentInventory ());
			this.AddUpdatedComponent (new ComponentTabReplacement ());
			this.AddInitializedComponent (new ComponentMainTabCloseButton ());
			this.AddInitializedComponent (new ComponentHideMainTabs ());
			this.AddInitializedComponent (new ComponentAlternateTimeDisplay ());
			this.AddInitializedComponent (new ComponentPauseOnStart ());
			this.AddInitializedComponent (new ComponentAlternateMaterialSelection ());
			this.AddInitializedComponent (new ComponentEmptyStockpile ());
			this.AddInitializedComponent (new ComponentColorCodedWorkPassions ());
			Preferences.Instance.Load ();
			HashSet<ICustomTextureComponent> hashSet = new HashSet<ICustomTextureComponent> ();
			foreach (IRenderedComponent current in this.renderedComponents) {
				ICustomTextureComponent customTextureComponent = current as ICustomTextureComponent;
				if (customTextureComponent != null && !hashSet.Contains (customTextureComponent)) {
					customTextureComponent.ResetTextures ();
					hashSet.Add (customTextureComponent);
				}
			}
			foreach (IUpdatedComponent current2 in this.updatedComponents) {
				ICustomTextureComponent customTextureComponent2 = current2 as ICustomTextureComponent;
				if (customTextureComponent2 != null && !hashSet.Contains (customTextureComponent2)) {
					customTextureComponent2.ResetTextures ();
					hashSet.Add (customTextureComponent2);
				}
			}
			foreach (IInitializedComponent current3 in this.initializedComponents) {
				ICustomTextureComponent customTextureComponent3 = current3 as ICustomTextureComponent;
				if (customTextureComponent3 != null && !hashSet.Contains (customTextureComponent3)) {
					customTextureComponent3.ResetTextures ();
					hashSet.Add (customTextureComponent3);
				}
			}
			foreach (IInitializedComponent current4 in this.initializedComponents) {
				current4.PrepareDependencies (this);
			}
			foreach (IInitializedComponent current5 in this.initializedComponents) {
				current5.Initialize (this);
			}
			hashSet.Clear ();
			this.ResetTextures ();
		}

		//
		// Methods
		//
		protected void AddComponentPreferences (IComponentWithPreferences component)
		{
			if (!this.componentsWithPreferences.Contains (component)) {
				foreach (IPreference current in component.Preferences) {
					Preferences.Instance.Add (current);
					this.componentsWithPreferences.Add (component);
				}
			}
		}

		public void AddInitializedComponent (IInitializedComponent component)
		{
			if (!this.initializedComponents.Contains (component)) {
				this.initializedComponents.Add (component);
				if (component is IComponentWithPreferences) {
					this.AddComponentPreferences (component as IComponentWithPreferences);
				}
				if (component is INamedComponent) {
					this.AddNamedComponent (component as INamedComponent);
				}
			}
		}

		protected void AddNamedComponent (INamedComponent component)
		{
			if (!this.componentDictionary.ContainsKey (component.Name)) {
				this.componentDictionary.Add (component.Name, component);
			}
		}

		protected bool AddRenderedComponent (IRenderedComponent component, string name, int offset)
		{
			int num = this.renderedComponents.FindIndex (delegate (IRenderedComponent c) {
				INamedComponent namedComponent = c as INamedComponent;
				return namedComponent != null && namedComponent.Name == name;
			});
			if (num == -1) {
				return false;
			}
			if (num + offset < 0) {
				return false;
			}
			this.InsertRenderedComponent (num + offset, component);
			return true;
		}

		public void AddRenderedComponent (IRenderedComponent component)
		{
			this.InsertRenderedComponent (this.renderedComponents.Count, component);
		}

		public bool AddRenderedComponentAbove (IRenderedComponent component, string name)
		{
			return this.AddRenderedComponent (component, name, 1);
		}

		public bool AddRenderedComponentBelow (IRenderedComponent component, string name)
		{
			return this.AddRenderedComponent (component, name, 0);
		}

		public void AddUpdatedComponent (IUpdatedComponent component)
		{
			this.updatedComponents.Add (component);
			if (component is IInitializedComponent) {
				this.AddInitializedComponent (component as IInitializedComponent);
			}
			if (component is IComponentWithPreferences) {
				this.AddComponentPreferences (component as IComponentWithPreferences);
			}
			if (component is INamedComponent) {
				this.AddNamedComponent (component as INamedComponent);
			}
		}

		protected void CallAncestorUIRootOnGUI ()
		{
			this.uiRootOnGUIDelegate ();
		}

		protected void CallAncestorUIRootUpdate ()
		{
			this.uiRootUpdateDelegate ();
		}

		public T FindMainTabOfType<T> () where T : MainTabWindow
		{
			foreach (MainTabDef current in this.mainTabsRoot.AllTabs) {
				MainTabWindow window = current.get_Window ();
				if (window != null && window is T) {
					return (T)((object)window);
				}
			}
			return (T)((object)null);
		}

		public INamedComponent FindNamedComponent (string name)
		{
			INamedComponent result = null;
			if (this.componentDictionary.TryGetValue (name, out result)) {
				return result;
			}
			return null;
		}

		public T FindNamedComponentAs<T> (string name)
		{
			INamedComponent namedComponent = null;
			if (this.componentDictionary.TryGetValue (name, out namedComponent) && namedComponent is T) {
				return (T)((object)namedComponent);
			}
			return default(T);
		}

		protected void InsertRenderedComponent (int index, IRenderedComponent component)
		{
			this.renderedComponents.Insert (index, component);
			if (component is IInitializedComponent) {
				this.AddInitializedComponent (component as IInitializedComponent);
			}
			if (component is IComponentWithPreferences) {
				this.AddComponentPreferences (component as IComponentWithPreferences);
			}
			if (component is INamedComponent) {
				this.AddNamedComponent (component as INamedComponent);
			}
		}

		private void OpenMainMenuShortcut ()
		{
			if (Event.get_current ().get_type () == 4 && Event.get_current ().get_keyCode () == 27) {
				Event.get_current ().Use ();
				this.mainTabsRoot.SetCurrentTab (MainTabDefOf.Menu, true);
			}
		}

		public bool ReplaceComponent (object component, string name)
		{
			bool result = false;
			if (this.ReplaceComponentOfType<IRenderedComponent> (component, name, this.renderedComponents)) {
				result = true;
			}
			if (this.ReplaceComponentOfType<IUpdatedComponent> (component, name, this.updatedComponents)) {
				result = true;
			}
			if (this.ReplaceComponentOfType<IInitializedComponent> (component, name, this.initializedComponents)) {
				result = true;
			}
			return result;
		}

		protected bool ReplaceComponentOfType<T> (object component, string name, List<T> list)
		{
			bool result = false;
			if (component is T) {
				int num = list.FindIndex (delegate (T c) {
					INamedComponent namedComponent = c as INamedComponent;
					return namedComponent != null && namedComponent.Name == name;
				});
				if (num != -1) {
					list [num] = (T)((object)component);
					result = true;
				}
			}
			return result;
		}

		protected void ResetTextures ()
		{
			Button.ResetTextures ();
		}

		public override void UIRootOnGUI ()
		{
			this.CallAncestorUIRootOnGUI ();
			this.screenSizeMonitor.Update ();
			this.thingOverlays.ThingOverlaysOnGUI ();
			for (int i = 0; i < Find.get_Map ().components.Count; i++) {
				Find.get_Map ().components [i].MapComponentOnGUI ();
			}
			bool filtersCurrentEvent = this.screenshotMode.get_FiltersCurrentEvent ();
			foreach (IRenderedComponent current in this.renderedComponents) {
				if (!filtersCurrentEvent || current.RenderWithScreenshots) {
					current.OnGUI ();
				}
			}
			BeautyDrawer.BeautyOnGUI ();
			this.selector.dragBox.DragBoxOnGUI ();
			DesignatorManager.DesignationManagerOnGUI ();
			this.targeter.TargeterOnGUI ();
			Find.get_TooltipGiverList ().DispenseAllThingTooltips ();
			Find.get_ColonyInfo ().ColonyInfoOnGUI ();
			DebugTools.DebugToolsOnGUI ();
			if (!this.screenshotMode.get_FiltersCurrentEvent ()) {
				this.globalControls.GlobalControlsOnGUI ();
				this.resourceReadout.ResourceReadoutOnGUI ();
				this.mainTabsRoot.MainTabsOnGUI ();
				this.mouseoverReadout.MouseoverReadoutOnGUI ();
				this.alerts.AlertsReadoutOnGUI ();
				ActiveTutorNoteManager.ActiveLessonManagerOnGUI ();
			}
			RoomStatsDrawer.RoomStatsOnGUI ();
			Find.get_DebugDrawer ().DebugDrawerOnGUI ();
			this.windows.WindowStackOnGUI ();
			DesignatorManager.ProcessInputEvents ();
			this.targeter.ProcessInputEvents ();
			this.mainTabsRoot.HandleLowPriorityShortcuts ();
			this.selector.SelectorOnGUI ();
			this.OpenMainMenuShortcut ();
		}

		public override void UIRootUpdate ()
		{
			this.CallAncestorUIRootUpdate ();
			try {
				Messages.Update ();
				this.targeter.TargeterUpdate ();
				SelectionDrawer.DrawSelectionOverlays ();
				RoomStatsDrawer.DrawRoomOverlays ();
				DesignatorManager.DesignatorManagerUpdate ();
				this.alerts.AlertsReadoutUpdate ();
				ConceptDecider.ConceptDeciderUpdate ();
				foreach (IUpdatedComponent current in this.updatedComponents) {
					current.Update ();
				}
			}
			catch (Exception ex) {
				Log.Error ("Exception in UIRootUpdate: " + ex.ToString ());
			}
		}
	}
}
using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class WidgetDrawer
	{
		//
		// Static Fields
		//
		public static readonly Texture2D RadioButOnTex;

		public static float LabelMargin;

		public static float CheckboxMargin;

		public static float CheckboxHeight;

		public static float CheckboxWidth;

		public static readonly Texture2D RadioButOffTex;

		//
		// Constructors
		//
		static WidgetDrawer ()
		{
			WidgetDrawer.CheckboxWidth = 24;
			WidgetDrawer.CheckboxHeight = 30;
			WidgetDrawer.CheckboxMargin = 18;
			WidgetDrawer.LabelMargin = WidgetDrawer.CheckboxWidth + WidgetDrawer.CheckboxMargin;
			WidgetDrawer.RadioButOnTex = ContentFinder<Texture2D>.Get ("UI/Widgets/RadioButOn", true);
			WidgetDrawer.RadioButOffTex = ContentFinder<Texture2D>.Get ("UI/Widgets/RadioButOff", true);
		}

		//
		// Static Methods
		//
		public static float DrawLabeledCheckbox (Rect rect, string labelText, ref bool value)
		{
			return WidgetDrawer.DrawLabeledCheckbox (rect, labelText, ref value, false);
		}

		public static float DrawLabeledCheckbox (Rect rect, string labelText, ref bool value, bool disabled)
		{
			Text.set_Anchor (0);
			float num = rect.get_width () - WidgetDrawer.LabelMargin;
			float num2 = Text.CalcHeight (labelText, num);
			Rect rect2 = new Rect (rect.get_x (), rect.get_y (), num, num2);
			Color color = GUI.get_color ();
			if (disabled) {
				GUI.set_color (Dialog_InterfaceOptions.DisabledControlColor);
			}
			Widgets.Label (rect2, labelText);
			GUI.set_color (color);
			Widgets.Checkbox (new Vector2 (rect.get_x () + num + WidgetDrawer.CheckboxMargin, rect.get_y () - 1), ref value, 24, disabled);
			return (num2 >= WidgetDrawer.CheckboxHeight) ? num2 : WidgetDrawer.CheckboxHeight;
		}

		public static bool DrawLabeledRadioButton (Rect rect, string labelText, bool chosen, bool playSound)
		{
			TextAnchor anchor = Text.get_Anchor ();
			Text.set_Anchor (3);
			Rect rect2 = new Rect (rect.get_x (), rect.get_y () - 2, rect.get_width (), rect.get_height ());
			Widgets.Label (rect2, labelText);
			Text.set_Anchor (anchor);
			bool flag = Widgets.InvisibleButton (rect);
			if (playSound && flag && !chosen) {
				SoundStarter.PlayOneShotOnCamera (SoundDefOf.RadioButtonClicked);
			}
			Vector2 topLeft = new Vector2 (rect.get_x () + rect.get_width () - 32, rect.get_y () + rect.get_height () / 2 - 16);
			WidgetDrawer.DrawRadioButton (topLeft, chosen);
			return flag;
		}

		public static void DrawRadioButton (Vector2 topLeft, bool chosen)
		{
			Texture2D texture2D;
			if (chosen) {
				texture2D = WidgetDrawer.RadioButOnTex;
			}
			else {
				texture2D = WidgetDrawer.RadioButOffTex;
			}
			Rect rect = new Rect (topLeft.x, topLeft.y, 24, 24);
			GUI.DrawTexture (rect, texture2D);
		}
	}
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public static class WidgetsWork
	{
		//
		// Static Fields
		//
		public const float WorkBoxSize = 25;

		private static Texture2D PassionWorkboxMajorIcon = ContentFinder<Texture2D>.Get ("UI/Icons/PassionMajorGray", true);

		private static Texture2D PassionWorkboxMinorIcon = ContentFinder<Texture2D>.Get ("UI/Icons/PassionMinorGray", true);

		private static readonly Texture2D WorkBoxCheckTex = ContentFinder<Texture2D>.Get ("UI/Widgets/WorkBoxCheck", true);

		private static readonly Texture2D WorkBoxBGTex_Excellent = ContentFinder<Texture2D>.Get ("UI/Widgets/WorkBoxBG_Excellent", true);

		private static readonly Texture2D WorkBoxBGTex_Mid = ContentFinder<Texture2D>.Get ("UI/Widgets/WorkBoxBG_Mid", true);

		private static readonly Texture2D WorkBoxBGTex_Bad = ContentFinder<Texture2D>.Get ("UI/Widgets/WorkBoxBG_Bad", true);

		private const float PassionOpacity = 0.4;

		private const int MidAptCutoff = 14;

		//
		// Static Properties
		//
		public static bool ColorCodedWorkPassions {
			get {
				return WidgetsWork.PreferenceColorCodedWorkPassions != null && WidgetsWork.PreferenceColorCodedWorkPassions.Value;
			}
		}

		public static PreferenceColorCodedWorkPassions PreferenceColorCodedWorkPassions {
			get;
			set;
		}

		//
		// Static Methods
		//
		private static Color ColorOfPriority (int prio, Passion passion)
		{
			if (!WidgetsWork.ColorCodedWorkPassions) {
				switch (prio) {
				case 1:
					return Color.get_green ();
				case 2:
					return new Color (1, 0.9, 0.6);
				case 3:
					return new Color (0.8, 0.7, 0.5);
				case 4:
					return new Color (0.6, 0.6, 0.6);
				default:
					return Color.get_grey ();
				}
			}
			else {
				if (passion == 1) {
					return new Color (1, 1, 0);
				}
				if (passion != 2) {
					return new Color (0.8, 0.8, 0.8);
				}
				return new Color (0, 1, 0);
			}
		}

		private static void DrawWorkBoxBackground (Rect rect, Pawn p, WorkTypeDef workDef)
		{
			float num = p.skills.AverageOfRelevantSkillsFor (workDef);
			Texture2D texture2D;
			Texture2D texture2D2;
			float num2;
			if (num <= 14) {
				texture2D = WidgetsWork.WorkBoxBGTex_Bad;
				texture2D2 = WidgetsWork.WorkBoxBGTex_Mid;
				num2 = num / 14;
			}
			else {
				texture2D = WidgetsWork.WorkBoxBGTex_Mid;
				texture2D2 = WidgetsWork.WorkBoxBGTex_Excellent;
				num2 = (num - 14) / 6;
			}
			GUI.DrawTexture (rect, texture2D);
			GUI.set_color (new Color (1, 1, 1, num2));
			GUI.DrawTexture (rect, texture2D2);
			Passion passion = p.skills.MaxPassionOfRelevantSkillsFor (workDef);
			if (passion > 0) {
				GUI.set_color (new Color (1, 1, 1, 0.4));
				Rect rect2 = rect;
				rect2.set_xMin (rect.get_center ().x);
				rect2.set_yMin (rect.get_center ().y);
				if (passion == 1) {
					GUI.DrawTexture (rect2, WidgetsWork.PassionWorkboxMinorIcon);
				}
				else if (passion == 2) {
					GUI.DrawTexture (rect2, WidgetsWork.PassionWorkboxMajorIcon);
				}
			}
			GUI.set_color (Color.get_white ());
		}

		private static void DrawWorkBoxBackgroundForSquad (Rect rect, WorkTypeDef workDef)
		{
			float num = 6;
			Texture2D texture2D;
			Texture2D texture2D2;
			float num2;
			if (num <= 14) {
				texture2D = WidgetsWork.WorkBoxBGTex_Bad;
				texture2D2 = WidgetsWork.WorkBoxBGTex_Mid;
				num2 = num / 14;
			}
			else {
				texture2D = WidgetsWork.WorkBoxBGTex_Mid;
				texture2D2 = WidgetsWork.WorkBoxBGTex_Excellent;
				num2 = (num - 14) / 6;
			}
			GUI.DrawTexture (rect, texture2D);
			GUI.set_color (new Color (1, 1, 1, num2));
			GUI.DrawTexture (rect, texture2D2);
			GUI.set_color (Color.get_white ());
		}

		public static void DrawWorkBoxFor (Vector2 topLeft, Pawn p, WorkTypeDef wType)
		{
			if (p.story == null || p.workSettings == null || p.story.WorkTypeIsDisabled (wType)) {
				return;
			}
			Rect rect = new Rect (topLeft.x, topLeft.y, 25, 25);
			WidgetsWork.DrawWorkBoxBackground (rect, p, wType);
			if (Find.get_PlaySettings ().useWorkPriorities) {
				int priority = p.workSettings.GetPriority (wType);
				string text;
				if (priority > 0) {
					text = priority.ToString ();
				}
				else {
					text = string.Empty;
				}
				Text.set_Anchor (4);
				GUI.set_color (WidgetsWork.ColorOfPriority (priority, p.skills.MaxPassionOfRelevantSkillsFor (wType)));
				Rect rect2 = GenUI.ContractedBy (rect, -3);
				Widgets.Label (rect2, text);
				GUI.set_color (Color.get_white ());
				Text.set_Anchor (0);
				if (Event.get_current ().get_type () == null && Mouse.IsOver (rect)) {
					if (Event.get_current ().get_button () == 0) {
						int num = p.workSettings.GetPriority (wType) - 1;
						if (num < 0) {
							num = 4;
						}
						p.workSettings.SetPriority (wType, num);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.AmountIncrement);
					}
					if (Event.get_current ().get_button () == 1) {
						int num2 = p.workSettings.GetPriority (wType) + 1;
						if (num2 > 4) {
							num2 = 0;
						}
						p.workSettings.SetPriority (wType, num2);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.AmountDecrement);
					}
					Event.get_current ().Use ();
				}
			}
			else {
				int priority2 = p.workSettings.GetPriority (wType);
				if (priority2 > 0) {
					GUI.DrawTexture (rect, WidgetsWork.WorkBoxCheckTex);
				}
				if (Widgets.InvisibleButton (rect)) {
					if (p.workSettings.GetPriority (wType) > 0) {
						p.workSettings.SetPriority (wType, 0);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.CheckboxTurnedOff);
					}
					else {
						p.workSettings.SetPriority (wType, 3);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.CheckboxTurnedOn);
					}
				}
			}
		}

		public static void DrawWorkBoxForSquad (Vector2 topLeft, WorkTypeDef wType, SquadPriorities priorities, IEnumerable<Pawn> pawns)
		{
			Rect rect = new Rect (topLeft.x, topLeft.y, 25, 25);
			WidgetsWork.DrawWorkBoxBackgroundForSquad (rect, wType);
			if (Find.get_PlaySettings ().useWorkPriorities) {
				int priority = priorities.GetPriority (wType);
				string text;
				if (priority > 0) {
					text = priority.ToString ();
				}
				else {
					text = string.Empty;
				}
				Text.set_Anchor (4);
				GUI.set_color (Color.get_white ());
				Rect rect2 = GenUI.ContractedBy (rect, -3);
				Widgets.Label (rect2, text);
				GUI.set_color (Color.get_white ());
				Text.set_Anchor (0);
				if (Event.get_current ().get_type () == null && Mouse.IsOver (rect)) {
					if (Event.get_current ().get_button () == 0) {
						int num = priorities.GetPriority (wType) - 1;
						if (num < 0) {
							num = 4;
						}
						priorities.SetPriority (wType, num);
						WidgetsWork.SetPriorityForPawns (pawns, wType, num);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.AmountIncrement);
					}
					if (Event.get_current ().get_button () == 1) {
						int num2 = priorities.GetPriority (wType) + 1;
						if (num2 > 4) {
							num2 = 0;
						}
						priorities.SetPriority (wType, num2);
						WidgetsWork.SetPriorityForPawns (pawns, wType, num2);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.AmountDecrement);
					}
					Event.get_current ().Use ();
				}
			}
			else {
				int priority2 = priorities.GetPriority (wType);
				if (priority2 > 0) {
					GUI.DrawTexture (rect, WidgetsWork.WorkBoxCheckTex);
				}
				if (Widgets.InvisibleButton (rect)) {
					if (priorities.GetPriority (wType) > 0) {
						priorities.SetPriority (wType, 0);
						WidgetsWork.SetPriorityForPawns (pawns, wType, 0);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.CheckboxTurnedOff);
					}
					else {
						priorities.SetPriority (wType, 3);
						WidgetsWork.SetPriorityForPawns (pawns, wType, 3);
						SoundStarter.PlayOneShotOnCamera (SoundDefOf.CheckboxTurnedOn);
					}
				}
			}
		}

		private static void SetPriorityForPawns (IEnumerable<Pawn> pawns, WorkTypeDef workType, int num)
		{
			foreach (Pawn current in pawns) {
				if (current.story != null && current.workSettings != null && !current.story.WorkTypeIsDisabled (workType)) {
					current.workSettings.SetPriority (workType, num);
				}
			}
		}

		public static TipSignal TipForPawnWorker (Pawn p, WorkTypeDef wDef)
		{
			StringBuilder stringBuilder = new StringBuilder ();
			stringBuilder.AppendLine (wDef.gerundLabel);
			if (p.story.WorkTypeIsDisabled (wDef)) {
				stringBuilder.Append (Translator.Translate ("CannotDoThisWork", new object[] {
					p.get_NameStringShort ()
				}));
			}
			else {
				string text = string.Empty;
				if (wDef.relevantSkills.Count == 0) {
					text = Translator.Translate ("NoneBrackets");
				}
				else {
					foreach (SkillDef current in wDef.relevantSkills) {
						text = text + current.skillLabel + ", ";
					}
					text = text.Substring (0, text.Length - 2);
				}
				stringBuilder.AppendLine (Translator.Translate ("RelevantSkills", new object[] {
					text,
					p.skills.AverageOfRelevantSkillsFor (wDef).ToString (),
					20
				}));
				stringBuilder.AppendLine ();
				stringBuilder.Append (wDef.description);
			}
			return stringBuilder.ToString ();
		}
	}
}
using System;
using Verse;

namespace EdB.Interface
{
	public static class WindowStackExtensions
	{
		//
		// Static Methods
		//
		public static Window Top (this WindowStack stack)
		{
			if (stack.get_Count () > 0) {
				return stack.get_Windows () [0];
			}
			return null;
		}
	}
}
