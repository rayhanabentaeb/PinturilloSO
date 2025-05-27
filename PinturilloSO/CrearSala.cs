using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class CrearSala: Form
    {
        Socket Servidor;
        Thread TPartida;
        MenuJugador MenuJugadorForm;
        MenuJugador.Jugador PerfilJugador;
        Partida NuevaPartida;

        string CodigoSala;
        string Usuario;
        string FotoPerfilUsuario;
        bool Host = true;
        int NumSala;

        private ControlScaler _scaler;

        public struct Partida
        {
            public int MaximoJugadores;
            public int NumeroJugadores;
            public int NumeroRondas;
            public string Codigo;
            public string Categoria;
            public string Dificultad;
            public string Privacidad;
            public List<MenuJugador.Jugador> Jugadores;
            public int[] PuntosJugadores;
            public bool PartidaIniciada;
        }

        public CrearSala(Socket Servidor, MenuJugador MenuJugadorForm, MenuJugador.Jugador PerfilJugador)
        {
            InitializeComponent();
            this.AcceptButton = BotonCrearSala;
            this.Servidor = Servidor;
            this.MenuJugadorForm = MenuJugadorForm;
            this.Usuario = PerfilJugador.Usuario;
            this.FotoPerfilUsuario = PerfilJugador.FotoPerfilUsuario;
            this.PerfilJugador = PerfilJugador;
        }

        private void CrearSala_Load(object sender, EventArgs e)
        {
            ComboBoxJugadores.SelectedIndex = 0;
            ComboBoxRondas.SelectedIndex = 0;
            ComboBoxCategoria.SelectedIndex = 0;
            ComboBoxDificultad.SelectedIndex = 0;
            ComboBoxPrivacidad.SelectedIndex = 0;

            LabelNombreUsuario.Text = Usuario;
            PictureBoxFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfilUsuario);
            PictureBoxFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

            string mensaje = "4/1/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);

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

        private void BotonCrearSala_Click(object sender, EventArgs e)
        {
            ThreadStart ts = delegate { IniciarPartida(); };
            TPartida = new Thread(ts);
            TPartida.Start();
        }

        public void IniciarPartida()
        {
            this.Invoke(new Action(() =>
            {
                NuevaPartida.Codigo = CodigoSala;
                NuevaPartida.NumeroJugadores = 1;
                NuevaPartida.MaximoJugadores = -ComboBoxJugadores.SelectedIndex + 4;
                NuevaPartida.NumeroRondas = ComboBoxRondas.SelectedIndex + 1;
                NuevaPartida.Categoria = QuitarTildes(ComboBoxCategoria.SelectedItem.ToString());
                NuevaPartida.Dificultad = QuitarTildes(ComboBoxDificultad.SelectedItem.ToString());
                NuevaPartida.Privacidad = QuitarTildes(ComboBoxPrivacidad.SelectedItem.ToString());
                NuevaPartida.Jugadores = new List<MenuJugador.Jugador>();
                NuevaPartida.Jugadores.Add(PerfilJugador);
                NuevaPartida.PartidaIniciada = false;

                SalaPartida SalaPartidaForm = new SalaPartida(Servidor, MenuJugadorForm, NuevaPartida, PerfilJugador, Host);
                MenuJugadorForm.SalasDeJuego.Add(SalaPartidaForm);
                NumSala = MenuJugadorForm.SalasDeJuego.Count;
                this.Hide();
                SalaPartidaForm.Show();
                TPartida.Abort();
                this.Close();
            }));
        }

        public void PonerCodigoSala(string CodigoDeSala)
        {
            this.CodigoSala = CodigoDeSala;
            LabelNumeroSala.Invoke(new Action(() =>
            {
                LabelNumeroSala.Text = "Sala Nº" + CodigoDeSala;
            }));
        }

        private void BotonVolverMenu_Click(object sender, EventArgs e)
        {
            string mensaje = "4/0/" + CodigoSala;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.Close();
        }

        private void CrearSala_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje = "4/0/" + CodigoSala;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        public static string QuitarTildes(string texto)
        {
            string normalizado = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizado)
            {
                UnicodeCategory categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (categoria != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
