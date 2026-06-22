using Biblioteca.Models;
using Biblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Controllers;

public class BibliotecaController : Controller
{
    private readonly ILivroRepository _livroRepository;
    private readonly IAutorRepository _autorRepository;

    public BibliotecaController(ILivroRepository livroRepository, IAutorRepository autorRepository)
    {
        _livroRepository = livroRepository;
        _autorRepository = autorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> index()
    {
        var livros = await _livroRepository.BuscarTodosLivrosAsync();
        return View(livros);
    }

    public async Task<IActionResult> Livro(int id)
    {
        var dbLivros = await _livroRepository.BuscarTodosLivrosAsync();
        var livro = dbLivros.FirstOrDefault(x => x.Id == id);

        if (livro == null) return NotFound();

        return View(livro);
    }

    public IActionResult Autor(int id)
    {
        var autor = _autorRepository.GetById(id);

        if (autor == null) return NotFound();

        return View(autor);
    }

    public async Task<IActionResult> CriarLivroAsync()
    {
        ViewBag.Autores = new SelectList(
            _autorRepository.GetAll(),
            "Id",
            "Nome"
        );

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditarLivro(int id)
    {
        var livros = await _livroRepository.BuscarTodosLivrosAsync();
        var livro = livros.FirstOrDefault(x => x.Id == id);

        if(livro == null) return NotFound();

        var viewModel = new EditarLivroViewModel
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            Genero = livro.Genero,
            NumPaginas = livro.NumPaginas,
            DataPublicacao = livro.DataPublicacao,
            AutorId = livro.Autor?.Id ?? 0,
            CorCapa = livro.CorCapa
        };

        ViewBag.Autores = new SelectList(
            await _autorRepository.BuscarTodosAutoresAsync(),
            "Id",
            "Nome",
            viewModel.AutorId
        );
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditarLivro(EditarLivroViewModel livroViewModel)
    {
        var autores = await _autorRepository.BuscarTodosAutoresAsync();
        var autorSelecionado = autores.FirstOrDefault(a => a.Id == livroViewModel.AutorId);

        Livro livro = new()
        {
            Id = livroViewModel.Id,
            Titulo = livroViewModel.Titulo,
            Genero = livroViewModel.Genero,
            NumPaginas = livroViewModel.NumPaginas,
            DataPublicacao = livroViewModel.DataPublicacao,
            CorCapa = livroViewModel.CorCapa,
            Autor = autorSelecionado
        };

        await _livroRepository.AtualizarLivroAsync(livro);

        return RedirectToAction("Livro", new { id = livroViewModel.Id });
    }


    [HttpGet]
    public IActionResult CriarAutor()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EditarAutor(int id)
    {
        var autor = _autorRepository.GetById(id);
        if (autor == null) return NotFound();

        var viewModel = new EditarAutorViewModel
        {
            Id = autor.Id,
            Nome = autor.Nome,
            Biografia = autor.Biografia,
            DataNascimento = autor.DataNascimento
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult EditarAutor(EditarAutorViewModel autorViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(autorViewModel);
        }

        var autor = _autorRepository.GetById(autorViewModel.Id);
        if (autor == null) return NotFound();

        autor.Nome = autorViewModel.Nome;
        autor.Biografia = autorViewModel.Biografia;
        autor.DataNascimento = autorViewModel.DataNascimento;

        _autorRepository.Update(autor);

        return RedirectToAction("Autor", new { id = autorViewModel.Id });
    }

    [HttpPost]
    public async Task<IActionResult> CriarLivroAsync(CriarLivroViewModel livroViewModel)
    {
        Livro livro = new()
        {
            DataPublicacao = livroViewModel.DataPublicacao,
            Genero = livroViewModel.Genero,
            NumPaginas = livroViewModel.NumPaginas,
            Titulo = livroViewModel.Titulo,
            CorCapa = livroViewModel.CorCapa
        };

        await _livroRepository.CriarLivroAsync(livro, livroViewModel.AutorId);
        return RedirectToAction("Index");
    }

    // GET: Biblioteca/DeletarLivro/Id
    [HttpGet]
    public async Task<IActionResult> DeletarLivro(int id)
    {
        var livros = await _livroRepository.BuscarTodosLivrosAsync();
        var livro = livros.FirstOrDefault(x => x.Id == id);
        if (livro == null) return NotFound();
        // Mapeia os dados do livro para a ViewModel de exibição
        var viewModel = new DeletarLivroViewModel
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            AutorNome = livro.Autor?.Nome ?? "Sem Autor"
        };
        return View(viewModel);
    }

    // POST: Biblioteca/DeletarLivro
    // O atributo ActionName garante que o formulário HTML possa chamar "DeletarLivro" via POST
    [HttpPost, ActionName("DeletarLivro")]
    public async Task<IActionResult> DeletarLivroConfirmado(int id)
    {
        await _livroRepository.ExcluirLivroAsync(id);
        // Redireciona o usuário de volta para a listagem principal
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CriarAutor(Autor autor)
    {
        if (ModelState.IsValid)
        {
            _autorRepository.Add(autor);
            return RedirectToAction("Index");
        }
        return View(autor);
    }
   
}
