using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstadoController : Controller
    {
        private readonly MinhaApiDbContext _context;

        public EstadoController(MinhaApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                var result = _context.Estado.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de estados. Exceçao: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostEstado([FromBody] Estado estado)
        {
            try
            {
                await _context.Estado.AddAsync(estado);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, estado incluido");
                }
                else
                {
                    return BadRequest("Erro, estado nao incluido");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado nao incluido. Exceçao: {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutEstado([FromBody] Estado estado)
        {
            try
            {
                _context.Estado.Update(estado);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, estado alterado");
                }
                else
                {
                    return BadRequest("Erro, estado nao alterado");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado nao alterado. Exceçao: {e.Message}");
            }
        }

        [HttpDelete("{sigla}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla);

                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    _context.Estado.Remove(estado);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, estado excluido");
                    }
                    else
                    {
                        return BadRequest("Erro, estado nao excluido");
                    }
                }
                else
                {
                    return NotFound("Erro, estado nao existe");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado nao Deletado. Exceçao: {e.Message}");
            }
        }

        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla);

                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    return Ok(estado);
                }
                else
                {
                    return NotFound("Erro, estado nao existe");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, consulta de estado. Exceçao: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {
                //Query Criteria
                var lista = from o in _context.Estado.ToList()
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de estado. Exceçao: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string? valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                var lista = from o in _context.Estado.ToList()
                            select o;

                // caso passe algum valor para a variavel
                if (!string.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                                    || o.Nome.ToUpper().Contains(valor.ToUpper())
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

                // (skip - 1) * take) faz com que no skip a lista nao comece do indice [0] e sim do 1 e que pegue a quantidade certa do take
                lista = lista.Skip((skip - 1) * take).Take(take).ToList();

                var paginacaoResponse = new PaginacaoResponse<Estado>(lista, qtde, skip, take);

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de estado. Exceçao: {e.Message}");
            }
        }
    }
}
