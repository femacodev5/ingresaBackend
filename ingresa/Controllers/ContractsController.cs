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
        private readonly DapperContext _contextDapper;

        public ContractsController(AppDBcontext context, IWebHostEnvironment webHostEnvironment, DapperContext connectionString)
        {
            _context = context;
            _enviroment = webHostEnvironment;
            _contextDapper = connectionString;

        }
        [HttpPost("End")]
        public IActionResult FinalizarContrato([FromForm] FinalizarContratoDTO dto)
        {
            // Buscar el contrato activo para la persona
            var contratoActivo = _context.Contratos.FirstOrDefault(c => c.EmpID == dto.EmpID && c.Estado);

            // Verificar si se encontró un contrato activo
            if (contratoActivo == null)
            {
                return BadRequest("La persona no tiene un contrato activo para finalizar.");
            }

            // Si se proporcionó un archivo para cargar
            if (dto.File != null)
            {
                // Guardar el archivo
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.File.FileName;
                var filePath = Path.Combine(_enviroment.ContentRootPath, "uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.File.CopyTo(stream);
                }

                // Crear un nuevo registro de archivo de contrato
                var contractFile = new ArchivoContrato
                {
                    ContratoId = contratoActivo.ContratoId,
                    FilePath = uniqueFileName,
                    Type = "Cese", // Tipo de archivo, ¿es este el tipo correcto?
                    FileName = dto.File.FileName,
                    FileContentType = dto.File.ContentType
                };

                // Agregar el nuevo archivo de contrato a la base de datos
                _context.ArchivoContratos.Add(contractFile);
            }

            // Finalizar el contrato activo
            contratoActivo.Estado = false;
            contratoActivo.FechaConclucionContrato = dto.FechaFinContrato;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok();
        }
        // GET: api/Contracts
        [HttpGet("ContractsByPersonId/{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContractsByPersonId(int id)
        {
            using (var connectionSAP = _contextDapper.CreateConnectionSAP())
            {

            }
                var contractsWithContractFiles = await _context.Contratos
                         .Where(c => c.EmpID == id)
                 .Select(c => new
                 {
                     c.ContratoId,
                     c.FechaInicio,
                     c.FechaFin,
                     c.Salario,
                     c.Vacaciones,
                     c.EmpID,
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
            if (id != contractUpdate.ContratoId)
            {
                return BadRequest();
            }

            var contract = await _context.Contratos.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            if (id != contractUpdate.ContratoId)
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




            contract.FechaInicio = contractUpdate.FechaInicio;
            contract.FechaFin = contractUpdate.FechaFin;
            contract.Salario = contractUpdate.Salario;
            contract.Vacaciones = contractUpdate.Vacaciones;
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



            var existingContracts = await _context.Contratos
        .Where(c => c.EmpID == contractDto.EmpID)
        .ToListAsync();

            foreach (var existingContract in existingContracts)
            {
                existingContract.Estado = false;
            }
            var contract = new Contrato
            {
                Salario = contractDto.Salario,
                Vacaciones= contractDto.Vacaciones,
                FechaInicio = contractDto.FechaInicio,
                FechaFin = contractDto.FechaFin,
                EmpID = contractDto.EmpID
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
