
-- 1. Crear la Base de Datos
CREATE DATABASE GestionCandidatosDB;
GO

USE GestionCandidatosDB;
GO

-- 2. Tabla VACANTES (La oferta de trabajo)
CREATE TABLE Vacantes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500),
    Departamento VARCHAR(50),
    Salario DECIMAL(10,2),
    Activa BIT DEFAULT 1, -- 1 = Disponible, 0 = Cerrada
    FechaPublicacion DATETIME DEFAULT GETDATE()
);

-- 3. Tabla POSTULANTES (Los alumnos)
CREATE TABLE Postulantes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto VARCHAR(100) NOT NULL,
    Correo VARCHAR(100),
    Telefono VARCHAR(20),
    CvUrl VARCHAR(200), 
    FechaPostulacion DATETIME DEFAULT GETDATE(),
    
    -- Relación: Un postulante aplica a UNA vacante
    VacanteId INT, 
    CONSTRAINT FK_Vacante_Postulante FOREIGN KEY (VacanteId) REFERENCES Vacantes(Id)
);

-- 4. Tabla ENTREVISTAS (El seguimiento)
CREATE TABLE Entrevistas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaEntrevista DATETIME NOT NULL,
    Notas VARCHAR(MAX), -- Comentarios del reclutador
    Realizada BIT DEFAULT 0, -- 0 = Pendiente, 1 = Ya pasó
    
    -- Relación: Una entrevista es para UN postulante
    PostulanteId INT NOT NULL, 
    CONSTRAINT FK_Postulante_Entrevista FOREIGN KEY (PostulanteId) REFERENCES Postulantes(Id)
);

-- 5. DATOS DE PRUEBA (Para que no empieces vacío)
INSERT INTO Vacantes (Titulo, Departamento, Salario) 
VALUES ('Desarrollador .NET Junior', 'Sistemas', 8000.00);

INSERT INTO Vacantes (Titulo, Departamento, Salario) 
VALUES ('Analista de Bases de Datos', 'Inteligencia de Negocios', 7500.00);