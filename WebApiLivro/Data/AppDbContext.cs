﻿using Microsoft.EntityFrameworkCore;
using WebApiLivro.Models;

namespace WebApiLivro.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }

        public DbSet<AutorModel> Autores { get; set; }
        public DbSet<LivroModel> Livros { get; set; }
        
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Editora> Editoras { get; set; }

    }
}
