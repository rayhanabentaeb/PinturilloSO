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
using static PinturilloSO.MenuJugador;

namespace PinturilloSO
{
    public partial class InformacionJugador : Form
    {
        Socket Servidor;
        string Relacion;
        string Usuario;

        public PerfilAmigo PerfilAmigoForm;
       
        private ControlScaler _scaler;

        public InformacionJugador(Socket Servidor, string Relacion, string Usuario)
        {
            InitializeComponent();
            this.Servidor = Servidor;
            this.Relacion = Relacion;
            this.Usuario = Usuario;
            btnFiltrarPorFecha.Click += btnFiltrarPorFecha_Click;
            btnFiltrarPorJugador.Click += btnFiltrarPorJugador_Click;
            btnBorrarFiltros.Click += btnBorrarFiltros_Click;


            this.StartPosition = FormStartPosition.CenterScreen;
            _scaler = new ControlScaler(this);
        }

        private void InformacionJugador_Load(object sender, EventArgs e)
        {
            dateTimeDesde.Visible = false;
            dateTimeHasta.Visible = false;
            txtBuscarJugador.Visible = false;
            btnFiltrarPorFecha.Visible = false;
            btnFiltrarPorJugador.Visible = false;
            btnBorrarFiltros.Visible = false;




            if (Relacion == "Ranking")
            {
                LabelInformacion.Text = "Ranking Global";

                string mensaje = "3/1/" + Usuario;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
            else if (Relacion == "Partidas")
            {
                dateTimeDesde.Visible = true;
                dateTimeHasta.Visible = true;
                txtBuscarJugador.Visible = true;
                btnFiltrarPorFecha.Visible = true;
                btnFiltrarPorJugador.Visible = true;
                btnBorrarFiltros.Visible = true;


                LabelInformacion.Text = "Partidas Jugadas";

                string mensaje = "3/2/" + Usuario;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }
            else if (Relacion == "Amigos")
            {
                LabelInformacion.Text = "Tus Amigos";

                string mensaje = "3/3/" + Usuario;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                Servidor.Send(msg);
            }

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

        public void PonerInformacion(string Informacion)
        {
            this.Invoke(new Action(() =>
            {
                txtBuscarJugador.Text = "";

                DataGridViewInformacion.Columns.Clear();

                if (Relacion == "Amigos")
                {
                    string[] Amigos = Informacion.Split('/');

                    DataGridViewInformacion.Columns.Add("Amigos", "Amigos");

                    for (int i = 0; i < Amigos.Length; i++)
                    {
                        int index = DataGridViewInformacion.Rows.Add($"{Amigos[i]}");
                        DataGridViewInformacion.Rows[index].Tag = Amigos[i];
                    }
                }
                else
                {
                    if (Relacion == "Partidas")
                    {
                        string[] Partidas = Informacion.Split('*');

                        DataGridViewInformacion.Columns.Add("Codigo", "Código");
                        DataGridViewInformacion.Columns.Add("Fecha", "Fecha");
                        DataGridViewInformacion.Columns.Add("Privacidad", "Privacidad");
                        DataGridViewInformacion.Columns.Add("Rondas", "Rondas");
                        DataGridViewInformacion.Columns.Add("Ganador", "Ganador");


                        DataGridViewInformacion.Columns.Add("Jugador1", "1er Jugador");
                        DataGridViewInformacion.Columns.Add("Jugador2", "2do Jugador");
                        DataGridViewInformacion.Columns.Add("Jugador3", "3er Jugador");
                        DataGridViewInformacion.Columns.Add("Jugador4", "4to Jugador");

                        DataGridViewInformacion.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 12, FontStyle.Bold);
                        DataGridViewInformacion.RowTemplate.DefaultCellStyle.Font = new Font("Consolas", 10);

                        string CodigoPartida;
                        string Fecha;
                        string Privacidad;
                        string Rondas;
                        string Ganador;
                        string Jugador1;
                        string Jugador2;
                        string Jugador3;
                        string Jugador4;
                        int index;

                        for (int i = 1; i < Partidas.Length; i++)
                        {
                            string[] DatosPartida = Partidas[i].Split('/');
                            if (DatosPartida.Length < 9)
                                continue; // Omitir partidas incompletas

                            CodigoPartida = DatosPartida[0];
                            Fecha = DatosPartida[1];
                            Privacidad = DatosPartida[2];
                            Rondas = DatosPartida[3];
                            Ganador = DatosPartida[4];
                            Jugador1 = DatosPartida[5];
                            Jugador2 = DatosPartida[6];
                            Jugador3 = DatosPartida[7];
                            Jugador4 = DatosPartida[8];

                            index = DataGridViewInformacion.Rows.Add(CodigoPartida, Fecha, Privacidad, Rondas, Ganador, Jugador1, Jugador2, Jugador3, Jugador4);
                            DataGridViewInformacion.Rows[index].Cells[4].Tag = Ganador;
                            DataGridViewInformacion.Rows[index].Cells[5].Tag = Jugador1;
                            DataGridViewInformacion.Rows[index].Cells[6].Tag = Jugador2;
                            DataGridViewInformacion.Rows[index].Cells[7].Tag = Jugador3;
                            DataGridViewInformacion.Rows[index].Cells[8].Tag = Jugador4;
                        }

                    }
                    else if (Relacion == "Ranking")
                    {
                        string[] Ranking = Informacion.Split('*');

                        DataGridViewInformacion.Columns.Add("Ranking", "Ranking");
                        DataGridViewInformacion.Columns.Add("Jugador", "Jugador");
                        DataGridViewInformacion.Columns.Add("Puntos", "Puntos");

                        string PosicionRanking;
                        string Jugador;
                        string Puntos;
                        int index;

                        for (int i = 1; i < Ranking.Length; i++)
                        {
                            string[] DatosRanking = Ranking[i].Split('/');

                            Jugador = DatosRanking[0];
                            Puntos = DatosRanking[1];
                            PosicionRanking = i.ToString();

                            index = DataGridViewInformacion.Rows.Add($"#{PosicionRanking}", Jugador, Puntos);
                            DataGridViewInformacion.Rows[index].Cells[1].Tag = Jugador;
                        }

                    }
                }
            }));
        }

        private void DataGridViewInformacion_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Relacion == "Amigos" && e.RowIndex >= 0)
            {
                Cursor = Cursors.Hand;
            }
            else if (Relacion == "Partidas" && e.RowIndex >= 0 && e.ColumnIndex >= 3)
            {
                Cursor = Cursors.Hand;
            }
            else if (Relacion == "Ranking" && e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void DataGridViewInformacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Relacion == "Amigos")
            {
                this.PerfilAmigoForm = new PerfilAmigo(Servidor, DataGridViewInformacion.Rows[e.RowIndex].Tag.ToString());
                PerfilAmigoForm.ShowDialog();
            }
            else if (Relacion == "Partidas" && e.ColumnIndex >= 3 && DataGridViewInformacion.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() != "-")
            {
                this.PerfilAmigoForm = new PerfilAmigo(Servidor, DataGridViewInformacion.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString());
                PerfilAmigoForm.ShowDialog();
            }
            else if (Relacion == "Ranking" && e.ColumnIndex == 1)
            {
                this.PerfilAmigoForm = new PerfilAmigo(Servidor, DataGridViewInformacion.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString());
                PerfilAmigoForm.ShowDialog();
            }
        }

        private void btnFiltrarPorFecha_Click(object sender, EventArgs e)
        {
            string fechaInicio = dateTimeDesde.Checked ? dateTimeDesde.Value.ToString("yyyy-MM-dd") : "-";
            string fechaFin = dateTimeHasta.Checked ? dateTimeHasta.Value.ToString("yyyy-MM-dd") : "-";

            string mensaje = $"3/6/{Usuario}/{fechaInicio}/{fechaFin}/-";
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void btnFiltrarPorJugador_Click(object sender, EventArgs e)
        {
            string jugador = string.IsNullOrWhiteSpace(txtBuscarJugador.Text) ? "-" : txtBuscarJugador.Text.Trim();

            string mensaje = $"3/6/{Usuario}/-/-/{jugador}";
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e)
        {
            dateTimeDesde.Checked = false;
            dateTimeHasta.Checked = false;
            txtBuscarJugador.Clear();

            string mensaje = $"3/6/{Usuario}/-/-/-";
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            Servidor.Send(msg);
        }


        private void LabelInformacion_Click(object sender, EventArgs e)
        {

        }
    }
}
