namespace ingresa.Models
{
    public class ContractFile
    {
        public int ContractFileId { get; set; }
        public int ContractId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string Type { get; set; }


        public Contract Contract { get; set; }


    }
}
