# 📋 Reporte Técnico — Sistema de Entradas Tecnológico
## Migración y Despliegue en Google Cloud Platform (GCP)

**Alumna:** Estela Valerio  
**Fecha:** 06 de Marzo de 2026  
**Proyecto:** Sistema de Control de Entradas — Tecnológico  
**Tecnologías:** C# · .NET Framework 4.7.2 · Windows Forms · SQL Server · Google Cloud SQL

---

## 📌 Objetivo General

Migrar la base de datos del sistema local hacia la nube utilizando **Google Cloud SQL para SQL Server**, configurar las credenciales seguras de conexión en el código fuente, optimizar el diseño de la interfaz y generar un instalador empaquetado (`.zip`) listo para distribuir.

---

## ☁️ FASE 1 — Creación de la Base de Datos en Google Cloud (Paso a Paso)

A continuación se detalla el procedimiento exacto que se realizó en la consola de **Google Cloud Platform (GCP)** para levantar el servidor SQL Server en la nube.

### Paso 1.1: Crear la Instancia de SQL
1. Iniciar sesión en la consola de **Google Cloud Platform**.
2. En el menú de navegación, ir a **Bases de Datos > SQL**.
3. Hacer clic en el botón azul **"Crear instancia"**.
4. Seleccionar el motor de base de datos: **"Elegir SQL Server"**.
5. Se habilitó opcionalmente la API de Compute Engine si la plataforma lo requirió.

### Paso 1.2: Configuración de la Instancia
En el formulario de creación, se configuró lo siguiente:
- **ID de la instancia:** `entradas-tecnologico-db`
- **Contraseña predeterminada (usuario `sqlserver`):** `Tecno.GCP.2026`
- **Versión de la base de datos:** `SQL Server 2017 Express` (o 2019 Express, versión gratuita/ligera).
- **Región y Zona:** `us-central1 (Iowa)` o la región recomendada por GCP.
- **Configuración de máquina (Tipo de máquina):** Se eligió la opción **"Núcleo compartido"** (`db-f1-micro` o similar) para mantener el menor costo posible.
- **Almacenamiento:** Disco SSD de 10 GB.

Se hizo clic en **"CREAR INSTANCIA"** y se esperó aproximadamente 10 minutos a que el servidor estuviera en estado *Disponible*. El sistema asignó automáticamente la **IP Pública: `35.232.208.144`**.

### Paso 1.3: Creación de la Base de Datos Interna
Una vez que la instancia estuvo lista:
1. Dentro de los detalles de la instancia en GCP, se seleccionó el menú lateral **"Bases de datos"**.
2. Clic en **"Crear base de datos"**.
3. **Nombre de la base de datos:** `entradas_tecnologico`
4. Clic en **Crear**.

---

## 🛡️ FASE 2 — Configuración de Redes Autorizadas (Firewall de GCP)

Para que el programa en C# pudiera conectarse al servidor en la nube desde cualquier computadora sin restricciones, se tuvo que abrir el acceso en la red.

1. Dentro de los detalles de la instancia SQL, ir a la pestaña **Conexiones**.
2. En la sección **Redes, Red pública (IP Pública)**, asegurarse de que la casilla de IP pública esté activada.
3. Buscar la sección **Redes autorizadas** y dar clic en **Agregar Red**.
4. Llenar los datos:
   - **Nombre:** `Permitir-Todo` (o `Acceso_General`)
   - **Red:** `0.0.0.0/0` (Esto indica que se aceptan conexiones desde cualquier IP de Internet).
5. Hacer clic en **Guardar** para aplicar las políticas de firewall.

---

## 💾 FASE 3 — Carga de Tablas y Datos (Migración SQL)

Para poblar la base de datos en blanco en Google Cloud, se utilizó **SQL Server Management Studio (SSMS)** u otra herramienta de comandos como `sqlcmd` para ejecutar el script de nuestra base de datos original.

1. Nos conectamos a `35.232.208.144` con el usuario `sqlserver` y nuestra contraseña.
2. Seleccionamos la base recién creada `entradas_tecnologico`.
3. Ejecutamos el archivo `entradas_tecnologico_sqlserver.sql`, el cual generó:
   - Tabla `alumnos` (cargando los registros).
   - Tabla `profesores` (cargando los registros).
   - Tabla `usuarios` (creando al administrador).

---

## 💻 FASE 4 — Integración en Visual Studio (C#)

Con la base de datos corriendo en Google Cloud, se actualizó el código de la aplicación.

### Actualización del `Config.cs`
Se modificó la clase estática central para apuntar a la IP de GCP:

```csharp
namespace proyecto
{
    public static class Config
    {
        // Conexión segura a la nube de Google Cloud SQL
        public static string ConnectionString = 
            "Server=35.232.208.144,1433;" +
            "Initial Catalog=entradas_tecnologico;" +
            "User ID=sqlserver;" +
            "Password=Tecno.GCP.2026;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=True;" +
            "Connection Timeout=30;";
    }
}
```
*También se sincronizó el archivo `App.config` con esta misma cadena de conexión.*

---

## 🎨 FASE 5 — Corrección de Interfaces de Usuario (UI)

Se adaptó el diseño en el "Diseñador de Formularios" de Visual Studio, ya que el texto de los menús superiores ("ESTUDIANTES" y "PROFESORES") se encontraba recortado. 

- **Form1 (Inicio), Estudiantes, Profesores, Ingresar y Mantenimiento:**  
  Se reestructuraron las posiciones (`Location`) y se extendió el ancho (`Size.Width`) del botón Estudiantes a **130 píxeles** y de Profesores a **120 píxeles**.  
- Resultado: El texto es totalmente legible en todas las pantallas.

---

## 📦 FASE 6 — Compilación y Generación del Instalable

Para tener una versión rápida y portable:

1. Se seleccionó la configuración **"Release"** en Visual Studio.
2. Se construyó (Rebuild) el proyecto garantizando 0 errores.
3. Se creó un archivo comprimido llamado `SistemaEntradas-GCP.zip` que incluye la lógica de la aplicación y la conexión web encriptada. Este archivo **no requiere instalaciones complejas** ni tener un servidor SQL local funcionando, solo requiere conexión a Internet.

---

## ✅ Resumen del Perfil Final

| Parámetro | Estado Actual |
|---|---|
| **Estatus del Proyecto** | Terminado y Operativo |
| **Alojamiento BD** | Google Cloud Platform |
| **Credenciales de APP** | `admin` / `admin123` |
| **Disponibilidad** | Desde cualquier PC con WiFi/Ethernet |

*Reporte técnico generado con todos los pasos de configuración en Google Cloud — Sistema de Entradas (Marzo 2026)*
