using RimWorld;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class ITab_Art_Alternate : ITab_Art
	{
		protected MethodInfo selectedCompArtGetter;

		protected static string cachedImageDescription;

		protected static CompArt cachedImageSource;

		protected static TaleReference cachedTaleRef;

		public ITab_Art_Alternate()
		{
			this.size = new Vector2(TabDrawer.TabPanelSize.x, 320f);
			PropertyInfo property = typeof(ITab_Art).GetProperty("SelectedCompArt", BindingFlags.Instance | BindingFlags.NonPublic);
			this.selectedCompArtGetter = property.GetGetMethod(true);
		}

		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(20f);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			CompArt compArt = (CompArt)this.selectedCompArtGetter.Invoke(this, null);
			Widgets.Label(rect2, compArt.Title);
			if (ITab_Art_Alternate.cachedImageSource != compArt || ITab_Art_Alternate.cachedTaleRef != compArt.TaleRef)
			{
				ITab_Art_Alternate.cachedImageDescription = compArt.ImageDescription();
				ITab_Art_Alternate.cachedImageSource = compArt;
				ITab_Art_Alternate.cachedTaleRef = compArt.TaleRef;
			}
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art_Alternate.cachedImageDescription);
		}
	}
}
