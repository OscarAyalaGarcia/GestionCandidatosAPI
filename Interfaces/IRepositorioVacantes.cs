using GestionCandidatosAPI.Modelos;

public interface IRepositorioVacantes
{
    Task<IEnumerable<Vacante>> ObtenerVacantes();
    Task<Vacante?> ObtenerPorId(int id);
    Task<Vacante> InsertarVacante(Vacante vacante);
    Task<Vacante?> ActualizarVacante(int id, Vacante vacante);
    Task<Vacante?> EliminarVacante(int id);
    Task<bool> Existe(int id);
}
