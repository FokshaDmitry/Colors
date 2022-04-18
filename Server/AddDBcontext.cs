using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace Server
{
    class AddDBcontext : DbContext
    {
        public DbSet<Lib.EntityContext.Entity> DBColors { get; set; }
        public AddDBcontext()
        {
                Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Проекты\Colors\Server\DBColors.mdf;Integrated Security=True");
        }
    }
}
