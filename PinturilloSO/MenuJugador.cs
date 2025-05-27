using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PinturilloSO.CrearSala;
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class MenuJugador: Form
    {
        string Usuario;
        Thread TPartida;
        public Socket Servidor;
        int CodigoFotoPerfil;
        string FotoPerfilUsuario;
        public Jugador JugadorYo;
        bool Host;
        public DataTable dataTable = new DataTable();
        string FraseFavorita;

        public CrearSala CrearSalaForm;
        public InformacionJugador InformacionJugadorForm;
        public List<SalaPartida> SalasDeJuego = new List<SalaPartida>();
        public List<string> JugadoresConectados = new List<string>();
        private ControlScaler _scaler;

        public struct Jugador
        {
            public string Usuario;
            public int CodigoFotoPerfil;
            public string FotoPerfilUsuario;
            public string FraseFavorita;
        }

        public MenuJugador(string Usuario, Socket Servidor, int CodigoFotoPerfil, string FraseFavorita)
        {
            InitializeComponent();
            this.Usuario = Usuario;
            this.Servidor = Servidor;
            this.CodigoFotoPerfil = CodigoFotoPerfil;
            this.FotoPerfilUsuario = @"FotosPerfil\" + CodigoFotoPerfil + ".png";
            this.LabelNombreUsuario.Text = Usuario;
            this.FraseFavorita = FraseFavorita;

            this.BackgroundImage = System.Drawing.Image.FromFile(@"FondosPantalla\Fondo.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            JugadorYo.Usuario = Usuario;
            JugadorYo.CodigoFotoPerfil = CodigoFotoPerfil;
            JugadorYo.FotoPerfilUsuario = this.FotoPerfilUsuario;
            JugadorYo.FraseFavorita = FraseFavorita;

            ButtonFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfilUsuario);
            ButtonFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

            BotonComoUnirse.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\ComoJugar.png");
            BotonComoUnirse.BackgroundImageLayout = ImageLayout.Stretch;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void MenuJugador_Load(object sender, EventArgs e)
        {
            JugadorYo.Usuario = this.Usuario;
            JugadorYo.CodigoFotoPerfil = this.CodigoFotoPerfil;
            JugadorYo.FotoPerfilUsuario = this.FotoPerfilUsuario;
            LabelFrase.Text = JugadorYo.FraseFavorita;

            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button boton)
                {
                    boton.Click += Boton_ClickConSonido;
                }
            }

            string mensaje = "2/1/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuRanking_Click(object sender, EventArgs e)
        {
            this.InformacionJugadorForm = new InformacionJugador(Servidor, "Ranking", JugadorYo.Usuario);
            InformacionJugadorForm.ShowDialog();
        }

        private void StripMenuPartidasJugadas_Click(object sender, EventArgs e)
        {
            this.InformacionJugadorForm = new InformacionJugador(Servidor, "Partidas", JugadorYo.Usuario);
            InformacionJugadorForm.ShowDialog();
        }

        private void StripMenuAmigos_Click(object sender, EventArgs e)
        {
            this.InformacionJugadorForm = new InformacionJugador(Servidor, "Amigos", JugadorYo.Usuario);
            InformacionJugadorForm.ShowDialog();
        }


        public void CambiarNumeroConectados(string NumeroConectados)
        {
            string[] Partes = NumeroConectados.Split('/');
            LableNumeroConectados.Invoke(new Action(() =>
            {
                LableNumeroConectados.Text = Partes[1];
            }));

            JugadoresConectados.Clear();
            for (int i = 2; i < Partes.Length; i++)
            {
                if (Partes[i] != JugadorYo.Usuario)
                    JugadoresConectados.Add(Partes[i]);
            }

            if (SalasDeJuego.Count > 0)
            {
                for (int i = 0; i < SalasDeJuego.Count; i++)
                {
                    if (SalasDeJuego[i].InvitarForm != null && !SalasDeJuego[i].InvitarForm.IsDisposed && SalasDeJuego[i].InvitarForm.Visible)
                    {
                        SalasDeJuego[i].InvitarForm.ActualizarConectados(JugadoresConectados);
                    }
                }
            }
        }

        private void BotonCrearSala_Click(object sender, EventArgs e)
        {
            this.CrearSalaForm = new CrearSala(Servidor, this, JugadorYo);
            this.CrearSalaForm.Show();
        }

        public void IniciarPartida(MenuJugador.Jugador JugadorYo, CrearSala.Partida Partida)
        {
            Host = false;
            SalaPartida SalaPartidaForm = new SalaPartida(Servidor, this, Partida, JugadorYo, Host);
            SalasDeJuego.Add(SalaPartidaForm);

            this.Invoke(new Action(() =>
            {
                SalaPartidaForm.Show();
                TPartida.Abort();
            }));
        }

        public void AccederPartida(MenuJugador.Jugador JugadorYo, CrearSala.Partida Partida)
        {
            ThreadStart ts = delegate { IniciarPartida(JugadorYo, Partida); };
            TPartida = new Thread(ts);
            TPartida.Start();
        }

        public void RecibirInvitacion(string JugadorInvitador, string CodigoSala)
        {
            DialogResult Resultado = MessageBox.Show(
                $"{JugadorInvitador} te ha invitado a una sala de partida.\n¿Quieres aceptar la invitación?",
                "Invitación recibida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            string Respuesta = Resultado == DialogResult.Yes ? "Aceptar" : "Rechazar";

            if (Resultado == DialogResult.Yes)
            {
                string mensaje = "5/2/" + CodigoSala;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        public void SolicitudAmistad(string Usuario)
        {
            DialogResult Resultado = MessageBox.Show(
            $"{Usuario} te ha enviado una solicitud de amistad...\n¿Quieres añadirle a tus amigos?",
            "Solicitud de amistad",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (Resultado == DialogResult.Yes)
            {
                string mensaje = "1/4/" + JugadorYo.Usuario + "/" + Usuario;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        public int NumeroFormPartida(string CodigoSala)
        {
            for (int i = 0; i < SalasDeJuego.Count; i++)
            {
                if (CodigoSala == SalasDeJuego[i].DameCodigoDeSala())
                {
                    return i;
                }
            }
            return -1;
        }

        private void ButtonFotoPerfil_Click(object sender, EventArgs e)
        {
            CambiarPerfil CambiarPerfilForm = new CambiarPerfil(JugadorYo, Servidor);
            CambiarPerfilForm.ShowDialog();
        }

        public void CambiarDatosPerfil(int CodigoFotoPerfil, string FotoPerfil, string NombreUsuario, string FraseFavorita)
        {
            this.Usuario = NombreUsuario;
            this.FotoPerfilUsuario = FotoPerfil;
            JugadorYo.Usuario = NombreUsuario;
            JugadorYo.FotoPerfilUsuario = FotoPerfil;
            JugadorYo.CodigoFotoPerfil = CodigoFotoPerfil;
            JugadorYo.FraseFavorita = FraseFavorita;

            

            ButtonFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfil);
            ButtonFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

            LabelNombreUsuario.Invoke(new Action(() =>
            {
                LabelNombreUsuario.Text = NombreUsuario;
            }));
            LabelFrase.Invoke(new Action(() =>
            {
                LabelFrase.Text = FraseFavorita;
            }));
        }

        public void ActualizarPartidas(string mensaje)
        {
            string[] Partes = mensaje.Split('/');

            DataGridViewPartidas.Invoke(new Action(() =>
            {
                DataGridViewPartidas.Columns.Clear();
                DataGridViewPartidas.Columns.Add("Partidas", "¡Únete a una sala!");
                DataGridViewPartidas.Rows.Clear();

                for (int i = 2; i < Partes.Length; i += 2)
                {
                    string Partida = Partes[i].Trim();
                    string Estado;

                    if (!string.IsNullOrEmpty(Partida))
                    {
                        if (Convert.ToInt32(Partes[i + 1].Trim()) == 0)
                            Estado = "Sala de espera";
                        else
                            Estado = "Partida Iniciada";

                        if (SalasDeJuego.Count > 0)
                        {
                            for (int j = 0; j < SalasDeJuego.Count; j++)
                            {
                                if (SalasDeJuego[j].DameCodigoDeSala() != Partida)
                                {
                                    int index = DataGridViewPartidas.Rows.Add($"Sala Nº{Partida}\nEstado: {Estado}");
                                    DataGridViewPartidas.Rows[index].Tag = Partida;
                                }
                            }
                        }
                        else
                        {
                            int index = DataGridViewPartidas.Rows.Add($"Sala Nº{Partida}\nEstado: {Estado}");
                            DataGridViewPartidas.Rows[index].Tag = Partida;
                        }
                    }
                }
            }));
        }

        private void DataGridViewPartidas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult Resultado = MessageBox.Show(
            $"¿Quieres unirte a la Sala Nº{DataGridViewPartidas.Rows[e.RowIndex].Tag.ToString()}?",
            "Unirse a Sala",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (Resultado == DialogResult.Yes)
            {
                string mensaje = "5/2/" + DataGridViewPartidas.Rows[e.RowIndex].Tag.ToString();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
        }

        private void DataGridViewPartidas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void BotonComoUnirse_Click(object sender, EventArgs e)
        {
            ComoUnirse ComoUnirseForm = new ComoUnirse();
            ComoUnirseForm.Show();
        }

        private void Boton_ClickConSonido(object sender, EventArgs e)
        {
            string ruta = @"Audios\Click.wav";
            SoundPlayer player = new SoundPlayer(ruta);
            player.Play();
        }

        private void BotonCerrarSesion_Click(object sender, EventArgs e)
        {
            string mensaje = "2/0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);

            this.Close();
        }

        private void MenuJugador_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje = "2/0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void darseDeBajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Estás seguro que quieres darte de baja? Se eliminarán todos tus datos.",
                "Confirmar baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                string mensaje = $"1/5/{Usuario}";
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
                this.Close();
            }
        
        }
    }
}