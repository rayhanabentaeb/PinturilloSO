using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        int TiempoRonda = 90;
        int DuracionRonda;
        int TotalRondas;
        int RondaActual = 0;
        int AciertosRonda = 0;

        bool Host;
        bool Pintor = false;
        bool PalabraAcertada = false;

        string Palabra = "";
        string MensajeChat;

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
        CrearSala.Partida Partida = new CrearSala.Partida();
        MenuJugador.Jugador JugadorYo = new MenuJugador.Jugador();
        List<MenuJugador.Jugador> Jugadores = new List<MenuJugador.Jugador>();

        bool dibujando = false;
        List<List<Point>> trazos = new List<List<Point>>();
        List<Point> puntosActivos = new List<Point>();

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
            this.AcceptButton = BotonEnviar;

            DuracionRonda = TiempoRonda;
            TimerPalabra = new Timer();
            TimerReloj = new Timer();
        }

        private void SalaPartida_Load(object sender, EventArgs e)
        {
            LabelJugador1.Text = "";
            LabelPuntosJugador1.Text = "";
            PictureBoxJugador1.BackgroundImage = null;
            PictureBoxJugador1.BorderStyle = BorderStyle.None;

            LabelJugador2.Text = "";
            LabelPuntosJugador2.Text = "";
            PictureBoxJugador2.BackgroundImage = null;
            PictureBoxJugador2.BorderStyle = BorderStyle.None;

            LabelJugador3.Text = "";
            LabelPuntosJugador3.Text = "";
            PictureBoxJugador3.BackgroundImage = null;
            PictureBoxJugador3.BorderStyle = BorderStyle.None;

            LabelJugador4.Text = "";
            LabelPuntosJugador4.Text = "";
            PictureBoxJugador4.BackgroundImage = null;
            PictureBoxJugador4.BorderStyle = BorderStyle.None;

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
                PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(JugadorYo.FotoPerfilUsuario);
                PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                string mensaje = "5/1/" + Partida.MaximoJugadores + "/" + Partida.NumeroRondas +
                    "/" + Partida.Codigo + "/" + Partida.Categoria + "/" + Partida.Dificultad +
                    "/" + Partida.Privacidad + "/" + Partida.Jugadores[0].Usuario + 
                    "/" + Partida.Jugadores[0].CodigoFotoPerfil;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
            else
            {
                BotonIniciarPartida.Text = "AÚN NO SE PUEDE DIBUJAR...";
                BotonIniciarPartida.FlatAppearance.MouseDownBackColor = SystemColors.HighlightText;
                BotonIniciarPartida.FlatAppearance.MouseOverBackColor = SystemColors.HighlightText;
                BotonIniciarPartida.Cursor = Cursors.Arrow;

                NumeroSala.Text = "Sala Nº" + Convert.ToString(Partida.Codigo);
                LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;
                LabelPalabra.Text = "PinturilloSO";
                LabelReloj.Text = TiempoRonda + " s";

                LabelPuntosJugador1.Text = "0 pts";
                LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;

                switch (Partida.NumeroJugadores)
                {
                    case 1:
                        LabelPuntosJugador1.Text = "0 pts";
                        LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                        PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                        PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case 2:
                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;

                        LabelPuntosJugador3.Text = "0 pts";
                        LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                        PictureBoxJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                        PictureBoxJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador3.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case 3:
                        LabelPuntosJugador2.Text = "0 pts";
                        LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                        PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                        PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;

                        LabelPuntosJugador3.Text = "0 pts";
                        LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                        PictureBoxJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                        PictureBoxJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador3.BorderStyle = BorderStyle.FixedSingle;

                        LabelPuntosJugador4.Text = "0 pts";
                        LabelJugador4.Text = Partida.Jugadores[3].Usuario;
                        PictureBoxJugador4.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[3].FotoPerfilUsuario);
                        PictureBoxJugador4.BackgroundImageLayout = ImageLayout.Stretch;
                        PictureBoxJugador4.BorderStyle = BorderStyle.FixedSingle;
                        break;
                }

                string mensaje = "5/3/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" + JugadorYo.CodigoFotoPerfil;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, PanelLienzo, new object[] { true });
        }

        public void ActualizarJugadores(CrearSala.Partida Partida)
        {
            if (!this.IsDisposed && this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    this.Partida = Partida;
                    TotalRondas = Partida.NumeroJugadores * Partida.NumeroRondas;
                    LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;

                    LabelPuntosJugador1.Text = "0 pts";
                    LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                    PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                    PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;

                    switch (Partida.NumeroJugadores)
                    {
                        case 1:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                            LabelJugador2.Text = "";
                            LabelPuntosJugador2.Text = "";
                            PictureBoxJugador2.BackgroundImage = null;
                            PictureBoxJugador2.BorderStyle = BorderStyle.None;

                            LabelJugador3.Text = "";
                            LabelPuntosJugador3.Text = "";
                            PictureBoxJugador3.BackgroundImage = null;
                            PictureBoxJugador3.BorderStyle = BorderStyle.None;

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            PictureBoxJugador4.BackgroundImage = null;
                            PictureBoxJugador4.BorderStyle = BorderStyle.None;
                            break;
                        case 2:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;

                            LabelJugador3.Text = "";
                            LabelPuntosJugador3.Text = "";
                            PictureBoxJugador3.BackgroundImage = null;
                            PictureBoxJugador3.BorderStyle = BorderStyle.None;

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            PictureBoxJugador4.BorderStyle = BorderStyle.None;
                            break;
                        case 3:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador3.Text = "0 pts";
                            LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                            PictureBoxJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                            PictureBoxJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador3.BorderStyle = BorderStyle.FixedSingle;

                            LabelJugador4.Text = "";
                            LabelPuntosJugador4.Text = "";
                            PictureBoxJugador4.BackgroundImage = null;
                            PictureBoxJugador4.BorderStyle = BorderStyle.None;
                            break;

                        case 4:
                            LabelPuntosJugador1.Text = "0 pts";
                            LabelJugador1.Text = Partida.Jugadores[0].Usuario;
                            PictureBoxJugador1.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[0].FotoPerfilUsuario);
                            PictureBoxJugador1.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador1.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador2.Text = "0 pts";
                            LabelJugador2.Text = Partida.Jugadores[1].Usuario;
                            PictureBoxJugador2.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[1].FotoPerfilUsuario);
                            PictureBoxJugador2.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador2.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador3.Text = "0 pts";
                            LabelJugador3.Text = Partida.Jugadores[2].Usuario;
                            PictureBoxJugador3.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[2].FotoPerfilUsuario);
                            PictureBoxJugador3.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador3.BorderStyle = BorderStyle.FixedSingle;

                            LabelPuntosJugador4.Text = "0 pts";
                            LabelJugador4.Text = Partida.Jugadores[3].Usuario;
                            PictureBoxJugador4.BackgroundImage = System.Drawing.Image.FromFile(Partida.Jugadores[3].FotoPerfilUsuario);
                            PictureBoxJugador4.BackgroundImageLayout = ImageLayout.Stretch;
                            PictureBoxJugador4.BorderStyle = BorderStyle.FixedSingle;
                            break;
                    }

                    this.Jugadores = Partida.Jugadores;
                    if (Jugadores[0].Usuario == JugadorYo.Usuario)
                    {
                        Host = true;
                    }
                }));
            }
        }

        private void PanelLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            if (Pintor)
            {
                dibujando = true;
                puntosActivos = new List<Point>();
                puntosActivos.Add(e.Location);

                string mensaje = "5/5/" + Partida.Codigo + "/1/" + $"{e.Location.X}/{e.Location.Y}";
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
                    string mensaje = "5/5/" + Partida.Codigo + "/2/" + $"{e.Location.X}/{e.Location.Y}";
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
                dibujando = false;
                if (puntosActivos.Count > 1)
                {
                    trazos.Add(new List<Point>(puntosActivos));
                }

                string mensaje = "5/5/" + Partida.Codigo + "/0/" + $"{e.Location.X}/{e.Location.Y}";
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
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

        public void ActualizarPanel(int CodigoDibujo, int DibujoX, int DibujoY)
        {
            this.Invoke(new Action(() =>
            {
                if (Pintor) return;

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
                            trazos.Add(new List<Point>(puntosActivos));
                        }
                        puntosActivos.Clear();
                        break;
                }
                PanelLienzo.Invalidate();
            }));
        }

        public void EscribirChat(string Usuario, string mensaje)
        {
            this.Invoke(new Action(() =>
            {
                RichTextBoxChat.SelectionStart = RichTextBoxChat.TextLength;
                RichTextBoxChat.SelectionLength = 0;

                if (!string.IsNullOrEmpty(mensaje) &&
                    string.Equals(mensaje, Palabra, StringComparison.OrdinalIgnoreCase) &&
                    Partida.PartidaIniciada)
                {
                    RichTextBoxChat.SelectionColor = Color.DarkOrange;
                    RichTextBoxChat.SelectionFont = new Font(RichTextBoxChat.Font, FontStyle.Bold);

                    if (JugadorYo.Usuario == Usuario)
                    {
                        PalabraAcertada = true;
                        LabelPalabra.Text = Palabra;
                    }

                    mensaje = $"* ¡EL JUGADOR '{Usuario}' HA ACERTADO LA PALABRA! *";

                    RichTextBoxChat.AppendText($"{mensaje}");
                    RichTextBoxChat.AppendText(Environment.NewLine);

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
                if (string.Equals(TextBoxEscribirChat.Text, Palabra, StringComparison.OrdinalIgnoreCase) &&
                    Partida.PartidaIniciada && PalabraAcertada || Pintor)
                {
                    MensajeChat = new string('*', Palabra.Length);
                }
                else
                {
                    MensajeChat = TextBoxEscribirChat.Text;
                }

                EscribirChat(JugadorYo.Usuario, MensajeChat);

                string mensaje = "5/4/" + Partida.Codigo + "/" + JugadorYo.Usuario + "/" + MensajeChat;
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
                Servidor.Send(msg);

                TextBoxEscribirChat.Clear();
            }
        }

        private void RichTextBoxChat_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void BotonIniciarPartida_Click(object sender, EventArgs e)
        {
            if (Partida.NumeroJugadores >= 2 && Host)
            {
                PedirPalabra();
            }
        }

        public void PedirPalabra()
        {
            string mensaje = "5/6/" + Partida.Codigo;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        public void RecibirPalabra(string Palabra)
        {
            this.Invoke(new Action(() =>
            {
                this.Palabra = Palabra;

                bool PalabraEncontrada = false;
                for (int i = 0; i < PalabrasPorAcertar.Count; i++)
                {
                    if (PalabrasPorAcertar[i] == Palabra)
                    {
                        Palabra = "";
                        PalabraEncontrada = true;
                        break;
                    }
                }

                if (!PalabraEncontrada)
                {
                    PalabrasPorAcertar.Add(Palabra);

                    Random random = new Random();
                    int numeroRandom = random.Next(0, Partida.NumeroJugadores);

                    if (Pintores.Count > 0 && Partida.Jugadores[numeroRandom].Usuario == Pintores[Pintores.Count - 1]
                    || Pintores.Count(Jugador => Jugador.Equals(Partida.Jugadores[numeroRandom].Usuario)) > Partida.NumeroRondas)
                    {
                        while (Partida.Jugadores[numeroRandom].Usuario == Pintores[Pintores.Count - 1]
                        || Pintores.Count(Jugador => Jugador.Equals(Partida.Jugadores[numeroRandom].Usuario)) > Partida.NumeroRondas)
                        {
                            random = new Random();
                            numeroRandom = random.Next(0, Partida.NumeroJugadores);
                        }
                    }

                    indicesAleatorios.Clear();
                    for (int j = 0; j < Palabra.Length; j++)
                    {
                        indicesAleatorios.Add(j);
                    }

                    Random rand = new Random();
                    indicesAleatorios = indicesAleatorios.OrderBy(x => rand.Next()).ToList();
                    string indicesComoTexto = string.Join("/", indicesAleatorios);

                    string mensaje = "5/7/" + Partida.Codigo + "/" + Palabra + "/" + Partida.Jugadores[numeroRandom].Usuario
                    + "/" + indicesAleatorios.Count + "/" + indicesComoTexto;
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(mensaje);
                    Servidor.Send(msg);
                }
                else
                {
                    PedirPalabra();
                }
            }));
        }

        public void IniciarRonda(string Palabra, string JugadorPintor, List<int> indicesAleatorios)
        {
            this.Invoke(new Action(() =>
            {
                if (!Partida.PartidaIniciada)
                    Partida.PartidaIniciada = true;

                RondaActual++;
                if (RondaActual > TotalRondas)
                {
                    MessageBox.Show("PARTIDA ACABADA");
                    this.Close();
                }
                else
                {
                    this.Palabra = Palabra;
                    this.indicesAleatorios = indicesAleatorios;
                    PalabraOculta = new string('_', Palabra.Length).ToCharArray();

                    AsignarPintor(JugadorPintor);
                    BotonIniciarPartida.Hide();

                    LabelRonda.Text = "Ronda: " + RondaActual + "/" + TotalRondas;

                    CronometroRonda.Restart();
                    TimerReloj.Interval = 1000; // ms
                    TimerReloj.Tick += TimerReloj_Tick;
                    TimerReloj.Start();
                }

                MensajeChat = $"RONDA {RondaActual}... ¡LE TOCA DIBUJAR A '{JugadorPintor}'! ";
                EscribirChat("ParaTodos", MensajeChat);
            }));
        }

        private void PasarDeRonda()
        {
            TimerReloj.Stop();
            TimerPalabra.Stop();
            LabelPalabra.Text = Palabra;

            TiempoRonda = 90;
            AciertosRonda = 0;
            LetrasMostradas = 0;
            TiempoPorLetra = 0;

            Pintor = false;
            PalabraAcertada = false;

            switch (Partida.NumeroJugadores)
            {
                case 2:
                    LabelPuntosJugador1.Text = "0 pts";
                    LabelPuntosJugador2.Text = "0 pts";
                    break;

                case 3:
                    LabelPuntosJugador1.Text = "0 pts";
                    LabelPuntosJugador2.Text = "0 pts";
                    LabelPuntosJugador3.Text = "0 pts";
                    break;

                case 4:
                    LabelPuntosJugador1.Text = "0 pts";
                    LabelPuntosJugador2.Text = "0 pts";
                    LabelPuntosJugador3.Text = "0 pts";
                    LabelPuntosJugador4.Text = "0 pts";
                    break;
            }

            if (Host)
                PedirPalabra();
        }

        public void AsignarPintor(string JugadorPintor)
        {
            this.Invoke(new Action(() =>
            {
                if (JugadorPintor == JugadorYo.Usuario)
                {
                    Pintor = true;
                    LabelPalabra.Text = Palabra;
                }
                else
                {
                    Pintor = false;
                    LabelPalabra.Text = new string(PalabraOculta);

                    TiempoPorLetra = DuracionRonda / (float)Palabra.Length;
                    TimerPalabra.Interval = TiempoRonda * 1000 / Palabra.Length;
                    CronometroPalabra.Restart();
                    TimerPalabra.Tick += TimerPalabra_Tick;
                    TimerPalabra.Start();
                }

                Pintores.Add(JugadorPintor);
            }));
        }

        private void TimerReloj_Tick(object sender, EventArgs e)
        {
            int TiempoRestante = DuracionRonda - (int)CronometroRonda.Elapsed.TotalSeconds;

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

        //public bool EstadoPartida()
        //{
        //    if (Partida.PartidaIniciada)
        //        return true;
        //    return false;
        //}

        //public bool HostSala()
        //{
        //    if (Host)
        //        return true;
        //    return false;
        //}

        private void SalaPartida_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje = "5/0/" + Partida.Codigo + "/" + JugadorYo.Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            Partida.Jugadores.Remove(JugadorYo);
            MenuJugadorForm.SalasDeJuego.Remove(this);
        }

        private void AbandonarPartidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        private void invitarJugadoresToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            Invitar formInvitar = new Invitar(Servidor, MenuJugadorForm, JugadorYo, Partida);
            formInvitar.ShowDialog();

        }
        //private void invitarJugadoresToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Invitar formInvitar = new Invitar(Servidor, MenuJugadorForm, JugadorYo);
        //    formInvitar.Show(); // Muestra la ventana
        //                        // Si quieres que sea modal (que bloquee la ventana actual hasta cerrarlo), usa:
        //                        // formInvitar.ShowDialog();
        //}
    }
}

