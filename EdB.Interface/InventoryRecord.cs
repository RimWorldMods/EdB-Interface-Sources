using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class InventoryRecord
	{
		public ThingDef thingDef;

		public ThingDef stuffDef;

		public InventoryType type;

		public int count;

		public int unfinishedCount;

		public int compressedCount;

		public Color color;

		public Texture texture;

		public Vector2 proportions;

		public float buildingScale;

		public InventoryResolver resolver;

		public int availableCount;

		public int reservedCount;

		public void ResetCounts()
		{
			this.count = 0;
			this.unfinishedCount = 0;
			this.compressedCount = 0;
			this.availableCount = -1;
			this.reservedCount = 0;
		}
	}
}
