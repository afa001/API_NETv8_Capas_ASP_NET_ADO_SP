using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class TblClientes
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public int IdTipoCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string RFC { get; set; }

        [JsonIgnore]
        public CatTipoCliente? TipoCliente { get; set; }
    }
}
