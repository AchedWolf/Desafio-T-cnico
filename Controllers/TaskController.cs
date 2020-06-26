using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Luby.Data;
using Task = Luby.Models.Task;

namespace Luby.Controllers
{
    // Necessario adicionar autenticação de usuario

    [ApiController]
    [Route("v1/tasks")]
    public class TaskController : ControllerBase
    {
        // Visualizar todas as Tasks
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Task>>> Get([FromServices] DataContext context)
        {
            var tasks = await context.Tasks.Include(x => x.User).ToListAsync();
            return tasks;
        }

        // Visualizar uma task peli Id
        [HttpGet]
        [Route("{id:int}")] // adicionar o token de autenticacao
        public async Task<ActionResult<Task>> GetById([FromServices] DataContext context, int id)
        {
            var task = await context.Tasks
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return task;
        }

        // Visualizar as task de um determinado usuario
        [HttpGet]
        [Route("user/{id:int}")]
        public async Task<ActionResult<List<Task>>> GetByUser([FromServices] DataContext context, int id)
        {
            var tasks = await context.Tasks
                .Include(x => x.User)
                .AsNoTracking()
                .Where(x => x.UserId == id)
                .ToListAsync();
            return tasks;
        }

        // Registro
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Task>> Post(
            [FromServices] DataContext context,
            [FromBody]Task model)
        {
            if(ModelState.IsValid)
            {
                context.Tasks.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}