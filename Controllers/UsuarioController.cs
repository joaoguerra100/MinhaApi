using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;
using MinhaApi.Services;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly MinhaApiDbContext _context;
        private readonly TokenService _service;

        public UsuarioController(MinhaApiDbContext context, TokenService service)
        {
            _service = service;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            var usuario = _context.Usuario.Where(x => x.Login == usuarioLogin.Login).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound("Usuario invalido.");
            }
            if (usuario.Password != usuarioLogin.Password)
            {
                return BadRequest("Senha invalida");
            }

            var token = _service.GerarToken(usuario);

            usuario.Password = "";

            var result = new UsuarioResponse()
            {
                Usuario = usuario,
                Token = token
            };

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var result = _context.Usuario.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de usuarios. Esceçao: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var listUsuario = _context.Usuario.Where(x => x.Login == usuario.Login).ToList();
                if (listUsuario.Count > 0)
                {
                    return BadRequest($"Erro, informaçao de Login invalido");
                }

                await _context.Usuario.AddAsync(usuario);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, usuario incluida");
                }
                else
                {
                    return BadRequest("Erro, usuario nao incluida");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusao de usuario. Esceçao: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
        {
            try
            {
                _context.Usuario.Update(usuario);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, Usuario alterado");
                }
                else
                {
                    return BadRequest("Erro, usuario nao alterado");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteraçao de usuario. Esceçao: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] Guid id)
        {
            try
            {
                Usuario usuario = await _context.Usuario.FindAsync(id);
                if (usuario != null)
                {
                    _context.Usuario.Remove(usuario);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, usuario Excluido");
                    }
                    else
                    {
                        return BadRequest("Erro, usuario nao excluido");
                    }
                }
                else
                {
                    return NotFound("Erro usuario nao existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao deletar usuario. Esceçao: {e.Message}");
            }
        }
    }
}