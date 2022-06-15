using Microsoft.EntityFrameworkCore;
using NationalParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParkApi.Data
{
    public class ParkDbContext : DbContext
    {
        public ParkDbContext(DbContextOptions<ParkDbContext> options) : base(options)
        {

        }

        public DbSet<Park> Parks { get; set; }
        public DbSet<Trail> Trails { get; set; }
    }
}
