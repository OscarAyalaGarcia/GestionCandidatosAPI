using GestionCandidatosAPI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionCandidatosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacantesController : ControllerBase
    {
        private readonly IRepositorioVacantes _repo;

        public VacantesController(IRepositorioVacantes repo)
        {
            _repo = repo;
        }

        // ============================================================
        // GET: api/vacantes
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vacante>>> Get()
        {
            var lista = await _repo.ObtenerVacantes();
            return Ok(lista);
        }

        // ============================================================
        // GET: api/vacantes/{id}
        // ============================================================
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Vacante>> GetPorId(int id)
        {
            var vacante = await _repo.ObtenerPorId(id);

            if (vacante is null)
                return NotFound(new { mensaje = "Vacante no encontrada" });

            return Ok(vacante);
        }

        // ============================================================
        // POST: api/vacantes
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<Vacante>> Post(Vacante vacante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creada = await _repo.InsertarVacante(vacante);

            return CreatedAtAction(nameof(GetPorId), new { id = creada.Id }, creada);
        }

        // ============================================================
        // PUT: api/vacantes/{id}
        // ============================================================
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Vacante>> Put(int id, Vacante vacante)
        {
            var existente = await _repo.ObtenerPorId(id);

            if (existente is null)
                return NotFound(new { mensaje = "La vacante que intentas actualizar no existe" });

            var actualizada = await _repo.ActualizarVacante(id, vacante);

            return Ok(actualizada);
        }

        // ============================================================
        // DELETE: api/vacantes/{id}
        // Regresa la entidad eliminada
        // ============================================================
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Vacante>> Delete(int id)
        {
            var eliminada = await _repo.EliminarVacante(id);

            if (eliminada is null)
                return NotFound(new { mensaje = "No se encontró la vacante a eliminar" });

            return Ok(eliminada);
        }

        // ============================================================
        // GET: api/vacantes/{id}/existe
        // ============================================================
        [HttpGet("{id:int}/existe")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var existe = await _repo.Existe(id);
            return Ok(existe);
        }
    }
}
