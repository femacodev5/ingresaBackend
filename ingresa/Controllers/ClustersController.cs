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

namespace ingresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClustersController : ControllerBase
    {
        private readonly AppDBcontext _context;

        public ClustersController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: api/Clusters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cluster>>> GetClusters()
        {
            return await _context.Clusters.ToListAsync();
        }

        // GET: api/Clusters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cluster>> GetCluster(int id)
        {
            var cluster = await _context.Clusters.FindAsync(id);

            if (cluster == null)
            {
                return NotFound();
            }

            return cluster;
        }

        // PUT: api/Clusters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCluster(int id, Cluster cluster)
        {
            if (id != cluster.ClusterId)
            {
                return BadRequest();
            }

            _context.Entry(cluster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClusterExists(id))
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

        // POST: api/Clusters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cluster>> PostCluster(ClusterCreateDto clusterDto)
        {
            if (!ModelState.IsValid) { 
                return BadRequest();
            }
            var cluster = new Cluster
            {
                ClusterName = clusterDto.ClusterName,
            };
            _context.Clusters.Add(cluster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCluster", new { id = cluster.ClusterId }, cluster);
        }

        // DELETE: api/Clusters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCluster(int id)
        {
            var cluster = await _context.Clusters.FindAsync(id);
            if (cluster == null)
            {
                return NotFound();
            }

            _context.Clusters.Remove(cluster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClusterExists(int id)
        {
            return _context.Clusters.Any(e => e.ClusterId == id);
        }
    }
}
