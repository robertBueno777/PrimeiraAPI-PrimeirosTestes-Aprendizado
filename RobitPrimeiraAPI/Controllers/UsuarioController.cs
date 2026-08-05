using Microsoft.AspNetCore.Mvc;
using RobitPrimeiraAPI.Models;
using BCrypt.Net; 

namespace RobitPrimeiraAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        public static List<UsuarioModel> _usuarios = new List<UsuarioModel>() { new UsuarioModel { IdUsuario = 1, NomeUsuario = "Robertinho", SenhaUsuario = "asnoitessabemcomoeuteesperei" } };

        private static int _proximoId = 2;
       ///<summary>
        ///Método na API para retornar todos os usuários da lista.
        ///</summary>
        
        [HttpGet("MostrarTodosOsUsuarioCadastradosNaLista")]
        public ActionResult ObterLista()
        {
            return Ok(_usuarios);
        }

        [HttpPost("CadastroDeUsuario")]
        public IActionResult CadastrarUsuario(string senhaUsuario, string nomeUsuario)
        {
            UsuarioModel usuario = new UsuarioModel();
            usuario.NomeUsuario = nomeUsuario;
            usuario.SenhaUsuario = senhaUsuario;
            if (usuario == null || string.IsNullOrEmpty(usuario.NomeUsuario) || string.IsNullOrEmpty(usuario.SenhaUsuario))
                return BadRequest(new { mensagem = "Erro: dados inválidos ou incompletos." });
            try
            {
                usuario.SenhaUsuario = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaUsuario);
                usuario.IdUsuario = _proximoId++;

                _usuarios.Add(usuario);
                return Ok(new { mensagem = "Usuario cadastrado com sucesso." });
            }
            catch
            {
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar" });
            }
            
        }
        [HttpPost("EclusaoDeUsuarioPorId")]
        public IActionResult ExcluirUsuarioPorId(int id)
        {
            var usuarioExclusao = _usuarios.FirstOrDefault(x => x.IdUsuario == id);
            return Ok(_usuarios.Remove(usuarioExclusao));
        }

        [HttpPut("{id}")]
        public IActionResult EditarUsuarioPorId(int id, [FromBody] UsuarioModel usuarioAtualizado)
        {
            var usuario = _usuarios.FirstOrDefault(x => x.IdUsuario == id);
            try
            {
                if(usuario is not null)
                {
                    usuario.IdUsuario = id;
                    usuario.NomeUsuario = usuarioAtualizado.NomeUsuario;
                    usuario.SenhaUsuario = usuarioAtualizado.SenhaUsuario;

                    return Ok(usuario);
                }
                return BadRequest(new { mensagem = "Usuario não encontrado." });
            }
            catch
            {
                return BadRequest(new { mensagem = "Erro na API."});
            }
        }

        [HttpPost("criptografiaDeSenha")]
        public IActionResult CriptografarSenha(int id)
        {
            var usuario = _usuarios.FirstOrDefault(x => x.IdUsuario == id);
            usuario.SenhaUsuario = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaUsuario);
            return Ok(usuario);

        }
    }
}
