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
		private bool anyWorldFiles;

		private bool anyMapFiles;

		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 372f);
			}
		}

		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuButtons(rect, this.anyWorldFiles, this.anyMapFiles, null);
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		public override void PreOpen()
		{
			base.PreOpen();
			ConceptDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyWorldFiles = SavedWorldsDatabase.AllWorldFiles.Any<FileInfo>();
			this.anyMapFiles = MapFilesUtility.AllMapFiles.Any<FileInfo>();
		}
	}
}
