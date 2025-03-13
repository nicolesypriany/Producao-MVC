using Producao_MVC.Responses;

namespace Producao_MVC.Models
{
    public class ProcessoProducaoViewModel
    {
        public ProcessoProducaoResponse Producao { get; set; }
        public DateTime Data { get; set; }
        public List<MaquinaResponse> Maquinas { get; set; }
        public List<FormaResponse> Formas { get; set; }
        public List<ProdutoResponse> Produtos { get; set; }
        public List<MateriaPrimaResponse> MateriasPrimas { get; set; }
        public int MaquinaId { get; set; }
        public int FormaId { get; set; }
        public int ProdutoId { get; set; }
        public int Ciclos { get; set; }
        public List<MateriaPrimaCheckboxViewModel> MateriasPrimasCheckbox { get; set; }
    }
}
