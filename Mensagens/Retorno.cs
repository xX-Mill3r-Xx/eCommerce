namespace Mensagens
{
    public static class Retorno
    {
        public static string RegistroNaoLocalizado => "Nenhum registro não localizado";
        public static string IdNaoCorrespondente => "O ID na URL não corresponde ao ID do usuário no corpo da requisição.";
        public static string RegistroNuloOuVazio => "O registro não pode ser nulo ou vazio";
        public static string IdDeveSerMaiorQueZero => "Id deve ser maior que zero.";
        public static string NomeNaoPodeSerVazio => "Nome não pode estar vazio";
        public static string DadosInseridosVazios => "Dados nao podem estar vazios.";
        public static string EmailInvalido => "Email deve ter um formato válido";
        public static string CPFInvalido => "CPF deve ter um formato válido";

        public static string CampoObrigatorio(string campo)
        {
            string msg = $"O campo [{campo}] é obtigatório!";
            return msg;
        }

        public static string RegistroJaExiste(int id)
        {
            string msg = $"Registro com ID {id} ja existe";
            return msg;
        }
    }
}
