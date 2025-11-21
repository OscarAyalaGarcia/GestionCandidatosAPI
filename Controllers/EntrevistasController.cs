using GestionCandidatosAPI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionCandidatosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntrevistasController : ControllerBase
    {
        private readonly IRepositorioEntrevistas _repositorio;

        public EntrevistasController(IRepositorioEntrevistas repositorio)
        {
            _repositorio = repositorio;
        }

        // ---------------------------------------------------------
        // GET: api/entrevistas
        // Listar todas las entrevistas pendientes
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> ListarPendientes()
        {
            var lista = await _repositorio.ListarPendientesAsync();
            return Ok(lista);
        }

        // ---------------------------------------------------------
        // GET: api/entrevistas/5
        // Obtener entrevista por Id
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var entrevista = await _repositorio.ObtenerPorIdAsync(id);
            if (entrevista == null)
                return NotFound($"No se encontró la entrevista con Id {id}");

            return Ok(entrevista);
        }

        // ---------------------------------------------------------
        // POST: api/entrevistas
        // Agendar nueva entrevista
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Agendar([FromBody] Entrevista entrevista)
        {
            if (entrevista == null) return BadRequest("La entrevista no puede ser nula.");

            // opcional: verificar si el postulante existe usando otro repositorio
            var id = await _repositorio.AgendarAsync(entrevista);
            entrevista.Id = id;

            return Ok(new { mensaje = "Entrevista agendada.", id });
        }

        // ---------------------------------------------------------
        // PUT: api/entrevistas/5
        // Actualizar entrevista
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Entrevista entrevista)
        {
            if (entrevista == null) return BadRequest("La entrevista no puede ser nula.");

            var actualizada = await _repositorio.ActualizarAsync(id, entrevista);
            if (actualizada == null)
                return NotFound($"No se encontró la entrevista con Id {id}");

            return Ok("Entrevista actualizada.");
        }

        // ---------------------------------------------------------
        // DELETE: api/entrevistas/5
        // Eliminar entrevista
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminada = await _repositorio.EliminarAsync(id);
            if (eliminada == null)
                return NotFound($"No se encontró la entrevista con Id {id}");

            return Ok("Entrevista eliminada.");
        }
    }
}
