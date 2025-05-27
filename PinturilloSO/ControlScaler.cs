using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PinturilloSO
{
    internal class ControlScaler
    {
        private Form _form;
        private Size _formOriginal;
        private Dictionary<Control, Rectangle> _origBounds = new Dictionary<Control, Rectangle>();
        private Dictionary<Control, float> _origFontSizes = new Dictionary<Control, float>();

        public ControlScaler(Form form)
        {
            _form = form;
            _form.AutoScaleMode = AutoScaleMode.None;
            _form.Load += Form_Load;
            _form.Resize += Form_Resize;
            _form.ControlAdded += ControlAdded_Handler;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            _formOriginal = _form.ClientSize;
            RegisterControls(_form);
        }

        private void RegisterControls(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                _origBounds[ctl] = new Rectangle(ctl.Location, ctl.Size);
                _origFontSizes[ctl] = ctl.Font.Size;
                ctl.ControlAdded += ControlAdded_Handler;
                RegisterControls(ctl);
            }
        }

        private void ControlAdded_Handler(object sender, ControlEventArgs e)
        {
            Control nuevo = e.Control;
            _origBounds[nuevo] = new Rectangle(nuevo.Location, nuevo.Size);
            _origFontSizes[nuevo] = nuevo.Font.Size;
            nuevo.ControlAdded += ControlAdded_Handler;
            RegisterControls(nuevo);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Minimized) return;
            if (_formOriginal.Width <= 0 || _formOriginal.Height <= 0) return;

            _form.Visible = false;
            _form.SuspendLayout();

            float sx = (float)_form.ClientSize.Width / _formOriginal.Width;
            float sy = (float)_form.ClientSize.Height / _formOriginal.Height;

            foreach (var kv in _origBounds)
            {
                Control ctl = kv.Key;
                Rectangle orig = kv.Value;
                ctl.Left = (int)(orig.Left * sx);
                ctl.Top = (int)(orig.Top * sy);
                ctl.Width = (int)(orig.Width * sx);
                ctl.Height = (int)(orig.Height * sy);

                if (_origFontSizes.TryGetValue(ctl, out float fontSize))
                {
                    float nuevoTam = fontSize * Math.Min(sx, sy);
                    ctl.Font = new Font(ctl.Font.FontFamily, nuevoTam);
                }
            }

            _form.ResumeLayout();
            _form.Visible = true;
        }
    }
}