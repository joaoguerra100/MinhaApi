using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApi.Data;
using MinhaApi.Models;


namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : Controller
    {
        private readonly MinhaApiDbContext _context;
        public ContaController(MinhaApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetContas()
        {
            try
            {
                var result = _context.Conta.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de contas. Esceçao: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);
                if (conta != null)
                {
                    return Ok(conta);
                }
                else
                {
                    return NotFound("Erro, Conta nao existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de conta. Esceçao: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetContaPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na pesquisa de conta. Esceçao: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Descricao descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Descricao ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista.Skip(skip).Take(take).ToList();

                var paginacaoResponse = new PaginacaoResponse<Conta>(lista, qtde, skip, take);

                return Ok(paginacaoResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na paginacao de conta. Esceçao: {e.Message}");
            }
        }

        [HttpGet("Pessoa/{pessoaId}")]
        public async Task<IActionResult> GetContasPessoa([FromRoute] Guid pessoaId)
        {
            try
            {
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.PessoaId == pessoaId
                            select o;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na pesquisa de de conta por pessoa. Esceçao: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostConta([FromBody] Conta conta)
        {
            try
            {
                await _context.Conta.AddAsync(conta);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, conta incluida");
                }
                else
                {
                    return BadRequest("Erro, conta nao incluida");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusao de conta. Exeçao: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutConta([FromBody] Conta conta)
        {
            try
            {
                _context.Conta.Update(conta);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, conta alterada");
                }
                else
                {
                    return BadRequest("Erro, conta nao alterada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteraçao de conta. Exeçao: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);
                if (conta != null)
                {
                    _context.Conta.Remove(conta);
                    var var = await _context.SaveChangesAsync();
                    if (var == 1)
                    {
                        return Ok("Sucesso, conta deletada");
                    }
                    {
                        return BadRequest("Erro, conta nao deletada");
                    }
                }
                {
                    return NotFound("Erro, conta nao existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao deletar conta. Exeçao: {e.Message}");
            }
        }
        
    }
}