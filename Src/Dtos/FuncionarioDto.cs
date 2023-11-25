namespace api_pim.Models;

public struct FuncionarioDto {
    public int Id {get; set;}
    public string Nome { get; set;}
    public DateTime DataNascimento {get; set;}
    public string Endereco { get; set;}
    public string Cpf  { get; set;}
    public string Rg { get; set; }
    public string Celular { get; set; }
    public string CelularContatoEmergencia { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Pis { get; set; }
    public string Cargo { get; set;}
    public double SalarioBase { get; set;}
    public double JornadaTrabalhoSemanal { get; set;}
    public string Email {get; set;}
    public string Empresa {get;set;}
    public string NivelPermissao {get; set;}
}