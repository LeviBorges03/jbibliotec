using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly BibliotecaContext _context;

    public AutorRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public IEnumerable<Autor> GetAll()
    {
        return _context.Autores.ToList();
    }

    public Autor? GetById(int id)
    {
        return _context.Autores.FirstOrDefault(a => a.Id == id);
    }

    public void Add(Autor autor)
    {
        _context.Autores.Add(autor);
        _context.SaveChanges();
    }

    public void Update(Autor autor)
    {
        _context.Autores.Update(autor);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var autor = _context.Autores.FirstOrDefault(a => a.Id == id);
        if (autor != null)
        {
            _context.Autores.Remove(autor);
            _context.SaveChanges();
        }
    }

    public async Task<List<Autor>> BuscarTodosAutoresAsync()
    {
        return await _context.Autores.ToListAsync();
    }
}

public interface IAutorRepository
{
    IEnumerable<Autor> GetAll();
    Autor? GetById(int id);
    void Add(Autor autor);
    void Update(Autor autor);
    void Delete(int id);
    Task<List<Autor>> BuscarTodosAutoresAsync();

}
