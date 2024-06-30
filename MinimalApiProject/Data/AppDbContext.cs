using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MinimalApiProject.Models;

namespace MinimalApiProject.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {          
        }

        public DbSet<UsuariosModel> Usuarios { get; set; }
    }
}
