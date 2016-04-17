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
		private const float TopPadding = 20f;

		private const float ThingIconSize = 28f;

		private const float ThingRowHeight = 30f;

		private const float ThingLeftX = 36f;

		private const float InfoRectHeight = 100f;

		private const float SeparatorLabelHeight = 20f;

		private static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		private static readonly Vector2 PanelSize = new Vector2(TabDrawer.TabPanelSize.x, 480f);

		private Vector2 scrollPosition = Vector2.zero;

		private float scrollViewHeight;

		public static float LineHeight = 25f;

		private bool CanEdit
		{
			get
			{
				return this.SelPawnForGear.IsColonistPlayerControlled;
			}
		}

		private Pawn SelPawnForGear
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.innerPawn;
				}
				throw new InvalidOperationException("Gear tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		public PreferenceTabBrowseButtons PreferenceTabBrowseButtons
		{
			get;
			set;
		}

		public ITab_Pawn_Gear_Alternate(PreferenceTabBrowseButtons preferenceTabBrowseButtons)
		{
			this.size = ITab_Pawn_Gear_Alternate.PanelSize;
			this.PreferenceTabBrowseButtons = preferenceTabBrowseButtons;
		}

		private void DrawThingRow(ref float y, float width, Thing thing)
		{
			Rect rect = new Rect(36f, y + 2f, width - 38f, 28f);
			string text = thing.LabelCap;
			if (thing is Apparel && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced((Apparel)thing))
			{
				text = text + ", " + "ApparelForcedLower".Translate();
			}
			float num = Text.CalcHeight(text, rect.width);
			rect.height = ((num >= 28f) ? num : 28f);
			Rect rect2 = new Rect(0f, y, width, rect.height + 4f);
			if (rect2.Contains(Event.current.mousePosition))
			{
				GUI.color = ITab_Pawn_Gear_Alternate.HighlightColor;
				GUI.DrawTexture(rect2, TexUI.HighlightTex);
			}
			if (Widgets.InvisibleButton(rect2) && Event.current.button == 1)
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("ThingInfo".Translate(), delegate
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing));
				}, MenuOptionPriority.Medium, null, null));
				if (this.CanEdit)
				{
					Action action = null;
					ThingWithComps eq = thing as ThingWithComps;
					Apparel ap = thing as Apparel;
					if (ap != null)
					{
						Apparel unused;
						action = delegate
						{
							this.SelPawnForGear.apparel.TryDrop(ap, out unused, this.SelPawnForGear.Position, true);
						};
					}
					else if (eq != null && this.SelPawnForGear.equipment.AllEquipment.Contains(eq))
					{
						ThingWithComps unused;
						action = delegate
						{
							this.SelPawnForGear.equipment.TryDropEquipment(eq, out unused, this.SelPawnForGear.Position, true);
						};
					}
					else if (!thing.def.destroyOnDrop)
					{
						Thing unused;
						action = delegate
						{
							this.SelPawnForGear.inventory.container.TryDrop(thing, this.SelPawnForGear.Position, ThingPlaceMode.Near, out unused);
						};
					}
					list.Add(new FloatMenuOption("DropThing".Translate(), action, MenuOptionPriority.Medium, null, null));
				}
				FloatMenu window = new FloatMenu(list, thing.LabelCap, false, false);
				Find.WindowStack.Add(window);
			}
			if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
			{
				Widgets.ThingIcon(new Rect(2f, y + 2f, 28f, 28f), thing);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_Gear_Alternate.ThingLabelColor;
			rect.y += 1f;
			Widgets.Label(rect, text);
			y += rect.height + 4f;
		}

		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 8f, this.size.x, this.size.y - 8f);
			Rect rect2 = rect.ContractedBy(24f);
			Rect position = new Rect(rect2.x, rect2.y, rect2.width, rect2.height - 24f);
			try
			{
				GUI.BeginGroup(position);
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
				Rect outRect = new Rect(0f, 0f, position.width, position.height);
				Rect viewRect = new Rect(0f, 0f, position.width - 16f, this.scrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);
				float num = 0f;
				bool flag = false;
				if (this.SelPawnForGear.equipment != null)
				{
					num += TabDrawer.DrawHeader(0f, num, viewRect.width, "Equipment".Translate(), true, TextAnchor.UpperLeft);
					foreach (ThingWithComps current in this.SelPawnForGear.equipment.AllEquipment)
					{
						this.DrawThingRow(ref num, viewRect.width, current);
						flag = true;
					}
					if (!flag)
					{
						num += 4f;
						Rect rect3 = new Rect(0f, num, viewRect.width, 24f);
						GUI.color = TabDrawer.SeparatorColor;
						Widgets.Label(rect3, "NoneLower".Translate());
						GUI.color = TabDrawer.TextColor;
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				flag = false;
				if (this.SelPawnForGear.apparel != null)
				{
					num += 10f;
					num += TabDrawer.DrawHeader(0f, num, viewRect.width, "Apparel".Translate(), true, TextAnchor.UpperLeft);
					num += 4f;
					foreach (Apparel current2 in from ap in this.SelPawnForGear.apparel.WornApparel
					orderby ap.def.apparel.bodyPartGroups[0].listOrder descending
					select ap)
					{
						this.DrawThingRow(ref num, viewRect.width, current2);
						flag = true;
					}
					if (!flag)
					{
						num += 4f;
						Rect rect4 = new Rect(0f, num, viewRect.width, 24f);
						GUI.color = TabDrawer.SeparatorColor;
						Widgets.Label(rect4, "NoneLower".Translate());
						GUI.color = TabDrawer.TextColor;
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				flag = false;
				if (this.SelPawnForGear.inventory != null)
				{
					num += 10f;
					num += TabDrawer.DrawHeader(0f, num, viewRect.width, "Inventory".Translate(), true, TextAnchor.UpperLeft);
					foreach (Thing current3 in this.SelPawnForGear.inventory.container)
					{
						this.DrawThingRow(ref num, viewRect.width, current3);
						flag = true;
					}
					if (!flag)
					{
						num += 4f;
						Rect rect5 = new Rect(0f, num, viewRect.width, 24f);
						GUI.color = TabDrawer.SeparatorColor;
						Widgets.Label(rect5, "NoneLower".Translate());
						GUI.color = TabDrawer.TextColor;
						num += ITab_Pawn_Gear_Alternate.LineHeight;
					}
				}
				if (Event.current.type == EventType.Layout)
				{
					this.scrollViewHeight = num + 8f;
				}
				Widgets.EndScrollView();
			}
			finally
			{
				GUI.EndGroup();
			}
			if (this.PreferenceTabBrowseButtons != null && this.PreferenceTabBrowseButtons.Value)
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				if (selPawnForGear != null)
				{
					BrowseButtonDrawer.DrawBrowseButtons(this.size, selPawnForGear);
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
