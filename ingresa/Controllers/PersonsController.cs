using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ingresa.Context;
using ingresa.Models;
using System.Security.Claims;
using ingresa.Services;
using ingresa.Dtos;
using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ingresa.Responses;
using Dapper;
using System.Data.SqlClient;

namespace ingresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly AppDBcontext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly DapperContext _contextDapper;


        public PersonsController(AppDBcontext context, AuthService authService, IConfiguration configuration, DapperContext connectionString)
        {
            _context = context;

            _authService = authService;
            _configuration = configuration;
            _contextDapper = connectionString;

        }

     

            // GET: api/Persons
            [HttpGet]
  
        public async Task<ActionResult<IEnumerable<GetPersonsDto>>> GetPersons()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            Console.WriteLine(identity.Claims.Count());
            try
            { 
            var persons = _context.Persons
            .Include(p => p.Cluster)
            .Include(p => p.Contracts)
            .Select(p => new GetPersonsDto
            {
                PersonId = p.PersonId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                DocumentNumber = p.DocumentNumber,
                ClusterName = p.Cluster.ClusterName,
                EstadoContrato = p.Contracts.Any() ? "ACTIVO" : "INACTIVO"
            })
            .ToList();

                    return Ok(persons);
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                return StatusCode(500, "Error al obtener personas: " + ex.Message);
            }
        }

        [HttpGet("Contract")]
        public async Task<IEnumerable<ContractReport>> GetPersonsAndDataContract()
        {
            using (var connection = _contextDapper.GetConnection())
            {
                connection.Open();
                var query = @"SELECT
    p.PersonId,
    p.FirstName,
    p.LastName,
    p.DocumentNumber,
	cf.FileName,
    CASE
        WHEN MAX(CASE WHEN c.StartDate IS NOT NULL THEN c.StartDate ELSE NULL END) IS NOT NULL THEN MAX(CASE WHEN c.StartDate IS NOT NULL THEN c.StartDate ELSE NULL END)
        ELSE NULL
    END AS StartDate,
    CASE
        WHEN MAX(CASE WHEN c.EndDate IS NOT NULL THEN c.EndDate ELSE NULL END) IS NOT NULL THEN MAX(CASE WHEN c.EndDate IS NOT NULL THEN c.EndDate ELSE NULL END)
        ELSE NULL
    END AS EndDate,
    GETDATE() AS FechaActual,
    COUNT(c.ContractId) AS NumContratos,
    CASE
        WHEN SUM(CASE WHEN c.State = 1 AND c.EndDate <= GETDATE() THEN 1 ELSE 0 END) > 0 THEN 'Contrato Vigente'
        WHEN SUM(CASE WHEN c.State = 0 AND c.EndDate > GETDATE() THEN 1 ELSE 0 END) > 0 THEN 'Contrato Expirado'
    END AS EstadoContrato
FROM
    Persons p
LEFT JOIN
    Contracts c ON p.PersonId = c.PersonId
Left join ContractFiles cf on cf.ContractId=c.ContractId and cf.Type='contrato' 
GROUP BY
    p.PersonId,
    p.FirstName,
    p.LastName,
    p.DocumentNumber,
		cf.FileName;";
                return await connection.QueryAsync<ContractReport>(query);
            }
        }


        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Persons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
        }

     


        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
    }
}
