using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace EdB.Interface
{
	public abstract class MultipleSelectionStringOptionsPreference : IPreference
	{
		public delegate void ValueChangedHandler(IEnumerable<string> selectedOptions);

		private string stringValue;

		private string setValue;

		public int tooltipId;

		public HashSet<string> selectedOptions = new HashSet<string>();

		public static float RadioButtonWidth = 24f;

		public static float RadioButtonMargin = 18f;

		public static float LabelMargin = MultipleSelectionStringOptionsPreference.RadioButtonWidth + MultipleSelectionStringOptionsPreference.RadioButtonMargin;

		public event MultipleSelectionStringOptionsPreference.ValueChangedHandler ValueChanged
		{
			add
			{
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<MultipleSelectionStringOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (MultipleSelectionStringOptionsPreference.ValueChangedHandler)Delegate.Combine(valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
			remove
			{
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler = this.ValueChanged;
				MultipleSelectionStringOptionsPreference.ValueChangedHandler valueChangedHandler2;
				do
				{
					valueChangedHandler2 = valueChangedHandler;
					valueChangedHandler = Interlocked.CompareExchange<MultipleSelectionStringOptionsPreference.ValueChangedHandler>(ref this.ValueChanged, (MultipleSelectionStringOptionsPreference.ValueChangedHandler)Delegate.Remove(valueChangedHandler2, value), valueChangedHandler);
				}
				while (valueChangedHandler != valueChangedHandler2);
			}
		}

		public IEnumerable<string> SelectedOptions
		{
			get
			{
				return this.selectedOptions;
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
				this.selectedOptions.Clear();
				string[] array = this.stringValue.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (this.OptionValues.Contains(text))
					{
						this.selectedOptions.Add(text);
					}
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
				return true;
			}
		}

		public MultipleSelectionStringOptionsPreference()
		{
		}

		public bool IsOptionSelected(string option)
		{
			return this.selectedOptions.Contains(option);
		}

		public void UpdateOption(string option, bool value)
		{
			if (!value)
			{
				if (this.selectedOptions.Contains(option))
				{
					this.selectedOptions.Remove(option);
					this.UpdateSerializedValue();
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this.selectedOptions);
					}
				}
			}
			else if (!this.selectedOptions.Contains(option))
			{
				this.selectedOptions.Add(option);
				this.UpdateSerializedValue();
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this.selectedOptions);
				}
			}
		}

		protected void UpdateSerializedValue()
		{
			this.stringValue = string.Join(",", this.selectedOptions.ToArray<string>());
		}

		public virtual string OptionTranslated(string optionValue)
		{
			return (this.OptionValuePrefix + "." + optionValue).Translate();
		}

		public void OnGUI(float positionX, ref float positionY, float width)
		{
			bool disabled = this.Disabled;
			if (disabled)
			{
				GUI.color = Dialog_InterfaceOptions.DisabledControlColor;
			}
			if (!string.IsNullOrEmpty(this.Name))
			{
				string text = this.Name.Translate();
				float num = Text.CalcHeight(text, width);
				Rect rect = new Rect(positionX, positionY, width, num);
				Widgets.Label(rect, text);
				if (this.Tooltip != null)
				{
					TipSignal tip = new TipSignal(() => this.Tooltip.Translate(), this.TooltipId);
					TooltipHandler.TipRegion(rect, tip);
				}
				positionY += num + Dialog_InterfaceOptions.PreferencePadding.y;
			}
			float num2 = (!this.Indent) ? 0f : Dialog_InterfaceOptions.IndentSize;
			foreach (string current in this.OptionValues)
			{
				string text2 = this.OptionTranslated(current);
				if (!string.IsNullOrEmpty(text2))
				{
					float num3 = Text.CalcHeight(text2, width - MultipleSelectionStringOptionsPreference.LabelMargin - num2);
					Rect rect2 = new Rect(positionX - 4f + num2, positionY - 3f, width + 6f - num2, num3 + 5f);
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
					}
					Rect rect3 = new Rect(positionX + num2, positionY, width - num2, num3);
					bool flag = this.IsOptionSelected(current);
					bool flag2 = flag;
					WidgetDrawer.DrawLabeledCheckbox(rect3, text2, ref flag);
					if (flag != flag2)
					{
						this.UpdateOption(current, flag);
					}
					positionY += num3 + Dialog_InterfaceOptions.PreferencePadding.y;
				}
			}
			positionY -= Dialog_InterfaceOptions.PreferencePadding.y;
			GUI.color = Color.white;
		}
	}
}
