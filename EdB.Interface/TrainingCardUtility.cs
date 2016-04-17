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
		private const float RowHeight = 28f;

		private const float InfoHeaderHeight = 50f;

		private static FieldInfo stepsField;

		static TrainingCardUtility()
		{
			TrainingCardUtility.stepsField = typeof(Pawn_TrainingTracker).GetField("steps", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public static int GetSteps(Pawn_TrainingTracker training, TrainableDef td)
		{
			DefMap<TrainableDef, int> defMap = (DefMap<TrainableDef, int>)TrainingCardUtility.stepsField.GetValue(training);
			return defMap[td];
		}

		public static void DrawTrainingCard(Rect rect, Pawn pawn)
		{
			try
			{
				GUI.BeginGroup(rect);
				Rect rect2 = new Rect(0f, 0f, rect.width, 25f);
				string text = "Master".Translate() + ": ";
				Vector2 vector = Text.CalcSize(text);
				Widgets.Label(rect2, text);
				Rect rect3 = new Rect(rect2.x + vector.x + 6f, rect2.y, 200f, rect2.height);
				if (pawn.training.IsCompleted(TrainableDefOf.Obedience))
				{
					rect3.y -= 1f;
					string label = TrainableUtility.MasterString(pawn);
					if (Widgets.TextButton(rect3, label, true, false))
					{
						TrainableUtility.OpenMasterSelectMenu(pawn);
					}
				}
				else
				{
					GUI.color = new Color(0.7f, 0.7f, 0.7f);
					Widgets.Label(rect3, "None".Translate());
					GUI.color = Color.white;
				}
				List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
				int count = trainableDefsInListOrder.Count;
				float height = (float)(count * 28 + 20);
				Rect rect4 = new Rect(0f, rect2.y + 35f, rect.width, height);
				TabDrawer.DrawBox(rect4);
				Rect rect5 = rect4.ContractedBy(10f);
				rect5.height = 28f;
				for (int i = 0; i < trainableDefsInListOrder.Count; i++)
				{
					if (TrainingCardUtility.TryDrawTrainableRow(rect5, pawn, trainableDefsInListOrder[i]))
					{
						rect5.y += 28f;
					}
				}
				Text.Anchor = TextAnchor.UpperRight;
				string label2 = "TrainableIntelligence".Translate() + ": " + pawn.RaceProps.trainableIntelligence.GetLabel();
				Widgets.Label(new Rect(0f, rect4.y + rect4.height + 5f, rect4.width - 2f, 25f), label2);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		private static void SetWantedRecursive(TrainableDef td, Pawn pawn, bool checkOn)
		{
			pawn.training.SetWanted(td, checkOn);
			if (checkOn)
			{
				if (td.prerequisites != null)
				{
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						TrainingCardUtility.SetWantedRecursive(td.prerequisites[i], pawn, true);
					}
				}
			}
			else
			{
				IEnumerable<TrainableDef> enumerable = from t in DefDatabase<TrainableDef>.AllDefsListForReading
				where t.prerequisites != null && t.prerequisites.Contains(td)
				select t;
				foreach (TrainableDef current in enumerable)
				{
					TrainingCardUtility.SetWantedRecursive(current, pawn, false);
				}
			}
		}

		private static bool TryDrawTrainableRow(Rect rect, Pawn pawn, TrainableDef td)
		{
			bool flag = pawn.training.IsCompleted(td);
			bool flag2;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(td, out flag2);
			if (!flag2)
			{
				return false;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = rect;
			rect2.width -= 50f;
			rect2.xMin += (float)td.indent * 15f;
			Rect rect3 = rect;
			rect3.xMin = rect3.xMax - 50f + 17f;
			if (!flag)
			{
				bool wanted = pawn.training.GetWanted(td);
				bool flag3 = wanted;
				Widgets.LabelCheckbox(rect2, td.LabelCap, ref wanted, !canTrain.Accepted);
				if (wanted != flag3)
				{
					ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTraining, KnowledgeAmount.Total);
					TrainingCardUtility.SetWantedRecursive(td, pawn, wanted);
				}
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect2, td.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (flag)
			{
				GUI.color = Color.green;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, TrainingCardUtility.GetSteps(pawn.training, td) + " / " + td.steps);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Game.GodMode && !pawn.training.IsCompleted(td))
			{
				Rect rect4 = rect3;
				rect4.yMin = rect4.yMax - 10f;
				rect4.xMin = rect4.xMax - 10f;
				if (Widgets.TextButton(rect4, "+", true, false))
				{
					pawn.training.Train(td, Find.ListerPawns.FreeColonistsSpawned.RandomElement<Pawn>());
				}
			}
			TooltipHandler.TipRegion(rect, delegate
			{
				string text = td.LabelCap + "\n\n" + td.description;
				if (!canTrain.Accepted)
				{
					text = text + "\n\n" + canTrain.Reason;
				}
				else if (!td.prerequisites.NullOrEmpty<TrainableDef>())
				{
					text += "\n";
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						if (!pawn.training.IsCompleted(td.prerequisites[i]))
						{
							text = text + "\n" + "TrainingNeedsPrerequisite".Translate(new object[]
							{
								td.prerequisites[i].LabelCap
							});
						}
					}
				}
				return text;
			}, (int)(rect.y * 612f));
			GUI.color = Color.white;
			return true;
		}
	}
}
