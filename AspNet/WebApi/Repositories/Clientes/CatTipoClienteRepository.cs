using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WebApi.Models;

public class CatTipoClienteRepository : ICatTipoClienteRepository
{
    private readonly string _connectionString;

    public CatTipoClienteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IEnumerable<CatTipoCliente> GetAll()
    {
        List<CatTipoCliente> tiposCliente = new List<CatTipoCliente>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                using (SqlCommand command = new SqlCommand("GetAlltiposCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tiposCliente.Add(new CatTipoCliente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TipoCliente = Convert.ToString(reader["TipoCliente"])
                            });
                        }
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        return tiposCliente;
    }

    public CatTipoCliente GetById(int id)
    {
        CatTipoCliente tipoCliente = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            try
            {
                using (SqlCommand command = new SqlCommand("GetTipoClienteById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipoCliente = new CatTipoCliente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TipoCliente = Convert.ToString(reader["TipoCliente"])
                            };
                        }
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        return tipoCliente;
    }
}
