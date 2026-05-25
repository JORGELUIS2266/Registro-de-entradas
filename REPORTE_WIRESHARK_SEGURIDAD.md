# REPORTE DE ANÁLISIS DE SEGURIDAD EN RED CON WIRESHARK
## Sistema: proyecto.exe — Entradas Tecnológico
**Fecha de análisis:** 20 de mayo de 2026  
**Analista:** Edgar Mauricio Sarmiento Ruiz  
**Institución:** TecNM  

---

## 1. OBJETIVO DEL ANÁLISIS

Capturar y analizar el tráfico de red generado por el sistema `proyecto.exe` al comunicarse con la base de datos SQL Server, con el fin de identificar posibles vulnerabilidades de seguridad en la transmisión de datos.

---

## 2. ENTORNO DE PRUEBA

| Componente | Descripción |
|-----------|-------------|
| Sistema analizado | `proyecto.exe` — Sistema de Entradas Tecnológico |
| Base de datos | SQL Server Express (instancia local) |
| Herramienta de análisis | Wireshark con Npcap |
| Sistema Operativo | Windows |
| Protocolo capturado | TDS (Tabular Data Stream) — protocolo nativo de SQL Server |
| Puerto analizado | 1433 (puerto estándar de SQL Server) |

---

## 3. PROCEDIMIENTO REALIZADO

### 3.1 — Preparación del Entorno

SQL Server en modo local usa por defecto el protocolo **Shared Memory**, que opera en la memoria del sistema operativo y es invisible para cualquier sniffer de red. Para poder analizar el tráfico con Wireshark, se cambió temporalmente el Connection String de `.\SQLEXPRESS` a `tcp:localhost,1433`, forzando el uso del protocolo **TCP/IP** a través de la interfaz de loopback.

```csharp
// Conexión modificada temporalmente para la prueba:
Server=tcp:localhost,1433;Initial Catalog=entradas_tecnologico;Integrated Security=True;
```

> **Nota:** Este mismo tráfico TCP/IP es el que viaja por la red real cuando el sistema se conecta en modo remoto (`red` o `nube`), por lo que los hallazgos de esta prueba aplican directamente al escenario de producción con conexión remota.

---

### 3.2 — Configuración de Wireshark

1. Se abrió Wireshark con permisos de **Administrador**.
2. Se seleccionó la interfaz **`Adapter for loopback traffic capture`**.
3. Se aplicó el filtro de captura: `tcp port 1433`

📸 **[INSERTAR AQUÍ: Captura de pantalla de Wireshark con la interfaz seleccionada y el filtro aplicado]**

---

### 3.3 — Acciones Realizadas Durante la Captura

Con Wireshark capturando tráfico, se ejecutaron las siguientes acciones en `proyecto.exe`:

1. **Login** con el usuario `admin` y su contraseña.
2. **Navegación** por los módulos de alumnos (por carrera).
3. **Búsqueda** de alumnos con filtros.

📸 **[INSERTAR AQUÍ: Captura de pantalla de Wireshark mostrando los paquetes TDS capturados]**

---

### 3.4 — Análisis con "Follow TCP Stream"

Para inspeccionar el contenido completo de la comunicación entre la aplicación y SQL Server, se utilizó la función **Follow → TCP Stream** (clic derecho sobre un paquete TDS). Esta función muestra en texto plano toda la conversación SQL.

📸 **[INSERTAR AQUÍ: Captura de pantalla de la ventana Follow TCP Stream]**

---

## 4. RIESGOS ENCONTRADOS

### 🔴 Riesgo 1 — Tráfico SQL sin cifrado TLS

**Descripción:**  
El protocolo TDS (comunicación entre la aplicación y SQL Server) viaja **sin cifrado TLS/SSL**. Esto significa que cualquier persona conectada a la misma red (Wi-Fi o Ethernet) puede usar Wireshark u otra herramienta de sniffing para interceptar y leer el contenido completo de las consultas SQL.

**Evidencia encontrada en el TCP Stream:**
```sql
UPDATE usuarios SET is_online = 1, ultima_conexion = GETDATE() WHERE usuario = @u
SELECT rol FROM usuarios WHERE usuario=@u AND contrasena=@c AND activo=1
SELECT * FROM alumnos WHERE carrera = @carrera ORDER BY Nombre
```

**Impacto:** Un atacante en la misma red podría ver la estructura completa de la base de datos, los nombres de las tablas, columnas y la lógica de las consultas del sistema.

**Nivel de riesgo en conexión remota:** 🔴 **ALTO** — El Connection String de modo red tiene `Encrypt=False`, lo que confirma que en producción el tráfico NO está cifrado.

📸 **[INSERTAR AQUÍ: Captura del TCP Stream mostrando las queries SQL en texto plano]**

---

### 🔴 Riesgo 2 — Datos personales de alumnos expuestos en tránsito

**Descripción:**  
Los resultados de las consultas SQL (registros completos de alumnos) viajan en texto plano por la red. Se pudieron leer completamente desde Wireshark los siguientes datos sensibles:

