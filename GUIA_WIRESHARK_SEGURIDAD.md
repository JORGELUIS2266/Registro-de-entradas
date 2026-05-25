# GUÍA: ANÁLISIS DE SEGURIDAD DE RED CON WIRESHARK
## Sistema: proyecto.exe — Entradas Tecnológico
**Fecha:** 20 de mayo de 2026

---

## ¿Qué es Wireshark y por qué usarlo?

Wireshark es un **analizador de tráfico de red** (sniffer). Captura todos los paquetes de datos que viajan por tu red y los muestra en detalle. Para un sistema como `proyecto.exe` que se comunica con SQL Server, te permite verificar:

- ✅ ¿Las contraseñas viajan cifradas o en texto plano?
- ✅ ¿Qué datos envía/recibe tu app a la base de datos?
- ✅ ¿Hay protocolos inseguros activos?

---

## PASO 1 — Instalar Wireshark

1. Descarga desde: https://www.wireshark.org/download.html
2. Instala con las opciones por defecto (incluye **Npcap**, requerido para capturar tráfico).
3. Reinicia tu PC si es necesario.

> ⚠️ **Importante:** Ejecuta Wireshark **como Administrador** para tener acceso a todas las interfaces de red.

---

## PASO 2 — Identificar tu Interfaz de Red

Antes de capturar, necesitas saber por qué interfaz sale el tráfico de tu app.

1. Abre Wireshark.
2. En la pantalla inicial verás la lista de interfaces (Wi-Fi, Ethernet, Loopback, etc.).
3. Para conexión **local** (`Server=.\SQLEXPRESS`): busca la interfaz **Loopback (127.0.0.1)** o **lo0**.
4. Para conexión **en red** (`IP 10.167.120.231`): busca tu interfaz **Wi-Fi** o **Ethernet**.

> 💡 **Tip:** La interfaz que tenga una línea de actividad moviéndose es la que está activa.

---

## PASO 3 — Configurar el Filtro de Captura

Para que Wireshark solo muestre tráfico de tu aplicación y no el ruido de toda la red, aplica estos filtros:

### Filtro para tráfico SQL Server (puerto 1433):
```
tcp.port == 1433
```

### Filtro para tráfico con IP específica (modo red):
```
ip.addr == 10.167.120.231 and tcp.port == 1433
```

### Filtro para loopback (modo local):
```
tcp.port == 1433
```
> (Captura la interfaz Loopback al mismo tiempo)

**Cómo aplicarlo:**
1. Selecciona tu interfaz de red.
2. En la barra de filtro (parte superior), escribe el filtro.
3. Presiona **Enter** para aplicarlo.

---

## PASO 4 — Capturar Tráfico de proyecto.exe

1. Con el filtro aplicado, haz clic en el botón **🦈 (Iniciar captura)** (ícono azul de tiburón).
2. Cambia a `proyecto.exe` y **realiza una acción** que genere tráfico a la BD:
   - Intenta hacer **Login** con un usuario y contraseña.
   - Consulta la lista de alumnos o profesores.
   - Registra un nuevo alumno.
3. Regresa a Wireshark y haz clic en **⏹ (Detener captura)** (botón rojo cuadrado).

---

## PASO 5 — Analizar los Paquetes Capturados

Una vez detenida la captura, deberías ver paquetes del protocolo **TDS** (Tabular Data Stream), que es el protocolo que usa SQL Server.

### Qué buscar en cada paquete:

#### 5.1 — Verificar si el tráfico va cifrado
Observa la columna **Protocol**:
- Si ves **TLS** o **SSL**: el tráfico está cifrado ✅
- Si ves solo **TDS**: el tráfico viaja en texto plano (potencialmente inseguro) ⚠️

#### 5.2 — Inspeccionar el contenido de un paquete
1. Haz clic en un paquete TDS de la lista.
2. En el panel inferior, ve a la sección **"Tabular Data Stream"** o a la vista **Hex**.
3. Busca si puedes ver texto como:
   - `SELECT`, `INSERT`, `UPDATE` — Queries SQL visibles.
   - `usuario`, `contrasena` — Nombres de campo.
   - Valores de contraseñas en texto plano.

