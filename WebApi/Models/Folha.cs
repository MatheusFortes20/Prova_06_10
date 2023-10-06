namespace WebApi.Models;
public class Folha
{
 public int FolhaId { get; set; }
 public int Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public decimal SalarioBruto { get; set; }
    public decimal DescontoINSS { get; set; }
    public decimal ImpostoDeRenda { get; set; }
    public decimal SalarioLiquido { get; set; }
    public decimal FGTS { get; set; }

}
