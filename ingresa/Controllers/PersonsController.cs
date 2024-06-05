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
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http.Timeouts;

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

        [HttpGet]
  
        public async Task<ActionResult<IEnumerable<GetPersonsDto>>> GetPersons()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            Console.WriteLine(identity.Claims.Count());
            try
            { 
            var persons = _context.Personas
            .Include(p => p.Grupo)
            .Include(p => p.Contractos)
            .Select(p => new GetPersonsDto
            {
                PersonId = p.PersonaId,
                FirstName = p.Nombre,
                LastName = p.Apellido,
                ShiftId = p.TurnoId,
                Email = p.Correo,
                DocumentNumber = p.NumeroDocumento,
                ClusterName = p.Grupo.Nombre,
                EstadoContrato = p.Contractos.Any() ? "ACTIVO" : "INACTIVO"
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


        [HttpPost("ComprobarDni")]
        public async Task<FichaTurnoReport> ComprobarDni([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string dni = data.dni;
            var person = _context.Personas
                           .Include(e => e.Turno)
                            .ThenInclude(s => s.DetalleTurnos)
                           .FirstOrDefault(e => e.NumeroDocumento == dni);

            if (person == null)
            {
                return new FichaTurnoReport{
                    ResponseType="sin Contrato"
                };
            }

            return await TargetaTurno(person);
        }
        [HttpPost("GuardarMarcacion")]
        public async Task<Marcacion> GuardarMarcacion([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());


            string type = data?.type?.ToString();
            int personId = data?.personId;
            string hour = data?.hour;
            string date = data?.date;

            var record = new Marcacion();
            record.PersonaId = personId;
            record.Fecha = DateTime.Parse(date + " " + hour);
            record.Tipo = type;

            _context.Marcaciones.Add(record);

            _context.SaveChanges();

            return record;
        }
        public async Task<FichaTurnoReport> TargetaTurno(Persona person)

        {
            DateTime today = DateTime.Today;
            DateOnly todayDate = DateOnly.FromDateTime(today);

            int currentDayNumber = (int)today.DayOfWeek;

            bool isHoliday = _context.Feriados.Any(e => e.Fecha == todayDate);

            var holiday = _context.Feriados.Where(e => e.Fecha == todayDate).FirstOrDefault();

            bool workRequirement = false;

            string buttonAbiable;

            object dataButton;

            string responseType;

            if (isHoliday) {
                return new FichaTurnoReport
                {
                    ResponseType = "feriado",
                    Feriado=holiday
                };
            }

            if (!isHoliday && !workRequirement) 
            {
                int registrosHoy = _context.Marcaciones
                    .Where(e => e.PersonaId == person.PersonaId && e.Fecha== today)
                    .Count();

                var horarioHoy = _context.DetalleTurnos
                    .Where(e => e.TurnoId == person.TurnoId && e.DiaSemana == currentDayNumber)
                    .FirstOrDefault();

                if (registrosHoy == 0 && horarioHoy is not null)
                {

                    TimeSpan horaActual = DateTime.Now.TimeOfDay;
                    TimeSpan inicioMarcacionEntrada = horarioHoy.InicioMarcacionEntrada;
                    TimeSpan finMarcacionEntrada = horarioHoy.FinMarcacionEntrada;

                    if (true)
                    //if (horaActual >= inicioMarcacionEntrada && horaActual < finMarcacionEntrada)
                    {

                        var fichaTurnoReport = new FichaTurnoReport
                    {
                        DataButton = new
                        {
                            tipo = "ingreso",
                            inicioMarcacion = horarioHoy.InicioMarcacionEntrada,
                            hora = horarioHoy.HoraEntrada,
                            finMarcacion = horarioHoy.FinMarcacionEntrada
                        },
                        ResponseType = "registro",
                        PersonaId=person.PersonaId,
                    };
                    return fichaTurnoReport;
                    }
                }
              
            }

            return new FichaTurnoReport();
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPerson(int id)
        {
            var person = await _context.Personas.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, UpdatePersonDto personDto)
        {
            if (id != personDto.PersonId)
            {
                return BadRequest();
            }
            var person = await _context.Personas.FindAsync(id);
            person.TurnoId = personDto.ShiftId;
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
        public async Task<ActionResult<Persona>> PostPerson(Persona person)
        {
            _context.Personas.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.PersonaId }, person);
        }

     


        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Personas.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Personas.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Personas.Any(e => e.PersonaId == id);
        }
    }
}
