using GestionCandidatosAPI.Modelos;

public interface IRepositorioPostulantes
{
    Task<IEnumerable<Postulante>> ObtenerPostulantes();
    Task<Postulante?> ObtenerPorId(int id);
    Task<int> InsertarPostulante(Postulante postulante);
    Task<IEnumerable<Postulante>> ListarPorVacante(int vacanteId);
    Task<Postulante?> ActualizarPostulante(int id, Postulante postulante);
    Task<Postulante?> EliminarPostulante(int id);
    Task<bool> Existe(int id);
}
