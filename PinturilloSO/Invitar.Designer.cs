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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.DataGridViewConectados = new System.Windows.Forms.DataGridView();
            this.JugadoresConectados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Cooper Black", 22.15385F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(679, 105);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selecciona jugadores para \r\ninvitarles a la partida";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataGridViewConectados
            // 
            this.DataGridViewConectados.AllowUserToAddRows = false;
            this.DataGridViewConectados.AllowUserToDeleteRows = false;
            this.DataGridViewConectados.AllowUserToResizeColumns = false;
            this.DataGridViewConectados.AllowUserToResizeRows = false;
            this.DataGridViewConectados.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DataGridViewConectados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewConectados.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridViewConectados.BackgroundColor = System.Drawing.Color.White;
            this.DataGridViewConectados.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridViewConectados.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 13.84615F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewConectados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewConectados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewConectados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JugadoresConectados});
            this.DataGridViewConectados.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewConectados.DefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridViewConectados.EnableHeadersVisualStyles = false;
            this.DataGridViewConectados.GridColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewConectados.Location = new System.Drawing.Point(12, 117);
            this.DataGridViewConectados.Name = "DataGridViewConectados";
            this.DataGridViewConectados.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewConectados.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewConectados.RowHeadersVisible = false;
            this.DataGridViewConectados.RowHeadersWidth = 56;
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewConectados.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.DataGridViewConectados.RowTemplate.Height = 36;
            this.DataGridViewConectados.RowTemplate.ReadOnly = true;
            this.DataGridViewConectados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataGridViewConectados.Size = new System.Drawing.Size(725, 349);
            this.DataGridViewConectados.TabIndex = 31;
            this.DataGridViewConectados.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewConectados_CellDoubleClick);
            this.DataGridViewConectados.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewConectados_CellMouseMove);
            // 
            // JugadoresConectados
            // 
            this.JugadoresConectados.HeaderText = "Jugadores Conectados";
            this.JugadoresConectados.MinimumWidth = 7;
            this.JugadoresConectados.Name = "JugadoresConectados";
            this.JugadoresConectados.ReadOnly = true;
            // 
            // Invitar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(749, 478);
            this.Controls.Add(this.DataGridViewConectados);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Invitar";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Invitar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConectados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DataGridViewConectados;
        private System.Windows.Forms.DataGridViewTextBoxColumn JugadoresConectados;
    }
}