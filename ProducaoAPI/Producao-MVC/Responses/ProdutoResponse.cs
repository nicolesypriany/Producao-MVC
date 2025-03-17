using System.ComponentModel.DataAnnotations;

namespace Producao_MVC.Responses;

public record ProdutoResponse(int Id, [Required(ErrorMessage = "O nome é obrigatório.")] string Nome, [Required(ErrorMessage = "As medidas são obrigatórias.")] string Medidas, [Required(ErrorMessage = "A unidade é obrigatória.")][StringLength(5, ErrorMessage = "A unidade deve ter no máximo 5 caracteres.")] string Unidade, [Required(ErrorMessage = "O número de peças por unidade é obrigatório.")][Range(1, 1000, ErrorMessage = "O número de peças por unidade deve ser maior do que 0.")] int PecasPorUnidade, bool Ativo);
