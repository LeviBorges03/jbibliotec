using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Repositories;
public class LivroRepository : ILivroRepository
{
    private readonly BibliotecaContext _context;

    public LivroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Livro>> BuscarTodosLivrosAsync() //método para buscar todos os livrosna busca
    {
        return await _context.Livros.Include(x => x.Autor).ToListAsync(); //Inclui os dados do autor relacionado a cada livro
    }

    public async Task<bool> CriarLivroAsync(Livro livro, int autorId)
    {
        livro.Autor = await _context.Autores.FirstOrDefaultAsync(a => a.Id == autorId);
        await _context.Livros.AddAsync(livro);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AtualizarLivroAsync(Livro livro)
    {
        var livroBanco = await _context.Livros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == livro.Id); 
        if(livroBanco == null) return false; 
         
        livroBanco.Titulo = livro.Titulo; //recebe dos inputs do usuário e atualizar o livroBanco.Titulo
        livroBanco.Genero = livro.Genero; //recebe os inputs do usuário e atualizar o livroBanco.Genero
        livroBanco.NumPaginas = livro.NumPaginas; //recebe dos inputs do usuário e atualizar o livroBanco.NumPaginas
        livroBanco.DataPublicacao = livro.DataPublicacao; //recebe dos inputs do usuario e atualizar o livro.DataPublicacao
        livroBanco.CorCapa = livro.CorCapa;

        // Atualiza o Autor (seguindo exatamente o padrão do PDF)
        livroBanco.Autor = livro.Autor;

        await _context.SaveChangesAsync();  //registrar mudanças e retornar as mudanças
        return true;
        
    }

    public async Task<bool> ExcluirLivroAsync(int id)
    {
        // Localiza o livro diretamente pelo ID
        var livro = await _context.Livros.FirstOrDefaultAsync(x => x.Id == id);
        // Se o livro não existir (por exemplo, se já foi deletado por outro usuário), retorna falso
        if (livro == null) return false;
        // Remove do Contexto e salva as alterações no banco de dados
        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
        return true;
    }
}

public interface ILivroRepository
{
    Task<List<Livro>> BuscarTodosLivrosAsync();
    Task<bool> CriarLivroAsync(Livro livro, int autorId); 
    Task<bool> AtualizarLivroAsync(Livro livro); //Assinatura de método para atualizar um livro?
    Task<bool> ExcluirLivroAsync(int id);
}