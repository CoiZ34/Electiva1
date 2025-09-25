public class Venta
{
    public int IdVenta { get; set; }
    public int IdFactura { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }

    // Campo derivado (no se guarda, pero se calcula)
    public decimal Total
    {
        get { return Cantidad * Precio; }
    }
}