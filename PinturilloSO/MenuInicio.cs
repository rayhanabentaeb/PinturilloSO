using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using NAudio.Wave;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Forms;
using static PinturilloSO.CrearSala;
using static PinturilloSO.MenuInicio;
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class MenuInicio : Form
    {

        Socket Servidor;
        Thread atender;
        Thread TMenuJugador;
        // máquina virtual
        //int Puerto = 9070;
        //string IP = "192.168.56.102";

        //// shiva
        int Puerto = 50086;
        string IP = "10.4.119.5";

        delegate void Delegado();
        delegate int DelegadoNumeroForm(string CodigoSala);

        string Correo;
        string Usuario;
        string Contrasena;
        string FotoPerfil;
        int CodigoFotoPerfil;
        string FraseFavorita;
        CrearSala.Partida Partida = new CrearSala.Partida();
        MenuJugador.Jugador JugadorYo = new MenuJugador.Jugador();

        bool ConexionEstablecida = false;
        bool ContrasenaOculta = true;
        string ContrasenaEscrita = "";

        private MenuJugador MenuJugadorForm;
        private CrearUsuario CrearUsuarioForm;
        private RecuperarCuenta RecuperarCuentaForm;
        private CrearSala CrearSalaForm;
        private SalaPartida SalaPartidaForm;
        private ControlScaler _scaler;
        private WaveOutEvent output;
        private AudioFileReader reader;

        public MenuInicio(Socket Servidor = null)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.AcceptButton = BotonAceptar;

            this.BackgroundImage = System.Drawing.Image.FromFile(@"FondosPantalla\Fondo3.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Desconectado.png");
            PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\VerContrasena.png");
            BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            BotonAyuda.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Ayuda.png");
            BotonAyuda.BackgroundImageLayout = ImageLayout.Stretch;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void MenuInicio_Load(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button boton)
                {
                    boton.Click += Boton_ClickConSonido;
                }
            }

            //string ruta = @"Audios\Musica.wav";
            //reader = new AudioFileReader(ruta);
            //var loop = new LoopStream(reader);

            //output = new WaveOutEvent();
            //output.Init(loop);
            //reader.Volume = 0.3f;
            //output.Play();
        }

        private void AtenderServidor()
        {
            while (true)
            {
                byte[] msg2 = new byte[2048];
                int bytesRecibidos = Servidor.Receive(msg2);
                string mensajeCompleto = Encoding.UTF8.GetString(msg2, 0, bytesRecibidos);
                string[] mensajes = mensajeCompleto.Split('|');

                for (int k = 0; k < mensajes.Length; k++)
                {
                    if (mensajes[k] != "")
                    {
                        string[] Trozos = mensajes[k].Split('/');
                        int Codigo = Convert.ToInt32(Trozos[0]);
                        string mensaje = string.Join("/", Trozos.Skip(1)).Split('\0')[0];
                        string[] Partes;
                        int TipoMensaje;
                        int NumeroFormDeSala;

                        switch (Codigo)
                        {
                            case 1: // Código para crear un Usuario nuevo
                                Partes = mensaje.Split('/');
                                TipoMensaje = Convert.ToInt32(Partes[0]);
                                mensaje = string.Join("/", Partes.Skip(1)).Split('\0')[0];

                                switch (TipoMensaje)
                                {
                                    case 0:
                                        this.Invoke(new Delegado(() =>
                                        {
                                            CrearUsuarioForm.EnviarVerificacion();
                                            this.CrearUsuarioForm.Close();
                                            this.Show();
                                        }));
                                        break;

                                    case 1:
                                        Partes = mensaje.Split('/');
                                        Usuario = Partes[0];
                                        Contrasena = Partes[1];

                                        this.Invoke(new Delegado(() => { RecuperarCuentaForm.EnviarCorreo(Usuario, Contrasena); }));
                                        break;

                                    case 2:
                                        Usuario = mensaje.Split('/')[0];
                                        this.CodigoFotoPerfil = Convert.ToInt32(mensaje.Split('/')[1]);
                                        FraseFavorita = mensaje.Split('/')[2];

                                        FotoPerfil = @"FotosPerfil\" + this.CodigoFotoPerfil + ".png";
                                        this.Invoke(new Delegado(() => { MenuJugadorForm.CambiarDatosPerfil(this.CodigoFotoPerfil, FotoPerfil, Usuario, FraseFavorita); }));
                                        break;

                                    case 3:
                                        Partes = mensaje.Split('/');
                                        this.Invoke(new Delegado(() => { MenuJugadorForm.SolicitudAmistad(Partes[0]); }));
                                        break;
                                }
                                break;


                            case 2:  // Código para acceder (verifica si el usuario está en la base de datos)
                                Partes = mensaje.Split('/');
                                TipoMensaje = Convert.ToInt32(Partes[0]);
                                mensaje = string.Join("/", Partes.Skip(2));

                                if (TipoMensaje == 0)
                                {
                                    
                                    this.CodigoFotoPerfil = Convert.ToInt32(Partes[1]);
                                    this.FraseFavorita = Partes[2];
                                    ThreadStart ts = delegate { IniciarSesion(); };
                                    TMenuJugador = new Thread(ts);
                                    TMenuJugador.Start();
                                }
                                break;

                            case 3: // Código para mostrar las relaciones de la BBDD
                                Partes = mensaje.Split('/');
                                TipoMensaje = Convert.ToInt32(Partes[0]);

                                switch (TipoMensaje)
                                {
                                    case 1:
                                        mensaje = string.Join("/", Partes.Skip(1));
                                        if (MenuJugadorForm.InformacionJugadorForm != null && !MenuJugadorForm.InformacionJugadorForm.IsDisposed && MenuJugadorForm.InformacionJugadorForm.Visible)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.InformacionJugadorForm.PonerInformacion(mensaje); }));
                                        }
                                        break;

                                    case 2:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].DatosPerfilAbierto(Partes[2], Partes[3], Partes[4], Partes[5]); }));
                                        }
                                        break;

                                    case 3:
                                        this.Invoke(new Delegado(() => {
                                            MenuJugadorForm.InformacionJugadorForm.PerfilAmigoForm.PonerFotoRankingVictorias(Partes[2], Partes[3], Partes[4], Partes[5], Partes[6]);
                                        }));

                                        break;
                                    case 4:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].DatosPerfilAbierto(Partes[2], Partes[3], Partes[4], Partes[5]); }));
                                        }
                                        break;



                                    case 6:
                                        if (Partes.Length > 1)
                                        {
                                            mensaje = string.Join("/", Partes.Skip(1));

                                            if (MenuJugadorForm.InformacionJugadorForm != null &&
                                                !MenuJugadorForm.InformacionJugadorForm.IsDisposed &&
                                                MenuJugadorForm.InformacionJugadorForm.Visible)
                                            {
                                                this.Invoke(new Delegado(() =>
                                                {
                                                    MenuJugadorForm.InformacionJugadorForm.PonerInformacion(mensaje);
                                                }));
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: el mensaje recibido para los filtros está vacío o mal formado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        break;

                                }
                                break;

                            case 4:
                                if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.CrearSalaForm != null && !MenuJugadorForm.CrearSalaForm.IsDisposed)
                                {
                                    this.Invoke(new Delegado(() =>
                                    {
                                        MenuJugadorForm.CrearSalaForm.PonerCodigoSala(mensaje);
                                    }));
                                }
                                break;

                            case 5:
                                Partes = mensaje.Split('/');
                                TipoMensaje = Convert.ToInt32(Partes[0]);

                                switch (TipoMensaje)
                                {
                                    case 2:
                                        InformacionPartida(Partes);
                                        this.Invoke(new Delegado(() => { MenuJugadorForm.AccederPartida(JugadorYo, Partida); }));
                                        break;

                                    case 3:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[4] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            InformacionPartida(Partes);
                                            if (MenuJugadorForm.SalasDeJuego[NumeroFormDeSala] != null && !MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].IsDisposed)
                                            {
                                                this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].ActualizarJugadores(Partida); }));
                                            }
                                        }
                                        break;

                                    case 4:
                                        if (Partes.Length == 5)
                                        {
                                            bool Acertada;
                                            if (Partes[4] == "1")
                                                Acertada = true;
                                            else
                                                Acertada = false;

                                            NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                            if (NumeroFormDeSala != -1 && MenuJugadorForm.SalasDeJuego[NumeroFormDeSala] != null && !MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].IsDisposed)
                                            {
                                                this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].EscribirChat(Partes[2], Partes[3], Acertada); }));
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 1; i < Partes.Length; i += 4)
                                            {
                                                bool Acertada;
                                                if (Partes[i + 3] == "1")
                                                    Acertada = true;
                                                else
                                                    Acertada = false;

                                                NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[i] });
                                                if (NumeroFormDeSala != -1 && MenuJugadorForm.SalasDeJuego[NumeroFormDeSala] != null && !MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].IsDisposed)
                                                {
                                                    this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].Ultimos20Mensajes(Partes[i + 1], Partes[i + 2], Acertada); }));
                                                }
                                            }
                                        }
                                        break;

                                    case 5:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].ActualizarPanel(Convert.ToInt32(Partes[2]), Convert.ToInt32(Partes[3]), Convert.ToInt32(Partes[4]), Convert.ToInt32(Partes[5]), Convert.ToSingle(Partes[6])); }));
                                        }
                                        break;

                                    case 6:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].RecibirPalabras(Partes[2], Partes[3], Partes[4], Partes[5]); }));
                                        }
                                        break;

                                    case 7:
                                        List<int> indicesAleatorios = new List<int>();
                                        for (int i = 0; i < Convert.ToInt32(Partes[3]); i++)
                                        {
                                            indicesAleatorios.Add(Convert.ToInt32(Partes[i + 4]));
                                        }

                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].IniciarRonda(Partes[2], indicesAleatorios); }));
                                        }
                                        break;

                                    case 8:
                                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.RecibirInvitacion(Partes[1], Partes[2]); }));
                                        }
                                        break;

                                    case 9:
                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].SumarPuntos(Partes[2], Partes[3], Partes[4], Partes[5]); }));
                                        }
                                        break;

                                    case 10:

                                        NumeroFormDeSala = (int)this.Invoke(new DelegadoNumeroForm(MenuJugadorForm.NumeroFormPartida), new object[] { Partes[1] });
                                        if (NumeroFormDeSala != -1)
                                        {
                                                this.Invoke(new Delegado(() => { MenuJugadorForm.SalasDeJuego[NumeroFormDeSala].Expulsion(Partes[2]); }));
                                        }
                                        break;

                                }
                                break;

                            case 99: // Notificación
                                Partes = mensaje.Split('/');
                                TipoMensaje = Convert.ToInt32(Partes[0]);

                                switch (TipoMensaje)
                                {
                                    case 1:
                                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.CambiarNumeroConectados(mensaje); }));
                                        }
                                        break;

                                    case 2:
                                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                                        {
                                            this.Invoke(new Delegado(() => { MenuJugadorForm.ActualizarPartidas(mensaje); }));
                                        }
                                        break;
                                }

                                break;

                            case -1:
                                this.Invoke(new Delegado(() => { MessageBox.Show(mensaje); }));
                                break;

                        }
                    }
                }
            }
        }

        private void BotonConectar_Click(object sender, EventArgs e)
        {
            if (!ConexionEstablecida)
            {
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(IP);
                IPEndPoint ipep = new IPEndPoint(direc, Puerto);


                //Creamos el socket 
                Servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Servidor.Connect(ipep);//Intentamos conectar el socket
                    LabelConexion.Text = "Conectado";
                    ConexionEstablecida = true;

                    // Poner en marcha el thread que atenderá los mensajes del servidor
                    ThreadStart ts = delegate { AtenderServidor(); };
                    atender = new Thread(ts);
                    atender.Start();

                    PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Conectado.png");
                    PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;

                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show($"No he podido conectar con el servidor.");
                    return;
                }
            }
        }

        private void BotonDesconectar_Click(object sender, EventArgs e)
        {
            if (ConexionEstablecida)
            {
                //Mensaje de desconexión
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);

                // Nos desconectamos
                atender.Abort();
                Servidor.Shutdown(SocketShutdown.Both);
                Servidor.Close();
                LabelConexion.Text = "Desconectado";
                ConexionEstablecida = false;
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Desconectado.png");
                PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void BotonCrearUsuario_Click(object sender, EventArgs e)
        {
            if (!ConexionEstablecida)
            {
                MessageBox.Show("No se ha establecido conexion con el servidor.");
            }
            else
            {
                this.CrearUsuarioForm = new CrearUsuario(Servidor);
                this.Hide();
                CrearUsuarioForm.ShowDialog();
                this.Show();
            }
        }

        private void BotonAceptar_Click(object sender, EventArgs e)
        {
            if (TextUsuario.Text == "" || TextContrasena.Text == "")
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
            else 
            {
                if (!ConexionEstablecida)
                {
                    MessageBox.Show("No se ha establecido conexion con el servidor.");
                }
                else 
                {
                    Usuario = TextUsuario.Text;

                    if (ContrasenaOculta)
                    {
                        Contrasena = ContrasenaEscrita;
                    }
                    else
                    {
                        Contrasena = TextContrasena.Text;
                    }

                    string mensaje = "2/1/" + Usuario + "/" + Contrasena;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);
                }
            }
        }

        private void TextContrasena_TextChanged(object sender, EventArgs e)
        {
            if (ContrasenaOculta)
            {
                TextContrasena.TextChanged -= TextContrasena_TextChanged;
                if (TextContrasena.Text.Length > ContrasenaEscrita.Length)
                {
                    ContrasenaEscrita += TextContrasena.Text.Substring(ContrasenaEscrita.Length);
                }
                else if (TextContrasena.Text.Length < ContrasenaEscrita.Length)
                {
                    ContrasenaEscrita = ContrasenaEscrita.Substring(0, TextContrasena.Text.Length);
                }
                TextContrasena.Text = new String('*', ContrasenaEscrita.Length);
                TextContrasena.SelectionStart = TextContrasena.Text.Length;
                TextContrasena.TextChanged += TextContrasena_TextChanged;
            }
            else
            {
                ContrasenaEscrita = TextContrasena.Text;
            }
        }

        private void BotonOcultarContrasena_Click(object sender, EventArgs e)
        {
            ContrasenaOculta = !ContrasenaOculta;

            if (ContrasenaOculta)
            {
                TextContrasena.Text = new String('*', ContrasenaEscrita.Length);
                BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\VerContrasena.png");
                BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                TextContrasena.Text = ContrasenaEscrita;
                BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\OcultarContrasena.png");
                BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            }
            TextContrasena.SelectionStart = TextContrasena.Text.Length;
        }

        private void BotonAceptar_MouseMove(object sender, MouseEventArgs e)
        {
            if (ConexionEstablecida)
            {
                BotonAceptar.Cursor = Cursors.Hand;
            }
            else
            {
                BotonAceptar.Cursor = Cursors.No;
            }
        }

        private void BotonCrearUsuario_MouseMove(object sender, MouseEventArgs e)
        {
            if (ConexionEstablecida)
            {
                BotonCrearUsuario.Cursor = Cursors.Hand;
            }
            else
            {
                BotonCrearUsuario.Cursor = Cursors.No;
            }
        }

        private void IniciarSesion()
        {
            this.Invoke(new Action(() =>
            {
                TextUsuario.Text = "";
                TextContrasena.Text = "";
                this.Hide();
            }));

            this.MenuJugadorForm = new MenuJugador(Usuario, Servidor, CodigoFotoPerfil, FraseFavorita);
            this.MenuJugadorForm.ShowDialog();

            this.Invoke(new Delegado(() =>
            {
                TMenuJugador.Abort();
                this.Show();
            }));
        }

        private void InformacionPartida(string[] Partes)
        {
            MenuJugador.Jugador JugadorHost = new MenuJugador.Jugador();
            MenuJugador.Jugador Jugador2 = new MenuJugador.Jugador();
            MenuJugador.Jugador Jugador3 = new MenuJugador.Jugador();
            MenuJugador.Jugador Jugador4 = new MenuJugador.Jugador();

            Partida.MaximoJugadores = Convert.ToInt32(Partes[1]);
            Partida.NumeroJugadores = Convert.ToInt32(Partes[2]);

            if (Partida.NumeroJugadores <= Partida.MaximoJugadores)
            {
                Partida.NumeroRondas = Convert.ToInt32(Partes[3]);
                Partida.Codigo = Partes[4];
                Partida.Categoria = Partes[5];
                Partida.Dificultad = Partes[6];
                Partida.Privacidad = Partes[7];

                Partida.Jugadores = new List<MenuJugador.Jugador>();
                JugadorHost.Usuario = Partes[8];
                JugadorHost.CodigoFotoPerfil = Convert.ToInt32(Partes[9]);
                JugadorHost.FotoPerfilUsuario = @"FotosPerfil/" + Partes[9] + ".png";
                Partida.Jugadores.Add(JugadorHost);

                switch (Partida.NumeroJugadores)
                {
                    case 1:
                        break;

                    case 2:
                        Jugador2.Usuario = Partes[10];
                        Jugador2.CodigoFotoPerfil = Convert.ToInt32(Partes[11]);
                        Jugador2.FotoPerfilUsuario = @"FotosPerfil/" + Partes[11] + ".png";
                        Partida.Jugadores.Add(Jugador2);
                        break;

                    case 3:
                        Jugador2.Usuario = Partes[10];
                        Jugador2.CodigoFotoPerfil = Convert.ToInt32(Partes[11]);
                        Jugador2.FotoPerfilUsuario = @"FotosPerfil/" + Partes[11] + ".png";
                        Partida.Jugadores.Add(Jugador2);

                        Jugador3.Usuario = Partes[12];
                        Jugador3.CodigoFotoPerfil = Convert.ToInt32(Partes[13]);
                        Jugador3.FotoPerfilUsuario = @"FotosPerfil/" + Partes[13] + ".png";
                        Partida.Jugadores.Add(Jugador3);
                        break;

                    case 4:
                        Jugador2.Usuario = Partes[10];
                        Jugador2.CodigoFotoPerfil = Convert.ToInt32(Partes[11]);
                        Jugador2.FotoPerfilUsuario = @"FotosPerfil/" + Partes[11] + ".png";
                        Partida.Jugadores.Add(Jugador2);

                        Jugador3.Usuario = Partes[12];
                        Jugador3.CodigoFotoPerfil = Convert.ToInt32(Partes[13]);
                        Jugador3.FotoPerfilUsuario = @"FotosPerfil/" + Partes[13] + ".png";
                        Partida.Jugadores.Add(Jugador3);

                        Jugador4.Usuario = Partes[14];
                        Jugador4.CodigoFotoPerfil = Convert.ToInt32(Partes[15]);
                        Jugador4.FotoPerfilUsuario = @"FotosPerfil/" + Partes[15] + ".png";
                        Partida.Jugadores.Add(Jugador4);
                        break;
                }

                if (Partes[0] == "2")
                {
                    JugadorYo.Usuario = Usuario;
                    JugadorYo.CodigoFotoPerfil = this.CodigoFotoPerfil;
                    JugadorYo.FotoPerfilUsuario = @"FotosPerfil/" + this.CodigoFotoPerfil + ".png";
                    Partida.Jugadores.Add(JugadorYo);
                }
            }
        }

        private void BotonRecuperarCuenta_Click(object sender, EventArgs e)
        {
            this.RecuperarCuentaForm = new RecuperarCuenta(Servidor);
            this.RecuperarCuentaForm.ShowDialog();
        }

        private void Boton_ClickConSonido(object sender, EventArgs e)
        {
            string ruta = @"Audios\Click.wav";
            SoundPlayer player = new SoundPlayer(ruta);
            player.Play();
        }

        private void MenuInicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ConexionEstablecida)
            {
                //Mensaje de desconexión
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);

                // Nos desconectamos
                atender.Abort();
                Servidor.Shutdown(SocketShutdown.Both);
                Servidor.Close();
                LabelConexion.Text = "Desconectado";
                ConexionEstablecida = false;
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Desconectado.png");
                PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
    }
}