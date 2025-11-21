using System.ComponentModel.DataAnnotations;

namespace GestionCandidatosAPI.Modelos
{
    public class Vacante
    {
        public int Id { get; set; }
        [Required]
        public required string Titulo { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
