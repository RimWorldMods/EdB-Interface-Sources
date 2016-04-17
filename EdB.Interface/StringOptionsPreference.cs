using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class StringOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler(string value);

		private string stringValue;

		private string setValue;

		public int tooltipId;

		public static float RadioButtonWidth = 24f;

		public static float RadioButtonMargin = 18f;

		public static float LabelMargin = StringOptionsPreference.RadioButtonWidth + StringOptionsPreference.RadioButtonMargin;

		public event StringOptionsPreference.ValueChangedHandler ValueChanged
		{
			add
			{
				StringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				StringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<StringOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (StringOptionsPreference.ValueChangedHandler)Delegate.Combine(valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove
			{
				StringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				StringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<StringOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (StringOptionsPreference.ValueChangedHandler)Delegate.Remove(valueChangedHandler2, value), valueChangedHandler);
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

		public abstract string DefaultValue
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

		public abstract IEnumerable<string> OptionValues
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
				this.stringValue = value;
				this.setValue = value;
			}
		}

		public virtual string Value
		{
			get
			{
				if (this.setValue != null)
				{
					return this.setValue;
				}
				return this.DefaultValue;
			}
			set
			{
				string text = this.setValue;
				this.setValue = value;
				this.stringValue = value.ToString();
				if ((text == null || text != value) && this.ValueChanged != null)
				{
					this.ValueChanged(value);
				}
			}
		}

		public virtual string ValueForDisplay
		{
			get
			{
				if (this.setValue != null)
				{
					return this.setValue;
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

		public StringOptionsPreference()
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
			foreach (string current in this.OptionValues)
			{
				string text = (this.OptionValuePrefix + "." + current).Translate();
				float num2 = Text.CalcHeight(text, width - StringOptionsPreference.LabelMargin - num);
				Rect rect = new Rect(positionX - 4f + num, positionY - 3f, width + 6f - num, num2 + 5f);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				Rect rect2 = new Rect(positionX + num, positionY, width - StringOptionsPreference.LabelMargin - num, num2);
				GUI.Label(rect2, text);
				if (this.Tooltip != null)
				{
					TipSignal tip = new TipSignal(() => this.Tooltip.Translate(), this.TooltipId);
					TooltipHandler.TipRegion(rect2, tip);
				}
				string valueForDisplay = this.ValueForDisplay;
				bool chosen = valueForDisplay == current;
				if (Widgets.RadioButton(new Vector2(positionX + width - StringOptionsPreference.RadioButtonWidth, positionY - 3f), chosen) && !disabled)
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
