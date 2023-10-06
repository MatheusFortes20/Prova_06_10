using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/funcionario")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FuncionarioController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
    [Route("listar")]
public IActionResult ListarFuncionarios()
{
    try
    {
        // Include
        List<Funcionario>? funcionarios = _ctx.Funcionarios?.ToList();

        return funcionarios?.Count == 0 ? NotFound() : Ok(funcionarios);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}

        [HttpPost]
[Route("cadastrar")]
public IActionResult CadastrarFuncionario([FromBody] Funcionario funcionario)
{
    try
    {
        // Verifique se o funcionário recebido não é nulo
        if (funcionario == null)
        {
            return BadRequest("Funcionário inválido.");
        }

        // Verifique se o contexto não é nulo e se Funcionarios não é nulo
        if (_ctx != null && _ctx.Funcionarios != null)
        {
            // Lógica para adicionar o funcionário ao contexto do banco de dados
            _ctx.Funcionarios.Add(funcionario);
            _ctx.SaveChanges();

            return Created("", funcionario);
        }
        else
        {
            return BadRequest("Contexto inválido.");
        }
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}
    }
}
