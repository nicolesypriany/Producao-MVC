using Producao_MVC.Responses;
using System.ComponentModel.DataAnnotations;

namespace Producao_MVC.Models
{
    public class FormaProdutoViewModel
    {
        public FormaResponse Forma { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O número de peças por ciclo é obrigatório.")]
        [Range(1, 1000, ErrorMessage = "O número de peças por ciclo deve ser maior do que 0.")]
        public int PecasPorCiclo { get; set; }
        public IEnumerable<ProdutoResponse> Produtos { get; set; }
        public List<MaquinaResponse> Maquinas { get; set; }
        public List<MaquinaCheckboxViewModel> MaquinasCheckbox { get; set; }
        public List<string> Nomes { get; set; }
    }
}
