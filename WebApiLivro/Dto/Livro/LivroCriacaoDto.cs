using WebApiLivro.Dto.Vinculo;
using WebApiLivro.Models;

namespace WebApiLivro.Dto.Livro
{
    public class LivroCriacaoDto
    {
        public string Titulo { get; set; }
        public AutorVinculoDto Autor { get; set; }
    }
}
