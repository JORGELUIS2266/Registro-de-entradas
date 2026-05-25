-- ============================================================
-- MIGRACION: Columna ced_profesional INT -> NVARCHAR(13) (RFC)
-- Ejecutar en bases de datos EXISTENTES que ya tienen datos.
-- ============================================================

USE entradas_tecnologico;
GO

-- Paso 1: Verificar si la columna es INT y agregar columna temporal
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('profesores') 
    AND name = 'ced_profesional' 
    AND system_type_id = 56  -- 56 = INT en SQL Server
)
BEGIN
    PRINT 'Columna ced_profesional es INT. Iniciando migracion a NVARCHAR(13)...';
    ALTER TABLE profesores ADD ced_profesional_rfc NVARCHAR(13);
    PRINT 'Columna temporal ced_profesional_rfc creada.';
END
ELSE
BEGIN
    PRINT 'La columna ced_profesional ya es NVARCHAR. No requiere migracion.';
END
GO

-- Paso 2: Copiar datos si la columna temporal existe
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('profesores') 
    AND name = 'ced_profesional_rfc'
)
BEGIN
    UPDATE profesores SET ced_profesional_rfc = CAST(ced_profesional AS NVARCHAR(13));
    PRINT 'Datos copiados a columna temporal.';
END
GO

-- Paso 3: Eliminar columna INT original si la temporal ya existe
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('profesores') 
    AND name = 'ced_profesional_rfc'
)
BEGIN
    ALTER TABLE profesores DROP COLUMN ced_profesional;
    PRINT 'Columna INT ced_profesional eliminada.';
END
GO

-- Paso 4: Renombrar la columna temporal al nombre original
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('profesores') 
    AND name = 'ced_profesional_rfc'
)
BEGIN
    EXEC sp_rename 'profesores.ced_profesional_rfc', 'ced_profesional', 'COLUMN';
    PRINT '✅ Migracion completada. ced_profesional ahora es NVARCHAR(13) (RFC).';
    PRINT '   IMPORTANTE: Actualiza los valores con el RFC real de cada profesor.';
END
GO

-- Verificacion final
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'profesores' AND COLUMN_NAME = 'ced_profesional';
GO
