namespace PinturilloSO
{
    partial class ComoJugar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComoJugar));
            this.LabelCrearSala1 = new System.Windows.Forms.Label();
            this.LabelComoJugar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelCrearSala1
            // 
            this.LabelCrearSala1.BackColor = System.Drawing.Color.Transparent;
            this.LabelCrearSala1.Font = new System.Drawing.Font("Courier New", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCrearSala1.Location = new System.Drawing.Point(12, 117);
            this.LabelCrearSala1.Name = "LabelCrearSala1";
            this.LabelCrearSala1.Size = new System.Drawing.Size(914, 615);
            this.LabelCrearSala1.TabIndex = 30;
            this.LabelCrearSala1.Text = resources.GetString("LabelCrearSala1.Text");
            this.LabelCrearSala1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelComoJugar
            // 
            this.LabelComoJugar.AutoSize = true;
            this.LabelComoJugar.BackColor = System.Drawing.Color.Transparent;
            this.LabelComoJugar.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelComoJugar.Location = new System.Drawing.Point(230, 17);
            this.LabelComoJugar.Name = "LabelComoJugar";
            this.LabelComoJugar.Size = new System.Drawing.Size(500, 75);
            this.LabelComoJugar.TabIndex = 29;
            this.LabelComoJugar.Text = "¿Cómo Jugar?";
            // 
            // ComoJugar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(954, 741);
            this.Controls.Add(this.LabelCrearSala1);
            this.Controls.Add(this.LabelComoJugar);
            this.Name = "ComoJugar";
            this.Text = "ComoJugar";
            this.Load += new System.EventHandler(this.ComoJugar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelCrearSala1;
        private System.Windows.Forms.Label LabelComoJugar;
    }
}