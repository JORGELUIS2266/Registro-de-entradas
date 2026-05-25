# REPORTE DE SEGURIDAD — SISTEMA DE ENTRADAS TECNOLÓGICO
**Fecha:** 18 de mayo de 2026

---

## 🔴 Problemas Críticos Encontrados

### 1. Contraseñas de Base de Datos en Texto Plano
**Archivo:** `Config.cs` — Líneas 23 y 27

**Problema:** Las contraseñas de conexión a la base de datos están hardcodeadas directamente en el código fuente. Las contraseñas `Password=TecnoLocal2026` (conexión red) y `Password=Tecno.GCP.2026` (conexión nube) son visibles para cualquier persona que acceda al código fuente, incluyendo el repositorio de GitHub.

**Riesgo:** Acceso no autorizado a la base de datos por parte de cualquier persona con acceso al repositorio.

---

### 2. Contraseñas de Usuarios sin Cifrado / Hash
**Archivo:** `BaseDeDatos.cs` — Líneas 369, 464, 512

**Problema:** Las contraseñas de los usuarios del sistema se almacenan y comparan en texto plano. El login compara directamente la contraseña sin encriptar, y al crear usuarios se guarda la contraseña tal cual en la base de datos.

**Riesgo:** Si alguien obtiene acceso a la base de datos, puede ver todas las contraseñas de todos los usuarios del sistema.

---

### 3. Connection String Expuesta en Mensajes de Error
**Archivo:** `BaseDeDatos.cs` — Línea 528

**Problema:** Cuando ocurre un error de base de datos, el sistema muestra al usuario el Connection String completo, incluyendo la IP del servidor, usuario y contraseña de la base de datos.

**Riesgo:** Exposición de credenciales sensibles a cualquier usuario de la aplicación al momento de un error.

---

## ✅ Lo Que Está Bien Implementado

- **Consultas SQL con parámetros (@param)** — Correcto en todo el código, sin riesgo de SQL Injection
- **Validación de columnas ORDER BY** — Implementada para evitar inyección por orden de columna
- **Roles de usuario (superusuario / usuario)** — Sistema de permisos implementado correctamente
- **Manejo de excepciones** — Try/catch en todas las operaciones críticas de la base de datos
- **Verificación de duplicados antes de insertar** — Implementado en usuarios y profesores

---

## 📋 Resumen de Prioridades de Corrección

| Prioridad | Problema | Archivo |
|-----------|----------|---------|
| 🔴 ALTA | Contraseñas de BD hardcodeadas | Config.cs |
| 🔴 ALTA | Contraseñas de usuarios sin hash | BaseDeDatos.cs |
| 🔴 ALTA | Connection String visible en errores | BaseDeDatos.cs L528 |

---

## 🛠️ Herramientas Recomendadas para Análisis Continuo

- **SonarLint** (plugin para Visual Studio) — análisis de vulnerabilidades en tiempo real
- **dotnet list package --vulnerable** — revisión de vulnerabilidades en paquetes NuGet
- **SQL Server Vulnerability Assessment** (integrado en SSMS) — análisis de la base de datos
