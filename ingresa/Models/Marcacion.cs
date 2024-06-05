namespace ingresa.Models
{
    public class Marcacion
    {

        public int MarcacionId { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public int PersonaId { get; set; }

        public Boolean Estado { get; set; } = true;


        public Persona Persona { get; set; }

    }
}
