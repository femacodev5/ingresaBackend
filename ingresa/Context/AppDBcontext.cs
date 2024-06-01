using ingresa.Models;
using Microsoft.EntityFrameworkCore;


namespace ingresa.Context
{
    public class AppDBcontext :DbContext
    {



        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {
            
        }



        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<Person> Persons { get; set; }  
        public DbSet<Contract> Contracts { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<ingresa.Models.Shift> Shift { get; set; } = default!;
        public DbSet<ShiftDetail> ShiftDetail { get; set; }

        public DbSet<ContractFile> ContractFiles { get; set; }




    }


}
