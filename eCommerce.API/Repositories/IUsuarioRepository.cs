using eCommerce.Models;

namespace eCommerce.API.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ObterTodos();
        Task<Usuario?> ObterPorId(int id);
        Task<Usuario?> ObterPorNome(string nome);
        Task Inserir(Usuario usuario);
        Task<bool> Atualizar(Usuario usuario);
        Task<bool> Deletar(int id);
    }
}
