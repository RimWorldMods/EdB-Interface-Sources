using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EdB.Interface
{
	public class MainTabWindow_Architect : RimWorld.MainTabWindow_Architect
	{
		private const float ButHeight = 32f;

		private List<ArchitectCategoryTab> desPanelsCached;

		public new ArchitectCategoryTab selectedDesPanel;

		public MainTabWindow_Architect()
		{
			this.CacheDesPanels();
		}

		private void CacheDesPanels()
		{
			this.desPanelsCached = new List<ArchitectCategoryTab>();
			foreach (DesignationCategoryDef current in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				this.desPanelsCached.Add(new ArchitectCategoryTab(current));
			}
		}

		protected void ClickedCategory(ArchitectCategoryTab Pan)
		{
			if (this.selectedDesPanel == Pan)
			{
				this.selectedDesPanel = null;
			}
			else
			{
				this.selectedDesPanel = Pan;
			}
			SoundDefOf.ArchitectCategorySelect.PlayOneShotOnCamera();
		}

		protected void BaseDoWindowContents(Rect inRect)
		{
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.currentWindowRect.x = 0f;
			}
			else
			{
				this.currentWindowRect.x = (float)Screen.width - this.currentWindowRect.width;
			}
			this.currentWindowRect.y = (float)(Screen.height - 35) - this.currentWindowRect.height;
			if (this.def.concept != null)
			{
				ConceptDatabase.KnowledgeDemonstrated(this.def.concept, KnowledgeAmount.GuiFrame);
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			this.BaseDoWindowContents(inRect);
			Text.Font = GameFont.Small;
			float num = inRect.width / 2f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.desPanelsCached.Count; i++)
			{
				Rect rect = new Rect(num2 * num, num3 * 32f, num, 32f);
				rect.height += 1f;
				if (num2 == 0f)
				{
					rect.width += 1f;
				}
				if (WidgetsSubtle.ButtonSubtle(rect, this.desPanelsCached[i].def.LabelCap, 0f, 8f, SoundDefOf.MouseoverButtonCategory))
				{
					this.ClickedCategory(this.desPanelsCached[i]);
				}
				num2 += 1f;
				if (num2 > 1f)
				{
					num2 = 0f;
					num3 += 1f;
				}
			}
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedDesPanel != null)
			{
				this.selectedDesPanel.DesignationTabOnGUI();
			}
		}

		public Designator ReplaceDesignator(Type searchingFor, Designator replacement)
		{
			DesignationCategoryDef designationCategoryDef = null;
			Designator designator = null;
			foreach (DesignationCategoryDef current in from dc in DefDatabase<DesignationCategoryDef>.AllDefs
			orderby dc.order descending
			select dc)
			{
				foreach (Designator current2 in current.resolvedDesignators)
				{
					if (current2.GetType().Equals(searchingFor))
					{
						designator = current2;
						designationCategoryDef = current;
						break;
					}
				}
				if (designator != null)
				{
					break;
				}
			}
			if (designator == null)
			{
				return null;
			}
			int index = designationCategoryDef.resolvedDesignators.IndexOf(designator);
			designationCategoryDef.resolvedDesignators[index] = replacement;
			return designator;
		}
	}
}
