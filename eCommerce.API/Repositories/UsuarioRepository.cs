using eCommerce.API.Database;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        #region Properties
        private readonly eCommerceContext _db;
        #endregion

        #region Constructors
        public UsuarioRepository(eCommerceContext db)
        {
            _db = db;
        }
        #endregion

        public async Task<IEnumerable<Usuario>> ObterTodos()
        {
            return await _db.Usuarios
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<Usuario?> ObterPorId(int id)
        {
            if (id <= 0)
                return null;

            return await _db.Usuarios
                .Include(u => u.Contato)
                .Include(u => u.EnderecosEntrega)
                .Include(u => u.Departamentos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObterPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return null;

            return await _db.Usuarios
                .Include(u => u.Contato)
                .Include(u => u.EnderecosEntrega)
                .Include(u => u.Departamentos)
                .FirstOrDefaultAsync(u => u.Nome.Contains(nome));
        }

        public async Task Inserir(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            var usuarioExistente = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.CPF == usuario.CPF);

            if (usuarioExistente != null)
                throw new InvalidOperationException($"Já existe um usuário com o CPF {usuario.CPF}");

            var emailExistente = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (emailExistente != null)
                throw new InvalidOperationException($"Já existe um usuário com o email {usuario.Email}");

            if (usuario.DataCadastro == default)
                usuario.DataCadastro = DateTimeOffset.Now;

            if (string.IsNullOrWhiteSpace(usuario.SituacaoCadastro))
                usuario.SituacaoCadastro = "Ativo";

            await _db.Usuarios.AddAsync(usuario);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Atualizar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            var usuarioExistente = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);

            if (usuarioExistente == null)
                return false;

            var cpfExistente = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.CPF == usuario.CPF && u.Id != usuario.Id);

            if (cpfExistente != null)
                throw new InvalidOperationException($"CPF {usuario.CPF} já está sendo usado por outro usuário");

            var emailExistente = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Id != usuario.Id);

            if (emailExistente != null)
                throw new InvalidOperationException($"Email {usuario.Email} já está sendo usado por outro usuário");

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Sexo = usuario.Sexo;
            usuarioExistente.RG = usuario.RG;
            usuarioExistente.CPF = usuario.CPF;
            usuarioExistente.NomeMae = usuario.NomeMae;
            usuarioExistente.SituacaoCadastro = usuario.SituacaoCadastro;

            _db.Usuarios.Update(usuarioExistente);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            if (id <= 0)
                return false;

            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return false;

            _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}