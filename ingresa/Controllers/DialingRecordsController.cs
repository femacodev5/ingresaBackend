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

namespace ingresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialingRecordsController : ControllerBase
    {
        private readonly AppDBcontext _context;

        public DialingRecordsController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: api/DialingRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marcacion>>> GetDialingRecords()
        {
            return await _context.Marcaciones.ToListAsync();
        }

        // GET: api/DialingRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Marcacion>> GetDialingRecord(int id)
        {
            var dialingRecord = await _context.Marcaciones.FindAsync(id);

            if (dialingRecord == null)
            {
                return NotFound();
            }

            return dialingRecord;
        }

        // PUT: api/DialingRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDialingRecord(int id, Marcacion dialingRecord)
        {
            if (id != dialingRecord.MarcacionId)
            {
                return BadRequest();
            }

            _context.Entry(dialingRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DialingRecordExists(id))
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

        // POST: api/DialingRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Marcacion>> PostDialingRecord([FromBody] Object optData)
        {

            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());


            string type = data?.type?.ToString();
            int empID = data?.empId;
            string hour = data?.hour;
            string date = data?.date;

            Console.WriteLine(data.type);

            DateTime dateTime = DateTime.Parse(date + " " + hour);

            var existingRecord = await _context.Marcaciones.

               FirstOrDefaultAsync(r => r.empID == empID && r.Fecha.Date == dateTime.Date &&
               r.Tipo==type
               );
     

            if (existingRecord != null)
            {
                existingRecord.Estado = false;
                await _context.SaveChangesAsync();
            }

            var record = new Marcacion();
            record.empID = empID;
            record.Fecha = dateTime;
            record.Tipo = type;


          
            _context.Marcaciones.Add(record);
            await _context.SaveChangesAsync();

            if (type== "ingreso")
            {
                var existingIngresoAsistencia = await _context.Asistencias
    .FirstOrDefaultAsync(a => a.empID == empID && a.Estado == "ingreso");

                if (existingIngresoAsistencia == null)
                {
                    // Si no existe una Asistencia de "ingreso", agregar una nueva
                    _context.Asistencias.Add(new Asistencia
                    {
                        InicioMarcacionId = record.MarcacionId,
                        Estado = "ingreso",
                        empID = empID,
                        Fecha = DateOnly.FromDateTime(DateTime.Today)
                    });
                    await _context.SaveChangesAsync();
                }
            }
            else if (type == "salida")
            {
                var lastAttendance = await _context.Asistencias
               .Where(a => a.empID == empID && a.Estado=="ingreso")
               .OrderByDescending(a => a.FechaCreacion)
               .FirstOrDefaultAsync();

                if (lastAttendance != null)
                {
                    lastAttendance.FinMarcacionId = record.MarcacionId;
                    lastAttendance.Estado = "salida";
                    await _context.SaveChangesAsync();
                }

            }else if (type == "inicioDescanso")
            {
                var lastAttendance = await _context.Asistencias
               .Where(a => a.empID == empID && a.Estado == "ingreso" && a.EstadoDescanso!= "inicioDescanso")
               .OrderByDescending(a => a.FechaCreacion)
               .FirstOrDefaultAsync();

                if (lastAttendance != null)
                {
                    lastAttendance.InicioDescansoId = record.MarcacionId;
                    lastAttendance.EstadoDescanso = "inicioDescanso";
                    await _context.SaveChangesAsync();
                }


            }
            else if (type == "finDescanso")
            {
                var lastAttendance = await _context.Asistencias
               .Where(a => a.empID == empID && a.Estado == "ingreso" && a.EstadoDescanso == "inicioDescanso")
               .OrderByDescending(a => a.FechaCreacion)
               .FirstOrDefaultAsync();

                if (lastAttendance != null)
                {
                    lastAttendance.FinDescansoId = record.MarcacionId;
                    lastAttendance.EstadoDescanso = "finDescanso";
                    await _context.SaveChangesAsync();
                }
            }



            _context.SaveChanges();

            return record;
        }

        // DELETE: api/DialingRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDialingRecord(int id)
        {
            var dialingRecord = await _context.Marcaciones.FindAsync(id);
            if (dialingRecord == null)
            {
                return NotFound();
            }

            _context.Marcaciones.Remove(dialingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DialingRecordExists(int id)
        {
            return _context.Marcaciones.Any(e => e.MarcacionId == id);
        }
    }
}
