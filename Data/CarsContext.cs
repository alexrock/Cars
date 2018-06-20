using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cars.Models
{
    public class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions<CarsContext> options)
            : base(options)
        {
        }

        public DbSet<Carro> Carro { get; set; }
        public DbSet<Acessorio> Acessorio { get; set; }
        public DbSet<CarroAcessorio> CarroAcessorio { get; set; }
    }
}