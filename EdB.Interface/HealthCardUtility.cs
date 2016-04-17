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
		private const float ThoughtLevelHeight = 25f;

		private const float IconSize = 20f;

		private const float ThoughtLevelSpacing = 4f;

		private static readonly Color SeverePainColor = new Color(0.9f, 0.5f, 0f);

		private static readonly Color MediumPainColor = new Color(0.9f, 0.9f, 0f);

		private static readonly Color StaticHighlightColor = new Color(0.75f, 0.75f, 0.85f, 1f);

		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		private static readonly Texture2D TreatedWellIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TreatedWell", true);

		private static readonly Texture2D TreatedPoorIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TreatedPoorly", true);

		private static readonly Texture2D BleedingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Bleeding", true);

		private static bool showHediffsDebugInfo = false;

		private static Vector2 scrollPosition = Vector2.zero;

		private static float scrollViewHeight = 0f;

		private static bool highlight = true;

		private static Vector2 billsScrollPosition = Vector2.zero;

		private static float billsScrollHeight = 1000f;

		protected MethodInfo visibleHediffGroupsInOrderMethod;

		protected MethodInfo visibleHediffsMethod;

		public HealthCardUtility()
		{
			this.visibleHediffGroupsInOrderMethod = typeof(RimWorld.HealthCardUtility).GetMethod("VisibleHediffGroupsInOrder", BindingFlags.Static | BindingFlags.NonPublic);
			this.visibleHediffsMethod = typeof(RimWorld.HealthCardUtility).GetMethod("VisibleHediffs", BindingFlags.Static | BindingFlags.NonPublic);
		}

		private static void DoRightRowHighlight(Rect rowRect)
		{
			if (HealthCardUtility.highlight)
			{
				GUI.color = HealthCardUtility.StaticHighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
			HealthCardUtility.highlight = !HealthCardUtility.highlight;
			if (rowRect.Contains(Event.current.mousePosition))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(rowRect, TexUI.HighlightTex);
			}
		}

		public void DrawHediffRow(Rect rect, Pawn pawn, IEnumerable<Hediff> diffs, ref float curY)
		{
			float num = 6f;
			float num2 = rect.width * 0.4f;
			float width = rect.width - num2 - 20f;
			num2 -= num;
			float a;
			if (diffs.First<Hediff>().Part == null)
			{
				a = Text.CalcHeight("WholeBody".Translate(), num2);
			}
			else
			{
				a = Text.CalcHeight(diffs.First<Hediff>().Part.def.LabelCap, num2);
			}
			float b = 0f;
			float num3 = curY;
			float num4 = 0f;
			foreach (IGrouping<int, Hediff> current in from x in diffs
			group x by x.UIGroupKey)
			{
				int num5 = current.Count<Hediff>();
				string text = current.First<Hediff>().LabelCap;
				if (num5 != 1)
				{
					text = text + " x" + num5.ToString();
				}
				num4 += Text.CalcHeight(text, width);
			}
			b = num4;
			Rect rect2 = new Rect(0f, curY, rect.width, Mathf.Max(a, b));
			HealthCardUtility.DoRightRowHighlight(rect2);
			StringBuilder stringBuilder = new StringBuilder();
			if (diffs.First<Hediff>().Part != null)
			{
				stringBuilder.Append(diffs.First<Hediff>().Part.def.LabelCap + ": ");
				GUI.color = HealthUtility.GetPartConditionLabel(pawn, diffs.First<Hediff>().Part).Second;
				Widgets.Label(new Rect(num, curY + 1f, num2, 100f), new GUIContent(diffs.First<Hediff>().Part.def.LabelCap));
				stringBuilder.Append(" " + pawn.health.hediffSet.GetPartHealth(diffs.First<Hediff>().Part).ToString() + " / " + diffs.First<Hediff>().Part.def.GetMaxHealth(pawn).ToString());
			}
			else
			{
				stringBuilder.Append("WholeBody".Translate());
				GUI.color = HealthUtility.DarkRedColor;
				Widgets.Label(new Rect(num, curY + 1f, num2, 100f), new GUIContent("WholeBody".Translate()));
			}
			GUI.color = Color.white;
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("------------------");
			foreach (IGrouping<int, Hediff> current2 in from x in diffs
			group x by x.UIGroupKey)
			{
				Hediff hediff = current2.First<Hediff>();
				int num6 = 0;
				bool flag = false;
				Texture2D texture2D = null;
				foreach (Hediff current3 in current2)
				{
					num6++;
					Hediff_Injury hediff_Injury = current3 as Hediff_Injury;
					if (hediff_Injury != null && hediff_Injury.IsTended() && !hediff_Injury.IsOld())
					{
						if (hediff_Injury.IsTendedWell())
						{
							texture2D = HealthCardUtility.TreatedWellIcon;
						}
						else
						{
							texture2D = HealthCardUtility.TreatedPoorIcon;
						}
					}
					float bleedRate = current3.BleedRate;
					if ((double)bleedRate > 1E-05)
					{
						flag = true;
					}
					bool flag2 = HealthCardUtility.showHediffsDebugInfo && !current3.DebugString.NullOrEmpty();
					string damageLabel = current3.DamageLabel;
					if (!current3.Label.NullOrEmpty() || !damageLabel.NullOrEmpty() || !current3.CapMods.NullOrEmpty<PawnCapacityModifier>() || flag2)
					{
						stringBuilder.Append(current3.LabelCap);
						if (!damageLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + damageLabel);
						}
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(current3.TipString.TrimEndNewlines().Indented());
						if (flag2)
						{
							stringBuilder.AppendLine(current3.DebugString.TrimEndNewlines().Indented());
						}
					}
				}
				string text2 = hediff.LabelCap;
				if (num6 != 1)
				{
					text2 = text2 + " x" + num6.ToString();
				}
				GUI.color = hediff.LabelColor;
				float num7 = Text.CalcHeight(text2, width);
				Rect rect3 = new Rect(num + num2, curY + 1f, width, num7);
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Rect position = new Rect(rect2.xMax - 20f, curY, 20f, 20f);
				if (flag)
				{
					GUI.DrawTexture(position, HealthCardUtility.BleedingIcon);
				}
				if (texture2D != null)
				{
					GUI.DrawTexture(position, texture2D);
				}
				curY += num7;
			}
			GUI.color = Color.white;
			curY = num3 + Mathf.Max(a, b);
			TooltipHandler.TipRegion(rect2, new TipSignal(stringBuilder.ToString().TrimEnd(new char[0]), (int)curY + 7857));
		}

		private float DrawLeftRow(Rect leftRect, float curY, string leftLabel, string rightLabel, Color rightLabelColor, string tipLabel)
		{
			Rect position = new Rect(0f, curY, leftRect.width, 20f);
			if (position.Contains(Event.current.mousePosition))
			{
				GUI.color = HealthCardUtility.HighlightColor;
				GUI.DrawTexture(position, TexUI.HighlightTex);
			}
			float num = leftRect.width * 0.5f;
			float num2 = leftRect.width - num;
			GUI.color = TabDrawer.TextColor;
			float num3 = 4f;
			Widgets.Label(new Rect(num3, curY, num - num3, 30f), new GUIContent(leftLabel));
			GUI.color = rightLabelColor;
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(num, curY, num2 - num3, 30f), new GUIContent(rightLabel));
			if (tipLabel != null)
			{
				TooltipHandler.TipRegion(new Rect(0f, curY, leftRect.width, 20f), new TipSignal(tipLabel));
			}
			Text.Anchor = anchor;
			curY += 20f;
			return curY;
		}

		public float DrawMedOperationsTab(Rect leftRect, Pawn pawn, Thing thingForMedBills, float curY)
		{
			curY += 2f;
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (RecipeDef current in thingForMedBills.def.AllRecipes)
				{
					if (current.AvailableNow)
					{
						IEnumerable<ThingDef> enumerable = current.PotentiallyMissingIngredients(null);
						if (!enumerable.Any((ThingDef x) => x.isBodyPartOrImplant))
						{
							IEnumerable<BodyPartRecord> partsToApplyOn = current.Worker.GetPartsToApplyOn(pawn, current);
							if (partsToApplyOn.Any<BodyPartRecord>())
							{
								foreach (BodyPartRecord current2 in partsToApplyOn)
								{
									RecipeDef localRecipe = current;
									BodyPartRecord localPart = current2;
									string text;
									if (localRecipe == RecipeDefOf.RemoveBodyPart)
									{
										text = RimWorld.HealthCardUtility.RemoveBodyPartSpecialLabel(pawn, current2);
									}
									else
									{
										text = localRecipe.LabelCap;
									}
									if (!current.hideBodyPartNames)
									{
										text = text + " (" + current2.def.label + ")";
									}
									Action action = null;
									if (enumerable.Any<ThingDef>())
									{
										text += " (";
										bool flag = true;
										foreach (ThingDef current3 in enumerable)
										{
											if (!flag)
											{
												text += ", ";
											}
											flag = false;
											text += "MissingMedicalBillIngredient".Translate(new object[]
											{
												current3.label
											});
										}
										text += ")";
									}
									else
									{
										action = delegate
										{
											if (!Find.ListerPawns.FreeColonists.Any((Pawn col) => localRecipe.PawnSatisfiesSkillRequirements(col)))
											{
												Bill.CreateNoPawnsWithSkillDialog(localRecipe);
											}
											Pawn pawn2 = thingForMedBills as Pawn;
											if (pawn2 != null && !pawn.InBed() && pawn.RaceProps.Humanlike)
											{
												if (!Find.ListerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && (x as Building_Bed).Medical))
												{
													Messages.Message("MessageNoMedicalBeds".Translate(), MessageSound.Negative);
												}
											}
											Bill_Medical bill_Medical = new Bill_Medical(localRecipe);
											pawn2.BillStack.AddBill(bill_Medical);
											bill_Medical.Part = localPart;
											if (pawn2.Faction != null && !pawn2.Faction.def.hidden && !pawn2.Faction.HostileTo(Faction.OfColony) && localRecipe.Worker.IsViolationOnPawn(pawn2, localPart, Faction.OfColony))
											{
												Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(new object[]
												{
													pawn2.Faction
												}), MessageSound.Negative);
											}
										};
									}
									list.Add(new FloatMenuOption(text, action, MenuOptionPriority.Medium, null, null));
								}
							}
						}
					}
				}
				return list;
			};
			Rect rect = new Rect(leftRect.x, curY, leftRect.width, leftRect.height - curY - 3f);
			this.DrawListing(pawn.BillStack, rect, recipeOptionsMaker, ref HealthCardUtility.billsScrollPosition, ref HealthCardUtility.billsScrollHeight);
			return curY;
		}

		public Bill DrawListing(BillStack billStack, Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, 150f, 29f);
			if (billStack.Count < 10)
			{
				if (Widgets.TextButton(rect2, "AddBill".Translate(), true, false))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker(), false));
				}
			}
			else
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
				Button.TextButton(rect2, "AddBill".Translate(), true, false, false);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
			float num = 0f;
			for (int i = 0; i < billStack.Count; i++)
			{
				Bill bill = billStack[i];
				Rect rect3 = BillDrawer.DrawMedicalBill(billStack, bill, 0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && rect3.Contains(Event.current.mousePosition))
				{
					result = bill;
				}
				num += rect3.height;
				if (i < billStack.Count - 1)
				{
					num += 6f;
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			return result;
		}

		public float DrawStatus(Rect leftRect, Pawn pawn, float curY, bool showBloodLoss)
		{
			Text.Font = GameFont.Small;
			float bleedingRate = pawn.health.hediffSet.BleedingRate;
			if ((double)bleedingRate > 0.01)
			{
				string rightLabel = string.Concat(new string[]
				{
					bleedingRate.ToStringPercent(),
					"/",
					"Day".Translate()
				});
				curY = this.DrawLeftRow(leftRect, curY, "BleedingRate".Translate(), rightLabel, HealthCardUtility.GetBleedingRateColor(bleedingRate), null);
			}
			if (pawn.def.race.isFlesh)
			{
				Pair<string, Color> painLabel = RimWorld.HealthCardUtility.GetPainLabel(pawn);
				string tipLabel = "PainLevel".Translate() + ": " + (pawn.health.hediffSet.Pain * 100f).ToString("F0") + "%";
				curY = this.DrawLeftRow(leftRect, curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, tipLabel);
			}
			if (!pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act)
				{
					if (PawnCapacityUtility.BodyCanEverDoActivity(pawn.RaceProps.body, current))
					{
						Pair<string, Color> efficiencyLabel = RimWorld.HealthCardUtility.GetEfficiencyLabel(pawn, current);
						string tipLabel2 = "Efficiency".Translate() + ": " + (pawn.health.capacities.GetEfficiency(current) * 100f).ToString("F0") + "%";
						curY = this.DrawLeftRow(leftRect, curY, current.GetLabelFor(pawn.RaceProps.isFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, tipLabel2);
					}
				}
			}
			return curY;
		}

		public void DrawInjuries(Rect leftRect, Pawn pawn, bool showBloodLoss)
		{
			float num = 0f;
			Rect position = new Rect(leftRect.x, num, leftRect.width, leftRect.height - num);
			GUI.BeginGroup(position);
			Rect outRect = new Rect(0f, 0f, position.width, position.height);
			Rect viewRect = new Rect(0f, 0f, position.width - 16f, HealthCardUtility.scrollViewHeight);
			Rect rect = new Rect(position.x, position.y, position.width - 16f, position.height);
			GUI.color = Color.white;
			Widgets.BeginScrollView(outRect, ref HealthCardUtility.scrollPosition, viewRect);
			float num2 = 0f;
			HealthCardUtility.highlight = true;
			bool flag = false;
			foreach (IGrouping<BodyPartRecord, Hediff> current in ((IEnumerable<IGrouping<BodyPartRecord, Hediff>>)this.visibleHediffGroupsInOrderMethod.Invoke(null, new object[]
			{
				pawn,
				showBloodLoss
			})))
			{
				flag = true;
				this.DrawHediffRow(rect, pawn, current, ref num2);
			}
			if (!flag)
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(new Rect(0f, 0f, position.width, 30f), new GUIContent("NoInjuries".Translate()));
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = num2;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		public float DrawOverviewTab(Rect leftRect, Pawn pawn, float curY, bool showBloodLoss)
		{
			curY += TabDrawer.DrawHeader(0f, curY, leftRect.width, "EdB.Status".Translate(), true, TextAnchor.UpperLeft);
			curY += 4f;
			Text.Font = GameFont.Small;
			float bleedingRate = pawn.health.hediffSet.BleedingRate;
			if ((double)bleedingRate > 0.01)
			{
				string rightLabel = string.Concat(new string[]
				{
					bleedingRate.ToStringPercent(),
					"/",
					"Day".Translate()
				});
				curY = this.DrawLeftRow(leftRect, curY, "BleedingRate".Translate(), rightLabel, HealthCardUtility.GetBleedingRateColor(bleedingRate), null);
			}
			if (pawn.def.race.isFlesh)
			{
				Pair<string, Color> painLabel = RimWorld.HealthCardUtility.GetPainLabel(pawn);
				string tipLabel = "PainLevel".Translate() + ": " + (pawn.health.hediffSet.Pain * 100f).ToString("F0") + "%";
				curY = this.DrawLeftRow(leftRect, curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, tipLabel);
			}
			if (!pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act)
				{
					if (PawnCapacityUtility.BodyCanEverDoActivity(pawn.RaceProps.body, current))
					{
						Pair<string, Color> efficiencyLabel = RimWorld.HealthCardUtility.GetEfficiencyLabel(pawn, current);
						string tipLabel2 = "Efficiency".Translate() + ": " + (pawn.health.capacities.GetEfficiency(current) * 100f).ToString("F0") + "%";
						curY = this.DrawLeftRow(leftRect, curY, current.GetLabelFor(pawn.RaceProps.isFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, tipLabel2);
					}
				}
			}
			curY += 16f;
			curY += TabDrawer.DrawHeader(0f, curY, leftRect.width, "EdB.Injuries".Translate(), true, TextAnchor.UpperLeft);
			curY += 4f;
			Rect position = new Rect(leftRect.x, curY, leftRect.width, leftRect.height - curY);
			GUI.BeginGroup(position);
			Rect outRect = new Rect(0f, 0f, position.width, position.height);
			Rect viewRect = new Rect(0f, 0f, position.width - 16f, HealthCardUtility.scrollViewHeight);
			Rect rect = new Rect(position.x, position.y, position.width - 16f, position.height);
			GUI.color = Color.white;
			Widgets.BeginScrollView(outRect, ref HealthCardUtility.scrollPosition, viewRect);
			float num = 0f;
			HealthCardUtility.highlight = true;
			bool flag = false;
			foreach (IGrouping<BodyPartRecord, Hediff> current2 in ((IEnumerable<IGrouping<BodyPartRecord, Hediff>>)this.visibleHediffGroupsInOrderMethod.Invoke(null, new object[]
			{
				pawn,
				showBloodLoss
			})))
			{
				flag = true;
				this.DrawHediffRow(rect, pawn, current2, ref num);
			}
			if (!flag)
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(new Rect(0f, 0f, position.width, 30f), new GUIContent("NoInjuries".Translate()));
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				HealthCardUtility.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
			return curY;
		}

		public static Color GetBleedingRateColor(float rate)
		{
			Color result = Color.white;
			if ((double)rate < 0.15)
			{
				result = HealthCardUtility.MediumPainColor;
			}
			else if ((double)rate < 0.4)
			{
				result = HealthCardUtility.MediumPainColor;
			}
			else if ((double)rate < 0.8)
			{
				result = HealthCardUtility.SeverePainColor;
			}
			else
			{
				result = HealthUtility.DarkRedColor;
			}
			return result;
		}

		public float DrawOverviewTab(Rect leftRect, Pawn pawn, float curY)
		{
			curY += 4f;
			if (pawn.playerSettings != null && !pawn.Dead)
			{
				Rect rect = new Rect(0f, curY, 140f, 28f);
				MedicalCareUtility.MedicalCareSetter(rect, ref pawn.playerSettings.medCare);
				curY += 32f;
			}
			Text.Font = GameFont.Small;
			if (pawn.def.race.isFlesh)
			{
				Pair<string, Color> painLabel = RimWorld.HealthCardUtility.GetPainLabel(pawn);
				string tipLabel = "PainLevel".Translate() + ": " + (pawn.health.hediffSet.Pain * 100f).ToString("F0") + "%";
				curY = this.DrawLeftRow(leftRect, curY, "PainLevel".Translate(), painLabel.First, painLabel.Second, tipLabel);
			}
			if (!pawn.Dead)
			{
				IEnumerable<PawnCapacityDef> source;
				if (pawn.def.race.Humanlike)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnHumanlikes
					select x;
				}
				else if (pawn.def.race.Animal)
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnAnimals
					select x;
				}
				else
				{
					source = from x in DefDatabase<PawnCapacityDef>.AllDefs
					where x.showOnMechanoids
					select x;
				}
				foreach (PawnCapacityDef current in from act in source
				orderby act.listOrder
				select act)
				{
					if (PawnCapacityUtility.BodyCanEverDoActivity(pawn.RaceProps.body, current))
					{
						Pair<string, Color> efficiencyLabel = RimWorld.HealthCardUtility.GetEfficiencyLabel(pawn, current);
						string tipLabel2 = "Efficiency".Translate() + ": " + (pawn.health.capacities.GetEfficiency(current) * 100f).ToString("F0") + "%";
						curY = this.DrawLeftRow(leftRect, curY, current.GetLabelFor(pawn.RaceProps.isFlesh, pawn.RaceProps.Humanlike).CapitalizeFirst(), efficiencyLabel.First, efficiencyLabel.Second, tipLabel2);
					}
				}
			}
			return curY;
		}
	}
}
