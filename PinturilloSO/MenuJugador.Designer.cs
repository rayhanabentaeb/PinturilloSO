namespace PinturilloSO
{
    partial class MenuJugador
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LableNumeroConectados = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BotonCerrarSesion = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.informacionDelJugadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuRanking = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuPartidasJugadas = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuAmigos = new System.Windows.Forms.ToolStripMenuItem();
            this.darseDeBajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelNombreUsuario = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.DataGridViewPartidas = new System.Windows.Forms.DataGridView();
            this.Partidas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BotonCrearSala = new System.Windows.Forms.Button();
            this.LabelCrearSala = new System.Windows.Forms.Label();
            this.LabelComoUnirse = new System.Windows.Forms.Label();
            this.BotonComoUnirse = new System.Windows.Forms.Button();
            this.ButtonFotoPerfil = new System.Windows.Forms.Button();
            this.LabelFrase = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewPartidas)).BeginInit();
            this.SuspendLayout();
            // 
            // LableNumeroConectados
            // 
            this.LableNumeroConectados.BackColor = System.Drawing.SystemColors.HighlightText;
            this.LableNumeroConectados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LableNumeroConectados.Font = new System.Drawing.Font("Cooper Black", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LableNumeroConectados.Location = new System.Drawing.Point(1572, 952);
            this.LableNumeroConectados.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LableNumeroConectados.Name = "LableNumeroConectados";
            this.LableNumeroConectados.Size = new System.Drawing.Size(274, 130);
            this.LableNumeroConectados.TabIndex = 23;
            this.LableNumeroConectados.Text = "0";
            this.LableNumeroConectados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(1588, 848);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 84);
            this.label1.TabIndex = 22;
            this.label1.Text = "Jugadores\r\nConectados";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonCerrarSesion
            // 
            this.BotonCerrarSesion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonCerrarSesion.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonCerrarSesion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonCerrarSesion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonCerrarSesion.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonCerrarSesion.Location = new System.Drawing.Point(19, 1085);
            this.BotonCerrarSesion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BotonCerrarSesion.Name = "BotonCerrarSesion";
            this.BotonCerrarSesion.Size = new System.Drawing.Size(376, 69);
            this.BotonCerrarSesion.TabIndex = 21;
            this.BotonCerrarSesion.Text = "CERRAR SESIÓN";
            this.BotonCerrarSesion.UseVisualStyleBackColor = true;
            this.BotonCerrarSesion.Click += new System.EventHandler(this.BotonCerrarSesion_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informacionDelJugadorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1891, 46);
            this.menuStrip1.TabIndex = 27;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // informacionDelJugadorToolStripMenuItem
            // 
            this.informacionDelJugadorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenuRanking,
            this.StripMenuPartidasJugadas,
            this.StripMenuAmigos,
            this.darseDeBajaToolStripMenuItem});
            this.informacionDelJugadorToolStripMenuItem.Font = new System.Drawing.Font("Cooper Black", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informacionDelJugadorToolStripMenuItem.Name = "informacionDelJugadorToolStripMenuItem";
            this.informacionDelJugadorToolStripMenuItem.Size = new System.Drawing.Size(452, 42);
            this.informacionDelJugadorToolStripMenuItem.Text = "Información del Jugador";
            // 
            // StripMenuRanking
            // 
            this.StripMenuRanking.Name = "StripMenuRanking";
            this.StripMenuRanking.Size = new System.Drawing.Size(440, 46);
            this.StripMenuRanking.Text = "Ranking";
            this.StripMenuRanking.Click += new System.EventHandler(this.StripMenuRanking_Click);
            // 
            // StripMenuPartidasJugadas
            // 
            this.StripMenuPartidasJugadas.Name = "StripMenuPartidasJugadas";
            this.StripMenuPartidasJugadas.Size = new System.Drawing.Size(440, 46);
            this.StripMenuPartidasJugadas.Text = "Partidas Jugadas";
            this.StripMenuPartidasJugadas.Click += new System.EventHandler(this.StripMenuPartidasJugadas_Click);
            // 
            // StripMenuAmigos
            // 
            this.StripMenuAmigos.Name = "StripMenuAmigos";
            this.StripMenuAmigos.Size = new System.Drawing.Size(440, 46);
            this.StripMenuAmigos.Text = "Amigos";
            this.StripMenuAmigos.Click += new System.EventHandler(this.StripMenuAmigos_Click);
            // 
            // darseDeBajaToolStripMenuItem
            // 
            this.darseDeBajaToolStripMenuItem.Name = "darseDeBajaToolStripMenuItem";
            this.darseDeBajaToolStripMenuItem.Size = new System.Drawing.Size(440, 46);
            this.darseDeBajaToolStripMenuItem.Text = "Darse de baja";
            this.darseDeBajaToolStripMenuItem.Click += new System.EventHandler(this.darseDeBajaToolStripMenuItem_Click);
            // 
            // LabelNombreUsuario
            // 
            this.LabelNombreUsuario.AutoSize = true;
            this.LabelNombreUsuario.BackColor = System.Drawing.Color.Transparent;
            this.LabelNombreUsuario.Font = new System.Drawing.Font("Cooper Black", 26.03077F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelNombreUsuario.ForeColor = System.Drawing.SystemColors.Window;
            this.LabelNombreUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelNombreUsuario.Location = new System.Drawing.Point(19, 69);
            this.LabelNombreUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelNombreUsuario.Name = "LabelNombreUsuario";
            this.LabelNombreUsuario.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LabelNombreUsuario.Size = new System.Drawing.Size(315, 80);
            this.LabelNombreUsuario.TabIndex = 28;
            this.LabelNombreUsuario.Text = "Usuario";
            this.LabelNombreUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataGridViewPartidas
            // 
            this.DataGridViewPartidas.AllowUserToAddRows = false;
            this.DataGridViewPartidas.AllowUserToDeleteRows = false;
            this.DataGridViewPartidas.AllowUserToResizeColumns = false;
            this.DataGridViewPartidas.AllowUserToResizeRows = false;
            this.DataGridViewPartidas.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DataGridViewPartidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewPartidas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridViewPartidas.BackgroundColor = System.Drawing.Color.White;
            this.DataGridViewPartidas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridViewPartidas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 13.84615F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewPartidas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewPartidas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Partidas});
            this.DataGridViewPartidas.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewPartidas.DefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridViewPartidas.EnableHeadersVisualStyles = false;
            this.DataGridViewPartidas.GridColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewPartidas.Location = new System.Drawing.Point(19, 475);
            this.DataGridViewPartidas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DataGridViewPartidas.Name = "DataGridViewPartidas";
            this.DataGridViewPartidas.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewPartidas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewPartidas.RowHeadersVisible = false;
            this.DataGridViewPartidas.RowHeadersWidth = 56;
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9.969231F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewPartidas.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewPartidas.RowTemplate.Height = 24;
            this.DataGridViewPartidas.RowTemplate.ReadOnly = true;
            this.DataGridViewPartidas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataGridViewPartidas.Size = new System.Drawing.Size(376, 631);
            this.DataGridViewPartidas.TabIndex = 30;
            this.DataGridViewPartidas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewPartidas_CellDoubleClick);
            this.DataGridViewPartidas.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewPartidas_CellMouseMove);
            // 
            // Partidas
            // 
            this.Partidas.HeaderText = "¡Únete a una sala!";
            this.Partidas.MinimumWidth = 7;
            this.Partidas.Name = "Partidas";
            this.Partidas.ReadOnly = true;
            // 
            // BotonCrearSala
            // 
            this.BotonCrearSala.AutoSize = true;
            this.BotonCrearSala.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonCrearSala.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonCrearSala.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonCrearSala.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonCrearSala.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonCrearSala.Font = new System.Drawing.Font("Cooper Black", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonCrearSala.Location = new System.Drawing.Point(888, 675);
            this.BotonCrearSala.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BotonCrearSala.Name = "BotonCrearSala";
            this.BotonCrearSala.Size = new System.Drawing.Size(337, 119);
            this.BotonCrearSala.TabIndex = 34;
            this.BotonCrearSala.Text = "Crear";
            this.BotonCrearSala.UseVisualStyleBackColor = true;
            this.BotonCrearSala.Click += new System.EventHandler(this.BotonCrearSala_Click);
            // 
            // LabelCrearSala
            // 
            this.LabelCrearSala.AutoSize = true;
            this.LabelCrearSala.BackColor = System.Drawing.Color.Transparent;
            this.LabelCrearSala.Font = new System.Drawing.Font("Cooper Black", 84.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCrearSala.ForeColor = System.Drawing.SystemColors.Window;
            this.LabelCrearSala.Location = new System.Drawing.Point(404, 422);
            this.LabelCrearSala.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelCrearSala.Name = "LabelCrearSala";
            this.LabelCrearSala.Size = new System.Drawing.Size(1297, 259);
            this.LabelCrearSala.TabIndex = 32;
            this.LabelCrearSala.Text = "Crear Sala";
            // 
            // LabelComoUnirse
            // 
            this.LabelComoUnirse.AutoSize = true;
            this.LabelComoUnirse.BackColor = System.Drawing.Color.Transparent;
            this.LabelComoUnirse.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelComoUnirse.ForeColor = System.Drawing.SystemColors.Window;
            this.LabelComoUnirse.Location = new System.Drawing.Point(1544, 69);
            this.LabelComoUnirse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelComoUnirse.Name = "LabelComoUnirse";
            this.LabelComoUnirse.Size = new System.Drawing.Size(307, 42);
            this.LabelComoUnirse.TabIndex = 36;
            this.LabelComoUnirse.Text = "¿Cómo me uno?";
            this.LabelComoUnirse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonComoUnirse
            // 
            this.BotonComoUnirse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonComoUnirse.BackColor = System.Drawing.Color.Transparent;
            this.BotonComoUnirse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BotonComoUnirse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonComoUnirse.FlatAppearance.BorderSize = 0;
            this.BotonComoUnirse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BotonComoUnirse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BotonComoUnirse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BotonComoUnirse.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonComoUnirse.Location = new System.Drawing.Point(1596, 100);
            this.BotonComoUnirse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BotonComoUnirse.Name = "BotonComoUnirse";
            this.BotonComoUnirse.Size = new System.Drawing.Size(229, 202);
            this.BotonComoUnirse.TabIndex = 37;
            this.BotonComoUnirse.UseVisualStyleBackColor = false;
            this.BotonComoUnirse.Click += new System.EventHandler(this.BotonComoUnirse_Click);
            // 
            // ButtonFotoPerfil
            // 
            this.ButtonFotoPerfil.BackColor = System.Drawing.Color.Transparent;
            this.ButtonFotoPerfil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFotoPerfil.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ButtonFotoPerfil.FlatAppearance.BorderSize = 3;
            this.ButtonFotoPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFotoPerfil.Location = new System.Drawing.Point(36, 158);
            this.ButtonFotoPerfil.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonFotoPerfil.Name = "ButtonFotoPerfil";
            this.ButtonFotoPerfil.Size = new System.Drawing.Size(240, 209);
            this.ButtonFotoPerfil.TabIndex = 31;
            this.ButtonFotoPerfil.UseVisualStyleBackColor = false;
            this.ButtonFotoPerfil.Click += new System.EventHandler(this.ButtonFotoPerfil_Click);
            // 
            // LabelFrase
            // 
            this.LabelFrase.AutoSize = true;
            this.LabelFrase.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFrase.Location = new System.Drawing.Point(284, 306);
            this.LabelFrase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelFrase.Name = "LabelFrase";
            this.LabelFrase.Size = new System.Drawing.Size(155, 65);
            this.LabelFrase.TabIndex = 38;
            this.LabelFrase.Text = "label2";
            // 
            // MenuJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(1891, 1218);
            this.Controls.Add(this.LabelFrase);
            this.Controls.Add(this.LabelComoUnirse);
            this.Controls.Add(this.BotonComoUnirse);
            this.Controls.Add(this.BotonCrearSala);
            this.Controls.Add(this.LabelCrearSala);
            this.Controls.Add(this.ButtonFotoPerfil);
            this.Controls.Add(this.DataGridViewPartidas);
            this.Controls.Add(this.LabelNombreUsuario);
            this.Controls.Add(this.LableNumeroConectados);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotonCerrarSesion);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MenuJugador";
            this.Text = "MenuJugador";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuJugador_FormClosing);
            this.Load += new System.EventHandler(this.MenuJugador_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewPartidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LableNumeroConectados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BotonCerrarSesion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem informacionDelJugadorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StripMenuRanking;
        private System.Windows.Forms.ToolStripMenuItem StripMenuPartidasJugadas;
        private System.Windows.Forms.ToolStripMenuItem StripMenuAmigos;
        private System.Windows.Forms.Label LabelNombreUsuario;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView DataGridViewPartidas;
        private System.Windows.Forms.Button BotonCrearSala;
        private System.Windows.Forms.Label LabelCrearSala;
        private System.Windows.Forms.Label LabelComoUnirse;
        private System.Windows.Forms.Button BotonComoUnirse;
        private System.Windows.Forms.Button ButtonFotoPerfil;
        private System.Windows.Forms.DataGridViewTextBoxColumn Partidas;
        private System.Windows.Forms.ToolStripMenuItem darseDeBajaToolStripMenuItem;
        private System.Windows.Forms.Label LabelFrase;
    }
}