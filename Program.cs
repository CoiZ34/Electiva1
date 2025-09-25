using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Inicializando ETL de Hansel!");

        ETL etl = new ETL();

        etl.CargarProductos("productos.csv");

        etl.CargarClientes("clientes.csv");
        
        etl.CargarFacturasYVentas("ventas.csv");

        Console.WriteLine("Proceso ETL realizado con exito.");
    }
}