using Microsoft.VisualBasic;

namespace Mensagens
{
    public static class CodigoStatus
    {
        public static string ErroInterno(Exception ex)
        {
            string msg = $"Erro interno {ex.Message}";
            return msg;
        }
    }
}
