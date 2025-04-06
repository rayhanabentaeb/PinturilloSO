using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinturilloSO
{
    internal class FormResizer
    {
        private static Dictionary<Control, Rectangle> initialControlBounds = new Dictionary<Control, Rectangle>();
        private static Size initialFormSize;

        public static void SaveInitialControlBounds(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                initialControlBounds[control] = control.Bounds;
                if (control.Controls.Count > 0)
                {
                    SaveInitialControlBounds(control);
                }
            }
        }

        public static void SetInitialFormSize(Size size)
        {
            initialFormSize = size;
        }

        public static void ScaleControls(Control parentControl, Size currentFormSize)
        {
            float scaleFactorWidth = (float)currentFormSize.Width / initialFormSize.Width;
            float scaleFactorHeight = (float)currentFormSize.Height / initialFormSize.Height;
            foreach (Control control in parentControl.Controls)
            {
                if (initialControlBounds.ContainsKey(control)) // Verifica si el control existe en el diccionario
                {
                    Rectangle initialBounds = initialControlBounds[control];
                    control.Bounds = new Rectangle((int)(initialBounds.Left * scaleFactorWidth), (int)(initialBounds.Top * scaleFactorHeight), (int)(initialBounds.Width * scaleFactorWidth), (int)(initialBounds.Height * scaleFactorHeight));
                }
                if (control.Controls.Count > 0)
                {
                    ScaleControls(control, currentFormSize);
                }
            }
        }
    }
}