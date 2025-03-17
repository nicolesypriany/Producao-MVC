using System.ComponentModel.DataAnnotations;

namespace Producao_MVC.Requests;

public record MateriaPrimaRequest([Required(ErrorMessage = "O nome é obrigatório.")] string Nome, [Required(ErrorMessage = "O fornecedor é obrigatório.")]
 string Fornecedor, [Required(ErrorMessage = "A unidade é obrigatória.")][StringLength(5, ErrorMessage = "A unidade deve ter no máximo 5 caracteres.")] string Unidade, [Required(ErrorMessage = "O preço é obrigatório.")][Range(0.01, 1000000, ErrorMessage = "O preço deve ser maior do que 0.")] double Preco);