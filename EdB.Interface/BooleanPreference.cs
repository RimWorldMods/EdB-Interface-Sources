using System;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class BooleanPreference : IPreference
	{
		public delegate void ValueChangedHandler(bool value);

		private string stringValue;

		private bool? boolValue;

		public int tooltipId;

		public static float CheckboxWidth = 24f;

		public static float CheckboxMargin = 18f;

		public static float LabelMargin = BooleanPreference.CheckboxWidth + BooleanPreference.CheckboxMargin;

		public event BooleanPreference.ValueChangedHandler ValueChanged;

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

		public abstract bool DefaultValue
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

		public string ValueForSerialization
		{
			get
			{
				return this.stringValue;
			}
			set
			{
				if ("true".Equals(value))
				{
					this.boolValue = new bool?(true);
					this.stringValue = value;
				}
				else if ("false".Equals(value))
				{
					this.boolValue = new bool?(false);
					this.stringValue = value;
				}
				else
				{
					if (!string.IsNullOrEmpty(value))
					{
						this.boolValue = null;
						this.stringValue = null;
						throw new ArgumentException("Cannot set this true/false preference to the specified non-boolean value.");
					}
					this.boolValue = null;
					this.stringValue = null;
				}
			}
		}

		public virtual bool Value
		{
			get
			{
				bool? flag = this.boolValue;
				if (flag.HasValue)
				{
					return this.boolValue.Value;
				}
				return this.DefaultValue;
			}
			set
			{
				bool? flag = this.boolValue;
				this.boolValue = new bool?(value);
				this.stringValue = ((!value) ? "false" : "true");
				if ((!flag.HasValue || flag.Value != this.boolValue) && this.ValueChanged != null)
				{
					this.ValueChanged(value);
				}
			}
		}

		public virtual bool ValueForDisplay
		{
			get
			{
				bool? flag = this.boolValue;
				if (flag.HasValue)
				{
					return this.boolValue.Value;
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

		public BooleanPreference()
		{
		}

		public void OnGUI(float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			float num = (!this.Indent) ? 0f : Dialog_InterfaceOptions.IndentSize;
			string label = this.Label;
			float num2 = Text.CalcHeight(label, width - BooleanPreference.LabelMargin - num);
			Rect rect = new Rect(positionX - 4f + num, positionY - 3f, width + 6f - num, num2 + 5f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = new Rect(positionX + num, positionY, width - BooleanPreference.LabelMargin - num, num2);
			if (disabled)
			{
				GUI.color = Dialog_InterfaceOptions.DisabledControlColor;
			}
			GUI.Label(rect2, label);
			GUI.color = Color.white;
			if (this.Tooltip != null)
			{
				TipSignal tip = new TipSignal(() => this.Tooltip.Translate(), this.TooltipId);
				TooltipHandler.TipRegion(rect2, tip);
			}
			bool valueForDisplay = this.ValueForDisplay;
			Widgets.Checkbox(new Vector2(positionX + width - BooleanPreference.CheckboxWidth, positionY - 2f), ref valueForDisplay, 24f, disabled);
			this.Value = valueForDisplay;
			positionY += num2;
		}
	}
}
