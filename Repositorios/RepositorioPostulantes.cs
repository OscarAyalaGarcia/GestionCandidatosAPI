using GestionCandidatosAPI.Modelos;
using Microsoft.Data.SqlClient;

namespace GestionCandidatosAPI.Repositorios
{
    public class RepositorioPostulantes : IRepositorioPostulantes
    {
        private readonly string _cadenaSql;

        public RepositorioPostulantes(IConfiguration config)
        {
            _cadenaSql = config.GetConnectionString("CadenaSQL")
                                ?? throw new Exception("Falta la cadena de conexión en appsettings");
        }

        // GET Obtener TODOS los postulantes
       
        public async Task<IEnumerable<Postulante>> ObtenerPostulantes()
        {
            var lista = new List<Postulante>();

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Postulantes", conexion);

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(Mapear(dr));
            }

            return lista;
        }

        // GET → Obtener postulante por ID
        public async Task<Postulante?> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM Postulantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return Mapear(dr);
            }

            return null;
        }

        // POST Insertar postulante (retorna el ID generado)
        public async Task<int> InsertarPostulante(Postulante postulante)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                INSERT INTO Postulantes (NombreCompleto, Correo, Telefono, CvUrl, FechaPostulacion, VacanteId)
                OUTPUT INSERTED.Id
                VALUES (@nombre, @correo, @tel, @cv, GETDATE(), @vacante);";

            var cmd = new SqlCommand(query, conexion);

            cmd.Parameters.AddWithValue("@nombre", postulante.NombreCompleto);
            cmd.Parameters.AddWithValue("@correo", postulante.Correo);
            cmd.Parameters.AddWithValue("@tel", postulante.Telefono);
            cmd.Parameters.AddWithValue("@cv", postulante.CvUrl ?? "");
            cmd.Parameters.AddWithValue("@vacante", postulante.VacanteId);

            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        // GET → Listar postulantes por vacante
        public async Task<IEnumerable<Postulante>> ListarPorVacante(int vacanteId)
        {
            var lista = new List<Postulante>();

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand(
                "SELECT * FROM Postulantes WHERE VacanteId = @vid",
                conexion
            );

            cmd.Parameters.AddWithValue("@vid", vacanteId);

            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(Mapear(dr));
            }

            return lista;
        }

        // PUT  Actualizar postulante (retorna la entidad actualizada o null)
        public async Task<Postulante?> ActualizarPostulante(int id, Postulante postulante)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var query = @"
                UPDATE Postulantes
                SET NombreCompleto = @nombre,
                    Correo = @correo,
                    Telefono = @tel,
                    CvUrl = @cv,
                    VacanteId = @vacante
                WHERE Id = @id";

            var cmd = new SqlCommand(query, conexion);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nombre", postulante.NombreCompleto);
            cmd.Parameters.AddWithValue("@correo", postulante.Correo);
            cmd.Parameters.AddWithValue("@tel", postulante.Telefono);
            cmd.Parameters.AddWithValue("@cv", postulante.CvUrl ?? "");
            cmd.Parameters.AddWithValue("@vacante", postulante.VacanteId);

            var filas = await cmd.ExecuteNonQueryAsync();
            if (filas == 0)
                return null;

            postulante.Id = id;
            return postulante;
        }

        // DELETE  Eliminar postulante (retorna la entidad borrada o null)
        public async Task<Postulante?> EliminarPostulante(int id)
        {
            // Antes de borrar, recuperamos el registro
            var existente = await ObtenerPorId(id);
            if (existente is null)
                return null;

            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("DELETE FROM Postulantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            var filas = await cmd.ExecuteNonQueryAsync();
            if (filas == 0)
                return null;

            return existente; // regresamos el registro que se eliminó
        }

        // EXISTS Saber si existe un postulante
        public async Task<bool> Existe(int id)
        {
            using var conexion = new SqlConnection(_cadenaSql);
            await conexion.OpenAsync();

            var cmd = new SqlCommand("SELECT 1 FROM Postulantes WHERE Id = @id", conexion);
            cmd.Parameters.AddWithValue("@id", id);

            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }

        // MÉTODO PRIVADO  Mapear,  DataReader,  Postulante
        private Postulante Mapear(SqlDataReader dr)
        {
            return new Postulante
            {
                Id = Convert.ToInt32(dr["Id"]),
                NombreCompleto = dr["NombreCompleto"].ToString()!,
                Correo = dr["Correo"].ToString()!,
                Telefono = dr["Telefono"].ToString()!,
                CvUrl = dr["CvUrl"].ToString()!,
                FechaPostulacion = Convert.ToDateTime(dr["FechaPostulacion"]),
                VacanteId = Convert.ToInt32(dr["VacanteId"])
            };
        }
    }
}
