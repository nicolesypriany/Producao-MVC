namespace Producao_MVC.Requests;

public record ProcessoProducaoRequest(DateTime Data, int MaquinaId, int FormaId, int Ciclos, List<ProcessoProducaoMateriaPrimaRequest> MateriasPrimas);
