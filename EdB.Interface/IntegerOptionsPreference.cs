using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class IntegerOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler(int value);

		private string stringValue;

		private int? intValue;

		public int tooltipId;

		public static float RadioButtonWidth = 24f;

		public static float RadioButtonMargin = 18f;

		public static float LabelMargin = IntegerOptionsPreference.RadioButtonWidth + IntegerOptionsPreference.RadioButtonMargin;

		public event IntegerOptionsPreference.ValueChangedHandler ValueChanged
		{
			add
			{
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<IntegerOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (IntegerOptionsPreference.ValueChangedHandler)Delegate.Combine(valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove
			{
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				IntegerOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<IntegerOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (IntegerOptionsPreference.ValueChangedHandler)Delegate.Remove(valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}

		public abstract string Name
		{
			get;
		}

		public abstract string Group
		{
			get;
		}

		public virtual string Tooltip
		{
			get
			{
				return null;
			}
		}

		public virtual bool DisplayInOptions
		{
			get
			{
				return true;
			}
		}

		protected virtual int TooltipId
		{
			get
			{
				if (this.tooltipId == 0)
				{
					this.tooltipId = this.Tooltip.Translate().GetHashCode();
					return this.tooltipId;
				}
				return 0;
			}
		}

		public virtual string Label
		{
			get
			{
				return this.Name.Translate();
			}
		}

		public abstract int DefaultValue
		{
			get;
		}

		public virtual bool Disabled
		{
			get
			{
				return false;
			}
		}

		public abstract IEnumerable<int> OptionValues
		{
			get;
		}

		public abstract string OptionValuePrefix
		{
			get;
		}

		public string ValueForSerialization
		{
			get
			{
				return this.stringValue;
			}
			set
			{
				int value2;
				if (int.TryParse(value, out value2) && this.OptionValues.Contains(value2))
				{
					this.stringValue = value;
					this.intValue = new int?(value2);
					return;
				}
				this.intValue = null;
				this.stringValue = null;
			}
		}

		public virtual int Value
		{
			get
			{
				int? num = this.intValue;
				if (num.HasValue)
				{
					return this.intValue.Value;
				}
				return this.DefaultValue;
			}
			set
			{
				int? num = this.intValue;
				this.intValue = new int?(value);
				this.stringValue = value.ToString();
				if ((!num.HasValue || num.Value != this.intValue) && this.ValueChanged != null)
				{
					this.ValueChanged(value);
				}
			}
		}

		public virtual int ValueForDisplay
		{
			get
			{
				int? num = this.intValue;
				if (num.HasValue)
				{
					return this.intValue.Value;
				}
				return this.DefaultValue;
			}
		}

		public virtual bool Indent
		{
			get
			{
				return false;
			}
		}

		public IntegerOptionsPreference()
		{
		}

		public void OnGUI(float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			if (disabled)
			{
				GUI.color = Dialog_InterfaceOptions.DisabledControlColor;
			}
			float num = (!this.Indent) ? 0f : Dialog_InterfaceOptions.IndentSize;
			foreach (int current in this.OptionValues)
			{
				string text = (this.OptionValuePrefix + "." + current).Translate();
				float num2 = Text.CalcHeight(text, width - IntegerOptionsPreference.LabelMargin - num);
				Rect rect = new Rect(positionX - 4f + num, positionY - 3f, width + 6f - num, num2 + 5f);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Rect rect2 = new Rect(positionX + num, positionY, width - IntegerOptionsPreference.LabelMargin - num, num2);
				GUI.Label(rect2, text);
				if (this.Tooltip != null)
				{
					TipSignal tip = new TipSignal(() => this.Tooltip.Translate(), this.TooltipId);
					TooltipHandler.TipRegion(rect2, tip);
				}
				int valueForDisplay = this.ValueForDisplay;
				bool chosen = valueForDisplay == current;
				if (Widgets.RadioButton(new Vector2(positionX + width - IntegerOptionsPreference.RadioButtonWidth, positionY - 3f), chosen) && !disabled)
				{
					this.Value = current;
				}
				positionY += num2 + Dialog_InterfaceOptions.PreferencePadding.y;
			}
			positionY -= Dialog_InterfaceOptions.PreferencePadding.y;
			GUI.color = Color.white;
		}
	}
}
