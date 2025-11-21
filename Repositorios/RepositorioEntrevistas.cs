using GestionCandidatosAPI.Modelos;
using Microsoft.Data.SqlClient;

namespace GestionCandidatosAPI.Repositorios
{
    public class RepositorioEntrevistas : IRepositorioEntrevistas
    {
        private readonly string _cadenaSql;

        public RepositorioEntrevistas(IConfiguration config)
        {
            _cadenaSql = config.GetConnectionString("CadenaSQL")
                               ?? throw new Exception("Falta la cadena de conexión en appsettings");
        }

        // POST Agendar entrevista (devuelve ID insertado)
        public async Task<int> AgendarAsync(Entrevista entrevista)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                INSERT INTO Entrevistas (PostulanteId, FechaEntrevista, Notas, Realizada)
                OUTPUT INSERTED.Id
                VALUES (@pid, @fecha, @notas, 0)";

            using var cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@pid", entrevista.PostulanteId);
            cmd.Parameters.AddWithValue("@fecha", entrevista.FechaEntrevista);
            cmd.Parameters.AddWithValue("@notas", entrevista.Notas ?? (object)DBNull.Value);

            var idGenerado = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(idGenerado);
        }

        // GET Obtener entrevista por ID
        public async Task<Entrevista?> ObtenerPorIdAsync(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = "SELECT * FROM Entrevistas WHERE Id = @id";
            using var cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return new Entrevista
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    PostulanteId = Convert.ToInt32(dr["PostulanteId"]),
                    FechaEntrevista = Convert.ToDateTime(dr["FechaEntrevista"]),
                    Notas = dr["Notas"]?.ToString() ?? string.Empty,
                    Realizada = Convert.ToBoolean(dr["Realizada"])
                };
            }

            return null;
        }

        // GET Listar entrevistas pendientes
        public async Task<IEnumerable<dynamic>> ListarPendientesAsync()
        {
            var lista = new List<dynamic>();

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                SELECT e.Id, e.FechaEntrevista, p.NombreCompleto, v.Titulo AS Puesto
                FROM Entrevistas e
                INNER JOIN Postulantes p ON e.PostulanteId = p.Id
                INNER JOIN Vacantes v ON p.VacanteId = v.Id
                WHERE e.Realizada = 0";

            using var cmd = new SqlCommand(query, conexion);
            using var dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Fecha = Convert.ToDateTime(dr["FechaEntrevista"]),
                    Candidato = dr["NombreCompleto"].ToString(),
                    Puesto = dr["Puesto"].ToString()
                });
            }

            return lista;
        }

        // PUT Actualizar entrevista
        public async Task<Entrevista?> ActualizarAsync(int id, Entrevista entrevista)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                UPDATE Entrevistas
                SET PostulanteId = @pid,
                    FechaEntrevista = @fecha,
                    Notas = @notas,
                    Realizada = @realizada
                WHERE Id = @id";

            using var cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@pid", entrevista.PostulanteId);
            cmd.Parameters.AddWithValue("@fecha", entrevista.FechaEntrevista);
            cmd.Parameters.AddWithValue("@notas", entrevista.Notas ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@realizada", entrevista.Realizada);
            cmd.Parameters.AddWithValue("@id", id);

            var filas = await cmd.ExecuteNonQueryAsync();
            return filas > 0 ? entrevista : null;
        }

        // DELETE Eliminar entrevista
        public async Task<Entrevista?> EliminarAsync(int id)
        {
            var entrevista = await ObtenerPorIdAsync(id);
            if (entrevista == null)
                return null;

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = "DELETE FROM Entrevistas WHERE Id = @id";
            using var cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
            return entrevista;
        }

        // Verificar existencia
        public async Task<bool> ExisteAsync(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = "SELECT COUNT(1) FROM Entrevistas WHERE Id = @id";
            using var cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@id", id);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }
    }
}
