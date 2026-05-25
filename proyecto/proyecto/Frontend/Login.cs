using System;
using System.Windows.Forms;

namespace proyecto
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                lblMensaje.Text = "Por favor ingresa usuario y contraseña.";
                return;
            }

            string rol = BaseDeDatos.ValidarLoginYObtenerRol(txtUsuario.Text.Trim(), txtContrasena.Text);

            if (rol != null)
            {
                // Guardar la sesión activa
                Session.UsuarioActual = txtUsuario.Text.Trim();
                Session.Rol = rol;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
