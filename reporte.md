# 📋 Reporte Técnico — Sistema de Entradas Tecnológico
## Migración: Azure → Local → AWS RDS

**Alumna:** Estela Valerio  
**Fecha:** 04 de Marzo de 2026  
**Proyecto:** Sistema de Control de Entradas — Tecnológico  
**Tecnologías:** C# · .NET Framework 4.7.2 · Windows Forms · SQL Server · AWS RDS

---

## 📌 Objetivo

Migrar la base de datos del sistema que originalmente estaba en **Microsoft Azure**, pasarla a una **base de datos local** (SQL Server Express) para pruebas, y finalmente desplegarla en **Amazon Web Services (AWS RDS)** de manera permanente y gratuita.

---

## 🔷 FASE 1 — Estado Inicial: Base de datos en Azure

El proyecto tenía la conexión hardcodeada a Azure SQL Server en dos archivos:

**[Backend/Config.cs](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/Backend/Config.cs)** — antes:
```csharp
// Conexión AZURE (ANTES)
public static string ConnectionString =
  "Server=tcp:servidortecno-sbj.database.windows.net,1433;
   Initial Catalog=entradas_tecnologico;
   User ID=adminuser; Password=Tecno.Azure.2024*;
   Encrypt=True; ...";
```

---

## 🔷 FASE 2 — Migración a Base de Datos Local

Se cambió la cadena de conexión en [Config.cs](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/Backend/Config.cs) y [App.config](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/App.config) para apuntar a **SQL Server Express local**:

```csharp
// Conexión LOCAL (FASE 2)
public static string ConnectionString =
  "Server=.\\SQLEXPRESS;
   Initial Catalog=entradas_tecnologico;
   Integrated Security=True; ...";
```

Se ejecutó el script [entradas_tecnologico_sqlserver.sql](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/entradas_tecnologico_sqlserver.sql) localmente, cargando:

| Tabla | Registros |
|---|---|
| `alumnos` | 14 |
| `profesores` | 5 |
| `usuarios` | 1 (admin/admin123) |

**Resultado:** El proyecto compiló y ejecutó correctamente de forma local conectándose a `PC4\SQLEXPRESS`.

---

## 🔷 FASE 3 — Despliegue en AWS RDS

### 3.1 — Panel de AWS RDS (sin bases de datos)

Se inició sesión en la consola de AWS con la cuenta de **Estela Valerio** (ID: 3924-2399-5009) en la región **US East (Ohio) — us-east-2**.

![Panel AWS RDS — sin bases de datos](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\databases_list_1772626174569.png)

---

### 3.2 — Formulario de Creación

Se hizo clic en **"Crear base de datos"**. La consola confirmó que la cuenta está en el **Plan Gratuito (Free Tier)**.

![Formulario de creación — Plan Gratuito activo](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\rds_create_db_form_loaded_1772631086721.png)

---

### 3.3 — Motor y Edición Seleccionados

Se configuró el motor como **Microsoft SQL Server** con la **Edición Express** (gratuita, límite 10 GB).

![Selección de edición SQL Server Express](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\rds_sql_server_edition_templates_1772631252300.png)

---

### 3.4 — Plantilla: Capa Gratuita + Credenciales

Se seleccionó la plantilla **"Capa gratuita"** y se llenaron las credenciales de acceso.

![Plantilla Capa gratuita y configuración de credenciales](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\rds_settings_section_1772631270005.png)

| Campo configurado | Valor |
|---|---|
| Identificador | `database-1entradas-tecnologico-db` |
| Usuario maestro | `adminuser` |
| Contraseña | `Tecno.AWS.2024` |
| Plantilla | ✅ Capa gratuita |
| Almacenamiento | 20 GiB SSD gp2 |

---

### 3.5 — Conectividad y Almacenamiento

Se configuró almacenamiento de **20 GiB SSD** y conectividad sin asociar a EC2 directamente.

![Conectividad y almacenamiento configurados](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\rds_connectivity_section_1772631354480.png)

---

### 3.6 — Base de Datos Iniciando Creación

Tras hacer clic en **"Crear base de datos"**, apareció la confirmación con estado **"Creando"**.

![Base de datos en estado Creando en AWS](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\rds_final_creation_button_v2_1772631460016.png)

---

### 3.7 — Base de Datos ✅ DISPONIBLE

Después de esperar aproximadamente 10 minutos, la base de datos cambió al estado **"Disponible"**.

![Base de datos Disponible en AWS RDS](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\aws_rds_db_available_1772636736039.png)

---

### 3.8 — Detalles de la Instancia en AWS

La instancia quedó configurada con las siguientes especificaciones confirmadas por la consola:

![Detalles de la instancia — SQL Server Express Edition, db.t3.micro, Disponible](C:\Users\User\.gemini\antigravity\brain\3639b263-ebbe-450e-aa8f-d673ff0140b3\aws_rds_db_details_1772637128478.png)

