using Producao_MVC.Responses;

namespace Producao_MVC.Models
{
    public class FormaProdutoViewModel
    {
        public FormaResponse Forma { get; set; }
        public string Nome { get; set; }
        public int ProdutoId { get; set; }
        public int PecasPorCiclo { get; set; }
        public IEnumerable<ProdutoResponse> Produtos { get; set; }
        public List<MaquinaResponse> Maquinas { get; set; }
        public List<MaquinaCheckboxViewModel> MaquinasCheckbox { get; set; }
    }
}
