# REPORTE DE IMPLEMENTACIÓN DE RESPALDOS AUTOMÁTICOS
## Sistema: proyecto.exe — Entradas Tecnológico
**Fecha de implementación:** 22 de mayo de 2026  
**Responsable:** Edgar Mauricio Sarmiento Ruiz  
**Institución:** TecNM  

---

## 1. OBJETIVO
Garantizar la integridad, disponibilidad y seguridad de la información del sistema de accesos automatizando una copia de seguridad (backup) completa de la base de datos `entradas_tecnologico` de forma diaria a las **2:00 AM**, mitigando el riesgo de pérdida de datos por fallas de hardware, software o errores humanos.

---

## 2. LIMITACIÓN TÉCNICA Y SOLUCIÓN
La base de datos del sistema está implementada sobre **SQL Server Express** (`.\SQLEXPRESS`). 

> [!WARNING]
> La edición **Express** de SQL Server no incluye ni permite ejecutar el servicio **SQL Server Agent**, que es la herramienta nativa corporativa de Microsoft para programar y automatizar tareas y respaldos.

### Solución Implementada:
Para superar esta limitación técnica sin incurrir en costos de licenciamiento, se implementó una solución híbrida:
1. **Script de Automatización (`respaldar_db.bat`):** Un archivo por lotes de Windows que se conecta de manera segura por línea de comandos (`sqlcmd`) a SQL Server, ejecuta el comando de respaldo y nombra el archivo resultante con la fecha y hora exacta de su creación.
2. **Programador de Tareas de Windows (Task Scheduler):** La herramienta del sistema operativo para ejecutar el script diariamente a las 2:00 AM con privilegios elevados de administrador.

---

## 3. PASO A PASO DE LA CONFIGURACIÓN

### Paso 3.1: Creación del Script de Respaldo (`respaldar_db.bat`)
Se ha guardado el script automatizado en la raíz del proyecto. Este script realiza las siguientes acciones:
1. Crea la carpeta de destino `C:\Backups_Entradas_Tecnologico` si no existe.
2. Extrae la marca de tiempo exacta usando PowerShell de forma consistente.
3. Ejecuta la consulta de respaldo completo de la base de datos `entradas_tecnologico` usando `sqlcmd` con seguridad integrada (`-E`).
4. Genera un archivo `.bak` único para evitar la sobreescritura de respaldos anteriores.

*Código del Script:*
```batch
@echo off
set BACKUP_DIR=C:\Backups_Entradas_Tecnologico
if not exist "%BACKUP_DIR%" (
    mkdir "%BACKUP_DIR%"
)
for /f "usebackq tokens=*" %%i in (`powershell -NoProfile -Command "Get-Date -Format 'yyyyMMdd_HHmmss'"`) do set TIMESTAMP=%%i
set BACKUP_FILE=%BACKUP_DIR%\entradas_tecnologico_%TIMESTAMP%.bak

sqlcmd -S .\SQLEXPRESS -E -Q "BACKUP DATABASE [entradas_tecnologico] TO DISK = N'%BACKUP_FILE%' WITH NOFORMAT, INIT, NAME = N'Respaldo Diario Autoprogramado', SKIP, NOREWIND, NOUNLOAD, STATS = 10"

if %ERRORLEVEL% EQU 0 (
    echo [OK] Respaldo generado exitosamente.
) else (
    echo [ERROR] Ocurrió un error al intentar crear el respaldo.
    exit /b %ERRORLEVEL%
)
```

---

### Paso 3.2: Programación en el Programador de Tareas de Windows

Sigue estos pasos detallados para programar la ejecución a las **2:00 AM**:

1. Abre el menú Inicio de Windows, escribe **Programador de Tareas** (Task Scheduler) y ejecútalo como Administrador.
2. En el panel derecho (*Acciones*), haz clic en **Crear Tarea...** (Create Task).
3. **Pestaña "General":**
   * **Nombre:** `Respaldo Diario Entradas Tecnologico`
   * **Descripción:** `Ejecuta el script para respaldar la base de datos entradas_tecnologico diariamente a las 2:00 AM.`
   * Selecciona la opción **Ejecutar solo cuando el usuario haya iniciado sesión** (si tu cuenta de Windows no tiene contraseña) o **Ejecutar tanto si el usuario inició sesión como si no** (si tu cuenta tiene contraseña configurada).

   * Marca la casilla **Ejecutar con los privilegios más elevados** (Run with highest privileges) — *Esto es fundamental para que el script tenga permisos de escribir en el disco C:\ y acceder a SQL Server.*
   * **Configurar para:** Selecciona **Windows 10** (esta opción es la correcta y totalmente compatible para sistemas con Windows 11, ya que comparten el mismo motor del Programador de Tareas).

