using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }
        public DbSet<Usuario> User { get; set; }
        public DbSet<DevLog> DevLog { get; set; }
        public DbSet<DevLogItem> DevLogItem{ get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }

    }
} 
