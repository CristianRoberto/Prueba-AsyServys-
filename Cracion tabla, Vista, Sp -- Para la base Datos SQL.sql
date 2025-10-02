-- =========================================
-- CREAR BASE DE DATOS
-- =========================================
CREATE DATABASE ControlTemperaturasDB;
GO

USE ControlTemperaturasDB;
GO

-- =========================================
-- TABLA ENCABEZADO DEL CONTROL
-- =========================================
CREATE TABLE ControlEncabezado (
    IdControl INT IDENTITY PRIMARY KEY,
    Destino NVARCHAR(120) NOT NULL,
    FechaDescongelacion DATE NOT NULL,
    FechaProduccion DATE NOT NULL,
    RealizadoPor NVARCHAR(120) NULL,
    RevisadoPor NVARCHAR(120) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- =========================================
-- TABLA DETALLE POR COCHE/PRODUCTO
-- =========================================
CREATE TABLE ControlDetalle (
    IdDetalle INT IDENTITY PRIMARY KEY,
    IdControl INT NOT NULL FOREIGN KEY REFERENCES ControlEncabezado(IdControl),
    NumeroCoche INT NOT NULL,
    CodigoProducto NVARCHAR(50) NOT NULL,
    HoraInicioDescongelado TIME NULL,
    TemperaturaProducto DECIMAL(5,2) NULL,
    HoraInicioConsumo TIME NULL,
    HoraFinConsumo TIME NULL,
    Observaciones NVARCHAR(250) NULL
);

-- =========================================
-- VISTA DE REPORTES (ENCABEZADO + DETALLES)
-- =========================================
CREATE VIEW Vista_ControlTemperaturas AS
SELECT 
    e.IdControl,
    e.Destino,
    e.FechaDescongelacion,
    e.FechaProduccion,
    d.NumeroCoche,
    d.CodigoProducto,
    d.HoraInicioDescongelado,
    d.TemperaturaProducto,
    d.HoraInicioConsumo,
    d.HoraFinConsumo,
    d.Observaciones,
    e.RealizadoPor,
    e.RevisadoPor,
    e.FechaRegistro
FROM ControlEncabezado e
INNER JOIN ControlDetalle d ON d.IdControl = e.IdControl;
GO

-- =========================================
-- PROCEDIMIENTO: CREAR ENCABEZADO
-- =========================================
CREATE PROCEDURE sp_CrearEncabezado
    @Destino NVARCHAR(120),
    @FechaDescongelacion DATE,
    @FechaProduccion DATE,
    @RealizadoPor NVARCHAR(120),
    @RevisadoPor NVARCHAR(120)
AS
BEGIN
    INSERT INTO ControlEncabezado (Destino, FechaDescongelacion, FechaProduccion, RealizadoPor, RevisadoPor)
    VALUES (@Destino, @FechaDescongelacion, @FechaProduccion, @RealizadoPor, @RevisadoPor);

    SELECT SCOPE_IDENTITY() AS IdNuevoControl;
END;
GO

-- =========================================
-- PROCEDIMIENTO: AGREGAR DETALLE
-- =========================================
CREATE PROCEDURE sp_AgregarDetalle
    @IdControl INT,
    @NumeroCoche INT,
    @CodigoProducto NVARCHAR(50),
    @HoraInicioDescongelado TIME,
    @TemperaturaProducto DECIMAL(5,2),
    @HoraInicioConsumo TIME,
    @HoraFinConsumo TIME,
    @Observaciones NVARCHAR(250)
AS
BEGIN
    INSERT INTO ControlDetalle
    (IdControl, NumeroCoche, CodigoProducto, HoraInicioDescongelado, TemperaturaProducto, HoraInicioConsumo, HoraFinConsumo, Observaciones)
    VALUES (@IdControl, @NumeroCoche, @CodigoProducto, @HoraInicioDescongelado, @TemperaturaProducto, @HoraInicioConsumo, @HoraFinConsumo, @Observaciones);
END;
GO

-- =========================================
-- PROCEDIMIENTO: LISTAR POR RANGO DE FECHAS
-- =========================================
CREATE PROCEDURE sp_ListarControles
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SELECT * 
    FROM Vista_ControlTemperaturas
    WHERE FechaProduccion BETWEEN @Desde AND @Hasta
    ORDER BY FechaProduccion DESC;
END;
GO

-- =========================================
-- PROCEDIMIENTO: OBTENER CONTROL POR ID
-- =========================================
CREATE PROCEDURE sp_ObtenerControlPorId
    @IdControl INT
AS
BEGIN
    SELECT * FROM Vista_ControlTemperaturas WHERE IdControl = @IdControl;
END;
GO


INSERT INTO ControlEncabezado (Destino, FechaDescongelacion, FechaProduccion, RealizadoPor, RevisadoPor)
VALUES 
('Planta Manta', '2025-09-30', '2025-10-01', 'Juan Pérez', 'María López'),
('Planta Rocafuerte', '2025-09-29', '2025-09-30', 'Carlos Andrade', 'Ana Torres'),
('Exportación Quito', '2025-09-28', '2025-09-29', 'Pedro Ramírez', 'José Herrera');


-- Detalles para el primer control (IdControl = 1)
INSERT INTO ControlDetalle (IdControl, NumeroCoche, CodigoProducto, HoraInicioDescongelado, TemperaturaProducto, HoraInicioConsumo, HoraFinConsumo, Observaciones)
VALUES
(11, 101, 'PROD-001', '06:00', 4.5, '07:00', '09:00', 'Sin novedades'),
(11, 102, 'PROD-002', '06:30', 5.0, '07:30', '09:30', 'Ligera variación de temperatura');

-- Detalles para el segundo control (IdControl = 2)
INSERT INTO ControlDetalle (IdControl, NumeroCoche, CodigoProducto, HoraInicioDescongelado, TemperaturaProducto, HoraInicioConsumo, HoraFinConsumo, Observaciones)
VALUES
(12, 201, 'PROD-003', '05:45', 3.8, '06:30', '08:30', 'Todo en rango'),
(12, 202, 'PROD-004', '06:15', 4.1, '07:15', '09:15', 'Producto con ligera humedad');

-- Detalles para el tercer control (IdControl = 3)
INSERT INTO ControlDetalle (IdControl, NumeroCoche, CodigoProducto, HoraInicioDescongelado, TemperaturaProducto, HoraInicioConsumo, HoraFinConsumo, Observaciones)
VALUES
(13, 301, 'PROD-005', '04:50', 3.5, '05:40', '07:40', 'Correcto'),
(3, 302, 'PROD-006', '05:10', 3.9, '06:00', '08:00', 'Revisar embalaje');


delete from ControlDetalle;
delete from ControlEncabezado;





SELECT * FROM Vista_ControlTemperaturas;
SELECT * FROM ControlDetalle;
SELECT * FROM ControlEncabezado;


