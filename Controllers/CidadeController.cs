using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class CidadeController : Controller
    {
        private readonly MinhaApiDbContext _context;

        public CidadeController(MinhaApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCidades()
        {
            try
            {
                var result = _context.Cidade.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"erro na listagem de cidades. Exceçao: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    return Ok(cidade);
                }
                else
                {
                    return NotFound("Erro, Cidade nao existe");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"erro ao Encontrar cidade. Exceçao: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetCidadePEsquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from o in _context.Cidade.ToList()
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select o;
                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"erro pesquisa de cidade. Exceçao: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetCidadePaginacao([FromQuery] string? valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                var lista = from o in _context.Cidade.ToList()
                            select o;

                if (!string.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                        where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select o;
                }
                            
                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Nome descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Nome ascending
                            select o;
                }
                var qtde = lista.Count();

                lista = lista.Skip((skip - 1) * take).Take(take).ToList();

                var paginaResponse = new PaginacaoResponse<Cidade>(lista, qtde, skip, take);

                return Ok(paginaResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"erro na Paginacao de cidade. Exceçao: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostCidade([FromBody] Cidade cidade)
        {
            try
            {
                await _context.Cidade.AddAsync(cidade);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, cidade incluida");
                }
                else
                {
                    return BadRequest("Erro, cidade nao incluida");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"erro na inclusao de cidades. Exceçao: {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutCidade([FromBody] Cidade cidade)
        {
            try
            {
                _context.Cidade.Update(cidade);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, cidade Alterada");
                }
                else
                {
                    return BadRequest("Erro, cidade nao Alterada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"erro na Alteraçao de cidades. Exceçao: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    _context.Cidade.Remove(cidade);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, ciade deletada");
                    }
                    else
                    {
                        return BadRequest("Erro,cidade nao deletada");
                    }
                }
                else
                {
                    return NotFound("Erro,cidade nao Existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"erro ao deletar cidade. Exceçao: {e.Message}");
            }
        }
    }
}