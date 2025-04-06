﻿namespace PinturilloSO
{
    partial class MenuInicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelTituloPrincipal = new System.Windows.Forms.Label();
            this.LabelIniciarSesion = new System.Windows.Forms.Label();
            this.BotonCrearUsuario = new System.Windows.Forms.Button();
            this.LabelUsuario = new System.Windows.Forms.Label();
            this.LabelContraseña = new System.Windows.Forms.Label();
            this.TextUsuario = new System.Windows.Forms.TextBox();
            this.TextContrasena = new System.Windows.Forms.TextBox();
            this.BotonAceptar = new System.Windows.Forms.Button();
            this.BotonConectar = new System.Windows.Forms.Button();
            this.BotonDesconectar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LableNumeroConectados = new System.Windows.Forms.Label();
            this.LabelConexion = new System.Windows.Forms.Label();
            this.PanelImagenConexion = new System.Windows.Forms.Panel();
            this.BotonOcultarContrasena = new System.Windows.Forms.Button();
            this.BotonAyuda = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelTituloPrincipal
            // 
            this.LabelTituloPrincipal.AutoSize = true;
            this.LabelTituloPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.LabelTituloPrincipal.Font = new System.Drawing.Font("Cooper Black", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTituloPrincipal.Location = new System.Drawing.Point(213, 9);
            this.LabelTituloPrincipal.Name = "LabelTituloPrincipal";
            this.LabelTituloPrincipal.Size = new System.Drawing.Size(906, 149);
            this.LabelTituloPrincipal.TabIndex = 0;
            this.LabelTituloPrincipal.Text = "PinturilloSO";
            // 
            // LabelIniciarSesion
            // 
            this.LabelIniciarSesion.AutoSize = true;
            this.LabelIniciarSesion.BackColor = System.Drawing.Color.Transparent;
            this.LabelIniciarSesion.Font = new System.Drawing.Font("Cooper Black", 26.03077F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelIniciarSesion.Location = new System.Drawing.Point(468, 158);
            this.LabelIniciarSesion.Name = "LabelIniciarSesion";
            this.LabelIniciarSesion.Size = new System.Drawing.Size(351, 54);
            this.LabelIniciarSesion.TabIndex = 1;
            this.LabelIniciarSesion.Text = "Iniciar Sesión";
            // 
            // BotonCrearUsuario
            // 
            this.BotonCrearUsuario.AutoSize = true;
            this.BotonCrearUsuario.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonCrearUsuario.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonCrearUsuario.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonCrearUsuario.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonCrearUsuario.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonCrearUsuario.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonCrearUsuario.Location = new System.Drawing.Point(516, 613);
            this.BotonCrearUsuario.Name = "BotonCrearUsuario";
            this.BotonCrearUsuario.Size = new System.Drawing.Size(245, 44);
            this.BotonCrearUsuario.TabIndex = 2;
            this.BotonCrearUsuario.Text = "Crear Usuario";
            this.BotonCrearUsuario.UseVisualStyleBackColor = true;
            this.BotonCrearUsuario.Click += new System.EventHandler(this.BotonCrearUsuario_Click);
            // 
            // LabelUsuario
            // 
            this.LabelUsuario.AutoSize = true;
            this.LabelUsuario.BackColor = System.Drawing.Color.Transparent;
            this.LabelUsuario.Font = new System.Drawing.Font("Cooper Black", 18.27692F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelUsuario.Location = new System.Drawing.Point(557, 225);
            this.LabelUsuario.Name = "LabelUsuario";
            this.LabelUsuario.Size = new System.Drawing.Size(150, 38);
            this.LabelUsuario.TabIndex = 3;
            this.LabelUsuario.Text = "Usuario";
            // 
            // LabelContraseña
            // 
            this.LabelContraseña.AutoSize = true;
            this.LabelContraseña.BackColor = System.Drawing.Color.Transparent;
            this.LabelContraseña.Font = new System.Drawing.Font("Cooper Black", 18.27692F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelContraseña.Location = new System.Drawing.Point(527, 340);
            this.LabelContraseña.Name = "LabelContraseña";
            this.LabelContraseña.Size = new System.Drawing.Size(210, 38);
            this.LabelContraseña.TabIndex = 4;
            this.LabelContraseña.Text = "Contraseña";
            // 
            // TextUsuario
            // 
            this.TextUsuario.Font = new System.Drawing.Font("Comic Sans MS", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextUsuario.Location = new System.Drawing.Point(444, 276);
            this.TextUsuario.Name = "TextUsuario";
            this.TextUsuario.Size = new System.Drawing.Size(375, 38);
            this.TextUsuario.TabIndex = 5;
            this.TextUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextContrasena
            // 
            this.TextContrasena.Font = new System.Drawing.Font("Comic Sans MS", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextContrasena.Location = new System.Drawing.Point(444, 391);
            this.TextContrasena.Name = "TextContrasena";
            this.TextContrasena.Size = new System.Drawing.Size(375, 38);
            this.TextContrasena.TabIndex = 6;
            this.TextContrasena.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextContrasena.TextChanged += new System.EventHandler(this.TextContrasena_TextChanged);
            // 
            // BotonAceptar
            // 
            this.BotonAceptar.AutoSize = true;
            this.BotonAceptar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonAceptar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonAceptar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonAceptar.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonAceptar.Location = new System.Drawing.Point(544, 474);
            this.BotonAceptar.Name = "BotonAceptar";
            this.BotonAceptar.Size = new System.Drawing.Size(180, 50);
            this.BotonAceptar.TabIndex = 7;
            this.BotonAceptar.Text = "Aceptar";
            this.BotonAceptar.UseVisualStyleBackColor = true;
            this.BotonAceptar.Click += new System.EventHandler(this.BotonAceptar_Click);
            // 
            // BotonConectar
            // 
            this.BotonConectar.AutoSize = true;
            this.BotonConectar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonConectar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonConectar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonConectar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonConectar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonConectar.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonConectar.Location = new System.Drawing.Point(12, 578);
            this.BotonConectar.Name = "BotonConectar";
            this.BotonConectar.Size = new System.Drawing.Size(211, 44);
            this.BotonConectar.TabIndex = 8;
            this.BotonConectar.Text = "CONECTAR";
            this.BotonConectar.UseVisualStyleBackColor = true;
            this.BotonConectar.Click += new System.EventHandler(this.BotonConectar_Click);
            // 
            // BotonDesconectar
            // 
            this.BotonDesconectar.AutoSize = true;
            this.BotonDesconectar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonDesconectar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonDesconectar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonDesconectar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonDesconectar.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonDesconectar.Location = new System.Drawing.Point(12, 628);
            this.BotonDesconectar.Name = "BotonDesconectar";
            this.BotonDesconectar.Size = new System.Drawing.Size(275, 44);
            this.BotonDesconectar.TabIndex = 9;
            this.BotonDesconectar.Text = "DESCONECTAR";
            this.BotonDesconectar.UseVisualStyleBackColor = true;
            this.BotonDesconectar.Click += new System.EventHandler(this.BotonDesconectar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 13.84615F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1075, 524);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 58);
            this.label1.TabIndex = 10;
            this.label1.Text = "Jugadores\r\nConectados";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LableNumeroConectados
            // 
            this.LableNumeroConectados.BackColor = System.Drawing.SystemColors.HighlightText;
            this.LableNumeroConectados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LableNumeroConectados.Font = new System.Drawing.Font("Cooper Black", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LableNumeroConectados.Location = new System.Drawing.Point(1066, 589);
            this.LableNumeroConectados.Name = "LableNumeroConectados";
            this.LableNumeroConectados.Size = new System.Drawing.Size(167, 83);
            this.LableNumeroConectados.TabIndex = 11;
            this.LableNumeroConectados.Text = "0";
            this.LableNumeroConectados.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LabelConexion
            // 
            this.LabelConexion.AutoSize = true;
            this.LabelConexion.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelConexion.Location = new System.Drawing.Point(12, 535);
            this.LabelConexion.Name = "LabelConexion";
            this.LabelConexion.Size = new System.Drawing.Size(224, 35);
            this.LabelConexion.TabIndex = 12;
            this.LabelConexion.Text = "Desconectado";
            this.LabelConexion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelImagenConexion
            // 
            this.PanelImagenConexion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelImagenConexion.Location = new System.Drawing.Point(12, 12);
            this.PanelImagenConexion.Name = "PanelImagenConexion";
            this.PanelImagenConexion.Size = new System.Drawing.Size(71, 62);
            this.PanelImagenConexion.TabIndex = 14;
            // 
            // BotonOcultarContrasena
            // 
            this.BotonOcultarContrasena.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonOcultarContrasena.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonOcultarContrasena.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonOcultarContrasena.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonOcultarContrasena.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BotonOcultarContrasena.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonOcultarContrasena.Location = new System.Drawing.Point(825, 391);
            this.BotonOcultarContrasena.Name = "BotonOcultarContrasena";
            this.BotonOcultarContrasena.Size = new System.Drawing.Size(52, 38);
            this.BotonOcultarContrasena.TabIndex = 15;
            this.BotonOcultarContrasena.UseVisualStyleBackColor = true;
            this.BotonOcultarContrasena.Click += new System.EventHandler(this.BotonOcultarContrasena_Click);
            // 
            // BotonAyuda
            // 
            this.BotonAyuda.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonAyuda.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonAyuda.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonAyuda.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BotonAyuda.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonAyuda.Location = new System.Drawing.Point(730, 481);
            this.BotonAyuda.Name = "BotonAyuda";
            this.BotonAyuda.Size = new System.Drawing.Size(37, 37);
            this.BotonAyuda.TabIndex = 16;
            this.BotonAyuda.UseVisualStyleBackColor = true;
            // 
            // MenuInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1245, 686);
            this.Controls.Add(this.BotonAyuda);
            this.Controls.Add(this.BotonOcultarContrasena);
            this.Controls.Add(this.PanelImagenConexion);
            this.Controls.Add(this.LabelConexion);
            this.Controls.Add(this.LableNumeroConectados);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotonDesconectar);
            this.Controls.Add(this.BotonConectar);
            this.Controls.Add(this.BotonAceptar);
            this.Controls.Add(this.TextContrasena);
            this.Controls.Add(this.TextUsuario);
            this.Controls.Add(this.LabelContraseña);
            this.Controls.Add(this.LabelUsuario);
            this.Controls.Add(this.BotonCrearUsuario);
            this.Controls.Add(this.LabelIniciarSesion);
            this.Controls.Add(this.LabelTituloPrincipal);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "MenuInicio";
            this.Text = "MenuInicio";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelTituloPrincipal;
        private System.Windows.Forms.Label LabelIniciarSesion;
        private System.Windows.Forms.Button BotonCrearUsuario;
        private System.Windows.Forms.Label LabelUsuario;
        private System.Windows.Forms.Label LabelContraseña;
        private System.Windows.Forms.TextBox TextUsuario;
        private System.Windows.Forms.TextBox TextContrasena;
        private System.Windows.Forms.Button BotonAceptar;
        private System.Windows.Forms.Button BotonConectar;
        private System.Windows.Forms.Button BotonDesconectar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LableNumeroConectados;
        private System.Windows.Forms.Label LabelConexion;
        private System.Windows.Forms.Panel PanelImagenConexion;
        private System.Windows.Forms.Button BotonOcultarContrasena;
        private System.Windows.Forms.Button BotonAyuda;
    }
}

