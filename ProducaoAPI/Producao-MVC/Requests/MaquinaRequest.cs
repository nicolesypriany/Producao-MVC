using System.ComponentModel.DataAnnotations;

namespace Producao_MVC.Requests;

public record MaquinaRequest([Required(ErrorMessage = "O nome é obrigatório.")] string Nome, [Required(ErrorMessage = "A marca é obrigatória.")] string Marca);

