﻿namespace ingresa.Dtos
{
    public class GetPersonsContractDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }

        public int ClusterId { get; set; }
        public string ClusterName { get; set; }
        public string ClusterDescription { get; set; }
        public string EstadoContrato { get; set; }
        public string? InicioContrato { get; set; }
        public string? FinContrato{ get; set; }

    }
}
