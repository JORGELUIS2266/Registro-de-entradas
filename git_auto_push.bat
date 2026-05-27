@echo off
:: ============================================================
::  GIT AUTO PUSH — Sistema de Entradas Tecnológico
::  Repositorio: https://github.com/JORGELUIS2266/Registro-de-entradas.git
::  Rama: jorge
::  Ejecutado por: Programador de Tareas de Windows
:: ============================================================

cd /d "C:\Users\User\Videos\ENTRADAS-TECNOLOGICO-TERMINADO"

:: Verificar si hay cambios para commitear
git status --porcelain > "%TEMP%\git_status.txt" 2>&1
for /f %%i in ("%TEMP%\git_status.txt") do set size=%%~zi
if "%size%"=="0" (
    echo [%date% %time%] Sin cambios nuevos. No se hizo push. >> "%~dp0git_auto_log.txt"
    exit /b 0
)

:: Agregar todos los cambios
git add .

:: Commit con fecha y hora automática
git commit -m "Auto-backup: %date% %time%"

:: Push al repositorio principal
git push origin jorge

:: Registrar resultado en log
echo [%date% %time%] Push realizado correctamente a origin/jorge >> "%~dp0git_auto_log.txt"
