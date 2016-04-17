using System;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public class Bootstrap : ITab
	{
		protected GameObject gameObject;

		public Bootstrap()
		{
			Log.Message("Initialized EdB Interface.");
			this.gameObject = new GameObject(Controller.GameObjectName);
			this.gameObject.AddComponent<Controller>();
			UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
		}

		protected override void FillTab()
		{
		}
	}
}
