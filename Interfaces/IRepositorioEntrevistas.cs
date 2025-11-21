using GestionCandidatosAPI.Modelos;

public interface IRepositorioEntrevistas
{
    Task<int> AgendarAsync(Entrevista entrevista);
    Task<Entrevista?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<dynamic>> ListarPendientesAsync();
    Task<Entrevista?> ActualizarAsync(int id, Entrevista entrevista);
    Task<Entrevista?> EliminarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
