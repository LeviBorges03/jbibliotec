using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Models;

public class BibliotecaContext : DbContext
{
public BibliotecaContext(DbContextOptions options)
: base(options)
{
}
public DbSet<Livro> Livros { get; set; }
public DbSet<Autor> Autores { get; set; }
}
