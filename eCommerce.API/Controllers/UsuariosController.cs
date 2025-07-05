using eCommerce.API.Repositories;
using eCommerce.Models;
using Mensagens;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        #region Properties

        private readonly IUsuarioRepository _usuarioRepository;

        #endregion

        #region Constructors

        public UsuariosController(IUsuarioRepository repository)
        {
            _usuarioRepository = repository;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var listaUsuarios = await _usuarioRepository.ObterTodos();
                return Ok(listaUsuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                if(id <= 0)
                    return BadRequest(Retorno.IdDeveSerMaiorQueZero);

                var result = await _usuarioRepository.ObterPorId(id);
                if (result == null)
                    return NotFound(Retorno.RegistroNaoLocalizado);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }

        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> ObterPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return BadRequest(Retorno.NomeNaoPodeSerVazio);

                var result = await _usuarioRepository.ObterPorNome(nome);
                if (result == null)
                    return NotFound(Retorno.RegistroNaoLocalizado);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    return BadRequest(Retorno.DadosInseridosVazios);

                string campo = "Nome Usuário";

                if (string.IsNullOrWhiteSpace(usuario.Nome))
                    return BadRequest(Retorno.CampoObrigatorio(campo));

                await _usuarioRepository.Inserir(usuario);
                return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Usuario usuario, int id)
        {
            try
            {
                if (usuario == null)
                    return BadRequest(Retorno.DadosInseridosVazios);

                if (id <= 0)
                    return BadRequest(Retorno.IdDeveSerMaiorQueZero);

                if (id != usuario.Id)
                    return BadRequest(Retorno.IdNaoCorrespondente);

                string campo = "Nome Usuário";
                if (string.IsNullOrWhiteSpace(usuario.Nome))
                    return BadRequest(Retorno.CampoObrigatorio(campo));

                var atualizado = await _usuarioRepository.Atualizar(usuario);
                if (!atualizado)
                    return NotFound(Retorno.RegistroNaoLocalizado);

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(Retorno.IdDeveSerMaiorQueZero);

                var result = await _usuarioRepository.Deletar(id);
                if (!result)
                    return NotFound(Retorno.RegistroNaoLocalizado);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, CodigoStatus.ErroInterno(ex));
            }
        }
    }
}
