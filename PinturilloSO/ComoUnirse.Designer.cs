namespace PinturilloSO
{
    partial class ComoUnirse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComoUnirse));
            this.label1 = new System.Windows.Forms.Label();
            this.LabelCrearSala1 = new System.Windows.Forms.Label();
            this.LabelCrearSala = new System.Windows.Forms.Label();
            this.LabelComoJugar = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(318, 606);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 50);
            this.label1.TabIndex = 32;
            this.label1.Text = "Código Sala";
            // 
            // LabelCrearSala1
            // 
            this.LabelCrearSala1.AutoSize = true;
            this.LabelCrearSala1.BackColor = System.Drawing.Color.Transparent;
            this.LabelCrearSala1.Font = new System.Drawing.Font("Courier New", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCrearSala1.Location = new System.Drawing.Point(67, 134);
            this.LabelCrearSala1.Name = "LabelCrearSala1";
            this.LabelCrearSala1.Size = new System.Drawing.Size(792, 500);
            this.LabelCrearSala1.TabIndex = 31;
            this.LabelCrearSala1.Text = resources.GetString("LabelCrearSala1.Text");
            this.LabelCrearSala1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCrearSala
            // 
            this.LabelCrearSala.AutoSize = true;
            this.LabelCrearSala.BackColor = System.Drawing.Color.Transparent;
            this.LabelCrearSala.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCrearSala.Location = new System.Drawing.Point(329, 84);
            this.LabelCrearSala.Name = "LabelCrearSala";
            this.LabelCrearSala.Size = new System.Drawing.Size(282, 50);
            this.LabelCrearSala.TabIndex = 30;
            this.LabelCrearSala.Text = "Crear Sala";
            // 
            // LabelComoJugar
            // 
            this.LabelComoJugar.AutoSize = true;
            this.LabelComoJugar.BackColor = System.Drawing.Color.Transparent;
            this.LabelComoJugar.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelComoJugar.Location = new System.Drawing.Point(208, 9);
            this.LabelComoJugar.Name = "LabelComoJugar";
            this.LabelComoJugar.Size = new System.Drawing.Size(539, 75);
            this.LabelComoJugar.TabIndex = 29;
            this.LabelComoJugar.Text = "¿Cómo Unirme?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Courier New", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(104, 656);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(714, 100);
            this.label2.TabIndex = 33;
            this.label2.Text = "Haz doble click en el código de una partida ya creada \r\nen la tabla de partidas y" +
    " únete, ¡así de fácil!\r\n\r\n\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComoUnirse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(932, 740);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelCrearSala1);
            this.Controls.Add(this.LabelCrearSala);
            this.Controls.Add(this.LabelComoJugar);
            this.Name = "ComoUnirse";
            this.Text = "ComoUnirse";
            this.Load += new System.EventHandler(this.ComoUnirse_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelCrearSala1;
        private System.Windows.Forms.Label LabelCrearSala;
        private System.Windows.Forms.Label LabelComoJugar;
        private System.Windows.Forms.Label label2;
    }
}