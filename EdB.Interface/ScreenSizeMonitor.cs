using System;
using System.Threading;
using UnityEngine;

namespace EdB.Interface
{
	public class ScreenSizeMonitor
	{
		public delegate void ScreenSizeChangeHandler(int width, int height);

		protected int width;

		protected int height;

		public event ScreenSizeMonitor.ScreenSizeChangeHandler Changed
		{
			add
			{
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler = this.Changed;
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler2;
				do
				{
					screenSizeChangeHandler2 = screenSizeChangeHandler;
					screenSizeChangeHandler = Interlocked.CompareExchange<ScreenSizeMonitor.ScreenSizeChangeHandler>(ref this.Changed, (ScreenSizeMonitor.ScreenSizeChangeHandler)Delegate.Combine(screenSizeChangeHandler2, value), screenSizeChangeHandler);
				}
				while (screenSizeChangeHandler != screenSizeChangeHandler2);
			}
			remove
			{
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler = this.Changed;
				ScreenSizeMonitor.ScreenSizeChangeHandler screenSizeChangeHandler2;
				do
				{
					screenSizeChangeHandler2 = screenSizeChangeHandler;
					screenSizeChangeHandler = Interlocked.CompareExchange<ScreenSizeMonitor.ScreenSizeChangeHandler>(ref this.Changed, (ScreenSizeMonitor.ScreenSizeChangeHandler)Delegate.Remove(screenSizeChangeHandler2, value), screenSizeChangeHandler);
				}
				while (screenSizeChangeHandler != screenSizeChangeHandler2);
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
		}

		public ScreenSizeMonitor()
		{
			this.width = Screen.width;
			this.height = Screen.height;
		}

		public void Update()
		{
			int num = Screen.width;
			int num2 = Screen.height;
			if (num != this.width || num2 != this.height)
			{
				this.width = num;
				this.height = num2;
				if (this.Changed != null)
				{
					this.Changed(num, num2);
				}
			}
		}
	}
}
