



/** USE SistemaVentasPF1;
GO

CREATE TABLE dbo.Productos(
  IdProducto INT NOT NULL PRIMARY KEY,
  Nombre     VARCHAR(200) NOT NULL,
  Precio     DECIMAL(18,2) NOT NULL,
  Categoria  VARCHAR(100) NULL
);

CREATE TABLE dbo.Clientes(
  IdCliente INT NOT NULL PRIMARY KEY,
  Nombre    VARCHAR(200) NOT NULL,
  Email     VARCHAR(200) NULL,
  Telefono  VARCHAR(50)  NULL
);

CREATE TABLE dbo.FuentesDatos(
  IdFuente    INT NOT NULL PRIMARY KEY,
  Tipo        VARCHAR(50)  NOT NULL,   -- Ej: CSV, API, DB
  Descripcion VARCHAR(200) NULL,
  Url         VARCHAR(300) NULL
);

CREATE TABLE dbo.Facturas(
  IdFactura     INT NOT NULL PRIMARY KEY,
  NumeroFactura VARCHAR(50) NOT NULL,
  Fecha         DATE NOT NULL,
  IdCliente     INT NOT NULL
    FOREIGN KEY REFERENCES dbo.Clientes(IdCliente),
  IdFuente      INT NULL
    FOREIGN KEY REFERENCES dbo.FuentesDatos(IdFuente)
);
CREATE UNIQUE INDEX UX_Facturas_Numero ON dbo.Facturas(NumeroFactura);


CREATE TABLE dbo.Ventas(
  IdVenta    INT NOT NULL PRIMARY KEY,
  IdFactura  INT NOT NULL FOREIGN KEY REFERENCES dbo.Facturas(IdFactura),
  IdProducto INT NOT NULL FOREIGN KEY REFERENCES dbo.Productos(IdProducto),
  Cantidad   INT NOT NULL,
  Precio     DECIMAL(18,2) NOT NULL
);

USE SistemaVentasPF1;
SELECT 'Productos' AS Tabla, COUNT(*) AS Registros FROM dbo.Productos;
SELECT 'Clientes'  AS Tabla, COUNT(*) AS Registros FROM dbo.Clientes;
SELECT 'Facturas'  AS Tabla, COUNT(*) AS Registros FROM dbo.Facturas;
SELECT 'Ventas'    AS Tabla, COUNT(*) AS Registros FROM dbo.Ventas;
SELECT 'FuentesDatos' AS Tabla, COUNT(*) AS Registros FROM dbo.FuentesDatos;


USE SistemaVentasPF1;
GO
SELECT TOP (10) * FROM dbo.Productos;
SELECT TOP (10) * FROM dbo.Clientes;
SELECT TOP (10) * FROM dbo.Facturas;
SELECT TOP (10) * FROM dbo.Ventas;
SELECT TOP (10) * FROM dbo.FuentesDatos; **/