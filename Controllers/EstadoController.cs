using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : Controller
    {
        private readonly MinhaApiDbContext _context;

        public EstadoController(MinhaApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEstados()
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
        public IActionResult PostEstado([FromBody] Estado estado)
        {
            try
            {
                _context.Estado.Add(estado);
                var valor = _context.SaveChanges();
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
        public IActionResult PutEstado([FromBody] Estado estado)
        {
            try
            {
                _context.Estado.Update(estado);
                var valor = _context.SaveChanges();
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
        public IActionResult DeleteEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = _context.Estado.Find(sigla);

                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    _context.Estado.Remove(estado);
                    var valor = _context.SaveChanges();
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
        public IActionResult GetEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = _context.Estado.Find(sigla);

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
        public IActionResult GetEstadoPesquisa([FromQuery] string valor)
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
    }
}
