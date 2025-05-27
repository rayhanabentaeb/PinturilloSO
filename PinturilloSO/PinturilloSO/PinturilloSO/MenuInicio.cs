using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
        int Puerto = 9070;
        string IP = "192.168.56.102";

        delegate void DelegadoSinMensaje();
        delegate void DelegadoConMensaje(string mensaje);
        delegate void DelegadoEntrarPartida(MenuJugador.Jugador Jugador, CrearSala.Partida Partida);
        delegate void DelegadoActualizarPartida(CrearSala.Partida Partida);
        delegate void DelegadoEscribirChat(string Usuario, string mensaje);
        delegate void DelegadoActualizarPanel(int CodigoDibujo, int DibujoX, int DibujoY);
        delegate void DelegadoRondas(string Palabra, string Pintor, List<int> indicesAleatorios);

        string Correo;
        string Usuario;
        string Contrasena;
        string FotoPerfil;
        int CodigoFotoPerfil;
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

        public MenuInicio(Socket Servidor = null)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.AcceptButton = BotonAceptar;

            FormResizer.SaveInitialControlBounds(this);
            FormResizer.SetInitialFormSize(Size);
            Resize += (s, e) => FormResizer.ScaleControls(this, Size);

            this.BackgroundImage = System.Drawing.Image.FromFile(@"FondosPantalla\Fondo2.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Desconectado.png");
            PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\VerContrasena.png");
            BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            BotonAyuda.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Ayuda.png");
            BotonAyuda.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                byte[] msg2 = new byte[320];
                int bytesRecibidos = Servidor.Receive(msg2);
                string mensajeCompleto = Encoding.UTF8.GetString(msg2, 0, bytesRecibidos);
                string[] Trozos = mensajeCompleto.Split('/');
                int Codigo = Convert.ToInt32(Trozos[0]);
                string mensaje = string.Join("/", Trozos.Skip(1)).Split('\0')[0];
                string[] Partes;
                int TipoMensaje;

                switch (Codigo)
                {
                    case 1: // Código para crear un Usuario nuevo
                        Partes = mensaje.Split('/');
                        TipoMensaje = Convert.ToInt32(Partes[0]);
                        mensaje = string.Join("/", Partes.Skip(1)).Split('\0')[0];

                        switch (TipoMensaje)
                        {
                            case 0:
                                this.Invoke(new DelegadoSinMensaje(() =>
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

                                this.Invoke(new DelegadoSinMensaje(() =>
                                {
                                    RecuperarCuentaForm.EnviarCorreo(Usuario, Contrasena);
                                }));
                                break;

                            case 2:
                                Usuario = mensaje.Split('/')[0];
                                this.CodigoFotoPerfil = Convert.ToInt16(mensaje.Split('/')[1]);

                                FotoPerfil = @"FotosPerfil\" + this.CodigoFotoPerfil + ".png";
                                this.Invoke(new DelegadoSinMensaje(() =>
                                {
                                    MenuJugadorForm.CambiarDatosPerfil(FotoPerfil, Usuario);
                                }));
                                break;
                        }
                        break;


                    case 2:  // Código para acceder (verifica si el usuario está en la base de datos)
                        Partes = mensaje.Split('/');
                        TipoMensaje = Convert.ToInt32(Partes[0]);
                        mensaje = string.Join("/", Partes.Skip(2));

                        if (TipoMensaje == 0)
                        {
                            mensaje = string.Join("/", Partes.Skip(1)).Split('\0')[0];
                            this.CodigoFotoPerfil = Convert.ToInt32(mensaje);
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
                                mensaje = Partes[1];
                                this.Invoke(new DelegadoConMensaje(MenuJugadorForm.MostrarRelacion), new object[] { "Posicion Ranking: " + mensaje });
                                break;

                            case 2:
                                mensaje = Partes[1];
                                this.Invoke(new DelegadoConMensaje(MenuJugadorForm.MostrarRelacion), new object[] { "Partidas Jugadas: " + mensaje });
                                break;

                            case 3:
                                mensaje = string.Join("\n\t", Partes.Skip(1));
                                this.Invoke(new DelegadoConMensaje(MenuJugadorForm.MostrarRelacion), new object[] { "Amigos: \n\t" + mensaje });
                                break;
                        }
                        break;

                    case 4:
                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.CrearSalaForm != null && !MenuJugadorForm.CrearSalaForm.IsDisposed)
                        {
                            this.Invoke(new DelegadoConMensaje(MenuJugadorForm.CrearSalaForm.PonerCodigoSala), new object[] { mensaje });
                        }
                        break;

                    case 5:
                        Partes = mensaje.Split('/');
                        TipoMensaje = Convert.ToInt32(Partes[0]);

                        switch (TipoMensaje)
                        {
                            case 2:
                                InformacionPartida(Partes);
                                if (Partida.NumeroJugadores < Partida.MaximoJugadores)
                                {
                                    this.Invoke(new DelegadoEntrarPartida(MenuJugadorForm.AccederPartida), new object[] { JugadorYo, Partida });
                                }
                                else
                                {
                                    MessageBox.Show("Esta sala de partida ya está completa.");
                                }
                                break;

                            case 3:
                                InformacionPartida(Partes);
                                if (MenuJugadorForm.SalasDeJuego[0] != null && !MenuJugadorForm.SalasDeJuego[0].IsDisposed)
                                {
                                    this.Invoke(new DelegadoActualizarPartida(MenuJugadorForm.SalasDeJuego[0].ActualizarJugadores), new object[] { Partida });
                                }
                                break;

                            case 4:
                                if (MenuJugadorForm.SalasDeJuego[0] != null && !MenuJugadorForm.SalasDeJuego[0].IsDisposed)
                                {
                                    if (Partes[1] != Usuario)
                                    {
                                        this.Invoke(new DelegadoEscribirChat(MenuJugadorForm.SalasDeJuego[0].EscribirChat), new object[] { Partes[1], Partes[2] });
                                    }
                                }
                                break;

                            case 5:
                                this.Invoke(new DelegadoActualizarPanel(MenuJugadorForm.SalasDeJuego[0].ActualizarPanel), new object[] { Convert.ToInt32(Partes[1]), Convert.ToInt32(Partes[2]), Convert.ToInt32(Partes[3]) });
                                break;

                            case 6:
                                this.Invoke(new DelegadoConMensaje(MenuJugadorForm.SalasDeJuego[0].RecibirPalabra), new object[] { Partes[1] });
                                break;

                            case 7:
                                List<int> indicesAleatorios = new List<int>();
                                for (int i = 4; i < Convert.ToInt32(Partes[3]); i++)
                                {
                                    indicesAleatorios.Add(Convert.ToInt32(Partes[i]));
                                }

                                this.Invoke(new DelegadoRondas(MenuJugadorForm.SalasDeJuego[0].IniciarRonda), new object[] { Partes[1], Partes[2], indicesAleatorios });
                                break;
                        }
                        break;

                    case 99: // Notificación
                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                        {
                            this.Invoke(new DelegadoConMensaje(MenuJugadorForm.CambiarNumeroConectados), new object[] { mensaje });
                            this.Invoke(new DelegadoConMensaje(MenuJugadorForm.ActualizarConectados), new object[] { mensaje });
                        }

                        break;

                    case -1:
                        this.Invoke(new DelegadoConMensaje(MostrarMensaje), new object[] { mensaje });
                        break;

                    case 7: // Invitación a partida
                        Partes = mensaje.Split('/');
                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                        {
                            this.Invoke(new Action(() =>
                            {
                                DialogResult Resultado = MessageBox.Show(
                                    $"{Partes[0]} te ha invitado a una partida.\n¿Quieres aceptar la invitación?",
                                    "Invitación recibida",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                );

                                string Respuesta = Resultado == DialogResult.Yes ? "aceptar" : "rechazar";

                                if (Resultado == DialogResult.Yes)
                                {
                                    string mensajeAcceso = "5/2/" + Partes[1];
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensajeAcceso);
                                    Servidor.Send(msg);
                                }
                            }));
                        }
                        break;
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
                this.CrearUsuarioForm = new CrearUsuario(ConexionEstablecida, Servidor);
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

            this.MenuJugadorForm = new MenuJugador(Usuario, Servidor, CodigoFotoPerfil);
            this.MenuJugadorForm.ShowDialog();

            this.Invoke(new DelegadoSinMensaje(() =>
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

            if (Partida.NumeroJugadores < Partida.MaximoJugadores)
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

        private void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        private void BotonRecuperarCuenta_Click(object sender, EventArgs e)
        {
            this.RecuperarCuentaForm = new RecuperarCuenta(Servidor);
            this.Hide();
            this.RecuperarCuentaForm.ShowDialog();
            this.Show();
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