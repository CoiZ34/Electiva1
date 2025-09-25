public class Venta
{
    public int IdVenta { get; set; }
    public int IdFactura { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }


    public decimal Total
    {
        get { return Cantidad * Precio; }
    }
}