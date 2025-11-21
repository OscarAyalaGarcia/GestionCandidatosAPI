using System.ComponentModel.DataAnnotations;

namespace GestionCandidatosAPI.Modelos
{
    public class Postulante
    {
        public int Id { get; set; }
        public required string NombreCompleto { get; set; }
        [Required]
        public required string Correo { get; set; }
        [Required]
        public required string Telefono { get; set; }
        [Required]
        public string CvUrl { get; set; } = string.Empty; 
        public DateTime FechaPostulacion { get; set; }

        //  conexión con la vacante
        public int VacanteId { get; set; }
    }
}