using Microsoft.Data.SqlClient;

public class Conexion
{
    public static SqlConnection ObtenerConexion()
    {
        string connectionString = "Server=GAM;Database=SistemaVentasPF1;Trusted_Connection=True;TrustServerCertificate=True;";
        return new SqlConnection(connectionString);
    }
}