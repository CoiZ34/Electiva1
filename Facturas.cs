using System;

// Clase Factura (representa la tabla Facturas)
public class Factura
{
    public int IdFactura { get; set; }
    public string NumeroFactura { get; set; }
    public DateTime Fecha { get; set; }
    public int IdCliente { get; set; }
}