using System;
using Microsoft.Data.SqlClient;
using System.IO;


public class ETL
{
   
    public void CargarProductos(string archivo)
    {
        foreach (var linea in File.ReadAllLines(archivo))
        {
            if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("Id")) continue;

            var datos = linea.Split(',');
        Producto p = new Producto
            {
            IdProducto = int.Parse(datos[0]),
                Nombre = datos[1],
                Precio = decimal.Parse(datos[2]),
                Categoria = datos[3]
            };

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
             conn.Open();
             string sql = "INSERT INTO Productos (IdProducto, Nombre, Precio, Categoria) VALUES (@id,@nombre,@precio,@categoria)";
             SqlCommand cmd = new SqlCommand(sql, conn);
             cmd.Parameters.AddWithValue("@id", p.IdProducto);
                cmd.Parameters.AddWithValue("@nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@precio", p.Precio);
                cmd.Parameters.AddWithValue("@categoria", p.Categoria);
                cmd.ExecuteNonQuery();
            }
        }
    }

  
    public void CargarClientes(string archivo)
    {
        foreach (var linea in File.ReadAllLines(archivo))
        {
            if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("Id")) continue;

            var datos = linea.Split(',');
            Cliente c = new Cliente
            {
                IdCliente = int.Parse(datos[0]),
                Nombre = datos[1],
                Email = datos[2],
                Telefono = datos[3]
            };

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string sql = "INSERT INTO Clientes (IdCliente, Nombre, Email, Telefono) VALUES (@id,@nombre,@correo,@telefono)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", c.IdCliente);

                cmd.Parameters.AddWithValue("@nombre", c.Nombre);

                cmd.Parameters.AddWithValue("@correo", c.Email);

                cmd.Parameters.AddWithValue("@telefono", c.Telefono);
                
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void CargarFacturasYVentas(string archivo)
    {
        foreach (var linea in File.ReadAllLines(archivo))
        {
            if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("Id")) continue;

            var datos = linea.Split(',');
            Factura f = new Factura
            {
                IdFactura = int.Parse(datos[0]),
                NumeroFactura = datos[1],
                Fecha = DateTime.Parse(datos[2]),
                IdCliente = int.Parse(datos[3])
            };

            Venta v = new Venta
            {
                IdVenta = int.Parse(datos[4]),
                IdFactura = f.IdFactura,
                IdProducto = int.Parse(datos[5]),
                Cantidad = int.Parse(datos[6]),
                Precio = decimal.Parse(datos[7])
            };

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();


                string sqlFactura = "IF NOT EXISTS (SELECT 1 FROM Facturas WHERE IdFactura=@idFactura) " +
                                    "INSERT INTO Facturas (IdFactura, NumeroFactura, Fecha, IdCliente) VALUES (@idFactura,@num,@fecha,@idCliente)";
                SqlCommand cmdFactura = new SqlCommand(sqlFactura, conn);

                cmdFactura.Parameters.AddWithValue("@idFactura", f.IdFactura);

                cmdFactura.Parameters.AddWithValue("@num", f.NumeroFactura);

                cmdFactura.Parameters.AddWithValue("@fecha", f.Fecha);

                cmdFactura.Parameters.AddWithValue("@idCliente", f.IdCliente);

                cmdFactura.ExecuteNonQuery();

              
                string sqlVenta = "INSERT INTO Ventas (IdVenta, IdFactura, IdProducto, Cantidad, Precio) " +
            "VALUES (@idVenta,@idFactura,@idProducto,@cantidad,@precio)";
                SqlCommand cmdVenta = new SqlCommand(sqlVenta, conn);

                cmdVenta.Parameters.AddWithValue("@idVenta", v.IdVenta);

                cmdVenta.Parameters.AddWithValue("@idFactura", v.IdFactura);

                cmdVenta.Parameters.AddWithValue("@idProducto", v.IdProducto);

                cmdVenta.Parameters.AddWithValue("@cantidad", v.Cantidad);

                cmdVenta.Parameters.AddWithValue("@precio", v.Precio);

                cmdVenta.ExecuteNonQuery();
            }
        }
    }
}