| Dato | Valor confirmado |
|---|---|
| **Identificador** | `database-1entradas-tecnologico-db` |
| **Estado** | ✅ Disponible |
| **Motor** | SQL Server Express Edition |
| **Clase** | db.t3.micro |
| **Región** | us-east-2b (Ohio) |

---

## 🔷 FASE 4 — Conexión y Carga de Datos en AWS

### 4.1 — Apertura del Puerto 1433

Se configuró el **Security Group** de AWS para abrir el puerto **1433** (SQL Server) al tráfico de entrada desde cualquier IP (`0.0.0.0/0`), permitiendo que la aplicación se conecte a la BD desde cualquier computadora.

### 4.2 — Prueba de Conexión Exitosa

Se verificó la conexión desde la consola local con `sqlcmd`:

```powershell
sqlcmd -S "database-1entradas-tecnologico-db.ctao62c4on29.us-east-2.rds.amazonaws.com,1433"
       -U adminuser -P "Tecno.AWS.2024" -N -C
       -Q "SELECT @@VERSION;"
```

**Resultado:** ✅ `SQL Server Express Edition (64-bit) on Windows Server Datacenter`

### 4.3 — Creación de la Base de Datos en AWS

```powershell
# Crear la base de datos
sqlcmd ... -Q "CREATE DATABASE entradas_tecnologico;"

# Ejecutar el script con tablas y datos
sqlcmd ... -d "entradas_tecnologico" -i "entradas_tecnologico_sqlserver.sql"
```

**Resultado del script:**
```
(1 rows affected)   ← tabla usuarios creada
(10 rows affected)  ← alumnos insertados
(3 rows affected)   ← profesores insertados
Base de datos entradas_tecnologico lista y actualizada.
```

### 4.4 — Verificación de Datos en AWS

```sql
SELECT COUNT(*) AS Alumnos    FROM alumnos;     -- 10
SELECT COUNT(*) AS Profesores FROM profesores;  -- 3
SELECT COUNT(*) AS Usuarios   FROM usuarios;    -- 1
```

| Tabla | Registros en AWS |
|---|---|
| `alumnos` | ✅ 10 |
| `profesores` | ✅ 3 |
| `usuarios` | ✅ 1 |

---

## 🔷 FASE 5 — Actualización del Proyecto y Compilación Final

### 5.1 — [Config.cs](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/Backend/Config.cs) actualizado a AWS

```csharp
// Conexión AWS RDS (FINAL)
public static string ConnectionString =
  "Server=database-1entradas-tecnologico-db.ctao62c4on29.us-east-2.rds.amazonaws.com,1433;
   Initial Catalog=entradas_tecnologico;
   User ID=adminuser; Password=Tecno.AWS.2024;
   Encrypt=True; TrustServerCertificate=True;
   Connection Timeout=30;";
```

### 5.2 — Compilación en modo Release

Se compiló el proyecto en modo **Release** con MSBuild de Visual Studio 2022:

```
✅ proyecto -> bin\Release\proyecto.exe
Exit code: 0 (sin errores)
```

### 5.3 — Ejecución Final

La aplicación se ejecutó exitosamente conectándose a **AWS RDS** en la nube de Amazon. Login disponible con: **admin / admin123**.

---

## 💰 Plan y Costos

> [!IMPORTANT]
> La base de datos en AWS es **COMPLETAMENTE GRATUITA** durante los primeros **12 meses** gracias al AWS Free Tier.

| Recurso | Especificación | Costo |
|---|---|---|
| Motor | SQL Server Express Edition | $0.00 |
| Instancia | db.t3.micro | $0.00 (Free Tier) |
| Almacenamiento | 20 GiB SSD gp2 | $0.00 (Free Tier) |
| Transferencia | 15 GB/mes saliente | $0.00 |
| **Total mensual** | | **$0.00 USD** |

*Después de 12 meses: aprox. $15-25 USD/mes*

---

## 📊 Resumen del Proceso Completo

```
AZURE (antes)
    ↓  Fase 1-2: Cambio de connection string
SQL SERVER LOCAL (PC4\SQLEXPRESS)
    ↓  Fase 3: Creación de AWS RDS
    ↓  Fase 4: Conexión + carga de datos via sqlcmd
    ↓  Fase 5: Actualización Config.cs + Compilación Release
AWS RDS SQL Server Express ✅ (FINAL)
    Endpoint: database-1entradas-tecnologico-db
              .ctao62c4on29.us-east-2.rds.amazonaws.com
    Puerto:   1433
    BD:       entradas_tecnologico
    Usuario:  adminuser
```

---

## 📁 Archivos Modificados

| Archivo | Cambio |
|---|---|
| [Backend/Config.cs](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/Backend/Config.cs) | Connection string: Azure → Local → **AWS RDS** |
| [App.config](file:///c:/Users/User/Music/ENTRADAS-TECNOLOGICO-TERMINADO/proyecto/proyecto/App.config) | Connection string actualizado |
| `bin/Release/proyecto.exe` | Ejecutable final listo para distribuir |

---

*Reporte generado — Sistema de Entradas Tecnológico · Marzo 2026*
