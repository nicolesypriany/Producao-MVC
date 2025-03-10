using Producao_MVC.Responses;

namespace Producao_MVC.Models
{
    public class FormaRequestViewModel
    {
        public IEnumerable<MaquinaResponse> Maquinas { get; set; }
        public IEnumerable<ProdutoResponse> Produtos { get; set; }
        public string Nome {  get; set; }
        public int PecasPorCiclo {  get; set; }

    }
}
