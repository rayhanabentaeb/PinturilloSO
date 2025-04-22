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
    public partial class CambiarPerfil: Form
    {
        Socket Servidor;
        MenuJugador.Jugador PerfilJugador;

        string Usuario;
        string NuevoUsuario;
        string NuevaFotoPerfil = "";
        string FotoPerfilUsuario;
        int CodigoFotoPerfil;

        public CambiarPerfil(MenuJugador.Jugador PerfilJugador, Socket Servidor = null)
        {
            InitializeComponent();
            this.AcceptButton = BotonGuardar;
            this.Servidor = Servidor;
            this.Usuario = PerfilJugador.Usuario;
            this.CodigoFotoPerfil = PerfilJugador.CodigoFotoPerfil;
            this.FotoPerfilUsuario = @"FotosPerfil\" + CodigoFotoPerfil + ".png";
            this.PerfilJugador = PerfilJugador;

            BotonConfirmar.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Enviar.png");
            BotonConfirmar.BackgroundImageLayout = ImageLayout.Stretch;

            LabelNombreUsuario.Text = Usuario;
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfilUsuario);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil1.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\1.png");
            BotonFotoPerfil1.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil2.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\2.png");
            BotonFotoPerfil2.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil3.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\3.png");
            BotonFotoPerfil3.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil4.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\4.png");
            BotonFotoPerfil4.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil5.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\5.png");
            BotonFotoPerfil5.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil6.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\6.png");
            BotonFotoPerfil6.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil7.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\7.png");
            BotonFotoPerfil7.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil8.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\8.png");
            BotonFotoPerfil8.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil9.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\9.png");
            BotonFotoPerfil9.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil10.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\10.png");
            BotonFotoPerfil10.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil11.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\11.png");
            BotonFotoPerfil11.BackgroundImageLayout = ImageLayout.Stretch;

            BotonFotoPerfil12.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\12.png");
            BotonFotoPerfil12.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil1_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 1;
            NuevaFotoPerfil = @"FotosPerfil\1.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil2_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 2;
            NuevaFotoPerfil = @"FotosPerfil\2.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil3_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 3;
            NuevaFotoPerfil = @"FotosPerfil\3.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil4_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 4;
            NuevaFotoPerfil = @"FotosPerfil\4.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil5_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 5;
            NuevaFotoPerfil = @"FotosPerfil\5.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil6_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 6;
            NuevaFotoPerfil = @"FotosPerfil\6.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil7_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 7;
            NuevaFotoPerfil = @"FotosPerfil\7.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil8_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 8;
            NuevaFotoPerfil = @"FotosPerfil\8.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil9_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 9;
            NuevaFotoPerfil = @"FotosPerfil\9.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil10_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 10;
            NuevaFotoPerfil = @"FotosPerfil\10.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil11_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 11;
            NuevaFotoPerfil = @"FotosPerfil\11.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonFotoPerfil12_Click(object sender, EventArgs e)
        {
            CodigoFotoPerfil = 12;
            NuevaFotoPerfil = @"FotosPerfil\12.png";
            PictureBoxUsuario.BackgroundImage = System.Drawing.Image.FromFile(NuevaFotoPerfil);
            PictureBoxUsuario.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void BotonConfirmar_Click(object sender, EventArgs e)
        {
            if (TextCambiarUsuario.Text == "")
            {
                MessageBox.Show("No ha introducido ningun nombre.");
            }
            else
            {
                LabelNombreUsuario.Text = TextCambiarUsuario.Text;
                TextCambiarUsuario.Text = "";
            }
        }

        private void BotonGuardar_Click(object sender, EventArgs e)
        {
            NuevoUsuario = LabelNombreUsuario.Text;

            string mensaje = "1/2/" + NuevoUsuario + "/" + CodigoFotoPerfil + "/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.Close();
        }
    }
}
