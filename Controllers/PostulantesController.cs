using GestionCandidatosAPI.Modelos;
using GestionCandidatosAPI.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace GestionCandidatosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostulantesController : ControllerBase
    {
        private readonly IRepositorioPostulantes _repo;

        public PostulantesController(IRepositorioPostulantes repo)
        {
            _repo = repo;
        }

        // GET: api/Postulantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulante>>> ObtenerPostulantes()
        {
            var lista = await _repo.ObtenerPostulantes();
            return Ok(lista);
        }

        // GET: api/Postulantes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Postulante>> ObtenerPorId(int id)
        {
            var postulante = await _repo.ObtenerPorId(id);

            if (postulante == null)
                return NotFound($"No existe un postulante con el ID {id}");

            return Ok(postulante);
        }

        // GET: api/Postulantes/Vacante/3
        [HttpGet("Vacante/{vacanteId:int}")]
        public async Task<ActionResult<IEnumerable<Postulante>>> ListarPorVacante(int vacanteId)
        {
            var lista = await _repo.ListarPorVacante(vacanteId);
            return Ok(lista);
        }

        // POST: api/Postulantes
        [HttpPost]
        public async Task<ActionResult> InsertarPostulante([FromBody] Postulante postulante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var idNuevo = await _repo.InsertarPostulante(postulante);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = idNuevo }, postulante);
        }

        // PUT: api/Postulantes/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Postulante>> ActualizarPostulante(int id, [FromBody] Postulante postulante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _repo.Existe(id);
            if (!existe)
                return NotFound($"No existe un postulante con el ID {id}");

            var actualizado = await _repo.ActualizarPostulante(id, postulante);
            return Ok(actualizado);
        }

        // DELETE: api/Postulantes/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarPostulante(int id)
        {
            var existe = await _repo.Existe(id);
            if (!existe)
                return NotFound($"No existe un postulante con el ID {id}");

            var eliminado = await _repo.EliminarPostulante(id);
            return Ok(eliminado);
        }
    }
}
