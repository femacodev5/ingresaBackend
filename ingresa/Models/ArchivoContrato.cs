namespace ingresa.Models
{
    public class ArchivoContrato
    {
        public int ArchivoContratoId { get; set; }
        public int ContratoId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string Type { get; set; }


        public Contrato Contrato { get; set; }


    }
}
