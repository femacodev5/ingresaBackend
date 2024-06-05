using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ingresa.Context;
using ingresa.Models;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using ingresa.Dtos;

namespace ingresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly AppDBcontext _context;

        public ShiftsController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: api/Shifts

        [HttpGet("Select")]
        public async Task<ActionResult<IEnumerable<Turno>>> GetAll()
        {
            return await _context.Turnos.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftResponse>>> GetShift()
        {
            var shifts = await _context.Turnos
                             .Include(s => s.DetalleTurnos)
                             .ToListAsync();

            var shiftResponses = shifts.Select(s => new ShiftResponse
            {
                TurnoId = s.TurnoId,
                Nombre = s.Nombre,
                Tipo = s.Tipo,
                DetalleTurnos = s.DetalleTurnos,
                TotalMinutosJornadaNeto = s.DetalleTurnos?
                                    .Select(sd => (int)(sd.MinutosJornadaNeto))
                                    .Sum() ?? 0
            }).ToList();

            return Ok(shiftResponses);

        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> GetShift(int id)
        {
            var shift = await _context.Turnos.FindAsync(id);

            if (shift == null)
            {
                return NotFound();
            }

            return shift;
        }

        // PUT: api/Shifts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(int id, Turno shift)
        {
            if (id != shift.TurnoId)
            {
                return BadRequest();
            }

            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
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

        // POST: api/Shifts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Turno>> PostShift([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            var shift = new Turno
            {
                Nombre = data.name.ToString(),
                Tipo = data.type.ToString(),
            };
            _context.Turnos.Add(shift);
            await _context.SaveChangesAsync();
            var shiftId = shift.TurnoId;

            foreach (var detalle in data.detalleTurno)
            {

                var detalleTurno = new DetalleTurno
                {
                    TurnoId = shiftId,
                    DiaSemana = (int)detalle.numero,
                    InicioMarcacionEntrada = new TimeSpan(DateTime.Parse((string)detalle.inicioMarcacionEntrada).Hour, DateTime.Parse((string)detalle.inicioMarcacionEntrada).Minute, 0),
                    FinMarcacionEntrada = new TimeSpan(DateTime.Parse((string)detalle.finMarcacionEntrada).Hour, DateTime.Parse((string)detalle.finMarcacionEntrada).Minute, 0),
                    HoraEntrada = new TimeSpan(DateTime.Parse((string)detalle.horaEntrada).Hour, DateTime.Parse((string)detalle.horaEntrada).Minute, 0),
                    InicioMarcacionDescanso = new TimeSpan(DateTime.Parse((string)detalle.inicioMarcacionDescanso).Hour, DateTime.Parse((string)detalle.inicioMarcacionDescanso).Minute, 0),
                    FinMarcacionDescanso = new TimeSpan(DateTime.Parse((string)detalle.finMarcacionDescanso).Hour, DateTime.Parse((string)detalle.finMarcacionDescanso).Minute, 0),
                    MinutosDescanso = (int)detalle.minutosDescanso,
                    HabilitarDescanso = (bool)detalle.habilitarDescanso,
                    InicioMarcacionSalida = new TimeSpan(DateTime.Parse((string)detalle.inicioMarcacionSalida).Hour, DateTime.Parse((string)detalle.inicioMarcacionSalida).Minute, 0),
                    HoraSalida = new TimeSpan(DateTime.Parse((string)detalle.horaSalida).Hour, DateTime.Parse((string)detalle.horaSalida).Minute, 0),
                    FinMarcacionSalida = new TimeSpan(DateTime.Parse((string)detalle.finMarcacionSalida).Hour, DateTime.Parse((string)detalle.finMarcacionSalida).Minute, 0),

                    MinutosJornada = (int)detalle.minutosJornada,
                    MinutosJornadaNeto = (int)detalle.minutosJornadaNeto,


                };
                     detalleTurno.Turno = shift;
                _context.DetalleTurnos.Add(detalleTurno);

            }
            await _context.SaveChangesAsync();



            return Ok(shift); // Devolver el objeto JObject como Ok response
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Turnos.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Turnos.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShiftExists(int id)
        {
            return _context.Turnos.Any(e => e.TurnoId == id);
        }
    }
}
