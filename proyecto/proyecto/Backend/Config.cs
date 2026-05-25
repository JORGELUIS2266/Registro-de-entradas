using System.Configuration;

namespace proyecto
{
    public static class Config
    {
        // =============================================
        // CAMBIA ESTE VALOR SEGÚN DÓNDE CORRES LA APP:
        //   "local"  = esta misma PC tiene SQL Server (el servidor)
        //   "red"    = cliente en otra PC, se conecta al servidor por red
        //   "nube"   = base de datos en Google Cloud
        // =============================================
        private static string Modo = "local";

        // ── Credenciales leídas desde App.config (NO hardcodeadas en código) ──
        private static string IPServidor =>
            ConfigurationManager.AppSettings["IPServidor"] ?? "10.88.87.231";

        private static string PasswordRed =>
            ConfigurationManager.AppSettings["PasswordRed"] ?? "";

        private static string PasswordNube =>
            ConfigurationManager.AppSettings["PasswordNube"] ?? "";

        // Conexión LOCAL — Forzando TCP/IP con TLS activo (Encrypt=True) para la prueba B de Wireshark.
        private static string ConnectionStringLocal =
            @"Server=tcp:localhost,1433;Initial Catalog=entradas_tecnologico;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=False;Connection Timeout=5;";

        // Conexión RED LOCAL — cliente se conecta al servidor por IP
        // SEGURIDAD: Encrypt=True activa cifrado TLS — el tráfico ya NO es legible con Wireshark.
        private static string ConnectionStringRed =>
            $"Server={IPServidor},1433;Initial Catalog=entradas_tecnologico;User ID=sa;Password={PasswordRed};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=10;";

        // Conexión NUBE — Google Cloud SQL
        private static string ConnectionStringNube =>
            $"Server=35.232.208.144,1433;Initial Catalog=entradas_tecnologico;User ID=sqlserver;Password={PasswordNube};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        // La app usa esta propiedad automáticamente
        public static string ConnectionString
        {
            get
            {
                switch (Modo)
                {
                    case "red":   return ConnectionStringRed;
                    case "nube":  return ConnectionStringNube;
                    default:      return ConnectionStringLocal;  // "local"
                }
            }
        }
    }
}
