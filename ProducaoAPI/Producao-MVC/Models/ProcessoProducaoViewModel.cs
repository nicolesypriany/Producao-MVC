using Producao_MVC.Responses;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "A máquina é obrigatória.")]
        public int MaquinaId { get; set; }
        [Required(ErrorMessage = "A forma é obrigatória.")]
        public int FormaId { get; set; }
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O número de ciclos é obrigatório.")]
        [Range(1, 10000, ErrorMessage = "O número de ciclos deve ser maior do que 0.")]
        public int Ciclos { get; set; }
        [Required(ErrorMessage = "A lista de matérias-primas não pode ser vazia.")]
        public List<MateriaPrimaCheckboxViewModel> MateriasPrimasCheckbox { get; set; }
    }
}
