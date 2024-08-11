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

--Cambio de Documento NVARCHAR a INT
ALTER TABLE Persona
DROP CONSTRAINT UQ__Persona__AF73706D41789839;

ALTER TABLE Persona
ALTER COLUMN Documento INT NOT NULL;

ALTER TABLE Persona
ADD CONSTRAINT UQ_Documento UNIQUE (Documento);

-- Inserción de cinco personas
INSERT INTO Persona (Nombre, Apellidos, Documento, FechaNacimiento, Sexo)
VALUES 
('Jhon', 'López', '123456789', '1997-11-07', 'M'),
('Lionel', 'Messi', '987654321', '1985-05-15', 'M'),
('Cristiano', 'Dos Santos Aveiro', '456123789', '1992-07-20', 'M'),
('Linda', 'Caicedo', '789456123', '1999-12-30', 'F'),
('James', 'Rodriguez', '321654987', '1982-09-25', 'M');
GO

-- Creación de una tabla de países
CREATE TABLE Pais (
    CodigoPais CHAR(3) PRIMARY KEY,
    NombrePais NVARCHAR(50) NOT NULL,
    PorDefecto BIT NOT NULL CHECK (PorDefecto IN (0, 1))
);
GO

-- Inserción de cuatro países
INSERT INTO Pais (CodigoPais, NombrePais, PorDefecto)
VALUES
('COL', 'Colombia', 1),
('POR', 'Portugal', 0),
('MEX', 'México', 0),
('ARG', 'Argentina', 0);
GO

-- Modificación de la tabla de personas para incluir el país
ALTER TABLE Persona
ADD CodigoPais CHAR(3) FOREIGN KEY REFERENCES Pais(CodigoPais);
GO

-- Actualización del país de las personas
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '123456789';
UPDATE Persona SET CodigoPais = 'ARG' WHERE Documento = '987654321';
UPDATE Persona SET CodigoPais = 'POR' WHERE Documento = '456123789';
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '789456123';
UPDATE Persona SET CodigoPais = 'COL' WHERE Documento = '321654987';
GO

-- Creación de una función para contar personas por país
CREATE FUNCTION ContarPersonasPorPais(@CodigoPais CHAR(3))
RETURNS INT
AS
BEGIN
    DECLARE @Cantidad INT;
    SELECT @Cantidad = COUNT(*) FROM Persona WHERE CodigoPais = @CodigoPais;
    RETURN @Cantidad;
END;
GO

--Ejecución de función ContarPersonasPorPais 
SELECT dbo.ContarPersonasPorPais('COL') AS NumeroDePersonas;

-- Creación de una vista para personas del país por defecto
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

-- Select de vista con formato de fecha ‘dd-mm-yyyy’
SELECT * FROM dbo.VistaPersonasPorDefecto;

-- Creación de una secuencia
CREATE SEQUENCE SecuenciaDocumento
    START WITH 100000
    INCREMENT BY 1;
GO

-- Construcción de un ciclo WHILE para insertar 1,000 personas
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

-- Creación del procedimiento almacenado
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

--Ejecución del sp

USE [Personas]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[ActualizarNombrePorEdad]
		@NombreNuevo = N'NombreNuevo',
		@Edad = 99

SELECT	'Return Value' = @return_value

GO



--delete from Persona where ID > 5;

