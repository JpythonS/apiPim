namespace api_pim.Models;

using System.ComponentModel.DataAnnotations;

public class CreateAdicionalRequest {
    public double ValorFixo { get; set; }
    public int TipoAdicionalCod { get; set; }
    public int? Porcentagem { get; set; }
    public int? MinSalario { get; set; }
    public int? MaxSalario { get; set; }
}