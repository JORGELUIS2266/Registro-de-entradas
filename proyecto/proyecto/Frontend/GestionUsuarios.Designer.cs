namespace proyecto
{
    partial class GestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridUsuarios = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.lblContrasena = new System.Windows.Forms.Label();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.lblConfirmar = new System.Windows.Forms.Label();
            this.txtConfirmar = new System.Windows.Forms.TextBox();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCambiarContrasena = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblSesion = new System.Windows.Forms.Label();
            this.panelFormulario = new System.Windows.Forms.Panel();
            this.panelTitulo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsuarios)).BeginInit();
            this.panelFormulario.SuspendLayout();
            this.panelTitulo.SuspendLayout();
            this.SuspendLayout();

            // panelTitulo
            this.panelTitulo.BackColor = System.Drawing.Color.FromArgb(30, 30, 60);
            this.panelTitulo.Controls.Add(this.lblTitulo);
            this.panelTitulo.Controls.Add(this.lblSesion);
            this.panelTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitulo.Height = 55;

            // lblTitulo
            this.lblTitulo.Text = "👑 GESTIÓN DE USUARIOS";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 10);
            this.lblTitulo.AutoSize = true;

            // lblSesion
            this.lblSesion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSesion.ForeColor = System.Drawing.Color.FromArgb(180, 220, 255);
            this.lblSesion.Text = "Sesión: " + proyecto.Session.UsuarioActual + " (Superusuario)";
            this.lblSesion.Location = new System.Drawing.Point(14, 36);
            this.lblSesion.AutoSize = true;

            // panelFormulario
            this.panelFormulario.BackColor = System.Drawing.Color.FromArgb(245, 247, 255);
            this.panelFormulario.Location = new System.Drawing.Point(0, 55);
            this.panelFormulario.Size = new System.Drawing.Size(620, 220);
            this.panelFormulario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormulario.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblUsuario, this.txtUsuario,
                this.lblNombre, this.txtNombre,
                this.lblRol, this.cmbRol,
                this.lblContrasena, this.txtContrasena,
                this.lblConfirmar, this.txtConfirmar,
                this.btnCrear, this.btnEliminar, this.btnCambiarContrasena, this.btnCerrar
            });

            // lblUsuario
            this.lblUsuario.Text = "Usuario:";
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsuario.Location = new System.Drawing.Point(12, 18);
            this.lblUsuario.AutoSize = true;

            // txtUsuario
            this.txtUsuario.Location = new System.Drawing.Point(120, 15);
            this.txtUsuario.Size = new System.Drawing.Size(160, 22);
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);

            // lblNombre
            this.lblNombre.Text = "Nombre completo:";
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNombre.Location = new System.Drawing.Point(12, 48);
            this.lblNombre.AutoSize = true;

            // txtNombre
            this.txtNombre.Location = new System.Drawing.Point(120, 45);
            this.txtNombre.Size = new System.Drawing.Size(280, 22);
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9F);

            // lblRol
            this.lblRol.Text = "Rol:";
            this.lblRol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRol.Location = new System.Drawing.Point(12, 78);
            this.lblRol.AutoSize = true;

            // cmbRol
            this.cmbRol.Location = new System.Drawing.Point(120, 75);
            this.cmbRol.Size = new System.Drawing.Size(160, 22);
            this.cmbRol.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "usuario", "superusuario" });

            // lblContrasena
            this.lblContrasena.Text = "Contraseña:";
            this.lblContrasena.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblContrasena.Location = new System.Drawing.Point(12, 108);
            this.lblContrasena.AutoSize = true;

            // txtContrasena
            this.txtContrasena.Location = new System.Drawing.Point(120, 105);
            this.txtContrasena.Size = new System.Drawing.Size(160, 22);
            this.txtContrasena.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtContrasena.PasswordChar = '●';

            // lblConfirmar
            this.lblConfirmar.Text = "Confirmar:";
            this.lblConfirmar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblConfirmar.Location = new System.Drawing.Point(12, 138);
            this.lblConfirmar.AutoSize = true;

            // txtConfirmar
            this.txtConfirmar.Location = new System.Drawing.Point(120, 135);
            this.txtConfirmar.Size = new System.Drawing.Size(160, 22);
            this.txtConfirmar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtConfirmar.PasswordChar = '●';

            // btnCrear
            this.btnCrear.Text = "✅ Crear Usuario";
            this.btnCrear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCrear.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.btnCrear.ForeColor = System.Drawing.Color.White;
            this.btnCrear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrear.Location = new System.Drawing.Point(12, 175);
            this.btnCrear.Size = new System.Drawing.Size(140, 30);
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);

            // btnCambiarContrasena
            this.btnCambiarContrasena.Text = "🔑 Cambiar Contraseña";
            this.btnCambiarContrasena.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCambiarContrasena.BackColor = System.Drawing.Color.FromArgb(30, 100, 200);
            this.btnCambiarContrasena.ForeColor = System.Drawing.Color.White;
            this.btnCambiarContrasena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarContrasena.Location = new System.Drawing.Point(162, 175);
            this.btnCambiarContrasena.Size = new System.Drawing.Size(175, 30);
            this.btnCambiarContrasena.Click += new System.EventHandler(this.btnCambiarContrasena_Click);

            // btnEliminar
            this.btnEliminar.Text = "🗑 Eliminar Usuario";
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(180, 30, 30);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Location = new System.Drawing.Point(347, 175);
            this.btnEliminar.Size = new System.Drawing.Size(145, 30);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // btnCerrar
            this.btnCerrar.Text = "✖ Cerrar";
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Location = new System.Drawing.Point(502, 175);
            this.btnCerrar.Size = new System.Drawing.Size(100, 30);
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            // dataGridUsuarios
            this.dataGridUsuarios.Location = new System.Drawing.Point(0, 280);
            this.dataGridUsuarios.Size = new System.Drawing.Size(620, 220);
            this.dataGridUsuarios.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dataGridUsuarios.BackgroundColor = System.Drawing.Color.White;
            this.dataGridUsuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridUsuarios.ReadOnly = true;
            this.dataGridUsuarios.AllowUserToAddRows = false;
            this.dataGridUsuarios.RowHeadersVisible = false;
            this.dataGridUsuarios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dataGridUsuarios.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 30, 60);
            this.dataGridUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridUsuarios.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dataGridUsuarios.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 240, 255);

            // GestionUsuarios (Form)
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 510);
            this.Controls.Add(this.dataGridUsuarios);
            this.Controls.Add(this.panelFormulario);
            this.Controls.Add(this.panelTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GestionUsuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestión de Usuarios — Solo Superusuario";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsuarios)).EndInit();
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            this.panelTitulo.ResumeLayout(false);
            this.panelTitulo.PerformLayout();
            this.ResumeLayout(false);
        }

        // Controles
        private System.Windows.Forms.DataGridView dataGridUsuarios;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSesion;
        private System.Windows.Forms.Panel panelFormulario;
        private System.Windows.Forms.Panel panelTitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label lblContrasena;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.Label lblConfirmar;
        private System.Windows.Forms.TextBox txtConfirmar;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCambiarContrasena;
        private System.Windows.Forms.Button btnCerrar;
    }
}
