using System.ComponentModel.DataAnnotations;

namespace Producao_MVC.Responses;

public record MaquinaResponse(int Id, [Required(ErrorMessage = "O nome é obrigatório.")] string Nome, [Required(ErrorMessage = "A marca é obrigatória.")] string Marca, bool Ativo);

