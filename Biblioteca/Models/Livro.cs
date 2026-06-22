namespace Biblioteca.Models;

public class Livro
{
    public int Id {get; set;}
    public int NumPaginas {get; set; }
    public Autor? Autor {get; set; }
    public string? Titulo {get; set; }
    public string? Genero {get; set; }
    public DateOnly DataPublicacao {get; set; }
    public string? CorCapa {get; set; }
}