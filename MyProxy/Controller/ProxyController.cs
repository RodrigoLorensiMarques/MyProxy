using Microsoft.AspNetCore.Mvc;

namespace MeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        // GET: api/pessoa
        [HttpGet]
        public IActionResult GetPessoas()
        {
            // Apenas um array de exemplo
            var pessoas = new string[] { "Alice", "Bob", "Carlos" };
            return Ok(pessoas);
        }

        // GET: api/pessoa/{id}
        [HttpGet("{id}")]
        public IActionResult GetPessoa(int id)
        {
            var pessoas = new string[] { "Alice", "Bob", "Carlos" };

            if (id < 0 || id >= pessoas.Length)
                return NotFound("Pessoa não encontrada");

            return Ok(pessoas[id]);
        }

        // POST: api/pessoa
        [HttpPost]
        public IActionResult CriarPessoa([FromBody] string nome)
        {
            // Aqui você normalmente adicionaria a pessoa a um banco de dados
            return CreatedAtAction(nameof(GetPessoa), new { id = 0 }, nome);
        }
    }
}
