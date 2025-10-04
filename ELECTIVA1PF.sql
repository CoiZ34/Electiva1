/**

CREATE TABLE Productos (
    IdProducto INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Categoria NVARCHAR(50) NULL
);

CREATE TABLE Clientes (
    IdCliente INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NULL
);

CREATE TABLE Facturas (
    IdFactura INT PRIMARY KEY,
    NumeroFactura NVARCHAR(50) NOT NULL,
    Fecha DATE NOT NULL,
    IdCliente INT NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente)
);

CREATE TABLE Ventas (
    IdVenta INT PRIMARY KEY,
    IdFactura INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Total AS (Cantidad * Precio) PERSISTED,
    FOREIGN KEY (IdFactura) REFERENCES Facturas(IdFactura),
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

CREATE TABLE FuentesDatos (
    IdFuente INT PRIMARY KEY IDENTITY(1,1),
    NombreFuente NVARCHAR(100) NOT NULL,
    TipoFuente NVARCHAR(50) NOT NULL,
    FechaCarga DATETIME DEFAULT GETDATE()
);
**/