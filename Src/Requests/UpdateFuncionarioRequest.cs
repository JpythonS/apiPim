namespace api_pim.Models;

public class UpdateFuncionarioRequest
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Celular { get; set; } = string.Empty;
    public string CelularContatoEmergencia { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string SalarioBase { get; set; } = string.Empty;
    public string JornadaTrabalhoSemanal { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public string NivelPermissao { get; set; }= string.Empty;
}