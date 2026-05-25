@echo off
:: ==============================================================================
:: respaldar_db.bat
:: Script para realizar el respaldo automático de la base de datos SQL Server
:: Diseñado para ejecutarse manualmente o a través de la Tarea Programada de Windows.
:: ==============================================================================

:: 1. Definir directorio donde se guardarán los respaldos
set BACKUP_DIR=C:\Backups_Entradas_Tecnologico

:: Crear el directorio si no existe
if not exist "%BACKUP_DIR%" (
    mkdir "%BACKUP_DIR%"
)

:: 2. Generar una marca de tiempo consistente (Formato: YYYYMMDD_HHMMSS) usando PowerShell
for /f "usebackq tokens=*" %%i in (`powershell -NoProfile -Command "Get-Date -Format 'yyyyMMdd_HHmmss'"`) do set TIMESTAMP=%%i

:: 3. Definir la ruta completa del archivo de respaldo (.bak)
set BACKUP_FILE=%BACKUP_DIR%\entradas_tecnologico_%TIMESTAMP%.bak

echo ==========================================================================
echo  RESPALDO AUTOMÁTICO DE BASE DE DATOS - ENTRADAS TECNOLÓGICO
echo  Fecha/Hora: %TIMESTAMP%
echo  Destino:    %BACKUP_FILE%
echo ==========================================================================

:: 4. Ejecutar el comando BACKUP mediante sqlcmd conectado a la instancia local de SQLEXPRESS
:: Se usa la autenticación integrada de Windows (-E)
sqlcmd -S .\SQLEXPRESS -E -Q "BACKUP DATABASE [entradas_tecnologico] TO DISK = N'%BACKUP_FILE%' WITH NOFORMAT, INIT, NAME = N'Respaldo Diario Autoprogramado', SKIP, NOREWIND, NOUNLOAD, STATS = 10"

:: 5. Validar si la ejecución fue exitosa
if %ERRORLEVEL% EQU 0 (
    echo [OK] Respaldo generado exitosamente.
) else (
    echo [ERROR] Ocurrió un error al intentar crear el respaldo.
    exit /b %ERRORLEVEL%
)

:: Si se ejecuta con el parámetro "/interactive", pausar al final (útil para pruebas manuales)
if "%1"=="/interactive" (
    echo.
    echo Presiona cualquier tecla para salir...
    pause > nul
)
