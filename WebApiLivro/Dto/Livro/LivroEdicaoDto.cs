using WebApiLivro.Dto.Vinculo;
using WebApiLivro.Models;

namespace WebApiLivro.Dto.Livro
{
    public class LivroEdicaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public AutorVinculoDto Autor { get; set; }
    }
}
