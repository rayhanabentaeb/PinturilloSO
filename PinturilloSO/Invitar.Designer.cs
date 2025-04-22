namespace PinturilloSO
{
    partial class Invitar
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
            this.label1 = new System.Windows.Forms.Label();
            this.DataGridViewConectados = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cooper Black", 15F);
            this.label1.Location = new System.Drawing.Point(36, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(728, 35);
            this.label1.TabIndex = 3;
            this.label1.Text = "Seleccione las personas a las que quiera invitar";
            // 
            // DataGridViewConectados
            // 
            this.DataGridViewConectados.AllowUserToAddRows = false;
            this.DataGridViewConectados.AllowUserToDeleteRows = false;
            this.DataGridViewConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            this.DataGridViewConectados.Location = new System.Drawing.Point(111, 104);
            this.DataGridViewConectados.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DataGridViewConectados.Name = "DataGridViewConectados";
            this.DataGridViewConectados.RowHeadersWidth = 62;
            this.DataGridViewConectados.RowTemplate.Height = 28;
            this.DataGridViewConectados.Size = new System.Drawing.Size(511, 304);
            this.DataGridViewConectados.TabIndex = 0;
            // 
            // ColumnaJugadores
            // 

            // 
            // Invitar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataGridViewConectados);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Invitar";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Invitar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridViewConectados;
        private System.Windows.Forms.Label label1;
        
       
    }
}