using Dapper;
using ingresa.Context;
using ingresa.Dtos;
using ingresa.Models;
using ingresa.Responses;
using ingresa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public async Task<ActionResult<IEnumerable<GetPersonsResponse>>> GetPersons()
        {
            try
            {
                List<GetPersonsResponse> sapResults;
                List<EmpleadoTurnoResponse> dbResults;

                using (var connectionSAP = _contextDapper.CreateConnectionSAP())
                {
                    connectionSAP.Open();
                    var querySAP = @"select ""empID"", ""lastName"",""middleName"",""firstName"",""Code"",""jobTitle"" from Ohem;";
                    sapResults = (await connectionSAP.QueryAsync<GetPersonsResponse>(querySAP)).ToList();
                }

                using (var connectionDB = _contextDapper.CreateConnection())
                {
                    connectionDB.Open();
                    var queryDb = @"select et.EmpId,t.turnoId, t.Nombre as TurnoNombre from EmpleadoTurnos et 
                        join Turnos t on t.TurnoId = et.TurnoId and et.Estado = 1";
                    dbResults = (await connectionDB.QueryAsync<EmpleadoTurnoResponse>(queryDb)).ToList();
                }

                // Combinar resultados
                var combinedResults = sapResults.Select(sapPerson =>
                {
                    var turno = dbResults.FirstOrDefault(dbTurno => dbTurno.EmpId == sapPerson.EmpID);
                    return new GetEmpleadoResponse
                    {
                        EmpID = sapPerson.EmpID,
                        LastName = sapPerson.LastName,
                        MiddleName = sapPerson.MiddleName,
                        FirstName = sapPerson.FirstName,
                        Code = sapPerson.Code,
                        JobTitle = sapPerson.JobTitle,
                        TurnoNombre = turno?.TurnoNombre,
                        TurnoId = turno?.TurnoId
                    };
                }).ToList();

                return Ok(combinedResults);

            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, "Error al obtener personas: " + ex.Message);
            }
        }

        [HttpGet("Prueba")]
        public async Task<IEnumerable<ContractReport>> Prueba()
        {
            using (var connection = _contextDapper.CreateConnection())
            {
                connection.Open();
                var query = @"SELECT
  ";


                return await connection.QueryAsync<ContractReport>(query);
            }
        }






        [HttpGet("Contract")]
        public async Task<IEnumerable<CombinedContractReport>> GetPersonsAndDataContract()
        {
            List<GetPersonsResponse> sapResults;
            List<ContractReport> dbResults;

            using (var connectionSAP = _contextDapper.CreateConnectionSAP())
            {
                connectionSAP.Open();
                var querySAP = @"select ""empID"", ""lastName"",""middleName"",""firstName"",""Code"",""jobTitle"" from Ohem;";
                sapResults = (await connectionSAP.QueryAsync<GetPersonsResponse>(querySAP)).ToList();
            }

            using (var connection = _contextDapper.CreateConnection())
            {
                connection.Open();
                var queryDB = @"SELECT
                            c.empID AS EmpID,
                            cf.FileName,
                            CASE
                                WHEN MAX(CASE WHEN c.FechaInicio IS NOT NULL THEN c.FechaInicio ELSE NULL END) IS NOT NULL THEN MAX(CASE WHEN c.FechaInicio IS NOT NULL THEN c.FechaInicio ELSE NULL END)
                                ELSE NULL
                            END AS FechaInicio,
                            CASE
                                WHEN MAX(CASE WHEN c.FechaFin IS NOT NULL THEN c.FechaFin ELSE NULL END) IS NOT NULL THEN MAX(CASE WHEN c.FechaFin IS NOT NULL THEN c.FechaFin ELSE NULL END)
                                ELSE NULL
                            END AS FechaFin,
                            GETDATE() AS FechaActual,
                            COUNT(c.ContratoId) AS NumContratos,
                            CASE
                                WHEN SUM(CASE WHEN c.Estado = 1 AND c.FechaFin <= GETDATE() THEN 1 ELSE 0 END) > 0 THEN 'Contrato Vigente'
                                WHEN SUM(CASE WHEN c.Estado = 0 AND c.FechaFin > GETDATE() THEN 1 ELSE 0 END) > 0
OR 
								 c.Estado=0
THEN 'Contrato Expirado'
                            END AS EstadoContrato
                        FROM Contratos c 
                        LEFT JOIN ArchivoContratos cf ON cf.ContratoId = c.ContratoId AND cf.Type = 'contrato' 
                        GROUP BY
                            c.empID,
c.Estado,
                            cf.FileName;";
                dbResults = (await connection.QueryAsync<ContractReport>(queryDB)).ToList();
            }

            var combinedResults = new List<CombinedContractReport>();

            foreach (var sapPerson in sapResults)
            {
                // Busca los detalles del contrato para esta persona
                var contrato = dbResults.FirstOrDefault(db => db.EmpID == sapPerson.EmpID);

                // Crea una nueva instancia de CombinedContractReport y asigna los detalles
                var combinedReport = new CombinedContractReport
                {
                    EmpID = sapPerson.EmpID,
                    Code = sapPerson.Code,
                    LastName = sapPerson.LastName,
                    MiddleName = sapPerson.MiddleName,
                    FirstName = sapPerson.FirstName,
                    JobTitle = sapPerson.JobTitle,
                    EstadoContrato = contrato != null ? contrato.EstadoContrato : "Sin Contrato",
                    FechaActual = DateTime.Now, // Asigna la fecha actual
                    FechaInicio = contrato != null ? contrato.FechaInicio : null,
                    FechaFin = contrato != null ? contrato.FechaFin : null,
                    NumContratos = contrato != null ? contrato.NumContratos : 0
                };

                // Agrega el CombinedContractReport a la lista de resultados combinados
                combinedResults.Add(combinedReport);
            }

            return combinedResults;
        }


        // GET: api/Persons/5



        [HttpPost("GuardarRostro")]
        public async Task<FichaTurnoReport> GuardarRostro([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            //  dynamic rostro = data.rostro;

            string empID = data.empID;
            string rostro = data.rostro;
            string rostroJsonString = data.rostro;
            int tamanoRostro = rostroJsonString.Length;
            Console.WriteLine("Tamaño del rostro en caracteres: " + tamanoRostro);
            using (var connectionSAP = _contextDapper.CreateConnectionSAP())
            {
                connectionSAP.Open();
                // Corrige la consulta SQL para asignar el valor del parámetro 'rostro' y 'empID'
                var querySAP = @"UPDATE ohem
                         SET ""FOTO"" = ?
                         WHERE ""empID"" = ?";

                // Ejecuta la consulta SQL utilizando Dapper, pasando los valores de los parámetros 'rostro' y 'empID'
                await connectionSAP.ExecuteAsync(querySAP, new { rostro, empID });
            }

            // Aquí podrías retornar algo si es necesario, por ejemplo, un mensaje de éxito
            return new FichaTurnoReport(); // Supongo que FichaTurnoReport es el tipo de dato que deseas devolver
        }

        public async Task<FichaTurnoReport> TargetaTurno(int empID)
        {
            DateTime today = DateTime.Today;
            DateOnly todayDate = DateOnly.FromDateTime(today);

            int currentDayNumber = (int)today.DayOfWeek;

            bool isHoliday = _context.Feriados.Any(e => e.Fecha == todayDate);

            var holiday = _context.Feriados.FirstOrDefault(e => e.Fecha == todayDate);

            bool workRequirement = false;

            string buttonAbiable;

            object dataButton;

            string responseType;



            if (isHoliday)
            {
                return new FichaTurnoReport
                {
                    ResponseType = "feriado",
                    Feriado = holiday
                };
            }

            if (!isHoliday && !workRequirement)
            {
                int registrosHoy = _context.Asistencias
                 .Count(e => e.empID == empID && e.Fecha == todayDate);


                Console.WriteLine(registrosHoy);


                var EmpleadoTurno = _context.EmpleadoTurnos.FirstOrDefault(e => e.EmpId == empID && e.Estado == true);

                DetalleTurno horarioHoy = null;

                if (EmpleadoTurno != null)
                {
                    horarioHoy = _context.DetalleTurnos
                        .FirstOrDefault(e => e.TurnoId == EmpleadoTurno.TurnoId && e.DiaSemana == currentDayNumber);
                }
                else
                {
                    return new FichaTurnoReport
                    {
                        ResponseType = "El empleado no tiene ningun horario Asignado"
                    };
                }
                if (horarioHoy == null)
                {
                    return new FichaTurnoReport
                    {
                        ResponseType = "Hoy no es un dia Laboral"
                    };
                }



                if (registrosHoy == 0)
                {
                    TimeSpan horaActual = DateTime.Now.TimeOfDay;
                    TimeSpan inicioMarcacionEntrada = horarioHoy.InicioMarcacionEntrada;
                    TimeSpan finMarcacionEntrada = horarioHoy.FinMarcacionEntrada;


                    //if (horaActual >= inicioMarcacionEntrada && horaActual < finMarcacionEntrada)
                    if(true)
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
                            EmpId = empID,
                        };
                        return fichaTurnoReport;
                    }
                    else
                    {
                        return new FichaTurnoReport
                        {
                            ResponseType = "asdasd",
                        };
                    }
                }
                else if (registrosHoy == 1)
                {
                    var estadoAsistencias = _context.Asistencias.FirstOrDefault(e => e.empID == empID && e.Fecha == todayDate);


                    if (estadoAsistencias.Estado == "ingreso")
                    {


                        if (estadoAsistencias.EstadoDescanso is null)
                        {
                            var fichaTurnoReport = new FichaTurnoReport
                            {
                                DataButton = new
                                {
                                    tipo = "inicioDescanso",
                                    inicioMarcacion = horarioHoy.InicioMarcacionDescanso,
                                    finMarcacion = horarioHoy.FinMarcacionDescanso
                                },
                                ResponseType = "registro",
                                EmpId = empID,
                            };

                            return fichaTurnoReport;
                        }
                        else if (estadoAsistencias.EstadoDescanso == "inicioDescanso")
                        {
                            var fichaTurnoReport = new FichaTurnoReport
                            {
                                DataButton = new
                                {
                                    tipo = "finDescanso",
                                    inicioMarcacion = horarioHoy.InicioMarcacionDescanso,
                                
                                    finMarcacion = horarioHoy.FinMarcacionDescanso
                                },
                                ResponseType = "registro",
                                EmpId = empID,
                            };

                            return fichaTurnoReport;
                        }
                        else if (estadoAsistencias.EstadoDescanso == "finDescanso")
                        {
                            var fichaTurnoReport = new FichaTurnoReport
                            {
                                DataButton = new
                                {
                                    tipo = "salida",
                                    inicioMarcacion = horarioHoy.InicioMarcacionEntrada,
                                    hora = horarioHoy.HoraEntrada,
                                    finMarcacion = horarioHoy.FinMarcacionEntrada
                                },
                                ResponseType = "registro",
                                EmpId = empID,
                            };

                            return fichaTurnoReport;
                        }


                    }



                }


            }

            return new FichaTurnoReport();
        }

        [HttpPost("ComprobarDni")]
        public async Task<FichaTurnoReport> ComprobarDni([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string code = data.dni;

            using (var connectionSAP = _contextDapper.CreateConnectionSAP())
            {
                connectionSAP.Open();
                // Corrige la consulta SQL para asignar el valor del parámetro 'rostro' y 'empID'
                var querySAP = @"select *  from ohem where ""Code""=?";

                // Ejecuta la consulta SQL utilizando Dapper, pasando los valores de los parámetros 'rostro' y 'empID'
                var result = await connectionSAP.QueryFirstOrDefaultAsync<EmployeeData>(querySAP, new { code });


                if (result == null)
                {
                    return new FichaTurnoReport
                    {
                        ResponseType = "código no encontrado"
                    };
                }
                var today = DateOnly.FromDateTime(DateTime.Today);

                var contrato = await _context.Contratos
    .FirstOrDefaultAsync(c => c.EmpID == result.empID && c.Estado && c.FechaFin >= today);
                if (contrato == null)
                {
                    return new FichaTurnoReport
                    {
                        ResponseType = "El empleado no tiene un contrato válido"
                    };
                }

                return await TargetaTurno(result.empID);
            }

        }
        [HttpPost("GuardarMarcacion")]
        public async Task<Marcacion> GuardarMarcacion([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());


            string type = data?.type?.ToString();
            int personId = data?.empID;
            string hour = data?.hour;
            string date = data?.date;

            var record = new Marcacion();
            record.empID = personId;
            record.Fecha = DateTime.Parse(date + " " + hour);
            record.Tipo = type;

            _context.Marcaciones.Add(record);

            _context.SaveChanges();

            return record;
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<GetPersonsResponse>> GetPerson(int id)
        {
            List<GetPersonsResponse> sapResults;

            using (var connectionSAP = _contextDapper.CreateConnectionSAP())
            {
                connectionSAP.Open();
                // Corrige la consulta SQL para filtrar por el parámetro 'id'
                var querySAP = @"select ""empID"", ""lastName"",""middleName"",""firstName"",""Code"",""jobTitle"" from Ohem where ""empID"" = ?";

                // Asegúrate de pasar el parámetro 'id' a la consulta
                sapResults = (await connectionSAP.QueryAsync<GetPersonsResponse>(querySAP, new { Id = id })).ToList();
            }

            // Verifica si se encontraron resultados
            if (sapResults.Count == 0)
            {
                // Si no se encontraron resultados, devuelve un NotFound
                return NotFound();
            }
            else if (sapResults.Count == 1)
            {
                // Si se encontró un único resultado, devuélvelo
                return sapResults[0];
            }
            else
            {
                // Si se encontraron múltiples resultados, devuelve un error ya que esta acción está configurada para devolver solo un resultado
                return BadRequest("Se encontraron múltiples resultados, pero esta acción solo puede devolver un resultado único.");
            }

        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, UpdatePersonDto personDto)
        {
            if (id != personDto.EmpId)
            {
                return BadRequest();
            }




            var existingEmpleadoTurnos = await _context.EmpleadoTurnos
               .Where(et => et.EmpId == personDto.EmpId)
               .ToListAsync();

            foreach (var existingEmpleadoTurno in existingEmpleadoTurnos)
            {
                existingEmpleadoTurno.Estado = false; // Suponiendo que hay una propiedad Estado
                _context.Entry(existingEmpleadoTurno).State = EntityState.Modified;
            }

            _context.EmpleadoTurnos.Add(new EmpleadoTurno
            {
                EmpId = personDto.EmpId,
                TurnoId = personDto.TurnoId
            });


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
