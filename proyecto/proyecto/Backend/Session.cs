namespace proyecto
{
    /// <summary>
    /// Guarda la sesión activa del usuario durante toda la ejecución.
    /// </summary>
    public static class Session
    {
        public static string UsuarioActual { get; set; } = "";
        public static string NombreCompleto { get; set; } = "";
        public static string Rol { get; set; } = "";  // "superusuario" o "usuario"

        public static bool EsSuperusuario => Rol == "superusuario";

        public static void Limpiar()
        {
            UsuarioActual = "";
            NombreCompleto = "";
            Rol = "";
        }
    }
}
