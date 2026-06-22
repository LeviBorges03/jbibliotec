namespace Biblioteca.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Biografia { get; set; } = string.Empty;
        public System.DateOnly DataNascimento { get; set; }
    }
}
