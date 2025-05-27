namespace PinturilloSO
{
    partial class RecuperarCuenta
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
            this.LabelTituloPrincipal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BotonEnviar = new System.Windows.Forms.Button();
            this.TextCambiarUsuario = new System.Windows.Forms.TextBox();
            this.BotonMenuPrincipal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelTituloPrincipal
            // 
            this.LabelTituloPrincipal.AutoSize = true;
            this.LabelTituloPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.LabelTituloPrincipal.Font = new System.Drawing.Font("Cooper Black", 29.90769F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTituloPrincipal.Location = new System.Drawing.Point(40, 55);
            this.LabelTituloPrincipal.Name = "LabelTituloPrincipal";
            this.LabelTituloPrincipal.Size = new System.Drawing.Size(1129, 62);
            this.LabelTituloPrincipal.TabIndex = 9;
            this.LabelTituloPrincipal.Text = "¿No recuerdas tu Usuario o Contraseña?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(146, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(898, 68);
            this.label1.TabIndex = 10;
            this.label1.Text = "Introduce tu dirección de correo electrónico y te enviaremos \r\nlos datos de tu cu" +
    "enta por e-mail.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BotonEnviar
            // 
            this.BotonEnviar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonEnviar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BotonEnviar.FlatAppearance.BorderColor = System.Drawing.Color.BurlyWood;
            this.BotonEnviar.FlatAppearance.BorderSize = 0;
            this.BotonEnviar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BotonEnviar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BotonEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BotonEnviar.Font = new System.Drawing.Font("Cooper Black", 19.93846F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonEnviar.Location = new System.Drawing.Point(911, 297);
            this.BotonEnviar.Name = "BotonEnviar";
            this.BotonEnviar.Size = new System.Drawing.Size(43, 41);
            this.BotonEnviar.TabIndex = 51;
            this.BotonEnviar.UseVisualStyleBackColor = true;
            this.BotonEnviar.Click += new System.EventHandler(this.BotonEnviar_Click);
            // 
            // TextCambiarUsuario
            // 
            this.TextCambiarUsuario.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextCambiarUsuario.Location = new System.Drawing.Point(278, 297);
            this.TextCambiarUsuario.Name = "TextCambiarUsuario";
            this.TextCambiarUsuario.Size = new System.Drawing.Size(627, 41);
            this.TextCambiarUsuario.TabIndex = 50;
            this.TextCambiarUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BotonMenuPrincipal
            // 
            this.BotonMenuPrincipal.AutoSize = true;
            this.BotonMenuPrincipal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BotonMenuPrincipal.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BotonMenuPrincipal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.BotonMenuPrincipal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BotonMenuPrincipal.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BotonMenuPrincipal.Font = new System.Drawing.Font("Cooper Black", 16.06154F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotonMenuPrincipal.Location = new System.Drawing.Point(435, 413);
            this.BotonMenuPrincipal.Name = "BotonMenuPrincipal";
            this.BotonMenuPrincipal.Size = new System.Drawing.Size(315, 44);
            this.BotonMenuPrincipal.TabIndex = 52;
            this.BotonMenuPrincipal.Text = "MENU PRINCIPAL";
            this.BotonMenuPrincipal.UseVisualStyleBackColor = true;
            this.BotonMenuPrincipal.Click += new System.EventHandler(this.BotonMenuPrincipal_Click);
            // 
            // RecuperarCuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(1199, 482);
            this.Controls.Add(this.BotonMenuPrincipal);
            this.Controls.Add(this.BotonEnviar);
            this.Controls.Add(this.TextCambiarUsuario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelTituloPrincipal);
            this.Name = "RecuperarCuenta";
            this.Text = "RecuperarCuenta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelTituloPrincipal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BotonEnviar;
        private System.Windows.Forms.TextBox TextCambiarUsuario;
        private System.Windows.Forms.Button BotonMenuPrincipal;
    }
}