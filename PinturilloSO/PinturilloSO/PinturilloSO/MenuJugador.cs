using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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

        public CrearSala CrearSalaForm;
        public List<SalaPartida> SalasDeJuego = new List<SalaPartida>();

        public struct Jugador
        {
            public string Usuario;
            public int CodigoFotoPerfil;
            public string FotoPerfilUsuario;
        }

        public MenuJugador(string Usuario, Socket Servidor, int CodigoFotoPerfil)
        {
            InitializeComponent();
            this.Usuario = Usuario;
            this.Servidor = Servidor;
            this.CodigoFotoPerfil = CodigoFotoPerfil;
            this.FotoPerfilUsuario = @"FotosPerfil\" + CodigoFotoPerfil + ".png";
            this.AcceptButton = BotonAcceder;
            this.LabelNombreUsuario.Text = Usuario;

            JugadorYo.Usuario = Usuario;
            JugadorYo.CodigoFotoPerfil = CodigoFotoPerfil;
            JugadorYo.FotoPerfilUsuario = this.FotoPerfilUsuario;


            FormResizer.SaveInitialControlBounds(this);
            FormResizer.SetInitialFormSize(Size);
            Resize += (s, e) => FormResizer.ScaleControls(this, Size);

            ButtonFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfilUsuario);
            ButtonFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

            BotonOcultarContrasena.BackgroundImage = System.Drawing.Image.FromFile(@"Iconos\ComoJugar.png");
            BotonOcultarContrasena.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void MenuJugador_Load(object sender, EventArgs e)
        {
            JugadorYo.Usuario = this.Usuario;
            JugadorYo.CodigoFotoPerfil = this.CodigoFotoPerfil;
            JugadorYo.FotoPerfilUsuario = this.FotoPerfilUsuario;

            string mensaje = "2/1/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuRanking_Click(object sender, EventArgs e)
        {
            string mensaje = "3/1/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuPartidasJugadas_Click(object sender, EventArgs e)
        {
            string mensaje = "3/2/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void StripMenuAmigos_Click(object sender, EventArgs e)
        {
            string mensaje = "3/3/" + Usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        public void MostrarRelacion(string Relacion)
        {
            MessageBox.Show(Relacion);
        }

        public void CambiarNumeroConectados(string NumeroConectados)
        {
            LableNumeroConectados.Invoke(new Action(() =>
            {
                LableNumeroConectados.Text = NumeroConectados;
            }));
        }
        private void BotonAcceder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextCodigoSala.Text))
            {
                bool CodigoEsNumerico = TextCodigoSala.Text.All(char.IsDigit);

                if (CodigoEsNumerico)
                {
                    string mensaje = "5/2/" + TextCodigoSala.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);
                }
                else
                {
                    MessageBox.Show("Por favor, introduce un código válido.");
                }
            }
        }

        public void AccederPartida(MenuJugador.Jugador JugadorYo, CrearSala.Partida Partida)
        {
            ThreadStart ts = delegate { IniciarPartida(JugadorYo, Partida); };
            TPartida = new Thread(ts);
            TPartida.Start();
        }

        public void IniciarPartida(MenuJugador.Jugador JugadorYo, CrearSala.Partida Partida)
        {
            Host = false;
            SalaPartida SalaPartidaForm = new SalaPartida(Servidor, this, Partida, JugadorYo, Host);
            SalasDeJuego.Add(SalaPartidaForm);

            this.Invoke(new Action(() =>
            {
                TextCodigoSala.Clear();
                this.Hide();
                SalaPartidaForm.ShowDialog();
                TPartida.Abort();
                this.Show();
            }));
        }

        private void ButtonFotoPerfil_Click(object sender, EventArgs e)
        {
            CambiarPerfil CambiarPerfilForm = new CambiarPerfil(JugadorYo, Servidor);
            this.Hide();
            CambiarPerfilForm.ShowDialog();
            this.Show();
        }

        public void CambiarDatosPerfil(string FotoPerfil, string NombreUsuario)
        {
            ButtonFotoPerfil.BackgroundImage = System.Drawing.Image.FromFile(FotoPerfil);
            ButtonFotoPerfil.BackgroundImageLayout = ImageLayout.Stretch;

            LabelNombreUsuario.Invoke(new Action(() =>
            {
                LabelNombreUsuario.Text = NombreUsuario;
            }));
        }

        private void BotonCrearSala_Click(object sender, EventArgs e)
        {
            this.CrearSalaForm = new CrearSala(Servidor, this, JugadorYo);
            //this.Hide();
            this.CrearSalaForm.Show();
            //this.Show();
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

        public void ActualizarConectados(string mensaje)
        {
            string[] Partes = mensaje.Split('/');

            DataGridViewConectados.Invoke(new Action(() =>
            {
                LableNumeroConectados.Text = Partes[0];

                DataGridViewConectados.Columns.Clear();
                DataGridViewConectados.Columns.Add("Jugadores", "Jugadores conectados");
                DataGridViewConectados.Rows.Clear();

                // Empezamos desde el índice 1 para omitir el número de conectados
                for (int i = 1; i < Partes.Length; i++)
                {
                    string usuarioLimpio = Partes[i].Trim();

                    // No agregamos si es el mismo usuario que el nuestro
                    if (!string.IsNullOrEmpty(usuarioLimpio) && usuarioLimpio != Usuario)
                    {
                        DataGridViewConectados.Rows.Add(usuarioLimpio);
                    }
                }
            }));
        }
        public void CopiarDataGridView(DataGridView CopiaDataGridView)
        {
            foreach (DataGridViewColumn column in DataGridViewConectados.Columns)
            {
                CopiaDataGridView.Columns.Add(column.Name, column.HeaderText);
            }

            foreach (DataGridViewRow row in DataGridViewConectados.Rows)
            {
                if (row.IsNewRow) continue;
                CopiaDataGridView.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value).ToArray());
            }
        }
    }
}
