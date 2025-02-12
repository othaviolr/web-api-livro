using WebApiLivro.Dto.Editora;
using WebApiLivro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiLivro.Services.Editora
{
    public interface IEditoraService
    {
        Task<ResponseModel<List<EditoraDto>>> ListarEditoras();
        Task<ResponseModel<EditoraDto>> BuscarEditoraPorIdLivro(int idLivro);
        Task<ResponseModel<EditoraDto>> CriarEditora(EditoraDto editoraCriacaoDto);
        Task<ResponseModel<EditoraDto>> EditarEditora(int id, EditoraDto editoraEdicaoDto);
        Task<ResponseModel<string>> ExcluirEditora(int idEditora); 
    }
}