📸 **[INSERTAR AQUÍ: Captura de pantalla de la pestaña General con el nombre de la tarea y la casilla de privilegios elevados marcada]**

4. **Pestaña "Desencadenadores" (Triggers):**
   * Haz clic en **Nuevo...** (New...).
   * **Comenzar la tarea:** *Según la programación* (On a schedule).
   * **Programación:** Selecciona **Diariamente** (Daily).
   * **Hora de inicio:** Configura a las **02:00:00 a.m.**
   * Asegúrate de que la casilla **Habilitado** (Enabled) al final de la ventana esté marcada.
   * Haz clic en *Aceptar*.

📸 **[INSERTAR AQUÍ: Captura de pantalla de la configuración del Desencadenador mostrando la hora 2:00 AM y frecuencia diaria]**

5. **Pestaña "Acciones" (Actions):**
   * Haz clic en **Nueva...** (New...).
   * **Acción:** *Iniciar un programa* (Start a program).
   * **Programa o script:** Haz clic en *Examinar...* y selecciona la ruta donde guardaste el archivo:  
     `C:\Users\User\Videos\ENTRADAS-TECNOLOGICO-TERMINADO\respaldar_db.bat`
   * **Iniciar en (opcional):** Escribe la ruta del directorio contenedor para evitar problemas con rutas relativas:  
     `C:\Users\User\Videos\ENTRADAS-TECNOLOGICO-TERMINADO`
   * Haz clic en *Aceptar*.

📸 **[INSERTAR AQUÍ: Captura de pantalla de la configuración de la Acción con la ruta completa a respaldar_db.bat]**

6. **Pestaña "Condiciones" (Conditions):**
   * Desmarca la opción *Iniciar la tarea solo si el equipo está conectado a la corriente alterna* si es una laptop que podría estar desconectada pero encendida a esa hora.
7. Haz clic en **Aceptar** para guardar la tarea. Te solicitará las credenciales de Administrador del equipo de Windows para confirmar la programación sin inicio de sesión activo.

---

## 4. VERIFICACIÓN DE FUNCIONAMIENTO Y PRUEBAS

### 4.1 Prueba Ejecución Manual Directa
1. Haz doble clic en el archivo `respaldar_db.bat` directamente desde el Explorador de Archivos.
2. Verifica que se haya creado la carpeta `C:\Backups_Entradas_Tecnologico`.
3. Confirma que exista el archivo de respaldo generado con el formato de nombre correcto (ej. `entradas_tecnologico_20260522_184500.bak`).

📸 **[INSERTAR AQUÍ: Captura de pantalla de la ventana negra de CMD tras una ejecución manual exitosa]**

📸 **[INSERTAR AQUÍ: Captura de pantalla de la carpeta C:\Backups_Entradas_Tecnologico conteniendo el archivo .bak generado]**

---

### 4.2 Prueba Ejecución en Programador de Tareas
1. Abre el **Programador de Tareas**.
2. Selecciona la *Biblioteca del Programador de Tareas* en el árbol de la izquierda.
3. Busca tu tarea `Respaldo Diario Entradas Tecnologico` en la lista central.
4. Haz clic derecho sobre ella y selecciona **Ejecutar** (Run) para forzar su inicio de inmediato.
5. Verifica el panel inferior en la pestaña **Historial** para confirmar que la tarea inició, se ejecutó y finalizó correctamente con el código `0` (Éxito).
6. Confirma en la carpeta de respaldos la aparición de un nuevo archivo `.bak`.

📸 **[INSERTAR AQUÍ: Captura de pantalla del Programador de Tareas mostrando la tarea en ejecución o finalizada con éxito en el Historial]**

---

## 5. CONCLUSIONES
La implementación de respaldos automáticos fuera de horas operativas (2:00 AM) asegura que:
* El impacto de rendimiento sobre el servidor es nulo, ya que no hay usuarios registrando entradas en el sistema escolar a esa hora.
* Toda la información histórica se encuentra protegida con copias físicas en el almacenamiento.
* Cumple rigurosamente con los requisitos de seguridad y planes de contingencia solicitados institucionalmente para el proyecto de Residencia Profesional.
