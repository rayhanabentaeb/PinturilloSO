using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PinturilloSO.CrearSala;
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class PerfilAmigo: Form
    {
        Socket Servidor;
        string Usuario;
        private ControlScaler _scaler;

        public PerfilAmigo(Socket Servidor, string Usuario)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.Usuario = Usuario;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void PerfilAmigo_Load(object sender, EventArgs e)
        {
            string mensaje = "3/5/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
            

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

        public void PonerFotoRankingVictorias(string FotoPerfil, string Ranking, string Victorias, string Conectado, string FraseFavorita)
        {
            this.Invoke(new Action(() =>
            {
                LabelJugador.Text = Usuario;

                BotonJugador.BackgroundImage = System.Drawing.Image.FromFile(@"FotosPerfil\" + FotoPerfil + ".png");
                BotonJugador.BackgroundImageLayout = ImageLayout.Stretch;

                LabelFraseFavorita.Text = $"\"{FraseFavorita}\"";


                LabelValorRanking.Text = $"#{Ranking}";
                LabelValorVictorias.Text = Victorias;

                if (Conectado == "0")
                    LabelConexion.Text = "'Conectado'";
                else if (Conectado == "-1")
                    LabelConexion.Text = "'Desconectado'";
            }));
        }

    }
}
