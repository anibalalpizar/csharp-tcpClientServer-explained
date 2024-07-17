namespace Client.UI.Mantenimientos
{
    partial class frmPelicula
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPelicula));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCategorias = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.txtIdPelicula = new System.Windows.Forms.TextBox();
            this.txtTituloPelicula = new System.Windows.Forms.TextBox();
            this.txtAnoLazamiento = new System.Windows.Forms.TextBox();
            this.txtIdioma = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.dgvPeliculas = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeliculas)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "id pelicula";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "titulo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Categoria pelicula";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Año lanzamiento";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Idioma";
            // 
            // cmbCategorias
            // 
            this.cmbCategorias.FormattingEnabled = true;
            this.cmbCategorias.Location = new System.Drawing.Point(177, 149);
            this.cmbCategorias.Name = "cmbCategorias";
            this.cmbCategorias.Size = new System.Drawing.Size(175, 21);
            this.cmbCategorias.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // txtIdPelicula
            // 
            this.txtIdPelicula.Location = new System.Drawing.Point(177, 45);
            this.txtIdPelicula.Name = "txtIdPelicula";
            this.txtIdPelicula.Size = new System.Drawing.Size(175, 20);
            this.txtIdPelicula.TabIndex = 7;
            // 
            // txtTituloPelicula
            // 
            this.txtTituloPelicula.Location = new System.Drawing.Point(177, 95);
            this.txtTituloPelicula.Name = "txtTituloPelicula";
            this.txtTituloPelicula.Size = new System.Drawing.Size(175, 20);
            this.txtTituloPelicula.TabIndex = 8;
            // 
            // txtAnoLazamiento
            // 
            this.txtAnoLazamiento.Location = new System.Drawing.Point(177, 192);
            this.txtAnoLazamiento.Name = "txtAnoLazamiento";
            this.txtAnoLazamiento.Size = new System.Drawing.Size(175, 20);
            this.txtAnoLazamiento.TabIndex = 9;
            // 
            // txtIdioma
            // 
            this.txtIdioma.Location = new System.Drawing.Point(177, 240);
            this.txtIdioma.Name = "txtIdioma";
            this.txtIdioma.Size = new System.Drawing.Size(175, 20);
            this.txtIdioma.TabIndex = 10;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(240, 292);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 11;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // dgvPeliculas
            // 
            this.dgvPeliculas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeliculas.Location = new System.Drawing.Point(399, 45);
            this.dgvPeliculas.Name = "dgvPeliculas";
            this.dgvPeliculas.Size = new System.Drawing.Size(374, 283);
            this.dgvPeliculas.TabIndex = 12;
            // 
            // frmPelicula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvPeliculas);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.txtIdioma);
            this.Controls.Add(this.txtAnoLazamiento);
            this.Controls.Add(this.txtTituloPelicula);
            this.Controls.Add(this.txtIdPelicula);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.cmbCategorias);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPelicula";
            this.Text = "frmPelicula";
            this.Load += new System.EventHandler(this.frmPelicula_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeliculas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbCategorias;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TextBox txtIdPelicula;
        private System.Windows.Forms.TextBox txtTituloPelicula;
        private System.Windows.Forms.TextBox txtAnoLazamiento;
        private System.Windows.Forms.TextBox txtIdioma;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.DataGridView dgvPeliculas;
    }
}