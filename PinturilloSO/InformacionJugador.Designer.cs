namespace PinturilloSO
{
    partial class InformacionJugador
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LabelInformacion = new System.Windows.Forms.Label();
            this.JugadoresConectados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewInformacion = new System.Windows.Forms.DataGridView();
            this.dateTimeDesde = new System.Windows.Forms.DateTimePicker();
            this.dateTimeHasta = new System.Windows.Forms.DateTimePicker();
            this.txtBuscarJugador = new System.Windows.Forms.TextBox();
            this.btnFiltrarPorFecha = new System.Windows.Forms.Button();
            this.btnFiltrarPorJugador = new System.Windows.Forms.Button();
            this.btnBorrarFiltros = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewInformacion)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelInformacion
            // 
            this.LabelInformacion.Font = new System.Drawing.Font("Cooper Black", 22.15385F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInformacion.Location = new System.Drawing.Point(378, 0);
            this.LabelInformacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelInformacion.Name = "LabelInformacion";
            this.LabelInformacion.Size = new System.Drawing.Size(1018, 125);
            this.LabelInformacion.TabIndex = 32;
            this.LabelInformacion.Text = "Información Jugador";
            this.LabelInformacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelInformacion.Click += new System.EventHandler(this.LabelInformacion_Click);
            // 
            // JugadoresConectados
            // 
            this.JugadoresConectados.HeaderText = "Jugadores Conectados";
            this.JugadoresConectados.MinimumWidth = 7;
            this.JugadoresConectados.Name = "JugadoresConectados";
            this.JugadoresConectados.ReadOnly = true;
            // 
            // DataGridViewInformacion
            // 
            this.DataGridViewInformacion.AllowUserToAddRows = false;
            this.DataGridViewInformacion.AllowUserToDeleteRows = false;
            this.DataGridViewInformacion.AllowUserToResizeColumns = false;
            this.DataGridViewInformacion.AllowUserToResizeRows = false;
            this.DataGridViewInformacion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DataGridViewInformacion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewInformacion.BackgroundColor = System.Drawing.Color.White;
            this.DataGridViewInformacion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 13.84615F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewInformacion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridViewInformacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewInformacion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JugadoresConectados});
            this.DataGridViewInformacion.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewInformacion.DefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridViewInformacion.EnableHeadersVisualStyles = false;
            this.DataGridViewInformacion.GridColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewInformacion.Location = new System.Drawing.Point(165, 275);
            this.DataGridViewInformacion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DataGridViewInformacion.Name = "DataGridViewInformacion";
            this.DataGridViewInformacion.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewInformacion.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DataGridViewInformacion.RowHeadersVisible = false;
            this.DataGridViewInformacion.RowHeadersWidth = 56;
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewInformacion.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewInformacion.RowTemplate.Height = 36;
            this.DataGridViewInformacion.RowTemplate.ReadOnly = true;
            this.DataGridViewInformacion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataGridViewInformacion.Size = new System.Drawing.Size(1706, 559);
            this.DataGridViewInformacion.TabIndex = 33;
            this.DataGridViewInformacion.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewInformacion_CellDoubleClick);
            this.DataGridViewInformacion.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewInformacion_CellMouseMove);
            // 
            // dateTimeDesde
            // 
            this.dateTimeDesde.Location = new System.Drawing.Point(1289, 57);
            this.dateTimeDesde.Name = "dateTimeDesde";
            this.dateTimeDesde.Size = new System.Drawing.Size(200, 31);
            this.dateTimeDesde.TabIndex = 34;
            // 
            // dateTimeHasta
            // 
            this.dateTimeHasta.Location = new System.Drawing.Point(1289, 127);
            this.dateTimeHasta.Name = "dateTimeHasta";
            this.dateTimeHasta.Size = new System.Drawing.Size(200, 31);
            this.dateTimeHasta.TabIndex = 35;
            // 
            // txtBuscarJugador
            // 
            this.txtBuscarJugador.Location = new System.Drawing.Point(1289, 224);
            this.txtBuscarJugador.Name = "txtBuscarJugador";
            this.txtBuscarJugador.Size = new System.Drawing.Size(204, 31);
            this.txtBuscarJugador.TabIndex = 37;
            // 
            // btnFiltrarPorFecha
            // 
            this.btnFiltrarPorFecha.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrarPorFecha.Location = new System.Drawing.Point(1540, 74);
            this.btnFiltrarPorFecha.Name = "btnFiltrarPorFecha";
            this.btnFiltrarPorFecha.Size = new System.Drawing.Size(155, 72);
            this.btnFiltrarPorFecha.TabIndex = 38;
            this.btnFiltrarPorFecha.Text = "Filtrar por Fecha";
            this.btnFiltrarPorFecha.UseVisualStyleBackColor = true;
            // 
            // btnFiltrarPorJugador
            // 
            this.btnFiltrarPorJugador.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrarPorJugador.Location = new System.Drawing.Point(1540, 183);
            this.btnFiltrarPorJugador.Name = "btnFiltrarPorJugador";
            this.btnFiltrarPorJugador.Size = new System.Drawing.Size(155, 72);
            this.btnFiltrarPorJugador.TabIndex = 39;
            this.btnFiltrarPorJugador.Text = "Filtrar por Jugador";
            this.btnFiltrarPorJugador.UseVisualStyleBackColor = true;
            // 
            // btnBorrarFiltros
            // 
            this.btnBorrarFiltros.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBorrarFiltros.Location = new System.Drawing.Point(1760, 127);
            this.btnBorrarFiltros.Name = "btnBorrarFiltros";
            this.btnBorrarFiltros.Size = new System.Drawing.Size(155, 72);
            this.btnBorrarFiltros.TabIndex = 40;
            this.btnBorrarFiltros.Text = "Borrar Filtros";
            this.btnBorrarFiltros.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1285, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 24);
            this.label1.TabIndex = 41;
            this.label1.Text = "Introducir Nombre";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1285, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 24);
            this.label2.TabIndex = 42;
            this.label2.Text = "Hasta:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Cooper Black", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1285, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 24);
            this.label3.TabIndex = 43;
            this.label3.Text = "Desde:";
            // 
            // InformacionJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(2037, 999);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBorrarFiltros);
            this.Controls.Add(this.btnFiltrarPorJugador);
            this.Controls.Add(this.btnFiltrarPorFecha);
            this.Controls.Add(this.txtBuscarJugador);
            this.Controls.Add(this.dateTimeHasta);
            this.Controls.Add(this.dateTimeDesde);
            this.Controls.Add(this.DataGridViewInformacion);
            this.Controls.Add(this.LabelInformacion);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "InformacionJugador";
            this.Text = "InformacionJugador";
            this.Load += new System.EventHandler(this.InformacionJugador_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewInformacion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LabelInformacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn JugadoresConectados;
        private System.Windows.Forms.DataGridView DataGridViewInformacion;
        private System.Windows.Forms.DateTimePicker dateTimeDesde;
        private System.Windows.Forms.DateTimePicker dateTimeHasta;
        private System.Windows.Forms.TextBox txtBuscarJugador;
        private System.Windows.Forms.Button btnFiltrarPorFecha;
        private System.Windows.Forms.Button btnFiltrarPorJugador;
        private System.Windows.Forms.Button btnBorrarFiltros;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}