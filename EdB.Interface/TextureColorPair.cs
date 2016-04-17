using System;
using UnityEngine;

namespace EdB.Interface
{
	public class TextureColorPair
	{
		public Texture texture;

		public Color color;

		public TextureColorPair(Texture texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}
	}
}
