-- ================================================
-- SISTEMA DE ENTRADAS - TECNOLÓGICO
-- Script de creación LOCAL (SQL Server Express / LocalDB)
-- Incluye: tablas, datos de ejemplo y sistema de roles
-- ================================================

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'entradas_tecnologico')
BEGIN
    CREATE DATABASE entradas_tecnologico;
END
GO

USE entradas_tecnologico;
GO

-- ==================== TABLA ALUMNOS ====================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'alumnos')
BEGIN
    CREATE TABLE alumnos (
        Numero_control_alumno NVARCHAR(20) PRIMARY KEY,
        Nombre                NVARCHAR(100) NOT NULL,
        carrera               NVARCHAR(100) NOT NULL,
        correo                NVARCHAR(100),
        semestre              INT,
        grupo                 NVARCHAR(10)
    );
END
GO

-- ==================== TABLA PROFESORES ====================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'profesores')
BEGIN
    CREATE TABLE profesores (
        id_profesor      INT IDENTITY(1,1) PRIMARY KEY,
        ced_profesional  NVARCHAR(13) NOT NULL,   -- RFC de persona fisica (13 caracteres)
        nombre           NVARCHAR(100) NOT NULL,
        telefono         NVARCHAR(15),
        correo           NVARCHAR(100),
        sexo             NVARCHAR(10)
    );
END
ELSE
BEGIN
    -- Migracion: si la columna ced_profesional es INT, convertirla a NVARCHAR(13) para RFC
    IF EXISTS (
        SELECT * FROM sys.columns 
        WHERE object_id = OBJECT_ID('profesores') 
        AND name = 'ced_profesional' 
        AND system_type_id = 56  -- 56 = INT en SQL Server
    )
    BEGIN
        ALTER TABLE profesores ADD ced_profesional_rfc NVARCHAR(13);
        UPDATE profesores SET ced_profesional_rfc = CAST(ced_profesional AS NVARCHAR(13));
        ALTER TABLE profesores DROP COLUMN ced_profesional;
        EXEC sp_rename 'profesores.ced_profesional_rfc', 'ced_profesional', 'COLUMN';
    END
END
GO

-- ==================== TABLA USUARIOS (CON ROLES) ====================
-- rol: 'superusuario' puede crear/eliminar usuarios
-- rol: 'usuario'      solo puede usar el sistema (sin gestión de usuarios)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'usuarios')
BEGIN
    CREATE TABLE usuarios (
        id_usuario  INT IDENTITY(1,1) PRIMARY KEY,
        usuario     NVARCHAR(50) NOT NULL UNIQUE,
        contrasena  NVARCHAR(100) NOT NULL,
        rol         NVARCHAR(20) NOT NULL DEFAULT 'usuario',  -- 'superusuario' o 'usuario'
        nombre_completo NVARCHAR(100),
        activo      BIT NOT NULL DEFAULT 1,
        fecha_creacion DATETIME DEFAULT GETDATE(),
        is_online   BIT NOT NULL DEFAULT 0,
        ultima_conexion DATETIME NULL
    );
END
ELSE
BEGIN
    -- Si la tabla ya existe, agregar columnas de rol si no existen
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'rol')
        ALTER TABLE usuarios ADD rol NVARCHAR(20) NOT NULL DEFAULT 'usuario';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'nombre_completo')
        ALTER TABLE usuarios ADD nombre_completo NVARCHAR(100);

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'activo')
        ALTER TABLE usuarios ADD activo BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'fecha_creacion')
        ALTER TABLE usuarios ADD fecha_creacion DATETIME DEFAULT GETDATE();

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'is_online')
        ALTER TABLE usuarios ADD is_online BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('usuarios') AND name = 'ultima_conexion')
        ALTER TABLE usuarios ADD ultima_conexion DATETIME NULL;
END
GO

-- ==================== USUARIOS INICIALES ====================
-- Superusuario principal (admin del sistema)
IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'admin')
BEGIN
    INSERT INTO usuarios (usuario, contrasena, rol, nombre_completo, activo)
    VALUES ('admin', 'admin123', 'superusuario', 'Administrador del Sistema', 1);
END
GO

-- Usuario normal de ejemplo
IF NOT EXISTS (SELECT 1 FROM usuarios WHERE usuario = 'capturista')
BEGIN
    INSERT INTO usuarios (usuario, contrasena, rol, nombre_completo, activo)
    VALUES ('capturista', 'captura2026', 'usuario', 'Usuario Capturista', 1);
END
GO

-- ==================== DATOS DE EJEMPLO (ALUMNOS) ====================
-- Puedes eliminar estos INSERT si ya tienes tus datos exportados de Heroku
IF NOT EXISTS (SELECT 1 FROM alumnos WHERE Numero_control_alumno = '23060001')
BEGIN
    INSERT INTO alumnos (Numero_control_alumno, Nombre, carrera, correo, semestre, grupo)
    VALUES
    ('23060001', 'JUAN CARLOS LOPEZ MARTINEZ', 'INGENIERIA EN SISTEMAS COMPUTACIONALES', 'juan@tec.mx', 3, 'A'),
    ('23060002', 'MARIA FERNANDA GARCIA RUIZ', 'INGENIERIA INDUSTRIAL', 'maria@tec.mx', 2, 'B'),
    ('22060003', 'PEDRO SANCHEZ HERNANDEZ', 'INGENIERIA MECATRONICA', 'pedro@tec.mx', 5, 'A');
END
GO

-- ==================== DATOS DE EJEMPLO (PROFESORES) ====================
IF NOT EXISTS (SELECT 1 FROM profesores)
BEGIN
    SET IDENTITY_INSERT profesores ON;
    INSERT INTO profesores (id_profesor, ced_profesional, nombre, telefono, correo, sexo)
    VALUES
    (1, 'MEPC560101ABC', 'CARLOS MENDOZA PEREZ', '9511234567', 'cmendoza@tec.mx', 'Hombre'),
    (2, 'RELP700215XY8', 'ANA PATRICIA REYES LUNA', '9519876543', 'areyes@tec.mx', 'Mujer');
    SET IDENTITY_INSERT profesores OFF;
END
GO

PRINT '✅ Base de datos local creada correctamente.';
PRINT '   Usuario superadmin: admin / admin123';
PRINT '   Usuario normal:     capturista / captura2026';
GO
