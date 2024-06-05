using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ingresa.Context;
using ingresa.Models;
using ingresa.Dtos;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.Extensions.Hosting;

namespace ingresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly AppDBcontext _context;
        private readonly IWebHostEnvironment _enviroment;

        public ContractsController(AppDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _enviroment = webHostEnvironment;

        }
        [HttpPost("End")]
        public IActionResult FinalizarContrato([FromForm] FinalizarContratoDTO dto)
        {

            var contratoActivo = _context.Contratos.FirstOrDefault(c => c.PersonaId == dto.PersonId && c.Estado);

            if (contratoActivo == null)
            {
                return BadRequest("La persona no tiene un contrato activo para finalizar.");
            }

            contratoActivo.Estado = false;
            contratoActivo.FechaConclucionContrato = dto.FechaFinContrato;
            _context.SaveChanges();


            return Ok();
        }
        // GET: api/Contracts
        [HttpGet("ContractsByPersonId/{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContractsByPersonId(int id)
        {

        var contractsWithContractFiles = await _context.Contratos
                 .Where(c => c.PersonaId == id)
         .Select(c => new
         {
             c.ContratoId,
             c.FechaInicio,
             c.FechaFin,
             c.Salario,
             c.Vacaciones,
             c.PersonaId,
             ContractFileUrl = _context.ArchivoContratos
                .Where(cf => cf.ContratoId == c.ContratoId && cf.Type == "contrato")
                .Select(cf => cf.FilePath)
                .FirstOrDefault()
         })
        .ToListAsync();
            return Ok(contractsWithContractFiles);

        }

        // GET: api/Contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> GetContract(int id)
        {
            var contract = await _context.Contratos.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        // PUT: api/Contracts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(int id, [FromForm] ContractUpdateDto contractUpdate)
        {
            if (id != contractUpdate.ContractId)
            {
                return BadRequest();
            }

            var contract = await _context.Contratos.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            if (id != contractUpdate.ContractId)
            {
                return BadRequest();
            }

            if (contract == null)
            {
                return NotFound();
            }

            if (contractUpdate.File != null)
            {
                // Verificar si existe un archivo asociado al contrato
                var existingFile = await _context.ArchivoContratos
                    .Where(cf => cf.ContratoId == contract.ContratoId && cf.Type == "contrato")
                    .FirstOrDefaultAsync();

                if (existingFile != null)
                {
                    // Eliminar el archivo existente del sistema de archivos
                    var existingFilePath = System.IO.Path.Combine(_enviroment.ContentRootPath, "uploads", existingFile.FilePath);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }

                    // Eliminar la entrada del archivo de la base de datos
                    _context.ArchivoContratos.Remove(existingFile);
                }

                // Guardar el nuevo archivo
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + contractUpdate.File.FileName;
                var filePath = System.IO.Path.Combine(_enviroment.ContentRootPath, "uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await contractUpdate.File.CopyToAsync(stream);
                }

                var contractFile = new ArchivoContrato
                {
                    ContratoId = contract.ContratoId,
                    FilePath = uniqueFileName,
                    Type = "contrato",
                    FileName = contractUpdate.File.FileName,
                    FileContentType = contractUpdate.File.ContentType
                };

                _context.ArchivoContratos.Add(contractFile);
            }




            contract.FechaInicio = contractUpdate.StartDate;
            contract.FechaFin = contractUpdate.EndDate;
            contract.Salario = contractUpdate.Salary;
            contract.Vacaciones = contractUpdate.Vacation;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updatedContract = await _context.Contratos.FindAsync(id);

            return Ok(updatedContract);
        }


        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContract([FromForm] CreateContractDto contractDto)
        {

            if (contractDto == null)
            {
                return BadRequest("Invalid contract data.");
            }

            var person = await _context.Personas.FindAsync(contractDto.PersonId);

            if (person == null)
            {
                return BadRequest("Person not Found");
            }

            var existingContracts = await _context.Contratos
        .Where(c => c.PersonaId == contractDto.PersonId)
        .ToListAsync();

            foreach (var existingContract in existingContracts)
            {
                existingContract.Estado = false;
            }
            var contract = new Contrato
            {
                Salario = contractDto.Salary,
                Vacaciones= contractDto.Vacation,
                FechaInicio = contractDto.StartDate,
                FechaFin = contractDto.EndDate,
                PersonaId = contractDto.PersonId
            };



            string uniqueFileName = Guid.NewGuid().ToString() + "_" + contractDto.File.FileName;

            var filePath = System.IO.Path.Combine(_enviroment.ContentRootPath,"uploads", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await contractDto.File.CopyToAsync(stream);
            }

            var contractFile = new ArchivoContrato
            {
                ContratoId = contract.ContratoId,
                FilePath = uniqueFileName,
                Type="contrato",
                FileName = contractDto.File.FileName,
                FileContentType = contractDto.File.ContentType
            };

            _context.ArchivoContratos.Add(contractFile);

            contractFile.Contrato= contract;
            // Asignar la persona al contrato
            contract.Persona = person;
            _context.Contratos.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContract", new { id = contract.ContratoId }, contract);


        }

        // DELETE: api/Contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contratos.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractExists(int id)
        {
            return _context.Contratos.Any(e => e.ContratoId == id);
        }
    }
}
