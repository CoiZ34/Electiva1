using System;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;

public class ETL
{

    private string Clean(string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    private bool IsHeader(string line, string startsWith) =>
        !string.IsNullOrWhiteSpace(line) && line.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase);

    private DateTime? ParseDate(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (DateTime.TryParse(s, out var dt)) return dt.Date;
        if (double.TryParse(s, out var serial)) return DateTime.FromOADate(serial).Date;
        return null;
    }

    private bool ParseDec(string s, out decimal v)
    {
        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return true;
        return decimal.TryParse(s, out v);
    }


    public void LoadCustomers(string file)
    {
        foreach (var line in File.ReadLines(file))
        {
            if (string.IsNullOrWhiteSpace(line) || IsHeader(line, "CustomerID")) continue;

            var d = line.Split(',');
            if (d.Length < 6) continue;

            if (!int.TryParse(d[0].Trim(), out int id)) continue;
            string first   = Clean(d[1]);
            string last    = Clean(d[2]);
            string email   = Clean(d[3]);
            string city    = Clean(d[4]);
            string country = Clean(d[5]);
            if (first == null || last == null) continue;

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string sql = "IF NOT EXISTS (SELECT 1 FROM Customers WHERE CustomerID=@id) " +
                             "INSERT INTO Customers (CustomerID, FirstName, LastName, Email, City, Country) " +
                             "VALUES (@id, @f, @l, @e, @c, @co)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@f", first);
                cmd.Parameters.AddWithValue("@l", last);
                cmd.Parameters.AddWithValue("@e", (object?)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@c", (object?)city ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@co", (object?)country ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
    }


    public void LoadProducts(string file)
    {
        foreach (var line in File.ReadLines(file))
        {
            if (string.IsNullOrWhiteSpace(line) || IsHeader(line, "ProductID")) continue;

            var d = line.Split(',');
            if (d.Length < 4) continue;

            if (!int.TryParse(d[0].Trim(), out int id)) continue;
            string name = Clean(d[1]);
            string category = Clean(d[2]);
            if (!ParseDec(Clean(d[3]) ?? "", out decimal price)) continue;
            if (name == null || price < 0) continue;

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string sql = "IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductID=@id) " +
                             "INSERT INTO Products (ProductID, ProductName, Category, Price) " +
                             "VALUES (@id, @n, @c, @p)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@c", (object?)category ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p", price);
                cmd.ExecuteNonQuery();
            }
        }
    }


    public void LoadOrders(string file)
    {
        using var lookup = Conexion.ObtenerConexion();
        lookup.Open();

        foreach (var line in File.ReadLines(file))
        {
            if (string.IsNullOrWhiteSpace(line) || IsHeader(line, "OrderID")) continue;

            var d = line.Split(',');
            if (d.Length < 4) continue;

            if (!int.TryParse(d[0].Trim(), out int orderId)) continue;
            if (!int.TryParse(d[1].Trim(), out int customerId)) continue;
            var orderDate = ParseDate(Clean(d[2]) ?? "");
            string status = Clean(d[3]);
            if (orderDate == null) continue;


            using (var chk = new SqlCommand("SELECT COUNT(1) FROM Customers WHERE CustomerID=@c", lookup))
            {
                chk.Parameters.AddWithValue("@c", customerId);
                if ((int)chk.ExecuteScalar() == 0) continue;
            }

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string sql = "IF NOT EXISTS (SELECT 1 FROM Orders WHERE OrderID=@id) " +
                             "INSERT INTO Orders (OrderID, CustomerID, OrderDate, Status) " +
                             "VALUES (@id, @cid, @dt, @st)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.Parameters.AddWithValue("@cid", customerId);
                cmd.Parameters.AddWithValue("@dt", orderDate.Value);
                cmd.Parameters.AddWithValue("@st", (object?)status ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
    }


    public void LoadOrderDetails(string file)
    {
        using var lookup = Conexion.ObtenerConexion();
        lookup.Open();

        foreach (var line in File.ReadLines(file))
        {
            if (string.IsNullOrWhiteSpace(line) || IsHeader(line, "OrderID")) continue;

            var d = line.Split(',');
            if (d.Length < 4) continue;

            if (!int.TryParse(d[0].Trim(), out int orderId)) continue;
            if (!int.TryParse(d[1].Trim(), out int productId)) continue;
            if (!int.TryParse(d[2].Trim(), out int qty) || qty <= 0) continue;

            decimal unitPrice = 0m, totalPrice = 0m;
            bool hasUnit  = ParseDec(Clean(d[3]) ?? "", out unitPrice);
            bool hasTotal = d.Length >= 5 && ParseDec(Clean(d[4]) ?? "", out totalPrice);

            if (!hasTotal && hasUnit) totalPrice = unitPrice * qty;
            if (!hasUnit  && hasTotal) unitPrice  = qty == 0 ? 0 : totalPrice / qty;
            if (!hasUnit && !hasTotal) continue;
            if (totalPrice < 0) continue;


            using (var c1 = new SqlCommand("SELECT COUNT(1) FROM Orders WHERE OrderID=@o", lookup))
            {
                c1.Parameters.AddWithValue("@o", orderId);
                if ((int)c1.ExecuteScalar() == 0) continue;
            }
            using (var c2 = new SqlCommand("SELECT COUNT(1) FROM Products WHERE ProductID=@p", lookup))
            {
                c2.Parameters.AddWithValue("@p", productId);
                if ((int)c2.ExecuteScalar() == 0) continue;
            }

            using (SqlConnection conn = Conexion.ObtenerConexion())
            {
                conn.Open();
                string sql = "IF NOT EXISTS (SELECT 1 FROM OrderDetails WHERE OrderID=@o AND ProductID=@p) " +
                             "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, TotalPrice) " +
                             "VALUES (@o, @p, @q, @u, @t)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@o", orderId);
                cmd.Parameters.AddWithValue("@p", productId);
                cmd.Parameters.AddWithValue("@q", qty);
                cmd.Parameters.AddWithValue("@u", unitPrice);
                cmd.Parameters.AddWithValue("@t", totalPrice);
                cmd.ExecuteNonQuery();
            }
        }
    }
}