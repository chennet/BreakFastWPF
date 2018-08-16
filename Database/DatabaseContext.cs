using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BFMenu
    {
        public int Id { get; set; }
        public string MenuId { get; set; }
        public string MenuType { get; set; }    //Solid or liquid?
        public string Style { get; set; }       //Chinese, Japaness, or America style?
        public string Title { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string ImageUri { get; set; }
        public string Desp { get; set; }
    }


    public class DatabaseContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<BFMenu> BFMenus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyDatabase.sqlite");
        }
    }
}
