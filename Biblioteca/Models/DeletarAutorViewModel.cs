namespace Biblioteca.Models;

public class DeletarAutorViewModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool PossuiLivrosVinculados { get; set; } // Propriedade chave para a regra de negócio
}
