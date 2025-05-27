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

namespace PinturilloSO
{
    public partial class EditarPartida: Form
    {
        Socket Servidor;
        SalaPartida SalaPartidaForm;
        CrearSala.Partida Partida = new CrearSala.Partida();
        MenuJugador.Jugador JugadorYo = new MenuJugador.Jugador();

        private ControlScaler _scaler;

        public EditarPartida(Socket Servidor, SalaPartida SalaPartidaForm, MenuJugador.Jugador Jugador, CrearSala.Partida Partida)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.SalaPartidaForm = SalaPartidaForm;
            this.JugadorYo = Jugador;
            this.Partida = Partida;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void EditarPartida_Load(object sender, EventArgs e)
        {
            ComboBoxJugadores.SelectedIndex = 0;
            ComboBoxRondas.SelectedIndex = 0;
            ComboBoxCategoria.SelectedIndex = 0;
            ComboBoxDificultad.SelectedIndex = 0;
            ComboBoxPrivacidad.SelectedIndex = 0;

            LabelNumeroSala.Text = $"Sala Nº{Partida.Codigo}";
            LabelNombreUsuario.Text = JugadorYo.Usuario;
            PictureBoxFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(JugadorYo.FotoPerfilUsuario);
            PictureBoxFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

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

        private void BotonAceptar_Click(object sender, EventArgs e)
        {
            Partida.MaximoJugadores = -ComboBoxJugadores.SelectedIndex + 4;
            Partida.NumeroRondas = ComboBoxRondas.SelectedIndex + 1;
            Partida.Categoria = CrearSala.QuitarTildes(ComboBoxCategoria.SelectedItem.ToString());
            Partida.Dificultad = CrearSala.QuitarTildes(ComboBoxDificultad.SelectedItem.ToString());
            Partida.Privacidad = CrearSala.QuitarTildes(ComboBoxPrivacidad.SelectedItem.ToString());

            SalaPartidaForm.EditarPartida(Partida);

            string mensaje = "5/13/" + Partida.MaximoJugadores + "/" + Partida.NumeroRondas +
                "/" + Partida.Codigo + "/" + Partida.Categoria + "/" + Partida.Dificultad +
                "/" + Partida.Privacidad;

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.Close();
        }

        private void BotonVolverMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
