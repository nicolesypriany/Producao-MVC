namespace Producao_MVC.Responses;

public record ProdutoResponse(int Id, string Nome, string Medidas, string Unidade, int PecasPorUnidade, bool Ativo);
