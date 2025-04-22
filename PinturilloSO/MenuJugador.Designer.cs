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
            this.LableNumeroConectados = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BotonCerrarSesion = new System.Windows.Forms.Button();
            this.LabelAccederSala = new System.Windows.Forms.Label();
            this.TextCodigoSala = new System.Windows.Forms.TextBox();
            this.BotonAcceder = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.informacionDelJugadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuRanking = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuPartidasJugadas = new System.Windows.Forms.ToolStripMenuItem();
            this.StripMenuAmigos = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelNombreUsuario = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.DataGridViewConectados = new System.Windows.Forms.DataGridView();
            this.BotonCrearSala = new System.Windows.Forms.Button();
            this.LabelCrearSala = new System.Windows.Forms.Label();
            this.LabelTituloPrincipal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BotonOcultarContrasena = new System.Windows.Forms.Button();
            this.ButtonFotoPerfil = new System.Windows.Forms.Button();
            this.ColumnaJugadores = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).BeginInit();
            this.SuspendLayout();
            // 
            // LableNumeroConectados
            // 
            this.LableNumeroConectados.BackColor = System.Drawing.SystemColors.HighlightText;
            this.LableNumeroConectados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LableNumeroConectados.Font = new System.Drawing.Font("Cooper Black", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LableNumeroConectados.Location = new System.Drawing.Point(1179, 761);
            this.LableNumeroConectados.Name = "LableNumeroConectados";
            this.LableNumeroConectados.Size = new System.Drawing.Size(206, 103);
            this.LableNumeroConectados.TabIndex = 23;
            this.LableNumeroConectados.Text = "0";
            this.LableNumeroConectados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1191, 678);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 64);
            this.label1.TabIndex = 22;
            this.label1.Text = "Jugadores\r\nConectados";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonCerrarSesion
            // 
            this.BotonCerrarSesion.AutoSize = true;
            this.BotonCerrarSesion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonCerrarSesion.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonCerrarSesion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonCerrarSesion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonCerrarSesion.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonCerrarSesion.Location = new System.Drawing.Point(14, 829);
            this.BotonCerrarSesion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BotonCerrarSesion.Name = "BotonCerrarSesion";
            this.BotonCerrarSesion.Size = new System.Drawing.Size(323, 47);
            this.BotonCerrarSesion.TabIndex = 21;
            this.BotonCerrarSesion.Text = "CERRAR SESION";
            this.BotonCerrarSesion.UseVisualStyleBackColor = true;
            this.BotonCerrarSesion.Click += new System.EventHandler(this.BotonCerrarSesion_Click);
            // 
            // LabelAccederSala
            // 
            this.LabelAccederSala.AutoSize = true;
            this.LabelAccederSala.BackColor = System.Drawing.Color.Transparent;
            this.LabelAccederSala.Font = new System.Drawing.Font("Cooper Black", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelAccederSala.Location = new System.Drawing.Point(470, 582);
            this.LabelAccederSala.Name = "LabelAccederSala";
            this.LabelAccederSala.Size = new System.Drawing.Size(563, 82);
            this.LabelAccederSala.TabIndex = 24;
            this.LabelAccederSala.Text = "Código de Sala";
            // 
            // TextCodigoSala
            // 
            this.TextCodigoSala.Font = new System.Drawing.Font("Cooper Black", 28.24615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextCodigoSala.Location = new System.Drawing.Point(554, 694);
            this.TextCodigoSala.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextCodigoSala.Name = "TextCodigoSala";
            this.TextCodigoSala.Size = new System.Drawing.Size(404, 72);
            this.TextCodigoSala.TabIndex = 25;
            this.TextCodigoSala.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BotonAcceder
            // 
            this.BotonAcceder.AutoSize = true;
            this.BotonAcceder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonAcceder.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonAcceder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonAcceder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonAcceder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonAcceder.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonAcceder.Location = new System.Drawing.Point(650, 794);
            this.BotonAcceder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BotonAcceder.Name = "BotonAcceder";
            this.BotonAcceder.Size = new System.Drawing.Size(198, 55);
            this.BotonAcceder.TabIndex = 26;
            this.BotonAcceder.Text = "Acceder";
            this.BotonAcceder.UseVisualStyleBackColor = true;
            this.BotonAcceder.Click += new System.EventHandler(this.BotonAcceder_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informacionDelJugadorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1418, 37);
            this.menuStrip1.TabIndex = 27;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // informacionDelJugadorToolStripMenuItem
            // 
            this.informacionDelJugadorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenuRanking,
            this.StripMenuPartidasJugadas,
            this.StripMenuAmigos});
            this.informacionDelJugadorToolStripMenuItem.Font = new System.Drawing.Font("Cooper Black", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.informacionDelJugadorToolStripMenuItem.Name = "informacionDelJugadorToolStripMenuItem";
            this.informacionDelJugadorToolStripMenuItem.Size = new System.Drawing.Size(342, 33);
            this.informacionDelJugadorToolStripMenuItem.Text = "Información del Jugador";
            // 
            // StripMenuRanking
            // 
            this.StripMenuRanking.Name = "StripMenuRanking";
            this.StripMenuRanking.Size = new System.Drawing.Size(332, 38);
            this.StripMenuRanking.Text = "Ranking";
            this.StripMenuRanking.Click += new System.EventHandler(this.StripMenuRanking_Click);
            // 
            // StripMenuPartidasJugadas
            // 
            this.StripMenuPartidasJugadas.Name = "StripMenuPartidasJugadas";
            this.StripMenuPartidasJugadas.Size = new System.Drawing.Size(332, 38);
            this.StripMenuPartidasJugadas.Text = "Partidas Jugadas";
            this.StripMenuPartidasJugadas.Click += new System.EventHandler(this.StripMenuPartidasJugadas_Click);
            // 
            // StripMenuAmigos
            // 
            this.StripMenuAmigos.Name = "StripMenuAmigos";
            this.StripMenuAmigos.Size = new System.Drawing.Size(332, 38);
            this.StripMenuAmigos.Text = "Amigos";
            this.StripMenuAmigos.Click += new System.EventHandler(this.StripMenuAmigos_Click);
            // 
            // LabelNombreUsuario
            // 
            this.LabelNombreUsuario.AutoSize = true;
            this.LabelNombreUsuario.BackColor = System.Drawing.Color.Transparent;
            this.LabelNombreUsuario.Font = new System.Drawing.Font("Cooper Black", 26.03077F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelNombreUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelNombreUsuario.Location = new System.Drawing.Point(14, 230);
            this.LabelNombreUsuario.Name = "LabelNombreUsuario";
            this.LabelNombreUsuario.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LabelNombreUsuario.Size = new System.Drawing.Size(240, 61);
            this.LabelNombreUsuario.TabIndex = 28;
            this.LabelNombreUsuario.Text = "Usuario";
            this.LabelNombreUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataGridViewConectados
            // 
            this.DataGridViewConectados.BackgroundColor = System.Drawing.Color.Chocolate;
            this.DataGridViewConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewConectados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnaJugadores});
            this.DataGridViewConectados.Location = new System.Drawing.Point(14, 312);
            this.DataGridViewConectados.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DataGridViewConectados.Name = "DataGridViewConectados";
            this.DataGridViewConectados.RowHeadersWidth = 56;
            this.DataGridViewConectados.RowTemplate.Height = 24;
            this.DataGridViewConectados.Size = new System.Drawing.Size(330, 505);
            this.DataGridViewConectados.TabIndex = 30;
            // 
            // BotonCrearSala
            // 
            this.BotonCrearSala.AutoSize = true;
            this.BotonCrearSala.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonCrearSala.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonCrearSala.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonCrearSala.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonCrearSala.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonCrearSala.Font = new System.Drawing.Font("Cooper Black", 28.24615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonCrearSala.Location = new System.Drawing.Point(650, 441);
            this.BotonCrearSala.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BotonCrearSala.Name = "BotonCrearSala";
            this.BotonCrearSala.Size = new System.Drawing.Size(205, 74);
            this.BotonCrearSala.TabIndex = 34;
            this.BotonCrearSala.Text = "Crear";
            this.BotonCrearSala.UseVisualStyleBackColor = true;
            this.BotonCrearSala.Click += new System.EventHandler(this.BotonCrearSala_Click);
            // 
            // LabelCrearSala
            // 
            this.LabelCrearSala.AutoSize = true;
            this.LabelCrearSala.BackColor = System.Drawing.Color.Transparent;
            this.LabelCrearSala.Font = new System.Drawing.Font("Cooper Black", 54.27692F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCrearSala.Location = new System.Drawing.Point(441, 298);
            this.LabelCrearSala.Name = "LabelCrearSala";
            this.LabelCrearSala.Size = new System.Drawing.Size(628, 125);
            this.LabelCrearSala.TabIndex = 32;
            this.LabelCrearSala.Text = "Crear Sala";
            // 
            // LabelTituloPrincipal
            // 
            this.LabelTituloPrincipal.AutoSize = true;
            this.LabelTituloPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.LabelTituloPrincipal.Font = new System.Drawing.Font("Cooper Black", 64.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTituloPrincipal.Location = new System.Drawing.Point(485, 59);
            this.LabelTituloPrincipal.Name = "LabelTituloPrincipal";
            this.LabelTituloPrincipal.Size = new System.Drawing.Size(906, 149);
            this.LabelTituloPrincipal.TabIndex = 35;
            this.LabelTituloPrincipal.Text = "PinturilloSO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1173, 441);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 32);
            this.label2.TabIndex = 36;
            this.label2.Text = "¿Cómo Jugar?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonOcultarContrasena
            // 
            this.BotonOcultarContrasena.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonOcultarContrasena.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BotonOcultarContrasena.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonOcultarContrasena.FlatAppearance.BorderSize = 0;
            this.BotonOcultarContrasena.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BotonOcultarContrasena.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BotonOcultarContrasena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BotonOcultarContrasena.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonOcultarContrasena.Location = new System.Drawing.Point(1197, 481);
            this.BotonOcultarContrasena.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BotonOcultarContrasena.Name = "BotonOcultarContrasena";
            this.BotonOcultarContrasena.Size = new System.Drawing.Size(172, 162);
            this.BotonOcultarContrasena.TabIndex = 37;
            this.BotonOcultarContrasena.UseVisualStyleBackColor = true;
            // 
            // ButtonFotoPerfil
            // 
            this.ButtonFotoPerfil.BackColor = System.Drawing.Color.Transparent;
            this.ButtonFotoPerfil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFotoPerfil.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ButtonFotoPerfil.FlatAppearance.BorderSize = 3;
            this.ButtonFotoPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFotoPerfil.Location = new System.Drawing.Point(24, 59);
            this.ButtonFotoPerfil.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonFotoPerfil.Name = "ButtonFotoPerfil";
            this.ButtonFotoPerfil.Size = new System.Drawing.Size(180, 168);
            this.ButtonFotoPerfil.TabIndex = 31;
            this.ButtonFotoPerfil.UseVisualStyleBackColor = false;
            this.ButtonFotoPerfil.Click += new System.EventHandler(this.ButtonFotoPerfil_Click);
            // 
            // ColumnaJugadores
            // 
            this.ColumnaJugadores.HeaderText = "Jugadores Conectados";
            this.ColumnaJugadores.MinimumWidth = 8;
            this.ColumnaJugadores.Name = "ColumnaJugadores";
            this.ColumnaJugadores.Width = 150;
            // 
            // MenuJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(1418, 899);
            this.Controls.Add(this.BotonOcultarContrasena);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LabelTituloPrincipal);
            this.Controls.Add(this.BotonCrearSala);
            this.Controls.Add(this.LabelCrearSala);
            this.Controls.Add(this.ButtonFotoPerfil);
            this.Controls.Add(this.DataGridViewConectados);
            this.Controls.Add(this.LabelNombreUsuario);
            this.Controls.Add(this.BotonAcceder);
            this.Controls.Add(this.TextCodigoSala);
            this.Controls.Add(this.LabelAccederSala);
            this.Controls.Add(this.LableNumeroConectados);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotonCerrarSesion);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MenuJugador";
            this.Text = "MenuJugador";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuJugador_FormClosing);
            this.Load += new System.EventHandler(this.MenuJugador_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LableNumeroConectados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BotonCerrarSesion;
        private System.Windows.Forms.Label LabelAccederSala;
        private System.Windows.Forms.TextBox TextCodigoSala;
        private System.Windows.Forms.Button BotonAcceder;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem informacionDelJugadorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StripMenuRanking;
        private System.Windows.Forms.ToolStripMenuItem StripMenuPartidasJugadas;
        private System.Windows.Forms.ToolStripMenuItem StripMenuAmigos;
        private System.Windows.Forms.Label LabelNombreUsuario;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView DataGridViewConectados;
        private System.Windows.Forms.Button BotonCrearSala;
        private System.Windows.Forms.Label LabelCrearSala;
        private System.Windows.Forms.Label LabelTituloPrincipal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BotonOcultarContrasena;
        private System.Windows.Forms.Button ButtonFotoPerfil;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnaJugadores;
    }
}