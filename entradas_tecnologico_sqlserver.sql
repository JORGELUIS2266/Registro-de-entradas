-- Script para SQL Server / SQL Server Express / Azure SQL
-- Base de datos: entradas_tecnologico

-- ============================================
-- Tabla: alumnos
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'alumnos')
BEGIN
    CREATE TABLE alumnos (
        Numero_control_alumno NVARCHAR(8) NOT NULL PRIMARY KEY,
        Nombre NVARCHAR(70) NOT NULL,
        carrera NVARCHAR(60) NOT NULL,
        correo NVARCHAR(70) NOT NULL,
        semestre INT NOT NULL,
        grupo NVARCHAR(20) NOT NULL
    );
END
ELSE
BEGIN
    -- Ampliar columna si ya existe con tamaño pequeño
    ALTER TABLE alumnos ALTER COLUMN carrera NVARCHAR(60) NOT NULL;
    
    -- Si existe como INT y necesitamos cambiar a NVARCHAR(8) (Opcional, en SQLite/SQL Server puede requerir DROP/CREATE si es PK)
    -- Asumimos que la BD se recrea o la tabla está vacía para probar.
END
GO

-- ============================================
-- Tabla: profesores
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'profesores')
BEGIN
    CREATE TABLE profesores (
        id_profesor INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        ced_profesional INT NOT NULL,
        nombre NVARCHAR(70) NOT NULL,
        telefono NVARCHAR(12) NOT NULL,
        correo NVARCHAR(70) NOT NULL,
        sexo NVARCHAR(10) NOT NULL
    );
END
GO

-- ============================================
-- Tabla: usuarios (para el login)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'usuarios')
BEGIN
    CREATE TABLE usuarios (
        usuario NVARCHAR(50) NOT NULL PRIMARY KEY,
        contrasena NVARCHAR(50) NOT NULL
    );
    INSERT INTO usuarios (usuario, contrasena) VALUES ('admin', 'admin123');
END
GO

-- ============================================
-- Datos de prueba: alumnos (carreras oficiales)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM alumnos)
BEGIN
    INSERT INTO alumnos (Numero_control_alumno, Nombre, carrera, correo, semestre, grupo) VALUES
    ('22620001', 'Ana Kimberly Gonzalez Roman',        'INGENIERIA EN SISTEMAS COMPUTACIONALES', 'anagonz@gmail.com',          4, '4BS'),
    ('22620002', 'Edgar Mauricio Sarmiento Ruiz',      'INGENIERIA MECATRONICA',                 'edgarsarmiento@gmail.com',   4, '4BS'),
    ('22620003', 'Marco Antonio Rosas Lopez',          'INGENIERIA EN SISTEMAS COMPUTACIONALES', 'marcorosas@gmail.com',       4, '4BS'),
    ('22620004', 'Sandy Santiago Hernandez',           'INGENIERIA MECATRONICA',                 'sansantiago@gmail.com',      4, '4BS'),
    ('22620005', 'Mario Dominguez Ruiz',               'INGENIERIA EN SISTEMAS COMPUTACIONALES', 'mariodominguez@gmail.com',   2, '2AS'),
    ('22620006', 'Marlene Osorio Ramirez',             'INGENIERIA INDUSTRIAL',                  'marleneosorio@gmail.com',    4, '4BS'),
    ('22620007', 'Jose Luis Perez Torres',             'INGENIERIA CIVIL',                       'joseperez@gmail.com',        6, '6CS'),
    ('22620008', 'Ari Lopez Aparicio',                 'INGENIERIA EN GESTION EMPRESARIAL',      'arilopez@gmail.com',         4, '4BS'),
    ('22620009', 'Lucia Mendoza Sanchez',              'LICENCIATURA EN ADMINISTRACION',         'luciamendoza@gmail.com',     3, '3AS'),
    ('A2620010', 'Carlos Rojas Espinoza',              'LICENCIATURA EN ARQUITECTURA',           'carlosrojas@gmail.com',      5, '5BS');
END
GO

-- ============================================
-- Datos de prueba: profesores
-- ============================================
SET IDENTITY_INSERT profesores ON;
IF NOT EXISTS (SELECT 1 FROM profesores)
BEGIN
    INSERT INTO profesores (id_profesor, ced_profesional, nombre, telefono, correo, sexo) VALUES
    (1, 123456789, 'Jose Alfredo Roman Cruz',   '9532303920', 'romanjosealfredo@gmail.com', 'Masculino'),
    (2, 123456782, 'Alondra Salcedo Morales',   '9532455878', 'alondrasalcedo@gmail.com',   'Femenino'),
    (3, 123654987, 'Lucia Sanchez Ramirez',     '9531597553', 'lucisanchez123@gmail.com',   'Femenino');
END
SET IDENTITY_INSERT profesores OFF;
GO

PRINT 'Base de datos entradas_tecnologico lista y actualizada.';
GO
