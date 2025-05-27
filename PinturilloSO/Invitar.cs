using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PinturilloSO.CrearSala;

namespace PinturilloSO
{
    public partial class Invitar : Form
    {
        Socket Servidor;
        MenuJugador MenuJugadorForm;
        MenuJugador.Jugador JugadorYo;
        CrearSala.Partida Partida;
        
        string Usuario;
        private ControlScaler _scaler;

        public Invitar(Socket Servidor, MenuJugador MenuJugadorForm, MenuJugador.Jugador PerfilJugador, CrearSala.Partida Partida)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.MenuJugadorForm = MenuJugadorForm;
            this.JugadorYo = PerfilJugador;
            this.Usuario = PerfilJugador.Usuario;
            this.Partida = Partida;

            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void Invitar_Load(object sender, EventArgs e)
        {
            ActualizarConectados(MenuJugadorForm.JugadoresConectados);

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

        public void ActualizarConectados(List<string> JugadoresConectados)
        {
            this.Invoke(new Action(() =>
            {
                for (int i = 0; i < MenuJugadorForm.JugadoresConectados.Count; i++)
                {
                    if (Partida.Jugadores.Count > 1)
                    {
                        for (int j = 0; j < Partida.Jugadores.Count; j++)
                        {
                            if (MenuJugadorForm.JugadoresConectados[i] != Partida.Jugadores[j].Usuario
                            && Partida.Jugadores[j].Usuario != JugadorYo.Usuario)
                            {
                                int index = DataGridViewConectados.Rows.Add($"{MenuJugadorForm.JugadoresConectados[i]}");
                                DataGridViewConectados.Rows[index].Tag = MenuJugadorForm.JugadoresConectados[i];
                            }
                        }
                    }
                    else
                    {
                        int index = DataGridViewConectados.Rows.Add($"{MenuJugadorForm.JugadoresConectados[i]}");
                        DataGridViewConectados.Rows[index].Tag = MenuJugadorForm.JugadoresConectados[i];
                    }
                }
            }));
        }

        private void DataGridViewConectados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string usuarioInvitado = DataGridViewConectados.Rows[e.RowIndex].Cells[0].Value.ToString();

            if (!string.IsNullOrEmpty(usuarioInvitado))
            {
                DialogResult resultado = MessageBox.Show($"¿Quieres invitar a {usuarioInvitado} a jugar?", "Invitación", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    string mensaje = $"5/8/{Usuario}/{usuarioInvitado}/{Partida.Codigo}";
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);

                    MessageBox.Show($"Invitación enviada a {usuarioInvitado}.");
                }
            }
        }

        private void DataGridViewConectados_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Cursor = Cursors.Hand;
            }
        }
    }
}
