using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace proyecto
{
    public partial class GestionUsuarios : Form
    {
        public GestionUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();

            // Iniciar temporizador para actualizar estado de conexión cada 5 segundos
            Timer timerRefresco = new Timer();
            timerRefresco.Interval = 5000; // 5 segundos
            timerRefresco.Tick += (s, e) => CargarUsuarios();
            timerRefresco.Start();

            // Configurar columnas básicas antes de cargar (opcional)
            dataGridUsuarios.AutoGenerateColumns = true;

            // Formatear visualmente la columna para incluir el estado
            dataGridUsuarios.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && e.RowIndex < dataGridUsuarios.Rows.Count)
                {
                    string colName = dataGridUsuarios.Columns[e.ColumnIndex].Name.ToLower();
                    
                    // Columna de Usuario o la de En Línea
                    if (colName == "usuario" || colName == "is_online")
                    {
                        object rawValue = dataGridUsuarios.Rows[e.RowIndex].Cells["is_online"].Value;
                        bool isOnline = false;
                        
                        if (rawValue != null && rawValue != DBNull.Value)
                        {
                            if (rawValue is bool b) isOnline = b;
                            else if (rawValue.ToString() == "1" || rawValue.ToString().ToLower() == "true") isOnline = true;
                        }

                        if (colName == "usuario")
                        {
                            // Mantener el nombre limpio sin emojis
                            string nombre = dataGridUsuarios.Rows[e.RowIndex].Cells["usuario"].Value?.ToString() ?? "";
                            nombre = nombre.Replace("🟢 ", "").Replace("🔴 ", "").Replace("⚪ ", ""); 
                            e.Value = nombre;
                            e.FormattingApplied = true;
                        }
                        else if (colName == "is_online")
                        {
                            if (isOnline)
                            {
                                e.Value = "✅ EN LINEA";
                                e.CellStyle.BackColor = Color.LimeGreen;
                                e.CellStyle.ForeColor = Color.White;
                                e.CellStyle.Font = new Font(dataGridUsuarios.Font, FontStyle.Bold);
                                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                            else
                            {
                                e.Value = ""; // No mostrar nada si está inactivo
                            }
                            e.FormattingApplied = true;
                        }
                    }
                }
            };

            // Manejar error de datos
            dataGridUsuarios.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void CargarUsuarios()
        {
            DataTable dt = BaseDeDatos.ObtenerUsuarios();
            dataGridUsuarios.DataSource = dt;

            if (dataGridUsuarios.Columns["is_online"] != null)
            {
                // Para eliminar el "cuadrito" (CheckBox) interno, debemos reemplazar la columna por una de texto
                int index = dataGridUsuarios.Columns["is_online"].Index;
                dataGridUsuarios.Columns.RemoveAt(index);
                
                DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                txtCol.Name = "is_online";
                txtCol.DataPropertyName = "is_online";
                txtCol.HeaderText = "Estado (En línea)";
                dataGridUsuarios.Columns.Insert(index, txtCol);
                
                dataGridUsuarios.Columns["is_online"].DisplayIndex = 2;
            }

            // Dar nombres amigables a las columnas
            if (dataGridUsuarios.Columns["id_usuario"] != null)
                dataGridUsuarios.Columns["id_usuario"].HeaderText = "ID";
            if (dataGridUsuarios.Columns["usuario"] != null)
                dataGridUsuarios.Columns["usuario"].HeaderText = "Usuario";
            if (dataGridUsuarios.Columns["rol"] != null)
                dataGridUsuarios.Columns["rol"].HeaderText = "Rol";
            if (dataGridUsuarios.Columns["nombre_completo"] != null)
                dataGridUsuarios.Columns["nombre_completo"].HeaderText = "Nombre Completo";
            if (dataGridUsuarios.Columns["activo"] != null)
                dataGridUsuarios.Columns["activo"].HeaderText = "Cuenta Habilitada";
            if (dataGridUsuarios.Columns["fecha_creacion"] != null)
                dataGridUsuarios.Columns["fecha_creacion"].HeaderText = "Fecha Registro";
            if (dataGridUsuarios.Columns["is_online"] != null)
            {
                dataGridUsuarios.Columns["is_online"].HeaderText = "Estado (En línea)";
                dataGridUsuarios.Columns["is_online"].DisplayIndex = 2; // Moverla al principio para que se vea
            }
            if (dataGridUsuarios.Columns["ultima_conexion"] != null)
                dataGridUsuarios.Columns["ultima_conexion"].HeaderText = "Última Conexión";
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;
            string confirmar = txtConfirmar.Text;
            string rol = cmbRol.SelectedItem?.ToString();
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena) || string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Usuario, contraseña y rol son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (contrasena != confirmar)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmar.Clear();
                txtConfirmar.Focus();
                return;
            }

            if (contrasena.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (BaseDeDatos.CrearUsuario(usuario, contrasena, rol, nombre))
            {
                MessageBox.Show($"✅ Usuario '{usuario}' creado como {rol}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarUsuarios();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario de la tabla para eliminar.", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string usuarioSeleccionado = dataGridUsuarios.SelectedRows[0].Cells["usuario"].Value?.ToString();
            int idSeleccionado = Convert.ToInt32(dataGridUsuarios.SelectedRows[0].Cells["id_usuario"].Value);

            // No permitir eliminar al propio usuario activo
            if (usuarioSeleccionado == Session.UsuarioActual)
            {
                MessageBox.Show("No puedes eliminar tu propia cuenta mientras estás conectado.", "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"¿Eliminar al usuario '{usuarioSeleccionado}'?\nEsta acción es permanente.", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (BaseDeDatos.EliminarUsuario(idSeleccionado))
                {
                    MessageBox.Show("✅ Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
            }
        }

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            if (dataGridUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario de la tabla.", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string usuarioSeleccionado = dataGridUsuarios.SelectedRows[0].Cells["usuario"].Value?.ToString();
            string nuevaContrasena = txtContrasena.Text;

            if (nuevaContrasena.Length < 6)
            {
                MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (BaseDeDatos.CambiarContrasena(usuarioSeleccionado, nuevaContrasena))
            {
                MessageBox.Show($"✅ Contraseña de '{usuarioSeleccionado}' actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
            }
        }

        private void LimpiarFormulario()
        {
            txtUsuario.Clear();
            txtContrasena.Clear();
            txtConfirmar.Clear();
            txtNombre.Clear();
            cmbRol.SelectedIndex = -1;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