**Evidencia encontrada en el TCP Stream:**
```
Numero_control_alumno | Nombre                          | Carrera                                    | Correo                        | Semestre | Grupo
12620123              | alma dulce gonzales flores       | INGENIERIA EN SISTEMAS COMPUTACIONALES     | alexandra12@gmail.com         | 7        | US
22620241              | Ameli Reyes Hernandez            | INGENIERIA EN SISTEMAS COMPUTACIONALES     | ameli90@gmail.com             | 8        | BS
20625003              | Ana Jimenez Rios                 | INGENIERIA EN SISTEMAS COMPUTACIONALES     | ana.jimenez3@gmail.com        | 11       | AS
22620235              | Jorge Luis Hernandez Matra       | INGENIERIA EN SISTEMAS COMPUTACIONALES     | jorgehdz14@gmail.com          |          | BS
22620269              | marlene maricela osorio ramirez  | INGENIERIA EN SISTEMAS COMPUTACIONALES     | marleneramirez@gmail.com      | 4        | BS
22620066              | edgar sarmiento ruiz             | INGENIERIA EN SISTEMAS COMPUTACIONALES     | edgarsarmiento133@gmail.com   | 4        | bs
```

Se capturaron datos de **todas las carreras**: Sistemas Computacionales, Industrial, Mecatrónica, Civil, Gestión Empresarial, Administración y Arquitectura.

**Impacto:** Cualquier persona en la misma red del Tecnológico podría capturar nombres completos, números de control, correos electrónicos y datos académicos de todos los alumnos registrados en el sistema.

**Nivel de riesgo en conexión remota:** 🔴 **ALTO** — Datos personales protegidos por normativas de privacidad quedan completamente expuestos.

📸 **[INSERTAR AQUÍ: Captura del TCP Stream mostrando los datos de alumnos visibles]**

---

### 🔴 Riesgo 3 — Información de operaciones del sistema visible

**Descripción:**  
Las operaciones administrativas del sistema también son visibles en el tráfico de red. Se pudo observar:

**Evidencia encontrada en el TCP Stream:**
```sql
-- Se puede ver quién inicia sesión y cuándo:
UPDATE usuarios SET is_online = 0 WHERE usuario = @u
UPDATE usuarios SET is_online = 1, ultima_conexion = GETDATE() WHERE usuario = @u

-- Se puede ver el rol del usuario:
rol = superusuario

-- Se puede ver qué carreras consulta y qué filtros aplica:
@carrera = INGENIERIA EN SISTEMAS COMPUTACIONALES
@carrera = INGENIERIA INDUSTRIAL
@carrera = INGENIERIA MECATRONICA
@filtro  = %22%
```

**Impacto:** Un atacante podría monitorear en tiempo real quién usa el sistema, cuándo inicia/cierra sesión, qué módulos consulta y qué búsquedas realiza.

**Nivel de riesgo en conexión remota:** 🔴 **ALTO** — Permite realizar un perfilado completo de la actividad de los usuarios del sistema.

📸 **[INSERTAR AQUÍ: Captura mostrando las operaciones de login/sesión visibles]**

---

### 🟢 Aspecto Positivo — Contraseñas de usuarios protegidas con Hash

**Descripción:**  
A pesar de que el tráfico no está cifrado, las contraseñas de los usuarios del sistema **sí están protegidas**. En el TCP Stream se verificó que la contraseña viaja como un hash SHA-256 irreversible, no como texto plano.

**Evidencia encontrada en el TCP Stream:**
```
SELECT rol FROM usuarios WHERE usuario=@u AND contrasena=@c AND activo=1
  @u = admin
  @c = b82216c7d2e8df90eb6753898504978d922cd0e0c58b1367a0a753cd8bd67b9f
```

El valor `b82216c7d2e8...` es el hash SHA-256 de la contraseña. **Aunque se intercepte este paquete, no se puede recuperar la contraseña original.**

**Nivel de riesgo:** 🟢 **BAJO** — Las credenciales de acceso están correctamente protegidas.

📸 **[INSERTAR AQUÍ: Captura del TCP Stream mostrando el hash SHA-256 en el parámetro @c]**

---

## 5. TABLA RESUMEN DE RIESGOS ENCONTRADOS

| # | Riesgo Encontrado | Nivel | Detalle |
|---|------------------|-------|---------|
| 1 | Tráfico SQL sin cifrado TLS | 🔴 ALTO | Queries visibles en texto plano por la red |
| 2 | Datos personales de alumnos expuestos | 🔴 ALTO | Nombres, correos, números de control legibles |
| 3 | Operaciones del sistema visibles | 🔴 ALTO | Login, sesiones y consultas monitoreables |
| 4 | Contraseñas de usuarios | 🟢 SEGURO | Hash SHA-256, no recuperable |
| 5 | SQL Injection | 🟢 SEGURO | Consultas con parámetros @param |

---

## 6. CONCLUSIÓN DEL ANÁLISIS

El análisis con Wireshark reveló que, si bien las **contraseñas de usuarios están correctamente protegidas** mediante hash SHA-256, el **tráfico de red entre la aplicación y SQL Server no cuenta con cifrado TLS**. Esto expone las consultas SQL, los datos personales de los alumnos y las operaciones del sistema a cualquier persona que tenga acceso a la misma red.

Este riesgo es especialmente relevante considerando que el sistema está diseñado para operar en **conexión remota** (`Encrypt=False` en el modo red), lo que significa que en un entorno de producción dentro de la red del Tecnológico, los datos transitarían sin protección.

---

*Análisis realizado con Wireshark durante la Residencia Profesional — TecNM*  
*Fecha: 20 de mayo de 2026*
