namespace Producao_MVC.Responses;

public record MateriaPrimaResponse(int Id, string Nome, string Fornecedor, string Unidade, double Preco, bool Ativo);
