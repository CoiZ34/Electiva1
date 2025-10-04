using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Comienza mi ETL Hansel P");
    ETL etl = new ETL();

        etl.LoadCustomers("Datos/customers.csv");
etl.LoadProducts("Datos/products.csv");
etl.LoadOrders("Datos/orders.csv");
etl.LoadOrderDetails("Datos/order_details.csv");

        Console.WriteLine("Se termino el ETL con exito.");
    }
}