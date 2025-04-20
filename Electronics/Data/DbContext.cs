using System.Data;
using Microsoft.EntityFrameworkCore;


namespace Electronics.Models
{
    public class ElectronicsContext : DbContext
    {
        public ElectronicsContext(DbContextOptions<ElectronicsContext> options)
            : base(options)
        { }
        public DbSet<Rol> Rol { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Productos> Productos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Imagenes> Imagenes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Categorias>().HasData(
                new Categorias { IdCategoria = 1, Nombre = "Smartphones" },
                new Categorias { IdCategoria = 2, Nombre = "Laptops" },
                new Categorias { IdCategoria = 3, Nombre = "Smartwatch" },
                new Categorias { IdCategoria = 4, Nombre = "Audifonos" },
                new Categorias { IdCategoria = 5, Nombre = "Tablets" },
                new Categorias { IdCategoria = 6, Nombre = "Monitores" },
                new Categorias { IdCategoria = 7, Nombre = "Accesorios" }
            );


            int productId = 1;
            var productosList = new List<Productos>();


            for (int cat = 1; cat <= 7; cat++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    productosList.Add(new Productos
                    {
                        IdProducto = productId,
                        IdCategoria = cat,
                        Nombre = $"Producto {i} - {GetNombreCategoria(cat)}",
                        Descripcion = $"Descripción del Producto {i} para la categoría {GetNombreCategoria(cat)}",
                        Precio = 100 * i, 
                        Stock = 50
                    });
                    productId++;
                }
            }

            modelBuilder.Entity<Productos>().HasData(productosList);
        }

        private string GetNombreCategoria(int idCategoria)
        {
            return idCategoria switch
            {
                1 => "Smartphones",
                2 => "Laptops",
                3 => "Smartwatch",
                4 => "Audifonos",
                5 => "Tablets",
                6 => "Monitores",
                7 => "Accesorios",
                _ => "General"
            };
        }
    }
}
