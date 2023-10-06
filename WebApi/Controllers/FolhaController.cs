using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/folha")]
    public class FolhaController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FolhaController(AppDataContext ctx)
        {
            _ctx = ctx;
            
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult ListarFolhas()
        {
            try
            {
                List<Folha> folhas = _ctx.Folhas?.Include(f => f.Funcionario).ToList();

                if (folhas == null || folhas.Count == 0)
                {
                    return NotFound();
                }

                return Ok(folhas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
[Route("cadastrar")]
public IActionResult CadastrarFolha([FromBody] Folha folha)
{
    try
    {
        if (folha == null)
        {
            return BadRequest();
        }

        // Cálculo do Salário Bruto
        folha.SalarioBruto = folha.Valor * folha.Quantidade;

        // Cálculo do INSS
        folha.DescontoINSS = CalcularDescontoINSS(folha.SalarioBruto);

        // Cálculo do Imposto de Renda
        folha.ImpostoDeRenda = CalcularImpostoDeRenda(folha.SalarioBruto);

        // Cálculo do FGTS
        folha.FGTS = folha.SalarioBruto * 0.08m; // 8% do salário bruto

        // Cálculo do Salário Líquido
        folha.SalarioLiquido = folha.SalarioBruto - folha.ImpostoDeRenda - folha.DescontoINSS;

        if (_ctx.Folhas == null)
        {
            _ctx.Folhas = _ctx.Set<Folha>(); // Crie um novo DbSet
        }

        _ctx.Folhas.Add(folha);
        _ctx.SaveChanges();

        return Created("", folha);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}

[HttpGet]
[Route("buscar/{cpf}/{mes}/{ano}")]
public IActionResult BuscarFolha(string cpf, int mes, int ano)
{
    try
    {
        // Aqui você pode adicionar lógica para buscar a folha com base no CPF, mês e ano
        // Você pode consultar seu DbContext (_ctx) para obter os dados necessários

        // Exemplo de consulta fictícia:
        var folha = _ctx.Folhas
            .Include(f => f.Funcionario)
            .FirstOrDefault(f => f.Funcionario.Cpf == cpf && f.Mes == mes && f.Ano == ano);

        if (folha == null)
        {
            return NotFound("Folha não encontrada");
        }

        return Ok(folha);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}

        private decimal CalcularDescontoINSS(decimal salarioBruto)
        {
            decimal descontoINSS;

            if (salarioBruto <= 1693.72m)
            {
                descontoINSS = salarioBruto * 0.08m;
            }
            else if (salarioBruto <= 2822.90m)
            {
                descontoINSS = salarioBruto * 0.09m;
            }
            else if (salarioBruto <= 5645.80m)
            {
                descontoINSS = salarioBruto * 0.11m;
            }
            else
            {
                descontoINSS = 621.03m; // Valor fixo para salários acima de R$ 5.645,81
            }

            return descontoINSS;
        }

        private decimal CalcularImpostoDeRenda(decimal salarioBruto)
        {
            decimal impostoDeRenda;

            if (salarioBruto <= 1903.98m)
            {
                impostoDeRenda = 0;
            }
            else if (salarioBruto <= 2826.65m)
            {
                impostoDeRenda = (salarioBruto * 0.075m) - 142.80m;
            }
            else if (salarioBruto <= 3751.05m)
            {
                impostoDeRenda = (salarioBruto * 0.15m) - 354.80m;
            }
            else if (salarioBruto <= 4664.68m)
            {
                impostoDeRenda = (salarioBruto * 0.225m) - 636.13m;
            }
            else
            {
                impostoDeRenda = (salarioBruto * 0.275m) - 869.36m;
            }

            return impostoDeRenda;
        }
    }
}
