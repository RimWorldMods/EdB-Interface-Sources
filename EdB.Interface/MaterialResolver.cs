using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class MaterialResolver
	{
		public static TextureColorPair Resolve(Thing thing)
		{
			if (thing.def.apparel != null)
			{
				return new TextureColorPair(thing.def.uiIcon, thing.DrawColor);
			}
			return new TextureColorPair(thing.def.uiIcon, thing.DrawColor);
		}

		public static TextureColorPair Resolve(ThingDef def)
		{
			return new TextureColorPair(def.uiIcon, Color.white);
		}
	}
}
