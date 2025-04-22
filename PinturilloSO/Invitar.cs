using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        MenuJugador.Jugador PerfilJugador;
        CrearSala.Partida Partida;
       
        
        string Usuario;
        public Invitar(Socket Servidor, MenuJugador MenuJugadorForm, MenuJugador.Jugador PerfilJugador, CrearSala.Partida Partida)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.MenuJugadorForm = MenuJugadorForm;
            this.PerfilJugador = PerfilJugador;
            this.Usuario = PerfilJugador.Usuario;
            this.Partida = Partida;

        }

        private void Invitar_Load(object sender, EventArgs e)
        {
            MenuJugadorForm.CopiarDataGridView(DataGridViewConectados);
            DataGridViewConectados.CellClick += DataGridViewConectados_CellClick;
        }
        private void DataGridViewConectados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignorar si hace clic en el encabezado o fuera de los límites
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string usuarioInvitado = DataGridViewConectados.Rows[e.RowIndex].Cells[0].Value.ToString();

            if (!string.IsNullOrEmpty(usuarioInvitado))
            {
                DialogResult resultado = MessageBox.Show($"¿Quieres invitar a {usuarioInvitado} a jugar?", "Invitación", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    // Aquí envías la invitación al servidor
                    string mensaje = $"7/{Usuario}/{usuarioInvitado}/{Partida.Codigo}";
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                    Servidor.Send(msg);

                    MessageBox.Show($"Invitación enviada a {usuarioInvitado}.");
                }
            }
        }
    }
}
