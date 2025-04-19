using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinturilloSO
{
    public partial class MenuJugador: Form
    {
        string Usuario;
        Socket Servidor;

        public MenuJugador(string Usuario, Socket Servidor)
        {
            InitializeComponent();
            this.Usuario = Usuario;
            this.Servidor = Servidor;
            this.AcceptButton = BotonAcceder;
            this.LabelNombreUsuario.Text = Usuario;

            FormResizer.SaveInitialControlBounds(this);
            FormResizer.SetInitialFormSize(Size);
            Resize += (s, e) => FormResizer.ScaleControls(this, Size);

            ButtonFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(@"FotoPerfil1.png");
            ButtonFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void MenuJugador_Load(object sender, EventArgs e)
        {
            string mensaje = "2/1/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuRanking_Click(object sender, EventArgs e)
        {
            string mensaje = "3/1/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuPartidasJugadas_Click(object sender, EventArgs e)
        {
            string mensaje = "3/2/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuAmigos_Click(object sender, EventArgs e)
        {
            string mensaje = "3/3/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void BotonCerrarSesion_Click(object sender, EventArgs e)
        {
            string mensaje = "2/0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.Close();
        }

        public void MostrarRelacion(string Relacion)
        {
            MessageBox.Show(Relacion);
        }

        public void CambiarNumeroConectados(string NumeroConectados)
        {
            LableNumeroConectados.Invoke(new Action(() =>
            {
                LableNumeroConectados.Text = NumeroConectados;
            }));
        }

        private void BotonAcceder_Click(object sender, EventArgs e)
        {
            this.Hide();
            SalaPartida SalaPartidaForm = new SalaPartida(this); // pasar la referencia del form
            SalaPartidaForm.ShowDialog();
            this.Show();
        }
    }
}
