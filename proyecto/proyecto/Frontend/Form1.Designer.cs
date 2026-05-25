namespace proyecto
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Interfaz = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.INICIO = new System.Windows.Forms.Button();
            this.ESTUDIANTES = new System.Windows.Forms.Button();
            this.PROFESORES = new System.Windows.Forms.Button();
            this.btnGestionUsuarios = new System.Windows.Forms.Button();
            this.LOGOUT = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.Interfaz.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Interfaz
            // 
            this.Interfaz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Interfaz.Controls.Add(this.pictureBox2);
            this.Interfaz.Controls.Add(this.panel3);
            this.Interfaz.Dock = System.Windows.Forms.DockStyle.Top;
            this.Interfaz.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Interfaz.Location = new System.Drawing.Point(0, 0);
            this.Interfaz.Name = "Interfaz";
            this.Interfaz.Size = new System.Drawing.Size(800, 48);
            this.Interfaz.TabIndex = 0;
            this.Interfaz.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.Interfaz.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LemonChiffon;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 402);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(192)))));
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(386, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(414, 48);
            this.panel3.TabIndex = 0;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Beige;
            this.panel4.Controls.Add(this.LOGOUT);
            this.panel4.Controls.Add(this.btnGestionUsuarios);
            this.panel4.Controls.Add(this.PROFESORES);
            this.panel4.Controls.Add(this.ESTUDIANTES);
            this.panel4.Controls.Add(this.INICIO);
            this.panel4.Location = new System.Drawing.Point(0, 29);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 32);
            this.panel4.TabIndex = 0;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // INICIO
            // 
            this.INICIO.Font = new System.Drawing.Font("Barlow Condensed", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.INICIO.Location = new System.Drawing.Point(14, 4);
            this.INICIO.Name = "INICIO";
            this.INICIO.Size = new System.Drawing.Size(80, 24);
            this.INICIO.TabIndex = 1;
            this.INICIO.Text = "INICIO";
            this.INICIO.UseVisualStyleBackColor = true;
            this.INICIO.Click += new System.EventHandler(this.INICIO_Click);
            // 
            // ESTUDIANTES
            // 
            this.ESTUDIANTES.Font = new System.Drawing.Font("Barlow Condensed", 9.749999F, System.Drawing.FontStyle.Bold);
            this.ESTUDIANTES.Location = new System.Drawing.Point(100, 4);
            this.ESTUDIANTES.Name = "ESTUDIANTES";
            this.ESTUDIANTES.Size = new System.Drawing.Size(130, 24);
            this.ESTUDIANTES.TabIndex = 1;
            this.ESTUDIANTES.Text = "ESTUDIANTES";
            this.ESTUDIANTES.UseVisualStyleBackColor = true;
            this.ESTUDIANTES.Click += new System.EventHandler(this.ESTUDIANTES_Click);
            // 
            // PROFESORES
            // 
            this.PROFESORES.Font = new System.Drawing.Font("Barlow Condensed", 9.749999F, System.Drawing.FontStyle.Bold);
            this.PROFESORES.Location = new System.Drawing.Point(240, 5);
            this.PROFESORES.Name = "PROFESORES";
            this.PROFESORES.Size = new System.Drawing.Size(120, 24);
            this.PROFESORES.TabIndex = 1;
            this.PROFESORES.Text = "PROFESORES";
            this.PROFESORES.UseVisualStyleBackColor = true;
            this.PROFESORES.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnGestionUsuarios
            // 
            this.btnGestionUsuarios.Font = new System.Drawing.Font("Barlow Condensed", 9.749999F, System.Drawing.FontStyle.Bold);
            this.btnGestionUsuarios.Location = new System.Drawing.Point(372, 5);
            this.btnGestionUsuarios.Name = "btnGestionUsuarios";
            this.btnGestionUsuarios.Size = new System.Drawing.Size(130, 24);
            this.btnGestionUsuarios.TabIndex = 3;
            this.btnGestionUsuarios.Text = "👑 USUARIOS";
            this.btnGestionUsuarios.UseVisualStyleBackColor = true;
            this.btnGestionUsuarios.Visible = false;
            this.btnGestionUsuarios.Click += new System.EventHandler(this.btnGestionUsuarios_Click);
            // 
            // LOGOUT
            // 
            this.LOGOUT.Font = new System.Drawing.Font("Barlow Condensed", 9.749999F, System.Drawing.FontStyle.Bold);
            this.LOGOUT.Location = new System.Drawing.Point(690, 5);
            this.LOGOUT.Name = "LOGOUT";
            this.LOGOUT.Size = new System.Drawing.Size(94, 24);
            this.LOGOUT.TabIndex = 2;
            this.LOGOUT.Text = "CERRAR SESIÓN";
            this.LOGOUT.UseVisualStyleBackColor = true;
            this.LOGOUT.Click += new System.EventHandler(this.LOGOUT_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(250, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(192, 48);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox3
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Interfaz);
            this.Name = "Form1";
            this.Text = "Interfaz";
            this.Interfaz.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Interfaz;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button INICIO;
        private System.Windows.Forms.Button PROFESORES;
        private System.Windows.Forms.Button ESTUDIANTES;
        private System.Windows.Forms.Button LOGOUT;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnGestionUsuarios;
    }
}

