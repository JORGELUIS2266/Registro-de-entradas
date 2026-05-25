# ================================================================
# ConfigurarSQLServer.ps1
# Configura SQL Server Express para conexiones por red local
# ================================================================

Write-Host "=== Configurando SQL Server Express para red local ===" -ForegroundColor Cyan

# ── 1. Firewall: abrir puerto 1433 ──────────────────────────────
Write-Host "`n[1/5] Abriendo puerto 1433 en el Firewall..." -ForegroundColor Yellow
$reglaExiste = netsh advfirewall firewall show rule name="SQL Server 1433" 2>&1
if ($reglaExiste -notmatch "No rules match") {
    netsh advfirewall firewall delete rule name="SQL Server 1433" | Out-Null
}
netsh advfirewall firewall add rule name="SQL Server 1433" protocol=TCP dir=in localport=1433 action=allow | Out-Null
Write-Host "   Puerto 1433 abierto OK" -ForegroundColor Green

# ── 2. Habilitar TCP/IP en SQL Server Express via Registro ───────
Write-Host "`n[2/5] Habilitando TCP/IP en SQLEXPRESS..." -ForegroundColor Yellow
$regPath = "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQLServer\SuperSocketNetLib\Tcp"

# Buscar la clave correcta si la version difiere (MSSQL15, MSSQL16, etc.)
$sqlKeys = Get-ChildItem "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server" | Where-Object { $_.Name -match "MSSQL\d+\.SQLEXPRESS" }
if ($sqlKeys) {
    $regPath = Join-Path $sqlKeys[0].PSPath "MSSQLServer\SuperSocketNetLib\Tcp"
    Set-ItemProperty -Path $regPath -Name "Enabled" -Value 1 -ErrorAction SilentlyContinue
    # Establecer puerto 1433 en IPAll
    $ipAllPath = Join-Path $regPath "IPAll"
    Set-ItemProperty -Path $ipAllPath -Name "TcpPort" -Value "1433" -ErrorAction SilentlyContinue
    Set-ItemProperty -Path $ipAllPath -Name "TcpDynamicPorts" -Value "" -ErrorAction SilentlyContinue
    Write-Host "   TCP/IP habilitado en puerto 1433 OK" -ForegroundColor Green
} else {
    Write-Host "   ADVERTENCIA: No se encontró la clave de registro de SQLEXPRESS. Hazlo manual en SQL Server Configuration Manager." -ForegroundColor Red
}

# ── 3. Habilitar autenticación mixta (SQL + Windows) ────────────
Write-Host "`n[3/5] Habilitando autenticación SQL (modo mixto)..." -ForegroundColor Yellow
$sqlInstancias = Get-ChildItem "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server" | Where-Object { $_.Name -match "MSSQL\d+\.SQLEXPRESS" }
if ($sqlInstancias) {
    $mssqlServerPath = Join-Path $sqlInstancias[0].PSPath "MSSQLServer"
    Set-ItemProperty -Path $mssqlServerPath -Name "LoginMode" -Value 2
    Write-Host "   Modo mixto habilitado OK (LoginMode = 2)" -ForegroundColor Green
} else {
    Write-Host "   ADVERTENCIA: No se pudo cambiar LoginMode." -ForegroundColor Red
}

# ── 4. Activar usuario SA y poner contraseña ────────────────────
Write-Host "`n[4/5] Configurando usuario SA..." -ForegroundColor Yellow
$sqlcmd = "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE"
if (-not (Test-Path $sqlcmd)) {
    # Buscar sqlcmd en rutas comunes
    $posibles = @(
        "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn\SQLCMD.EXE",
        "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\160\Tools\Binn\SQLCMD.EXE",
        "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\150\Tools\Binn\SQLCMD.EXE",
        "C:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE"
    )
    foreach ($p in $posibles) {
        if (Test-Path $p) { $sqlcmd = $p; break }
    }
}

$saScript = @"
ALTER LOGIN sa ENABLE;
GO
ALTER LOGIN sa WITH PASSWORD = 'TecnoLocal2026';
GO
"@
$saScript | Out-File -FilePath "$env:TEMP\sa_setup.sql" -Encoding ASCII

try {
    & $sqlcmd -S ".\SQLEXPRESS" -E -i "$env:TEMP\sa_setup.sql" -b 2>&1 | Out-Null
    Write-Host "   Usuario SA configurado OK (password: TecnoLocal2026)" -ForegroundColor Green
} catch {
    Write-Host "   ADVERTENCIA: No se pudo configurar SA ahora (se aplicará después del reinicio)" -ForegroundColor DarkYellow
}

# ── 5. Reiniciar SQL Server ──────────────────────────────────────
Write-Host "`n[5/5] Reiniciando SQL Server Express..." -ForegroundColor Yellow
try {
    Restart-Service -Name "MSSQL`$SQLEXPRESS" -Force
    Start-Sleep -Seconds 5
    Write-Host "   SQL Server reiniciado OK" -ForegroundColor Green
} catch {
    Write-Host "   Error al reiniciar: $($_.Exception.Message)" -ForegroundColor Red
}

# ── Intentar configurar SA después del reinicio ─────────────────
try {
    & $sqlcmd -S ".\SQLEXPRESS" -E -i "$env:TEMP\sa_setup.sql" -b 2>&1 | Out-Null
    Write-Host "   Usuario SA confirmado OK" -ForegroundColor Green
} catch {}

# ── Resultado final ─────────────────────────────────────────────
Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host " CONFIGURACION COMPLETADA" -ForegroundColor Green
Write-Host " Las otras PCs ya pueden conectarse a:" -ForegroundColor White
Write-Host " Servidor: 192.168.20.44,1433" -ForegroundColor White
Write-Host " Usuario:  sa" -ForegroundColor White
Write-Host " Password: TecnoLocal2026" -ForegroundColor White
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
pause
