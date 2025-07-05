using eCommerce.API.Repositories;
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
                if (id <= 0)
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

                if (string.IsNullOrWhiteSpace(usuario.Nome))
                    return BadRequest(Retorno.CampoObrigatorio("Nome"));

                if (string.IsNullOrWhiteSpace(usuario.Email))
                    return BadRequest(Retorno.CampoObrigatorio("Email"));

                if (string.IsNullOrWhiteSpace(usuario.CPF))
                    return BadRequest(Retorno.CampoObrigatorio("CPF"));

                if (!IsValidEmail(usuario.Email))
                    return BadRequest(Retorno.EmailInvalido);

                if (!IsValidCPF(usuario.CPF))
                    return BadRequest(Retorno.CPFInvalido);

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

                if (string.IsNullOrWhiteSpace(usuario.Nome))
                    return BadRequest(Retorno.CampoObrigatorio("Nome"));

                if (string.IsNullOrWhiteSpace(usuario.Email))
                    return BadRequest(Retorno.CampoObrigatorio("Email"));

                if (string.IsNullOrWhiteSpace(usuario.CPF))
                    return BadRequest(Retorno.CampoObrigatorio("CPF"));

                if (!IsValidEmail(usuario.Email))
                    return BadRequest(Retorno.EmailInvalido);

                if (!IsValidCPF(usuario.CPF))
                    return BadRequest(Retorno.CPFInvalido);

                var atualizado = await _usuarioRepository.Atualizar(usuario);
                if (!atualizado)
                    return NotFound(Retorno.RegistroNaoLocalizado);

                return Ok(usuario);
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

        #region Métodos auxiliares
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            return cpf.All(char.IsDigit);
        }
        #endregion
    }
}