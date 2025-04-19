using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinturilloSO
{
    public partial class SalaPartida : Form
    {
        private MenuJugador menuJugadorForm; // referencia form MenuJugador
        bool dibujando = false;
        List<List<Point>> trazos = new List<List<Point>>();
        List<Point> puntosActivos = new List<Point>();

        public SalaPartida(MenuJugador menuJugador)
        {
            InitializeComponent();
            this.menuJugadorForm = menuJugador; // referencia form MenuJugador

            PictureBoxJuagador1.BackgroundImage = System.Drawing.Image.FromFile(@"FotoPerfil1.png");
            PictureBoxJuagador1.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxModoJuego1.BackgroundImage = System.Drawing.Image.FromFile(@"Pintor.png");
            PictureBoxModoJuego1.BackgroundImageLayout = ImageLayout.Stretch;

            PictureBoxJuagador2.BackgroundImage = System.Drawing.Image.FromFile(@"FotoPerfil2.png");
            PictureBoxJuagador2.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxModoJuego2.BackgroundImage = System.Drawing.Image.FromFile(@"Mecenas.png");
            PictureBoxModoJuego2.BackgroundImageLayout = ImageLayout.Stretch;

            PictureBoxJuagador3.BackgroundImage = System.Drawing.Image.FromFile(@"FotoPerfil3.png");
            PictureBoxJuagador3.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxModoJuego3.BackgroundImage = System.Drawing.Image.FromFile(@"Mecenas.png");
            PictureBoxModoJuego3.BackgroundImageLayout = ImageLayout.Stretch;

            PictureBoxJuagador4.BackgroundImage = System.Drawing.Image.FromFile(@"FotoPerfil4.png");
            PictureBoxJuagador4.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxModoJuego4.BackgroundImage = System.Drawing.Image.FromFile(@"Mecenas.png");
            PictureBoxModoJuego4.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void PanelLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            dibujando = true;
            puntosActivos = new List<Point>();
            puntosActivos.Add(e.Location);
        }

        private void PanelLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            if (dibujando)
            {
                puntosActivos.Add(e.Location);
                PanelLienzo.Invalidate();
            }
        }

        private void PanelLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            dibujando = false;
            if (puntosActivos.Count > 1)
            {
                trazos.Add(new List<Point>(puntosActivos));
            }
        }

        private void PanelLienzo_Paint(object sender, PaintEventArgs e)
        {
            foreach (var trazo in trazos)
            {
                for (int i = 0; i < trazo.Count - 1; i++)
                {
                    e.Graphics.DrawLine(Pens.Black, trazo[i], trazo[i + 1]);
                }
            }

            if (puntosActivos.Count > 1)
            {
                for (int i = 0; i < puntosActivos.Count - 1; i++)
                {
                    e.Graphics.DrawLine(Pens.Black, puntosActivos[i], puntosActivos[i + 1]);
                }
            }
        }

        private void abandonarPartidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            menuJugadorForm.Show();
        }

        private void LabelPalabra_Click(object sender, EventArgs e)
        {

        }

        public void IntroducirPalabras(string Palabra)
        {
            LabelPalabra.Invoke(new Action(() =>
            {
                LabelPalabra.Text = Palabra;
            }));
        }


    }
}
