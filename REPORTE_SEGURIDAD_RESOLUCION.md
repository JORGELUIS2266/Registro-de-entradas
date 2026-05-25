# REPORTE DE IMPLEMENTACIÓN DE SEGURIDAD — SISTEMA DE ENTRADAS TECNOLÓGICO
**Fecha de Revisión:** 20 de mayo de 2026

---

## 🟢 Resumen Ejecutivo
Se han verificado y validado las correcciones de seguridad aplicadas al código fuente del sistema. Todas las vulnerabilidades críticas reportadas anteriormente (contraseñas en código, almacenamiento en texto plano y fuga de información en excepciones) han sido **mitigadas exitosamente**.

---

## ✅ Vulnerabilidades Resueltas

### 1. Extracción de Contraseñas de Base de Datos
**Estado:** Resuelto 🟢
**Archivo modificado:** `Config.cs`

**Implementación:**
- Se eliminaron completamente las contraseñas "hardcodeadas" del código fuente.
- Se implementó el uso de `ConfigurationManager.AppSettings` para recuperar dinámicamente los valores `IPServidor`, `PasswordRed` y `PasswordNube` en tiempo de ejecución (presumiblemente desde `App.config`).
- **Beneficio de Seguridad:** El código base puede ser publicado o compartido en repositorios (como GitHub) sin exponer el acceso a las bases de datos de producción o red local.

---

### 2. Cifrado (Hashing) Seguro de Contraseñas de Usuarios
**Estado:** Resuelto 🟢
**Archivo modificado:** `BaseDeDatos.cs`

**Implementación:**
- Se integró el algoritmo criptográfico **SHA-256** junto con un valor "Salt" estático (`EntradaTecNM_2026`) a través de la función `HashPassword`.
- Los flujos de `CrearUsuario` y `CambiarContrasena` ahora garantizan un guardado cifrado e irreversible en la base de datos.
- **Migración Transparente (Login):** El método `ValidarLoginYObtenerRol` incluye lógica de actualización automática. Si detecta una cuenta antigua (con contraseña en texto plano), permite el acceso e inmediatamente la convierte a Hash SHA-256 en la base de datos de manera silenciosa para el usuario.
- **Beneficio de Seguridad:** Si la base de datos sufriera alguna filtración, las credenciales reales de los usuarios estarían protegidas e ilegibles.

---

### 3. Mitigación de Fuga de Información en Errores (Connection String)
**Estado:** Resuelto 🟢
**Archivo modificado:** `BaseDeDatos.cs` (Método global de manejo de excepciones)

**Implementación:**
- Se refactorizó el método utilitario `MostrarError`.
- La información presentada en las ventanas de diálogo (MessageBox) se limitó a `ex.Message`, evitando hacer volcados de memoria o impresión de propiedades de conexión completas (las cuales incluían IP y password del servidor de SQL).
- Se agregó el comentario de control: `// SEGURIDAD: No exponer el Connection String (contiene credenciales) al usuario`.
- **Beneficio de Seguridad:** Un error fortuito o un intento malicioso de causar una excepción ya no revelará las topologías de red ni las credenciales de la infraestructura.

---

## 🚀 Conclusión
El aplicativo C# cuenta ahora con un diseño de persistencia y manejo de credenciales robusto. Se encuentra preparado para su despliegue tanto en entornos locales (`local`), de red interna (`red`) o en la nube (`nube`) de manera segura.

### Recomendaciones Adicionales
1. **Encriptar Archivos de Configuración:** En los equipos de despliegue final, asegurar que la sección `appSettings` del `App.config` donde residen las contraseñas ahora extraídas, tenga permisos restrictivos a nivel sistema de archivos (NTFS) o usar protección de configuración de .NET (aspnet_regiis).
2. **Rotación de Contraseñas Reales:** Tras la entrada en vigor de los "Hashes", solicitar a los superusuarios que usaban contraseñas genéricas realizar un cambio voluntario para fortalecer la seguridad de las cuentas de administración.
