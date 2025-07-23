using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class PessoaController : Controller
    {
        private readonly MinhaApiDbContext _context;

        public PessoaController(MinhaApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPessoas()
        {
            try
            {
                var result = _context.Pessoa.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de pessoas. Exceçao {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);
                if (pessoa != null)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return NotFound("Erro, Pessoa nao existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na Procura de pessoa. Exceçao {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from o in _context.Pessoa.ToList()
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.Telefone.ToUpper().Contains(valor.ToUpper())
                            || o.Email.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de Pessoa. Exceçao: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string? valor, int skip, int take, bool ordemDesc)
        {
            try
            {

                var lista = from o in _context.Pessoa.ToList()
                            select o;

                if (!string.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.Telefone.ToUpper().Contains(valor.ToUpper())
                            || o.Email.ToUpper().Contains(valor.ToUpper())
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

                var paginaResponse = new PaginacaoResponse<Pessoa>(lista, qtde, skip, take);

                return Ok(paginaResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, Paginacao de Pessoa. Exceçao: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                await _context.Pessoa.AddAsync(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, pessoa incluida");
                }
                else
                {
                    return BadRequest("Erro, pessoa nao incluida");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusao de pessoa. Exceçao {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                _context.Pessoa.Update(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, pessoa alterada");
                }
                else
                {
                    return BadRequest("Erro, pessoa nao alterada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteraçao de pessoa. Exceçao {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeletePessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);
                if (pessoa != null)
                {
                    _context.Pessoa.Remove(pessoa);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, pessoa Excluida");
                    }
                    else
                    {
                        return BadRequest("Erro, pessoa nao Exlcluida");
                    }
                }
                else
                {
                    return NotFound("Erro, pessoa nao existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteraçao de pessoa. Exceçao {e.Message}");
            }
        }
    }
}