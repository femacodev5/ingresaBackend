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

            var contratoActivo = _context.Contracts.FirstOrDefault(c => c.PersonId == dto.PersonId && c.State);

            if (contratoActivo == null)
            {
                return BadRequest("La persona no tiene un contrato activo para finalizar.");
            }

            contratoActivo.State = false;
            contratoActivo.FechaConclucionContrato = dto.FechaFinContrato;
            _context.SaveChanges();


            return Ok();
        }
        // GET: api/Contracts
        [HttpGet("ContractsByPersonId/{id}")]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContractsByPersonId(int id)
        {

            var contractsWithContractFiles = await _context.Contracts
                 .Where(c => c.PersonId == id)
         .Select(c => new
         {
             c.ContractId,
             c.StartDate,
             c.EndDate,
             c.Salary,
             c.Vacation,
             c.PersonId,
             ContractFileUrl = _context.ContractFiles
                .Where(cf => cf.ContractId == c.ContractId && cf.Type == "contrato")
                .Select(cf => cf.FilePath)
                .FirstOrDefault()
         })
        .ToListAsync();
            return Ok(contractsWithContractFiles);

        }

        // GET: api/Contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

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

            var contract = await _context.Contracts.FindAsync(id);
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
                var existingFile = await _context.ContractFiles
                    .Where(cf => cf.ContractId == contract.ContractId && cf.Type == "contrato")
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
                    _context.ContractFiles.Remove(existingFile);
                }

                // Guardar el nuevo archivo
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + contractUpdate.File.FileName;
                var filePath = System.IO.Path.Combine(_enviroment.ContentRootPath, "uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await contractUpdate.File.CopyToAsync(stream);
                }

                var contractFile = new ContractFile
                {
                    ContractId = contract.ContractId,
                    FilePath = uniqueFileName,
                    Type = "contrato",
                    FileName = contractUpdate.File.FileName,
                    FileContentType = contractUpdate.File.ContentType
                };

                _context.ContractFiles.Add(contractFile);
            }




            contract.StartDate = contractUpdate.StartDate;
            contract.EndDate = contractUpdate.EndDate;
            contract.Salary = contractUpdate.Salary;
            contract.Vacation = contractUpdate.Vacation;
            contract.Vacation = contractUpdate.Vacation;
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
            var updatedContract = await _context.Contracts.FindAsync(id);

            return Ok(updatedContract);
        }


        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract([FromForm] CreateContractDto contractDto)
        {

            if (contractDto == null)
            {
                return BadRequest("Invalid contract data.");
            }

            var person = await _context.Persons.FindAsync(contractDto.PersonId);

            if (person == null)
            {
                return BadRequest("Person not Found");
            }

            var existingContracts = await _context.Contracts
        .Where(c => c.PersonId == contractDto.PersonId)
        .ToListAsync();

            foreach (var existingContract in existingContracts)
            {
                existingContract.State = false;
            }
            var contract = new Contract
            {
                Salary = contractDto.Salary,
                Vacation = contractDto.Vacation,
                StartDate = contractDto.StartDate,
                EndDate = contractDto.EndDate,
                PersonId = contractDto.PersonId
            };



            string uniqueFileName = Guid.NewGuid().ToString() + "_" + contractDto.File.FileName;

            var filePath = System.IO.Path.Combine(_enviroment.ContentRootPath,"uploads", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await contractDto.File.CopyToAsync(stream);
            }

            var contractFile = new ContractFile
            {
                ContractId = contract.ContractId,
                FilePath = uniqueFileName,
                Type="contrato",
                FileName = contractDto.File.FileName,
                FileContentType = contractDto.File.ContentType
            };

            _context.ContractFiles.Add(contractFile);

            contractFile.Contract= contract;
            // Asignar la persona al contrato
            contract.Person = person;
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContract", new { id = contract.ContractId }, contract);


        }

        // DELETE: api/Contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.ContractId == id);
        }
    }
}
