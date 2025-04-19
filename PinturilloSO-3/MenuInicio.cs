using System;
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

namespace PinturilloSO
{
    public partial class MenuInicio : Form
    {
        Socket Servidor;
        Thread atender;
        Thread TMenuJugador;

        delegate void DelegadoSinMensaje();
        delegate void DelegadoConMensaje(string mensaje);


        string Correo;
        string Usuario;
        string Contrasena;

        bool ConexionEstablecida = false;
        bool ContrasenaOculta = true;
        string ContrasenaEscrita = "";

        private MenuJugador MenuJugadorForm;

        public MenuInicio(Socket Servidor = null)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.AcceptButton = BotonAceptar;

            FormResizer.SaveInitialControlBounds(this);
            FormResizer.SetInitialFormSize(Size);
            Resize += (s, e) => FormResizer.ScaleControls(this, Size);

            this.BackgroundImage = System.Drawing.Image.FromFile(@"Fondo2.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Desconectado.png");
            PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"VerContrasena.png");
            BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            BotonAyuda.BackgroundImage = System.Drawing.Image.FromFile(@"Ayuda.png");
            BotonAyuda.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void AtenderServidor()
        {
            while (true)
            {
                byte[] msg2 = new byte[80];
                Servidor.Receive(msg2);
                string mensajeCompleto = Encoding.ASCII.GetString(msg2);
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

                        if (TipoMensaje == 2)
                        {
                            mensaje = "El correo ya está registrado.";
                        }
                        else if (TipoMensaje == 3)
                        {
                            mensaje = "El usuario ya existe. Por favor, elige otro nombre.";
                        }
                        else
                        {
                            //mensaje = string.Join("/", Partes.Skip(2));
                            mensaje = "Usuario creado con éxito. Por favor, inicie sesión";
                        }

                        this.Invoke(new DelegadoConMensaje(MostrarMensaje), new object[] { mensaje });
                        break;


                    case 2:  // Código para acceder (verifica si el usuario está en la base de datos)
                        Partes = mensaje.Split('/');
                        TipoMensaje = Convert.ToInt32(Partes[0]);
                        mensaje = string.Join("/", Partes.Skip(2));

                        if (TipoMensaje == 0)
                        {
                            ThreadStart ts = delegate { IniciarSesion(); };
                            TMenuJugador = new Thread(ts);
                            TMenuJugador.Start();
                        }
                        else
                        {
                            if (TipoMensaje == 3)
                            {
                                mensaje = "La sesión ya está iniciada en otro cliente.";
                            }
                            this.Invoke(new DelegadoConMensaje(MostrarMensaje), new object[] { mensaje });
                        }
                        break;

                    case 3: // Código para mostrar las relaciones de la BBDD
                        Partes = mensaje.Split('/');
                        TipoMensaje = Convert.ToInt32(Partes[0]);

                        switch (TipoMensaje)
                        {
                            case 1:
                                mensaje = Partes[1];
                                this.Invoke(new DelegadoConMensaje(MostrarRelacionRanking), new object[] { mensaje });
                                break;
                            case 2:
                                mensaje = Partes[1];
                                this.Invoke(new DelegadoConMensaje(MostrarRelacionPartidas), new object[] { mensaje });
                                break;
                            case 3:
                                mensaje = string.Join("\n\t", Partes.Skip(1));
                                this.Invoke(new DelegadoConMensaje(MostrarRelacionAmigos), new object[] { mensaje });
                                break;
                        }
                        break;

                    case 99: // Notificación
                        if (MenuJugadorForm != null && !MenuJugadorForm.IsDisposed && MenuJugadorForm.Visible)
                        {
                            this.Invoke(new DelegadoConMensaje(MenuJugadorForm.CambiarNumeroConectados), new object[] { mensaje });
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
                IPAddress direc = IPAddress.Parse("10.4.119.5");
                IPEndPoint ipep = new IPEndPoint(direc, 50086);


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

                    PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Conectado.png");
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
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Desconectado.png");
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
                this.Hide();
                CrearUsuario CrearUsuarioForm = new CrearUsuario(ConexionEstablecida, Servidor);
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
                BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"VerContrasena.png");
                BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                TextContrasena.Text = ContrasenaEscrita;
                BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"OcultarContrasena.png");
                BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
            }
            TextContrasena.SelectionStart = TextContrasena.Text.Length;
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
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Desconectado.png");
                PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            }
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
                this.Hide();
            }));

            this.MenuJugadorForm = new MenuJugador(Usuario, Servidor);
            this.MenuJugadorForm.ShowDialog();

            this.Invoke(new DelegadoSinMensaje(() =>
            {
                TMenuJugador.Abort();
                this.Show();
            }));
        }

        public void Notificacion(string mensaje)
        {
            if (this.MenuJugadorForm != null && !this.MenuJugadorForm.IsDisposed)
            {
                this.MenuJugadorForm.CambiarNumeroConectados(mensaje);
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        private void MostrarRelacionRanking(string mensaje)
        {
            MenuJugadorForm.MostrarRelacion("Posicion Ranking: " + mensaje);
        }

        private void MostrarRelacionPartidas(string mensaje)
        {
            MenuJugadorForm.MostrarRelacion("Partidas Jugadas: " + mensaje);
        }

        private void MostrarRelacionAmigos(string mensaje)
        {
            MenuJugadorForm.MostrarRelacion("Amigos: \n\t" + mensaje);
        }
    }
}