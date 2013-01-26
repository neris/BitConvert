using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BitConvert
{
	public partial class MainForm : Form
	{
		private readonly CheckBox[] _BitFieldCheckBoxes;

		public MainForm()
		{
			
			InitializeComponent();

			this._BitFieldCheckBoxes = new [] {
				this._BitFieldCheckBox00,
				this._BitFieldCheckBox01,
				this._BitFieldCheckBox02,
				this._BitFieldCheckBox03,
				this._BitFieldCheckBox04,
				this._BitFieldCheckBox05,
				this._BitFieldCheckBox06,
				this._BitFieldCheckBox07,
				this._BitFieldCheckBox08,
				this._BitFieldCheckBox09,
				this._BitFieldCheckBox10,
				this._BitFieldCheckBox11,
				this._BitFieldCheckBox12,
				this._BitFieldCheckBox13,
				this._BitFieldCheckBox14,
				this._BitFieldCheckBox15,
				this._BitFieldCheckBox16,
				this._BitFieldCheckBox17,
				this._BitFieldCheckBox18,
				this._BitFieldCheckBox19,
				this._BitFieldCheckBox20,
				this._BitFieldCheckBox21,
				this._BitFieldCheckBox22,
				this._BitFieldCheckBox23,
				this._BitFieldCheckBox24,
				this._BitFieldCheckBox25,
				this._BitFieldCheckBox26,
				this._BitFieldCheckBox27,
				this._BitFieldCheckBox28,
				this._BitFieldCheckBox29,
				this._BitFieldCheckBox30,
				this._BitFieldCheckBox31
			};

			if (!Properties.Settings.Default.ShowBitFields)
			{
				this._BitFieldsPanel.Hide();
			}

			this._DecimalValue.Select(0, 10);
		}

		private void UpdateDecimal(uint newValue)
		{
			this._DecimalValue.Value = newValue;
		}

		private void UpdateHex(uint newValue)
		{
			this._HexValue.Value = newValue;
		}

		private void UpdateBytes(uint newValue)
		{
			this._HexBytesInput.Text = ByteHelper.BytesToHexString(BitConverter.GetBytes(newValue));
		}

		private void UpdateBits(uint newValue)
		{
			for (int i = 0; i < 32; i++)
			{
				this._BitFieldCheckBoxes[i].Checked = (((int)newValue & (1 << i)) == (1 << i));
			}
		}

		private void _OnDecimalValueValueChanged(object sender, EventArgs e)
		{
			if (this.ActiveControl == this._DecimalValue)
			{
				this.UpdateHex((uint)this._DecimalValue.Value);
				this.UpdateBytes((uint)this._DecimalValue.Value);
				this.UpdateBits((uint)this._DecimalValue.Value);
			}
		}

		private void _OnHexValueValueChanged(object sender, EventArgs e)
		{
			if (this.ActiveControl == this._HexValue)
			{
				this.UpdateDecimal((uint)this._HexValue.Value);
				this.UpdateBytes((uint)this._HexValue.Value);
				this.UpdateBits((uint)this._HexValue.Value);
			}
		}

		private void _OnHexBytesInputKeyUp(object sender, KeyEventArgs e)
		{
			var text = this._HexBytesInput.Text;

			var value = BitConverter.ToUInt32(ByteHelper.HexStringToBytes(text), 0);
			this.UpdateDecimal(value);
			this.UpdateHex(value);
			this.UpdateBits(value);

			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Back)
			{
				this._HexBytesInput.Select(Math.Max(this._HexBytesInput.SelectionStart - 1, 0), 1);
			}
		}

		private void _OnHexBytesInputTextChanged(object sender, EventArgs e)
		{
			this._HexBytesInput.SelectionLength = 1;
			var position = this._HexBytesInput.SelectionStart;

			var text = Regex.Replace(this._HexBytesInput.Text.ToUpper(), @"[^0-9A-F]", "");
			text += "00000000000";
			text = text.Substring(0, 8);
			this._HexBytesInput.Text = text;

			this._HexBytesInput.SelectionStart = Math.Min(position, 7);
		}

		private void _OnHexBytesInputMouseUp(object sender, MouseEventArgs e)
		{
			this._HexBytesInput.SelectionStart = Math.Min(this._HexBytesInput.SelectionStart, 7);
			this._HexBytesInput.SelectionLength = 1;
		}

		private void _OnInvertButtonClick(object sender, EventArgs e)
		{
			var value = (uint) this._DecimalValue.Value;
			value ^= 0xFFFFFFFF;
			this.UpdateDecimal(value);
			this.UpdateHex(value);
			this.UpdateBytes(value);
			this.UpdateBits(value);
		}

		private void _OnShiftLeftButtonClick(object sender, EventArgs e)
		{
			var value = (uint)this._DecimalValue.Value << 1;
			this.UpdateDecimal(value);
			this.UpdateHex(value);
			this.UpdateBytes(value);
			this.UpdateBits(value);
		}

		private void _OnShiftRightButtonClick(object sender, EventArgs e)
		{
			var value = (uint)this._DecimalValue.Value >> 1;
			this.UpdateDecimal(value);
			this.UpdateHex(value);
			this.UpdateBytes(value);
			this.UpdateBits(value);
		}

		private void _OnBitFieldCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			if (this.ActiveControl is CheckBox)
			{
				var checkbox = (CheckBox)sender;
				var index = (int)checkbox.Tag;

				var value = (uint)this._DecimalValue.Value;

				value = (uint)(value ^ (1 << index));

				this.UpdateDecimal(value);
				this.UpdateHex(value);
				this.UpdateBytes(value);
				this.UpdateBits(value);
			}
		}

		private void _OnClearButtonClick(object sender, EventArgs e)
		{
			this._DecimalValue.Value = 0;
			this.UpdateHex(0);
			this.UpdateBytes(0);
			this.UpdateBits(0);
		}

		private void _OnOptionsButtonClick(object sender, EventArgs e)
		{
			var position = this._OptionsButton.PointToScreen(new Point(0, 0));

			this._OptionsMenuStrip.Show(position.X, position.Y + this._OptionsButton.Height);
			this._AlwaysOnTopToolStripMenuItem.Checked = this.TopMost;
			this._ShowBitFieldsToolStripMenuItem.Checked = Properties.Settings.Default.ShowBitFields;
		}

		private void _OnAlwaysOnTopToolStripMenuItemClick(object sender, EventArgs e)
		{
			this._AlwaysOnTopToolStripMenuItem.Checked = !this._AlwaysOnTopToolStripMenuItem.Checked;

			this.TopMost = this._AlwaysOnTopToolStripMenuItem.Checked;
			Properties.Settings.Default.TopMost = this._AlwaysOnTopToolStripMenuItem.Checked;
		}

		private void _OnShowBitFieldsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.ShowBitFields)
			{
				this._BitFieldsPanel.Hide();
			}
			else
			{
				this._BitFieldsPanel.Show();
			}

			Properties.Settings.Default.ShowBitFields = !Properties.Settings.Default.ShowBitFields;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case (Keys.Control | Keys.I):
					this._OnInvertButtonClick(this, null);
					return true;

				case (Keys.Control | Keys.O):
					this._OnOptionsButtonClick(this, null);
					return true;

				case (Keys.Control | Keys.Left):
					this._OnShiftLeftButtonClick(this, null);
					return true;

				case (Keys.Control | Keys.Right):
					this._OnShiftRightButtonClick(this, null);
					return true;

				default:
					return base.ProcessCmdKey(ref msg, keyData);
			}
		}
	}
}
