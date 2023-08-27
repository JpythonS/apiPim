namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateDescontoRequest {
    public double ValorFixo { get; set; }
    public int TipoDescontoCod { get; set; }
    public int? Porcentagem { get; set; }
    public int? MinSalario { get; set; }
    public int? MaxSalario { get; set; }
}