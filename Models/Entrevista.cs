namespace GestionCandidatosAPI.Modelos
{
    public class Entrevista
    {
        public int Id { get; set; }
        public DateTime FechaEntrevista { get; set; }
        public string Notas { get; set; } = string.Empty;
        public bool Realizada { get; set; }

        // Relación: A quién vamos a entrevistar
        public int PostulanteId { get; set; }
    }
}