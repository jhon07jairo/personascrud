CREATE DATABASE Personas;
GO

USE Personas;
GO

CREATE TABLE Persona (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Apellidos NVARCHAR(50),
    Documento NVARCHAR(20) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) CHECK (Sexo IN ('M', 'F')) NOT NULL
);
GO

ALTER TABLE Persona
DROP CONSTRAINT UQ__Persona__AF73706D41789839;

-- Convertir la columna Documento a BIGINT (o INT si es más adecuado)
ALTER TABLE Persona
ALTER COLUMN Documento INT NOT NULL;

-- Volver a agregar la restricción UNIQUE
ALTER TABLE Persona
ADD CONSTRAINT UQ_Documento UNIQUE (Documento);

-- 4. Inserción de cinco personas
INSERT INTO Persona (Nombre, Apellidos, Documento, FechaNacimiento, Sexo)
VALUES 
('Jhon', 'López', '123456789', '1997-11-07', 'M'),
('Lionel', 'Messi', '987654321', '1985-05-15', 'M'),
('Cristiano', 'Dos Santos Aveiro', '456123789', '1992-07-20', 'M'),
('Linda', 'Caicedo', '789456123', '1999-12-30', 'F'),
('James', 'Rodriguez', '321654987', '1982-09-25', 'M');
GO

-- 5. Creación de una tabla de países
CREATE TABLE Pais (
    CodigoPais CHAR(3) PRIMARY KEY,
    NombrePais NVARCHAR(50) NOT NULL,
    PorDefecto BIT NOT NULL CHECK (PorDefecto IN (0, 1))
);
GO

-- 6. Restricciones ya implementadas:
-- - Clave primaria en CodigoPais
-- - Valores no nulos en NombrePais y PorDefecto
-- - Restricción CHECK en PorDefecto (0 para falso, 1 para verdadero)

-- 7. Inserción de cuatro países
INSERT INTO Pais (CodigoPais, NombrePais, PorDefecto)
VALUES
('COL', 'Colombia', 1),
('POR', 'Portugal', 0),
('MEX', 'México', 0),
('ARG', 'Argentina', 0);
GO

-- 8. Modificación de la tabla de personas para incluir el país
ALTER TABLE Persona
ADD CodigoPais CHAR(3) FOREIGN KEY REFERENCES Pais(CodigoPais);
GO

-- 9. Actualización del país de las personas
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '123456789';
UPDATE Persona SET CodigoPais = 'ARG' WHERE Documento = '987654321';
UPDATE Persona SET CodigoPais = 'POR' WHERE Documento = '456123789';
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '789456123';
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '321654987';
GO

-- 10. Creación de una función para contar personas por país
CREATE FUNCTION ContarPersonasPorPais(@CodigoPais CHAR(3))
RETURNS INT
AS
BEGIN
    DECLARE @Cantidad INT;
    SELECT @Cantidad = COUNT(*) FROM Persona WHERE CodigoPais = @CodigoPais;
    RETURN @Cantidad;
END;
GO

-- 11. Creación de una vista para personas del país por defecto
CREATE VIEW VistaPersonasPorDefecto AS
SELECT 
    Nombre, 
    Apellidos, 
    Documento, 
    Sexo, 
    FORMAT(FechaNacimiento, 'dd-MM-yyyy') AS FechaNacimiento
FROM 
    Persona
WHERE 
    CodigoPais = (SELECT CodigoPais FROM Pais WHERE PorDefecto = 1);
GO

-- 12. Creación de una secuencia
CREATE SEQUENCE SecuenciaDocumento
    START WITH 100000
    INCREMENT BY 1;
GO

-- 13. Construcción de un ciclo WHILE para insertar 1,000 personas
DECLARE @i INT = 1;
DECLARE @Documento NVARCHAR(20);

WHILE @i <= 1000
BEGIN
    SET @Documento = CAST(NEXT VALUE FOR SecuenciaDocumento AS NVARCHAR(20));
    INSERT INTO Persona (Nombre, Apellidos, Documento, FechaNacimiento, Sexo, CodigoPais)
    VALUES ('Nombre' + CAST(@i AS NVARCHAR(10)), 'Apellido' + CAST(@i AS NVARCHAR(10)), @Documento, '2000-01-01', 'M', 'COL');
    SET @i = @i + 1;
END;
GO

-- 14. Creación de un procedimiento almacenado
CREATE PROCEDURE ActualizarNombrePorEdad
    @NombreNuevo NVARCHAR(50),
    @Edad INT
AS
BEGIN
    IF @NombreNuevo IS NULL OR @Edad IS NULL
    BEGIN
        RAISERROR ('Parámetros no informados correctamente.', 16, 1);
        RETURN;
    END

    UPDATE Persona
    SET Nombre = @NombreNuevo
    WHERE DATEDIFF(YEAR, FechaNacimiento, GETDATE()) > @Edad;
END;
GO

--delete from Persona where ID > 5;
 