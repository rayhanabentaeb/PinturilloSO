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

namespace PinturilloSO
{
    public partial class PerfilJugador: Form
    {
        Socket Servidor;
        MenuJugador.Jugador Jugador;
        MenuJugador.Jugador JugadorYo;
        CrearSala.Partida Partida;
        SalaPartida Sala;

        bool Host;
        private ControlScaler _scaler;

        public PerfilJugador(Socket Servidor, SalaPartida Sala, 
            CrearSala.Partida Partida, MenuJugador.Jugador PerfilJugador,
            MenuJugador.Jugador JugadorYo, bool Host)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.Sala = Sala;
            this.Partida = Partida;
            this.Jugador = PerfilJugador;
            this.JugadorYo = JugadorYo;
            this.Host = Host;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void PerfilJugador_Load(object sender, EventArgs e)
        {
            LabelJugador.Text = Jugador.Usuario;
            ButtonJugador.BackgroundImage = System.Drawing.Image.FromFile(Jugador.FotoPerfilUsuario);
            ButtonJugador.BackgroundImageLayout = ImageLayout.Stretch;
            

            if (Jugador.Usuario == JugadorYo.Usuario)
            {
                BotonAmistad.Text = "¿Cómo Jugar?";
                BotonExpulsar.Text = "Abandonar Partida";
            }

            string mensaje = "3/4/" + Partida.Codigo + "/" + Jugador.Usuario;
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

        public string DameUsuarioPerfil()
        {
            return Jugador.Usuario;
        }

        public void PonerRankingVictorias(string Ranking, string Victorias, string FraseFavorita)
        {
            this.Invoke(new Action(() =>
            {
                LabelValorRanking.Text = $"#{Ranking}";
                LabelValorVictorias.Text = Victorias;
                LabelFraseFavorita.Text = $"\"{FraseFavorita}\"";
            }));
        }

        private void PerfilJugador_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sala.PerfilesAbiertos.Remove(this);
        }

        private void BotonAmistad_Click(object sender, EventArgs e)
        {
            if (Jugador.Usuario != JugadorYo.Usuario)
            {
                DialogResult Resultado = MessageBox.Show(
                $"¿Quieres enviar una solicitud de amistad a {Jugador.Usuario}?",
                "Solicitud de amistad",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (Resultado == DialogResult.Yes)
                {
                    string mensaje = "1/3/" + JugadorYo.Usuario + "/" + Jugador.Usuario;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);
                }
            }
            else 
            {
                ComoJugar ComoJugarForm = new ComoJugar();
                ComoJugarForm.Show();
            }
        }

        private void BotonExpulsar_Click(object sender, EventArgs e)
        {
            if (Jugador.Usuario != JugadorYo.Usuario)
            {
                DialogResult Resultado = MessageBox.Show(
                $"¿Quieres expulsar al jugador {Jugador.Usuario}?",
                "Votación de expulsión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (Resultado == DialogResult.Yes)
                {
                    int SoyHost = 0;
                    if (Host)
                        SoyHost = 1;

                    string mensaje = "5/10/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" 
                        + Jugador.Usuario + "/" + SoyHost;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);

                    MessageBox.Show("Votación de expulsión enviada.");
                }
            }
            else
            {
                DialogResult Resultado = MessageBox.Show(
                $"¿Seguro que quieres abandonar la partida?",
                "Abandonar partida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (Resultado == DialogResult.Yes)
                {
                    Sala.Close();
                    this.Close();
                }
            }
        }
    }
}