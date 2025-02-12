using Microsoft.AspNetCore.Mvc;
using WebApiLivro.Dto.Editora;
using WebApiLivro.Services.Editora;
using WebApiLivro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiLivro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditoraController : ControllerBase
    {
        private readonly IEditoraService _editoraService;

        public EditoraController(IEditoraService editoraService)
        {
            _editoraService = editoraService;
        }

        [HttpGet("ListarEditoras")]
        public async Task<ActionResult<ResponseModel<List<EditoraDto>>>> ListarEditoras()
        {
            var response = await _editoraService.ListarEditoras();
            return Ok(response);
        }

        [HttpGet("BuscarEditoraPorId/{id}")]
        public async Task<ActionResult<ResponseModel<EditoraDto>>> BuscarEditoraPorId(int id)
        {
            var response = await _editoraService.BuscarEditoraPorIdLivro(id);
            if (!response.Status)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("CriarEditora")]
        public async Task<ActionResult<ResponseModel<EditoraDto>>> CriarEditora(EditoraDto editoraDto)
        {
            var response = await _editoraService.CriarEditora(editoraDto);
            if (!response.Status)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(BuscarEditoraPorId), new { id = response.Dados.Id }, response);
        }

        [HttpPut("EditarEditora/{id}")]
        public async Task<ActionResult<ResponseModel<EditoraDto>>> EditarEditora(int id, EditoraDto editoraDto)
        {
            var response = await _editoraService.EditarEditora(id, editoraDto);
            if (!response.Status)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("ExcluirEditora/{id}")]
        public async Task<ActionResult<ResponseModel<string>>> ExcluirEditora(int id)
        {
            var response = await _editoraService.ExcluirEditora(id);
            if (!response.Status)
            {
                return NotFound(response);
            }

            return NoContent();
        }
    }
}
