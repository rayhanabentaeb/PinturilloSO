using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinturilloSO
{
    public partial class ComoUnirse: Form
    {
        private ControlScaler _scaler;

        public ComoUnirse()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void ComoUnirse_Load(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button boton)
                {
                    boton.Click += Boton_ClickConSonido;
                }
            }
        }

        private void Boton_ClickConSonido(object sender, EventArgs e)
        {
            string ruta = @"Audios\Click.wav";
            SoundPlayer player = new SoundPlayer(ruta);
            player.Play();
        }
    }
}