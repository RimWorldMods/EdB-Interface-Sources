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
		private static readonly Vector2 WinSize = new Vector2(TabDrawer.TabPanelSize.x, 480f);

		private ScrollView scrollView = new ScrollView();

		protected FieldInfo mouseoverBillField;

		public ITab_Bills_Alternate()
		{
			this.size = ITab_Bills_Alternate.WinSize;
			this.mouseoverBillField = typeof(ITab_Bills).GetField("mouseoverBill", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		protected override void FillTab()
		{
			ConceptDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.GuiFrame);
			Rect rect = new Rect(0f, 0f, ITab_Bills_Alternate.WinSize.x, ITab_Bills_Alternate.WinSize.y).ContractedBy(10f);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < base.SelTable.def.AllRecipes.Count; i++)
				{
					RecipeDef recipe = base.SelTable.def.AllRecipes[i];
					list.Add(new FloatMenuOption(recipe.LabelCap, delegate
					{
						if (!Find.ListerPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
						{
							Bill.CreateNoPawnsWithSkillDialog(recipe);
						}
						Bill bill = recipe.MakeNewBill();
						this.SelTable.billStack.AddBill(bill);
					}, MenuOptionPriority.Medium, null, null));
				}
				return list;
			};
			Bill value = BillDrawer.DrawListing(base.SelTable.billStack, rect, recipeOptionsMaker, this.scrollView);
			this.mouseoverBillField.SetValue(this, value);
		}
	}
}
