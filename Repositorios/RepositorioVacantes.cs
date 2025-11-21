using GestionCandidatosAPI.Modelos;
using Microsoft.Data.SqlClient;

namespace GestionCandidatosAPI.Repositorios
{
    public class RepositorioVacantes : IRepositorioVacantes
    {
        private readonly string _cadenaSql;

        public RepositorioVacantes(IConfiguration config)
        {
            _cadenaSql = config.GetConnectionString("CadenaSQL")
                                ?? throw new Exception("Falta la cadena de conexión en appsettings");
        }

        public async Task<IEnumerable<Vacante>> ObtenerVacantes()
        {
            var lista = new List<Vacante>();

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Vacantes", conexion);

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(MapearVacante(dr));
            }

            return lista;
        }

        // GET POR ID

        public async Task<Vacante?> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Vacantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = await cmd.ExecuteReaderAsync();
            if (!await dr.ReadAsync())
                return null;

            return MapearVacante(dr);
        }

        // Insertar y Regresa la entidad con ID
        public async Task<Vacante> InsertarVacante(Vacante vacante)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                INSERT INTO Vacantes (Titulo, Descripcion, Departamento, Salario, Activa, FechaPublicacion)
                OUTPUT INSERTED.Id
                VALUES (@titulo, @desc, @dep, @sal, @activa, @fecha)";

            var cmd = new SqlCommand(query, conexion);

            cmd.Parameters.AddWithValue("@titulo", vacante.Titulo);
            cmd.Parameters.AddWithValue("@desc", vacante.Descripcion);
            cmd.Parameters.AddWithValue("@dep", vacante.Departamento);
            cmd.Parameters.AddWithValue("@sal", vacante.Salario);
            cmd.Parameters.AddWithValue("@activa", vacante.Activa);
            cmd.Parameters.AddWithValue("@fecha", vacante.FechaPublicacion);

            var newId = await cmd.ExecuteScalarAsync();
            vacante.Id = Convert.ToInt32(newId);

            return vacante;
        }

        // ACTUALIZAR → Regresa la entidad actualizada o null
        public async Task<Vacante?> ActualizarVacante(int id, Vacante vacante)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                UPDATE Vacantes
                SET Titulo = @titulo,
                    Descripcion = @desc,
                    Departamento = @dep,
                    Salario = @sal,
                    Activa = @activa
                WHERE Id = @id";

            var cmd = new SqlCommand(query, conexion);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@titulo", vacante.Titulo);
            cmd.Parameters.AddWithValue("@desc", vacante.Descripcion);
            cmd.Parameters.AddWithValue("@dep", vacante.Departamento);
            cmd.Parameters.AddWithValue("@sal", vacante.Salario);
            cmd.Parameters.AddWithValue("@activa", vacante.Activa);

            var filas = await cmd.ExecuteNonQueryAsync();

            if (filas == 0)
                return null;

            vacante.Id = id;
            return vacante;
        }

        // ELIMINAR → Regresa la entidad eliminada o null
        public async Task<Vacante?> EliminarVacante(int id)
        {
            // 1. Verificar si existe
            var existente = await ObtenerPorId(id);
            if (existente is null)
                return null;

            // 2. Eliminarla
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("DELETE FROM Vacantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();

            return existente;
        }

        // EXISTS
        public async Task<bool> Existe(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT 1 FROM Vacantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }

        // MÉTODO AUXILIAR PARA MAPEAR
        private Vacante MapearVacante(SqlDataReader dr)
        {
            return new Vacante
            {
                Id = Convert.ToInt32(dr["Id"]),
                Titulo = dr["Titulo"].ToString()!,
                Descripcion = dr["Descripcion"].ToString()!,
                Departamento = dr["Departamento"].ToString()!,
                Salario = dr["Salario"] != DBNull.Value ? Convert.ToDecimal(dr["Salario"]) : 0,
                Activa = Convert.ToBoolean(dr["Activa"]),
                FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"])
            };
        }
    }
}
