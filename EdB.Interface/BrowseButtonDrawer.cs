using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class BrowseButtonDrawer
	{
		public static SelectorUtility selector = new SelectorUtility();

		public static Vector2 ButtonSize = new Vector2(15f, 17f);

		public static Color ButtonColor = new Color(0.75f, 0.75f, 0.75f);

		public static Texture2D ButtonTextureNext;

		public static Texture2D ButtonTexturePrevious;

		public static void ResetTextures()
		{
			BrowseButtonDrawer.ButtonTextureNext = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/BrowseNext", true);
			BrowseButtonDrawer.ButtonTexturePrevious = ContentFinder<Texture2D>.Get("EdB/Interface/TabReplacement/BrowsePrevious", true);
			BrowseButtonDrawer.ButtonSize = new Vector2((float)BrowseButtonDrawer.ButtonTextureNext.width, (float)BrowseButtonDrawer.ButtonTextureNext.height);
		}

		public static void DrawBrowseButtons(Vector2 tabSize, Pawn currentPawn)
		{
			float num = 14f;
			float padding = 30f;
			float num2 = BrowseButtonDrawer.ButtonSize.y + num * 2f;
			BrowseButtonDrawer.DrawBrowseButtons(tabSize.y - num2 + num, tabSize.x, padding, currentPawn);
		}

		public static void DrawBrowseButtons(float top, float width, float padding, Pawn currentPawn)
		{
			if (currentPawn != null)
			{
				Action action = null;
				Action action2 = null;
				if (currentPawn.IsColonist)
				{
					if (Find.ListerPawns.FreeColonists.Count<Pawn>() > 1)
					{
						action = delegate
						{
							BrowseButtonDrawer.selector.SelectNextColonist();
						};
						action2 = delegate
						{
							BrowseButtonDrawer.selector.SelectPreviousColonist();
						};
					}
				}
				else if (currentPawn.IsPrisonerOfColony)
				{
					if (Find.ListerPawns.PrisonersOfColonyCount > 1)
					{
						action = delegate
						{
							BrowseButtonDrawer.selector.SelectNextPrisoner();
						};
						action2 = delegate
						{
							BrowseButtonDrawer.selector.SelectPreviousPrisoner();
						};
					}
				}
				else
				{
					Faction faction = currentPawn.Faction;
					if (faction != null)
					{
						if (faction != Faction.OfColony)
						{
							FactionRelation factionRelation = faction.RelationWith(Faction.OfColony);
							if (factionRelation != null)
							{
								bool hostile = factionRelation.hostile;
								if (hostile)
								{
									if (BrowseButtonDrawer.selector.MoreThanOneHostilePawn)
									{
										action = delegate
										{
											BrowseButtonDrawer.selector.SelectNextEnemy();
										};
										action2 = delegate
										{
											BrowseButtonDrawer.selector.SelectPreviousEnemy();
										};
									}
								}
								else if (BrowseButtonDrawer.selector.MoreThanOneVisitorPawn)
								{
									action = delegate
									{
										BrowseButtonDrawer.selector.SelectNextVisitor();
									};
									action2 = delegate
									{
										BrowseButtonDrawer.selector.SelectPreviousVisitor();
									};
								}
							}
						}
						else if (BrowseButtonDrawer.selector.MoreThanOneColonyAnimal)
						{
							action = delegate
							{
								BrowseButtonDrawer.selector.SelectNextColonyAnimal();
							};
							action2 = delegate
							{
								BrowseButtonDrawer.selector.SelectPreviousColonyAnimal();
							};
						}
					}
				}
				Rect rect = new Rect(0f, 0f, BrowseButtonDrawer.ButtonSize.x, BrowseButtonDrawer.ButtonSize.y);
				if (action != null && action2 != null)
				{
					rect.x = padding - rect.width;
					rect.y = top;
					if (rect.Contains(Event.current.mousePosition))
					{
						GUI.color = Color.white;
					}
					else
					{
						GUI.color = BrowseButtonDrawer.ButtonColor;
					}
					GUI.DrawTexture(rect, BrowseButtonDrawer.ButtonTexturePrevious);
					if (Widgets.InvisibleButton(rect))
					{
						action2();
					}
					rect.x = width - padding;
					rect.y = top;
					if (rect.Contains(Event.current.mousePosition))
					{
						GUI.color = Color.white;
					}
					else
					{
						GUI.color = BrowseButtonDrawer.ButtonColor;
					}
					GUI.DrawTexture(rect, BrowseButtonDrawer.ButtonTextureNext);
					if (Widgets.InvisibleButton(rect))
					{
						action();
					}
				}
			}
		}
	}
}
