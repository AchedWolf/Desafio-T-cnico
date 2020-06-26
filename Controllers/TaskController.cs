using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Luby.Data;
using Task = Luby.Models.Task;

namespace Luby.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : ControllerBase
    {
        // Registrar nova task
        [HttpPost]
        [Route("")]
        [Authorize]
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

        // Visualizar todas as Tasks
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Task>>> Get([FromServices] DataContext context)
        {
            var tasks = await context.Tasks.Include(x => x.User).ToListAsync();
            return tasks;
        }

        // Visualizar uma task pelo Id
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

        /*[HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Task>> GetById([FromServices] DataContext context1, int id)
        {
            var 
        }*/
        
    }
}