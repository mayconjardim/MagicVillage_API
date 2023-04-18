using MagicVillage_API.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicVillage_API.Data
{ 
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Villa> Villas { get; set; }
    }
}
