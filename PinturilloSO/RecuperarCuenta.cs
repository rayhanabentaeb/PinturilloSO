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
using System.Net.Mime;
using System.Media;

namespace PinturilloSO
{
    public partial class RecuperarCuenta: Form
    {
        Socket Servidor;
        string Correo;
        string Usuario;
        string Contrasena;
        private ControlScaler _scaler;

        public RecuperarCuenta(Socket Servidor)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.AcceptButton = BotonEnviar;

            BotonEnviar.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\Enviar.png");
            BotonEnviar.BackgroundImageLayout = ImageLayout.Stretch;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void BotonEnviar_Click(object sender, EventArgs e)
        {
            if (TextCambiarUsuario.Text == "")
            {
                MessageBox.Show("Por favor, introduzca un correo electrónico primero.");
            }
            else
            {
                Correo = TextCambiarUsuario.Text;

                string mensaje = "1/1/" + Correo;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        public void EnviarCorreo(string Usuario, string Contrasena)
        {
            try
            {
                string remitente = "infodospruebas@gmail.com";
                string asunto = "Recuperación Cuenta PinturilloSO";
                string imagePath = @"Iconos\LadronDigital.png";

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
                                font-size: 22px;
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
                        <p class='large'><strong><em><span style='color: #FFB486;'>Hemos visto que has perdido tus credenciales de acceso...<br>¡Aqui las tienes!</span></em></strong></p>
                        <p class='large'><strong><span style='color: #FFB486;'>Usuario: </span><span style='color: #FA5F00;'>{Usuario}</span><br \><span style='color: #FFB486;'>Contraseña: </span><span style='color: #FA5F00;'>{Contrasena}</span></strong></p>
                        <p class='medium'><span style='color: #FFB486;'>¡Ya puedes volver a pintar junto a tus amigos!</span></p>
                        <p class='medium'><span style='color: #FFB486;'></span><span style='color: #FA5F00;'><strong>¡RECUERDA!</strong></span><span style='color: #FFB486;'> Cuidado con quien compartes tus datos de acceso, podrian robarte la cuenta...</span></p>
                        <img class='image' src='cid:logoImage' alt='PinturilloSO'/>
                        <p class='small'><span style='color: #FFB486;'>Se han recuperado correctamente las credenciales de acceso del usuario '{Usuario}' para PinturilloSO.</span></p>
                        <p class='small'><span style='color: #FFB486;'>© 2025 PinturilloSO - Rayhana · Nora · Pol</span></p>
                        <center>
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
            this.Close();
        }

        private void BotonMenuPrincipal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RecuperarCuenta_Load(object sender, EventArgs e)
        {
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
    }
}
