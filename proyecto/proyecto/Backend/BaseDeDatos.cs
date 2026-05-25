using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace proyecto
{
    public static class BaseDeDatos
    {
        // ==================== ALUMNOS ====================

        public static DataTable ObtenerAlumnos(string filtro = "")
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string query = "SELECT * FROM alumnos";
                    if (!string.IsNullOrWhiteSpace(filtro))
                        query += " WHERE Nombre LIKE @filtro OR carrera LIKE @filtro OR grupo LIKE @filtro OR CAST(Numero_control_alumno AS NVARCHAR) LIKE @filtro";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    if (!string.IsNullOrWhiteSpace(filtro))
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MostrarError("obtener alumnos", ex);
                return new DataTable();
            }
        }

        public static DataTable ObtenerAlumnosPorCarrera(string carrera)
        {
            return ObtenerAlumnosPorCarrera(carrera, "Nombre");
        }

        public static DataTable ObtenerAlumnosPorCarrera(string carrera, string ordenarPor)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    // Validar columna de orden para evitar SQL injection
                    string[] columnasValidas = { "Nombre", "Numero_control_alumno ASC", "semestre ASC, grupo ASC, Nombre ASC", "" };
                    bool esValido = false;
                    foreach (var col in columnasValidas)
                        if (col == ordenarPor) { esValido = true; break; }
                    if (!esValido) ordenarPor = "Nombre";

                    string query = "SELECT * FROM alumnos WHERE carrera = @carrera";
                    if (!string.IsNullOrWhiteSpace(ordenarPor))
                        query += " ORDER BY " + ordenarPor;
                    else
                        query += " ORDER BY Nombre";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@carrera", carrera);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MostrarError("obtener alumnos por carrera", ex);
                return new DataTable();
            }
        }

        // Alias de búsqueda global (usado por Lista completa)
        public static DataTable BuscarAlumnos(string filtro)
        {
            return ObtenerAlumnos(filtro);
        }

        public static DataRow ObtenerAlumnoPorControl(string numControl)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "SELECT * FROM alumnos WHERE Numero_control_alumno=@nc";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@nc", numControl);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0) return dt.Rows[0];
                    return null;
                }
            }
            catch (Exception ex)
            {
                MostrarError("buscar alumno", ex);
                return null;
            }
        }

        public static bool ExisteAlumno(string numControl)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "SELECT COUNT(*) FROM alumnos WHERE Numero_control_alumno=@nc";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@nc", numControl);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool InsertarAlumno(string numControl, string nombre, string apellido, string carrera, string correo, int semestre, string grupo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "INSERT INTO alumnos(Numero_control_alumno,Nombre,apellido,carrera,correo,semestre,grupo) VALUES(@nc,@nombre,@apellido,@carrera,@correo,@semestre,@grupo)";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@nc", numControl);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@carrera", carrera);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@semestre", semestre);
                    cmd.Parameters.AddWithValue("@grupo", grupo);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("insertar alumno", ex);
                return false;
            }
        }

        public static bool ActualizarAlumno(string numControl, string nombre, string apellido, string carrera, string correo, int semestre, string grupo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "UPDATE alumnos SET Nombre=@nombre,apellido=@apellido,carrera=@carrera,correo=@correo,semestre=@semestre,grupo=@grupo WHERE Numero_control_alumno=@nc";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@nc", numControl);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@carrera", carrera);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@semestre", semestre);
                    cmd.Parameters.AddWithValue("@grupo", grupo);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("actualizar alumno", ex);
                return false;
            }
        }

        public static bool EliminarAlumno(string numControl)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "DELETE FROM alumnos WHERE Numero_control_alumno=@nc";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@nc", numControl);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("eliminar alumno", ex);
                return false;
            }
        }

        // ==================== PROFESORES ====================

        public static DataTable ObtenerProfesores(string filtro = "")
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string query = "SELECT id_profesor, ced_profesional, nombre, apellido, telefono, correo, sexo FROM profesores";
                    if (!string.IsNullOrWhiteSpace(filtro))
                        query += " WHERE nombre LIKE @filtro OR correo LIKE @filtro OR CAST(id_profesor AS NVARCHAR) LIKE @filtro OR ced_profesional LIKE @filtro";
                    query += " ORDER BY id_profesor ASC";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    if (!string.IsNullOrWhiteSpace(filtro))
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MostrarError("obtener profesores", ex);
                return new DataTable();
            }
        }

        /// <summary>
        /// Obtiene el siguiente ID disponible para un nuevo profesor (max + 1).
        /// </summary>
        public static int ObtenerSiguienteIdProfesor()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(id_profesor), 0) + 1 FROM profesores", cn);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Verifica si un RFC ya está registrado en otro profesor.
        /// Si idExcluir > 0, se ignora ese profesor (útil al modificar).
        /// </summary>
        public static bool ExisteRFCProfesor(string rfc, int idExcluir = 0)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = idExcluir > 0
                        ? "SELECT COUNT(*) FROM profesores WHERE ced_profesional=@rfc AND id_profesor<>@id"
                        : "SELECT COUNT(*) FROM profesores WHERE ced_profesional=@rfc";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@rfc", rfc.Trim().ToUpper());
                    if (idExcluir > 0) cmd.Parameters.AddWithValue("@id", idExcluir);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch { return false; }
        }

        public static bool InsertarProfesor(int idProfesor, string rfc, string nombre, string apellido, string telefono, string correo, string sexo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "SET IDENTITY_INSERT profesores ON; " +
                        "INSERT INTO profesores(id_profesor,ced_profesional,nombre,apellido,telefono,correo,sexo) VALUES(@id,@ced,@nombre,@apellido,@tel,@correo,@sexo); " +
                        "SET IDENTITY_INSERT profesores OFF;";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@id", idProfesor);
                    cmd.Parameters.AddWithValue("@ced", rfc.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@tel", telefono);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@sexo", sexo);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("insertar profesor", ex);
                return false;
            }
        }

        public static bool ActualizarProfesor(int idProfesor, string rfc, string nombre, string apellido, string telefono, string correo, string sexo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "UPDATE profesores SET ced_profesional=@ced,nombre=@nombre,apellido=@apellido,telefono=@tel,correo=@correo,sexo=@sexo WHERE id_profesor=@id";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@id", idProfesor);
                    cmd.Parameters.AddWithValue("@ced", rfc.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@tel", telefono);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@sexo", sexo);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("actualizar profesor", ex);
                return false;
            }
        }

        public static bool EliminarProfesor(int idProfesor)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "DELETE FROM profesores WHERE id_profesor=@id";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@id", idProfesor);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("eliminar profesor", ex);
                return false;
            }
        }

        // ==================== LOGIN Y ROLES ====================

        /// <summary>
        /// Valida el login y devuelve el rol del usuario ('superusuario', 'usuario', o null si falla).
        /// </summary>
        public static string ValidarLoginYObtenerRol(string usuario, string contrasena)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "SELECT rol FROM usuarios WHERE usuario=@u AND contrasena=@c AND activo=1";

                    // --- INTENTO 1: Comparar con contraseña hasheada (nueva forma segura) ---
                    string contrasenaHash = HashPassword(contrasena);
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.Parameters.AddWithValue("@c", contrasenaHash);
                    object resultado = cmd.ExecuteScalar();

                    // --- INTENTO 2: Migración transparente (cuentas antiguas en texto plano) ---
                    if (resultado == null)
                    {
                        cmd.Parameters["@c"].Value = contrasena;
                        resultado = cmd.ExecuteScalar();

                        // Si el texto plano funciona, actualizar automáticamente al hash
                        if (resultado != null)
                        {
                            string migracionSql = "UPDATE usuarios SET contrasena=@hash WHERE usuario=@u2";
                            SqlCommand cmdMigracion = new SqlCommand(migracionSql, cn);
                            cmdMigracion.Parameters.AddWithValue("@hash", contrasenaHash);
                            cmdMigracion.Parameters.AddWithValue("@u2", usuario);
                            cmdMigracion.ExecuteNonQuery();
                        }
                    }

                    if (resultado != null)
                    {
                        // Marcar como conectado y actualizar última conexión
                        string updateSql = "UPDATE usuarios SET is_online = 1, ultima_conexion = GETDATE() WHERE usuario = @u";
                        SqlCommand cmdUpdate = new SqlCommand(updateSql, cn);
                        cmdUpdate.Parameters.AddWithValue("@u", usuario);
                        cmdUpdate.ExecuteNonQuery();

                        return resultado.ToString();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MostrarError("validar login", ex);
                return null;
            }
        }

        /// <summary>
        /// Marca a un usuario como desconectado.
        /// </summary>
        public static void CerrarSesion(string usuario)
        {
            if (string.IsNullOrEmpty(usuario)) return;
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "UPDATE usuarios SET is_online = 0 WHERE usuario = @u";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // No mostrar error aquí para no interrumpir el cierre de la app
                Console.WriteLine("Error al cerrar sesión: " + ex.Message);
            }
        }

        // Compatibilidad con el login anterior
        public static bool ValidarLogin(string usuario, string contrasena)
        {
            return ValidarLoginYObtenerRol(usuario, contrasena) != null;
        }

        // ==================== GESTIÓN DE USUARIOS (solo superusuario) ====================

        public static DataTable ObtenerUsuarios()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "SELECT id_usuario, usuario, rol, nombre_completo, activo, fecha_creacion, is_online, ultima_conexion FROM usuarios ORDER BY is_online DESC, rol DESC, usuario";
                    SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MostrarError("obtener usuarios", ex);
                return new DataTable();
            }
        }

        public static bool CrearUsuario(string usuario, string contrasena, string rol, string nombreCompleto)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    // Verificar si ya existe
                    SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM usuarios WHERE usuario=@u", cn);
                    chk.Parameters.AddWithValue("@u", usuario);
                    if ((int)chk.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Ya existe un usuario con ese nombre.", "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // Guardar contraseña como hash SHA-256 (nunca en texto plano)
                    string sql = "INSERT INTO usuarios(usuario,contrasena,rol,nombre_completo,activo) VALUES(@u,@c,@r,@n,1)";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.Parameters.AddWithValue("@c", HashPassword(contrasena));
                    cmd.Parameters.AddWithValue("@r", rol);
                    cmd.Parameters.AddWithValue("@n", nombreCompleto);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("crear usuario", ex);
                return false;
            }
        }

        public static bool EliminarUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    string sql = "DELETE FROM usuarios WHERE id_usuario=@id";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("eliminar usuario", ex);
                return false;
            }
        }

        public static bool CambiarContrasena(string usuario, string nuevaContrasena)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    cn.Open();
                    // Guardar siempre como hash SHA-256
                    string sql = "UPDATE usuarios SET contrasena=@c WHERE usuario=@u";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.Parameters.AddWithValue("@c", HashPassword(nuevaContrasena));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MostrarError("cambiar contraseña", ex);
                return false;
            }
        }

        // ==================== UTILIDAD ====================

        private static void MostrarError(string operacion, Exception ex)
        {
            // SEGURIDAD: No exponer el Connection String (contiene credenciales) al usuario
            string msg = "Error al " + operacion + ":\n\n" + ex.Message;
            MessageBox.Show(msg, "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Genera un hash SHA-256 de la contraseña con salt interno.
        /// NUNCA se almacena la contraseña en texto plano.
        /// </summary>
        private static string HashPassword(string password)
        {
            // Salt fijo de la aplicación (evita ataques de diccionario simples)
            const string appSalt = "EntradaTecNM_2026";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + appSalt));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
