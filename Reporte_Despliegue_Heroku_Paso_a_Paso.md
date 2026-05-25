# Guía Paso a Paso: Migración de SQL Server a MySQL (Heroku JawsDB) para App de Escritorio en C#

Este documento detalla rigurosamente todos los pasos realizados para tomar un proyecto de Windows Forms (C# / .NET Framework 4.7.2) originalmente enlazado a Microsoft SQL Server, y migrar su base de datos a un clúster MySQL gratuito hospedado en Heroku usando JawsDB, incluyendo la resolución de problemas durante la inserción y filtrado de los registros.

---

## Fase 1: Creación de la Base de Datos MySQL en Heroku
Dado que Heroku no soporta Microsoft SQL Server ni aplicaciones gráficas de Windows de forma nativa, la estrategia fue montar el motor de datos en la nube (MySQL) y permitir que la app de escritorio (Vista) actúe como cliente externo.

1. **Creación de la App en Heroku:**
   - Inicio de sesión en el [Heroku Dashboard](https://dashboard.heroku.com/).
   - Clic en el botón **New** > **Create new app**.
   - Asignación de un nombre en minúsculas (ej. `base-datos-tec-1234`) y clic en **Create app**.
   
2. **Configuración del Motor JawsDB MySQL:**
   - Dentro del panel de la nueva aplicación, entrar a la pestaña **Resources**.
   - En la barra de búsqueda de **Add-ons**, buscar y seleccionar `JawsDB MySQL`.
   - Aparece un cuadro de confirmación, asegurar que esté seleccionado el plan **Kitefin Shared — Free** y hacer clic en **Submit Order Form** (Nota: Se requirió vincular una tarjeta bancaria sólo por cumplimiento de verificación de identidad, el servicio fue gratuito).

3. **Obtención de las Credenciales:**
   - Hacer clic en el enlace azul recién generado de `JawsDB MySQL` en la pestaña de Resources.
   - En el panel de JawsDB que se abre en una nueva pestaña, copiar la información de conexión mostrada en "**Connection String**" y "**Credentials**":
     - **Host:** (ej. `yhrz9vns005e0734.cbetxkdyhwsb.us-east-1.rds.amazonaws.com`)
     - **Username:** Alfanumérico provisto.
     - **Password:** Contraseña de alta entropía.
     - **Database / Schema:** Nombre alfanumérico generado (ej. `tth2xh9ih4455x1y`).
     - **Port:** `3306`.

---

## Fase 2: Configuración de Tablas y Base de Datos con MySQL Workbench
Se requiere configurar la estructura antes de que la app C# pueda ingresar datos.

1. **Establecer la conexión remota:**
   - Abrir el software **MySQL Workbench** a nivel local.
   - Hacer clic en el icono "**+**" junto a *MySQL Connections*.
   - Rellenar el **Hostname**, **Username**, y **Default Schema** basándose en las credenciales anteriores. Introducir la contraseña usando el botón "Store in Vault".
   - Probar y guardar la conexión temporal ("Heroku Nube"), y abrir el entorno SQL.

2. **Crear las Tablas Primarias (Migración del script .sql):**
   - Abrir el documento previo en SQL (`entradas_tecnologico.sql`) y ejecutar sus comandos `CREATE TABLE` usando el botón del "rayo".
   - Creación manual de la tabla `usuarios` de administradores (la cual faltaba en el script base original de MySQL) ejecutando:
     ```sql
     CREATE TABLE usuarios (
         usuario VARCHAR(50) NOT NULL PRIMARY KEY,
         contrasena VARCHAR(50) NOT NULL
     );
     INSERT INTO usuarios (usuario, contrasena) VALUES ('admin', 'admin123');
     ```

---

## Fase 3: Adaptación del Proyecto en Visual Studio (C#)
1. **Adición de la Librería Cliente:**
   - En Visual Studio Explorer, hacer clic derecho sobre el proyecto en la jerarquía general.
   - Seleccionar **Administrar paquetes NuGet**.
   - Buscar e instalar la librería **`MySql.Data`** (Recomendado v8.0.33 de Oracle). Esto adapta a .NET Framework para soportar la capa de datos.

2. **Configuración de la Cadena de Conexión:**
   - Abrir el archivo `Backend/Config.cs`.
   - Modificar la propiedad estática `ConnectionString` insertando las credenciales precisas del Host, Database, Username y Password obtenidas de JawsDB.

3. **Reescritura del CRUD (`BaseDeDatos.cs`):**
   - Reemplazar la importación `using System.Data.SqlClient;` por `using MySql.Data.MySqlClient;`.
   - Actualizar masivamente todos los adaptadores de la clase. Ejemplos de reemplazo:
     - `SqlConnection` -> `MySqlConnection`
     - `SqlCommand` -> `MySqlCommand`
     - `SqlDataAdapter` -> `MySqlDataAdapter`
   - **Mejora en las consultas:** En MySQL no es necesario habilitar `SET IDENTITY_INSERT` para autoincrementables, con lo cual se limpiaron esas instrucciones de las inserciones de `profesores`. Del mismo modo, comandos como `CAST(Numero_control_alumno AS NVARCHAR)` que SQL Server usa, debieron ajustarse a la sintaxis universal `CAST(Numero_control_alumno AS CHAR)`.

---

## Fase 4: Resolución de Error al Mostrar y Filtrar Datos (Truncamiento y Safe Mode)
### Detalle del problema
Al correr la aplicación y abrir la pantalla **"Lista Completa de Alumnos"**, la información se presentaba en blanco pese a que la base de datos *sí* tenía registros. 
> *Causa:* El diseño original de la tabla limitaba el campo `carrera` a `VARCHAR(20)`. Esto estaba **truncando / cortando** carreras largas antes de ser guardadas (ej. "INGENIERIA EN SISTEMAS COMPUTACIONALES" era recordado como "INGENIERIA EN SISTEM"). El panel de frontend en C# intentaba filtrar la tabla usando comparaciones exactas de string completos (`WHERE carrera = 'INGENIERIA EN SISTEMAS COMPUTACIONALES'`), por lo que nunca encontraba correspondencia en la BD.

### Flujo de solución en Workbench
1. **Desactivación del Modo Seguro de Actualización (Safe Mode Exception 1175):** 
   - Workbench bloqueó los Updates iniciales por buscar en nombres en lugar de Primary Keys. Se desactivó el filtro a nivel sesión mediante `SET SQL_SAFE_UPDATES = 0;`.
2. **Ampliación del Límite Estructural:**
   - La tabla se alteró en caliente: `ALTER TABLE alumnos MODIFY COLUMN carrera VARCHAR(60) NOT NULL;`
3. **Escaneo y Corrección de Datos Corruptos:**
   - Se ejecutó un batch de actualizaciones `UPDATE alumnos SET carrera = 'TITULO OFICIAL...' WHERE carrera LIKE '%fragmento_truncado%';` (Por ejemplo para Sistemas, Mecatrónica e Industrial).
4. **Reactivación General:** 
   - Se activó de nuevo el seguro: `SET SQL_SAFE_UPDATES = 1;` 

---

## Fase 5: Compilación Final y Generación de Distribución  
Para asegurar de que la versión que se comparte a los usuarios tenga las optimizaciones necesarias (excluyendo consolas de debug y depuradores):
1. **Settings:** En la cinta superior de las interfaces de Visual Studio, se cambió la directiva principal de modo `Debug` a modo `Release`.
2. **Reconstrucción:** A través de la barra de herramientas principal **Compilar (Build)** -> **Recomenzar solución / Compilar solución (Rebuild Solution)**. Múltiples librerías (`dll`) se compilaron.
3. **Validación:** El CLI MSBuild dictó Cero Errores y creó explícitamente el paquete empaquetado final:
   ```text
   proyecto -> C:\Users\User\Desktop\ENTRADAS-TECNOLOGICO-TERMINADO\proyecto\proyecto\bin\Release\proyecto.exe
   ```
4. **Entrega:** La carpeta `\bin\Release\` se validó de que contenga obligatoriamente tanto el `proyecto.exe` final, como la recién integrada librería `MySql.Data.dll` (Motor de conexión). La distribución de la carpeta completa permite ejecutar el sistema en cualquier PC y conectarse sin problemas hacia el clúster JawsDB remoto en AWS/DigitalOcean, reflejando el tráfico en todas las estaciones simuláneamente.
