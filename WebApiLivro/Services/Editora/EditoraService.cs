using WebApiLivro.Data;
using WebApiLivro.Dto.Editora;
using WebApiLivro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLivro.Services.Editora
{
    public class EditoraService : IEditoraService
    {
        private readonly AppDbContext _context;

        public EditoraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<EditoraDto>>> ListarEditoras()
        {
            var editoras = await _context.Editoras.ToListAsync();

            var editoraDtos = editoras.Select(e => new EditoraDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Cnpj = e.Cnpj
            }).ToList();

            return new ResponseModel<List<EditoraDto>>
            {
                Status = true,
                Mensagem = "Editoras encontradas com sucesso.",
                Dados = editoraDtos
            };
        }

        public async Task<ResponseModel<EditoraDto>> BuscarEditoraPorIdLivro(int id)
        {
            var editora = await _context.Editoras.FirstOrDefaultAsync(e => e.Id == id);
            if (editora == null)
            {
                return new ResponseModel<EditoraDto>
                {
                    Status = false,
                    Mensagem = "Editora não encontrada.",
                    Dados = null
                };
            }

            var editoraDto = new EditoraDto
            {
                Nome = editora.Nome,
                Cnpj = editora.Cnpj
            };

            return new ResponseModel<EditoraDto>
            {
                Status = true,
                Mensagem = "Editora encontrada com sucesso.",
                Dados = editoraDto
            };
        }

        public async Task<ResponseModel<EditoraDto>> CriarEditora(EditoraDto editoraDto)
        {
            var editora = new WebApiLivro.Models.Editora
            {
                Nome = editoraDto.Nome,
                Cnpj = editoraDto.Cnpj
            };

            await _context.Editoras.AddAsync(editora);
            await _context.SaveChangesAsync();

            var createdEditoraDto = new EditoraDto
            {
                Nome = editora.Nome,
                Cnpj = editora.Cnpj
            };

            return new ResponseModel<EditoraDto>
            {
                Status = true,
                Mensagem = "Editora criada com sucesso.",
                Dados = createdEditoraDto
            };
        }

        public async Task<ResponseModel<EditoraDto>> EditarEditora(int id, EditoraDto editoraDto)
        {
            var editora = await _context.Editoras.FirstOrDefaultAsync(e => e.Id == id);
            if (editora == null)
            {
                return new ResponseModel<EditoraDto>
                {
                    Status = false,
                    Mensagem = "Editora não encontrada.",
                    Dados = null
                };
            }

            editora.Nome = editoraDto.Nome;
            editora.Cnpj = editoraDto.Cnpj;

            _context.Editoras.Update(editora);
            await _context.SaveChangesAsync();

            var updatedEditoraDto = new EditoraDto
            {
                Nome = editora.Nome,
                Cnpj = editora.Cnpj
            };

            return new ResponseModel<EditoraDto>
            {
                Status = true,
                Mensagem = "Editora atualizada com sucesso.",
                Dados = updatedEditoraDto
            };
        }

        public async Task<ResponseModel<string>> ExcluirEditora(int id)
        {
            var editora = await _context.Editoras.FirstOrDefaultAsync(e => e.Id == id);
            if (editora == null)
            {
                return new ResponseModel<string>
                {
                    Status = false,
                    Mensagem = "Editora não encontrada.",
                    Dados = null
                };
            }

            _context.Editoras.Remove(editora);
            await _context.SaveChangesAsync();

            return new ResponseModel<string>
            {
                Status = true,
                Mensagem = "Editora deletada com sucesso.",
                Dados = null
            };
        }
    }
}
