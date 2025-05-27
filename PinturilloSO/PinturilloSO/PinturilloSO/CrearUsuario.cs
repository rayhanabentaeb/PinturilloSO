using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PinturilloSO;
using System.Net.Mime;

namespace PinturilloSO
{
    public partial class CrearUsuario: Form
    {
        Socket Servidor;

        string Correo;
        string Usuario;
        string Contrasena;

        bool ContrasenaOculta = true;
        string ContrasenaEscrita = "";

        public CrearUsuario(bool ConexionEstablecida, Socket Servidor = null)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.AcceptButton = BotonCrear;

            FormResizer.SaveInitialControlBounds(this);
            FormResizer.SetInitialFormSize(Size);
            Resize += (s, e) => FormResizer.ScaleControls(this, Size);

            BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\VerContrasena.png");
            BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;

            if (ConexionEstablecida)
            {
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Conectado.png");
                PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                PanelImagenConexion.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Desconectado.png");
                PanelImagenConexion.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                var mail = new MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void BotonCrear_Click(object sender, EventArgs e)
        {
            Correo = TextCorreoElectronico.Text;
            Usuario = TextUsuario.Text;
            Contrasena = TextContrasena.Text;

            if (ContrasenaOculta)
            {
                Contrasena = ContrasenaEscrita;
            }
            else
            {
                Contrasena = TextContrasena.Text;
            }

            if (Correo == "" || Usuario == "" || Contrasena == "")
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
            else if (!EsCorreoValido(Correo))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.");
            }
            else
            {
                string mensaje = "1/0/" + Correo + "/" + Usuario + "/" + Contrasena + "/1";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        private void BotonMenuPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
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

        public void EnviarVerificacion()
        {
            try 
            {
                string remitente = "infodospruebas@gmail.com";
                string asunto = "¡REGISTRO COMPLETADO EN PINTURILLOSO!";
                string imagePath = @"FondosPantalla\FotoCorreo.png";

                string cuerpoMensaje = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                text-align: center;
                            }}
                            .small {{
                                font-size: 10px;
                            }}
                            .medium {{
                                font-size: 18px;
                            }}
                            .large {{
                                font-size: 32px;
                            }}
                            .image {{
                                padding: 3px 3px;
                                width: 300px;
                                border-color: black;
                                border-style: solid;
                                border-width: 6px;
                                border-radius: 30px;
                            }}
                        </style>
                    </head>
                    <body>
                        <center>
                        <p class='large'><strong><em><span style='color: #FFB486;'>¡Hola, </span><span style='color: #FA5F00;'>{Usuario}!</span>
                        <p class='medium'><span style='color: #FFB486;'>Desde el equipo de </span><i><span style='color: #FA5F00;'>PinturilloSO</i></span><span style='color: #FFB486;'> sabemos bien que escondes un gran talento para la pintura... </span><br><br>
                        <span style='color: #FA5F00;'>¡INICIA SESIÓN AHORA PARA DEMOSTRARLO JUGANDO CON TUS AMIGOS!</span></p>
                        <img class='image' src='cid:logoImage' alt='PinturilloSO'/>
                        <p class='small'><span style='color: #FFB486;'>Se ha registrado correctamente el usuario '{Usuario}' en nuestra Base de Datos de PinturilloSO.</span></p>
                        <p class='small'><span style='color: #FFB486;'>© 2025 PinturilloSO - Rayhana · Nora · Pol</span></p>
                        </center>
                    </body>
                </html>";

                using (MailMessage email = new MailMessage(remitente, Correo))
                {
                    email.Subject = asunto;
                    email.IsBodyHtml = true;

                    AlternateView vistaHtml = AlternateView.CreateAlternateViewFromString(cuerpoMensaje, null, MediaTypeNames.Text.Html);

                    LinkedResource imagen = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
                    imagen.ContentId = "logoImage";
                    imagen.TransferEncoding = TransferEncoding.Base64;

                    vistaHtml.LinkedResources.Add(imagen);
                    email.AlternateViews.Add(vistaHtml);

                    string[] servidor = Correo.Split('@');

                    using (SmtpClient smtpClient = new SmtpClient($"smtp.{servidor[1]}"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential("infodospruebas@gmail.com", "tvtz usna bntm sdkp");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(email);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
