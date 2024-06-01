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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftResponse>>> GetShift()
        {
            var shifts = await _context.Shift
                             .Include(s => s.ShiftDetails)
                             .ToListAsync();

            var shiftResponses = shifts.Select(s => new ShiftResponse
            {
                ShiftId = s.ShiftId,
                Name = s.Name,
                Type = s.Type,
                ShiftDetails = s.ShiftDetails,
                TotalMinutosJornadaNeto = s.ShiftDetails?
                                    .Select(sd => (int)(sd.MinutosJornadaNeto))
                                    .Sum() ?? 0
            }).ToList();

            return Ok(shiftResponses);

        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(int id)
        {
            var shift = await _context.Shift.FindAsync(id);

            if (shift == null)
            {
                return NotFound();
            }

            return shift;
        }

        // PUT: api/Shifts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(int id, Shift shift)
        {
            if (id != shift.ShiftId)
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
        public async Task<ActionResult<Shift>> PostShift([FromBody] Object optData)
        {
            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());


  

            var shift = new Shift
            {
                Name = data.name.ToString(),
                Type = data.type.ToString(),
            };
            _context.Shift.Add(shift);
            await _context.SaveChangesAsync();
            var shiftId = shift.ShiftId;

            foreach (var detalle in data.detalleTurno)
            {

                var detalleTurno = new ShiftDetail
                {
                    ShiftId = shiftId,
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
                     detalleTurno.Shift = shift;
                _context.ShiftDetail.Add(detalleTurno);

            }
            await _context.SaveChangesAsync();



            return Ok(shift); // Devolver el objeto JObject como Ok response
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shift.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Shift.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShiftExists(int id)
        {
            return _context.Shift.Any(e => e.ShiftId == id);
        }
    }
}