#### 5.3 — Buscar texto sensible (Follow TCP Stream)
Este es el análisis más importante:
1. Haz clic derecho sobre cualquier paquete TDS.
2. Selecciona: **Follow → TCP Stream**.
3. Se abrirá una ventana con la conversación completa en texto.
4. Busca manualmente si aparecen:
   - Contraseñas de usuarios del sistema.
   - El Connection String completo.
   - Datos sensibles de alumnos/profesores.

---

## PASO 6 — Interpretar Resultados para tu Sistema

### Escenario A: Modo LOCAL (`.\SQLEXPRESS`, Integrated Security=True)
| Aspecto | Resultado Esperado |
|---------|-------------------|
| Protocolo | TDS sobre Loopback (127.0.0.1) |
| Credenciales en tráfico | ❌ No aparecen (usa Autenticación Windows) |
| Queries SQL visibles | ⚠️ Posiblemente sí (sin `Encrypt=True`) |
| Contraseñas de usuarios hasheadas | ✅ Solo se verá el hash SHA-256, nunca el texto plano |
| Riesgo general | Bajo (tráfico no sale de la máquina) |

### Escenario B: Modo RED (`10.167.120.231:1433`)
| Aspecto | Resultado Esperado |
|---------|-------------------|
| Protocolo | TDS sobre Ethernet/Wi-Fi |
| Credenciales en tráfico | ⚠️ La contraseña `sa` puede viajar expuesta si `Encrypt=False` |
| Queries SQL visibles | ⚠️ Sí, sin cifrado de capa de transporte |
| Contraseñas de usuarios hasheadas | ✅ Solo se verá el hash SHA-256 |
| Riesgo general | **MEDIO-ALTO** sin TLS/SSL |

---

## PASO 7 — Evidencias para tu Reporte

Wireshark te permite guardar evidencias de manera profesional:

### 7.1 — Guardar la captura completa
- Archivo → Guardar como → `captura_proyecto_login.pcap`

### 7.2 — Exportar capturas de pantalla
- Cuando tengas el TCP Stream abierto, haz captura de pantalla (Win + Shift + S).
- Guarda como evidencia en tu reporte.

### 7.3 — Exportar paquetes específicos
- File → Export Specified Packets (para exportar solo paquetes relevantes)

---

## PASO 8 — Conclusiones para tu Reporte de Seguridad

Con base en el análisis de Wireshark, tu reporte debería incluir:

```
Hallazgo 1: Contraseñas de usuarios protegidas
  - Resultado Wireshark: El campo contraseña en tráfico de red muestra 
    únicamente el hash SHA-256, confirmando que nunca viaja en texto plano.
  - Evidencia: [screenshot del TCP Stream mostrando el hash]

Hallazgo 2: Tráfico SQL (modo local)
  - Resultado Wireshark: El tráfico se limita a la interfaz loopback (127.0.0.1),
    no es accesible desde la red externa.
  - Evidencia: [captura de pantalla de la interfaz de origen]

Hallazgo 3 (si aplica): Tráfico en modo RED
  - Resultado Wireshark: El protocolo TDS viaja sin cifrado TLS 
    debido a Encrypt=False en el Connection String.
  - Recomendación: Habilitar Encrypt=True y configurar certificado SSL en SQL Server.
```

---

## ⚠️ Nota Importante sobre tu Config Actual

Revisando tu `proyecto.exe.config`, se detectó que las contraseñas siguen en texto plano en el archivo de configuración:

```xml
<add key="PasswordRed" value="TecnoLocal2026"/>
<add key="PasswordNube" value="Tecno.GCP.2026"/>
```

**Riesgo:** Este archivo `.config` viaja junto al `.exe` en el despliegue. Cualquier persona con acceso al equipo puede leerlo.

**Recomendación:** Para el reporte, documentar que las credenciales del Config están fuera del código fuente (✅ avance), pero que idealmente deberían estar **cifradas** usando `aspnet_regiis` o almacenadas en variables de entorno del sistema operativo.

---

## Resumen de Pasos Rápidos

```
1. Instalar Wireshark (con Npcap)
2. Abrir como Administrador
3. Seleccionar interfaz (Loopback para local, Wi-Fi/Ethernet para red)
4. Filtrar: tcp.port == 1433
5. Iniciar captura → Usar proyecto.exe (Login + operaciones)
6. Detener captura
7. Follow → TCP Stream en un paquete TDS
8. Documentar y capturar pantalla de los hallazgos
9. Guardar .pcap como evidencia
```
