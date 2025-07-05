using eCommerce.Models;
using Mensagens;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        #region Properties
        public static List<Usuario> _db = new List<Usuario>();
        private static int _nextId = 1;
        #endregion

        #region Constructors

        #endregion

        public Task<IEnumerable<Usuario>> ObterTodos()
        {
            return Task.FromResult(_db.AsEnumerable());
        }

        public Task<Usuario> ObterPorId(int id)
        {
            if (id <= 0)
                return Task.FromResult<Usuario?>(null);

            var usuario = _db.First(x => x.Id == id);
            return Task.FromResult(usuario);
        }

        public Task<Usuario> ObterPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                //throw new ArgumentNullException(Retorno.RegistroNuloOuVazio, nameof(nome));
                return Task.FromResult<Usuario?>(null);

            var usuario = _db.FirstOrDefault(x => x.Nome?.Equals(nome, StringComparison.OrdinalIgnoreCase) == true);
            return Task.FromResult(usuario);
        }

        public Task Inserir(Usuario usuario)
        {
            if(usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if(usuario.Id == 0)
                usuario.Id = _nextId++;
            else
            {
                if (_db.Any(x => x.Id == usuario.Id))
                    throw new InvalidOperationException(Retorno.RegistroJaExiste(usuario.Id));

                if (usuario.Id >= _nextId)
                    _nextId = usuario.Id + 1;
            }

            _db.Add(usuario);
            return Task.CompletedTask;
        }

        public Task<bool> Atualizar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException(nameof(usuario));

            var usuarioExistente = _db.FirstOrDefault(x => x.Id == usuario.Id);
            if(usuarioExistente == null)
                return Task.FromResult(false);

            _db.Remove(usuarioExistente);
            _db.Add(usuario);

            return Task.FromResult(true);
        }

        public Task<bool> Deletar(int id)
        {
            if(id <= 0)
                return Task.FromResult(false);

            var usuario = _db.FirstOrDefault(x => x.Id == id);
            if(usuario == null)
                return Task.FromResult(false);

            _db.Remove(usuario);
            return Task.FromResult(true);
        }
    }
}
