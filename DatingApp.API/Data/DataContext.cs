using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}

        public DbSet<Value> values {get; set;}

        public DbSet<user> Users{ get; set;}

        public DbSet<Photo> Photos { get; set; }
    }
}