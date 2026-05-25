# 🚀 Guía de Respaldo y Subida al Repositorio Git

Esta guía detalla el estado actual del repositorio, los comandos necesarios para subir tus cambios a GitHub y los métodos disponibles para automatizar este proceso en Windows.

---

## 📌 1. Repositorios Configurados

Actualmente, tu proyecto local tiene configurados dos repositorios remotos en GitHub. Puedes verlos ejecutando `git remote -v`:

1. **`origin`** (Repositorio Principal)
   * **URL:** `https://github.com/edgarmauricio123/ENTRADAS-TECNOLOGICO-TERMINADO.git`
   * **Propósito:** Es tu repositorio de trabajo personal/principal.
2. **`jorgeluis`** (Repositorio Secundario / Colaborador)
   * **URL:** `https://github.com/JORGELUIS2266/Registro-de-entradas.git`
   * **Propósito:** Repositorio de tu compañero o colaborador.

### Rama de Trabajo Activa:
* Estás en la rama local **`jorge`**.
* Tus cambios se subirán a la rama **`jorge`** de tu repositorio remoto (`origin`).

---

## 💻 2. Comandos Git para Subir Cambios Manualmente

Para subir tus cambios actuales al repositorio, se deben seguir estos pasos secuenciales:

### Paso A: Verificar el estado de los archivos
Antes de agregar nada, verifica qué archivos han sido modificados, eliminados o agregados:
```bash
git status
```

### Paso B: Preparar los archivos para el commit (Stage)
Para agregar todos los cambios nuevos, modificados y eliminados al área de preparación:
```bash
git add .
```
> [!TIP]
> Si solo quieres agregar un archivo específico (por ejemplo, el reporte), usa:
> `git add REPORTE_RESPALDOS_AUTOMATICOS.md`

### Paso C: Confirmar los cambios (Commit)
Crea una instantánea local de los cambios agregados con un mensaje descriptivo:
```bash
git commit -m "docs: Agregar guías de respaldo y reportes de seguridad"
```

### Paso D: Subir los cambios a GitHub (Push)
Envía tus commits locales al repositorio remoto (`origin`) en la rama correspondiente (`jorge`):
```bash
git push origin jorge
```

---

## 🤖 3. ¿Cómo Subir Cambios Automáticamente?

Sí, es posible automatizar la subida de cambios a GitHub en Windows. Aquí tienes las mejores opciones:

### Opción A: Crear un script automatizado (`.bat`) y usar el Programador de Tareas de Windows (Recomendado)

Podemos crear un script por lotes que agregue, confirme y suba los cambios con un solo doble clic, o programarlo para que se ejecute solo.

#### Paso 1: Crear el archivo `subir_automatico.bat` en la raíz del proyecto
Crea un archivo llamado `subir_automatico.bat` con el siguiente contenido:

```batch
@echo off
cd /d "C:\Users\User\Videos\ENTRADAS-TECNOLOGICO-TERMINADO"
echo === INICIANDO RESPALDO AUTOMÁTICO EN GITHUB ===
date /t
time /t

:: 1. Agregar todos los cambios
git add .

:: 2. Crear commit con fecha y hora automática
git commit -m "Auto-backups: Respaldo automatico del %date% a las %time%"

:: 3. Subir a la rama 'jorge' del remote 'origin'
git push origin jorge

echo === RESPALDO COMPLETADO ===
timeout /t 5
```

#### Paso 2: Programar la ejecución en Windows
Para que este script se ejecute automáticamente cada cierto tiempo sin tu intervención:
1. Presiona la tecla Windows, escribe **Programador de tareas** (Task Scheduler) y ábrelo.
2. En el panel derecho, haz clic en **Crear tarea básica...** (Create Basic Task...).
3. Ponle un nombre (ej. `Git Auto Backup`) y haz clic en Siguiente.
4. Selecciona la frecuencia: **Diariamente**, **Semanalmente** o **Al iniciar el equipo**.
5. En **Acción**, selecciona **Iniciar un programa**.
6. En **Programa o script**, haz clic en Examinar y selecciona el archivo `subir_automatico.bat` que creaste.
7. Haz clic en Finalizar. ¡Listo! Se ejecutará solo según la frecuencia elegida.

---

### Opción B: Usar extensiones de VS Code o Visual Studio
Si usas VS Code o Visual Studio para programar:
* En **VS Code**, puedes instalar la extensión **"Git Auto Commit"** o **"Git Autocommit"**.
* Estas extensiones guardan tus archivos, hacen commit con un mensaje por defecto y ejecutan `git push` automáticamente cada vez que guardas un archivo (`Ctrl + S`) o cada determinado número de minutos.

---

## ⚠️ Advertencias sobre la Automatización (Pros y Contras)

> [!WARNING]
> Automatizar las subidas de Git tiene riesgos importantes en el desarrollo de software profesional.

| Ventajas (Pros) | Desventajas (Contras) |
| :--- | :--- |
| **No lo olvidas**: Tus respaldos siempre estarán al día en la nube de GitHub. | **Sube código roto**: Si estás a mitad de un cambio que no compila, el script lo subirá igual, afectando a tus compañeros. |
| **Ahorro de tiempo**: Evita escribir los comandos cada vez. | **Historial sucio (Spam)**: El historial de commits se llenará de cientos de mensajes genéricos como `"Auto-backups: ..."`. |
| **Automatización total**: Ideal para carpetas que solo contienen reportes o notas estáticas. | **Fugas de seguridad**: Si por error guardas una contraseña o credencial en un archivo local, el script la subirá a GitHub públicamente antes de que puedas borrarla. |

**Recomendación profesional:** Es preferible realizar commits y pushes manuales cuando termines una funcionalidad específica o una sesión de trabajo. De esta forma, mantienes el código limpio, seguro y funcional.
