using Microsoft.AspNetCore.Mvc;
using MinhaApi.Models;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private static List<Estado> listaEstado = new List<Estado>();

        [HttpGet]
        public IActionResult Index()
        {
            var result = "Retorno em texto";
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }

        [HttpGet("info2")]
        public IActionResult Index2()
        {
            var result = "Retorno em texto 2";
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }

        [HttpGet("info3/{valor}")]
        public IActionResult Index3([FromRoute] string valor)
        {
            var result = "Retorno em texto 3 - Valor:" + valor;
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }

        [HttpPost("info4")]
        public IActionResult Index4(string valor)
        {
            var result = "Retorno em texto 4 - Valor:" + valor;
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }

        [HttpGet("info5")]
        public IActionResult Index5([FromHeader] string valor)
        {
            var result = "Retorno em texto 5 - Valor:" + valor;
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }

        [HttpPost("info6")]
        public IActionResult Index6([FromBody] Corpo corpo)
        {
            var result = "Retorno em texto 6 - Valor:" + corpo.valor;
            // OK = retorna o 200 que siguinifica que esta funcionando esta ok
            return Ok(result);
        }
    }

    public class Corpo
    {
        public string valor { get; set; }
    }
}