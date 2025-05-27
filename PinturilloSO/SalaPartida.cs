using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class SalaPartida : Form
    {
        int TiempoRonda = 99;
        int TiempoRestante;
        int DuracionRonda;
        int TotalRondas;
        int RondaActual = 0;
        int AciertosRonda = 0;
        Queue<object[]> ColaUltimos20Mensajes = new Queue<object[]>();

        bool Host;
        bool Pintor = false;
        bool PalabraAcertada = false;
        bool EnSala = false;
        bool PartidaTerminada = false;

        string Palabra = "";
        string MensajeChat;
        string UsuarioPintor = "";

        int LetrasMostradas = 0;
        char[] PalabraOculta;
        double TiempoPorLetra = 0;

        private List<string> PalabrasPorAcertar = new List<string>();
        private List<string> Pintores = new List<string>();
        private List<int> indicesAleatorios = new List<int>();
        private Timer TimerReloj;
        private Timer TimerPalabra;
        private Stopwatch CronometroRonda = new Stopwatch();
        private Stopwatch CronometroPalabra = new Stopwatch();
        private DateTime lastSendTime = DateTime.MinValue;
        private const int sendIntervalMilliseconds = 40;

        Socket Servidor;
        MenuJugador MenuJugadorForm;
        public Invitar InvitarForm;
        CrearSala.Partida Partida = new CrearSala.Partida();
        MenuJugador.Jugador JugadorYo = new MenuJugador.Jugador();
        EditarPartida EditarPartidaForm;
        List<MenuJugador.Jugador> Jugadores = new List<MenuJugador.Jugador>();
        public List<PerfilJugador> PerfilesAbiertos = new List<PerfilJugador>();

        bool dibujando = false;
        private Image Lapiz;
        private List<Trazo> trazos = new List<Trazo>();
        private List<Point> puntosActivos = new List<Point>();
        private Color colorActual = Color.Black;
        private float grosorActual = 4.0f;
        int numeroColor = 8;

        public class Trazo
        {
            public List<Point> Puntos { get; set; }
            public Color Color { get; set; }
            public float Grosor { get; set; }

            public Trazo(List<Point> puntos, Color color, float grosor)
            {
                Puntos = puntos;
                Color = color;
                Grosor = grosor;
            }
        }

        Color[] colores = new Color[]
        {
                Color.DarkGray,
                Color.White,
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Yellow,
                Color.Orange,
                Color.Purple
        };

        public SalaPartida(Socket Servidor, MenuJugador MenuJugadorForm, CrearSala.Partida NuevaPartida, MenuJugador.Jugador PerfilJugador, bool Host)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.MenuJugadorForm = MenuJugadorForm;
            this.JugadorYo = PerfilJugador;
            this.Jugadores.Add(this.JugadorYo);
            this.Partida = NuevaPartida;
            this.Host = Host;

            BotonEnviar.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos/Enviar.png");
            BotonEnviar.BackgroundImageLayout = ImageLayout.Stretch;
            BotonBorrar.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos/Papelera.png");
            BotonBorrar.BackgroundImageLayout = ImageLayout.Stretch;
            BotonFino.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos/Fino.png");
            BotonFino.BackgroundImageLayout = ImageLayout.Stretch;
            BotonMedio.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos/Medio.png");
            BotonMedio.BackgroundImageLayout = ImageLayout.Stretch;
            BotonGrueso.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos/Grueso.png");
            BotonGrueso.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxCubo.BackgroundImage = System.Drawing.Image.FromFile(@"Colores/8.png");
            PictureBoxCubo.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxCubo.Hide();
            BotonBorrar.Hide();
            BotonFino.Hide();
            BotonMedio.Hide();
            BotonGrueso.Hide();
            DataGridViewPaleta.Hide();
            LabelDibujando.Text = "Sala de espera...";

            this.Lapiz = Image.FromFile(@"Iconos\Lapiz.png");
            this.AcceptButton = BotonEnviar;
            this.MaximizeBox = false;

            DuracionRonda = TiempoRonda;
            TimerPalabra = new Timer();
            TimerReloj = new Timer();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void SalaPartida_Load(object sender, EventArgs e)
        {
            LabelEscogePalabra.Hide();
            BotonPalabra1.Hide();
            BotonPalabra2.Hide();
            BotonPalabra3.Hide();

            LabelJugador1.Text = "";
            LabelPuntosJugador1.Text = "";
            BotonJugador1.BackgroundImage = null;
            BotonJugador1.FlatAppearance.BorderSize = 0;

            LabelJugador2.Text = "";
            LabelPuntosJugador2.Text = "";
            BotonJugador2.BackgroundImage = null;
            BotonJugador2.FlatAppearance.BorderSize = 0;

            LabelJugador3.Text = "";
            LabelPuntosJugador3.Text = "";
            BotonJugador3.BackgroundImage = null;
            BotonJugador3.FlatAppearance.BorderSize = 0;

            LabelJugador4.Text = "";
            LabelPuntosJugador4.Text = "";
            BotonJugador4.BackgroundImage = null;
            BotonJugador4.FlatAppearance.BorderSize = 0;

            TotalRondas = Partida.NumeroJugadores * Partida.NumeroRondas;

            if (Host)
            {
                BotonIniciarPartida.Text = "¡PULSA PARA INICIAR PARTIDA!";

                NumeroSala.Text = "Sala Nº" + Convert.ToString(Partida.Codigo);
                LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;
                LabelPalabra.Text = "PinturilloSO";
                LabelReloj.Text = TiempoRonda + " s";

                LabelPuntosJugador1.Text = "0 pts";
                LabelJugador1.Text = JugadorYo.Usuario;
                BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(JugadorYo.FotoPerfilUsuario);
                BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                BotonJugador1.FlatAppearance.BorderSize = 3;
                BotonJugador1.Tag = Partida.Jugadores[0].Usuario;

                BotonJugador2.Enabled = false;
                BotonJugador3.Enabled = false;
                BotonJugador4.Enabled = false;

                string mensaje = "5/1/" + Partida.MaximoJugadores + "/" + Partida.NumeroRondas +
                    "/" + Partida.Codigo + "/" + Partida.Categoria + "/" + Partida.Dificultad +
                    "/" + Partida.Privacidad + "/" + Partida.Jugadores[0].Usuario + 
                    "/" + Partida.Jugadores[0].CodigoFotoPerfil;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
            else
            {
                EditarPartidaToolStripMenuItem.Visible = false;

                BotonIniciarPartida.Text = "AÚN NO SE PUEDE DIBUJAR...";
                BotonIniciarPartida.FlatAppearance.MouseDownBackColor = SystemColors.HighlightText;
                BotonIniciarPartida.FlatAppearance.MouseOverBackColor = SystemColors.HighlightText;
                BotonIniciarPartida.Cursor = Cursors.Arrow;

                NumeroSala.Text = "Sala Nº" + Convert.ToString(Partida.Codigo);
                LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;
                LabelPalabra.Text = "PinturilloSO";
                LabelReloj.Text = TiempoRonda + " s";

                switch (Partida.NumeroJugadores)
                {
                    case 1:
                        LabelPuntosJugador1.Text = "0 pts";
                        LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                        BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                        BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador1.FlatAppearance.BorderSize = 3;
                        BotonJugador1.Tag = Partida.Jugadores[0].Usuario;

                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador2.FlatAppearance.BorderSize = 3;
                        BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                        BotonJugador1.Enabled = true;
                        break;
                    case 2:
                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador2.FlatAppearance.BorderSize = 3;
                        BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                        BotonJugador2.Enabled = true;

                        LabelPuntosJugador3.Text = "0 pts";
                        LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                        BotonJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                        BotonJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador3.FlatAppearance.BorderSize = 3;
                        BotonJugador3.Tag = Partida.Jugadores[2].Usuario;
                        BotonJugador3.Enabled = true;
                        break;
                    case 3:
                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador2.FlatAppearance.BorderSize = 3;
                        BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                        BotonJugador2.Enabled = true;

                        LabelPuntosJugador3.Text = "0 pts";
                        LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                        BotonJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                        BotonJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador3.FlatAppearance.BorderSize = 3;
                        BotonJugador3.Tag = Partida.Jugadores[2].Usuario;
                        BotonJugador3.Enabled = true;

                        LabelPuntosJugador4.Text = "0 pts";
                        LabelJugador4.Text = Partida.Jugadores[3].Usuario;
                        BotonJugador4.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[3].FotoPerfilUsuario);
                        BotonJugador4.BackgroundImageLayout = ImageLayout.Stretch;
                        BotonJugador4.FlatAppearance.BorderSize = 3;
                        BotonJugador4.Tag = Partida.Jugadores[3].Usuario;
                        BotonJugador4.Enabled = true;
                        break;
                }

                string mensaje = "5/3/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" + JugadorYo.CodigoFotoPerfil;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }

            DataGridViewPaleta.ColumnCount = 8;
            DataGridViewPaleta.RowCount = 2;
            DataGridViewPaleta.Rows[0].Height = 31;
            DataGridViewPaleta.Rows[1].Height = 31;

            for (int col = 0; col < colores.Length; col++)
            {
                DataGridViewPaleta[col, 0].Style.BackColor = colores[col];
                DataGridViewPaleta[col, 0].Style.SelectionBackColor = colores[col];
                DataGridViewPaleta[col, 0].Style.SelectionForeColor = colores[col];

                if (col == 0)
                {
                    DataGridViewPaleta[col, 1].Style.BackColor = Color.Black;
                    DataGridViewPaleta[col, 1].Style.SelectionBackColor = Color.Black;
                    DataGridViewPaleta[col, 1].Style.SelectionForeColor = Color.Black;
                }
                else 
                {
                    Color oscuro = ControlPaint.Dark(colores[col]);
                    DataGridViewPaleta[col, 1].Style.BackColor = oscuro;
                    DataGridViewPaleta[col, 1].Style.SelectionBackColor = oscuro;
                    DataGridViewPaleta[col, 1].Style.SelectionForeColor = oscuro;
                }
            }

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, PanelLienzo, new object[] { true });

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

        public void ActualizarJugadores(CrearSala.Partida Partida)
        {
            if (!this.IsDisposed && this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    Partida.PuntosJugadores = new int[Partida.NumeroJugadores];
                    this.Partida = Partida;
                    TotalRondas = Partida.NumeroJugadores * Partida.NumeroRondas;
                    LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;

                    LabelPuntosJugador1.Text = "0 pts";
                    LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                    BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                    BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                    BotonJugador1.Tag = Partida.Jugadores[0].Usuario;

                    switch (Partida.NumeroJugadores)
                    {
                        case 1:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador1.FlatAppearance.BorderSize = 3;
                            BotonJugador1.Tag = Partida.Jugadores[0].Usuario;
                            BotonJugador1.Enabled = true;
                            BotonJugador1.Show();

                            LabelJugador2.Text = "";
                            LabelPuntosJugador2.Text = "";
                            BotonJugador2.BackgroundImage = null;
                            BotonJugador2.FlatAppearance.BorderSize = 0;
                            BotonJugador2.Enabled = false;
                            PictureBoxModoJuego2.Visible = false;
                            BotonJugador2.Hide();

                            LabelJugador3.Text = "";
                            LabelPuntosJugador3.Text = "";
                            BotonJugador3.BackgroundImage = null;
                            BotonJugador3.FlatAppearance.BorderSize = 0;
                            BotonJugador3.Enabled = false;
                            PictureBoxModoJuego3.Visible = false;
                            BotonJugador3.Hide();

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            BotonJugador4.BackgroundImage = null;
                            BotonJugador4.FlatAppearance.BorderSize = 0;
                            BotonJugador4.Enabled = false;
                            PictureBoxModoJuego4.Visible = false;
                            BotonJugador4.Hide();
                            break;
                        case 2:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador1.FlatAppearance.BorderSize = 3;
                            BotonJugador1.Tag = Partida.Jugadores[0].Usuario;
                            BotonJugador1.Enabled = true;
                            BotonJugador1.Show();

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador2.FlatAppearance.BorderSize = 3;
                            BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                            BotonJugador2.Enabled = true;
                            BotonJugador2.Show();

                            LabelJugador3.Text = "";
                            LabelPuntosJugador3.Text = "";
                            BotonJugador3.BackgroundImage = null;
                            BotonJugador3.FlatAppearance.BorderSize = 0;
                            BotonJugador3.Enabled = false;
                            PictureBoxModoJuego3.Visible = false;
                            BotonJugador3.Hide();

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            BotonJugador4.BackgroundImage = null;
                            BotonJugador4.FlatAppearance.BorderSize = 0;
                            BotonJugador4.Enabled = false;
                            PictureBoxModoJuego4.Visible = false;
                            BotonJugador4.Hide();
                            break;
                        case 3:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador1.FlatAppearance.BorderSize = 3;
                            BotonJugador1.Tag = Partida.Jugadores[0].Usuario;
                            BotonJugador1.Enabled = true;
                            BotonJugador1.Show();

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador2.FlatAppearance.BorderSize = 3;
                            BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                            BotonJugador2.Enabled = true;
                            BotonJugador2.Show();

                            LabelPuntosJugador3.Text = "0 pts";
                            LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                            BotonJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                            BotonJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador3.FlatAppearance.BorderSize = 3;
                            BotonJugador3.Tag = Partida.Jugadores[2].Usuario;
                            BotonJugador3.Enabled = true;
                            BotonJugador3.Show();

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            BotonJugador4.BackgroundImage = null;
                            BotonJugador4.FlatAppearance.BorderSize = 0;
                            BotonJugador4.Enabled = false;
                            PictureBoxModoJuego4.Visible = false;
                            BotonJugador4.Hide();
                            break;

                        case 4:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            BotonJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            BotonJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador1.FlatAppearance.BorderSize = 3;
                            BotonJugador1.Tag = Partida.Jugadores[0].Usuario;
                            BotonJugador1.Enabled = true;
                            BotonJugador1.Show();

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            BotonJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            BotonJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador2.FlatAppearance.BorderSize = 3;
                            BotonJugador2.Tag = Partida.Jugadores[1].Usuario;
                            BotonJugador2.Enabled = true;
                            BotonJugador2.Show();

                            LabelPuntosJugador3.Text = "0 pts";
                            LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                            BotonJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                            BotonJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador3.FlatAppearance.BorderSize = 3;
                            BotonJugador3.Tag = Partida.Jugadores[2].Usuario;
                            BotonJugador3.Enabled = true;
                            BotonJugador3.Show();

                            LabelPuntosJugador4.Text = "0 pts";
                            LabelJugador4.Text = Partida.Jugadores[3].Usuario;
                            BotonJugador4.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[3].FotoPerfilUsuario);
                            BotonJugador4.BackgroundImageLayout = ImageLayout.Stretch;
                            BotonJugador4.FlatAppearance.BorderSize = 3;
                            BotonJugador4.Tag = Partida.Jugadores[3].Usuario;
                            BotonJugador4.Enabled = true;
                            BotonJugador4.Show();
                            break;
                    }

                    this.Jugadores = Partida.Jugadores;
                    if (Jugadores[0].Usuario == JugadorYo.Usuario && !Host)
                    {
                        Host = true;
                        if (Partida.PartidaIniciada && !PartidaTerminada)
                        {
                            PartidaTerminada = true;
                            PasarDeRonda();
                        }
                    }

                    if (!Host && !EnSala)
                    {
                        while (ColaUltimos20Mensajes.Count > 0)
                        {
                            object[] MensajeChat = ColaUltimos20Mensajes.Dequeue();
                            string Usuario = (string)MensajeChat[0];
                            string Mensaje = (string)MensajeChat[1];
                            bool Acertada = (bool)MensajeChat[2];

                            EscribirChat(Usuario, Mensaje, Acertada);
                        }
                        EnSala = true;
                    }
                }));
            }
        }

        public void EditarPartida(CrearSala.Partida Partida)
        {
            this.Partida = Partida;
        }

        public void Ultimos20Mensajes(string Usuario, string Mensaje, bool Acertada)
        {
            ColaUltimos20Mensajes.Enqueue(new object[] { Usuario, Mensaje, Acertada });
        }

        private void PanelLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            if (Pintor)
            {
                Cursor.Hide();
                dibujando = true;
                puntosActivos = new List<Point>();
                puntosActivos.Add(e.Location);

                string mensaje = "5/5/" + Partida.Codigo + "/1/" + $"{e.Location.X}/{e.Location.Y}" +
                    "/" + numeroColor + "/" + grosorActual;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        private void PanelLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            if (Pintor && dibujando)
            {
                puntosActivos.Add(e.Location);
                PanelLienzo.Invalidate();

                if ((DateTime.Now - lastSendTime).TotalMilliseconds >= sendIntervalMilliseconds)
                {
                    string mensaje = "5/5/" + Partida.Codigo + "/2/" + $"{e.Location.X}/{e.Location.Y}" +
                        "/" + numeroColor + "/" + grosorActual;
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);
                    lastSendTime = DateTime.Now;
                }
            }
        }

        private void PanelLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            if (Pintor)
            {
                Cursor.Show();
                dibujando = false;
                if (puntosActivos.Count > 1)
                {
                    trazos.Add(new Trazo(new List<Point>(puntosActivos), colorActual, grosorActual));
                }

                string mensaje = "5/5/" + Partida.Codigo + "/0/" + $"{e.Location.X}/{e.Location.Y}" +
                    "/" + numeroColor + "/" + grosorActual;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);

                puntosActivos.Clear();
                PanelLienzo.Invalidate();
            }
        }

        private void PanelLienzo_Paint(object sender, PaintEventArgs e)
        {
            foreach (var trazo in trazos)
            {
                using (Pen lapiz = new Pen(trazo.Color, trazo.Grosor))
                {
                    for (int i = 0; i < trazo.Puntos.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(lapiz, trazo.Puntos[i], trazo.Puntos[i + 1]);
                    }
                }
            }

            if (puntosActivos.Count > 1)
            {
                using (Pen lapiz = new Pen(colorActual, grosorActual))
                {
                    for (int i = 0; i < puntosActivos.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(lapiz, puntosActivos[i], puntosActivos[i + 1]);
                    }
                }
            }

            if (puntosActivos.Count > 0)
            {
                Point ultimoPunto = puntosActivos[puntosActivos.Count - 1];
                int ancho = Lapiz.Width / 3;
                int alto = Lapiz.Height / 3;
                e.Graphics.DrawImage(Lapiz, new Rectangle(ultimoPunto.X, ultimoPunto.Y - alto, ancho, alto));
            }
        }

        public void ActualizarPanel(int CodigoDibujo, int DibujoX, int DibujoY, int NumeroColor, float Grosor)
        {
            this.Invoke(new Action(() =>
            {
                if (Pintor) return;

                if (CodigoDibujo == -1 && DibujoX == -1 && DibujoY == -1 && NumeroColor == -1 && Grosor == -1)
                {
                    trazos.Clear();
                    puntosActivos.Clear();
                }
                else
                {
                    Color Color;

                    if (NumeroColor <= 7)
                        Color = colores[NumeroColor];
                    else if (NumeroColor == 8)
                        Color = Color.Black;
                    else
                        Color = ControlPaint.Dark(colores[NumeroColor - 8]);

                    this.colorActual = Color;
                    this.grosorActual = Grosor;

                    Point pt = new Point(DibujoX, DibujoY);
                    switch (CodigoDibujo)
                    {
                        case 1:
                            puntosActivos = new List<Point> { pt };
                            break;

                        case 2:
                            if (puntosActivos == null)
                            {
                                puntosActivos = new List<Point>();
                            }
                            puntosActivos.Add(pt);
                            break;

                        case 0:
                            if (puntosActivos != null && puntosActivos.Count > 1)
                            {
                                trazos.Add(new Trazo(new List<Point>(puntosActivos), colorActual, grosorActual));
                            }
                            puntosActivos.Clear();
                            break;
                    }
                }
                PanelLienzo.Invalidate();
            }));
        }

        private void DataGridViewPaleta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Color colorSeleccionado = DataGridViewPaleta[e.ColumnIndex, e.RowIndex].Style.BackColor;

                if (colorSeleccionado.A != 0)
                {
                    colorActual = colorSeleccionado;
                    numeroColor = e.ColumnIndex + (e.RowIndex * DataGridViewPaleta.ColumnCount);
                    PictureBoxCubo.BackgroundImage = System.Drawing.Image.FromFile(@"Colores/" + numeroColor + ".png");
                    PictureBoxCubo.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        private void BotonBorrar_Click(object sender, EventArgs e)
        {
            if (Pintor)
            {
                trazos.Clear();
                puntosActivos.Clear();
                PanelLienzo.Invalidate();

                string mensaje = "5/5/" + Partida.Codigo + "/-1/-1/-1/-1/-1/-1";
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        private void BotonFino_Click(object sender, EventArgs e)
        {
            if (Pintor)
                grosorActual = 4.0f;
        }

        private void BotonMedio_Click(object sender, EventArgs e)
        {
            if (Pintor)
                grosorActual = 6.0f;
        }

        private void BotonGrueso_Click(object sender, EventArgs e)
        {
            if (Pintor)
                grosorActual = 12.0f;
        }

        public void EscribirChat(string Usuario, string mensaje, bool Acertada)
        {
            this.Invoke(new Action(() =>
            {
                RichTextBoxChat.SelectionStart = RichTextBoxChat.TextLength;
                RichTextBoxChat.SelectionLength = 0;

                if (!string.IsNullOrEmpty(mensaje) && Acertada && Partida.PartidaIniciada)
                {
                    RichTextBoxChat.SelectionColor = Color.DarkOrange;
                    RichTextBoxChat.SelectionFont = new Font(RichTextBoxChat.Font, FontStyle.Bold);

                    mensaje = $"* ¡EL JUGADOR {Usuario} HA ACERTADO LA PALABRA! *";

                    RichTextBoxChat.AppendText($"{mensaje}");
                    RichTextBoxChat.AppendText(Environment.NewLine);

                    if (JugadorYo.Usuario == Usuario)
                    {
                        PalabraAcertada = true;
                        LabelPalabra.Text = Palabra;
                        CalcularPuntos();
                    }

                    RichTextBoxChat.SelectionStart = RichTextBoxChat.Text.Length;
                    RichTextBoxChat.Refresh();
                    RichTextBoxChat.ScrollToCaret();

                    AciertosRonda++;
                    if (AciertosRonda == Partida.NumeroJugadores - 1)
                    {
                        PasarDeRonda();
                    }

                    return;
                }

                try
                {
                    if (Usuario == Jugadores[0].Usuario)
                    {
                        RichTextBoxChat.SelectionColor = Color.DarkBlue;
                    }
                    else if (Usuario == Jugadores[1].Usuario)
                    {
                        RichTextBoxChat.SelectionColor = Color.DarkMagenta;
                    }
                    else if (Usuario == Jugadores[2].Usuario)
                    {
                        RichTextBoxChat.SelectionColor = Color.DarkGreen;
                    }
                    else if (Usuario == Jugadores[3].Usuario)
                    {
                        RichTextBoxChat.SelectionColor = Color.DarkRed;
                    }
                    else if (Usuario != "ParaTodos")
                    {
                        RichTextBoxChat.SelectionColor = Color.DarkGray;
                    }

                    RichTextBoxChat.SelectionFont = new Font(RichTextBoxChat.Font, FontStyle.Bold);
                    RichTextBoxChat.AppendText($"{Usuario}: ");

                    RichTextBoxChat.SelectionColor = RichTextBoxChat.ForeColor;
                    RichTextBoxChat.SelectionFont = new Font(RichTextBoxChat.Font, FontStyle.Regular);
                    RichTextBoxChat.AppendText(mensaje);
                    RichTextBoxChat.AppendText(Environment.NewLine);

                    RichTextBoxChat.SelectionStart = RichTextBoxChat.Text.Length;
                    RichTextBoxChat.Refresh();
                    RichTextBoxChat.ScrollToCaret();
                }
                catch(Exception ex)
                {
                    RichTextBoxChat.SelectionFont = new Font(RichTextBoxChat.Font, FontStyle.Bold);
                    RichTextBoxChat.AppendText($"{mensaje}");
                    RichTextBoxChat.AppendText(Environment.NewLine);

                    RichTextBoxChat.SelectionStart = RichTextBoxChat.Text.Length;
                    RichTextBoxChat.Refresh();
                    RichTextBoxChat.ScrollToCaret();
                }
            }));
        }

        private void BotonEnviar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxEscribirChat.Text))
            {
                int Acertada;
                if (PalabraAcertada)
                    Acertada = 1;
                else
                    Acertada = 0;

                if (string.IsNullOrEmpty(Palabra))
                    Palabra = "PartidaSinIniciar";

                string mensaje = "5/4/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" + 
                    TextBoxEscribirChat.Text + "/" + Acertada;
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
                Servidor.Send(msg);

                TextBoxEscribirChat.Clear();
            }
        }

        private void RichTextBoxChat_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private async void BotonIniciarPartida_Click(object sender, EventArgs e)
        {
            if (Partida.NumeroJugadores >= 2 && Host && !PartidaTerminada)
            {
                PedirPalabra();
            }
            else if (Partida.NumeroJugadores >= 2 && Host && PartidaTerminada)
            {
                PartidaTerminada = false;

                RondaActual = 0;
                AciertosRonda = 0;

                LabelPuntosJugador1.Text = "0 pts";
                LabelPuntosJugador2.Text = "0 pts";
                switch (Partida.NumeroJugadores)
                {
                    case 3:
                        LabelPuntosJugador4.Text = "0 pts";
                        break;

                    case 4:
                        LabelPuntosJugador3.Text = "0 pts";
                        LabelPuntosJugador4.Text = "0 pts";
                        break;
                }

                string mensaje = "5/12/" + Partida.Codigo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);

                await Task.Delay(50); // ms

                PedirPalabra();
            }
        }

        public void PedirPalabra()
        {
            string mensaje = "5/6/" + Partida.Codigo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        public void RecibirPalabras(string JugadorPintor, string Palabra1, string Palabra2, string Palabra3)
        {
            this.Invoke(new Action(() =>
            {
                if (PartidaTerminada)
                    PartidaTerminada = false;

                AsignarPintor(JugadorPintor);

                if (!PartidaTerminada)
                {
                    LabelPalabra.Text = "Esperando...";
                    BotonIniciarPartida.Hide();

                    if (Pintor)
                    {
                        LabelEscogePalabra.Show();

                        BotonPalabra1.Text = Palabra1;
                        BotonPalabra2.Text = Palabra2;
                        BotonPalabra3.Text = Palabra3;

                        BotonPalabra1.Show();
                        BotonPalabra2.Show();
                        BotonPalabra3.Show();
                    }
                    else
                    {
                        BotonIniciarPartida.Text = "Escogiendo palabra...";
                        BotonIniciarPartida.FlatAppearance.MouseDownBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.FlatAppearance.MouseOverBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.Cursor = Cursors.Arrow;
                        BotonIniciarPartida.Show();
                    }
                }
            }));
        }

        private void EnviarPalabra(string Palabra)
        {
            string mensaje = "5/7/" + Partida.Codigo + "/" + Palabra;
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void BotonPalabra1_Click(object sender, EventArgs e)
        {
            LabelEscogePalabra.Hide();
            BotonPalabra1.Hide();
            BotonPalabra2.Hide();
            BotonPalabra3.Hide();

            EnviarPalabra(BotonPalabra1.Text);
        }

        private void BotonPalabra2_Click(object sender, EventArgs e)
        {
            LabelEscogePalabra.Hide();
            BotonPalabra1.Hide();
            BotonPalabra2.Hide();
            BotonPalabra3.Hide();

            EnviarPalabra(BotonPalabra2.Text);
        }

        private void BotonPalabra3_Click(object sender, EventArgs e)
        {
            LabelEscogePalabra.Hide();
            BotonPalabra1.Hide();
            BotonPalabra2.Hide();
            BotonPalabra3.Hide();

            EnviarPalabra(BotonPalabra3.Text);
        }

        public void IniciarRonda(string Palabra, List<int> indicesAleatorios)
        {
            this.Invoke(new Action(() =>
            {
                if (!Partida.PartidaIniciada)
                    Partida.PartidaIniciada = true;

                this.Palabra = Palabra;
                this.indicesAleatorios = indicesAleatorios;
                PalabraOculta = new string('_', Palabra.Length).ToCharArray();
                BotonIniciarPartida.Hide();

                if (Pintor)
                {
                    LabelPalabra.Text = Palabra;
                    PictureBoxCubo.Show();
                    BotonBorrar.Show();
                    BotonFino.Show();
                    BotonMedio.Show();
                    BotonGrueso.Show();
                    DataGridViewPaleta.Show();
                    LabelDibujando.Hide();
                }
                else 
                {
                    LabelDibujando.Text = "Pintor Dibujando...";
                    LabelPalabra.Text = new string(PalabraOculta);

                    TiempoPorLetra = DuracionRonda / (float)Palabra.Length;
                    TimerPalabra.Interval = TiempoRonda * 1000 / Palabra.Length;
                    CronometroPalabra.Restart();
                    TimerPalabra.Tick += TimerPalabra_Tick;
                    TimerPalabra.Start();
                }

                CronometroRonda.Restart();
                TimerReloj.Interval = 1000;
                TimerReloj.Tick += TimerReloj_Tick;
                TimerReloj.Start();
            }));
        }


        private async void PasarDeRonda()
        {
            TimerReloj.Stop();
            TimerPalabra.Stop();
            LabelPalabra.Text = Palabra;

            TiempoRonda = 99;
            AciertosRonda = 0;
            LetrasMostradas = 0;
            TiempoPorLetra = 0;

            if (Pintor)
            {
                Pintor = false;
                PictureBoxCubo.Hide();
                BotonBorrar.Hide();
                BotonFino.Hide();
                BotonMedio.Hide();
                BotonGrueso.Hide();
                DataGridViewPaleta.Hide();
            }
            if (PalabraAcertada)
                PalabraAcertada = false;

            await Task.Delay(5000); // ms

            trazos.Clear();
            puntosActivos.Clear();
            PanelLienzo.Invalidate();

           
                PedirPalabra();
        }

        public void AsignarPintor(string JugadorPintor)
        {
            RondaActual++;

            if (RondaActual > TotalRondas)
            {
                this.Invoke(new Action(() =>
                {
                    LabelDibujando.Text = "Sala de espera...";
                    LabelPalabra.Text = "Esperando...";
                    Partida.PartidaIniciada = false;
                    PartidaTerminada = true;

                    if (Host)
                    {
                        BotonIniciarPartida.Font = new Font(BotonIniciarPartida.Font.FontFamily, 44, BotonIniciarPartida.Font.Style);
                        BotonIniciarPartida.Text = "¡PULSA PARA JUGAR DE NUEVO!";
                        BotonIniciarPartida.FlatAppearance.MouseDownBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.FlatAppearance.MouseOverBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.Cursor = Cursors.Hand;
                        BotonIniciarPartida.Show();

                        string mensaje = "5/11/" + Partida.Codigo;
                        byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
                        Servidor.Send(msg);
                    }
                    else
                    {
                        BotonIniciarPartida.Font = new Font(BotonIniciarPartida.Font.FontFamily, 44, BotonIniciarPartida.Font.Style);
                        BotonIniciarPartida.Text = "¡LA PARTIDA HA TERMINADO!";
                        BotonIniciarPartida.FlatAppearance.MouseDownBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.FlatAppearance.MouseOverBackColor = SystemColors.HighlightText;
                        BotonIniciarPartida.Cursor = Cursors.Arrow;
                        BotonIniciarPartida.Show();
                    }
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    UsuarioPintor = JugadorPintor;
                    
                    LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;
                    MensajeChat = $"RONDA {RondaActual}... ¡LE TOCA DIBUJAR A {JugadorPintor}! ";
                    EscribirChat("ParaTodos", MensajeChat, false);

                    if (JugadorPintor == JugadorYo.Usuario)
                    {
                        Pintor = true;
                    }
                    else
                    {
                        Pintor = false;
                        PictureBoxCubo.Hide();
                    }
                    Pintores.Add(JugadorPintor);

                    for (int i = 0; i < Partida.NumeroJugadores; i++)
                    {
                        switch(i)
                        {
                            case 0:
                                if (Partida.Jugadores[i].Usuario == UsuarioPintor)
                                {
                                    PictureBoxModoJuego1.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Pintor.png");
                                    PictureBoxModoJuego1.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                else
                                {
                                    PictureBoxModoJuego1.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Mecenas.png");
                                    PictureBoxModoJuego1.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                break;

                            case 1:
                                if (Partida.Jugadores[i].Usuario == UsuarioPintor)
                                {
                                    PictureBoxModoJuego2.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Pintor.png");
                                    PictureBoxModoJuego2.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                else
                                {
                                    PictureBoxModoJuego2.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Mecenas.png");
                                    PictureBoxModoJuego2.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                break;

                            case 2:
                                if (Partida.Jugadores[i].Usuario == UsuarioPintor)
                                {
                                    PictureBoxModoJuego3.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Pintor.png");
                                    PictureBoxModoJuego3.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                else
                                {
                                    PictureBoxModoJuego3.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Mecenas.png");
                                    PictureBoxModoJuego3.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                break;

                            case 3:
                                if (Partida.Jugadores[i].Usuario == UsuarioPintor)
                                {
                                    PictureBoxModoJuego4.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Pintor.png");
                                    PictureBoxModoJuego4.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                else
                                {
                                    PictureBoxModoJuego4.BackgroundImage = System.Drawing.Image.FromFile(@"FotosJuego/Mecenas.png");
                                    PictureBoxModoJuego4.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                break;
                        }
                    }
                }));
            }
        }

        private void TimerReloj_Tick(object sender, EventArgs e)
        {
            TiempoRestante = DuracionRonda - (int)CronometroRonda.Elapsed.TotalSeconds;

            if (TiempoRestante <= 0)
            {
                LabelReloj.Text = "0 s";
                PasarDeRonda();
            }
            else
            {
                LabelReloj.Text = TiempoRestante + " s";
            }
        }

        private void TimerPalabra_Tick(object sender, EventArgs e)
        {
            int LetrasOcultas = (int)(CronometroPalabra.Elapsed.TotalSeconds / TiempoPorLetra);

            while (LetrasMostradas < LetrasOcultas && LetrasMostradas < Palabra.Length)
            {
                int indice = indicesAleatorios[LetrasMostradas];
                PalabraOculta[indice] = Palabra[indice];
                LetrasMostradas++;
            }

            LabelPalabra.Text = new string(PalabraOculta);

            if (LetrasMostradas >= Palabra.Length)
            {
                TimerPalabra.Stop();
            }
        }

        private void CalcularPuntos()
        {
            string mensaje = "5/9/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" + TiempoRestante;
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        public void SumarPuntos(string Usuario, string Puntos, string Pintor, string PuntosPintor)
        {
            this.Invoke(new Action(() =>
            {
                for (int i = 0; i < Partida.NumeroJugadores; i++)
                {
                    if (Partida.Jugadores[i].Usuario == Usuario)
                    {
                        switch (i)
                        {
                            case 0:
                                LabelPuntosJugador1.Text = Puntos + " pts";
                                break;

                            case 1:
                                LabelPuntosJugador2.Text = Puntos + " pts";
                                break;

                            case 2:
                                LabelPuntosJugador3.Text = Puntos + " pts";
                                break;

                            case 3:
                                LabelPuntosJugador4.Text = Puntos + " pts";
                                break;
                        }
                    }
                    else if (Partida.Jugadores[i].Usuario == Pintor)
                    {
                        switch (i)
                        {
                            case 0:
                                LabelPuntosJugador1.Text = PuntosPintor + " pts";
                                break;

                            case 1:
                                LabelPuntosJugador2.Text = PuntosPintor + " pts";
                                break;

                            case 2:
                                LabelPuntosJugador3.Text = PuntosPintor + " pts";
                                break;

                            case 3:
                                LabelPuntosJugador4.Text = PuntosPintor + " pts";
                                break;
                        }
                    }
                }
            }));
        }

        public string DameCodigoDeSala()
        {
            return Partida.Codigo;
        }

        private void ButtonJugador1_Click(object sender, EventArgs e)
        {
            string UsuarioPerfil = (string)BotonJugador1.Tag;
            VerPerfilJugador(UsuarioPerfil);
        }

        private void ButtonJugador2_Click(object sender, EventArgs e)
        {
            string UsuarioPerfil = (string)BotonJugador2.Tag;
            VerPerfilJugador(UsuarioPerfil);
        }

        private void ButtonJugador3_Click(object sender, EventArgs e)
        {
            string UsuarioPerfil = (string)BotonJugador3.Tag;
            VerPerfilJugador(UsuarioPerfil);
        }

        private void ButtonJugador4_Click(object sender, EventArgs e)
        {
            string UsuarioPerfil = (string)BotonJugador4.Tag;
            VerPerfilJugador(UsuarioPerfil);
        }

        private void VerPerfilJugador(string UsuarioPerfil)
        {
            int NumeroJugadores = Partida.NumeroJugadores;
            MenuJugador.Jugador PerfilJugador;
            bool PerfilAbierto = false;

            for (int i = 0; i < PerfilesAbiertos.Count; i++)
            {
                if (PerfilesAbiertos[i].DameUsuarioPerfil() == UsuarioPerfil)
                {
                    PerfilAbierto = true;
                    break;
                }
            }

            if (!PerfilAbierto)
            {
                for (int i = 0; i < NumeroJugadores; i++)
                {
                    if (Partida.Jugadores[i].Usuario == UsuarioPerfil)
                    {
                        PerfilJugador = Partida.Jugadores[i];
                        PerfilJugador PerfilJugadorForm = new PerfilJugador(Servidor, this, Partida, PerfilJugador, JugadorYo, Host);
                        PerfilesAbiertos.Add(PerfilJugadorForm);
                        PerfilJugadorForm.Show();
                        break;
                    }
                }
            }
        }

        public void DatosPerfilAbierto(string Usuario, string Ranking, string Victorias, string FraseFavorita)
        {
            for (int i = 0; i < PerfilesAbiertos.Count; i++)
            {
                if (PerfilesAbiertos[i].DameUsuarioPerfil() == Usuario)
                {
                    PerfilesAbiertos[i].PonerRankingVictorias(Ranking, Victorias, FraseFavorita);
                    break;
                }
            }
        }

        public void Expulsion(string Usuario)
        {
            this.Invoke(new Action(() =>
            {
                if (JugadorYo.Usuario == Usuario)
                {
                    MessageBox.Show("Has sido expulsado de la sala.");
                    this.Close();
                }
            }));
        }

        private void ComoJugarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComoJugar ComoJugarForm = new ComoJugar();
            ComoJugarForm.Show();
        }

        private void EditarPartidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Host && !Partida.PartidaIniciada)
            {
                this.EditarPartidaForm = new EditarPartida(Servidor, this, JugadorYo, Partida);
                this.EditarPartidaForm.ShowDialog();
            }
        }

        private void InvitarJugadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.InvitarForm = new Invitar(Servidor, MenuJugadorForm, JugadorYo, Partida);
            this.InvitarForm.ShowDialog();
        }

        private void AbandonarPartidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Resultado = MessageBox.Show(
            $"¿Seguro que quieres abandonar la partida?",
            "Abandonar partida",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (Resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void SalaPartida_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje = "5/0/" + Partida.Codigo + "/" + JugadorYo.Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            Partida.Jugadores.Remove(JugadorYo);
            MenuJugadorForm.SalasDeJuego.Remove(this);
        }
    }
}