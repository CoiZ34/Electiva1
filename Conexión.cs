using Microsoft.Data.SqlClient;
// Clase para manejar la conexión a la base de datos
public class Conexion
{
    public static SqlConnection ObtenerConexion()
    {
        string connectionString = "Server=GAM;Database=SistemaVentasPF1;Trusted_Connection=True;TrustServerCertificate=True;";
        return new SqlConnection(connectionString);
    }
}