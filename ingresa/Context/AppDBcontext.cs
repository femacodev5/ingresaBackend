using ingresa.Models;
using Microsoft.EntityFrameworkCore;


namespace ingresa.Context
{
    public class AppDBcontext :DbContext
    {



        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {
            
        }



        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Persona> Personas { get; set; }  
        public DbSet<Contrato> Contratos { get; set; }

        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<DetalleTurno> DetalleTurnos { get; set; }

        public DbSet<ArchivoContrato> ArchivoContratos { get; set; }

        public DbSet<Marcacion> Marcaciones { get; set; }

        public DbSet<Asistencia> Asistencias { get; set; }

    }


}
